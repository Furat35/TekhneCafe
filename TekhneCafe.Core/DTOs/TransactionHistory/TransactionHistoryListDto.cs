namespace TekhneCafe.Core.DTOs.Transaction
{
    public class TransactionHistoryListDto
    {
        public Guid? OrderId { get; set; }
        public List<string> Products { get; set; } = new List<string>();
        public string TransactionType { get; set; }
        public float Amount { get; set; }
        public string Personel { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
