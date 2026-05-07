namespace NexLibrary.Desktop.Forms;

partial class FrmBooks
{
    private System.ComponentModel.IContainer components = null;
    private Panel pnlTop;
    private TextBox txtSearch;
    private Button btnSearch;
    private Button btnRefresh;
    private Button btnAdd;
    private Button btnEdit;
    private Button btnDelete;
    private DataGridView dgvBooks;
    private Label lblTitle;
    private Label lblCount;

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
        btnDelete = new Button();
        btnEdit = new Button();
        btnAdd = new Button();
        btnRefresh = new Button();
        btnSearch = new Button();
        txtSearch = new TextBox();
        lblTitle = new Label();
        dgvBooks = new DataGridView();

        pnlTop.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvBooks).BeginInit();
        SuspendLayout();

        pnlTop.BackColor = Color.White;
        pnlTop.Controls.Add(lblCount);
        pnlTop.Controls.Add(btnDelete);
        pnlTop.Controls.Add(btnEdit);
        pnlTop.Controls.Add(btnAdd);
        pnlTop.Controls.Add(btnRefresh);
        pnlTop.Controls.Add(btnSearch);
        pnlTop.Controls.Add(txtSearch);
        pnlTop.Controls.Add(lblTitle);
        pnlTop.Dock = DockStyle.Top;
        pnlTop.Location = new Point(0, 0);
        pnlTop.Name = "pnlTop";
        pnlTop.Padding = new Padding(15);
        pnlTop.Size = new Size(980, 115);
        pnlTop.TabIndex = 0;

        lblTitle.AutoSize = true;
        lblTitle.Font = new Font("Segoe UI", 15F, FontStyle.Bold);
        lblTitle.Location = new Point(15, 15);
        lblTitle.Name = "lblTitle";
        lblTitle.Size = new Size(86, 28);
        lblTitle.TabIndex = 0;
        lblTitle.Text = "Kitaplar";

        txtSearch.Location = new Point(18, 65);
        txtSearch.Name = "txtSearch";
        txtSearch.PlaceholderText = "Kitap adı ara...";
        txtSearch.Size = new Size(260, 23);
        txtSearch.TabIndex = 1;
        txtSearch.KeyDown += txtSearch_KeyDown;

        btnSearch.Location = new Point(285, 64);
        btnSearch.Name = "btnSearch";
        btnSearch.Size = new Size(90, 26);
        btnSearch.TabIndex = 2;
        btnSearch.Text = "Ara";
        btnSearch.UseVisualStyleBackColor = true;
        btnSearch.Click += btnSearch_Click;

        btnRefresh.Location = new Point(380, 64);
        btnRefresh.Name = "btnRefresh";
        btnRefresh.Size = new Size(90, 26);
        btnRefresh.TabIndex = 3;
        btnRefresh.Text = "Yenile";
        btnRefresh.UseVisualStyleBackColor = true;
        btnRefresh.Click += btnRefresh_Click;

        btnAdd.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnAdd.Location = new Point(650, 63);
        btnAdd.Name = "btnAdd";
        btnAdd.Size = new Size(95, 30);
        btnAdd.TabIndex = 4;
        btnAdd.Text = "Yeni";
        btnAdd.UseVisualStyleBackColor = true;
        btnAdd.Click += btnAdd_Click;

        btnEdit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnEdit.Location = new Point(750, 63);
        btnEdit.Name = "btnEdit";
        btnEdit.Size = new Size(95, 30);
        btnEdit.TabIndex = 5;
        btnEdit.Text = "Düzenle";
        btnEdit.UseVisualStyleBackColor = true;
        btnEdit.Click += btnEdit_Click;

        btnDelete.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnDelete.Location = new Point(850, 63);
        btnDelete.Name = "btnDelete";
        btnDelete.Size = new Size(95, 30);
        btnDelete.TabIndex = 6;
        btnDelete.Text = "Pasif Yap";
        btnDelete.UseVisualStyleBackColor = true;
        btnDelete.Click += btnDelete_Click;

        lblCount.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        lblCount.Font = new Font("Segoe UI", 9F);
        lblCount.ForeColor = Color.DimGray;
        lblCount.Location = new Point(650, 20);
        lblCount.Name = "lblCount";
        lblCount.Size = new Size(295, 20);
        lblCount.TabIndex = 7;
        lblCount.Text = "Toplam kayıt: 0";
        lblCount.TextAlign = ContentAlignment.MiddleRight;

        dgvBooks.BackgroundColor = Color.White;
        dgvBooks.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dgvBooks.Dock = DockStyle.Fill;
        dgvBooks.Location = new Point(0, 115);
        dgvBooks.Name = "dgvBooks";
        dgvBooks.RowHeadersVisible = false;
        dgvBooks.RowTemplate.Height = 25;
        dgvBooks.Size = new Size(980, 515);
        dgvBooks.TabIndex = 1;
        dgvBooks.SelectionChanged += dgvBooks_SelectionChanged;
        dgvBooks.CellDoubleClick += dgvBooks_CellDoubleClick;

        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(245, 247, 250);
        ClientSize = new Size(980, 630);
        Controls.Add(dgvBooks);
        Controls.Add(pnlTop);
        Name = "FrmBooks";
        Text = "Kitaplar";
        Load += FrmBooks_Load;

        pnlTop.ResumeLayout(false);
        pnlTop.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)dgvBooks).EndInit();
        ResumeLayout(false);
    }
}