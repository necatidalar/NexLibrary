using NexLibrary.Contracts.Books;
using NexLibrary.Desktop.Services;

namespace NexLibrary.Desktop.Forms;

public partial class FrmBookDetail : Form
{
    private readonly BookApiService _bookApiService;
    private readonly int _bookId;

    public FrmBookDetail(BookApiService bookApiService, int bookId)
    {
        InitializeComponent();

        _bookApiService = bookApiService;
        _bookId = bookId;
    }

    private async void FrmBookDetail_Load(object sender, EventArgs e)
    {
        await LoadBookDetailAsync();
    }

    private async Task LoadBookDetailAsync()
    {
        try
        {
            var detail = await _bookApiService.GetByIdAsync(_bookId);

            if (detail is null)
            {
                MessageBox.Show(
                    "Kitap detayı alınamadı.",
                    "NexLibrary",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                Close();
                return;
            }

            BindDetail(detail);
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"Kitap detayı yüklenirken hata oluştu:\n{ex.Message}",
                "NexLibrary",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    private void BindDetail(BookDetailResponse detail)
    {
        lblTitle.Text = detail.KitapAdi;
        txtKitapAdi.Text = detail.KitapAdi;
        txtAktifMi.Text = detail.AktifMi ? "Aktif" : "Pasif";
        txtOlusturmaTarihi.Text = detail.OlusturmaTarihi.ToString("dd.MM.yyyy HH:mm");
        txtGuncellemeTarihi.Text = detail.GuncellemeTarihi?.ToString("dd.MM.yyyy HH:mm") ?? "-";

        pnlDynamicFields.Controls.Clear();

        var y = 10;

        foreach (var field in detail.DinamikAlanlar)
        {
            var label = new Label
            {
                Text = field.AlanAdi,
                Location = new Point(15, y + 4),
                Size = new Size(170, 25),
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleLeft
            };

            var valueBox = new TextBox
            {
                Text = field.Deger ?? "-",
                Location = new Point(195, y),
                Size = new Size(360, 25),
                ReadOnly = true,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };

            pnlDynamicFields.Controls.Add(label);
            pnlDynamicFields.Controls.Add(valueBox);

            y += 38;
        }

        if (detail.DinamikAlanlar.Count == 0)
        {
            var emptyLabel = new Label
            {
                Text = "Bu kitap için dinamik alan değeri bulunmuyor.",
                Location = new Point(15, 15),
                Size = new Size(500, 25),
                ForeColor = Color.DimGray
            };

            pnlDynamicFields.Controls.Add(emptyLabel);
        }
    }

    private void btnClose_Click(object sender, EventArgs e)
    {
        Close();
    }
}