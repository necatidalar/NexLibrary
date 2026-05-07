using NexLibrary.Desktop.Services;

namespace NexLibrary.Desktop;

public partial class Form1 : Form
{
    private readonly FormFieldApiService _formFieldApiService;

    public Form1(FormFieldApiService formFieldApiService)
    {
        InitializeComponent();

        _formFieldApiService = formFieldApiService;
    }

    protected override async void OnLoad(EventArgs e)
    {
        base.OnLoad(e);

        try
        {
            var formDesign = await _formFieldApiService.GetFormDesignAsync("Kitaplar");

            if (formDesign is null)
            {
                MessageBox.Show(
                    "API bağlantısı kuruldu ama form tasarımı alınamadı.",
                    "NexLibrary",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            MessageBox.Show(
                $"API bağlantısı başarılı.\nModül: {formDesign.ModulAdi}\nAlan sayısı: {formDesign.Alanlar.Count}",
                "NexLibrary",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"API bağlantı hatası:\n{ex.Message}",
                "NexLibrary",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }
}