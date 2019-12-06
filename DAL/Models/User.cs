using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using DAL.Identity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    [Table("User")]
    public class User: BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None), Key]
        [ForeignKey("ApplicationUser")]
        public override int Id { get; set; }
        [MaxLength(30)]
        public string FirstName { get; set; }
        [MaxLength(30)]
        public string LastName { get; set; }

        [Index(IsUnique = true)]
        [MaxLength(30)]
        [MinLength(3)]
        public string Login { get; set; }
        public bool IsBlocked { get; set; }
        public DateTime? RegistrationDate { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }

        public virtual ICollection<Ad> Ads { get; set; }

        public User()
        {
            Ads = new List<Ad>();
        }
    }
}
