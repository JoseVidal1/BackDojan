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
    public class DBRango:IDBRango
    {
        private readonly DbDojankwonContext dbDojankwonContext;

        public DBRango(DbDojankwonContext dbDojankwonContext)
        {
            this.dbDojankwonContext = dbDojankwonContext;
        }
        
        public async Task<List<Rango>> Leer()
        {
            return await dbDojankwonContext.Rangos.ToListAsync();
        }
        
        public async Task<Rango> Buscar(int id)
        {
            return await dbDojankwonContext.Rangos.FindAsync(id);
        }
        
    }
}
