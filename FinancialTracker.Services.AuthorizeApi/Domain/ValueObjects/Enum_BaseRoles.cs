namespace FinancialTracker.Services.AuthorizeApi.Domain.ValueObjects
{
    [Flags]
    public enum Enum_BaseRoles: byte
    {
        SUPERUSER = 1 << 0,
        ADMIN     = 1 << 1,
        USER      = 1 << 2,

        AllRoles = SUPERUSER | ADMIN | USER,
        SuperUserAndAdmin = SUPERUSER | ADMIN,
        AdminAndUser = ADMIN | USER,
    }
}
