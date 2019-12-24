using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models
{
    [Table("Ad")]
    public class Ad: BaseEntity
    {
        public string PositionName { get; set; }
        public string Location { get; set; }
        public string Company { get; set; }
        public string PositionDescription { get; set; }
        public DateTime? CreateDate { get; set; }
        public bool IsBlocked { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }
        public int UserId { get; set; }

        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }
        public int CategoryId { get; set; }
    }
}
