using DAL.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public interface IDBExamen
    {
        public  Task Agregar(Examen examen);
        public  Task<List<Examen>> Leer();
        public  Task<bool> Actualizar(Examen actualizado);
        public  Task<Examen> Buscar(string cc);
        public  Task<bool> Eliminar(Examen eliminar);
    }
}
