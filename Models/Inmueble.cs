using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppInmobiliaria.Models;

[Table("Inmuebles")]

public class Inmueble
{

    [Display(Name = "N°")]
    public int? Id { get; set; }

    [Display(Name = "Dirección")]
    public string? Direccion { get; set; }
    public int? Uso { get; set; }
    public int? Tipo { get; set; }

    [Display(Name = "Cant.Ambientes")]
    public int? CantidadDeAmbientes { get; set; }
    // como seria lat y long
    public decimal? Latitud { get; set; }
    public decimal? Longitud { get; set; }
    public decimal? Precio { get; set; }
    public decimal? Superficie { get; set; }
    public bool? Activo { get; set; }

    [Display(Name = "Propietario")]//nuestra en las vista el dato

    public int? PropietarioId { get; set; }

    [ForeignKey(nameof(PropietarioId))]
    public Propietario? Duenio { get; set; }// Propietario

    public Inmueble() { }

    public override string ToString()
    {
        return $"{Direccion}";
    }
}

