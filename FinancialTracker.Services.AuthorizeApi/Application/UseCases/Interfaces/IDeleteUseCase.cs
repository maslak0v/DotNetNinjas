using FinancialTracker.Services.AuthorizeApi.Application.Features;
using FinancialTracker.Services.AuthorizeApi.Domain.Interfaces;

namespace FinancialTracker.Services.AuthorizeApi.Application.UseCases.Interfaces
{
    public interface IDeleteUseCase : ICommandAsync<OperationResult>
    {

    }
}
