﻿using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Smartpetrol.Models;
using Smartpetrol.Models.Users;

namespace Smartpetrol.Data
{
    public class SmartDbContext : IdentityDbContext
    {
        public SmartDbContext(DbContextOptions<SmartDbContext> options): base(options)
        {
            Database.EnsureCreated();
        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Book> Books { get; set; }
    }
}
