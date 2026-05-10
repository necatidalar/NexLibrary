namespace NexLibrary.Desktop.Forms;

partial class FrmMembers
{
    private System.ComponentModel.IContainer components = null;
    private Panel pnlTop;
    private Label lblTitle;
    private TextBox txtSearch;
    private Button btnSearch;
    private Button btnRefresh;
    private Button btnAdd;
    private Label lblCount;
    private DataGridView dgvMembers;

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
        lblCount = new Label();
        btnAdd = new Button();
        btnRefresh = new Button();
        btnSearch = new Button();
        txtSearch = new TextBox();
        lblTitle = new Label();
        dgvMembers = new DataGridView();

        pnlTop.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvMembers).BeginInit();
        SuspendLayout();

        pnlTop.BackColor = Color.White;
        pnlTop.Controls.Add(lblCount);
        pnlTop.Controls.Add(btnAdd);
        pnlTop.Controls.Add(btnRefresh);
        pnlTop.Controls.Add(btnSearch);
        pnlTop.Controls.Add(txtSearch);
        pnlTop.Controls.Add(lblTitle);
        pnlTop.Dock = DockStyle.Top;
        pnlTop.Location = new Point(0, 0);
        pnlTop.Size = new Size(980, 115);

        lblTitle.AutoSize = true;
        lblTitle.Font = new Font("Segoe UI", 15F, FontStyle.Bold);
        lblTitle.Location = new Point(15, 15);
        lblTitle.Text = "Üyeler";

        txtSearch.Location = new Point(18, 65);
        txtSearch.PlaceholderText = "Üye ara...";
        txtSearch.Size = new Size(260, 23);

        btnSearch.Location = new Point(285, 64);
        btnSearch.Size = new Size(90, 26);
        btnSearch.Text = "Ara";
        btnSearch.Click += btnSearch_Click;

        btnRefresh.Location = new Point(380, 64);
        btnRefresh.Size = new Size(90, 26);
        btnRefresh.Text = "Yenile";
        btnRefresh.Click += btnRefresh_Click;

        btnAdd.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnAdd.Location = new Point(850, 63);
        btnAdd.Size = new Size(95, 30);
        btnAdd.Text = "Yeni";
        btnAdd.Click += btnAdd_Click;

        lblCount.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        lblCount.Location = new Point(650, 20);
        lblCount.Size = new Size(295, 20);
        lblCount.Text = "Toplam üye: 0";
        lblCount.TextAlign = ContentAlignment.MiddleRight;

        dgvMembers.BackgroundColor = Color.White;
        dgvMembers.Dock = DockStyle.Fill;
        dgvMembers.Location = new Point(0, 115);
        dgvMembers.RowHeadersVisible = false;
        dgvMembers.Size = new Size(980, 515);

        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(245, 247, 250);
        ClientSize = new Size(980, 630);
        Controls.Add(dgvMembers);
        Controls.Add(pnlTop);
        Name = "FrmMembers";
        Text = "Üyeler";
        Load += FrmMembers_Load;

        pnlTop.ResumeLayout(false);
        pnlTop.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)dgvMembers).EndInit();
        ResumeLayout(false);
    }
}