using FinancialTracker.Services.AuthorizeApi.Application.Features;
using FinancialTracker.Services.AuthorizeApi.Domain.Interfaces;
using System.Windows.Input;

namespace FinancialTracker.Services.AuthorizeApi.Application.UseCases.Interfaces
{
    public interface ILogoutUseCase : ICommandAsync<OperationResult>
    {
    }
}