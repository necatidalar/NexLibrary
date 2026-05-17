namespace NexLibrary.Contracts.Permissions;

public static class PermissionCodes
{
    public const string DashboardView = "Dashboard.View";

    public const string BooksView = "Books.View";
    public const string BooksCreate = "Books.Create";
    public const string BooksEdit = "Books.Edit";
    public const string BooksDelete = "Books.Delete";

    public const string MembersView = "Members.View";
    public const string MembersCreate = "Members.Create";
    public const string MembersEdit = "Members.Edit";
    public const string MembersDelete = "Members.Delete";

    public const string LoansView = "Loans.View";
    public const string LoansCreate = "Loans.Create";
    public const string LoansReturn = "Loans.Return";
    public const string LoansCancel = "Loans.Cancel";

    public const string BookCopiesView = "BookCopies.View";
    public const string BookCopiesCreate = "BookCopies.Create";
    public const string BookCopiesEdit = "BookCopies.Edit";
    public const string BookCopiesDelete = "BookCopies.Delete";

    public const string ReportsView = "Reports.View";
    public const string ReportsPrint = "Reports.Print";
    public const string ReportsExport = "Reports.Export";

    public const string FormFieldsView = "FormFields.View";
    public const string FormFieldsEdit = "FormFields.Edit";

    public const string UsersView = "Users.View";
    public const string UsersCreate = "Users.Create";
    public const string UsersEdit = "Users.Edit";
    public const string UsersRoleManage = "Users.RoleManage";

    public const string PermissionsView = "Permissions.View";
    public const string PermissionsManage = "Permissions.Manage";
}