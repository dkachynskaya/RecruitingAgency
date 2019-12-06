using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTOs
{
    public class AdDTO
    {
        public int Id { get; set; }
        public string PositionName { get; set; }
        public string Location { get; set; }
        public string Company { get; set; }
        public string PositionDescription { get; set; }
        public DateTime? CreateDate { get; set; }
        public bool IsBlocked { get; set; }

        public int UserId { get; set; }
        public virtual UserDTO User { get; set; }
    }
}
