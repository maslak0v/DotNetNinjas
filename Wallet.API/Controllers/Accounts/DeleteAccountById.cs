using Microsoft.AspNetCore.Mvc;
using Wallet.Domain.Interfaces;

namespace Wallet.API.Controllers.Accounts;

public class DeleteAccountById : AccountBase
{
    private readonly IAccountRepository _accountRepository;
    private readonly IAccountService _accountService;

    public DeleteAccountById(IAccountRepository accountRepository, IAccountService accountService)
    {
        _accountRepository = accountRepository;
        _accountService = accountService;
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteAccountByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var account = await _accountRepository.GetByIdAsync(id, cancellationToken);

        if (account == null)
        {
            return NotFound();
        }

        await _accountRepository.SoftDelete(account, cancellationToken);
        
        return Ok();

    }
}