using System.ComponentModel.DataAnnotations;
using Microsoft.OpenApi.Models;

namespace Wallet.API.Models.Transactions;

public class TransactionRequest
{
       [Required]
       public Guid AccountId { get; set; }
       [Required]
       public OperationType OperationType { get; set; }
       [Required]
       public int CategoryId { get; set; }
       [Required]
       public decimal Amount { get; set; }
       
       public string Comment { get; set; } = string.Empty;
       public string? Image { get; set; }
       public string? Tag { get; set; }
}