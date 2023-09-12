using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;




namespace AppInmobiliaria.Models;

[Table("Inmuebles")]

public class Inmueble
{

    [Display(Name = "N°")]
    public int Id { get; set; }


    [Display(Name = "Dirección")]

    public string? direccion { get; set; }

    public int uso { get; set; }
    public int tipo { get; set; }

    [Display(Name = "Cant.Ambientes")]
    public int? ambientes { get; set; }
    public decimal? latitud { get; set; }
    public decimal? longitud { get; set; }
    public decimal? precio { get; set; }
    public decimal? superficie { get; set; }

    [Required]
    public bool estado { get; set; }

    [Display(Name = "Propietario")]

    [Required]
    public int? propietarioId { get; set; }

    [ForeignKey(nameof(propietarioId))]
    public Propietario? duenio { get; set; }

    public Inmueble()
    {
    }

    public override string ToString()
    {
        return $"{direccion}";
    }

}