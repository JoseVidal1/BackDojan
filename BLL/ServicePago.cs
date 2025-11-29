using DAL;
using DAL.Modelos;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class ServicePago: IServicePago
    {
        private readonly IDBPago dBPago;
        private readonly IServiceEstudiante serviceEstudiante;
        public ServicePago(IDBPago dBPago, IServiceEstudiante serviceEstudiante)
        {
            this.dBPago = dBPago;
            this.serviceEstudiante = serviceEstudiante;
        }
        public async Task ModificarEstadoEstudiante()
        {
            var estudiantes = await serviceEstudiante.Leer();
            var hoy = DateTime.Today;

            foreach (var item in estudiantes)
            {
                if (item.estado == "activo" && item.FechaRegistro.HasValue)
                {
                    var fechaRegistro = item.FechaRegistro.Value.ToDateTime(TimeOnly.MinValue);

                    // Verifica que el día del mes coincida
                    bool mismoDiaMes = fechaRegistro.Day == hoy.Day;

                    // Y que no sea hoy la fecha exacta
                    bool noEsHoy = fechaRegistro.Date != hoy;

                    if (mismoDiaMes && noEsHoy)
                    {
                        item.estado = "pendiente";
                        await serviceEstudiante.Actualizar(item);
                    }
                }
            }
        }
        public async Task<Pago> ActualizarPago(Pago actualizado)
        {

                actualizado.Estado="pagado";
                if(actualizado.IdEstudianteNavigation.estado=="Inactivo")
                {
                    actualizado.IdEstudianteNavigation.estado = "Activo";
                    await serviceEstudiante.Actualizar(actualizado.IdEstudianteNavigation);
                }
                return await dBPago.Actualizar(actualizado);
            

        }
        public async Task AgregarPago(Pago pago)
        {
            await dBPago.Agregar(pago);
        }
        public async Task GenerarPagos()
        {
            List<Estudiante> estudiantes = await serviceEstudiante.Leer();

            foreach (var estudiante in estudiantes)
            {
                var ultimoPago = await dBPago.Buscar(estudiante.Id);
                DateTime fechaBase = ultimoPago?.FechaPago.ToDateTime(TimeOnly.MinValue)
                                    ?? estudiante.FechaRegistro!.Value.ToDateTime(TimeOnly.MinValue);

                DateTime fechaProximoPago = fechaBase.AddMonths(1);

                if (DebeGenerarPago(fechaProximoPago, estudiante.estado))
                {
                    await CrearPagoPendiente(estudiante, fechaProximoPago);
                }

                if (DebeEnviarRecordatorio(fechaProximoPago))
                {
                    await EnviarRecordatorio(estudiante, fechaProximoPago);
                }
            }
        }
        private bool DebeGenerarPago(DateTime fechaProximoPago, string estadoEstudiante)
        {
            return fechaProximoPago <= DateTime.Today && estadoEstudiante == "Activo";
        }

        private bool DebeEnviarRecordatorio(DateTime fechaProximoPago)
        {
            var diasRestantes = (fechaProximoPago - DateTime.Today).TotalDays;
            return diasRestantes <= 3 && diasRestantes >= 0;
        }

        private async Task CrearPagoPendiente(Estudiante estudiante, DateTime fechaPago)
        {
            Pago nuevoPago = new Pago
            {
                IdEstudiante = estudiante.Id,
                FechaPago = DateOnly.FromDateTime(fechaPago),
                Estado = "Pendiente"
            };

            await dBPago.Agregar(nuevoPago);

            estudiante.estado = "Inactivo";
            await serviceEstudiante.Actualizar(estudiante);

            await EnviarCorreoPagoPendiente(estudiante, fechaPago);
        }

        private async Task EnviarCorreoPagoPendiente(Estudiante estudiante, DateTime fechaPago)
        {
            try
            {
                await NotificacionesCorreo.EnviarCorreoAsync(
                    estudiante.Correo,
                    "Pago Pendiente",
                    $"Estimado {estudiante.Nombres} {estudiante.Apellidos}, su pago está pendiente" +
                    $" desde el día {fechaPago:dd/MM/yyyy}.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al enviar correo de pago pendiente: {ex.Message}");
            }
        }

        private async Task EnviarRecordatorio(Estudiante estudiante, DateTime fechaPago)
        {
            try
            {
                await NotificacionesCorreo.EnviarCorreoAsync(
                    estudiante.Correo,
                    "Recordatorio de Pago",
                    $"Estimado {estudiante.Nombres} {estudiante.Apellidos}, " +
                    $"su próximo pago es el día {fechaPago:dd/MM/yyyy}.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al enviar recordatorio: {ex.Message}");
            }
        }


        public async Task<List<Pago>> Leer()
        {
            return await dBPago.Leer();
        }
    }
}
