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
    public class DBArticulo:IDBArticulo
    {
        private readonly DbDojankwonContext dbDojankwonContext;

        public DBArticulo(DbDojankwonContext dbDojankwonContext)
        {
            this.dbDojankwonContext = dbDojankwonContext;
        }
        public async Task Agregar(Articulo articulo)
        {
            dbDojankwonContext.Articulos.Add(articulo);
            await dbDojankwonContext.SaveChangesAsync();
        }
        public async Task<List<Articulo>> Leer()
        {
            return await dbDojankwonContext.Articulos.ToListAsync();
        }
        public async Task<bool> Actualizar(Articulo actualizado)
        {
            if (!dbDojankwonContext.Articulos.Any(a => a.Id == actualizado.Id)) return false;
            dbDojankwonContext.Articulos.Update(actualizado);
            await dbDojankwonContext.SaveChangesAsync();
            return true;
        }
        public async Task<Articulo> Buscar(int id)
        {
            return await dbDojankwonContext.Articulos.FindAsync(id);
        }
        public async Task<bool> Eliminar(int id)
        {
            Articulo eliminar = await Buscar(id);
            if (eliminar == null) return false;
            dbDojankwonContext.Articulos.Remove(eliminar);
            await dbDojankwonContext.SaveChangesAsync();
            return true;
        }
        }
}
