﻿using System;
using System.Collections.Generic;
using DAL.Models;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTOs
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Login { get; set; }
        public bool IsBlocked { get; set; }
        public DateTime? RegistrationDate { get; set; }

        public virtual ICollection<JobOffer> JobOffers { get; set; }
        public List<string> Roles { get; set; }
    }
}
