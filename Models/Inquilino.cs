using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppInmobiliaria.Models;

public class Inquilino
{

    [Display(Name = "Código")]
    public int? Id { get; set; }
    public string? Dni { get; set; }
    public string? Nombre { get; set; }
    public string? Apellido { get; set; }

    [DataType(DataType.PhoneNumber)]
    [Display(Name = "Dirección")]
    public string? Direccion { get; set; }

    [Display(Name = "Teléfono")]
    public string? Telefono { get; set; }
    public string? Email { get; set; }

    [DataType(DataType.Date)]
    [Display(Name = "Fecha de Nacimiento")]
    public DateTime? FechaNacimiento { get; set; }

    public Inquilino() { }

    public override string ToString()
    {
        return $"{Nombre} {Apellido}";
    }

}
