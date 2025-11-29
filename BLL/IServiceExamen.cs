using DAL;
using DAL.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public interface IServiceExamen
    {
        Task<Examen> Registrar(Examen examen);
        Task<bool> Actualizar(Examen actualizar);
        Task<bool> Eliminar(string idEstudiante);
        Task<List<Examen>> Obtener();
        Task<List<Examen>> Leer();
    }
}
