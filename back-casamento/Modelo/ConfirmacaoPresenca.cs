using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace back_casamento.Modelo
{
    public class ConfirmacaoPresenca : ToEmail
    {
        public int id { get; set; }
        public string nome { get; set; }
        public string email { get; set; }
        public int quantidadeAdultos { get; set; }
        public int quantidadeCriancas { get; set; }
        public string mensagem { get; set; }
        public DateTime data { get; set; }
        public bool enviado { get; set; }
        public DateTime dtEnviado { get; set; }

        public static string caminhoArquivo = "ConfirmacaoPresenca.txt";

        public void posEnvio()
        {
            return;
        }

        public static void salvar(ConfirmacaoPresenca c)
        {
            List<ConfirmacaoPresenca> confP = ConfirmacaoPresenca.GetListaConfirmaPresenca();
            c.data = DateTime.Now;
            confP.Add(c);
            File.WriteAllText(caminhoArquivo, JsonConvert.SerializeObject(confP));
        }

        public static List<ConfirmacaoPresenca> GetListaConfirmaPresenca()
        {
            string json = !File.Exists(ConfirmacaoPresenca.caminhoArquivo) ? 
                String.Empty :
                File.ReadAllText(ConfirmacaoPresenca.caminhoArquivo);

            List<ConfirmacaoPresenca> confP = String.IsNullOrEmpty(json) ? 
                new List<ConfirmacaoPresenca>() : 
                JsonConvert.DeserializeObject<List<ConfirmacaoPresenca>>(json).ToList();

            return confP;
        }

        public static void SetaConfirmacoesEnviadas()
        {
            List<ConfirmacaoPresenca> listConfPresenca = ConfirmacaoPresenca.GetListaConfirmaPresenca();

            foreach (ConfirmacaoPresenca c in listConfPresenca) {
                c.enviado = true;
                c.dtEnviado = DateTime.Now;
            }

            File.WriteAllText(caminhoArquivo, JsonConvert.SerializeObject(listConfPresenca));
        }

        public void posEnAvio() => ConfirmacaoPresenca.SetaConfirmacoesEnviadas();

            
        private string ToLinhaCSV () => 
            "\"" + this.nome.Replace(';', '•') + "\";" +
            "\"" + this.quantidadeAdultos + "\";" +
            "\"" + this.quantidadeCriancas + "\";" +
            "\"" + this.data.ToString("dd/MM/yyyy HH:MM") + "\";" +
            "\"" + this.dtEnviado.ToString("dd/MM/yyyy HH:MM") + "\";";

        public static string _getEmailMensagem()
        {
            string confirmacoes = "\"Nome\";\"QuantidadeAdultos\";\"QuantidadeCriancas\";\"DataEnvio\";\"DataConfirmacao\";" + Environment.NewLine;

            List<ConfirmacaoPresenca> confAtual = ConfirmacaoPresenca.GetListaConfirmaPresenca();

            foreach (ConfirmacaoPresenca c in confAtual)
                confirmacoes += c.ToLinhaCSV() + Environment.NewLine;


            return confirmacoes;
        }

        public string getEmailMensagem()
        {
            string confirmacoes = "\"Nome\";\"QuantidadeAdultos\";\"QuantidadeCriancas\";\"DataConfirmacao\";" + Environment.NewLine;
            List<ConfirmacaoPresenca> confAtual = ConfirmacaoPresenca.GetListaConfirmaPresenca();

            foreach (ConfirmacaoPresenca c in confAtual)
                confirmacoes += c.ToLinhaCSV() + Environment.NewLine;
            

            return confirmacoes;
        }

        public string getNome() => this.nome;

        public TipoEmail GetTipo() => TipoEmail.presenca;

        public string fromEmail() => "maquinaCasorio@rafaemila.com.br";

        public List<EmailAddress> getEmailDestinatario() => new List<EmailAddress>() {
                new EmailAddress("rafontas@gmail.com", "Rafael Freitas"),
                new EmailAddress("kmilaxavier@hotmail.com", "Camila Xavier")
         };

    }
}
