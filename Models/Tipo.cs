using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppInmobiliaria.Models;

public class Tipo
{
    public int? Id { get; set; }
    public string? tipo { get; set; }


    public Tipo() { }

}


