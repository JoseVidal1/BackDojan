using DAL.Modelos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class DBGrupo:IDBGrupo
    {
        public readonly DbDojankwonContext dbDojankwonContext;
        public DBGrupo(DbDojankwonContext dbDojankwonContext)
        {
            this.dbDojankwonContext = dbDojankwonContext;
        }

        public async Task<List<Grupo>> Leer()
        {
            return await dbDojankwonContext.Grupos.ToListAsync();
        }

        public async Task<Grupo> Buscar(int id)
        {
            return await dbDojankwonContext.Grupos.FindAsync(id);
        }
    }
}
