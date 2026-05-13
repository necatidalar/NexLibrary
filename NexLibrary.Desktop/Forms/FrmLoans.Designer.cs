namespace NexLibrary.Desktop.Forms;

partial class FrmLoans
{
    private System.ComponentModel.IContainer components = null;
    private Panel pnlTop;
    private Label lblTitle;
    private TextBox txtSearch;
    private Button btnSearch;
    private Button btnRefresh;
    private Button btnAdd;
    private Button btnCancelLoan;
    private Button btnReturn;
    private Button btnShowOverdue;
    private Label lblCount;
    private DataGridView dgvLoans;

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
        btnCancelLoan = new Button();
        btnAdd = new Button();
        btnRefresh = new Button();
        btnSearch = new Button();
        btnReturn = new Button();
        btnShowOverdue = new Button();
        txtSearch = new TextBox();
        lblTitle = new Label();
        dgvLoans = new DataGridView();

        pnlTop.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvLoans).BeginInit();
        SuspendLayout();

        pnlTop.BackColor = Color.White;
        pnlTop.Controls.Add(lblCount);
        pnlTop.Controls.Add(btnCancelLoan);
        pnlTop.Controls.Add(btnAdd);
        pnlTop.Controls.Add(btnRefresh);
        pnlTop.Controls.Add(btnSearch);
        pnlTop.Controls.Add(txtSearch);
        pnlTop.Controls.Add(lblTitle);
        pnlTop.Controls.Add(btnShowOverdue);
        pnlTop.Controls.Add(btnReturn);
        pnlTop.Dock = DockStyle.Top;
        pnlTop.Location = new Point(0, 0);
        pnlTop.Padding = new Padding(15);
        pnlTop.Size = new Size(980, 115);

        lblTitle.AutoSize = true;
        lblTitle.Font = new Font("Segoe UI", 15F, FontStyle.Bold);
        lblTitle.Location = new Point(15, 15);
        lblTitle.Text = "Ödünç İşlemleri";

        txtSearch.Location = new Point(18, 65);
        txtSearch.PlaceholderText = "Kitap veya üye ara...";
        txtSearch.Size = new Size(260, 23);
        txtSearch.KeyDown += txtSearch_KeyDown;

        btnSearch.Location = new Point(285, 64);
        btnSearch.Size = new Size(90, 26);
        btnSearch.Text = "Ara";
        btnSearch.Click += btnSearch_Click;

        btnRefresh.Location = new Point(380, 64);
        btnRefresh.Size = new Size(90, 26);
        btnRefresh.Text = "Yenile";
        btnRefresh.Click += btnRefresh_Click;

        btnAdd.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnAdd.Location = new Point(730, 63);
        btnAdd.Size = new Size(100, 30);
        btnAdd.Text = "Ödünç Ver";
        btnAdd.Click += btnAdd_Click;

        btnCancelLoan.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnCancelLoan.Location = new Point(840, 63);
        btnCancelLoan.Size = new Size(105, 30);
        btnCancelLoan.Text = "İptal Et";
        btnCancelLoan.Click += btnCancelLoan_Click;

        btnShowOverdue.Location = new Point(480, 64);
        btnShowOverdue.Name = "btnShowOverdue";
        btnShowOverdue.Size = new Size(130, 28);
        btnShowOverdue.TabIndex = 4;
        btnShowOverdue.Text = "Gecikenleri Göster";
        btnShowOverdue.UseVisualStyleBackColor = true;
        btnShowOverdue.Click += btnShowOverdue_Click;

        btnAdd.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnAdd.Location = new Point(620, 63);
        btnAdd.Size = new Size(100, 30);

        btnReturn.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnReturn.Location = new Point(730, 63);
        btnReturn.Name = "btnReturn";
        btnReturn.Size = new Size(100, 30);
        btnReturn.TabIndex = 6;
        btnReturn.Text = "İade Al";
        btnReturn.UseVisualStyleBackColor = true;
        btnReturn.Click += btnReturn_Click;

        btnCancelLoan.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnCancelLoan.Location = new Point(840, 63);
        btnCancelLoan.Size = new Size(105, 30);

        lblCount.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        lblCount.Location = new Point(650, 20);
        lblCount.Size = new Size(295, 20);
        lblCount.Text = "Toplam ödünç: 0";
        lblCount.TextAlign = ContentAlignment.MiddleRight;

        dgvLoans.BackgroundColor = Color.White;
        dgvLoans.Dock = DockStyle.Fill;
        dgvLoans.Location = new Point(0, 115);
        dgvLoans.RowHeadersVisible = false;
        dgvLoans.Size = new Size(980, 515);
        dgvLoans.SelectionChanged += dgvLoans_SelectionChanged;

        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(245, 247, 250);
        ClientSize = new Size(980, 630);
        Controls.Add(dgvLoans);
        Controls.Add(pnlTop);
        Name = "FrmLoans";
        Text = "Ödünç İşlemleri";
        Load += FrmLoans_Load;

        pnlTop.ResumeLayout(false);
        pnlTop.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)dgvLoans).EndInit();
        ResumeLayout(false);
    }
}