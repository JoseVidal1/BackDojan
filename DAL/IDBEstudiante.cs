using DAL.Modelos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public interface IDBEstudiante
    {
        public  Task Agregar(Estudiante estudiante);
        public  Task<List<Estudiante>> Leer();
        public  Task<bool> Actualizar(Estudiante actualizado);
        public  Task<Estudiante> Buscar(string id);
        public  Task<bool> Eliminar(string id);
    }
}
