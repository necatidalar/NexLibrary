namespace NexLibrary.Desktop.Forms;

partial class FrmBookCopies
{
    private System.ComponentModel.IContainer components = null;
    private Panel pnlTop;
    private Label lblTitle;
    private Button btnRefresh;
    private Button btnAddCopy;
    private Label lblStockCount;
    private SplitContainer splitContainer;
    private DataGridView dgvStock;
    private Panel pnlCopiesHeader;
    private Label lblCopiesTitle;
    private Label lblCopiesCount;
    private DataGridView dgvCopies;

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
        lblStockCount = new Label();
        btnAddCopy = new Button();
        btnRefresh = new Button();
        lblTitle = new Label();
        splitContainer = new SplitContainer();
        dgvStock = new DataGridView();
        pnlCopiesHeader = new Panel();
        lblCopiesCount = new Label();
        lblCopiesTitle = new Label();
        dgvCopies = new DataGridView();

        pnlTop.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)splitContainer).BeginInit();
        splitContainer.Panel1.SuspendLayout();
        splitContainer.Panel2.SuspendLayout();
        splitContainer.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvStock).BeginInit();
        pnlCopiesHeader.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvCopies).BeginInit();
        SuspendLayout();

        pnlTop.BackColor = Color.White;
        pnlTop.Controls.Add(lblStockCount);
        pnlTop.Controls.Add(btnAddCopy);
        pnlTop.Controls.Add(btnRefresh);
        pnlTop.Controls.Add(lblTitle);
        pnlTop.Dock = DockStyle.Top;
        pnlTop.Location = new Point(0, 0);
        pnlTop.Name = "pnlTop";
        pnlTop.Padding = new Padding(15);
        pnlTop.Size = new Size(980, 90);
        pnlTop.TabIndex = 0;

        lblTitle.AutoSize = true;
        lblTitle.Font = new Font("Segoe UI", 15F, FontStyle.Bold);
        lblTitle.Location = new Point(15, 15);
        lblTitle.Name = "lblTitle";
        lblTitle.Size = new Size(238, 28);
        lblTitle.TabIndex = 0;
        lblTitle.Text = "Kitap Kopya / Stok Yönetimi";

        btnRefresh.Location = new Point(18, 55);
        btnRefresh.Name = "btnRefresh";
        btnRefresh.Size = new Size(90, 28);
        btnRefresh.TabIndex = 1;
        btnRefresh.Text = "Yenile";
        btnRefresh.UseVisualStyleBackColor = true;
        btnRefresh.Click += btnRefresh_Click;

        btnAddCopy.Location = new Point(115, 55);
        btnAddCopy.Name = "btnAddCopy";
        btnAddCopy.Size = new Size(120, 28);
        btnAddCopy.TabIndex = 2;
        btnAddCopy.Text = "Yeni Kopya Ekle";
        btnAddCopy.UseVisualStyleBackColor = true;
        btnAddCopy.Click += btnAddCopy_Click;

        lblStockCount.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        lblStockCount.Font = new Font("Segoe UI", 9F);
        lblStockCount.ForeColor = Color.DimGray;
        lblStockCount.Location = new Point(650, 20);
        lblStockCount.Name = "lblStockCount";
        lblStockCount.Size = new Size(305, 20);
        lblStockCount.TabIndex = 3;
        lblStockCount.Text = "Kitap sayısı: 0";
        lblStockCount.TextAlign = ContentAlignment.MiddleRight;

        splitContainer.Dock = DockStyle.Fill;
        splitContainer.Location = new Point(0, 90);
        splitContainer.Name = "splitContainer";
        splitContainer.Orientation = Orientation.Horizontal;
        splitContainer.Size = new Size(980, 540);
        splitContainer.SplitterDistance = 270;
        splitContainer.TabIndex = 1;

        splitContainer.Panel1.Controls.Add(dgvStock);

        dgvStock.BackgroundColor = Color.White;
        dgvStock.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dgvStock.Dock = DockStyle.Fill;
        dgvStock.Location = new Point(0, 0);
        dgvStock.Name = "dgvStock";
        dgvStock.RowHeadersVisible = false;
        dgvStock.RowTemplate.Height = 25;
        dgvStock.Size = new Size(980, 270);
        dgvStock.TabIndex = 0;
        dgvStock.SelectionChanged += dgvStock_SelectionChanged;

        splitContainer.Panel2.Controls.Add(dgvCopies);
        splitContainer.Panel2.Controls.Add(pnlCopiesHeader);

        pnlCopiesHeader.BackColor = Color.White;
        pnlCopiesHeader.Controls.Add(lblCopiesCount);
        pnlCopiesHeader.Controls.Add(lblCopiesTitle);
        pnlCopiesHeader.Dock = DockStyle.Top;
        pnlCopiesHeader.Location = new Point(0, 0);
        pnlCopiesHeader.Name = "pnlCopiesHeader";
        pnlCopiesHeader.Size = new Size(980, 45);
        pnlCopiesHeader.TabIndex = 0;

        lblCopiesTitle.AutoSize = true;
        lblCopiesTitle.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
        lblCopiesTitle.Location = new Point(15, 12);
        lblCopiesTitle.Name = "lblCopiesTitle";
        lblCopiesTitle.Size = new Size(72, 20);
        lblCopiesTitle.TabIndex = 0;
        lblCopiesTitle.Text = "Kopyalar";

        lblCopiesCount.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        lblCopiesCount.Font = new Font("Segoe UI", 9F);
        lblCopiesCount.ForeColor = Color.DimGray;
        lblCopiesCount.Location = new Point(650, 13);
        lblCopiesCount.Name = "lblCopiesCount";
        lblCopiesCount.Size = new Size(305, 20);
        lblCopiesCount.TabIndex = 1;
        lblCopiesCount.Text = "Kopya sayısı: 0";
        lblCopiesCount.TextAlign = ContentAlignment.MiddleRight;

        dgvCopies.BackgroundColor = Color.White;
        dgvCopies.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dgvCopies.Dock = DockStyle.Fill;
        dgvCopies.Location = new Point(0, 45);
        dgvCopies.Name = "dgvCopies";
        dgvCopies.RowHeadersVisible = false;
        dgvCopies.RowTemplate.Height = 25;
        dgvCopies.Size = new Size(980, 221);
        dgvCopies.TabIndex = 1;

        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(245, 247, 250);
        ClientSize = new Size(980, 630);
        Controls.Add(splitContainer);
        Controls.Add(pnlTop);
        Name = "FrmBookCopies";
        Text = "Kitap Kopyaları";
        Load += FrmBookCopies_Load;

        pnlTop.ResumeLayout(false);
        pnlTop.PerformLayout();
        splitContainer.Panel1.ResumeLayout(false);
        splitContainer.Panel2.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)splitContainer).EndInit();
        splitContainer.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)dgvStock).EndInit();
        pnlCopiesHeader.ResumeLayout(false);
        pnlCopiesHeader.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)dgvCopies).EndInit();
        ResumeLayout(false);
    }
}