using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApi.Models.JobOffer;

namespace WebApi.Models.Category
{
    public class CategoryViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<AdViewModel> Ads { get; set; }
    }
}