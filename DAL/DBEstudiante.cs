using DAL.Modelos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class DBEstudiante:IDBEstudiante
    {
        private readonly DbDojankwonContext dbDojankwonContext;

        public DBEstudiante(DbDojankwonContext dbDojankwonContext)
        {
            this.dbDojankwonContext = dbDojankwonContext;
        }
        public async Task Agregar(Estudiante estudiante)
        {
            try
            {
                dbDojankwonContext.Estudiantes.Add(estudiante);
                await dbDojankwonContext.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine("Ocurrió un error al guardar el estudiante:");
                Console.WriteLine(ex.InnerException?.Message ?? ex.Message);
            }
        }
        public async Task<List<Estudiante>> Leer()
        {
            return await dbDojankwonContext.Estudiantes
                .Include(e => e.IdGrupoNavigation)
                .Include(e => e.IdRangoNavigation)
                .ToListAsync();
        }
        public async Task<bool> Actualizar(Estudiante actualizado)
        {
            if (!dbDojankwonContext.Estudiantes.Any(p => p.Id == actualizado.Id)) return false;
            dbDojankwonContext.Estudiantes.Update(actualizado);
            await dbDojankwonContext.SaveChangesAsync();
            return true;
        }
        public async Task<Estudiante> Buscar(string id)
        {
            return await dbDojankwonContext.Estudiantes
                .Include(e => e.IdGrupoNavigation)
                .Include(e => e.IdRangoNavigation)
                .FirstOrDefaultAsync(e => e.Id == id);
        }
        public async Task<bool> Eliminar(string id)
        {
            Estudiante eliminar = await Buscar(id);
            if (eliminar == null) return false;
            dbDojankwonContext.Estudiantes.Remove(eliminar);
            await dbDojankwonContext.SaveChangesAsync();
            return true;
        }
    }
}
