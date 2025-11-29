namespace DTOS
{
    public class PrestamoDTO
    {
            public int Id { get; set; }
            public string EstudianteId { get; set; } = null!;
            public DateOnly FechaPrestamo { get; set; }
            public DateOnly FechaDevolucion { get; set; }
            public string? Estado { get; set; }

            public List<DetallePrestamoDTO> Detalles { get; set; } = new();
            public EstudianteDTO? Estudiante { get; set; }  // <-- aquí

    }
}
