using System.ComponentModel.DataAnnotations;

namespace FinancialTracker.Frontend.Models;

public class LoginRequest
{
    [Required(ErrorMessage = "Обязательное поле")]
    [EmailAddress(ErrorMessage = "Неправильный формат Email")]
    public string Email { get; set; }
    [Required(ErrorMessage = "Обязательное поле")]
    [MinLength(8, ErrorMessage = "Пароль должен содержать не менее 8 символов")]
    public string Password { get; set; }
}