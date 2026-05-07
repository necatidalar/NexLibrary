using NexLibrary.Contracts.DynamicForms;
using NexLibrary.Desktop.Services;

namespace NexLibrary.Desktop.Forms;

public partial class FrmListSettings : Form
{
    private readonly FormFieldApiService _formFieldApiService;

    private List<FormFieldResponse> _fields = new();

    public FrmListSettings(FormFieldApiService formFieldApiService)
    {
        InitializeComponent();

        _formFieldApiService = formFieldApiService;
    }

    private async void FrmListSettings_Load(object sender, EventArgs e)
    {
        cmbModule.SelectedIndex = 0;
        await LoadSettingsAsync();
    }

    private async Task LoadSettingsAsync()
    {
        try
        {
            btnRefresh.Enabled = false;
            btnSave.Enabled = false;

            var moduleCode = cmbModule.SelectedItem?.ToString() ?? "Kitaplar";

            _fields = await _formFieldApiService.GetByModuleAsync(moduleCode);

            BindGrid();
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"Liste ayarları yüklenirken hata oluştu:\n{ex.Message}",
                "NexLibrary",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
        finally
        {
            btnRefresh.Enabled = true;
            btnSave.Enabled = true;
        }
    }

    private void BindGrid()
    {
        dgvSettings.Columns.Clear();
        dgvSettings.Rows.Clear();

        dgvSettings.AutoGenerateColumns = false;
        dgvSettings.AllowUserToAddRows = false;
        dgvSettings.AllowUserToDeleteRows = false;
        dgvSettings.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvSettings.MultiSelect = false;
        dgvSettings.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvSettings.ReadOnly = false;

        dgvSettings.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "Id",
            HeaderText = "ID",
            FillWeight = 35,
            ReadOnly = true
        });

        dgvSettings.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "AlanKodu",
            HeaderText = "Alan Kodu",
            FillWeight = 120,
            ReadOnly = true
        });

        dgvSettings.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "AlanAdi",
            HeaderText = "Alan Adı",
            FillWeight = 150,
            ReadOnly = true
        });

        dgvSettings.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "AlanTipi",
            HeaderText = "Tip",
            FillWeight = 80,
            ReadOnly = true
        });

        dgvSettings.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "SiraNo",
            HeaderText = "Sıra",
            FillWeight = 55,
            ReadOnly = false
        });

        dgvSettings.Columns.Add(new DataGridViewCheckBoxColumn
        {
            Name = "ListedeGorunsunMu",
            HeaderText = "Listede",
            FillWeight = 70,
            ReadOnly = false
        });

        dgvSettings.Columns.Add(new DataGridViewCheckBoxColumn
        {
            Name = "AramadaGorunsunMu",
            HeaderText = "Aramada",
            FillWeight = 75,
            ReadOnly = false
        });

        dgvSettings.Columns.Add(new DataGridViewCheckBoxColumn
        {
            Name = "DetaydaGorunsunMu",
            HeaderText = "Detayda",
            FillWeight = 75,
            ReadOnly = false
        });

        dgvSettings.Columns.Add(new DataGridViewCheckBoxColumn
        {
            Name = "HizliKayittaGorunsunMu",
            HeaderText = "Hızlı Kayıt",
            FillWeight = 85,
            ReadOnly = false
        });

        dgvSettings.Columns.Add(new DataGridViewCheckBoxColumn
        {
            Name = "SistemAlaniMi",
            HeaderText = "Sistem",
            FillWeight = 65,
            ReadOnly = true
        });

        dgvSettings.Columns.Add(new DataGridViewCheckBoxColumn
        {
            Name = "AktifMi",
            HeaderText = "Aktif",
            FillWeight = 60,
            ReadOnly = true
        });

        foreach (var field in _fields.OrderBy(x => x.SiraNo).ThenBy(x => x.Id))
        {
            var rowIndex = dgvSettings.Rows.Add();
            var row = dgvSettings.Rows[rowIndex];

            row.Cells["Id"].Value = field.Id;
            row.Cells["AlanKodu"].Value = field.AlanKodu;
            row.Cells["AlanAdi"].Value = field.AlanAdi;
            row.Cells["AlanTipi"].Value = field.AlanTipi;
            row.Cells["SiraNo"].Value = field.SiraNo;
            row.Cells["ListedeGorunsunMu"].Value = field.ListedeGorunsunMu;
            row.Cells["AramadaGorunsunMu"].Value = field.AramadaGorunsunMu;
            row.Cells["DetaydaGorunsunMu"].Value = field.DetaydaGorunsunMu;
            row.Cells["HizliKayittaGorunsunMu"].Value = field.HizliKayittaGorunsunMu;
            row.Cells["SistemAlaniMi"].Value = field.SistemAlaniMi;
            row.Cells["AktifMi"].Value = field.AktifMi;

            row.Tag = field;

            if (field.SistemAlaniMi)
            {
                row.DefaultCellStyle.BackColor = Color.FromArgb(245, 245, 245);
                row.Cells["ListedeGorunsunMu"].ReadOnly = true;
            }

            if (!field.AktifMi)
            {
                row.DefaultCellStyle.ForeColor = Color.Gray;
            }
        }

        lblCount.Text = $"Toplam alan: {_fields.Count}";
    }

    private async void cmbModule_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (IsHandleCreated)
        {
            await LoadSettingsAsync();
        }
    }

    private async void btnRefresh_Click(object sender, EventArgs e)
    {
        await LoadSettingsAsync();
    }

    private async void btnSave_Click(object sender, EventArgs e)
    {
        dgvSettings.EndEdit();

        var confirm = MessageBox.Show(
            "Liste ayarları kaydedilecek. Devam edilsin mi?",
            "NexLibrary",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);

        if (confirm != DialogResult.Yes)
        {
            return;
        }

        btnSave.Enabled = false;

        try
        {
            foreach (DataGridViewRow row in dgvSettings.Rows)
            {
                if (row.Tag is not FormFieldResponse field)
                {
                    continue;
                }

                var siraNo = GetIntCellValue(row, "SiraNo", field.SiraNo);

                if (siraNo <= 0)
                {
                    MessageBox.Show(
                        $"{field.AlanAdi} için sıra no 1 veya daha büyük olmalıdır.",
                        "NexLibrary",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }

                var listedeGorunsunMu = GetBoolCellValue(row, "ListedeGorunsunMu");
                var aramadaGorunsunMu = GetBoolCellValue(row, "AramadaGorunsunMu");
                var detaydaGorunsunMu = GetBoolCellValue(row, "DetaydaGorunsunMu");
                var hizliKayittaGorunsunMu = GetBoolCellValue(row, "HizliKayittaGorunsunMu");

                if (field.SistemAlaniMi)
                {
                    listedeGorunsunMu = field.ListedeGorunsunMu;
                }

                var request = new FormFieldUpdateRequest
                {
                    Id = field.Id,
                    AlanAdi = field.AlanAdi,
                    MinimumKarakter = field.MinimumKarakter,
                    MaksimumKarakter = field.MaksimumKarakter,
                    ZorunluMu = field.ZorunluMu,
                    BenzersizMi = field.BenzersizMi,
                    VarsayilanDeger = field.VarsayilanDeger,
                    Aciklama = field.Aciklama,
                    Placeholder = field.Placeholder,
                    SiraNo = siraNo,
                    FormdaGorunsunMu = field.FormdaGorunsunMu,
                    ListedeGorunsunMu = listedeGorunsunMu,
                    AramadaGorunsunMu = aramadaGorunsunMu,
                    DetaydaGorunsunMu = detaydaGorunsunMu,
                    HizliKayittaGorunsunMu = hizliKayittaGorunsunMu,
                    AktifMi = field.AktifMi
                };

                var result = await _formFieldApiService.UpdateAsync(field.Id, request);

                if (result is null)
                {
                    MessageBox.Show(
                        $"{field.AlanAdi} alanı güncellenemedi.",
                        "NexLibrary",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }
            }

            MessageBox.Show(
                "Liste ayarları başarıyla kaydedildi.",
                "NexLibrary",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            await LoadSettingsAsync();
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"Liste ayarları kaydedilirken hata oluştu:\n{ex.Message}",
                "NexLibrary",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
        finally
        {
            btnSave.Enabled = true;
        }
    }

    private static int GetIntCellValue(DataGridViewRow row, string columnName, int defaultValue)
    {
        var value = row.Cells[columnName].Value?.ToString();

        return int.TryParse(value, out var result)
            ? result
            : defaultValue;
    }

    private static bool GetBoolCellValue(DataGridViewRow row, string columnName)
    {
        var value = row.Cells[columnName].Value;

        if (value is bool boolValue)
        {
            return boolValue;
        }

        return bool.TryParse(value?.ToString(), out var result) && result;
    }
}