using DAL;
using DAL.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class ServiceEstudiante: IServiceEstudiante
    {
        private readonly IDBEstudiante dBEstudiante;
        private readonly IDBGrupo dBGrupo;
        public ServiceEstudiante(IDBEstudiante dBEstudiante, IDBGrupo dBGrupo)
        {
            this.dBEstudiante = dBEstudiante;
            this.dBGrupo = dBGrupo;
        }
        public async Task<Estudiante> Agregar(Estudiante agregar)
        {
            if (await dBEstudiante.Buscar(agregar.Id) == null)
            {
                agregar.estado = "Activo";
                agregar.CalcularEdad();
                if (agregar.edad >= 6 && agregar.edad <= 10)
                {
                    agregar.IdGrupo = 1;
                }
                if (agregar.edad>10 && agregar.edad <= 15)
                {
                    agregar.IdGrupo = 2;

                }
                if (agregar.edad >= 16)
                {
                    agregar.IdGrupo= 3;

                }
                await dBEstudiante.Agregar(agregar);
                return agregar;

            }
            throw new ArgumentException("Estudiante ya ha sido registrado");
        }
        public async Task<List<Estudiante>> Leer()
        {
            return await dBEstudiante.Leer();
        }
        public async Task<Estudiante> Buscar(string id)
        {
            return await dBEstudiante.Buscar(id);
        }
        public async Task<Estudiante> Actualizar(Estudiante actualizado)
        {
            await dBEstudiante.Actualizar(actualizado);
            List<Estudiante> estudiantes = await dBEstudiante.Leer();
            var estudianteActualizado = estudiantes.FirstOrDefault(e => e.Id == actualizado.Id);
            return estudianteActualizado;
        }
        public async Task<bool> Eliminar(string id)
        {
            return await dBEstudiante.Eliminar(id);
        }
    }
}
