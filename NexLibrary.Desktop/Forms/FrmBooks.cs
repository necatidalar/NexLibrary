using NexLibrary.Contracts.Books;
using NexLibrary.Desktop.Services;

namespace NexLibrary.Desktop.Forms;

public partial class FrmBooks : Form
{
    private readonly BookApiService _bookApiService;
    private readonly FormFieldApiService _formFieldApiService;

    private List<BookListResponse> _books = new();
    private int _selectedBookId;

    public FrmBooks(
        BookApiService bookApiService,
        FormFieldApiService formFieldApiService)
    {
        InitializeComponent();

        _bookApiService = bookApiService;
        _formFieldApiService = formFieldApiService;
    }

    private async void FrmBooks_Load(object sender, EventArgs e)
    {
        await LoadBooksAsync();
    }

    private async Task LoadBooksAsync()
    {
        try
        {
            btnRefresh.Enabled = false;

            var result = await _bookApiService.GetPagedAsync(
                pageNumber: 1,
                pageSize: 100,
                search: txtSearch.Text.Trim());

            _books = result?.Items ?? new List<BookListResponse>();

            BindGrid();
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"Kitaplar yüklenirken hata oluştu:\n{ex.Message}",
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
        dgvBooks.Columns.Clear();
        dgvBooks.Rows.Clear();

        dgvBooks.AutoGenerateColumns = false;
        dgvBooks.AllowUserToAddRows = false;
        dgvBooks.AllowUserToDeleteRows = false;
        dgvBooks.ReadOnly = true;
        dgvBooks.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvBooks.MultiSelect = false;
        dgvBooks.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

        dgvBooks.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "Id",
            HeaderText = "ID",
            DataPropertyName = "Id",
            FillWeight = 40
        });

        dgvBooks.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "KitapAdi",
            HeaderText = "Kitap Adı",
            DataPropertyName = "KitapAdi",
            FillWeight = 180
        });

        var dynamicColumnNames = _books
            .SelectMany(x => x.DinamikAlanlar.Keys)
            .Distinct()
            .ToList();

        foreach (var columnName in dynamicColumnNames)
        {
            dgvBooks.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = columnName,
                HeaderText = columnName,
                FillWeight = 120
            });
        }

        dgvBooks.Columns.Add(new DataGridViewCheckBoxColumn
        {
            Name = "AktifMi",
            HeaderText = "Aktif",
            DataPropertyName = "AktifMi",
            FillWeight = 50
        });

        foreach (var book in _books)
        {
            var rowIndex = dgvBooks.Rows.Add();
            var row = dgvBooks.Rows[rowIndex];

            row.Cells["Id"].Value = book.Id;
            row.Cells["KitapAdi"].Value = book.KitapAdi;
            row.Cells["AktifMi"].Value = book.AktifMi;

            foreach (var dynamicValue in book.DinamikAlanlar)
            {
                if (dgvBooks.Columns.Contains(dynamicValue.Key))
                {
                    row.Cells[dynamicValue.Key].Value = dynamicValue.Value;
                }
            }
        }

        lblCount.Text = $"Toplam kayıt: {_books.Count}";
    }

    private async void btnRefresh_Click(object sender, EventArgs e)
    {
        await LoadBooksAsync();
    }

    private async void btnSearch_Click(object sender, EventArgs e)
    {
        await LoadBooksAsync();
    }

    private async void txtSearch_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Enter)
        {
            e.SuppressKeyPress = true;
            await LoadBooksAsync();
        }
    }

    private async void btnAdd_Click(object sender, EventArgs e)
    {
        using var form = new FrmBookEdit(_bookApiService, _formFieldApiService);

        if (form.ShowDialog(this) == DialogResult.OK)
        {
            await LoadBooksAsync();
        }
    }

    private async void btnEdit_Click(object sender, EventArgs e)
    {
        if (_selectedBookId <= 0)
        {
            MessageBox.Show(
                "Lütfen düzenlenecek kitabı seçin.",
                "NexLibrary",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            return;
        }

        using var form = new FrmBookEdit(
            _bookApiService,
            _formFieldApiService,
            _selectedBookId);

        if (form.ShowDialog(this) == DialogResult.OK)
        {
            await LoadBooksAsync();
        }
    }

    private async void btnDelete_Click(object sender, EventArgs e)
    {
        if (_selectedBookId <= 0)
        {
            MessageBox.Show(
                "Lütfen pasif yapılacak kitabı seçin.",
                "NexLibrary",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            return;
        }

        var confirm = MessageBox.Show(
            "Seçili kitap pasif hale getirilecek. Devam edilsin mi?",
            "NexLibrary",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);

        if (confirm != DialogResult.Yes)
        {
            return;
        }

        var success = await _bookApiService.DeleteAsync(_selectedBookId);

        if (!success)
        {
            MessageBox.Show(
                "Kitap pasif hale getirilemedi.",
                "NexLibrary",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
            return;
        }

        await LoadBooksAsync();
    }

    private void dgvBooks_SelectionChanged(object sender, EventArgs e)
    {
        _selectedBookId = 0;

        if (dgvBooks.CurrentRow?.Cells["Id"].Value is null)
        {
            return;
        }

        int.TryParse(dgvBooks.CurrentRow.Cells["Id"].Value.ToString(), out _selectedBookId);
    }

    private void dgvBooks_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex >= 0)
        {
            btnEdit.PerformClick();
        }
    }
}