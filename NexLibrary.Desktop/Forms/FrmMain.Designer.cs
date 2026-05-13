namespace NexLibrary.Desktop.Forms;

partial class FrmMain
{
    private System.ComponentModel.IContainer components = null;
    private Panel pnlSidebar;
    private Panel pnlHeader;
    private Panel pnlContent;
    private Label lblTitle;
    private Label lblStatus;
    private Button btnBooks;
    private Button btnFormFields;
    private Button btnListSettings;
    private Button btnMembers;
    private Button btnLoans;
    private Button btnBookCopies;

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
        pnlSidebar = new Panel();
        btnBooks = new Button();
        btnFormFields = new Button();
        btnListSettings = new Button();
        btnMembers = new Button();
        btnLoans = new Button();
        btnBookCopies = new Button();
        pnlHeader = new Panel();
        lblTitle = new Label();
        lblStatus = new Label();
        pnlContent = new Panel();

        pnlSidebar.SuspendLayout();
        pnlHeader.SuspendLayout();
        SuspendLayout();

        pnlSidebar.BackColor = Color.FromArgb(35, 45, 60);
        pnlSidebar.Controls.Add(btnBooks);
        pnlSidebar.Controls.Add(btnFormFields);
        pnlSidebar.Controls.Add(btnListSettings);
        pnlSidebar.Controls.Add(btnMembers);
        pnlSidebar.Controls.Add(btnLoans);
        pnlSidebar.Controls.Add(btnBookCopies);
        pnlSidebar.Dock = DockStyle.Left;
        pnlSidebar.Location = new Point(0, 0);
        pnlSidebar.Name = "pnlSidebar";
        pnlSidebar.Size = new Size(220, 700);
        pnlSidebar.TabIndex = 0;

        btnBookCopies.BackColor = Color.FromArgb(52, 73, 94);
        btnBookCopies.FlatAppearance.BorderSize = 0;
        btnBookCopies.FlatStyle = FlatStyle.Flat;
        btnBookCopies.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        btnBookCopies.ForeColor = Color.White;
        btnBookCopies.Location = new Point(15, 355);
        btnBookCopies.Name = "btnBookCopies";
        btnBookCopies.Size = new Size(190, 45);
        btnBookCopies.TabIndex = 5;
        btnBookCopies.Text = "Kitap Kopyaları";
        btnBookCopies.UseVisualStyleBackColor = false;
        btnBookCopies.Click += btnBookCopies_Click;

        btnBooks.BackColor = Color.FromArgb(52, 73, 94);
        btnBooks.FlatAppearance.BorderSize = 0;
        btnBooks.FlatStyle = FlatStyle.Flat;
        btnBooks.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        btnBooks.ForeColor = Color.White;
        btnBooks.Location = new Point(15, 80);
        btnBooks.Name = "btnBooks";
        btnBooks.Size = new Size(190, 45);
        btnBooks.TabIndex = 0;
        btnBooks.Text = "Kitaplar";
        btnBooks.UseVisualStyleBackColor = false;
        btnBooks.Click += btnBooks_Click;

        btnMembers.BackColor = Color.FromArgb(52, 73, 94);
        btnMembers.FlatAppearance.BorderSize = 0;
        btnMembers.FlatStyle = FlatStyle.Flat;
        btnMembers.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        btnMembers.ForeColor = Color.White;
        btnMembers.Location = new Point(15, 245);
        btnMembers.Name = "btnMembers";
        btnMembers.Size = new Size(190, 45);
        btnMembers.TabIndex = 3;
        btnMembers.Text = "Üyeler";
        btnMembers.UseVisualStyleBackColor = false;
        btnMembers.Click += btnMembers_Click;

        btnFormFields.BackColor = Color.FromArgb(52, 73, 94);
        btnFormFields.FlatAppearance.BorderSize = 0;
        btnFormFields.FlatStyle = FlatStyle.Flat;
        btnFormFields.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        btnFormFields.ForeColor = Color.White;
        btnFormFields.Location = new Point(15, 135);
        btnFormFields.Name = "btnFormFields";
        btnFormFields.Size = new Size(190, 45);
        btnFormFields.TabIndex = 1;
        btnFormFields.Text = "Form Alanları";
        btnFormFields.UseVisualStyleBackColor = false;
        btnFormFields.Click += btnFormFields_Click;

        btnListSettings.BackColor = Color.FromArgb(52, 73, 94);
        btnListSettings.FlatAppearance.BorderSize = 0;
        btnListSettings.FlatStyle = FlatStyle.Flat;
        btnListSettings.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        btnListSettings.ForeColor = Color.White;
        btnListSettings.Location = new Point(15, 190);
        btnListSettings.Name = "btnListSettings";
        btnListSettings.Size = new Size(190, 45);
        btnListSettings.TabIndex = 2;
        btnListSettings.Text = "Liste Ayarları";
        btnListSettings.UseVisualStyleBackColor = false;
        btnListSettings.Click += btnListSettings_Click;

        btnLoans.BackColor = Color.FromArgb(52, 73, 94);
        btnLoans.FlatAppearance.BorderSize = 0;
        btnLoans.FlatStyle = FlatStyle.Flat;
        btnLoans.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        btnLoans.ForeColor = Color.White;
        btnLoans.Location = new Point(15, 300);
        btnLoans.Name = "btnLoans";
        btnLoans.Size = new Size(190, 45);
        btnLoans.TabIndex = 4;
        btnLoans.Text = "Ödünç İşlemleri";
        btnLoans.UseVisualStyleBackColor = false;
        btnLoans.Click += btnLoans_Click;

        pnlHeader.BackColor = Color.White;
        pnlHeader.Controls.Add(lblStatus);
        pnlHeader.Controls.Add(lblTitle);
        pnlHeader.Dock = DockStyle.Top;
        pnlHeader.Location = new Point(220, 0);
        pnlHeader.Name = "pnlHeader";
        pnlHeader.Size = new Size(980, 70);
        pnlHeader.TabIndex = 1;

        lblTitle.AutoSize = true;
        lblTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
        lblTitle.Location = new Point(20, 17);
        lblTitle.Name = "lblTitle";
        lblTitle.Size = new Size(246, 30);
        lblTitle.TabIndex = 0;
        lblTitle.Text = "NexLibrary Yönetim";

        lblStatus.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        lblStatus.Font = new Font("Segoe UI", 9F);
        lblStatus.ForeColor = Color.DimGray;
        lblStatus.Location = new Point(650, 25);
        lblStatus.Name = "lblStatus";
        lblStatus.Size = new Size(300, 20);
        lblStatus.TabIndex = 1;
        lblStatus.Text = "Hazır";
        lblStatus.TextAlign = ContentAlignment.MiddleRight;

        pnlContent.BackColor = Color.FromArgb(245, 247, 250);
        pnlContent.Dock = DockStyle.Fill;
        pnlContent.Location = new Point(220, 70);
        pnlContent.Name = "pnlContent";
        pnlContent.Size = new Size(980, 630);
        pnlContent.TabIndex = 2;

        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1200, 700);
        Controls.Add(pnlContent);
        Controls.Add(pnlHeader);
        Controls.Add(pnlSidebar);
        MinimumSize = new Size(1000, 600);
        Name = "FrmMain";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "NexLibrary";
        WindowState = FormWindowState.Maximized;
        Load += FrmMain_Load;

        pnlSidebar.ResumeLayout(false);
        pnlHeader.ResumeLayout(false);
        pnlHeader.PerformLayout();
        ResumeLayout(false);
    }
}