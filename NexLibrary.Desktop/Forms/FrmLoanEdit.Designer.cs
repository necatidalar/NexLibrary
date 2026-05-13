namespace NexLibrary.Desktop.Forms;

partial class FrmLoanEdit
{
    private System.ComponentModel.IContainer components = null;
    private Panel pnlHeader;
    private Panel pnlFooter;
    private Panel pnlMain;
    private Label lblTitle;
    private Label lblBook;
    private ComboBox cmbBooks;
    private Label lblMember;
    private ComboBox cmbMembers;
    private Label lblPlanlananIadeTarihi;
    private DateTimePicker dtpPlanlananIadeTarihi;
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
        lblBook = new Label();
        cmbBooks = new ComboBox();
        lblMember = new Label();
        cmbMembers = new ComboBox();
        lblPlanlananIadeTarihi = new Label();
        dtpPlanlananIadeTarihi = new DateTimePicker();
        lblAciklama = new Label();
        txtAciklama = new TextBox();

        pnlHeader.SuspendLayout();
        pnlFooter.SuspendLayout();
        pnlMain.SuspendLayout();
        SuspendLayout();

        pnlHeader.BackColor = Color.White;
        pnlHeader.Controls.Add(lblTitle);
        pnlHeader.Dock = DockStyle.Top;
        pnlHeader.Size = new Size(600, 65);

        lblTitle.AutoSize = true;
        lblTitle.Font = new Font("Segoe UI", 15F, FontStyle.Bold);
        lblTitle.Location = new Point(20, 18);
        lblTitle.Text = "Ödünç Ver";

        pnlFooter.BackColor = Color.White;
        pnlFooter.Controls.Add(btnCancel);
        pnlFooter.Controls.Add(btnSave);
        pnlFooter.Dock = DockStyle.Bottom;
        pnlFooter.Location = new Point(0, 360);
        pnlFooter.Size = new Size(600, 70);

        btnCancel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnCancel.Location = new Point(385, 18);
        btnCancel.Size = new Size(90, 32);
        btnCancel.Text = "İptal";
        btnCancel.Click += btnCancel_Click;

        btnSave.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnSave.Location = new Point(485, 18);
        btnSave.Size = new Size(90, 32);
        btnSave.Text = "Kaydet";
        btnSave.Click += btnSave_Click;

        pnlMain.BackColor = Color.FromArgb(245, 247, 250);
        pnlMain.Controls.Add(lblBook);
        pnlMain.Controls.Add(cmbBooks);
        pnlMain.Controls.Add(lblMember);
        pnlMain.Controls.Add(cmbMembers);
        pnlMain.Controls.Add(lblPlanlananIadeTarihi);
        pnlMain.Controls.Add(dtpPlanlananIadeTarihi);
        pnlMain.Controls.Add(lblAciklama);
        pnlMain.Controls.Add(txtAciklama);
        pnlMain.Dock = DockStyle.Fill;
        pnlMain.Location = new Point(0, 65);

        lblBook.Location = new Point(35, 35);
        lblBook.Size = new Size(150, 23);
        lblBook.Text = "Kitap *";

        cmbBooks.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbBooks.Location = new Point(195, 32);
        cmbBooks.Size = new Size(330, 23);

        lblMember.Location = new Point(35, 75);
        lblMember.Size = new Size(150, 23);
        lblMember.Text = "Üye *";

        cmbMembers.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbMembers.Location = new Point(195, 72);
        cmbMembers.Size = new Size(330, 23);

        lblPlanlananIadeTarihi.Location = new Point(35, 115);
        lblPlanlananIadeTarihi.Size = new Size(150, 23);
        lblPlanlananIadeTarihi.Text = "Planlanan İade *";

        dtpPlanlananIadeTarihi.Format = DateTimePickerFormat.Short;
        dtpPlanlananIadeTarihi.Location = new Point(195, 112);
        dtpPlanlananIadeTarihi.Size = new Size(160, 23);

        lblAciklama.Location = new Point(35, 155);
        lblAciklama.Size = new Size(150, 23);
        lblAciklama.Text = "Açıklama";

        txtAciklama.Location = new Point(195, 152);
        txtAciklama.Multiline = true;
        txtAciklama.ScrollBars = ScrollBars.Vertical;
        txtAciklama.Size = new Size(330, 110);

        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(600, 430);
        Controls.Add(pnlMain);
        Controls.Add(pnlFooter);
        Controls.Add(pnlHeader);
        MinimumSize = new Size(600, 430);
        Name = "FrmLoanEdit";
        StartPosition = FormStartPosition.CenterParent;
        Text = "Ödünç Ver";
        Load += FrmLoanEdit_Load;

        pnlHeader.ResumeLayout(false);
        pnlHeader.PerformLayout();
        pnlFooter.ResumeLayout(false);
        pnlMain.ResumeLayout(false);
        pnlMain.PerformLayout();
        ResumeLayout(false);
    }
}