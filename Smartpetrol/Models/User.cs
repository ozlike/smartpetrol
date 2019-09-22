﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Smartpetrol.Models
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
    }
}
