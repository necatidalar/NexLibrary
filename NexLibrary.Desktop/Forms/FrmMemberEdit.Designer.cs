namespace NexLibrary.Desktop.Forms;

partial class FrmMemberEdit
{
    private System.ComponentModel.IContainer components = null;
    private Panel pnlHeader;
    private Panel pnlFooter;
    private Panel pnlMain;
    private Label lblTitle;
    private Label lblUyeAdiSoyadi;
    private TextBox txtUyeAdiSoyadi;
    private Panel pnlDynamicFields;
    private Button btnSave;
    private Button btnCancel;
    private CheckBox chkAktifMi;

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
        btnCancel = new Button();
        btnSave = new Button();
        pnlMain = new Panel();
        pnlDynamicFields = new Panel();
        txtUyeAdiSoyadi = new TextBox();
        lblUyeAdiSoyadi = new Label();
        chkAktifMi = new CheckBox();

        pnlHeader.SuspendLayout();
        pnlFooter.SuspendLayout();
        pnlMain.SuspendLayout();
        SuspendLayout();

        pnlHeader.BackColor = Color.White;
        pnlHeader.Controls.Add(lblTitle);
        pnlHeader.Dock = DockStyle.Top;
        pnlHeader.Size = new Size(620, 65);

        lblTitle.AutoSize = true;
        lblTitle.Font = new Font("Segoe UI", 15F, FontStyle.Bold);
        lblTitle.Location = new Point(20, 18);
        lblTitle.Text = "Yeni Üye";

        pnlFooter.BackColor = Color.White;
        pnlFooter.Controls.Add(btnCancel);
        pnlFooter.Controls.Add(btnSave);
        pnlFooter.Dock = DockStyle.Bottom;
        pnlFooter.Location = new Point(0, 520);
        pnlFooter.Size = new Size(620, 70);

        btnCancel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnCancel.Location = new Point(405, 18);
        btnCancel.Size = new Size(90, 32);
        btnCancel.Text = "İptal";
        btnCancel.Click += btnCancel_Click;

        btnSave.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnSave.Location = new Point(505, 18);
        btnSave.Size = new Size(90, 32);
        btnSave.Text = "Kaydet";
        btnSave.Click += btnSave_Click;

        pnlMain.AutoScroll = true;
        pnlMain.BackColor = Color.FromArgb(245, 247, 250);
        pnlMain.Controls.Add(pnlDynamicFields);
        pnlMain.Controls.Add(txtUyeAdiSoyadi);
        pnlMain.Controls.Add(lblUyeAdiSoyadi);
        pnlMain.Controls.Add(chkAktifMi);
        pnlMain.Dock = DockStyle.Fill;
        pnlMain.Location = new Point(0, 65);
        pnlMain.Size = new Size(620, 455);

        chkAktifMi.AutoSize = true;
        chkAktifMi.Checked = true;
        chkAktifMi.CheckState = CheckState.Checked;
        chkAktifMi.Location = new Point(195, 62);
        chkAktifMi.Name = "chkAktifMi";
        chkAktifMi.Size = new Size(63, 19);
        chkAktifMi.TabIndex = 2;
        chkAktifMi.Text = "Aktif";
        chkAktifMi.UseVisualStyleBackColor = true;

        lblUyeAdiSoyadi.Location = new Point(35, 30);
        lblUyeAdiSoyadi.Size = new Size(150, 25);
        lblUyeAdiSoyadi.Text = "Üye Adı Soyadı *";

        txtUyeAdiSoyadi.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        txtUyeAdiSoyadi.Location = new Point(195, 27);
        txtUyeAdiSoyadi.MaxLength = 200;
        txtUyeAdiSoyadi.PlaceholderText = "Örn: Ahmet Yılmaz";
        txtUyeAdiSoyadi.Size = new Size(330, 23);

        pnlDynamicFields.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        pnlDynamicFields.AutoScroll = true;
        pnlDynamicFields.BackColor = Color.White;
        pnlDynamicFields.BorderStyle = BorderStyle.FixedSingle;
        pnlDynamicFields.Location = new Point(20, 105);
        pnlDynamicFields.Size = new Size(575, 320);

        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(620, 590);
        Controls.Add(pnlMain);
        Controls.Add(pnlFooter);
        Controls.Add(pnlHeader);
        MinimumSize = new Size(600, 500);
        Name = "FrmMemberEdit";
        StartPosition = FormStartPosition.CenterParent;
        Text = "Üye";
        Load += FrmMemberEdit_Load;

        pnlHeader.ResumeLayout(false);
        pnlHeader.PerformLayout();
        pnlFooter.ResumeLayout(false);
        pnlMain.ResumeLayout(false);
        pnlMain.PerformLayout();
        ResumeLayout(false);
    }
}