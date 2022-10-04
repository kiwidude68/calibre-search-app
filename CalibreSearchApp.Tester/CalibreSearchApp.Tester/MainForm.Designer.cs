namespace CalibreSearchApp.Tester
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.bntTestConnectivity = new System.Windows.Forms.Button();
            this.btnTestSearch = new System.Windows.Forms.Button();
            this.radFirefox = new System.Windows.Forms.RadioButton();
            this.radChrome = new System.Windows.Forms.RadioButton();
            this.txtLibraryName = new System.Windows.Forms.TextBox();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.lblLibraryName = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.rtbLog = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // bntTestConnectivity
            // 
            this.bntTestConnectivity.Location = new System.Drawing.Point(12, 12);
            this.bntTestConnectivity.Name = "bntTestConnectivity";
            this.bntTestConnectivity.Size = new System.Drawing.Size(129, 23);
            this.bntTestConnectivity.TabIndex = 0;
            this.bntTestConnectivity.Text = "Test Connectivity";
            this.bntTestConnectivity.UseVisualStyleBackColor = true;
            this.bntTestConnectivity.Click += new System.EventHandler(this.bntTestConnectivity_Click);
            // 
            // btnTestSearch
            // 
            this.btnTestSearch.Location = new System.Drawing.Point(12, 41);
            this.btnTestSearch.Name = "btnTestSearch";
            this.btnTestSearch.Size = new System.Drawing.Size(129, 23);
            this.btnTestSearch.TabIndex = 3;
            this.btnTestSearch.Text = "Test Search";
            this.btnTestSearch.UseVisualStyleBackColor = true;
            this.btnTestSearch.Click += new System.EventHandler(this.btnTestSearch_Click);
            // 
            // radFirefox
            // 
            this.radFirefox.AutoSize = true;
            this.radFirefox.Location = new System.Drawing.Point(246, 14);
            this.radFirefox.Name = "radFirefox";
            this.radFirefox.Size = new System.Drawing.Size(61, 19);
            this.radFirefox.TabIndex = 2;
            this.radFirefox.Text = "Firefox";
            this.radFirefox.UseVisualStyleBackColor = true;
            // 
            // radChrome
            // 
            this.radChrome.AutoSize = true;
            this.radChrome.Checked = true;
            this.radChrome.Location = new System.Drawing.Point(161, 14);
            this.radChrome.Name = "radChrome";
            this.radChrome.Size = new System.Drawing.Size(68, 19);
            this.radChrome.TabIndex = 1;
            this.radChrome.TabStop = true;
            this.radChrome.Text = "Chrome";
            this.radChrome.UseVisualStyleBackColor = true;
            // 
            // txtLibraryName
            // 
            this.txtLibraryName.Location = new System.Drawing.Point(246, 42);
            this.txtLibraryName.Name = "txtLibraryName";
            this.txtLibraryName.Size = new System.Drawing.Size(145, 23);
            this.txtLibraryName.TabIndex = 5;
            this.txtLibraryName.Text = "CalibreBooks";
            // 
            // txtSearch
            // 
            this.txtSearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSearch.Location = new System.Drawing.Point(466, 41);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(237, 23);
            this.txtSearch.TabIndex = 7;
            this.txtSearch.Text = "Smith";
            // 
            // lblLibraryName
            // 
            this.lblLibraryName.AutoSize = true;
            this.lblLibraryName.Location = new System.Drawing.Point(161, 45);
            this.lblLibraryName.Name = "lblLibraryName";
            this.lblLibraryName.Size = new System.Drawing.Size(79, 15);
            this.lblLibraryName.TabIndex = 4;
            this.lblLibraryName.Text = "Library name:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(397, 45);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 15);
            this.label1.TabIndex = 6;
            this.label1.Text = "Search for:";
            // 
            // rtbLog
            // 
            this.rtbLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rtbLog.Location = new System.Drawing.Point(12, 71);
            this.rtbLog.Name = "rtbLog";
            this.rtbLog.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.rtbLog.Size = new System.Drawing.Size(691, 228);
            this.rtbLog.TabIndex = 8;
            this.rtbLog.Text = "";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(715, 311);
            this.Controls.Add(this.rtbLog);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblLibraryName);
            this.Controls.Add(this.txtSearch);
            this.Controls.Add(this.txtLibraryName);
            this.Controls.Add(this.radChrome);
            this.Controls.Add(this.radFirefox);
            this.Controls.Add(this.btnTestSearch);
            this.Controls.Add(this.bntTestConnectivity);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(650, 350);
            this.Name = "MainForm";
            this.Text = "Calibre Search App Tester";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button bntTestConnectivity;
        private Button btnTestSearch;
        private RadioButton radFirefox;
        private RadioButton radChrome;
        private TextBox txtLibraryName;
        private TextBox txtSearch;
        private Label lblLibraryName;
        private Label label1;
        private RichTextBox rtbLog;
    }
}