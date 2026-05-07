namespace NexLibrary.Desktop.Forms;

partial class FrmBookEdit
{
    private System.ComponentModel.IContainer components = null;
    private Panel pnlHeader;
    private Panel pnlFooter;
    private Panel pnlMain;
    private Label lblTitle;
    private Label lblKitapAdi;
    private TextBox txtKitapAdi;
    private CheckBox chkAktifMi;
    private Panel pnlDynamicFields;
    private Button btnSave;
    private Button btnCancel;

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
        chkAktifMi = new CheckBox();
        txtKitapAdi = new TextBox();
        lblKitapAdi = new Label();

        pnlHeader.SuspendLayout();
        pnlFooter.SuspendLayout();
        pnlMain.SuspendLayout();
        SuspendLayout();

        pnlHeader.BackColor = Color.White;
        pnlHeader.Controls.Add(lblTitle);
        pnlHeader.Dock = DockStyle.Top;
        pnlHeader.Location = new Point(0, 0);
        pnlHeader.Name = "pnlHeader";
        pnlHeader.Size = new Size(620, 65);
        pnlHeader.TabIndex = 0;

        lblTitle.AutoSize = true;
        lblTitle.Font = new Font("Segoe UI", 15F, FontStyle.Bold);
        lblTitle.Location = new Point(20, 18);
        lblTitle.Name = "lblTitle";
        lblTitle.Size = new Size(104, 28);
        lblTitle.TabIndex = 0;
        lblTitle.Text = "Yeni Kitap";

        pnlFooter.BackColor = Color.White;
        pnlFooter.Controls.Add(btnCancel);
        pnlFooter.Controls.Add(btnSave);
        pnlFooter.Dock = DockStyle.Bottom;
        pnlFooter.Location = new Point(0, 520);
        pnlFooter.Name = "pnlFooter";
        pnlFooter.Size = new Size(620, 70);
        pnlFooter.TabIndex = 2;

        btnCancel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnCancel.Location = new Point(405, 18);
        btnCancel.Name = "btnCancel";
        btnCancel.Size = new Size(90, 32);
        btnCancel.TabIndex = 1;
        btnCancel.Text = "İptal";
        btnCancel.UseVisualStyleBackColor = true;
        btnCancel.Click += btnCancel_Click;

        btnSave.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnSave.Location = new Point(505, 18);
        btnSave.Name = "btnSave";
        btnSave.Size = new Size(90, 32);
        btnSave.TabIndex = 0;
        btnSave.Text = "Kaydet";
        btnSave.UseVisualStyleBackColor = true;
        btnSave.Click += btnSave_Click;

        pnlMain.AutoScroll = true;
        pnlMain.BackColor = Color.FromArgb(245, 247, 250);
        pnlMain.Controls.Add(pnlDynamicFields);
        pnlMain.Controls.Add(chkAktifMi);
        pnlMain.Controls.Add(txtKitapAdi);
        pnlMain.Controls.Add(lblKitapAdi);
        pnlMain.Dock = DockStyle.Fill;
        pnlMain.Location = new Point(0, 65);
        pnlMain.Name = "pnlMain";
        pnlMain.Padding = new Padding(20);
        pnlMain.Size = new Size(620, 455);
        pnlMain.TabIndex = 1;

        lblKitapAdi.Location = new Point(35, 30);
        lblKitapAdi.Name = "lblKitapAdi";
        lblKitapAdi.Size = new Size(150, 25);
        lblKitapAdi.TabIndex = 0;
        lblKitapAdi.Text = "Kitap Adı *";

        txtKitapAdi.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        txtKitapAdi.Location = new Point(195, 27);
        txtKitapAdi.MaxLength = 200;
        txtKitapAdi.Name = "txtKitapAdi";
        txtKitapAdi.PlaceholderText = "Örn: Nutuk";
        txtKitapAdi.Size = new Size(330, 23);
        txtKitapAdi.TabIndex = 1;

        chkAktifMi.AutoSize = true;
        chkAktifMi.Checked = true;
        chkAktifMi.CheckState = CheckState.Checked;
        chkAktifMi.Location = new Point(195, 65);
        chkAktifMi.Name = "chkAktifMi";
        chkAktifMi.Size = new Size(63, 19);
        chkAktifMi.TabIndex = 2;
        chkAktifMi.Text = "Aktif";
        chkAktifMi.UseVisualStyleBackColor = true;

        pnlDynamicFields.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        pnlDynamicFields.AutoScroll = true;
        pnlDynamicFields.BackColor = Color.White;
        pnlDynamicFields.BorderStyle = BorderStyle.FixedSingle;
        pnlDynamicFields.Location = new Point(20, 105);
        pnlDynamicFields.Name = "pnlDynamicFields";
        pnlDynamicFields.Size = new Size(575, 320);
        pnlDynamicFields.TabIndex = 3;

        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(620, 590);
        Controls.Add(pnlMain);
        Controls.Add(pnlFooter);
        Controls.Add(pnlHeader);
        MinimumSize = new Size(600, 500);
        Name = "FrmBookEdit";
        StartPosition = FormStartPosition.CenterParent;
        Text = "Kitap";
        Load += FrmBookEdit_Load;

        pnlHeader.ResumeLayout(false);
        pnlHeader.PerformLayout();
        pnlFooter.ResumeLayout(false);
        pnlMain.ResumeLayout(false);
        pnlMain.PerformLayout();
        ResumeLayout(false);
    }
}