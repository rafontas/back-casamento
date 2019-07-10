using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace back_casamento.Modelo
{
    interface ToEmail
    {
        string toEmail();

        TipoEmail GetTipo();

        string getNome();

        void posEnvio();
    }
}
