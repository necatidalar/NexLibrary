using NexLibrary.Contracts.Books;
using NexLibrary.Contracts.Loans;
using NexLibrary.Contracts.Members;
using NexLibrary.Desktop.Services;

namespace NexLibrary.Desktop.Forms;

public partial class FrmLoanEdit : Form
{
    private readonly LoanApiService _loanApiService;
    private readonly BookApiService _bookApiService;
    private readonly MemberApiService _memberApiService;

    private List<BookListResponse> _books = new();
    private List<MemberListResponse> _members = new();

    public FrmLoanEdit(
        LoanApiService loanApiService,
        BookApiService bookApiService,
        MemberApiService memberApiService)
    {
        InitializeComponent();

        _loanApiService = loanApiService;
        _bookApiService = bookApiService;
        _memberApiService = memberApiService;
    }

    private async void FrmLoanEdit_Load(object sender, EventArgs e)
    {
        dtpPlanlananIadeTarihi.Value = DateTime.Today.AddDays(14);

        await LoadBooksAsync();
        await LoadMembersAsync();
    }

    private async Task LoadBooksAsync()
    {
        var result = await _bookApiService.GetPagedAsync(1, 500);

        _books = result?.Items
            .Where(x => x.AktifMi)
            .OrderBy(x => x.KitapAdi)
            .ToList() ?? new List<BookListResponse>();

        cmbBooks.DataSource = _books;
        cmbBooks.DisplayMember = nameof(BookListResponse.KitapAdi);
        cmbBooks.ValueMember = nameof(BookListResponse.Id);
    }

    private async Task LoadMembersAsync()
    {
        var result = await _memberApiService.GetPagedAsync(1, 500);

        _members = result?.Items
            .Where(x => x.AktifMi)
            .OrderBy(x => x.UyeAdiSoyadi)
            .ToList() ?? new List<MemberListResponse>();

        cmbMembers.DataSource = _members;
        cmbMembers.DisplayMember = nameof(MemberListResponse.UyeAdiSoyadi);
        cmbMembers.ValueMember = nameof(MemberListResponse.Id);
    }

    private async void btnSave_Click(object sender, EventArgs e)
    {
        if (cmbBooks.SelectedValue is null)
        {
            MessageBox.Show("Kitap seçilmelidir.");
            return;
        }

        if (cmbMembers.SelectedValue is null)
        {
            MessageBox.Show("Üye seçilmelidir.");
            return;
        }

        var request = new LoanCreateRequest
        {
            KitapId = Convert.ToInt32(cmbBooks.SelectedValue),
            UyeId = Convert.ToInt32(cmbMembers.SelectedValue),
            PlanlananIadeTarihi = dtpPlanlananIadeTarihi.Value.Date,
            Aciklama = string.IsNullOrWhiteSpace(txtAciklama.Text)
                ? null
                : txtAciklama.Text.Trim()
        };

        var result = await _loanApiService.CreateAsync(request);

        if (result is null)
        {
            MessageBox.Show(
                "Ödünç verme işlemi başarısız. Kitap zaten ödünçte olabilir.",
                "NexLibrary",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
            return;
        }

        DialogResult = DialogResult.OK;
        Close();
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }
}