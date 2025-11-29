using DAL.Modelos;
using DTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public interface IServiceUsuario
    {
        public Task<bool> Agregar(Usuario agregar);
        public Task<List<Usuario>> Leer();
        public Task<Usuario> Buscar(string cc);
        public Task<Usuario> Actualizar(Usuario usuario);
        public Task<bool> Eliminar(string cc);
        public Task<Usuario> Login(UsuarioDTO usuarioDTO);
    }
}
