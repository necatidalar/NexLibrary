using NexLibrary.Contracts.BookCopies;
using NexLibrary.Desktop.Services;

namespace NexLibrary.Desktop.Forms;

public partial class FrmBookCopyEdit : Form
{
    private readonly BookCopyApiService _bookCopyApiService;
    private readonly int _bookId;
    private readonly string _bookName;

    public FrmBookCopyEdit(
        BookCopyApiService bookCopyApiService,
        int bookId,
        string bookName)
    {
        InitializeComponent();

        _bookCopyApiService = bookCopyApiService;
        _bookId = bookId;
        _bookName = bookName;
    }

    private void FrmBookCopyEdit_Load(object sender, EventArgs e)
    {
        txtKitapAdi.Text = _bookName;
        txtBarkod.Text = $"BK-{_bookId}-{DateTime.Now:yyyyMMddHHmmss}";
    }

    private async void btnSave_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtBarkod.Text))
        {
            MessageBox.Show(
                "Barkod zorunludur.",
                "NexLibrary",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);

            txtBarkod.Focus();
            return;
        }

        btnSave.Enabled = false;

        try
        {
            var request = new BookCopyCreateRequest
            {
                KitapId = _bookId,
                Barkod = txtBarkod.Text.Trim(),
                DemirbasNo = string.IsNullOrWhiteSpace(txtDemirbasNo.Text)
                    ? null
                    : txtDemirbasNo.Text.Trim(),
                Aciklama = string.IsNullOrWhiteSpace(txtAciklama.Text)
                    ? null
                    : txtAciklama.Text.Trim()
            };

            var result = await _bookCopyApiService.CreateAsync(request);

            if (result is null)
            {
                MessageBox.Show(
                    "Kitap kopyası oluşturulamadı. Barkod daha önce kullanılmış olabilir.",
                    "NexLibrary",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            MessageBox.Show(
                "Kitap kopyası başarıyla oluşturuldu.",
                "NexLibrary",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);

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

    private void btnCancel_Click(object sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }
}