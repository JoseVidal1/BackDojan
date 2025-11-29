using DAL;
using DAL.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class ServiceArticulo:IServiceArticulo
    {
        private readonly  IDBArticulo dBArticulo;
        public ServiceArticulo(IDBArticulo dBArticulo)
        {
            this.dBArticulo = dBArticulo;
        }
        public async Task<bool> Agregar(Articulo agregar)
        {
            if (await dBArticulo.Buscar(agregar.Id) == null)
            {
                await dBArticulo.Agregar(agregar);
                return true;
            }
            throw new ArgumentException("Articulo existente");
        }
        public async Task<List<Articulo>> Leer()
        {
            return await dBArticulo.Leer();
        }
        public async Task<Articulo> Buscar(int id)
        {
            return await dBArticulo.Buscar(id);
        }
        public async Task<bool> Actualizar(Articulo actualizado)
        {
            return await dBArticulo.Actualizar(actualizado);
        }
        public async Task<bool> Eliminar(int id)
        {
            return await dBArticulo.Eliminar(id);
        }
        
    }
}
