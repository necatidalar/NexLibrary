using Microsoft.Extensions.DependencyInjection;

namespace NexLibrary.Desktop.Forms;

public partial class FrmMain : Form
{
    private readonly IServiceProvider _serviceProvider;
    private Form? _activeForm;

    public FrmMain(IServiceProvider serviceProvider)
    {
        InitializeComponent();

        _serviceProvider = serviceProvider;
    }

    private void FrmMain_Load(object sender, EventArgs e)
    {
        lblStatus.Text = "NexLibrary hazır.";
    }

    private void btnMembers_Click(object sender, EventArgs e)
    {
        var form = _serviceProvider.GetRequiredService<FrmMembers>();
        OpenChildForm(form);
    }

    private void btnBooks_Click(object sender, EventArgs e)
    {
        var form = _serviceProvider.GetRequiredService<FrmBooks>();
        OpenChildForm(form);
    }

    private void btnBookCopies_Click(object sender, EventArgs e)
    {
        var form = _serviceProvider.GetRequiredService<FrmBookCopies>();
        OpenChildForm(form);
    }

    private void btnFormFields_Click(object sender, EventArgs e)
    {
        var form = _serviceProvider.GetRequiredService<FrmFormFields>();
        OpenChildForm(form);
    }

    private void btnListSettings_Click(object sender, EventArgs e)
    {
        var form = _serviceProvider.GetRequiredService<FrmListSettings>();
        OpenChildForm(form);
    }

    private void btnLoans_Click(object sender, EventArgs e)
    {
        var form = _serviceProvider.GetRequiredService<FrmLoans>();
        OpenChildForm(form);
    }

    private void OpenChildForm(Form childForm)
    {
        _activeForm?.Close();

        _activeForm = childForm;
        childForm.TopLevel = false;
        childForm.FormBorderStyle = FormBorderStyle.None;
        childForm.Dock = DockStyle.Fill;

        pnlContent.Controls.Clear();
        pnlContent.Controls.Add(childForm);

        childForm.Show();
    }
}