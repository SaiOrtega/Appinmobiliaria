using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppInmobiliaria.Models;

public enum LoginView
{

}
public class LoginViewModel
{
    [Required]
    [EmailAddress]
    public string? Usuario { get; set; }
    [Required]
    [DataType(DataType.Password)]
    public string? Clave { get; set; }
}