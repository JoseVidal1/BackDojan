using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace DAL.Modelos;

public partial class Estudiante
{
    public string Id { get; set; } = null!;

    public string Nombres { get; set; } = null!;

    public string Apellidos { get; set; } = null!;

    public string Telefono { get; set; } = null!;

    public string Correo { get; set; } = null!;

    public string Direccion { get; set; } = null!;

    public string Eps { get; set; } = null!;

    public int IdRango { get; set; }
    public int? IdGrupo { get; set; }
    public int? edad { get; set; }
    public DateOnly FechaNacimiento { get; set; }
    [JsonIgnore]
    public DateOnly? FechaRegistro { get; set; }
    public string? estado { get; set; }
    [JsonIgnore]
    public virtual ICollection<Examen> Examenes { get; set; } = new List<Examen>();
    public virtual Grupo? IdGrupoNavigation { get; set; }
    public virtual Rango? IdRangoNavigation { get; set; } = null!;
    [JsonIgnore]
    public virtual ICollection<Pago> Pagos { get; set; } = new List<Pago>();
    [JsonIgnore]
    public virtual ICollection<Prestamo> Prestamos { get; set; } = new List<Prestamo>();

    public void CalcularEdad()
    {
        int edad = DateTime.Today.Year - FechaNacimiento.Year;

        if (DateTime.Today.Month < FechaNacimiento.Month ||
           (DateTime.Today.Month == FechaNacimiento.Month && DateTime.Today.Day < FechaNacimiento.Day))
        {
            edad--;
        }

        this.edad = edad;
    }
}
