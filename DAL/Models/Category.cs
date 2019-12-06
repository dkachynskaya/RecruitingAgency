using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    [Table("Category")]
    public class Category: BaseEntity
    {
        [MaxLength(255)]
        public string Name { get; set; }

        public virtual ICollection<Ad> Ads { get; set; }
    }
}
