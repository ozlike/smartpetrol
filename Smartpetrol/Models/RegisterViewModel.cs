using System.ComponentModel.DataAnnotations;

namespace Smartpetrol.Models
{
    public class RegisterViewModel
    {
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Электронная почта")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Поле \"{0}\" должно быть заполнено")]
        public string Email { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Поле \"{0}\" должно быть заполнено")]
        [DataType(DataType.Password)]
        [StringLength(90, ErrorMessage = "{0} должен иметь длину минимум {2} и максимум {1} символов.", MinimumLength = 6)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Поле \"{0}\" должно быть заполнено")]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        [Display(Name = "Подтвердить пароль")]
        public string PasswordConfirm { get; set; }
    }
}
