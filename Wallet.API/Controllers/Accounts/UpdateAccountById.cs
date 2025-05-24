using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Wallet.API.Models.Accounts;
using Wallet.Domain.Entities;
using Wallet.Domain.Interfaces;

namespace Wallet.API.Controllers.Accounts;

public class UpdateAccountById : AccountBase
{
    private readonly IAccountService _accountService;
    private readonly IMapper _mapper;

    public UpdateAccountById(IAccountService accountService, IMapper mapper)
    {
        _accountService = accountService;
        _mapper = mapper;
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateAccountRequest accountRequest, CancellationToken cancellationToken)
    {
        // todo Добавить валидацию
        var account = _mapper.Map<Account>(accountRequest);
        account.AccountId = id;

        await _accountService.UpdateAsync(account, cancellationToken);

        return NoContent();
    }
}