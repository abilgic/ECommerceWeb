using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Core.Models
{
    public class ProductCreateModel
    {
        public string Name { get; set; }
        [StringLength(250)]
        public string Description { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal DiscountedPrice { get; set; }
        public bool Discontinued { get; set; }
        public int CategoryId { get; set; }
    }

    public class ProductModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal DiscountedPrice { get; set; }
        public bool Discontinued { get; set; }
        public int CategoryId { get; set; }
        public int AccountId { get; set; }
        public string CategoryName { get; set; }
        public string AccountName { get; set; }
        public string AccountCompanyName { get; set; }
    }
}
