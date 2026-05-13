using NexLibrary.Contracts.Members;
using NexLibrary.Desktop.Services;

namespace NexLibrary.Desktop.Forms;

public partial class FrmMembers : Form
{
    private readonly MemberApiService _memberApiService;
    private readonly FormFieldApiService _formFieldApiService;

    private List<MemberListResponse> _members = new();
    private int _selectedMemberId;

    public FrmMembers(
        MemberApiService memberApiService,
        FormFieldApiService formFieldApiService)
    {
        InitializeComponent();

        _memberApiService = memberApiService;
        _formFieldApiService = formFieldApiService;
    }

    private async void FrmMembers_Load(object sender, EventArgs e)
    {
        await LoadMembersAsync();
    }

    private async Task LoadMembersAsync()
    {
        try
        {
            btnRefresh.Enabled = false;

            var result = await _memberApiService.GetPagedAsync(
                1,
                100,
                txtSearch.Text.Trim());

            _members = result?.Items ?? new List<MemberListResponse>();

            BindGrid();
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"Üyeler yüklenirken hata oluştu:\n{ex.Message}",
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
        dgvMembers.Columns.Clear();
        dgvMembers.Rows.Clear();

        dgvMembers.AutoGenerateColumns = false;
        dgvMembers.AllowUserToAddRows = false;
        dgvMembers.AllowUserToDeleteRows = false;
        dgvMembers.ReadOnly = true;
        dgvMembers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvMembers.MultiSelect = false;
        dgvMembers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

        dgvMembers.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "Id",
            HeaderText = "ID",
            FillWeight = 40
        });

        dgvMembers.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "UyeAdiSoyadi",
            HeaderText = "Üye Adı Soyadı",
            FillWeight = 180
        });

        var dynamicColumnNames = _members
            .SelectMany(x => x.DinamikAlanlar.Keys)
            .Distinct()
            .ToList();

        foreach (var columnName in dynamicColumnNames)
        {
            dgvMembers.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = columnName,
                HeaderText = columnName,
                FillWeight = 120
            });
        }

        dgvMembers.Columns.Add(new DataGridViewCheckBoxColumn
        {
            Name = "AktifMi",
            HeaderText = "Aktif",
            FillWeight = 50
        });

        foreach (var member in _members)
        {
            var rowIndex = dgvMembers.Rows.Add();
            var row = dgvMembers.Rows[rowIndex];

            row.Cells["Id"].Value = member.Id;
            row.Cells["UyeAdiSoyadi"].Value = member.UyeAdiSoyadi;
            row.Cells["AktifMi"].Value = member.AktifMi;

            foreach (var dynamicValue in member.DinamikAlanlar)
            {
                if (dgvMembers.Columns.Contains(dynamicValue.Key))
                {
                    row.Cells[dynamicValue.Key].Value = dynamicValue.Value;
                }
            }
        }

        lblCount.Text = $"Toplam üye: {_members.Count}";
        _selectedMemberId = 0;
    }

    private async void btnRefresh_Click(object sender, EventArgs e)
    {
        await LoadMembersAsync();
    }

    private async void btnSearch_Click(object sender, EventArgs e)
    {
        await LoadMembersAsync();
    }

    private async void txtSearch_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Enter)
        {
            e.SuppressKeyPress = true;
            await LoadMembersAsync();
        }
    }

    private async void btnAdd_Click(object sender, EventArgs e)
    {
        using var form = new FrmMemberEdit(_memberApiService, _formFieldApiService);

        if (form.ShowDialog(this) == DialogResult.OK)
        {
            await LoadMembersAsync();
        }
    }

    private async void btnEdit_Click(object sender, EventArgs e)
    {
        if (_selectedMemberId <= 0)
        {
            MessageBox.Show(
                "Lütfen düzenlenecek üyeyi seçin.",
                "NexLibrary",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            return;
        }

        using var form = new FrmMemberEdit(
            _memberApiService,
            _formFieldApiService,
            _selectedMemberId);

        if (form.ShowDialog(this) == DialogResult.OK)
        {
            await LoadMembersAsync();
        }
    }

    private void btnDetail_Click(object sender, EventArgs e)
    {
        OpenDetailForm();
    }

    private async void btnDelete_Click(object sender, EventArgs e)
    {
        if (_selectedMemberId <= 0)
        {
            MessageBox.Show(
                "Lütfen pasif yapılacak üyeyi seçin.",
                "NexLibrary",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            return;
        }

        var confirm = MessageBox.Show(
            "Seçili üye pasif hale getirilecek. Devam edilsin mi?",
            "NexLibrary",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);

        if (confirm != DialogResult.Yes)
        {
            return;
        }

        var success = await _memberApiService.DeleteAsync(_selectedMemberId);

        if (!success)
        {
            MessageBox.Show(
                "Üye pasif hale getirilemedi.",
                "NexLibrary",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
            return;
        }

        await LoadMembersAsync();
    }

    private void OpenDetailForm()
    {
        if (_selectedMemberId <= 0)
        {
            MessageBox.Show(
                "Lütfen detayını görmek istediğiniz üyeyi seçin.",
                "NexLibrary",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            return;
        }

        using var form = new FrmMemberDetail(_memberApiService, _selectedMemberId);
        form.ShowDialog(this);
    }

    private void dgvMembers_SelectionChanged(object sender, EventArgs e)
    {
        _selectedMemberId = 0;

        if (dgvMembers.CurrentRow?.Cells["Id"].Value is null)
        {
            return;
        }

        int.TryParse(dgvMembers.CurrentRow.Cells["Id"].Value.ToString(), out _selectedMemberId);
    }

    private void dgvMembers_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex >= 0)
        {
            OpenDetailForm();
        }
    }
}