using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http.Cors;
using back_casamento.Modelo;
using Microsoft.AspNetCore.Mvc;

namespace back_casamento.Controllers
{
    [Route("api/[controller]")]
    [EnableCors(origins: "http://localhost:4200/", headers: "*", methods: "*")]
    [ApiController]
    public class ConfirmacaoPresencaController : Controller
    {

        [HttpGet]
        public ActionResult<IEnumerable<ConfirmacaoPresenca>> Get()
        {
            return new ConfirmacaoPresenca[] { new ConfirmacaoPresenca() {
                data = DateTime.Now,
                email = "Teste@teste.com.br",
                mensagem = "Mensagem de teste.",
                nome = "Nome teste",
                quantidadeAdultos = 1,
                quantidadeCriancas = 2
            } };
        }

        [HttpPost]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public async Task<ActionResult<ConfirmacaoPresenca>> PostConfirmacaoPresenca([FromBody] ConfirmacaoPresenca value)
        {
            try
            {
                // Envia com a confirmação de presença
                if (value.nome.ToLower() == "confirmação de presença" && value.quantidadeAdultos == 0 && value.quantidadeCriancas == 0)
                { 
                    await Email.SendEmail<ConfirmacaoPresenca>(value);
                    return Ok(new RespostaBase(situacaoResposta.EmailEnviadoConfPres));
                }

                // Envia com a confirmação de presença
                if (string.IsNullOrWhiteSpace(value.nome) || value.quantidadeAdultos <= 0 || value.quantidadeCriancas < 0 ) 
                    return Ok(new RespostaBase(situacaoResposta.SucessoErroNegocio));

                ConfirmacaoPresenca.salvar(value);
                return Ok(new RespostaBase(situacaoResposta.Sucesso));

            }
            catch
            {
                return  Ok(new RespostaBase(situacaoResposta.Erro));
            }
        }
    }
}
