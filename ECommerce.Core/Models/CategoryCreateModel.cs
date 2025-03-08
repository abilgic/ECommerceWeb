using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Core.Models
{
    public class CategoryModel
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        [StringLength(250)]
        public string Description { get; set; }

    }
    public class CategoryCreateModel
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        [StringLength(250)]
        public string Description { get; set; }
    }

    public class CategoryUpdateModel
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        [StringLength(250)]
        public string Description { get; set; }
    }
}
