using NexLibrary.Contracts.BookCopies;
using NexLibrary.Desktop.Services;

namespace NexLibrary.Desktop.Forms;

public partial class FrmBookCopies : Form
{
    private readonly BookCopyApiService _bookCopyApiService;

    private List<BookCopyStockSummaryResponse> _stockSummary = new();
    private List<BookCopyListResponse> _copies = new();

    private int _selectedBookId;
    private string _selectedBookName = string.Empty;

    public FrmBookCopies(BookCopyApiService bookCopyApiService)
    {
        InitializeComponent();

        _bookCopyApiService = bookCopyApiService;
    }

    private async void FrmBookCopies_Load(object sender, EventArgs e)
    {
        await LoadStockSummaryAsync();
    }

    private async Task LoadStockSummaryAsync()
    {
        try
        {
            btnRefresh.Enabled = false;

            _stockSummary = await _bookCopyApiService.GetStockSummaryAsync();

            BindStockGrid();
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"Stok özeti yüklenirken hata oluştu:\n{ex.Message}",
                "NexLibrary",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
        finally
        {
            btnRefresh.Enabled = true;
        }
    }

    private void BindStockGrid()
    {
        dgvStock.Columns.Clear();
        dgvStock.Rows.Clear();

        dgvStock.AutoGenerateColumns = false;
        dgvStock.AllowUserToAddRows = false;
        dgvStock.AllowUserToDeleteRows = false;
        dgvStock.ReadOnly = true;
        dgvStock.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvStock.MultiSelect = false;
        dgvStock.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

        dgvStock.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "KitapId",
            HeaderText = "ID",
            FillWeight = 40
        });

        dgvStock.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "KitapAdi",
            HeaderText = "Kitap Adı",
            FillWeight = 220
        });

        dgvStock.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "ToplamKopya",
            HeaderText = "Toplam",
            FillWeight = 70
        });

        dgvStock.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "Musait",
            HeaderText = "Müsait",
            FillWeight = 70
        });

        dgvStock.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "Oduncte",
            HeaderText = "Ödünçte",
            FillWeight = 70
        });

        dgvStock.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "Gecikti",
            HeaderText = "Gecikti",
            FillWeight = 70
        });

        dgvStock.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "Kayip",
            HeaderText = "Kayıp",
            FillWeight = 60
        });

        dgvStock.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "Hasarli",
            HeaderText = "Hasarlı",
            FillWeight = 70
        });

        foreach (var item in _stockSummary)
        {
            var rowIndex = dgvStock.Rows.Add();
            var row = dgvStock.Rows[rowIndex];

            row.Cells["KitapId"].Value = item.KitapId;
            row.Cells["KitapAdi"].Value = item.KitapAdi;
            row.Cells["ToplamKopya"].Value = item.ToplamKopya;
            row.Cells["Musait"].Value = item.Musait;
            row.Cells["Oduncte"].Value = item.Oduncte;
            row.Cells["Gecikti"].Value = item.Gecikti;
            row.Cells["Kayip"].Value = item.Kayip;
            row.Cells["Hasarli"].Value = item.Hasarli;

            row.Tag = item;

            if (item.Musait == 0 && item.ToplamKopya > 0)
            {
                row.DefaultCellStyle.BackColor = Color.MistyRose;
                row.DefaultCellStyle.ForeColor = Color.DarkRed;
            }
            else if (item.Musait > 0)
            {
                row.DefaultCellStyle.BackColor = Color.Honeydew;
                row.DefaultCellStyle.ForeColor = Color.DarkGreen;
            }
        }

        lblStockCount.Text = $"Kitap sayısı: {_stockSummary.Count}";

        _selectedBookId = 0;
        _selectedBookName = string.Empty;
        _copies.Clear();
        BindCopiesGrid();
    }

    private async Task LoadCopiesAsync(int kitapId)
    {
        try
        {
            _copies = await _bookCopyApiService.GetByBookIdAsync(kitapId);

            BindCopiesGrid();
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"Kitap kopyaları yüklenirken hata oluştu:\n{ex.Message}",
                "NexLibrary",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    private void BindCopiesGrid()
    {
        dgvCopies.Columns.Clear();
        dgvCopies.Rows.Clear();

        dgvCopies.AutoGenerateColumns = false;
        dgvCopies.AllowUserToAddRows = false;
        dgvCopies.AllowUserToDeleteRows = false;
        dgvCopies.ReadOnly = true;
        dgvCopies.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvCopies.MultiSelect = false;
        dgvCopies.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

        dgvCopies.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "Id",
            HeaderText = "Kopya ID",
            FillWeight = 60
        });

        dgvCopies.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "Barkod",
            HeaderText = "Barkod",
            FillWeight = 140
        });

        dgvCopies.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "DemirbasNo",
            HeaderText = "Demirbaş No",
            FillWeight = 130
        });

        dgvCopies.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "Durum",
            HeaderText = "Durum",
            FillWeight = 100
        });

        dgvCopies.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "Aciklama",
            HeaderText = "Açıklama",
            FillWeight = 180
        });

        dgvCopies.Columns.Add(new DataGridViewCheckBoxColumn
        {
            Name = "AktifMi",
            HeaderText = "Aktif",
            FillWeight = 60
        });

        foreach (var copy in _copies)
        {
            var rowIndex = dgvCopies.Rows.Add();
            var row = dgvCopies.Rows[rowIndex];

            row.Cells["Id"].Value = copy.Id;
            row.Cells["Barkod"].Value = copy.Barkod;
            row.Cells["DemirbasNo"].Value = copy.DemirbasNo ?? "-";
            row.Cells["Durum"].Value = copy.Durum;
            row.Cells["Aciklama"].Value = copy.Aciklama ?? "-";
            row.Cells["AktifMi"].Value = copy.AktifMi;

            if (copy.Durum == "Musait")
            {
                row.DefaultCellStyle.BackColor = Color.Honeydew;
                row.DefaultCellStyle.ForeColor = Color.DarkGreen;
            }
            else if (copy.Durum == "Oduncte" || copy.Durum == "Gecikti")
            {
                row.DefaultCellStyle.BackColor = Color.MistyRose;
                row.DefaultCellStyle.ForeColor = Color.DarkRed;
            }
            else if (copy.Durum == "Kayip" || copy.Durum == "Hasarli" || copy.Durum == "Pasif")
            {
                row.DefaultCellStyle.BackColor = Color.Gainsboro;
                row.DefaultCellStyle.ForeColor = Color.DimGray;
            }
        }

        lblCopiesTitle.Text = _selectedBookId > 0
            ? $"Kopyalar: {_selectedBookName}"
            : "Kopyalar";

        lblCopiesCount.Text = $"Kopya sayısı: {_copies.Count}";
    }

    private async void dgvStock_SelectionChanged(object sender, EventArgs e)
    {
        if (dgvStock.CurrentRow?.Tag is not BookCopyStockSummaryResponse selected)
        {
            return;
        }

        _selectedBookId = selected.KitapId;
        _selectedBookName = selected.KitapAdi;

        await LoadCopiesAsync(_selectedBookId);
    }

    private async void btnRefresh_Click(object sender, EventArgs e)
    {
        await LoadStockSummaryAsync();
    }

    private async void btnAddCopy_Click(object sender, EventArgs e)
    {
        if (_selectedBookId <= 0)
        {
            MessageBox.Show(
                "Lütfen kopya eklenecek kitabı seçin.",
                "NexLibrary",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            return;
        }

        using var form = new FrmBookCopyEdit(
            _bookCopyApiService,
            _selectedBookId,
            _selectedBookName);

        if (form.ShowDialog(this) == DialogResult.OK)
        {
            await LoadStockSummaryAsync();

            var row = dgvStock.Rows
                .Cast<DataGridViewRow>()
                .FirstOrDefault(x =>
                    x.Cells["KitapId"].Value is not null &&
                    Convert.ToInt32(x.Cells["KitapId"].Value) == _selectedBookId);

            if (row is not null)
            {
                row.Selected = true;
                dgvStock.CurrentCell = row.Cells["KitapAdi"];
            }

            await LoadCopiesAsync(_selectedBookId);
        }
    }
}