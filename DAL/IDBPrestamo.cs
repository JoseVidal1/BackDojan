using DAL.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public interface IDBPrestamo
    {
        public  Task Agregar(Prestamo prestamo);
        public  Task<List<Prestamo>> Leer();
        public  Task<bool> Actualizar(Prestamo actualizado);
        public  Task<Prestamo> Buscar(int id);
        public  Task<Prestamo> BuscarUltimoPrestamo();
        public Task<bool> Eliminar(int id);
    }
}
