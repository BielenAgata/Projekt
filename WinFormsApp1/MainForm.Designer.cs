namespace WinFormsApp1
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Button btnAddPart;
        private System.Windows.Forms.Button btnSaveReport;
        private System.Windows.Forms.ListBox lstParts;
        private System.Windows.Forms.TextBox txtReportPath;
        private System.Windows.Forms.Button btnBrowse;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            // 
            // btnAddPart
            // 
            this.btnAddPart = new System.Windows.Forms.Button();
            this.btnAddPart.Text = "Add Part";
            this.btnAddPart.Left = 10;
            this.btnAddPart.Top = 10;
            this.btnAddPart.Width = 100;
            this.btnAddPart.Click += BtnAddPart_Click;

            // 
            // btnSaveReport
            // 
            this.btnSaveReport = new System.Windows.Forms.Button();
            this.btnSaveReport.Text = "Save Report";
            this.btnSaveReport.Left = 120;
            this.btnSaveReport.Top = 10;
            this.btnSaveReport.Width = 100;
            this.btnSaveReport.Click += BtnSaveReport_Click;

            // 
            // btnBrowse
            // 
            this.btnBrowse = new System.Windows.Forms.Button();
            this.btnBrowse.Text = "Browse";
            this.btnBrowse.Left = 480;
            this.btnBrowse.Top = 320;
            this.btnBrowse.Width = 100;
            this.btnBrowse.Click += BtnBrowse_Click;

            // 
            // txtReportPath
            // 
            this.txtReportPath = new System.Windows.Forms.TextBox();
            this.txtReportPath.Left = 10;
            this.txtReportPath.Top = 320;
            this.txtReportPath.Width = 460;

            // 
            // lstParts
            // 
            this.lstParts = new System.Windows.Forms.ListBox();
            this.lstParts.Left = 10;
            this.lstParts.Top = 50;
            this.lstParts.Width = 560;
            this.lstParts.Height = 250;

            // Dodanie kontrolek do formularza
            this.Controls.Add(this.btnAddPart);
            this.Controls.Add(this.btnSaveReport);
            this.Controls.Add(this.lstParts);
            this.Controls.Add(this.txtReportPath);
            this.Controls.Add(this.btnBrowse);

            this.Text = "Production Tracking";
            this.Size = new System.Drawing.Size(600, 400);
            this.ResumeLayout(false);
        }
    }
}
