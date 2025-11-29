using DAL;
using DAL.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public interface IServicePago
    {
        public  Task ModificarEstadoEstudiante();
        public  Task<Pago> ActualizarPago(Pago actualizado);
        public  Task AgregarPago(Pago pago);
        public  Task GenerarPagos();
        public  Task<List<Pago>> Leer();
    }
}
