using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Wallet.API.Models.Accounts;
using Wallet.Domain.Interfaces;

namespace Wallet.API.Controllers.Accounts;

public class GetAllAccountsByUserId : AccountBase
{
    private readonly IAccountService _accountService;
    private readonly IMapper _mapper;

    public GetAllAccountsByUserId(IAccountService accountService, IMapper mapper)
    {
        _accountService = accountService;
        _mapper = mapper;
    }

    [HttpGet("{id:guid}/all")]
    public async Task<IActionResult> Update(Guid id, CancellationToken cancellationToken)
    {
        var accounts = await _accountService.GetAllByUserIdAsync(id, cancellationToken);
        var response = _mapper.Map<List<AccountResponse>>(accounts);
        return Ok(response);
    }
}