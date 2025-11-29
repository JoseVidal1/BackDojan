using DAL;
using DAL.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public interface IServiceArticulo
    {
        public Task<bool> Agregar(Articulo agregar);
        public   Task<List<Articulo>> Leer();
        public  Task<Articulo> Buscar(int id);
        public  Task<bool> Actualizar(Articulo actualizado);
        public  Task<bool> Eliminar(int id);
    }
}
