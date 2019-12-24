using System.ComponentModel.DataAnnotations;

namespace DAL.Models
{
    public abstract class BaseEntity
    {
        [Key]
        public virtual int Id { get; set; }
    }
}
