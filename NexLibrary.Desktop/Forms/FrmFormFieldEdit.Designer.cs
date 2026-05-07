namespace NexLibrary.Desktop.Forms;

partial class FrmFormFieldEdit
{
    private System.ComponentModel.IContainer components = null;
    private Panel pnlHeader;
    private Panel pnlFooter;
    private Panel pnlMain;
    private Label lblTitle;
    private Button btnSave;
    private Button btnCancel;

    private Label lblModule;
    private TextBox txtModule;
    private Label lblAlanAdi;
    private TextBox txtAlanAdi;
    private Label lblAlanKodu;
    private TextBox txtAlanKodu;
    private Label lblAlanTipi;
    private ComboBox cmbAlanTipi;
    private Label lblMinKarakter;
    private NumericUpDown numMinKarakter;
    private Label lblMaxKarakter;
    private NumericUpDown numMaxKarakter;
    private Label lblSiraNo;
    private NumericUpDown numSiraNo;
    private Label lblVarsayilanDeger;
    private TextBox txtVarsayilanDeger;
    private Label lblPlaceholder;
    private TextBox txtPlaceholder;
    private Label lblAciklama;
    private TextBox txtAciklama;

    private CheckBox chkZorunluMu;
    private CheckBox chkBenzersizMi;
    private CheckBox chkFormdaGorunsunMu;
    private CheckBox chkListedeGorunsunMu;
    private CheckBox chkAramadaGorunsunMu;
    private CheckBox chkDetaydaGorunsunMu;
    private CheckBox chkHizliKayittaGorunsunMu;
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

        lblModule = new Label();
        txtModule = new TextBox();
        lblAlanAdi = new Label();
        txtAlanAdi = new TextBox();
        lblAlanKodu = new Label();
        txtAlanKodu = new TextBox();
        lblAlanTipi = new Label();
        cmbAlanTipi = new ComboBox();
        lblMinKarakter = new Label();
        numMinKarakter = new NumericUpDown();
        lblMaxKarakter = new Label();
        numMaxKarakter = new NumericUpDown();
        lblSiraNo = new Label();
        numSiraNo = new NumericUpDown();
        lblVarsayilanDeger = new Label();
        txtVarsayilanDeger = new TextBox();
        lblPlaceholder = new Label();
        txtPlaceholder = new TextBox();
        lblAciklama = new Label();
        txtAciklama = new TextBox();

        chkZorunluMu = new CheckBox();
        chkBenzersizMi = new CheckBox();
        chkFormdaGorunsunMu = new CheckBox();
        chkListedeGorunsunMu = new CheckBox();
        chkAramadaGorunsunMu = new CheckBox();
        chkDetaydaGorunsunMu = new CheckBox();
        chkHizliKayittaGorunsunMu = new CheckBox();
        chkAktifMi = new CheckBox();

        pnlHeader.SuspendLayout();
        pnlFooter.SuspendLayout();
        pnlMain.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)numMinKarakter).BeginInit();
        ((System.ComponentModel.ISupportInitialize)numMaxKarakter).BeginInit();
        ((System.ComponentModel.ISupportInitialize)numSiraNo).BeginInit();
        SuspendLayout();

        pnlHeader.BackColor = Color.White;
        pnlHeader.Controls.Add(lblTitle);
        pnlHeader.Dock = DockStyle.Top;
        pnlHeader.Location = new Point(0, 0);
        pnlHeader.Name = "pnlHeader";
        pnlHeader.Size = new Size(720, 65);
        pnlHeader.TabIndex = 0;

        lblTitle.AutoSize = true;
        lblTitle.Font = new Font("Segoe UI", 15F, FontStyle.Bold);
        lblTitle.Location = new Point(20, 18);
        lblTitle.Name = "lblTitle";
        lblTitle.Size = new Size(159, 28);
        lblTitle.TabIndex = 0;
        lblTitle.Text = "Yeni Form Alanı";

        pnlFooter.BackColor = Color.White;
        pnlFooter.Controls.Add(btnCancel);
        pnlFooter.Controls.Add(btnSave);
        pnlFooter.Dock = DockStyle.Bottom;
        pnlFooter.Location = new Point(0, 560);
        pnlFooter.Name = "pnlFooter";
        pnlFooter.Size = new Size(720, 70);
        pnlFooter.TabIndex = 2;

        btnCancel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnCancel.Location = new Point(505, 18);
        btnCancel.Name = "btnCancel";
        btnCancel.Size = new Size(90, 32);
        btnCancel.TabIndex = 1;
        btnCancel.Text = "İptal";
        btnCancel.UseVisualStyleBackColor = true;
        btnCancel.Click += btnCancel_Click;

        btnSave.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnSave.Location = new Point(605, 18);
        btnSave.Name = "btnSave";
        btnSave.Size = new Size(90, 32);
        btnSave.TabIndex = 0;
        btnSave.Text = "Kaydet";
        btnSave.UseVisualStyleBackColor = true;
        btnSave.Click += btnSave_Click;

        pnlMain.AutoScroll = true;
        pnlMain.BackColor = Color.FromArgb(245, 247, 250);
        pnlMain.Controls.Add(lblModule);
        pnlMain.Controls.Add(txtModule);
        pnlMain.Controls.Add(lblAlanAdi);
        pnlMain.Controls.Add(txtAlanAdi);
        pnlMain.Controls.Add(lblAlanKodu);
        pnlMain.Controls.Add(txtAlanKodu);
        pnlMain.Controls.Add(lblAlanTipi);
        pnlMain.Controls.Add(cmbAlanTipi);
        pnlMain.Controls.Add(lblMinKarakter);
        pnlMain.Controls.Add(numMinKarakter);
        pnlMain.Controls.Add(lblMaxKarakter);
        pnlMain.Controls.Add(numMaxKarakter);
        pnlMain.Controls.Add(lblSiraNo);
        pnlMain.Controls.Add(numSiraNo);
        pnlMain.Controls.Add(lblVarsayilanDeger);
        pnlMain.Controls.Add(txtVarsayilanDeger);
        pnlMain.Controls.Add(lblPlaceholder);
        pnlMain.Controls.Add(txtPlaceholder);
        pnlMain.Controls.Add(lblAciklama);
        pnlMain.Controls.Add(txtAciklama);

        pnlMain.Controls.Add(chkZorunluMu);
        pnlMain.Controls.Add(chkBenzersizMi);
        pnlMain.Controls.Add(chkFormdaGorunsunMu);
        pnlMain.Controls.Add(chkListedeGorunsunMu);
        pnlMain.Controls.Add(chkAramadaGorunsunMu);
        pnlMain.Controls.Add(chkDetaydaGorunsunMu);
        pnlMain.Controls.Add(chkHizliKayittaGorunsunMu);
        pnlMain.Controls.Add(chkAktifMi);

        pnlMain.Dock = DockStyle.Fill;
        pnlMain.Location = new Point(0, 65);
        pnlMain.Name = "pnlMain";
        pnlMain.Padding = new Padding(25);
        pnlMain.Size = new Size(720, 495);
        pnlMain.TabIndex = 1;

        lblModule.Location = new Point(30, 30);
        lblModule.Size = new Size(150, 23);
        lblModule.Text = "Modül";

        txtModule.Location = new Point(190, 27);
        txtModule.ReadOnly = true;
        txtModule.Size = new Size(440, 23);

        lblAlanAdi.Location = new Point(30, 65);
        lblAlanAdi.Size = new Size(150, 23);
        lblAlanAdi.Text = "Alan Adı *";

        txtAlanAdi.Location = new Point(190, 62);
        txtAlanAdi.Size = new Size(440, 23);

        lblAlanKodu.Location = new Point(30, 100);
        lblAlanKodu.Size = new Size(150, 23);
        lblAlanKodu.Text = "Alan Kodu *";

        txtAlanKodu.Location = new Point(190, 97);
        txtAlanKodu.Size = new Size(440, 23);
        txtAlanKodu.PlaceholderText = "Örn: RAF_NO";

        lblAlanTipi.Location = new Point(30, 135);
        lblAlanTipi.Size = new Size(150, 23);
        lblAlanTipi.Text = "Alan Tipi *";

        cmbAlanTipi.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbAlanTipi.Location = new Point(190, 132);
        cmbAlanTipi.Size = new Size(220, 23);

        lblMinKarakter.Location = new Point(30, 170);
        lblMinKarakter.Size = new Size(150, 23);
        lblMinKarakter.Text = "Minimum Karakter";

        numMinKarakter.Location = new Point(190, 167);
        numMinKarakter.Maximum = 10000;
        numMinKarakter.Size = new Size(120, 23);

        lblMaxKarakter.Location = new Point(330, 170);
        lblMaxKarakter.Size = new Size(130, 23);
        lblMaxKarakter.Text = "Maksimum Karakter";

        numMaxKarakter.Location = new Point(510, 167);
        numMaxKarakter.Maximum = 10000;
        numMaxKarakter.Size = new Size(120, 23);

        lblSiraNo.Location = new Point(30, 205);
        lblSiraNo.Size = new Size(150, 23);
        lblSiraNo.Text = "Sıra No";

        numSiraNo.Location = new Point(190, 202);
        numSiraNo.Maximum = 10000;
        numSiraNo.Minimum = 1;
        numSiraNo.Size = new Size(120, 23);
        numSiraNo.Value = 1;

        lblVarsayilanDeger.Location = new Point(30, 240);
        lblVarsayilanDeger.Size = new Size(150, 23);
        lblVarsayilanDeger.Text = "Varsayılan Değer";

        txtVarsayilanDeger.Location = new Point(190, 237);
        txtVarsayilanDeger.Size = new Size(440, 23);

        lblPlaceholder.Location = new Point(30, 275);
        lblPlaceholder.Size = new Size(150, 23);
        lblPlaceholder.Text = "Placeholder";

        txtPlaceholder.Location = new Point(190, 272);
        txtPlaceholder.Size = new Size(440, 23);

        lblAciklama.Location = new Point(30, 310);
        lblAciklama.Size = new Size(150, 23);
        lblAciklama.Text = "Açıklama";

        txtAciklama.Location = new Point(190, 307);
        txtAciklama.Multiline = true;
        txtAciklama.ScrollBars = ScrollBars.Vertical;
        txtAciklama.Size = new Size(440, 65);

        chkZorunluMu.Location = new Point(190, 390);
        chkZorunluMu.Size = new Size(120, 24);
        chkZorunluMu.Text = "Zorunlu";

        chkBenzersizMi.Location = new Point(330, 390);
        chkBenzersizMi.Size = new Size(120, 24);
        chkBenzersizMi.Text = "Benzersiz";

        chkAktifMi.Location = new Point(470, 390);
        chkAktifMi.Size = new Size(120, 24);
        chkAktifMi.Text = "Aktif";

        chkFormdaGorunsunMu.Location = new Point(190, 420);
        chkFormdaGorunsunMu.Size = new Size(150, 24);
        chkFormdaGorunsunMu.Text = "Formda görünsün";

        chkListedeGorunsunMu.Location = new Point(360, 420);
        chkListedeGorunsunMu.Size = new Size(150, 24);
        chkListedeGorunsunMu.Text = "Listede görünsün";

        chkAramadaGorunsunMu.Location = new Point(530, 420);
        chkAramadaGorunsunMu.Size = new Size(160, 24);
        chkAramadaGorunsunMu.Text = "Aramada kullanılsın";

        chkDetaydaGorunsunMu.Location = new Point(190, 450);
        chkDetaydaGorunsunMu.Size = new Size(150, 24);
        chkDetaydaGorunsunMu.Text = "Detayda görünsün";

        chkHizliKayittaGorunsunMu.Location = new Point(360, 450);
        chkHizliKayittaGorunsunMu.Size = new Size(170, 24);
        chkHizliKayittaGorunsunMu.Text = "Hızlı kayıtta görünsün";

        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(720, 630);
        Controls.Add(pnlMain);
        Controls.Add(pnlFooter);
        Controls.Add(pnlHeader);
        MinimumSize = new Size(720, 630);
        Name = "FrmFormFieldEdit";
        StartPosition = FormStartPosition.CenterParent;
        Text = "Form Alanı";
        Load += FrmFormFieldEdit_Load;

        pnlHeader.ResumeLayout(false);
        pnlHeader.PerformLayout();
        pnlFooter.ResumeLayout(false);
        pnlMain.ResumeLayout(false);
        pnlMain.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)numMinKarakter).EndInit();
        ((System.ComponentModel.ISupportInitialize)numMaxKarakter).EndInit();
        ((System.ComponentModel.ISupportInitialize)numSiraNo).EndInit();
        ResumeLayout(false);
    }
}