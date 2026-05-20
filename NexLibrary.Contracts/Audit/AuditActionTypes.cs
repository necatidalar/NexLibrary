namespace NexLibrary.Contracts.Audit;

public static class AuditActionTypes
{
    public const string UserLoginSuccess = "User.Login.Success";

    public const string UserLoginFailed = "User.Login.Failed";

    public const string ApiClientTokenSuccess = "ApiClient.Token.Success";

    public const string ApiClientTokenFailed = "ApiClient.Token.Failed";

    public const string PermissionsUpdate = "Permissions.Update";

    public const string BookCreate = "Book.Create";

    public const string BookUpdate = "Book.Update";

    public const string BookDelete = "Book.Delete";

    public const string MemberCreate = "Member.Create";

    public const string MemberUpdate = "Member.Update";

    public const string MemberDelete = "Member.Delete";

    public const string LoanCreate = "Loan.Create";

    public const string LoanReturn = "Loan.Return";

    public const string LoanCancel = "Loan.Cancel";
}