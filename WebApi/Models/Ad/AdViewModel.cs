using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApi.Models.User;

namespace WebApi.Models.JobOffer
{
    public class AdViewModel
    {
        public int Id { get; set; }
        public string PositionName { get; set; }
        public string Location { get; set; }
        public string Company { get; set; }
        public string PositionDescription { get; set; }
        
        public int UseId { get; set; }
        public virtual ProfileViewModel User { get; set; }

        public DateTime? CreateDate { get; set; }
        public bool IsBlocked { get; set; }

        public int CategoryId { get; set; }
    }
}