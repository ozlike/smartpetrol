using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Smartpetrol.Models.Users;

namespace Smartpetrol.Data
{
    public class Book
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Genre { get; set; }
        public string Publisher { get; set; }
    }
}