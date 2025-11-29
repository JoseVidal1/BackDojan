using DAL;
using DAL.Modelos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class DBPrestamo: IDBPrestamo
    {
        private readonly DbDojankwonContext dbDojankwonContext;

        public DBPrestamo(DbDojankwonContext dbDojankwonContext)
        {
            this.dbDojankwonContext = dbDojankwonContext;
        }
        public async Task Agregar(Prestamo prestamo)
        {
            var ultimoPrestamo = await dbDojankwonContext.Prestamos.OrderByDescending(p => p.Id).FirstOrDefaultAsync();
            if (ultimoPrestamo != null)
            {

                foreach (var detalle in prestamo.DetallePrestamos)
                {
                    detalle.IdPrestamo = ultimoPrestamo.Id+1; // Asegurarse de que el IdPrestamo esté configurado

                }
            }
            else { 
                foreach (var detalle in prestamo.DetallePrestamos)
                {
                    detalle.IdPrestamo = 1; // Si no hay prestamos, el primer IdPrestamo es 1
                }
            }

            dbDojankwonContext.Prestamos.Add(prestamo);
            await dbDojankwonContext.SaveChangesAsync();
            
        }
        public async Task<List<Prestamo>> Leer()
        {
            return await dbDojankwonContext.Prestamos.Include(p=>p.Estudiante).Include(p=>p.DetallePrestamos).ThenInclude(d=>d.IdArticuloNavigation).ToListAsync();
        }
        public async Task<bool> Actualizar(Prestamo actualizado)
        {
            if (!dbDojankwonContext.Prestamos.Any(p => p.Id == actualizado.Id)) return false;
            dbDojankwonContext.Prestamos.Update(actualizado);
            await dbDojankwonContext.SaveChangesAsync();
            return true;
        }
        public async Task<Prestamo> Buscar(int id)
        {
            return await dbDojankwonContext.Prestamos.Include(p=>p.Estudiante).Include(p=>p.DetallePrestamos).ThenInclude(d=>d.IdArticuloNavigation).FirstOrDefaultAsync(p => p.Id == id); ;
        }
        public async Task<Prestamo> BuscarUltimoPrestamo()
        {
            return await dbDojankwonContext.Prestamos.OrderByDescending(p => p.Id).FirstOrDefaultAsync();
        }
        public async Task<bool> Eliminar(int id)
        {
            Prestamo eliminar = await Buscar(id);
            if (eliminar == null) return false;
            dbDojankwonContext.Prestamos.Remove(eliminar);
            await dbDojankwonContext.SaveChangesAsync();
            return true;
        }
    }
}
