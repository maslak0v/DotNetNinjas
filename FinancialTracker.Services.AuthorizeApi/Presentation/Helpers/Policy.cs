using FinancialTracker.Services.AuthorizeApi.Domain.ValueObjects;

namespace FinancialTracker.Services.AuthorizeApi.Presentation.Helpers
{
    internal record PolicyType(string Name, string[] Roles);
    internal class Policy
    {
        internal static PolicyType CreatePolicy(Enum_BaseRoles roles)
        {
            (string name, string[] listRoles) = roles switch
            {
                Enum_BaseRoles.SUPERUSER => (
                    nameof(Enum_AuthPolicy.CanAccess_SuperUserOnly),
                    [nameof(Enum_BaseRoles.SUPERUSER)]),

                Enum_BaseRoles.SuperUserAndAdmin => (
                    nameof(Enum_AuthPolicy.CanAccess_AdminAndSuperUser),
                    [nameof(Enum_BaseRoles.SUPERUSER), nameof(Enum_BaseRoles.ADMIN)]),

                Enum_BaseRoles.AllRoles => (
                 nameof(Enum_AuthPolicy.CanAccess_AllAuthUsers),
                    [nameof(Enum_BaseRoles.SUPERUSER), nameof(Enum_BaseRoles.ADMIN), nameof(Enum_BaseRoles.USER)]),

                Enum_BaseRoles.AdminAndUser => (
                    nameof(Enum_AuthPolicy.CanAccess_AdminAndUser),
                    new string[]{ nameof(Enum_BaseRoles.USER), nameof(Enum_BaseRoles.ADMIN) }),

                _ => throw new ArgumentException("Unknown policy for roles")
            };
            return new PolicyType(name, listRoles);
        }
    }
}