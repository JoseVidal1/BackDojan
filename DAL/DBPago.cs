using DAL.Modelos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class DBPago: IDBPago
    {
        private readonly DbDojankwonContext dbDojankwonContext;

        public DBPago(DbDojankwonContext dbDojankwonContext)
        {
            this.dbDojankwonContext = dbDojankwonContext;
        }
        public async Task Agregar(Pago pago)
        {
            dbDojankwonContext.Pagos.Add(pago);
            await dbDojankwonContext.SaveChangesAsync();
        }
        public async Task<List<Pago>> Leer()
        {
            return await dbDojankwonContext.Pagos
                 .Include(e => e.IdEstudianteNavigation)
                 .ToListAsync();
        }
        public async Task<Pago> Actualizar(Pago actualizado)
        {
            dbDojankwonContext.Pagos.Update(actualizado);
            await dbDojankwonContext.SaveChangesAsync();
            return await dbDojankwonContext.Pagos
                .Include(e => e.IdEstudianteNavigation)
                .FirstOrDefaultAsync(p => p.Id == actualizado.Id);
        }
        public async Task<Pago> Buscar(string cc)
        {
            return await dbDojankwonContext.Pagos
                .Where(p => p.IdEstudiante == cc)
                .Include(p => p.IdEstudianteNavigation)
                .OrderByDescending(p => p.FechaPago)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> Eliminar(string cc)
        {
            Pago eliminar = await Buscar(cc);
            if (eliminar == null) return false;
            dbDojankwonContext.Pagos.Remove(eliminar);
            await dbDojankwonContext.SaveChangesAsync();
            return true;
        }
    }
}
