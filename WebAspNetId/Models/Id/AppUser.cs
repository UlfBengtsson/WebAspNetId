﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAspNetId.Models.Id
{
    public class AppUser : IdentityUser
    {
        public int Age { get; set; }
    }
}
