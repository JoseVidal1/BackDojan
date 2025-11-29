using DAL.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public interface IDBArticulo
    {
        public  Task Agregar(Articulo articulo);
        public  Task<List<Articulo>> Leer();
        public  Task<bool> Actualizar(Articulo actualizado);
        public  Task<Articulo> Buscar(int id);
        public  Task<bool> Eliminar(int id);
    }
}
