using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
            string json = File.ReadAllText(ConfirmacaoPresenca.caminhoArquivo);
            List<ConfirmacaoPresenca> confP = JsonConvert.DeserializeObject<List<ConfirmacaoPresenca>>(json);
            confP.Add(c);
            File.WriteAllText(caminhoArquivo, JsonConvert.SerializeObject(confP));
        }

        public static void salvar(List<ConfirmacaoPresenca> list)
        {
            string json = File.ReadAllText(ConfirmacaoPresenca.caminhoArquivo);
            List<ConfirmacaoPresenca> confP = JsonConvert.DeserializeObject<List<ConfirmacaoPresenca>>(json);
            File.WriteAllText("ConfEnviado" + DateTime.Now.ToString("yyyyMMdd-HHmmss") + "_BKP.txt", JsonConvert.SerializeObject(confP));
            File.WriteAllText(caminhoArquivo, JsonConvert.SerializeObject(list));
        }

        public static List<ConfirmacaoPresenca> GetListaConfirmaPresenca()
        {
            string json = File.ReadAllText(ConfirmacaoPresenca.caminhoArquivo);
            return JsonConvert.DeserializeObject<List<ConfirmacaoPresenca>>(json);
        }

        public static void SetaConfirmacoesEnviadas()
        {
            List<ConfirmacaoPresenca> listConfPresenca = ConfirmacaoPresenca.GetListaConfirmaPresenca();

            foreach (ConfirmacaoPresenca c in listConfPresenca) {
                c.enviado = true;
                c.dtEnviado = DateTime.Now;
            }

            ConfirmacaoPresenca.salvar(listConfPresenca);
        }

        public void posEnAvio() => ConfirmacaoPresenca.SetaConfirmacoesEnviadas();

            
        private string ToLinhaCSV () => (this.nome.Replace(';', '•') + ";" + this.quantidadeAdultos + ";" + this.quantidadeCriancas + ";" + this.data.ToString("dd/MM/yyyy HH:MM") + ";" + this.email + ";" + this.mensagem.Replace(';', '•').ToString()) ;

        public string toEmail()
        {
            string confirmacoes = "Nome;QuantidadeAdultos;QuantidadeCriancas;DataConfirmacao;Email;Mensagem" + Environment.NewLine;
            List<ConfirmacaoPresenca> confAtual = ConfirmacaoPresenca.GetListaConfirmaPresenca();

            foreach (ConfirmacaoPresenca c in confAtual) {
                if (c.enviado) continue;

                confirmacoes += c.ToLinhaCSV() + Environment.NewLine;
            }

            return confirmacoes;
        }

        public string getNome() => this.nome;

        public TipoEmail GetTipo() => TipoEmail.presenca;
    }
}
