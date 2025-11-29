using DAL.Modelos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class DBExamen: IDBExamen
    {
        private readonly DbDojankwonContext dbDojankwonContext;

        public DBExamen(DbDojankwonContext dbDojankwonContext)
        {
            this.dbDojankwonContext = dbDojankwonContext;
        }
        public async Task Agregar(Examen examen)
        {
            dbDojankwonContext.Examenes.Add(examen);
            await dbDojankwonContext.SaveChangesAsync();
        }
        public async Task<List<Examen>> Leer()
        {
            return await dbDojankwonContext.Examenes.Include(e=>e.Estudiante).ToListAsync();
        }
        public async Task<bool> Actualizar(Examen actualizado)
        {
            if (!dbDojankwonContext.Examenes.Any(p => p.Id == actualizado.Id)) return false;
            dbDojankwonContext.Examenes.Update(actualizado);
            await dbDojankwonContext.SaveChangesAsync();
            return true;
        }
        public async Task<Examen> Buscar(string cc)
        {
            return await dbDojankwonContext.Examenes
                .Include(e => e.Estudiante)
                .FirstOrDefaultAsync(e => e.Estudiante.Id == cc);
        }
        public async Task<bool> Eliminar(Examen eliminar)
        {
            dbDojankwonContext.Examenes.Remove(eliminar);
            await dbDojankwonContext.SaveChangesAsync();
            return true;
        }
    }
}
