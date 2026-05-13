namespace NexLibrary.Desktop.Forms;

partial class FrmDashboard
{
    private System.ComponentModel.IContainer components = null;
    private Panel pnlTop;
    private Label lblTitle;
    private Button btnRefresh;
    private Label lblLastUpdate;
    private FlowLayoutPanel flpCards;
    private Panel pnlRecentHeader;
    private Label lblRecentTitle;
    private Label lblRecentLoansCount;
    private DataGridView dgvRecentLoans;

    protected override void Dispose(bool disposing)
    {
        if (disposing && components != null)
        {
            components.Dispose();
        }

        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        pnlTop = new Panel();
        lblLastUpdate = new Label();
        btnRefresh = new Button();
        lblTitle = new Label();
        flpCards = new FlowLayoutPanel();
        pnlRecentHeader = new Panel();
        lblRecentLoansCount = new Label();
        lblRecentTitle = new Label();
        dgvRecentLoans = new DataGridView();

        pnlTop.SuspendLayout();
        pnlRecentHeader.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvRecentLoans).BeginInit();
        SuspendLayout();

        pnlTop.BackColor = Color.White;
        pnlTop.Controls.Add(lblLastUpdate);
        pnlTop.Controls.Add(btnRefresh);
        pnlTop.Controls.Add(lblTitle);
        pnlTop.Dock = DockStyle.Top;
        pnlTop.Location = new Point(0, 0);
        pnlTop.Name = "pnlTop";
        pnlTop.Padding = new Padding(15);
        pnlTop.Size = new Size(980, 80);
        pnlTop.TabIndex = 0;

        lblTitle.AutoSize = true;
        lblTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
        lblTitle.Location = new Point(15, 15);
        lblTitle.Name = "lblTitle";
        lblTitle.Size = new Size(125, 30);
        lblTitle.TabIndex = 0;
        lblTitle.Text = "Dashboard";

        btnRefresh.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnRefresh.Location = new Point(845, 24);
        btnRefresh.Name = "btnRefresh";
        btnRefresh.Size = new Size(110, 32);
        btnRefresh.TabIndex = 1;
        btnRefresh.Text = "Yenile";
        btnRefresh.UseVisualStyleBackColor = true;
        btnRefresh.Click += btnRefresh_Click;

        lblLastUpdate.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        lblLastUpdate.Font = new Font("Segoe UI", 9F);
        lblLastUpdate.ForeColor = Color.DimGray;
        lblLastUpdate.Location = new Point(500, 30);
        lblLastUpdate.Name = "lblLastUpdate";
        lblLastUpdate.Size = new Size(330, 20);
        lblLastUpdate.TabIndex = 2;
        lblLastUpdate.Text = "Son güncelleme: -";
        lblLastUpdate.TextAlign = ContentAlignment.MiddleRight;

        flpCards.AutoScroll = true;
        flpCards.BackColor = Color.FromArgb(245, 247, 250);
        flpCards.Dock = DockStyle.Top;
        flpCards.Location = new Point(0, 80);
        flpCards.Name = "flpCards";
        flpCards.Padding = new Padding(8);
        flpCards.Size = new Size(980, 230);
        flpCards.TabIndex = 1;

        pnlRecentHeader.BackColor = Color.White;
        pnlRecentHeader.Controls.Add(lblRecentLoansCount);
        pnlRecentHeader.Controls.Add(lblRecentTitle);
        pnlRecentHeader.Dock = DockStyle.Top;
        pnlRecentHeader.Location = new Point(0, 310);
        pnlRecentHeader.Name = "pnlRecentHeader";
        pnlRecentHeader.Size = new Size(980, 45);
        pnlRecentHeader.TabIndex = 2;

        lblRecentTitle.AutoSize = true;
        lblRecentTitle.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
        lblRecentTitle.Location = new Point(15, 12);
        lblRecentTitle.Name = "lblRecentTitle";
        lblRecentTitle.Size = new Size(146, 20);
        lblRecentTitle.TabIndex = 0;
        lblRecentTitle.Text = "Son Ödünç Kayıtları";

        lblRecentLoansCount.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        lblRecentLoansCount.Font = new Font("Segoe UI", 9F);
        lblRecentLoansCount.ForeColor = Color.DimGray;
        lblRecentLoansCount.Location = new Point(650, 13);
        lblRecentLoansCount.Name = "lblRecentLoansCount";
        lblRecentLoansCount.Size = new Size(305, 20);
        lblRecentLoansCount.TabIndex = 1;
        lblRecentLoansCount.Text = "Son ödünç kayıtları: 0";
        lblRecentLoansCount.TextAlign = ContentAlignment.MiddleRight;

        dgvRecentLoans.BackgroundColor = Color.White;
        dgvRecentLoans.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dgvRecentLoans.Dock = DockStyle.Fill;
        dgvRecentLoans.Location = new Point(0, 355);
        dgvRecentLoans.Name = "dgvRecentLoans";
        dgvRecentLoans.RowHeadersVisible = false;
        dgvRecentLoans.RowTemplate.Height = 25;
        dgvRecentLoans.Size = new Size(980, 275);
        dgvRecentLoans.TabIndex = 3;

        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(245, 247, 250);
        ClientSize = new Size(980, 630);
        Controls.Add(dgvRecentLoans);
        Controls.Add(pnlRecentHeader);
        Controls.Add(flpCards);
        Controls.Add(pnlTop);
        Name = "FrmDashboard";
        Text = "Dashboard";
        Load += FrmDashboard_Load;

        pnlTop.ResumeLayout(false);
        pnlTop.PerformLayout();
        pnlRecentHeader.ResumeLayout(false);
        pnlRecentHeader.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)dgvRecentLoans).EndInit();
        ResumeLayout(false);
    }
}