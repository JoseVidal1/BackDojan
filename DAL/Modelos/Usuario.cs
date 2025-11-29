using System;
using System.Collections.Generic;

namespace DAL.Modelos;

public partial class Usuario
{
    public string Cc { get; set; } = null!;

    public string Nombres { get; set; } = null!;

    public string Apellidos { get; set; } = null!;

    public string UserName { get; set; } = null!;

    public string Contraseña { get; set; } = null!;

    public string Rol { get; set; } = null!;

    public string Telefono { get; set; } = null!;

    public string Correo { get; set; } = null!;

    public string Direccion { get; set; } = null!;
}
