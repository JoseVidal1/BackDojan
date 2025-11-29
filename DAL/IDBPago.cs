using DAL.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public interface IDBPago
    {
        public  Task Agregar(Pago pago);
        public  Task<List<Pago>> Leer();
        public  Task<Pago> Actualizar(Pago actualizado);
        public  Task<Pago> Buscar(string cc);
        public Task<bool> Eliminar(string cc);
    }
}
