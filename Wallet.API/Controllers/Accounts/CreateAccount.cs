using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Wallet.API.Models.Accounts;
using Wallet.Application.Dto.Accounts;
using Wallet.Application.Interfaces.Services;

namespace Wallet.API.Controllers.Accounts;

public class CreateAccount : AccountBase
{
    private readonly IAccountService _accountService;
    private readonly IMapper _mapper;

    public CreateAccount(IMapper mapper, IAccountService accountService)
    {
        _mapper = mapper;
        _accountService = accountService;
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create([FromBody] AccountRequest accountRequest, CancellationToken cancellationToken)
    {
        var account = _mapper.Map<AccountDto>(accountRequest);
        
        await _accountService.AddAsync(account, cancellationToken);
        return NoContent();
    }
}