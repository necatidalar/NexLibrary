using NexLibrary.Contracts.DynamicForms;
using NexLibrary.Contracts.Members;
using NexLibrary.Desktop.Services;

namespace NexLibrary.Desktop.Forms;

public partial class FrmMemberEdit : Form
{
    private readonly MemberApiService _memberApiService;
    private readonly FormFieldApiService _formFieldApiService;
    private readonly int? _memberId;

    private readonly Dictionary<string, Control> _dynamicControls = new();
    private List<FormFieldResponse> _fields = new();

    public FrmMemberEdit(
        MemberApiService memberApiService,
        FormFieldApiService formFieldApiService,
        int? memberId = null)
    {
        InitializeComponent();

        _memberApiService = memberApiService;
        _formFieldApiService = formFieldApiService;
        _memberId = memberId;
    }

    private async void FrmMemberEdit_Load(object sender, EventArgs e)
    {
        Text = _memberId.HasValue ? "Üye Düzenle" : "Yeni Üye";
        lblTitle.Text = _memberId.HasValue ? "Üye Düzenle" : "Yeni Üye";

        await LoadFormAsync();
    }

    private async Task LoadFormAsync()
    {
        var design = await _formFieldApiService.GetFormDesignAsync("Uyeler");

        if (design is null)
        {
            MessageBox.Show(
                "Üye form tasarımı alınamadı.",
                "NexLibrary",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);

            Close();
            return;
        }

        _fields = design.Alanlar.OrderBy(x => x.SiraNo).ToList();

        CreateDynamicControls();

        if (_memberId.HasValue)
        {
            await LoadMemberDetailAsync(_memberId.Value);
        }
    }

    private void CreateDynamicControls()
    {
        pnlDynamicFields.Controls.Clear();
        _dynamicControls.Clear();

        var y = 10;

        foreach (var field in _fields.Where(x => !x.SistemAlaniMi && x.FormdaGorunsunMu && x.AktifMi))
        {
            var label = new Label
            {
                Text = field.ZorunluMu ? $"{field.AlanAdi} *" : field.AlanAdi,
                Location = new Point(15, y + 4),
                Size = new Size(150, 25),
                TextAlign = ContentAlignment.MiddleLeft
            };

            var control = CreateControlByFieldType(field);
            control.Location = new Point(175, y);
            control.Size = new Size(330, 25);
            control.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

            pnlDynamicFields.Controls.Add(label);
            pnlDynamicFields.Controls.Add(control);

            _dynamicControls[field.AlanKodu] = control;

            y += field.AlanTipi == "UzunMetin" ? 85 : 40;
        }
    }

    private static Control CreateControlByFieldType(FormFieldResponse field)
    {
        return field.AlanTipi switch
        {
            "Sayi" or "OndalikliSayi" or "Para" => new NumericUpDown
            {
                Minimum = 0,
                Maximum = 100000000,
                DecimalPlaces = field.AlanTipi == "Sayi" ? 0 : 2
            },

            "Tarih" or "TarihSaat" => new DateTimePicker
            {
                Format = field.AlanTipi == "Tarih"
                    ? DateTimePickerFormat.Short
                    : DateTimePickerFormat.Custom,
                CustomFormat = field.AlanTipi == "TarihSaat"
                    ? "dd.MM.yyyy HH:mm"
                    : null
            },

            "EvetHayir" => new CheckBox
            {
                Text = "Evet"
            },

            "UzunMetin" => new TextBox
            {
                Multiline = true,
                Height = 70,
                ScrollBars = ScrollBars.Vertical,
                MaxLength = field.MaksimumKarakter ?? 0
            },

            _ => new TextBox
            {
                PlaceholderText = field.Placeholder ?? string.Empty,
                MaxLength = field.MaksimumKarakter ?? 0
            }
        };
    }

    private async Task LoadMemberDetailAsync(int memberId)
    {
        var detail = await _memberApiService.GetByIdAsync(memberId);

        if (detail is null)
        {
            MessageBox.Show(
                "Üye detayı alınamadı.",
                "NexLibrary",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
            return;
        }

        txtUyeAdiSoyadi.Text = detail.UyeAdiSoyadi;
        chkAktifMi.Checked = detail.AktifMi;

        foreach (var value in detail.DinamikAlanlar)
        {
            if (!_dynamicControls.TryGetValue(value.AlanKodu, out var control))
            {
                continue;
            }

            SetControlValue(control, value.Deger);
        }
    }

    private static void SetControlValue(Control control, string? value)
    {
        if (control is TextBox textBox)
        {
            textBox.Text = value ?? string.Empty;
        }
        else if (control is NumericUpDown numericUpDown)
        {
            if (decimal.TryParse(value, out var decimalValue))
            {
                numericUpDown.Value = Math.Min(
                    numericUpDown.Maximum,
                    Math.Max(numericUpDown.Minimum, decimalValue));
            }
        }
        else if (control is DateTimePicker dateTimePicker)
        {
            if (DateTime.TryParse(value, out var dateValue))
            {
                dateTimePicker.Value = dateValue;
            }
        }
        else if (control is CheckBox checkBox)
        {
            checkBox.Checked =
                value?.Equals("true", StringComparison.OrdinalIgnoreCase) == true ||
                value == "1" ||
                value?.Equals("evet", StringComparison.OrdinalIgnoreCase) == true;
        }
    }

    private async void btnSave_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtUyeAdiSoyadi.Text))
        {
            MessageBox.Show(
                "Üye adı soyadı zorunludur.",
                "NexLibrary",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            return;
        }

        btnSave.Enabled = false;

        try
        {
            if (_memberId.HasValue)
            {
                var request = new MemberUpdateRequest
                {
                    Id = _memberId.Value,
                    UyeAdiSoyadi = txtUyeAdiSoyadi.Text.Trim(),
                    AktifMi = chkAktifMi.Checked,
                    DinamikAlanlar = GetDynamicValues()
                };

                var result = await _memberApiService.UpdateAsync(_memberId.Value, request);

                if (result is null)
                {
                    MessageBox.Show(
                        "Üye güncellenemedi.",
                        "NexLibrary",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }
            }
            else
            {
                var request = new MemberCreateRequest
                {
                    UyeAdiSoyadi = txtUyeAdiSoyadi.Text.Trim(),
                    DinamikAlanlar = GetDynamicValues()
                };

                var result = await _memberApiService.CreateAsync(request);

                if (result is null)
                {
                    MessageBox.Show(
                        "Üye kaydedilemedi.",
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

    private List<DynamicFieldValueRequest> GetDynamicValues()
    {
        var values = new List<DynamicFieldValueRequest>();

        foreach (var field in _fields.Where(x => !x.SistemAlaniMi && x.FormdaGorunsunMu && x.AktifMi))
        {
            if (!_dynamicControls.TryGetValue(field.AlanKodu, out var control))
            {
                continue;
            }

            values.Add(new DynamicFieldValueRequest
            {
                AlanKodu = field.AlanKodu,
                Deger = GetControlValue(control)
            });
        }

        return values;
    }

    private static string? GetControlValue(Control control)
    {
        return control switch
        {
            TextBox textBox => textBox.Text,
            NumericUpDown numericUpDown => numericUpDown.Value.ToString(),
            DateTimePicker dateTimePicker => dateTimePicker.Value.ToString("yyyy-MM-dd HH:mm:ss"),
            CheckBox checkBox => checkBox.Checked ? "true" : "false",
            _ => control.Text
        };
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }
}