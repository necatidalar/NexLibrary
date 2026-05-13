namespace NexLibrary.Desktop.Forms;

partial class FrmMemberDetail
{
    private System.ComponentModel.IContainer components = null;
    private Panel pnlHeader;
    private Panel pnlFooter;
    private Panel pnlMain;
    private Label lblTitle;
    private Button btnClose;

    private Label lblUyeAdiSoyadi;
    private TextBox txtUyeAdiSoyadi;
    private Label lblAktifMi;
    private TextBox txtAktifMi;
    private Label lblOlusturmaTarihi;
    private TextBox txtOlusturmaTarihi;
    private Label lblGuncellemeTarihi;
    private TextBox txtGuncellemeTarihi;
    private Label lblDynamicTitle;
    private Panel pnlDynamicFields;

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
        pnlHeader = new Panel();
        lblTitle = new Label();
        pnlFooter = new Panel();
        btnClose = new Button();
        pnlMain = new Panel();

        lblUyeAdiSoyadi = new Label();
        txtUyeAdiSoyadi = new TextBox();
        lblAktifMi = new Label();
        txtAktifMi = new TextBox();
        lblOlusturmaTarihi = new Label();
        txtOlusturmaTarihi = new TextBox();
        lblGuncellemeTarihi = new Label();
        txtGuncellemeTarihi = new TextBox();
        lblDynamicTitle = new Label();
        pnlDynamicFields = new Panel();

        pnlHeader.SuspendLayout();
        pnlFooter.SuspendLayout();
        pnlMain.SuspendLayout();
        SuspendLayout();

        pnlHeader.BackColor = Color.White;
        pnlHeader.Controls.Add(lblTitle);
        pnlHeader.Dock = DockStyle.Top;
        pnlHeader.Location = new Point(0, 0);
        pnlHeader.Name = "pnlHeader";
        pnlHeader.Size = new Size(700, 70);
        pnlHeader.TabIndex = 0;

        lblTitle.AutoSize = true;
        lblTitle.Font = new Font("Segoe UI", 15F, FontStyle.Bold);
        lblTitle.Location = new Point(20, 20);
        lblTitle.Name = "lblTitle";
        lblTitle.Size = new Size(111, 28);
        lblTitle.TabIndex = 0;
        lblTitle.Text = "Üye Detayı";

        pnlFooter.BackColor = Color.White;
        pnlFooter.Controls.Add(btnClose);
        pnlFooter.Dock = DockStyle.Bottom;
        pnlFooter.Location = new Point(0, 550);
        pnlFooter.Name = "pnlFooter";
        pnlFooter.Size = new Size(700, 70);
        pnlFooter.TabIndex = 2;

        btnClose.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnClose.Location = new Point(585, 18);
        btnClose.Name = "btnClose";
        btnClose.Size = new Size(90, 32);
        btnClose.TabIndex = 0;
        btnClose.Text = "Kapat";
        btnClose.UseVisualStyleBackColor = true;
        btnClose.Click += btnClose_Click;

        pnlMain.AutoScroll = true;
        pnlMain.BackColor = Color.FromArgb(245, 247, 250);
        pnlMain.Controls.Add(lblUyeAdiSoyadi);
        pnlMain.Controls.Add(txtUyeAdiSoyadi);
        pnlMain.Controls.Add(lblAktifMi);
        pnlMain.Controls.Add(txtAktifMi);
        pnlMain.Controls.Add(lblOlusturmaTarihi);
        pnlMain.Controls.Add(txtOlusturmaTarihi);
        pnlMain.Controls.Add(lblGuncellemeTarihi);
        pnlMain.Controls.Add(txtGuncellemeTarihi);
        pnlMain.Controls.Add(lblDynamicTitle);
        pnlMain.Controls.Add(pnlDynamicFields);
        pnlMain.Dock = DockStyle.Fill;
        pnlMain.Location = new Point(0, 70);
        pnlMain.Name = "pnlMain";
        pnlMain.Padding = new Padding(20);
        pnlMain.Size = new Size(700, 480);
        pnlMain.TabIndex = 1;

        lblUyeAdiSoyadi.Location = new Point(35, 30);
        lblUyeAdiSoyadi.Name = "lblUyeAdiSoyadi";
        lblUyeAdiSoyadi.Size = new Size(150, 23);
        lblUyeAdiSoyadi.TabIndex = 0;
        lblUyeAdiSoyadi.Text = "Üye Adı Soyadı";

        txtUyeAdiSoyadi.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        txtUyeAdiSoyadi.Location = new Point(210, 27);
        txtUyeAdiSoyadi.Name = "txtUyeAdiSoyadi";
        txtUyeAdiSoyadi.ReadOnly = true;
        txtUyeAdiSoyadi.Size = new Size(420, 23);
        txtUyeAdiSoyadi.TabIndex = 1;

        lblAktifMi.Location = new Point(35, 65);
        lblAktifMi.Name = "lblAktifMi";
        lblAktifMi.Size = new Size(150, 23);
        lblAktifMi.TabIndex = 2;
        lblAktifMi.Text = "Durum";

        txtAktifMi.Location = new Point(210, 62);
        txtAktifMi.Name = "txtAktifMi";
        txtAktifMi.ReadOnly = true;
        txtAktifMi.Size = new Size(160, 23);
        txtAktifMi.TabIndex = 3;

        lblOlusturmaTarihi.Location = new Point(35, 100);
        lblOlusturmaTarihi.Name = "lblOlusturmaTarihi";
        lblOlusturmaTarihi.Size = new Size(150, 23);
        lblOlusturmaTarihi.TabIndex = 4;
        lblOlusturmaTarihi.Text = "Oluşturma Tarihi";

        txtOlusturmaTarihi.Location = new Point(210, 97);
        txtOlusturmaTarihi.Name = "txtOlusturmaTarihi";
        txtOlusturmaTarihi.ReadOnly = true;
        txtOlusturmaTarihi.Size = new Size(160, 23);
        txtOlusturmaTarihi.TabIndex = 5;

        lblGuncellemeTarihi.Location = new Point(35, 135);
        lblGuncellemeTarihi.Name = "lblGuncellemeTarihi";
        lblGuncellemeTarihi.Size = new Size(150, 23);
        lblGuncellemeTarihi.TabIndex = 6;
        lblGuncellemeTarihi.Text = "Güncelleme Tarihi";

        txtGuncellemeTarihi.Location = new Point(210, 132);
        txtGuncellemeTarihi.Name = "txtGuncellemeTarihi";
        txtGuncellemeTarihi.ReadOnly = true;
        txtGuncellemeTarihi.Size = new Size(160, 23);
        txtGuncellemeTarihi.TabIndex = 7;

        lblDynamicTitle.AutoSize = true;
        lblDynamicTitle.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
        lblDynamicTitle.Location = new Point(35, 180);
        lblDynamicTitle.Name = "lblDynamicTitle";
        lblDynamicTitle.Size = new Size(123, 20);
        lblDynamicTitle.TabIndex = 8;
        lblDynamicTitle.Text = "Dinamik Alanlar";

        pnlDynamicFields.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
        pnlDynamicFields.AutoScroll = true;
        pnlDynamicFields.BackColor = Color.White;
        pnlDynamicFields.BorderStyle = BorderStyle.FixedSingle;
        pnlDynamicFields.Location = new Point(35, 210);
        pnlDynamicFields.Name = "pnlDynamicFields";
        pnlDynamicFields.Size = new Size(630, 240);
        pnlDynamicFields.TabIndex = 9;

        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(700, 620);
        Controls.Add(pnlMain);
        Controls.Add(pnlFooter);
        Controls.Add(pnlHeader);
        MinimumSize = new Size(650, 550);
        Name = "FrmMemberDetail";
        StartPosition = FormStartPosition.CenterParent;
        Text = "Üye Detayı";
        Load += FrmMemberDetail_Load;

        pnlHeader.ResumeLayout(false);
        pnlHeader.PerformLayout();
        pnlFooter.ResumeLayout(false);
        pnlMain.ResumeLayout(false);
        pnlMain.PerformLayout();
        ResumeLayout(false);
    }
}