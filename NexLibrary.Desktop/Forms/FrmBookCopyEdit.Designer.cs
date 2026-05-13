namespace NexLibrary.Desktop.Forms;

partial class FrmBookCopyEdit
{
    private System.ComponentModel.IContainer components = null;
    private Panel pnlHeader;
    private Panel pnlFooter;
    private Panel pnlMain;
    private Label lblTitle;
    private Label lblKitapAdi;
    private TextBox txtKitapAdi;
    private Label lblBarkod;
    private TextBox txtBarkod;
    private Label lblDemirbasNo;
    private TextBox txtDemirbasNo;
    private Label lblAciklama;
    private TextBox txtAciklama;
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
        lblKitapAdi = new Label();
        txtKitapAdi = new TextBox();
        lblBarkod = new Label();
        txtBarkod = new TextBox();
        lblDemirbasNo = new Label();
        txtDemirbasNo = new TextBox();
        lblAciklama = new Label();
        txtAciklama = new TextBox();

        pnlHeader.SuspendLayout();
        pnlFooter.SuspendLayout();
        pnlMain.SuspendLayout();
        SuspendLayout();

        pnlHeader.BackColor = Color.White;
        pnlHeader.Controls.Add(lblTitle);
        pnlHeader.Dock = DockStyle.Top;
        pnlHeader.Location = new Point(0, 0);
        pnlHeader.Name = "pnlHeader";
        pnlHeader.Size = new Size(600, 65);
        pnlHeader.TabIndex = 0;

        lblTitle.AutoSize = true;
        lblTitle.Font = new Font("Segoe UI", 15F, FontStyle.Bold);
        lblTitle.Location = new Point(20, 18);
        lblTitle.Name = "lblTitle";
        lblTitle.Size = new Size(174, 28);
        lblTitle.TabIndex = 0;
        lblTitle.Text = "Yeni Kitap Kopyası";

        pnlFooter.BackColor = Color.White;
        pnlFooter.Controls.Add(btnCancel);
        pnlFooter.Controls.Add(btnSave);
        pnlFooter.Dock = DockStyle.Bottom;
        pnlFooter.Location = new Point(0, 350);
        pnlFooter.Name = "pnlFooter";
        pnlFooter.Size = new Size(600, 70);
        pnlFooter.TabIndex = 2;

        btnCancel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnCancel.Location = new Point(385, 18);
        btnCancel.Name = "btnCancel";
        btnCancel.Size = new Size(90, 32);
        btnCancel.TabIndex = 1;
        btnCancel.Text = "İptal";
        btnCancel.UseVisualStyleBackColor = true;
        btnCancel.Click += btnCancel_Click;

        btnSave.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnSave.Location = new Point(485, 18);
        btnSave.Name = "btnSave";
        btnSave.Size = new Size(90, 32);
        btnSave.TabIndex = 0;
        btnSave.Text = "Kaydet";
        btnSave.UseVisualStyleBackColor = true;
        btnSave.Click += btnSave_Click;

        pnlMain.BackColor = Color.FromArgb(245, 247, 250);
        pnlMain.Controls.Add(lblKitapAdi);
        pnlMain.Controls.Add(txtKitapAdi);
        pnlMain.Controls.Add(lblBarkod);
        pnlMain.Controls.Add(txtBarkod);
        pnlMain.Controls.Add(lblDemirbasNo);
        pnlMain.Controls.Add(txtDemirbasNo);
        pnlMain.Controls.Add(lblAciklama);
        pnlMain.Controls.Add(txtAciklama);
        pnlMain.Dock = DockStyle.Fill;
        pnlMain.Location = new Point(0, 65);
        pnlMain.Name = "pnlMain";
        pnlMain.Size = new Size(600, 285);
        pnlMain.TabIndex = 1;

        lblKitapAdi.Location = new Point(35, 35);
        lblKitapAdi.Name = "lblKitapAdi";
        lblKitapAdi.Size = new Size(140, 23);
        lblKitapAdi.TabIndex = 0;
        lblKitapAdi.Text = "Kitap";

        txtKitapAdi.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        txtKitapAdi.Location = new Point(190, 32);
        txtKitapAdi.Name = "txtKitapAdi";
        txtKitapAdi.ReadOnly = true;
        txtKitapAdi.Size = new Size(340, 23);
        txtKitapAdi.TabIndex = 1;

        lblBarkod.Location = new Point(35, 75);
        lblBarkod.Name = "lblBarkod";
        lblBarkod.Size = new Size(140, 23);
        lblBarkod.TabIndex = 2;
        lblBarkod.Text = "Barkod *";

        txtBarkod.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        txtBarkod.Location = new Point(190, 72);
        txtBarkod.Name = "txtBarkod";
        txtBarkod.Size = new Size(340, 23);
        txtBarkod.TabIndex = 3;

        lblDemirbasNo.Location = new Point(35, 115);
        lblDemirbasNo.Name = "lblDemirbasNo";
        lblDemirbasNo.Size = new Size(140, 23);
        lblDemirbasNo.TabIndex = 4;
        lblDemirbasNo.Text = "Demirbaş No";

        txtDemirbasNo.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        txtDemirbasNo.Location = new Point(190, 112);
        txtDemirbasNo.Name = "txtDemirbasNo";
        txtDemirbasNo.Size = new Size(340, 23);
        txtDemirbasNo.TabIndex = 5;

        lblAciklama.Location = new Point(35, 155);
        lblAciklama.Name = "lblAciklama";
        lblAciklama.Size = new Size(140, 23);
        lblAciklama.TabIndex = 6;
        lblAciklama.Text = "Açıklama";

        txtAciklama.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        txtAciklama.Location = new Point(190, 152);
        txtAciklama.Multiline = true;
        txtAciklama.Name = "txtAciklama";
        txtAciklama.ScrollBars = ScrollBars.Vertical;
        txtAciklama.Size = new Size(340, 90);
        txtAciklama.TabIndex = 7;

        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(600, 420);
        Controls.Add(pnlMain);
        Controls.Add(pnlFooter);
        Controls.Add(pnlHeader);
        MinimumSize = new Size(600, 420);
        Name = "FrmBookCopyEdit";
        StartPosition = FormStartPosition.CenterParent;
        Text = "Yeni Kitap Kopyası";
        Load += FrmBookCopyEdit_Load;

        pnlHeader.ResumeLayout(false);
        pnlHeader.PerformLayout();
        pnlFooter.ResumeLayout(false);
        pnlMain.ResumeLayout(false);
        pnlMain.PerformLayout();
        ResumeLayout(false);
    }
}