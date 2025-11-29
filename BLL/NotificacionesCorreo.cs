using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class NotificacionesCorreo
    {
        public static async Task EnviarCorreoAsync(string destinatario, string asunto, string mensaje)
        {
            var remitente = "academiataekwondonovalito@gmail.com";
            var contraseña = "rbee tnfz koas jrcl";
            var smtp = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential(remitente, contraseña),
                EnableSsl = true
            };
            var enviar = new MailMessage(remitente, destinatario, asunto, mensaje);
            await smtp.SendMailAsync(enviar);
        }

    }
}
