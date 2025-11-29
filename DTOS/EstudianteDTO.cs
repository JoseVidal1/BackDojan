using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOS
{
    public class EstudianteDTO
    {
        public string Id { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Telefono { get; set; }
        public string Correo { get; set; }
        public string Direccion { get; set; }
        public string Eps { get; set; }
        public int IdRango { get; set; }
        public int? IdGrupo { get; set; }
        public int? Edad { get; set; }
        public DateOnly FechaNacimiento { get; set; }
    }

}
