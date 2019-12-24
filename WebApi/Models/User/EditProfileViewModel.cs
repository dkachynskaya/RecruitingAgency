using System;
using System.ComponentModel.DataAnnotations;

namespace WebApi.Models.User
{
    public class EditProfileViewModel
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
        public DateTime? RegistrationDate { get; set; }
        public bool IsBlocked { get; set; }
    }
}