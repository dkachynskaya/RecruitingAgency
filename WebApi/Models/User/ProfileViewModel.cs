using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using WebApi.Models.JobOffer;

namespace WebApi.Models.User
{
    public class ProfileViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Login:")]
        public string Login { get; set; }

        [Display(Name = "FirstName:")]
        public string FirstName { get; set; }

        [Display(Name = "Lastname:")]
        public string LastName { get; set; }

        [Display(Name = "Email:")]
        public string Email { get; set; }

        public List<JobOfferViewModel> JobOffers { get; set; }

        public DateTime? RegistrationDate { get; set; }

        public bool IsAdmin { get; set; }
        public bool IsModerator { get; set; }

        public bool IsBlocked { get; set; }
    }
}