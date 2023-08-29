using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppInmobiliaria.Models;

public class Contrato
{
    public int? Id { get; set; }
    public int? InmuebleId { get; set; }
    public int? InquilinoId { get; set; }

    [DataType(DataType.Date)]
    [Display(Name = "Fecha de Contrato")]
    public DateTime? FechaContrato { get; set; }

    [DataType(DataType.Date)]
    [Display(Name = "Fecha de Vencimiento")]
    public DateTime? FechaVencimiento { get; set; }

    [Display(Name = "Monto")]
    public decimal? MontoMensual { get; set; }

    public Inmueble inmueble { get; set; }
    public Inquilino inquilino { get; set; }

    public Contrato() { }


    public override string ToString()
    {
        return $"{InmuebleId} {InquilinoId}";
    }


}


