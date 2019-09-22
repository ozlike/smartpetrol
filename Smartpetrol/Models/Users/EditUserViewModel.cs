using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Smartpetrol.Models.Users
{
    public class EditUserViewModel
    {
        public  string Id { get; set; }

        [Display(Name = "Имя пользователя")]
        public string FirstName { get; set; }

        [DataType(DataType.EmailAddress)]
        [Display(Name = "Электронная почта")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Поле \"{0}\" должно быть заполнено")]
        public string Email { get; set; }
        
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }
        
        [Display(Name = "Роль пользователя")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Роль должна быть выбрана")]
        public RolesList RolesList { get; set; } = new RolesList();
    }
}
