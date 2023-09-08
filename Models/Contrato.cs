using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppInmobiliaria.Models;

public class Contrato
{
    [Display(Name = "Cod")]
    public int? Id { get; set; }
    [Display(Name = "n°- inmueble")]
    public int? InmuebleId { get; set; }

    [Display(Name = "Alquilado a")]
    public int? InquilinoId { get; set; }

    [DataType(DataType.DateTime)]
    [Display(Name = "Fecha de Contrato")]
    public DateTime? FechaInicio { get; set; }

    [DataType(DataType.DateTime)]
    [FechaActual(ErrorMessage = null)]
    [Display(Name = "Fecha de Vencimiento")]
    public DateTime? FechaFinal { get; set; }

    [Display(Name = "Monto de Contrato")]
    public decimal? MontoMensual { get; set; }

    public Inmueble inmueble { get; set; }
    public Inquilino inquilino { get; set; }

    public Contrato() { }


    public override string ToString()
    {
        return $"{InmuebleId} {InquilinoId}";
    }


}
public class FechaActualAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        DateTime fechaFinal = Convert.ToDateTime(value);
        var contrato = (Contrato)validationContext.ObjectInstance;

        if (fechaFinal < DateTime.Now)
        {
            return new ValidationResult(ErrorMessage ?? $"La {validationContext.DisplayName} no puede ser anterior a la fecha actual.");
        }

        if (contrato.FechaInicio.HasValue && fechaFinal < contrato.FechaInicio.Value)
        {
            return new ValidationResult(ErrorMessage ?? $"La {validationContext.DisplayName} no puede ser menor que la fecha de inicio.");
        }
        if (contrato.FechaFinal.HasValue && contrato.FechaInicio.HasValue)
        {

        }

        return ValidationResult.Success;
    }
}


