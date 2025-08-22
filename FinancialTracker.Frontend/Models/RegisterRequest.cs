using System.ComponentModel.DataAnnotations;

namespace FinancialTracker.Frontend.Models;

public class RegisterRequest
{
    [Required(ErrorMessage = "Обязательное поле")]
    [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Имя должно содержать только символы латинского алфавита")]
    [MaxLength(50, ErrorMessage = "Не более 50 символов")]
    public string FullName { get; set; }

    [Required(ErrorMessage = "Обязательное поле")]
    [EmailAddress(ErrorMessage = "Неправильный формат Email")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Обязательное поле")]
    [MinLength(8, ErrorMessage = "Пароль должен содержать не менее 8 символов")]
    public string Password { get; set; }

    [Required(ErrorMessage = "Обязательное поле")]
    [Compare(nameof(Password), ErrorMessage = "Пароли не совпадают")]
    public string ConfirmedPassword { get; set; }
    
    [Required(ErrorMessage = "Пожалуйста, подтвердите что вы не робот")]
    public string CaptchaToken { get; set; }
}