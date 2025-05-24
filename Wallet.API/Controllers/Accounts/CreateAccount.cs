using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Wallet.API.Models.Accounts;
using Wallet.Domain.Entities;
using Wallet.Domain.Interfaces;

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
    public async Task<IActionResult> Create([FromBody] AccountRequest аccountRequest, CancellationToken cancellationToken)
    {
        var account = _mapper.Map<Account>(аccountRequest);
        await _accountService.AddAsync(account, cancellationToken);
        return Ok();
    }
}