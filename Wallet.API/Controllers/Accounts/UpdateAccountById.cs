using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Wallet.API.Models.Accounts;
using Wallet.Application.Dto.Accounts;
using Wallet.Application.Interfaces.Services;

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
        var account = _mapper.Map<AccountDto>(accountRequest);
        account.AccountId = id;

        var result = await _accountService.UpdateAsync(account, cancellationToken);

        return StatusCode((int)result.StatusCode, result.Message);
    }
}