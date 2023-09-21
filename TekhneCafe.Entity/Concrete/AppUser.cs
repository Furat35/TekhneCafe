using TekhneCafe.Core.Entities.Concrete;

namespace TekhneCafe.Entity.Concrete
{
    public class AppUser : BaseEntity
    {
        public Guid LdapId { get; set; }
        public string FullName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string? Department { get; set; }
        public string? Phone { get; set; }
        public string? InternalPhone { get; set; }
        public string? ImagePath { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public float Wallet { get; set; }
        public virtual ICollection<TransactionHistory>? TransactionHistories { get; set; }
        public virtual ICollection<Notification>? Notifications { get; set; }
    }
}

