using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace DAL.Modelos;

public partial class Articulo
{
    public int Id { get; set; }

    public int Cantidad { get; set; }

    public int Disponibles { get; set; }

    public string Nombre { get; set; } = null!;
    [JsonIgnore]
    public virtual ICollection<DetallePrestamo>? DetallePrestamos { get; set; } = new List<DetallePrestamo>();
}
