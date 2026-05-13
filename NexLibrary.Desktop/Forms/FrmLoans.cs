using NexLibrary.Contracts.Loans;
using NexLibrary.Desktop.Services;

namespace NexLibrary.Desktop.Forms;

public partial class FrmLoans : Form
{
    private readonly LoanApiService _loanApiService;
    private readonly BookApiService _bookApiService;
    private readonly MemberApiService _memberApiService;
    private readonly BookCopyApiService _bookCopyApiService;

    private List<LoanListResponse> _loans = new();
    private int _selectedLoanId;
    private bool _showingOverdueOnly;

    public FrmLoans(
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

    private async void FrmLoans_Load(object sender, EventArgs e)
    {
        await MarkOverdueAndLoadAsync();
    }

    private async Task MarkOverdueAndLoadAsync()
    {
        await _loanApiService.MarkOverdueAsync();
        await LoadLoansAsync();
    }

    private async Task LoadLoansAsync()
    {
        try
        {
            btnRefresh.Enabled = false;

            var result = _showingOverdueOnly
                ? await _loanApiService.GetOverdueAsync(1, 100)
                : await _loanApiService.GetPagedAsync(1, 100, txtSearch.Text.Trim());

            _loans = result?.Items ?? new List<LoanListResponse>();

            BindGrid();
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"Ödünç kayıtları yüklenirken hata oluştu:\n{ex.Message}",
                "NexLibrary",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
        finally
        {
            btnRefresh.Enabled = true;
        }
    }

    private void BindGrid()
    {
        dgvLoans.Columns.Clear();
        dgvLoans.Rows.Clear();

        dgvLoans.AutoGenerateColumns = false;
        dgvLoans.AllowUserToAddRows = false;
        dgvLoans.AllowUserToDeleteRows = false;
        dgvLoans.ReadOnly = true;
        dgvLoans.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvLoans.MultiSelect = false;
        dgvLoans.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

        dgvLoans.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "Id",
            HeaderText = "ID",
            FillWeight = 40
        });

        dgvLoans.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "KitapAdi",
            HeaderText = "Kitap",
            FillWeight = 180
        });

        dgvLoans.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "KitapKopyaBarkod",
            HeaderText = "Barkod",
            FillWeight = 100
        });

        dgvLoans.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "UyeAdiSoyadi",
            HeaderText = "Üye",
            FillWeight = 160
        });

        dgvLoans.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "VerilisTarihi",
            HeaderText = "Veriliş",
            FillWeight = 90
        });

        dgvLoans.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "PlanlananIadeTarihi",
            HeaderText = "Planlanan İade",
            FillWeight = 110
        });

        dgvLoans.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "IadeTarihi",
            HeaderText = "İade Tarihi",
            FillWeight = 100
        });

        dgvLoans.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "Durum",
            HeaderText = "Durum",
            FillWeight = 80
        });

        foreach (var loan in _loans)
        {
            var rowIndex = dgvLoans.Rows.Add();
            var row = dgvLoans.Rows[rowIndex];

            row.Cells["Id"].Value = loan.Id;
            row.Cells["KitapAdi"].Value = loan.KitapAdi;
            row.Cells["KitapKopyaBarkod"].Value = loan.KitapKopyaBarkod ?? "-";
            row.Cells["UyeAdiSoyadi"].Value = loan.UyeAdiSoyadi;
            row.Cells["VerilisTarihi"].Value = loan.VerilisTarihi.ToString("dd.MM.yyyy");
            row.Cells["PlanlananIadeTarihi"].Value = loan.PlanlananIadeTarihi.ToString("dd.MM.yyyy");
            row.Cells["IadeTarihi"].Value = loan.IadeTarihi?.ToString("dd.MM.yyyy") ?? "-";
            row.Cells["Durum"].Value = loan.Durum;

            if (loan.Durum == "Gecikti")
            {
                row.DefaultCellStyle.BackColor = Color.MistyRose;
                row.DefaultCellStyle.ForeColor = Color.DarkRed;
            }
            else if (loan.Durum == "IadeEdildi")
            {
                row.DefaultCellStyle.BackColor = Color.Honeydew;
                row.DefaultCellStyle.ForeColor = Color.DarkGreen;
            }
            else if (loan.Durum == "IptalEdildi")
            {
                row.DefaultCellStyle.BackColor = Color.Gainsboro;
                row.DefaultCellStyle.ForeColor = Color.DimGray;
            }
        }

        lblCount.Text = _showingOverdueOnly
            ? $"Geciken kayıt: {_loans.Count}"
            : $"Toplam ödünç: {_loans.Count}";

        btnShowOverdue.Text = _showingOverdueOnly ? "Tümünü Göster" : "Gecikenleri Göster";

        _selectedLoanId = 0;
    }

    private async void btnSearch_Click(object sender, EventArgs e)
    {
        _showingOverdueOnly = false;
        await LoadLoansAsync();
    }

    private async void btnRefresh_Click(object sender, EventArgs e)
    {
        await MarkOverdueAndLoadAsync();
    }

    private async void btnShowOverdue_Click(object sender, EventArgs e)
    {
        _showingOverdueOnly = !_showingOverdueOnly;
        await LoadLoansAsync();
    }

    private async void txtSearch_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Enter)
        {
            e.SuppressKeyPress = true;
            _showingOverdueOnly = false;
            await LoadLoansAsync();
        }
    }

    private async void btnAdd_Click(object sender, EventArgs e)
    {
        using var form = new FrmLoanEdit(
            _loanApiService,
            _bookApiService,
            _memberApiService,
            _bookCopyApiService);

        if (form.ShowDialog(this) == DialogResult.OK)
        {
            await MarkOverdueAndLoadAsync();
        }
    }

    private async void btnReturn_Click(object sender, EventArgs e)
    {
        if (_selectedLoanId <= 0)
        {
            MessageBox.Show(
                "Lütfen iade alınacak ödünç kaydını seçin.",
                "NexLibrary",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            return;
        }

        var selectedLoan = _loans.FirstOrDefault(x => x.Id == _selectedLoanId);

        if (selectedLoan is null)
        {
            return;
        }

        if (selectedLoan.Durum == "IadeEdildi")
        {
            MessageBox.Show(
                "Bu kayıt zaten iade edilmiş.",
                "NexLibrary",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            return;
        }

        if (selectedLoan.Durum == "IptalEdildi")
        {
            MessageBox.Show(
                "İptal edilmiş kayıt iade alınamaz.",
                "NexLibrary",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            return;
        }

        var confirm = MessageBox.Show(
            $"'{selectedLoan.KitapAdi}' kitabı iade alınacak. Devam edilsin mi?",
            "NexLibrary",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);

        if (confirm != DialogResult.Yes)
        {
            return;
        }

        var request = new LoanReturnRequest
        {
            Aciklama = "Desktop üzerinden iade alındı."
        };

        var result = await _loanApiService.ReturnAsync(_selectedLoanId, request);

        if (result is null)
        {
            MessageBox.Show(
                "İade alma işlemi başarısız.",
                "NexLibrary",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
            return;
        }

        MessageBox.Show(
            "Kitap başarıyla iade alındı.",
            "NexLibrary",
            MessageBoxButtons.OK,
            MessageBoxIcon.Information);

        await MarkOverdueAndLoadAsync();
    }

    private async void btnCancelLoan_Click(object sender, EventArgs e)
    {
        if (_selectedLoanId <= 0)
        {
            MessageBox.Show(
                "Lütfen iptal edilecek ödünç kaydını seçin.",
                "NexLibrary",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            return;
        }

        var confirm = MessageBox.Show(
            "Seçili ödünç kaydı iptal edilecek. Devam edilsin mi?",
            "NexLibrary",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);

        if (confirm != DialogResult.Yes)
        {
            return;
        }

        var success = await _loanApiService.CancelAsync(_selectedLoanId);

        if (!success)
        {
            MessageBox.Show(
                "Ödünç kaydı iptal edilemedi.",
                "NexLibrary",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
            return;
        }

        await MarkOverdueAndLoadAsync();
    }

    private void dgvLoans_SelectionChanged(object sender, EventArgs e)
    {
        _selectedLoanId = 0;

        if (dgvLoans.CurrentRow?.Cells["Id"].Value is null)
        {
            return;
        }

        int.TryParse(dgvLoans.CurrentRow.Cells["Id"].Value.ToString(), out _selectedLoanId);
    }
}