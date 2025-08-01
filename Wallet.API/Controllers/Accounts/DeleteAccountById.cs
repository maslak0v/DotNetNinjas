using Microsoft.AspNetCore.Mvc;
using Wallet.API.Helpers;
using Wallet.Application.Interfaces.Services;

namespace Wallet.API.Controllers.Accounts;

public class DeleteAccountById : AccountBase
{
    private readonly IAccountService _accountService;

    public DeleteAccountById(IAccountService accountService)
    {
        _accountService = accountService;
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteAccountByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var result = await _accountService.SoftDeleteAsync(id, cancellationToken);
        var response = ResponseCreator.Create(result);
        return response;
    }
}