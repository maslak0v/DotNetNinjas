namespace FinancialTracker.Services.AuthorizeApi.Presentation.Helpers
{
    public enum Enum_AuthPolicy
    {
        CanAccess_AllAuthUsers,
        CanAccess_AdminAndSuperUser,
        CanAccess_SuperUserOnly,
        CanAccess_AdminAndUser,
    }
}
