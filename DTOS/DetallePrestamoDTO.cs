using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOS
{
    public class DetallePrestamoDTO
    {
        public int Id { get; set; }
        public int IdArticulo { get; set; }
        public int Cantidad { get; set; }

        public ArticuloDTO? Articulo { get; set; }
    }
}
