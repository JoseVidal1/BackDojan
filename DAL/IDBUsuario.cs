using DAL.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public interface IDBUsuario
    {
        public  Task Agregar(Usuario usuario);
        public  Task<List<Usuario>> Leer();
        public  Task<bool> Actualizar(Usuario actualizado);
        public  Task<Usuario> Buscar(string cc);
        public  Task<bool> Eliminar(string cc);
    }
}
