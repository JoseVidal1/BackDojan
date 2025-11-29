using DAL.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public interface IDBGrupo
    {
        public Task<List<Grupo>> Leer();

        public Task<Grupo> Buscar(int id);
    }
}
