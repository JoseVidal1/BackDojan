using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOS
{
    public class ArticuloDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public int Cantidad { get; set; }
        public int Disponibles { get; set; }
    }
}
