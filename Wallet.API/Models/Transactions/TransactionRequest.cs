using System.ComponentModel;
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
       [DefaultValue(12)]
       public int CategoryId { get; set; }
       [Required]
       public decimal Amount { get; set; }
      
       public string? Comment { get; set; } = null;
       public string? Image { get; set; } = null;
       public string? Tag { get; set; } =  null;
}