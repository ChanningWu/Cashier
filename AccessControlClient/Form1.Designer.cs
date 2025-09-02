namespace AccessControlClient
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;
        private TextBox txtBaseUrl;
        private Button btnStatus;
        private Button btnOpen;
        private Button btnClose;
        private Button btnRefresh;
        private TextBox txtDays;
        private TextBox txtSecret;
        private TextBox txtResult;
        private Label lblUrl;
        private Label lblDays;
        private Label lblSecret;
        private Label lblResult;

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
            txtBaseUrl = new TextBox();
            btnStatus = new Button();
            btnOpen = new Button();
            btnClose = new Button();
            btnRefresh = new Button();
            txtDays = new TextBox();
            txtSecret = new TextBox();
            txtResult = new TextBox();
            lblUrl = new Label();
            lblDays = new Label();
            lblSecret = new Label();
            lblResult = new Label();
            SuspendLayout();
            // 
            // txtBaseUrl
            // 
            txtBaseUrl.Location = new Point(100, 15);
            txtBaseUrl.Size = new Size(400, 23);
            // 
            // btnStatus
            // 
            btnStatus.Location = new Point(520, 15);
            btnStatus.Size = new Size(100, 23);
            btnStatus.Text = "Check Status";
            btnStatus.Click += btnStatus_Click;
            // 
            // btnOpen
            // 
            btnOpen.Location = new Point(520, 50);
            btnOpen.Size = new Size(100, 23);
            btnOpen.Text = "Open";
            btnOpen.Click += btnOpen_Click;
            // 
            // btnClose
            // 
            btnClose.Location = new Point(520, 85);
            btnClose.Size = new Size(100, 23);
            btnClose.Text = "Close";
            btnClose.Click += btnClose_Click;
            // 
            // btnRefresh
            // 
            btnRefresh.Location = new Point(520, 120);
            btnRefresh.Size = new Size(100, 23);
            btnRefresh.Text = "Refresh Key";
            btnRefresh.Click += btnRefresh_Click;
            // 
            // txtDays
            // 
            txtDays.Location = new Point(100, 50);
            txtDays.Size = new Size(100, 23);
            txtDays.Text = "3";
            // 
            // txtSecret
            // 
            txtSecret.Location = new Point(100, 85);
            txtSecret.Size = new Size(400, 23);
            // 
            // txtResult
            // 
            txtResult.Location = new Point(100, 150);
            txtResult.Multiline = true;
            txtResult.ScrollBars = ScrollBars.Vertical;
            txtResult.Size = new Size(520, 200);
            // 
            // lblUrl
            // 
            lblUrl.Location = new Point(20, 18);
            lblUrl.Text = "Base URL:";
            // 
            // lblDays
            // 
            lblDays.Location = new Point(20, 53);
            lblDays.Text = "Days:";
            // 
            // lblSecret
            // 
            lblSecret.Location = new Point(20, 88);
            lblSecret.Text = "Secret:";
            // 
            // lblResult
            // 
            lblResult.Location = new Point(20, 150);
            lblResult.Text = "Result:";
            // 
            // Form1
            // 
            ClientSize = new Size(650, 380);
            Controls.Add(txtBaseUrl);
            Controls.Add(btnStatus);
            Controls.Add(btnOpen);
            Controls.Add(btnClose);
            Controls.Add(btnRefresh);
            Controls.Add(txtDays);
            Controls.Add(txtSecret);
            Controls.Add(txtResult);
            Controls.Add(lblUrl);
            Controls.Add(lblDays);
            Controls.Add(lblSecret);
            Controls.Add(lblResult);
            Text = "Access Control Client";
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
