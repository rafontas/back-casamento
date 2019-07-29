using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace back_casamento.Modelo
{
    public class Email
    {
        public static IConfiguration Config;

        private static string getAssunto<T>(T email) where T : ToEmail
        {
            string assunto = string.Empty;

            switch (email.GetTipo())
            {
                case TipoEmail.contatoConvidado:
                    assunto += "Obrigado pelo Contato! ";
                    break;

                case TipoEmail.contatoNoivos:
                    assunto += "[Casório] Contato de " + email.getNome();
                    break;

                case TipoEmail.presenca:
                    assunto += "[Casório] " + email.getNome() + " confirmou presença no casório! S2";
                    break;

            }

            return assunto;
        }

        internal async static Task SendEmail<T>(T email) where T : ToEmail 
        {
            //var apiKey = Email.Config.GetSection("API_KEY_SENDGRID").Value;
            var apiKey = System.Environment.GetEnvironmentVariable("API_KEY_SENDGRID");
            //string milaEmail  = System.Environment.GetEnvironmentVariable("EMAIL_RAFA");

            var client = new SendGridClient(apiKey);

            string assunto = Email.getAssunto<ToEmail>(email);

            var msg = new SendGridMessage() {
                From = new EmailAddress(email.fromEmail(), "Rafa e Mila"),
                Subject = assunto,
                PlainTextContent = "Conteúdo teste de email.",
                HtmlContent = "<div style='background-color: #ededed; border-radius: 5px; color:#54006e; padding: 10px;'>" + email.getEmailMensagem() + "</div>"
            };

            var recebedores = email.getEmailDestinatario();
            msg.AddTos(recebedores);

            Response response = await client.SendEmailAsync(msg);

            email.posEnvio();
        }

    }

    public enum TipoEmail
    {
        contatoConvidado = 1,
        presenca = 2,
        contatoNoivos = 3
    }
}
