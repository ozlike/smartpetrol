using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Smartpetrol.Models.Books;
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
        public BookStatus Status { get; set; }
        public DateTime? ReservationTime { get; set; }
        public DateTime? RentalTime { get; set; }

        public User Tenant { get; set; }
        public string TenantId { get; set; }
    }
}