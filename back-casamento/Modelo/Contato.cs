﻿using Newtonsoft.Json;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace back_casamento.Modelo
{
    public class Contato : ToEmail
    {
        public string nome { get; set; }
        public string email { get; set; }
        public string assunto { get; set; }
        public string mensagem { get; set; }
        public DateTime data { get; set; }
        public bool enviado { get; set; }    
        public DateTime dtEnviado { get; set; }
        public TipoEmail tipo { get; set; } = TipoEmail.contatoNoivos;

        public static string caminhoArquivo = "Contato.txt";

        public Contato() => this.data = DateTime.Now;

        public void salvar()
        {
            string json = JsonConvert.SerializeObject(this) + Environment.NewLine;
            File.AppendAllText(Contato.caminhoArquivo, json);
        }

        public static List<Contato> GetListaContato()
        {
            string json = File.ReadAllText(Contato.caminhoArquivo);
            List<Contato> l = JsonConvert.DeserializeObject<List<Contato>>(json);

            if (l.Count <= 0)
            {
                l.Add(new Contato() {
                    assunto = "Não há nenhum contato."
                });
            }

            return l;
        }

        public string getEmailMensagem() => 
            this.GetTipo() == TipoEmail.contatoConvidado ? this.toEmailConvidado() : this.toEmailNoivos();

        private string toEmailConvidado()
        {
            string msg = "Muito obrigado pelo contato. Em breve responderemos!";
            return msg;
        }
        public string toEmailNoivos()
        {
            string msg = "<br /><strong>Nome</strong>: " + this.nome + "<br>";
            msg += "<strong>Email</strong>: " + this.email + "<br>";
            msg += "<strong>Assunto</strong>: " + this.assunto + "<br>";
            msg += "<strong>Data</strong>: " + this.data + "<br>";
            msg += "<strong>Mensagem</strong>: " + this.mensagem + "<br /><br />";

            return msg;
        }

        public string getNome() => this.nome;

        public void posEnvio()
        {
            throw new NotImplementedException();
        }

        public TipoEmail GetTipo() => this.tipo;

        public string fromEmail() =>
            this.GetTipo() == TipoEmail.contatoConvidado ? "rafaemila@rafaemila.com.br" : "maquinaCasorio@rafaemila.com.br";

        public List<EmailAddress> getEmailDestinatario()
        {
            if (this.GetTipo() == TipoEmail.contatoConvidado)
            {
                return new List<EmailAddress>() {
                    new EmailAddress(this.email, this.nome)
                };
            }

            return new List<EmailAddress>() {
                new EmailAddress("rafontas@gmail.com", "Rafael Freitas"),
                new EmailAddress("kmilaxavier@hotmail.com", "Camila Xavier"),
            };
        }
    }
}
