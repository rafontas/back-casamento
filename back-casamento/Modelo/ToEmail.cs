using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace back_casamento.Modelo
{
    interface ToEmail
    {
        string getEmailMensagem();

        List<EmailAddress> getEmailDestinatario();

        string fromEmail();

        TipoEmail GetTipo();

        string getNome();

        void posEnvio();
    }
}
