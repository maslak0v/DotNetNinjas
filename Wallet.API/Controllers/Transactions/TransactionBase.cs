using Microsoft.AspNetCore.Mvc;

namespace Wallet.API.Controllers.Transactions;
[ApiController]
[Route("api/account/transaction")]
[ApiExplorerSettings(GroupName = "Управление транзакциями")]
public class TransactionBase : ControllerBase
{
}