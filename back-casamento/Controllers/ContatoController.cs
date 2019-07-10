using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http.Cors;
using back_casamento.Modelo;
using Microsoft.AspNetCore.Mvc;

namespace back_casamento.Controllers
{
    [Route("api/[controller]")]
    [EnableCors(origins: "http://localhost:4200/", headers: "*", methods: "*")]
    [ApiController]
    public class ContatoController : Controller
    {
        [HttpGet]
        public ActionResult<IEnumerable<Contato>> Get()
        {
            return new Contato[] { new Contato()
            {
                assunto = "Assunto 1",
                email = "teste@email.com",
                nome = "NomeTeste",
                mensagem = "Mensagem de teste bacana",
                data = DateTime.Now
            } };
        }

        // POST api/values
        [HttpPost]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public async Task<ActionResult<RespostaBase>> PostContato([FromBody] Contato value)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(value.email) || string.IsNullOrWhiteSpace(value.nome))
                    return Ok(new RespostaBase(situacaoResposta.SucessoErroNegocio));

                value.salvar();
                await Email.SendEmail<Contato>(value);

                value.tipo = TipoEmail.contatoConvidado;
                await Email.SendEmail<Contato>(value);

                return Ok(new RespostaBase (situacaoResposta.Sucesso));
            }
            catch
            {
                return Ok(new RespostaBase(situacaoResposta.Erro));
            }
        }
    }
}