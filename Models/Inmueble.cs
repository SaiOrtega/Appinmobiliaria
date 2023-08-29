using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.Build.Framework;

namespace AppInmobiliaria.Models;

[Table("Inmuebles")]

public class Inmueble
{

    [Display(Name = "N°")]
    public int Id { get; set; }


    [Display(Name = "Dirección")]

    public string direccion { get; set; }

    public string uso { get; set; }
    public string tipo { get; set; }

    [Display(Name = "Cant.Ambientes")]
    public int ambientes { get; set; }
    public decimal latitud { get; set; }
    public decimal longitud { get; set; }
    public decimal precio { get; set; }
    public decimal superficie { get; set; }
    public string estado { get; set; }

    [Display(Name = "Propietario")]//nuestra en las vista el dato

    public int propietarioId { get; set; }

    [ForeignKey(nameof(propietarioId))]
    public Propietario? duenio { get; set; }// Propietario

    public Inmueble() { }

    public override string ToString()
    {
        return $"{direccion}";
    }
}