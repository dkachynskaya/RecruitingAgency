﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models.JobOffer
{
    public class JobOfferEditViewModel
    {
        public string PositionName { get; set; }
        public string Location { get; set; }
        public string Company { get; set; }
        public string PositionDescription { get; set; }
    }
}