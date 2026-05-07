namespace NexLibrary.Desktop.Forms;

partial class FrmListSettings
{
    private System.ComponentModel.IContainer components = null;
    private Panel pnlTop;
    private Label lblTitle;
    private Label lblModule;
    private ComboBox cmbModule;
    private Button btnRefresh;
    private Button btnSave;
    private Label lblCount;
    private DataGridView dgvSettings;
    private Label lblInfo;

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
        lblInfo = new Label();
        lblCount = new Label();
        btnSave = new Button();
        btnRefresh = new Button();
        cmbModule = new ComboBox();
        lblModule = new Label();
        lblTitle = new Label();
        dgvSettings = new DataGridView();

        pnlTop.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvSettings).BeginInit();
        SuspendLayout();

        pnlTop.BackColor = Color.White;
        pnlTop.Controls.Add(lblInfo);
        pnlTop.Controls.Add(lblCount);
        pnlTop.Controls.Add(btnSave);
        pnlTop.Controls.Add(btnRefresh);
        pnlTop.Controls.Add(cmbModule);
        pnlTop.Controls.Add(lblModule);
        pnlTop.Controls.Add(lblTitle);
        pnlTop.Dock = DockStyle.Top;
        pnlTop.Location = new Point(0, 0);
        pnlTop.Name = "pnlTop";
        pnlTop.Padding = new Padding(15);
        pnlTop.Size = new Size(980, 125);
        pnlTop.TabIndex = 0;

        lblTitle.AutoSize = true;
        lblTitle.Font = new Font("Segoe UI", 15F, FontStyle.Bold);
        lblTitle.Location = new Point(15, 15);
        lblTitle.Name = "lblTitle";
        lblTitle.Size = new Size(135, 28);
        lblTitle.TabIndex = 0;
        lblTitle.Text = "Liste Ayarları";

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

        btnSave.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnSave.Location = new Point(840, 64);
        btnSave.Name = "btnSave";
        btnSave.Size = new Size(115, 32);
        btnSave.TabIndex = 4;
        btnSave.Text = "Ayarları Kaydet";
        btnSave.UseVisualStyleBackColor = true;
        btnSave.Click += btnSave_Click;

        lblCount.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        lblCount.Font = new Font("Segoe UI", 9F);
        lblCount.ForeColor = Color.DimGray;
        lblCount.Location = new Point(650, 20);
        lblCount.Name = "lblCount";
        lblCount.Size = new Size(305, 20);
        lblCount.TabIndex = 5;
        lblCount.Text = "Toplam alan: 0";
        lblCount.TextAlign = ContentAlignment.MiddleRight;

        lblInfo.AutoSize = true;
        lblInfo.Font = new Font("Segoe UI", 9F);
        lblInfo.ForeColor = Color.DimGray;
        lblInfo.Location = new Point(18, 100);
        lblInfo.Name = "lblInfo";
        lblInfo.Size = new Size(568, 15);
        lblInfo.TabIndex = 6;
        lblInfo.Text = "Not: Sistem alanları korunur. Listede kapatılan alanlar Kitaplar ekranındaki DataGrid kolonlarından kaldırılır.";

        dgvSettings.BackgroundColor = Color.White;
        dgvSettings.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dgvSettings.Dock = DockStyle.Fill;
        dgvSettings.Location = new Point(0, 125);
        dgvSettings.Name = "dgvSettings";
        dgvSettings.RowHeadersVisible = false;
        dgvSettings.RowTemplate.Height = 25;
        dgvSettings.Size = new Size(980, 505);
        dgvSettings.TabIndex = 1;

        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(245, 247, 250);
        ClientSize = new Size(980, 630);
        Controls.Add(dgvSettings);
        Controls.Add(pnlTop);
        Name = "FrmListSettings";
        Text = "Liste Ayarları";
        Load += FrmListSettings_Load;

        pnlTop.ResumeLayout(false);
        pnlTop.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)dgvSettings).EndInit();
        ResumeLayout(false);
    }
}