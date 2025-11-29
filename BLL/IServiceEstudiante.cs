using DAL;
using DAL.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public interface IServiceEstudiante
    {

        public  Task<Estudiante> Agregar(Estudiante agregar);
        public  Task<List<Estudiante>> Leer();
        public  Task<Estudiante> Buscar(string id);
        public  Task<Estudiante> Actualizar(Estudiante actualizado);
        public  Task<bool> Eliminar(string id);
    }
}
