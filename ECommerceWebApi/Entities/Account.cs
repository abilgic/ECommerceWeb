using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Principal;

namespace ECommerceWebApi.Entities
{
    [Table("Accounts")]
    public class Account
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(25)]
        public string Username { get; set; }
        [StringLength(50)]
        public string Password { get; set; }
        [StringLength(50)]
        public string CompanyName { get; set; }
        [StringLength(50)]
        public string ContactName { get; set; }
        [StringLength(50)]
        [EmailAddress]
        public string ContactEmail { get; set; }
        public bool isBlocked {get; set;}
        public bool isApplyment { get; set; }
        public AccountType Type {get; set;}

        public virtual List<Product> Products { get; set; }
        public virtual List<Cart> Carts { get; set; }
        public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    }
    public enum AccountType
    {
        Member = 1,
        Admin = 2,
        Merchant =3
    }

}
