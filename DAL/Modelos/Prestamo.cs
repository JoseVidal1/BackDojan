using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace DAL.Modelos;

public partial class Prestamo
{
    public int Id { get; set; }

    public string EstudianteId { get; set; } = null!;

    public DateOnly FechaPrestamo { get; set; }

    public DateOnly FechaDevolucion { get; set; }

    public string? Estado { get; set; } = null!;

    public virtual ICollection<DetallePrestamo> DetallePrestamos { get; set; } = new List<DetallePrestamo>();
    public virtual Estudiante? Estudiante { get; set; } = null!;
}
