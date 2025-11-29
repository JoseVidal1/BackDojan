using DAL;
using DAL.Modelos;
using DTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class ServicePrestamo:IServicePrestamo
    {
        private readonly IDBPrestamo dBPrestamo;
        private readonly IServiceArticulo serviceArticulo;
        private readonly IServiceEstudiante serviceEstudiante;
        public ServicePrestamo(IDBPrestamo dBPrestamo, IServiceArticulo serviceArticulo, IServiceEstudiante serviceEstudiante)
        {
            this.dBPrestamo = dBPrestamo;
            this.serviceArticulo = serviceArticulo;
            this.serviceEstudiante = serviceEstudiante;
        }
        public async Task<bool> VerificarDisponibilidad(ICollection<DetallePrestamo> detallePrestamos)
        {
            foreach (var item in detallePrestamos)
            {
                var articulo = await serviceArticulo.Buscar(item.IdArticulo);
                if (item.Cantidad > articulo.Disponibles)
                {
                    return false;
                }
            }
            return true;
        }
        public async Task<bool> Alquilar(Prestamo agregar)
        {
            Estudiante estudiante = await serviceEstudiante.Buscar(agregar.EstudianteId);
            if (estudiante.estado=="Inactivo") 
                throw new ArgumentException("Al estudiante no se le puede prestar estado inactivo");
            if (!await VerificarDisponibilidad(agregar.DetallePrestamos)) 
                throw new ArgumentException("No existen disponibles");
            if(await PrestamoEstudiante(agregar.EstudianteId) != null)
                throw new ArgumentException("El estudiante ya tiene un prestamo activo");
            foreach (var item in agregar.DetallePrestamos)
            {
                Articulo articulo = await serviceArticulo.Buscar(item.IdArticulo);
                articulo.Disponibles = articulo.Disponibles - item.Cantidad;
                await serviceArticulo.Actualizar(articulo);
            }
            agregar.Estado = "En prestamo";
            await dBPrestamo.Agregar(agregar);
            return true;

        }
        public async Task<Prestamo> PrestamoEstudiante(string IdEstudiante)
        {
            List<Prestamo> prestamos = await dBPrestamo.Leer();
            var prestamosEstudiante=prestamos.Where(p => p.Estudiante.Id == IdEstudiante).ToList();
            return prestamosEstudiante.FirstOrDefault(p => p.Estado == "En prestamo");
        }
        public async Task EnMora()
        {
            List<Prestamo> prestamos = await dBPrestamo.Leer();
            DateTime fechaActual = DateTime.Now;
            foreach (var prestamo in prestamos)
            {
                if (prestamo.Estado == "En prestamo" && prestamo.FechaDevolucion.ToDateTime(TimeOnly.MinValue) < fechaActual)
                {
                    prestamo.Estado = "En mora";
                    await NotificacionesCorreo.EnviarCorreoAsync(prestamo.Estudiante.Correo, "Prestamo en Mora", $"El prestamo está en mora. Por favor, devuélvelo lo antes posible.");
                    await dBPrestamo.Actualizar(prestamo);
                }
            }
        }
        public async Task<bool> Devolver(int id)
        {
            Prestamo prestamo = await Buscar(id);
            if (prestamo == null || prestamo.Estado=="Devuelto") return false;
    
            foreach (var item in prestamo.DetallePrestamos)
            {
                Articulo articulo = await serviceArticulo.Buscar(item.IdArticulo);
                articulo.Disponibles = articulo.Disponibles + item.Cantidad;
                await serviceArticulo.Actualizar(articulo);
            }
            prestamo.Estado = "Devuelto";
            await dBPrestamo.Actualizar(prestamo);
            return true;
        }
        public async Task<List<Prestamo>> Leer()
        {
            return await dBPrestamo.Leer();
        }
        public async Task<Prestamo> Buscar(int id)
        {
            return await dBPrestamo.Buscar(id);
        }
        public async Task<bool> Actualizar(Prestamo actualizado)
        {
            return await Devolver(actualizado.Id);
        }
        public async Task<bool> Eliminar(int id)
        {
            return await dBPrestamo.Eliminar(id);
        }
        public async Task<List<Prestamo>> EnPrestamo()
        {
            var prestamos = await Leer();
            return prestamos.Where(p => p.Estado == "En Prestamo").ToList();
        }
        public PrestamoDTO MapearPrestamoADTO(Prestamo prestamo)
        {
            return new PrestamoDTO
            {
                Id = prestamo.Id,
                EstudianteId = prestamo.EstudianteId,
                FechaPrestamo = prestamo.FechaPrestamo,
                FechaDevolucion = prestamo.FechaDevolucion,
                Estado = prestamo.Estado,
                Detalles = prestamo.DetallePrestamos.Select(dp => new DetallePrestamoDTO
                {
                    Id = dp.Id,
                    IdArticulo = dp.IdArticulo,
                    Cantidad = dp.Cantidad,
                    Articulo = dp.IdArticuloNavigation != null ? new ArticuloDTO
                    {
                        Id = dp.IdArticuloNavigation.Id,
                        Nombre = dp.IdArticuloNavigation.Nombre,
                        Cantidad = dp.IdArticuloNavigation.Cantidad,
                        Disponibles = dp.IdArticuloNavigation.Disponibles
                    } : null
                }).ToList(),
                Estudiante = prestamo.Estudiante != null ? new EstudianteDTO
                {
                    Id = prestamo.Estudiante.Id,
                    Nombres = prestamo.Estudiante.Nombres,
                    Apellidos = prestamo.Estudiante.Apellidos,
                    Telefono = prestamo.Estudiante.Telefono,
                    Correo = prestamo.Estudiante.Correo,
                    Direccion = prestamo.Estudiante.Direccion,
                    Eps = prestamo.Estudiante.Eps,
                    IdRango = prestamo.Estudiante.IdRango,
                    IdGrupo = prestamo.Estudiante.IdGrupo,
                    Edad = prestamo.Estudiante.edad,
                    FechaNacimiento = prestamo.Estudiante.FechaNacimiento
                } : null
            };
        }

    }
}
