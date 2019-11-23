using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    [Table("JobOffer")]
    public class JobOffer: BaseEntity
    {
        public string PositionName { get; set; }
        public string Location { get; set; }
        public string Company { get; set; }
        public string PositionDescription { get; set; }
        public DateTime? CreateDate { get; set; }
        public bool IsActual { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }
        public int UserId { get; set; }
    }
}
