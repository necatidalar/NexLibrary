using NexLibrary.Contracts.DynamicForms;
using NexLibrary.Desktop.Services;

namespace NexLibrary.Desktop.Forms;

public partial class FrmFormFieldEdit : Form
{
    private readonly FormFieldApiService _formFieldApiService;
    private readonly string _moduleCode;
    private readonly FormFieldResponse? _field;

    public FrmFormFieldEdit(
        FormFieldApiService formFieldApiService,
        string moduleCode,
        FormFieldResponse? field = null)
    {
        InitializeComponent();

        _formFieldApiService = formFieldApiService;
        _moduleCode = moduleCode;
        _field = field;
    }

    private void FrmFormFieldEdit_Load(object sender, EventArgs e)
    {
        FillFieldTypes();

        txtModule.Text = _moduleCode;

        if (_field is null)
        {
            Text = "Yeni Form Alanı";
            lblTitle.Text = "Yeni Form Alanı";
            chkAktifMi.Checked = true;
            chkFormdaGorunsunMu.Checked = true;
            chkListedeGorunsunMu.Checked = true;
            chkDetaydaGorunsunMu.Checked = true;
            numSiraNo.Value = 10;
            cmbAlanTipi.SelectedItem = "Metin";
            return;
        }

        Text = "Form Alanı Düzenle";
        lblTitle.Text = "Form Alanı Düzenle";

        txtAlanAdi.Text = _field.AlanAdi;
        txtAlanKodu.Text = _field.AlanKodu;
        cmbAlanTipi.SelectedItem = _field.AlanTipi;

        numMinKarakter.Value = _field.MinimumKarakter ?? 0;
        numMaxKarakter.Value = _field.MaksimumKarakter ?? 0;
        numSiraNo.Value = _field.SiraNo;

        txtVarsayilanDeger.Text = _field.VarsayilanDeger ?? string.Empty;
        txtPlaceholder.Text = _field.Placeholder ?? string.Empty;
        txtAciklama.Text = _field.Aciklama ?? string.Empty;

        chkZorunluMu.Checked = _field.ZorunluMu;
        chkBenzersizMi.Checked = _field.BenzersizMi;
        chkFormdaGorunsunMu.Checked = _field.FormdaGorunsunMu;
        chkListedeGorunsunMu.Checked = _field.ListedeGorunsunMu;
        chkAramadaGorunsunMu.Checked = _field.AramadaGorunsunMu;
        chkDetaydaGorunsunMu.Checked = _field.DetaydaGorunsunMu;
        chkHizliKayittaGorunsunMu.Checked = _field.HizliKayittaGorunsunMu;
        chkAktifMi.Checked = _field.AktifMi;

        txtAlanKodu.ReadOnly = true;
        cmbAlanTipi.Enabled = false;

        if (_field.SistemAlaniMi)
        {
            chkAktifMi.Enabled = false;
            chkZorunluMu.Enabled = false;
        }
    }

    private void FillFieldTypes()
    {
        cmbAlanTipi.Items.Clear();

        cmbAlanTipi.Items.AddRange(new object[]
        {
            "Metin",
            "UzunMetin",
            "Sayi",
            "OndalikliSayi",
            "Tarih",
            "TarihSaat",
            "EvetHayir",
            "Liste",
            "CokluListe",
            "Telefon",
            "Eposta",
            "Para",
            "Barkod"
        });
    }

    private async void btnSave_Click(object sender, EventArgs e)
    {
        if (!ValidateForm())
        {
            return;
        }

        btnSave.Enabled = false;

        try
        {
            if (_field is null)
            {
                var request = new FormFieldCreateRequest
                {
                    ModulKodu = _moduleCode,
                    AlanAdi = txtAlanAdi.Text.Trim(),
                    AlanKodu = txtAlanKodu.Text.Trim(),
                    AlanTipi = cmbAlanTipi.SelectedItem?.ToString() ?? "Metin",
                    MinimumKarakter = GetNullableInt(numMinKarakter),
                    MaksimumKarakter = GetNullableInt(numMaxKarakter),
                    ZorunluMu = chkZorunluMu.Checked,
                    BenzersizMi = chkBenzersizMi.Checked,
                    VarsayilanDeger = GetNullableText(txtVarsayilanDeger),
                    Aciklama = GetNullableText(txtAciklama),
                    Placeholder = GetNullableText(txtPlaceholder),
                    SiraNo = Convert.ToInt32(numSiraNo.Value),
                    FormdaGorunsunMu = chkFormdaGorunsunMu.Checked,
                    ListedeGorunsunMu = chkListedeGorunsunMu.Checked,
                    AramadaGorunsunMu = chkAramadaGorunsunMu.Checked,
                    DetaydaGorunsunMu = chkDetaydaGorunsunMu.Checked,
                    HizliKayittaGorunsunMu = chkHizliKayittaGorunsunMu.Checked
                };

                var result = await _formFieldApiService.CreateAsync(request);

                if (result is null)
                {
                    MessageBox.Show(
                        "Form alanı oluşturulamadı. Alan kodu daha önce kullanılmış olabilir.",
                        "NexLibrary",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }
            }
            else
            {
                var request = new FormFieldUpdateRequest
                {
                    Id = _field.Id,
                    AlanAdi = txtAlanAdi.Text.Trim(),
                    MinimumKarakter = GetNullableInt(numMinKarakter),
                    MaksimumKarakter = GetNullableInt(numMaxKarakter),
                    ZorunluMu = chkZorunluMu.Checked,
                    BenzersizMi = chkBenzersizMi.Checked,
                    VarsayilanDeger = GetNullableText(txtVarsayilanDeger),
                    Aciklama = GetNullableText(txtAciklama),
                    Placeholder = GetNullableText(txtPlaceholder),
                    SiraNo = Convert.ToInt32(numSiraNo.Value),
                    FormdaGorunsunMu = chkFormdaGorunsunMu.Checked,
                    ListedeGorunsunMu = chkListedeGorunsunMu.Checked,
                    AramadaGorunsunMu = chkAramadaGorunsunMu.Checked,
                    DetaydaGorunsunMu = chkDetaydaGorunsunMu.Checked,
                    HizliKayittaGorunsunMu = chkHizliKayittaGorunsunMu.Checked,
                    AktifMi = chkAktifMi.Checked
                };

                var result = await _formFieldApiService.UpdateAsync(_field.Id, request);

                if (result is null)
                {
                    MessageBox.Show(
                        "Form alanı güncellenemedi.",
                        "NexLibrary",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }
            }

            DialogResult = DialogResult.OK;
            Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"Kayıt sırasında hata oluştu:\n{ex.Message}",
                "NexLibrary",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
        finally
        {
            btnSave.Enabled = true;
        }
    }

    private bool ValidateForm()
    {
        if (string.IsNullOrWhiteSpace(txtAlanAdi.Text))
        {
            MessageBox.Show(
                "Alan adı zorunludur.",
                "NexLibrary",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            txtAlanAdi.Focus();
            return false;
        }

        if (string.IsNullOrWhiteSpace(txtAlanKodu.Text))
        {
            MessageBox.Show(
                "Alan kodu zorunludur.",
                "NexLibrary",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            txtAlanKodu.Focus();
            return false;
        }

        if (cmbAlanTipi.SelectedItem is null)
        {
            MessageBox.Show(
                "Alan tipi seçilmelidir.",
                "NexLibrary",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            cmbAlanTipi.Focus();
            return false;
        }

        var min = GetNullableInt(numMinKarakter);
        var max = GetNullableInt(numMaxKarakter);

        if (min.HasValue && max.HasValue && min.Value > max.Value)
        {
            MessageBox.Show(
                "Minimum karakter, maksimum karakterden büyük olamaz.",
                "NexLibrary",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            return false;
        }

        return true;
    }

    private static int? GetNullableInt(NumericUpDown numericUpDown)
    {
        return numericUpDown.Value <= 0
            ? null
            : Convert.ToInt32(numericUpDown.Value);
    }

    private static string? GetNullableText(TextBox textBox)
    {
        return string.IsNullOrWhiteSpace(textBox.Text)
            ? null
            : textBox.Text.Trim();
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }
}