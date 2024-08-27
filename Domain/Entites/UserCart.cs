namespace Domain.Entites
{
    public enum PaymentMethod { Delivery , Online }
	public enum PaymentStatus { Pending, Completed, Failed }
	public class UserCart
	{
		public int Id { get; set; }
		public decimal TotalPrice { get; set; }
		public string? UserId { get; set; }
		public AppUser? User { get; set; }
		public PaymentMethod PaymentMethod { get; set; }
		public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Pending;

		public virtual ICollection<Sold>? Sold { get; set; }
	}
}
