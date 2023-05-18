using System.ComponentModel.DataAnnotations;

namespace GRSMU.Bot.Web.Core.ViewModels.Authetication;

public class LoginViewModel
{
    [Required]
    public string Login { get; set; }
    [Required]
    public string Password { get; set; }
}