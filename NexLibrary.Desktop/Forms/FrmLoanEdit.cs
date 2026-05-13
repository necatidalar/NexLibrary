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
    private readonly BookCopyApiService _bookCopyApiService;

    private List<BookListResponse> _books = new();
    private List<MemberListResponse> _members = new();

    public FrmLoanEdit(
    LoanApiService loanApiService,
    BookApiService bookApiService,
    MemberApiService memberApiService,
    BookCopyApiService bookCopyApiService)
    {
        InitializeComponent();

        _loanApiService = loanApiService;
        _bookApiService = bookApiService;
        _memberApiService = memberApiService;
        _bookCopyApiService = bookCopyApiService;
    }

    private async void FrmLoanEdit_Load(object sender, EventArgs e)
    {
        dtpPlanlananIadeTarihi.Value = DateTime.Today.AddDays(14);

        await LoadBooksAsync();
        await LoadMembersAsync();
    }

    private async Task LoadBooksAsync()
    {
        var bookResult = await _bookApiService.GetPagedAsync(1, 500);
        var stockSummary = await _bookCopyApiService.GetStockSummaryAsync();

        var availableBookIds = stockSummary
            .Where(x => x.Musait > 0)
            .Select(x => x.KitapId)
            .ToHashSet();

        _books = bookResult?.Items
            .Where(x => x.AktifMi && availableBookIds.Contains(x.Id))
            .OrderBy(x => x.KitapAdi)
            .ToList() ?? new List<BookListResponse>();

        cmbBooks.DataSource = null;
        cmbBooks.DataSource = _books;
        cmbBooks.DisplayMember = nameof(BookListResponse.KitapAdi);
        cmbBooks.ValueMember = nameof(BookListResponse.Id);

        if (_books.Count == 0)
        {
            MessageBox.Show(
                "Ödünç verilebilecek müsait kitap bulunamadı.",
                "NexLibrary",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }
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
            KitapKopyaId = null,
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