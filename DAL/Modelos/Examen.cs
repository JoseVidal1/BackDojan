using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace DAL.Modelos;

public partial class Examen
{
    public int Id { get; set; }
    
    public string EstudianteId { get; set; } = null!;

    public int Calentamiento { get; set; }

    public int TecMano { get; set; }

    public int TecPatada { get; set; }

    public int TecEspecial { get; set; }

    public int Combate { get; set; }

    public int Rompimiento { get; set; }

    public int Teorica { get; set; }

    public int? NotaFinal { get; set; }
    [JsonIgnore]
    public DateOnly? FechaRegistro { get; set; }
    public virtual Estudiante? Estudiante { get; set; } = null!;
    public void CalcularNotaFinal()
    {
        NotaFinal = Calentamiento + TecMano + TecPatada + TecEspecial + Combate + Rompimiento + Teorica;
    }
}
