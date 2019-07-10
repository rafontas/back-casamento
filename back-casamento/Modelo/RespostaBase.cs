using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace back_casamento.Modelo
{
    public class RespostaBase
    {
        public bool status { get; set; } = true;
        public situacaoResposta situacao { get; set; }
        public string mensagemSucesso { get; set; }

        public RespostaBase(situacaoResposta status) {
            switch (status) {
                case situacaoResposta.Sucesso:
                    this.situacao = situacaoResposta.Sucesso;
                    this.mensagemSucesso = "Sucesso!";
                    break;

                case situacaoResposta.EmailContatoEnviado:
                    this.situacao = situacaoResposta.EmailContatoEnviado;
                    this.mensagemSucesso = "Email's enviados com sucesso!";
                    break;

                case situacaoResposta.EmailEnviadoConfPres:
                    this.situacao = situacaoResposta.EmailEnviadoConfPres;
                    this.mensagemSucesso = "Sucesso!";
                    break;

                case situacaoResposta.SucessoErroNegocio:
                    this.situacao = situacaoResposta.EmailEnviadoConfPres;
                    this.mensagemSucesso = "Preencha algo.";
                    break;

                default:
                    this.situacao = situacaoResposta.Erro;
                    this.mensagemSucesso = "Erro";
                    this.status = false;
                    break;
            }
        }
    }

    public enum situacaoResposta
    {
        Sucesso = 1,
        EmailContatoEnviado = 2,
        EmailEnviadoConfPres = 3,
        Erro = 4,
        SucessoErroNegocio = 5
    }
}
