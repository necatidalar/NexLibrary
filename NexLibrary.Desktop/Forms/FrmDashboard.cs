using NexLibrary.Contracts.Dashboard;
using NexLibrary.Desktop.Services;

namespace NexLibrary.Desktop.Forms;

public partial class FrmDashboard : Form
{
    private readonly DashboardApiService _dashboardApiService;

    public FrmDashboard(DashboardApiService dashboardApiService)
    {
        InitializeComponent();

        _dashboardApiService = dashboardApiService;
    }

    private async void FrmDashboard_Load(object sender, EventArgs e)
    {
        await LoadDashboardAsync();
    }

    private async Task LoadDashboardAsync()
    {
        try
        {
            btnRefresh.Enabled = false;

            var summary = await _dashboardApiService.GetSummaryAsync();

            if (summary is null)
            {
                MessageBox.Show(
                    "Dashboard verileri alınamadı.",
                    "NexLibrary",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            BindCards(summary);
            BindRecentLoans(summary.SonOduncler);

            lblLastUpdate.Text = $"Son güncelleme: {DateTime.Now:dd.MM.yyyy HH:mm:ss}";
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"Dashboard yüklenirken hata oluştu:\n{ex.Message}",
                "NexLibrary",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
        finally
        {
            btnRefresh.Enabled = true;
        }
    }

    private void BindCards(DashboardSummaryResponse summary)
    {
        flpCards.Controls.Clear();

        AddCard("Toplam Kitap", summary.ToplamKitap.ToString(), "Aktif kitap sayısı");
        AddCard("Toplam Üye", summary.ToplamUye.ToString(), "Aktif üye sayısı");
        AddCard("Toplam Kopya", summary.ToplamKopya.ToString(), "Fiziksel kitap adedi");
        AddCard("Müsait Kopya", summary.MusaitKopya.ToString(), "Ödünç verilebilir");
        AddCard("Ödünçte", summary.OdunctekiKopya.ToString(), "Şu an üyelerde");
        AddCard("Geciken", summary.GecikenOdunc.ToString(), "İade tarihi geçmiş");
        AddCard("Bugün İade", summary.BugunIadeEdilen.ToString(), "Bugün iade alınan");
        AddCard("Son 7 Gün Ödünç", summary.Son7GunOdunc.ToString(), "Haftalık ödünç");
        AddCard("Son 7 Gün İade", summary.Son7GunIade.ToString(), "Haftalık iade");
        AddCard("Kayıp / Hasarlı", $"{summary.KayipKopya} / {summary.HasarliKopya}", "Sorunlu kopyalar");
    }

    private void AddCard(string title, string value, string description)
    {
        var panel = new Panel
        {
            Width = 180,
            Height = 95,
            Margin = new Padding(10),
            BackColor = Color.White,
            BorderStyle = BorderStyle.FixedSingle
        };

        var lblValue = new Label
        {
            Text = value,
            Font = new Font("Segoe UI", 18F, FontStyle.Bold),
            Location = new Point(12, 8),
            Size = new Size(155, 32),
            TextAlign = ContentAlignment.MiddleLeft
        };

        var lblTitle = new Label
        {
            Text = title,
            Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
            ForeColor = Color.FromArgb(40, 40, 40),
            Location = new Point(12, 43),
            Size = new Size(155, 22),
            TextAlign = ContentAlignment.MiddleLeft
        };

        var lblDescription = new Label
        {
            Text = description,
            Font = new Font("Segoe UI", 8F),
            ForeColor = Color.DimGray,
            Location = new Point(12, 65),
            Size = new Size(155, 20),
            TextAlign = ContentAlignment.MiddleLeft
        };

        panel.Controls.Add(lblValue);
        panel.Controls.Add(lblTitle);
        panel.Controls.Add(lblDescription);

        flpCards.Controls.Add(panel);
    }

    private void BindRecentLoans(List<RecentLoanSummaryResponse> recentLoans)
    {
        dgvRecentLoans.Columns.Clear();
        dgvRecentLoans.Rows.Clear();

        dgvRecentLoans.AutoGenerateColumns = false;
        dgvRecentLoans.AllowUserToAddRows = false;
        dgvRecentLoans.AllowUserToDeleteRows = false;
        dgvRecentLoans.ReadOnly = true;
        dgvRecentLoans.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvRecentLoans.MultiSelect = false;
        dgvRecentLoans.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

        dgvRecentLoans.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "Id",
            HeaderText = "ID",
            FillWeight = 40
        });

        dgvRecentLoans.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "KitapAdi",
            HeaderText = "Kitap",
            FillWeight = 180
        });

        dgvRecentLoans.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "Barkod",
            HeaderText = "Barkod",
            FillWeight = 100
        });

        dgvRecentLoans.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "UyeAdiSoyadi",
            HeaderText = "Üye",
            FillWeight = 150
        });

        dgvRecentLoans.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "VerilisTarihi",
            HeaderText = "Veriliş",
            FillWeight = 90
        });

        dgvRecentLoans.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "PlanlananIadeTarihi",
            HeaderText = "Planlanan İade",
            FillWeight = 110
        });

        dgvRecentLoans.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "IadeTarihi",
            HeaderText = "İade",
            FillWeight = 90
        });

        dgvRecentLoans.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "Durum",
            HeaderText = "Durum",
            FillWeight = 90
        });

        foreach (var item in recentLoans)
        {
            var rowIndex = dgvRecentLoans.Rows.Add();
            var row = dgvRecentLoans.Rows[rowIndex];

            row.Cells["Id"].Value = item.Id;
            row.Cells["KitapAdi"].Value = item.KitapAdi;
            row.Cells["Barkod"].Value = item.Barkod ?? "-";
            row.Cells["UyeAdiSoyadi"].Value = item.UyeAdiSoyadi;
            row.Cells["VerilisTarihi"].Value = item.VerilisTarihi.ToString("dd.MM.yyyy");
            row.Cells["PlanlananIadeTarihi"].Value = item.PlanlananIadeTarihi.ToString("dd.MM.yyyy");
            row.Cells["IadeTarihi"].Value = item.IadeTarihi?.ToString("dd.MM.yyyy") ?? "-";
            row.Cells["Durum"].Value = item.Durum;

            if (item.Durum == "Gecikti")
            {
                row.DefaultCellStyle.BackColor = Color.MistyRose;
                row.DefaultCellStyle.ForeColor = Color.DarkRed;
            }
            else if (item.Durum == "IadeEdildi")
            {
                row.DefaultCellStyle.BackColor = Color.Honeydew;
                row.DefaultCellStyle.ForeColor = Color.DarkGreen;
            }
            else if (item.Durum == "IptalEdildi")
            {
                row.DefaultCellStyle.BackColor = Color.Gainsboro;
                row.DefaultCellStyle.ForeColor = Color.DimGray;
            }
        }

        lblRecentLoansCount.Text = $"Son ödünç kayıtları: {recentLoans.Count}";
    }

    private async void btnRefresh_Click(object sender, EventArgs e)
    {
        await LoadDashboardAsync();
    }
}