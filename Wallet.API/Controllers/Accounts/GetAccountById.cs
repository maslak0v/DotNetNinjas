using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Wallet.API.Models.Accounts;
using Wallet.Domain.Interfaces;

namespace Wallet.API.Controllers.Accounts;

public class GetAccountById : AccountBase
{
    private readonly IAccountService _accountService;
    private readonly IMapper _mapper;

    public GetAccountById(IMapper mapper, IAccountService accountService)
    {
        _mapper = mapper;
        _accountService = accountService;
    }
    
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var category = await _accountService.GetByIdAsync(id, cancellationToken);
        var response = _mapper.Map<AccountResponse>(category);
        return Ok(response);
    }
}