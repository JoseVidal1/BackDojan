using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Modelos;
using DTOS;
namespace BLL
{
    public interface IServicePrestamo
    {
        Task<bool> VerificarDisponibilidad(ICollection<DetallePrestamo> detallePrestamos);

        Task<bool> Alquilar(Prestamo agregar);

        Task<Prestamo> PrestamoEstudiante(string IdEstudiante);

        Task EnMora();

        Task<bool> Devolver(int id);

        Task<List<Prestamo>> Leer();

        Task<Prestamo> Buscar(int id);

        Task<bool> Actualizar(Prestamo actualizado);

        Task<bool> Eliminar(int id);

        Task<List<Prestamo>> EnPrestamo();

        PrestamoDTO MapearPrestamoADTO(Prestamo prestamo);
    }

}
