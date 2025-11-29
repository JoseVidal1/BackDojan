using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace DAL.Modelos;

public partial class Pago
{
    public int Id { get; set; }
    public string Estado { get; set; } = null!;
    public DateOnly FechaPago { get; set; }
    public string IdEstudiante { get; set; } = null!;
    public virtual Estudiante? IdEstudianteNavigation { get; set; } = null!;
}
