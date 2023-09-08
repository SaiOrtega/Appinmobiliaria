using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppInmobiliaria.Models;

public class Pago
{
    [DisplayName("N°")]
    public int? id { get; set; }
    [DisplayName("Contrato")]
    public int? contratoId { get; set; }

    [DisplayName("Cuota")]
    public int? numeroCuota { get; set; } = 0;
    [Required]
    public decimal? importe { get; set; } = default(decimal?);

    [DisplayName("Fecha de Pago")]
    [DataType(DataType.Date)]
    public DateTime? fechaPago { get; set; }
    [DataType(DataType.Date)]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
    public DateTime? periodo { get; set; }


    public Pago() { }

}


