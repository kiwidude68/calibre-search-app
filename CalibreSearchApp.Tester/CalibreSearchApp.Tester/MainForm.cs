namespace CalibreSearchApp.Tester
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void bntTestConnectivity_Click(object sender, EventArgs e)
        {
            var messagePublisher = new MessagePublisherService();
            IEnumerable<string> output = messagePublisher.SendTestMessage(radFirefox.Checked);
            DisplayOutput(output);
        }

        private void btnTestSearch_Click(object sender, EventArgs e)
        {
            var messagePublisher = new MessagePublisherService();
            IEnumerable<string> output = messagePublisher.SendSearch(radFirefox.Checked, txtLibraryName.Text, txtSearch.Text);
            DisplayOutput(output);
        }

        private void DisplayOutput(IEnumerable<string> lines)
        {
            rtbLog.Clear();
            foreach (string line in lines)
            {
                rtbLog.AppendText(line + "\r\n");
            }
        }
    }
}