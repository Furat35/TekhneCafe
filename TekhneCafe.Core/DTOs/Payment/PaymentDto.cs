namespace TekhneCafe.Core.DTOs.Payment
{
    public class PaymentDto
    {
        public string UserId { get; set; }
        public float Amount { get; set; }
        public string? Description { get; set; }
    }
}
