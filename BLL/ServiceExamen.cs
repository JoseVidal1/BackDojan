using DAL;
using DAL.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using QuestPDF.Drawing;
using System.Collections.Generic;
namespace BLL
{
    public class ServiceExamen: IServiceExamen
    {
        private readonly IDBExamen dBExamen;
        private readonly IServiceEstudiante serviceEstudiante;
        private readonly IDBRango dBRango;
        public ServiceExamen(IDBExamen dBExamen,IServiceEstudiante serviceEstudiante, IDBRango dBRango)
        {
            this.dBExamen = dBExamen;
            this.serviceEstudiante = serviceEstudiante;
            this.dBRango = dBRango;
        }
        public async Task<Examen> Registrar(Examen examen)
        {
            Estudiante estudiante = await serviceEstudiante.Buscar(examen.EstudianteId);

            // ⭐ AGREGAR ESTA LÍNEA para que examen.Estudiante no sea null
            examen.Estudiante = estudiante;

            if (await TieneExamenVigente(examen.EstudianteId))
                throw new ArgumentException("El estudiante ya tiene un examen registrado");
            if (estudiante.estado == "Inactivo")
                throw new ArgumentException("Al estudiante no se le puede evaluar su estado es inactivo");
            var ultimoExamen = await ObtenerUltimoExamenPorEstudiante(examen.EstudianteId);
            if (ultimoExamen == null || !ultimoExamen.FechaRegistro.HasValue)
            {
                examen.CalcularNotaFinal();
                await dBExamen.Agregar(examen);
                if (examen.NotaFinal >= 70)
                {
                    Rango rango = await dBRango.Buscar(estudiante.IdRango + 1);
                    estudiante.IdRango = rango.Id;
                    await serviceEstudiante.Actualizar(estudiante); // ⭐ Usar estudiante en vez de examen.Estudiante
                }
                return await dBExamen.Buscar(examen.EstudianteId);
            }
            if (HanPasadoCincoMeses(ultimoExamen.FechaRegistro.Value.ToDateTime(TimeOnly.MinValue)))
            {
                examen.CalcularNotaFinal();
                await dBExamen.Agregar(examen);
                if (examen.NotaFinal >= 70)
                {
                    Rango rango = await dBRango.Buscar(estudiante.IdRango + 1); // ⭐ Usar estudiante en vez de examen.Estudiante
                    estudiante.IdRango = rango.Id;
                    estudiante.IdRangoNavigation = rango;
                    await serviceEstudiante.Actualizar(estudiante); // ⭐ Usar estudiante en vez de examen.Estudiante
                }
                return await dBExamen.Buscar(examen.EstudianteId);
            }
            throw new ArgumentException("No han pasado 5 meses desde el último examen");
        }

        private async Task<bool> TieneExamenVigente(string estudianteId)
        {
            List<Examen> examenes = await Leer(); // Este método es como LeerFiltradosPorUltimoMes
            return examenes.Any(e => e.EstudianteId == estudianteId);
        }

        private async Task<Examen?> ObtenerUltimoExamenPorEstudiante(string estudianteId)
        {
            List<Examen> examenes = await Obtener();
            return examenes
                .Where(e => e.EstudianteId == estudianteId && e.FechaRegistro.HasValue)
                .OrderByDescending(e => e.FechaRegistro)
                .FirstOrDefault();
        }

        private bool HanPasadoCincoMeses(DateTime fecha)
        {
            return fecha.Date.AddMonths(5) <= DateTime.Today;
        }
        public async Task<List<Examen>> Obtener()
        {
            return await dBExamen.Leer();
        }
        public async Task<List<Examen>> Leer()
        {
            List<Examen> examenes = await dBExamen.Leer();

            DateTime fechaLimite = DateTime.Today.AddDays(-14);

            return examenes
                .Where(e => e.FechaRegistro.HasValue &&
                            e.FechaRegistro.Value.ToDateTime(TimeOnly.MinValue) >= fechaLimite)
                .ToList();
        }
        


        public async Task<bool> Actualizar(Examen actualizar)
        {
            await dBExamen.Actualizar(actualizar);
            return true;
               
        }
        public async Task<bool> Eliminar(string idEstudiante)
        {
            Examen eliminar = await ObtenerUltimoExamenPorEstudiante(idEstudiante);
            await dBExamen.Eliminar(eliminar);
            return true;
        }
    }
}
