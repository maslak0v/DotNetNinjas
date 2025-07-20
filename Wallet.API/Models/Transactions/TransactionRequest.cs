using System.ComponentModel.DataAnnotations;
using Wallet.Domain.Enums;


namespace Wallet.API.Models.Transactions;

public class TransactionRequest
{
       [Required]
       public Guid AccountId { get; set; }

       [Required]
       [Range(1, byte.MaxValue, ErrorMessage = "Недопустимый тип операции (OperationType)")]
       public OperationType OperationType { get; set; }
       [Required]
       public int CategoryId { get; set; }
       [Required]
       public decimal Amount { get; set; }
       
       public string Comment { get; set; } = string.Empty;
       public string? Image { get; set; }
       public string? Tag { get; set; }
}