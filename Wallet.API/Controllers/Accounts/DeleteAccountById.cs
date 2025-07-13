using Microsoft.AspNetCore.Mvc;
using Wallet.Application.Interfaces;

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
        await _accountService.SoftDeleteAsync(id, cancellationToken);
        return Ok();
    }
}