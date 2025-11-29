using DAL;
using DAL.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public interface IServiceRango
    {
        public Task<List<Rango>> Leer();
        public Task<Rango> Buscar(int id);
    }
}
