
using FinancialTracker.Services.AuthorizeApi.Domain.Interfaces.Requests;
using FinancialTracker.Services.AuthorizeApi.Infrastructure.Contracts;

namespace FinancialTracker.Services.AuthorizeApi.Tests.Helpers
{
    public static class RequestCreator
    {
        public static IUserRegisterRequest CreateUserRegisterGoodRequest()
          => new UserRegisterRequest(
              "mail@mail.ru",
              "derParol1$",
              "derParol1$",
              "uniqueName");

        public static IUserRegisterRequest CreateUserRegisterBadRequest()
            => new UserRegisterRequest(
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty);
    }
}
