using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppInmobiliaria.Models;

public class Propietario
{

    public int? Id { get; set; }
    public string? Dni { get; set; }
    public string? Nombre { get; set; }
    public string? Apellido { get; set; }

    [DataType(DataType.PhoneNumber)]
    public string? Direccion { get; set; }
    public string? Telefono { get; set; }
    public string? Email { get; set; }

    [DataType(DataType.Date)]
    public DateTime? FechaNacimiento { get; set; }

    public Propietario() { }

    public override string ToString()
    {
        return $"{Nombre} {Apellido}";
    }

}
