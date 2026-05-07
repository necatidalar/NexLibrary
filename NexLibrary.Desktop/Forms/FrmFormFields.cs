using NexLibrary.Contracts.DynamicForms;
using NexLibrary.Desktop.Services;

namespace NexLibrary.Desktop.Forms;

public partial class FrmFormFields : Form
{
    private readonly FormFieldApiService _formFieldApiService;

    private List<FormFieldResponse> _fields = new();
    private FormFieldResponse? _selectedField;

    public FrmFormFields(FormFieldApiService formFieldApiService)
    {
        InitializeComponent();

        _formFieldApiService = formFieldApiService;
    }

    private async void FrmFormFields_Load(object sender, EventArgs e)
    {
        cmbModule.SelectedIndex = 0;
        await LoadFieldsAsync();
    }

    private async Task LoadFieldsAsync()
    {
        try
        {
            btnRefresh.Enabled = false;

            var moduleCode = cmbModule.SelectedItem?.ToString() ?? "Kitaplar";

            _fields = await _formFieldApiService.GetByModuleAsync(moduleCode);

            BindGrid();
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"Form alanları yüklenirken hata oluştu:\n{ex.Message}",
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
        dgvFields.Columns.Clear();
        dgvFields.Rows.Clear();

        dgvFields.AutoGenerateColumns = false;
        dgvFields.AllowUserToAddRows = false;
        dgvFields.AllowUserToDeleteRows = false;
        dgvFields.ReadOnly = true;
        dgvFields.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvFields.MultiSelect = false;
        dgvFields.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

        dgvFields.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "Id",
            HeaderText = "ID",
            FillWeight = 40
        });

        dgvFields.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "AlanKodu",
            HeaderText = "Alan Kodu",
            FillWeight = 130
        });

        dgvFields.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "AlanAdi",
            HeaderText = "Alan Adı",
            FillWeight = 160
        });

        dgvFields.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "AlanTipi",
            HeaderText = "Tip",
            FillWeight = 90
        });

        dgvFields.Columns.Add(new DataGridViewCheckBoxColumn
        {
            Name = "ZorunluMu",
            HeaderText = "Zorunlu",
            FillWeight = 70
        });

        dgvFields.Columns.Add(new DataGridViewCheckBoxColumn
        {
            Name = "ListedeGorunsunMu",
            HeaderText = "Listede",
            FillWeight = 70
        });

        dgvFields.Columns.Add(new DataGridViewCheckBoxColumn
        {
            Name = "FormdaGorunsunMu",
            HeaderText = "Formda",
            FillWeight = 70
        });

        dgvFields.Columns.Add(new DataGridViewCheckBoxColumn
        {
            Name = "SistemAlaniMi",
            HeaderText = "Sistem",
            FillWeight = 70
        });

        dgvFields.Columns.Add(new DataGridViewCheckBoxColumn
        {
            Name = "AktifMi",
            HeaderText = "Aktif",
            FillWeight = 60
        });

        foreach (var field in _fields.OrderBy(x => x.SiraNo).ThenBy(x => x.Id))
        {
            var rowIndex = dgvFields.Rows.Add();
            var row = dgvFields.Rows[rowIndex];

            row.Cells["Id"].Value = field.Id;
            row.Cells["AlanKodu"].Value = field.AlanKodu;
            row.Cells["AlanAdi"].Value = field.AlanAdi;
            row.Cells["AlanTipi"].Value = field.AlanTipi;
            row.Cells["ZorunluMu"].Value = field.ZorunluMu;
            row.Cells["ListedeGorunsunMu"].Value = field.ListedeGorunsunMu;
            row.Cells["FormdaGorunsunMu"].Value = field.FormdaGorunsunMu;
            row.Cells["SistemAlaniMi"].Value = field.SistemAlaniMi;
            row.Cells["AktifMi"].Value = field.AktifMi;

            row.Tag = field;
        }

        lblCount.Text = $"Toplam alan: {_fields.Count}";
        _selectedField = null;
    }

    private async void cmbModule_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (IsHandleCreated)
        {
            await LoadFieldsAsync();
        }
    }

    private async void btnRefresh_Click(object sender, EventArgs e)
    {
        await LoadFieldsAsync();
    }

    private async void btnAdd_Click(object sender, EventArgs e)
    {
        var moduleCode = cmbModule.SelectedItem?.ToString() ?? "Kitaplar";

        using var form = new FrmFormFieldEdit(_formFieldApiService, moduleCode);

        if (form.ShowDialog(this) == DialogResult.OK)
        {
            await LoadFieldsAsync();
        }
    }

    private async void btnEdit_Click(object sender, EventArgs e)
    {
        if (_selectedField is null)
        {
            MessageBox.Show(
                "Lütfen düzenlenecek alanı seçin.",
                "NexLibrary",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            return;
        }

        using var form = new FrmFormFieldEdit(
            _formFieldApiService,
            _selectedField.ModulKodu,
            _selectedField);

        if (form.ShowDialog(this) == DialogResult.OK)
        {
            await LoadFieldsAsync();
        }
    }

    private async void btnSetActive_Click(object sender, EventArgs e)
    {
        if (_selectedField is null)
        {
            MessageBox.Show(
                "Lütfen işlem yapılacak alanı seçin.",
                "NexLibrary",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            return;
        }

        if (_selectedField.SistemAlaniMi && _selectedField.AktifMi)
        {
            MessageBox.Show(
                "Sistem alanı pasif yapılamaz.",
                "NexLibrary",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            return;
        }

        var newStatus = !_selectedField.AktifMi;

        var confirm = MessageBox.Show(
            newStatus
                ? "Seçili alan aktif yapılacak. Devam edilsin mi?"
                : "Seçili alan pasif yapılacak. Devam edilsin mi?",
            "NexLibrary",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);

        if (confirm != DialogResult.Yes)
        {
            return;
        }

        var success = await _formFieldApiService.SetActiveAsync(_selectedField.Id, newStatus);

        if (!success)
        {
            MessageBox.Show(
                "Alan durumu güncellenemedi.",
                "NexLibrary",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
            return;
        }

        await LoadFieldsAsync();
    }

    private void dgvFields_SelectionChanged(object sender, EventArgs e)
    {
        _selectedField = dgvFields.CurrentRow?.Tag as FormFieldResponse;

        if (_selectedField is null)
        {
            btnSetActive.Text = "Aktif/Pasif";
            return;
        }

        btnSetActive.Text = _selectedField.AktifMi ? "Pasif Yap" : "Aktif Yap";
    }

    private void dgvFields_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex >= 0)
        {
            btnEdit.PerformClick();
        }
    }
}