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
    public class DBUsuario: IDBUsuario
    {
        private readonly DbDojankwonContext dbDojankwonContext;

        public DBUsuario(DbDojankwonContext dbDojankwonContext)
        {
            this.dbDojankwonContext = dbDojankwonContext;
        }
        public async Task Agregar(Usuario usuario)
        {
            dbDojankwonContext.Usuarios.Add(usuario);
            await dbDojankwonContext.SaveChangesAsync();
        }
        public async Task<List<Usuario>> Leer()
        {
            return await dbDojankwonContext.Usuarios.ToListAsync();
        }
        public async Task<bool> Actualizar(Usuario actualizado)
        {
            if (!dbDojankwonContext.Usuarios.Any(u => u.Cc == actualizado.Cc)) return false;
            dbDojankwonContext.Usuarios.Update(actualizado);
            await dbDojankwonContext.SaveChangesAsync();
            return true;
        }
        public async Task<Usuario> Buscar(string cc)
        {
            return await dbDojankwonContext.Usuarios.FindAsync(cc);
        }
        public async Task<bool> Eliminar(string cc)
        {
            Usuario eliminar = await Buscar(cc);
            if (eliminar == null) return false;
            dbDojankwonContext.Usuarios.Remove(eliminar);
            await dbDojankwonContext.SaveChangesAsync();
            return true;
        }
    }
}
