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
    public IFormFile? AvatarFile { get; set; }

    public int Rol { get; set; }

    public string RolNombre => Rol > 0 ? ((eRoles)Rol).ToString() : "";



    public static IDictionary<int, string> ObtenerRoles()
    {
        SortedDictionary<int, string> roles = new SortedDictionary<int, string>();
        Type tipoEnumRol = typeof(eRoles);
        foreach (var valor in Enum.GetValues(tipoEnumRol))
        {
            roles.Add((int)valor, Enum.GetName(tipoEnumRol, valor));
        }
        return roles;
    }

    public Usuario()
    {
    }


}
