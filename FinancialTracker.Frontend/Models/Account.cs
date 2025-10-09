namespace FinancialTracker.Frontend.Models
{
	public class Account
	{
		public Guid AccountId { get; set; }
		public Guid UserId { get; set; }
		public string Name { get; set; }
		public double CurrentBalance { get; set; }
		public int Currency { get; set; }
		public string CreatedAt { get; set; }
		public string? UpdatedAt { get; set; }
	}
}
