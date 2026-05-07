namespace NexLibrary.Desktop.Forms;

partial class FrmFormFields
{
    private System.ComponentModel.IContainer components = null;
    private Panel pnlTop;
    private Label lblTitle;
    private Label lblModule;
    private ComboBox cmbModule;
    private Button btnRefresh;
    private Button btnAdd;
    private Button btnEdit;
    private Button btnSetActive;
    private Label lblCount;
    private DataGridView dgvFields;

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
        btnSetActive = new Button();
        btnEdit = new Button();
        btnAdd = new Button();
        btnRefresh = new Button();
        cmbModule = new ComboBox();
        lblModule = new Label();
        lblTitle = new Label();
        dgvFields = new DataGridView();

        pnlTop.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvFields).BeginInit();
        SuspendLayout();

        pnlTop.BackColor = Color.White;
        pnlTop.Controls.Add(lblCount);
        pnlTop.Controls.Add(btnSetActive);
        pnlTop.Controls.Add(btnEdit);
        pnlTop.Controls.Add(btnAdd);
        pnlTop.Controls.Add(btnRefresh);
        pnlTop.Controls.Add(cmbModule);
        pnlTop.Controls.Add(lblModule);
        pnlTop.Controls.Add(lblTitle);
        pnlTop.Dock = DockStyle.Top;
        pnlTop.Location = new Point(0, 0);
        pnlTop.Name = "pnlTop";
        pnlTop.Padding = new Padding(15);
        pnlTop.Size = new Size(980, 120);
        pnlTop.TabIndex = 0;

        lblTitle.AutoSize = true;
        lblTitle.Font = new Font("Segoe UI", 15F, FontStyle.Bold);
        lblTitle.Location = new Point(15, 15);
        lblTitle.Name = "lblTitle";
        lblTitle.Size = new Size(132, 28);
        lblTitle.TabIndex = 0;
        lblTitle.Text = "Form Alanları";

        lblModule.AutoSize = true;
        lblModule.Location = new Point(18, 72);
        lblModule.Name = "lblModule";
        lblModule.Size = new Size(45, 15);
        lblModule.TabIndex = 1;
        lblModule.Text = "Modül:";

        cmbModule.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbModule.FormattingEnabled = true;
        cmbModule.Items.AddRange(new object[]
        {
            "Kitaplar",
            "Uyeler",
            "Personeller",
            "Oduncler",
            "Iadeler"
        });
        cmbModule.Location = new Point(70, 68);
        cmbModule.Name = "cmbModule";
        cmbModule.Size = new Size(170, 23);
        cmbModule.TabIndex = 2;
        cmbModule.SelectedIndexChanged += cmbModule_SelectedIndexChanged;

        btnRefresh.Location = new Point(250, 66);
        btnRefresh.Name = "btnRefresh";
        btnRefresh.Size = new Size(90, 28);
        btnRefresh.TabIndex = 3;
        btnRefresh.Text = "Yenile";
        btnRefresh.UseVisualStyleBackColor = true;
        btnRefresh.Click += btnRefresh_Click;

        btnAdd.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnAdd.Location = new Point(565, 64);
        btnAdd.Name = "btnAdd";
        btnAdd.Size = new Size(95, 32);
        btnAdd.TabIndex = 4;
        btnAdd.Text = "Yeni Alan";
        btnAdd.UseVisualStyleBackColor = true;
        btnAdd.Click += btnAdd_Click;

        btnEdit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnEdit.Location = new Point(665, 64);
        btnEdit.Name = "btnEdit";
        btnEdit.Size = new Size(95, 32);
        btnEdit.TabIndex = 5;
        btnEdit.Text = "Düzenle";
        btnEdit.UseVisualStyleBackColor = true;
        btnEdit.Click += btnEdit_Click;

        btnSetActive.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnSetActive.Location = new Point(765, 64);
        btnSetActive.Name = "btnSetActive";
        btnSetActive.Size = new Size(110, 32);
        btnSetActive.TabIndex = 6;
        btnSetActive.Text = "Aktif/Pasif";
        btnSetActive.UseVisualStyleBackColor = true;
        btnSetActive.Click += btnSetActive_Click;

        lblCount.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        lblCount.Font = new Font("Segoe UI", 9F);
        lblCount.ForeColor = Color.DimGray;
        lblCount.Location = new Point(650, 20);
        lblCount.Name = "lblCount";
        lblCount.Size = new Size(305, 20);
        lblCount.TabIndex = 7;
        lblCount.Text = "Toplam alan: 0";
        lblCount.TextAlign = ContentAlignment.MiddleRight;

        dgvFields.BackgroundColor = Color.White;
        dgvFields.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dgvFields.Dock = DockStyle.Fill;
        dgvFields.Location = new Point(0, 120);
        dgvFields.Name = "dgvFields";
        dgvFields.RowHeadersVisible = false;
        dgvFields.RowTemplate.Height = 25;
        dgvFields.Size = new Size(980, 510);
        dgvFields.TabIndex = 1;
        dgvFields.SelectionChanged += dgvFields_SelectionChanged;
        dgvFields.CellDoubleClick += dgvFields_CellDoubleClick;

        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(245, 247, 250);
        ClientSize = new Size(980, 630);
        Controls.Add(dgvFields);
        Controls.Add(pnlTop);
        Name = "FrmFormFields";
        Text = "Form Alanları";
        Load += FrmFormFields_Load;

        pnlTop.ResumeLayout(false);
        pnlTop.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)dgvFields).EndInit();
        ResumeLayout(false);
    }
}