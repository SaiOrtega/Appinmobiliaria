using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppInmobiliaria.Models;

public enum eRoles
{
    Administrador = 1,
    Empleado = 2,
}
public class Usuario
{
    [Key]
    [Display(Name = "Código")]
    public int? Id { get; set; }
    [Required]
    public string? Nombre { get; set; }
    [Required]
    public string? Apellido { get; set; }

    public string? Avatar { get; set; } = null;
    [Required, DataType(DataType.Password)]
    public string? Clave { get; set; } = null;

    [Required, EmailAddress]
    public string? Email { get; set; }
    [NotMapped]
    public IFormFile? FormFile { get; set; }

    public int rol { get; set; }

    public string rolNombre => rol == 1 ? ((eRoles)rol).ToString() : "Empleado";


    public Usuario() { }

    public static IDictionary<int, string> traerRoles()
    {
        IDictionary<int, string> roles = new Dictionary<int, string>();
        roles.Add(1, "Administrador");
        roles.Add(2, "Empleado");
        return roles;
    }


}
