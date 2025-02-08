using System.ComponentModel.DataAnnotations;

namespace ECommerceWebApi.Entities
{
    public class Payment
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public decimal TotalPrice { get; set; }
        [StringLength(50)]
        public string Type { get; set; }
        [Required]
        [StringLength(160)]
        public string InvoiceAddress { get; set; }
        [Required]
        [StringLength(160)]
        public string ShippedAddress { get; set; }
        public bool IsCompleted { get; set; }
        [StringLength(50)]
        public string TransactionId { get; set; }

        // Nullable Foreign Keys
        public int? CartId { get; set; }
        public int? AccountId { get; set; }

        // Navigation Properties
        public virtual Cart Cart { get; set; }
        public virtual Account Account { get; set; }

    }
}
