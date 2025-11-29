using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace DAL.Modelos;

public partial class Grupo
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;
    [JsonIgnore]
    public virtual ICollection<Estudiante>? Estudiantes { get; set; } = new List<Estudiante>();
}
