using NexLibrary.Contracts.DynamicForms;
using NexLibrary.Contracts.Members;
using NexLibrary.Desktop.Services;

namespace NexLibrary.Desktop.Forms;

public partial class FrmMemberEdit : Form
{
    private readonly MemberApiService _memberApiService;
    private readonly FormFieldApiService _formFieldApiService;

    private readonly Dictionary<string, Control> _dynamicControls = new();
    private List<FormFieldResponse> _fields = new();

    public FrmMemberEdit(
        MemberApiService memberApiService,
        FormFieldApiService formFieldApiService)
    {
        InitializeComponent();

        _memberApiService = memberApiService;
        _formFieldApiService = formFieldApiService;
    }

    private async void FrmMemberEdit_Load(object sender, EventArgs e)
    {
        var design = await _formFieldApiService.GetFormDesignAsync("Uyeler");

        if (design is null)
        {
            MessageBox.Show("Üye form tasarımı alınamadı.");
            Close();
            return;
        }

        _fields = design.Alanlar.OrderBy(x => x.SiraNo).ToList();

        CreateDynamicControls();
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
                Size = new Size(150, 25)
            };

            var textBox = new TextBox
            {
                Location = new Point(175, y),
                Size = new Size(330, 25),
                PlaceholderText = field.Placeholder ?? string.Empty,
                MaxLength = field.MaksimumKarakter ?? 0
            };

            pnlDynamicFields.Controls.Add(label);
            pnlDynamicFields.Controls.Add(textBox);

            _dynamicControls[field.AlanKodu] = textBox;

            y += 40;
        }
    }

    private async void btnSave_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtUyeAdiSoyadi.Text))
        {
            MessageBox.Show("Üye adı soyadı zorunludur.");
            return;
        }

        var request = new MemberCreateRequest
        {
            UyeAdiSoyadi = txtUyeAdiSoyadi.Text.Trim(),
            DinamikAlanlar = GetDynamicValues()
        };

        var result = await _memberApiService.CreateAsync(request);

        if (result is null)
        {
            MessageBox.Show("Üye kaydedilemedi.");
            return;
        }

        DialogResult = DialogResult.OK;
        Close();
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
                Deger = control.Text
            });
        }

        return values;
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }
}