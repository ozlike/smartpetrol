using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Smartpetrol.Models.Users;

namespace Smartpetrol.Models.Books
{
    public class BookViewModel
    {
        public Guid Id { get; set; }
        public BookStatus Status { get; set; }
        public DateTime? ReservationTime { get; set; }
        public DateTime? RentalTime { get; set; }
        public DateTime? ReservationEndTime { get; set; }
        
        [Display(Name = "Название книги")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Поле \"{0}\" должно быть заполнено")]
        public string Title { get; set; }

        [Display(Name = "Автор")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Поле \"{0}\" должно быть заполнено")]
        public string Author { get; set; }

        [Display(Name = "Жанр")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Поле \"{0}\" должно быть заполнено")]
        public string Genre { get; set; }

        [Display(Name = "Издатель")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Поле \"{0}\" должно быть заполнено")]
        public string Publisher { get; set; }
    }
}
