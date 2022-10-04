
using Microsoft.Win32;
using System.Diagnostics;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace CalibreSearchApp.Tester
{
    internal class MessagePublisherService
    {
        internal IEnumerable<string> SendTestMessage(bool isFirefox)
        {
            var output = new List<string>();

            string json = "{\"action\":\"TEST_CONNECTIVITY\"}";
            
            PublishMessage(isFirefox, json, output);

            return output;
        }

        internal IEnumerable<string> SendSearch(bool isFirefox, string libraryName, string search)
        {
            var output = new List<string>();

            string json = GetSafeJson(libraryName, search);
            
            PublishMessage(isFirefox, json, output);

            return output;
        }

        private string GetSafeJson(string libraryName, string search)
        {
            string safeLibraryName = libraryName.Trim().Replace(" ", "_");
            string safeSearch = Uri.EscapeDataString(search);
            string url = $"calibre://search/{safeLibraryName}?q={safeSearch}";
            return "{\"action\":\"SEARCH\",\"search\":\"" + url + "\"}";
        }

        private void PublishMessage(bool isFirefox, string json, List<string> output)
        {
            output.Add($"Sending message: {json} for: " + (isFirefox ? "Firefox" : "Chrome"));
            try
            {
                string? manifestLocation = CheckRegistry(isFirefox, output);
                if (manifestLocation == null) return;
                string? targetExePath = CheckManifestForExePath(manifestLocation, output);
                if (targetExePath == null) return;

                SendMessageToExecutable(targetExePath, json, output);
            }
            catch (Exception e)
            {
                output.Add($"ERROR: Publishing - {e.Message}");
            }
        }

        private void SendMessageToExecutable(string targetExePath, string json, List<string> output)
        {
            try
            {
                var startInfo = new ProcessStartInfo();
                startInfo.FileName = targetExePath;
                startInfo.UseShellExecute = false;
                startInfo.RedirectStandardInput = true;
                startInfo.RedirectStandardOutput = true;
                startInfo.RedirectStandardError = true;
                startInfo.WorkingDirectory = Path.GetDirectoryName(targetExePath);
                startInfo.CreateNoWindow = true;

                output.Add("");
                output.Add("Starting executable process...");
                using (Process process = new Process())
                {
                    process.StartInfo = startInfo;
                    process.Start();
                    Thread.Sleep(100);

                    using (var binaryWriter = new BinaryWriter(process.StandardInput.BaseStream))
                    {
                        // Write the message length in first 4 bytes
                        int messageLength = json.Length;
                        byte[] intBytes = BitConverter.GetBytes(messageLength);
                        binaryWriter.Write(intBytes);
                        output.Add("Sent message length bytes");

                        // Then write the JSON
                        process.StandardInput.Write(json);
                        output.Add($"Sent json: {json}");

                        var errors = process.StandardError.ReadToEnd();
                        var results = process.StandardOutput.ReadToEnd();

                        process.WaitForExit();
                        results = DisplayResult(output, errors, results);
                    }
                }
            }
            catch (Exception e)
            {
                output.Add($"ERROR: Sending message - {e.Message}");
            }
        }

        private static string DisplayResult(List<string> output, string errors, string results)
        {
            output.Add("Process exited");
            if (results.Length > 0)
            {
                // This is a bit hacky, but this is just a simple testing app...
                try
                {
                    // The first 4 bytes are the length
                    results = results.Substring(4); 
                    // From Python we get escaping weirdness in the json serialization that our Golang app does not have...
                    results = results.Replace("\\\\\\", "").Replace("\\\"", "\"");
                    // Python also surrounds the json with a string we will strip out
                    if (results.StartsWith("\\") && results.EndsWith("\\"))
                    {
                        results = results.Substring(1, results.Length - 2);
                    }

                    output.Add("");
                    output.Add($"Results: {results}");
                    output.Add("SUCCESS!");
                }
                catch (Exception e)
                {
                    output.Add($"ERROR: Results invalid: {e.Message}");
                }
            }
            if (errors.Length > 0)
            {
                output.Add("Errors: " + errors);
            }

            return results;
        }

        private string? CheckRegistry(bool isFirefox, List<string> output)
        {
            string subKey = isFirefox 
                ? "SOFTWARE\\Mozilla\\NativeMessagingHosts\\com.kiwidude.calibre_search" 
                : "Software\\Google\\Chrome\\NativeMessagingHosts\\com.kiwidude.calibre_search";

            output.Add("Checking registry key: HKCU\\" + subKey);
            try
            {
                using (RegistryKey key = Registry.CurrentUser)
                using (var appKey = key.OpenSubKey(subKey))
                {
                    var defaultValue = appKey?.GetValue("");
                    if (defaultValue == null)
                    {
                        output.Add("ERROR: No default registry key found");
                    }
                    return defaultValue?.ToString();
                }

            }
            catch (Exception e)
            {
                output.Add($"ERROR: Querying registry - {e.Message}");
                return null;
            }
        }

        private string? CheckManifestForExePath(string manifestPath, List<string> output)
        {
            try
            {
                output.Add($"Manifest path: {manifestPath}");
                if (!File.Exists(manifestPath))
                {
                    output.Add($"ERROR: Manifest not found at: {manifestPath}");
                }
                string json = File.ReadAllText(manifestPath);
                Manifest manifest = JsonSerializer.Deserialize<Manifest>(json)!;
                string exePath = manifest.Path;
                output.Add($"Exe path: {exePath}");
                if (exePath == null || !File.Exists(exePath))
                {
                    output.Add($"ERROR: Exe not found at: {exePath}");
                    return null;
                }
                return exePath;
            }
            catch (JsonException je)
            {
                output.Add($"ERROR: Invalid manifest JSON - {je.Message}");
                return null;
            }
            catch (Exception e)
            {
                output.Add($"ERROR: Checking manifest - {e.Message}");
                return null;
            }
        }
    }
}
