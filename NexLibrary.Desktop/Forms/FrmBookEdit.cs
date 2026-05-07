using NexLibrary.Contracts.Books;
using NexLibrary.Contracts.DynamicForms;
using NexLibrary.Desktop.Services;

namespace NexLibrary.Desktop.Forms;

public partial class FrmBookEdit : Form
{
    private readonly BookApiService _bookApiService;
    private readonly FormFieldApiService _formFieldApiService;
    private readonly int? _bookId;

    private readonly Dictionary<string, Control> _dynamicControls = new();
    private List<FormFieldResponse> _fields = new();

    public FrmBookEdit(
        BookApiService bookApiService,
        FormFieldApiService formFieldApiService,
        int? bookId = null)
    {
        InitializeComponent();

        _bookApiService = bookApiService;
        _formFieldApiService = formFieldApiService;
        _bookId = bookId;
    }

    private async void FrmBookEdit_Load(object sender, EventArgs e)
    {
        Text = _bookId.HasValue ? "Kitap Düzenle" : "Yeni Kitap";
        lblTitle.Text = _bookId.HasValue ? "Kitap Düzenle" : "Yeni Kitap";

        await LoadFormAsync();
    }

    private async Task LoadFormAsync()
    {
        var design = await _formFieldApiService.GetFormDesignAsync("Kitaplar");

        if (design is null)
        {
            MessageBox.Show(
                "Form tasarımı API'den alınamadı.",
                "NexLibrary",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);

            DialogResult = DialogResult.Cancel;
            Close();
            return;
        }

        _fields = design.Alanlar
            .OrderBy(x => x.SiraNo)
            .ToList();

        CreateDynamicControls();

        if (_bookId.HasValue)
        {
            await LoadBookDetailAsync(_bookId.Value);
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

    private async Task LoadBookDetailAsync(int bookId)
    {
        var detail = await _bookApiService.GetByIdAsync(bookId);

        if (detail is null)
        {
            MessageBox.Show(
                "Kitap detayı alınamadı.",
                "NexLibrary",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
            return;
        }

        txtKitapAdi.Text = detail.KitapAdi;
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
                numericUpDown.Value = Math.Min(numericUpDown.Maximum, Math.Max(numericUpDown.Minimum, decimalValue));
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
            checkBox.Checked = value?.Equals("true", StringComparison.OrdinalIgnoreCase) == true
                               || value == "1"
                               || value?.Equals("evet", StringComparison.OrdinalIgnoreCase) == true;
        }
    }

    private async void btnSave_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtKitapAdi.Text))
        {
            MessageBox.Show(
                "Kitap adı zorunludur.",
                "NexLibrary",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            return;
        }

        btnSave.Enabled = false;

        try
        {
            if (_bookId.HasValue)
            {
                var request = new BookUpdateRequest
                {
                    Id = _bookId.Value,
                    KitapAdi = txtKitapAdi.Text.Trim(),
                    AktifMi = chkAktifMi.Checked,
                    DinamikAlanlar = GetDynamicFieldValues()
                };

                var result = await _bookApiService.UpdateAsync(_bookId.Value, request);

                if (result is null)
                {
                    MessageBox.Show(
                        "Kitap güncellenemedi.",
                        "NexLibrary",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }
            }
            else
            {
                var request = new BookCreateRequest
                {
                    KitapAdi = txtKitapAdi.Text.Trim(),
                    DinamikAlanlar = GetDynamicFieldValues()
                };

                var result = await _bookApiService.CreateAsync(request);

                if (result is null)
                {
                    MessageBox.Show(
                        "Kitap kaydedilemedi. API doğrulama hatası olabilir.",
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

    private List<DynamicFieldValueRequest> GetDynamicFieldValues()
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