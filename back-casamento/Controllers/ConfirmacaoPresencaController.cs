﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http.Cors;
using back_casamento.Modelo;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace back_casamento.Controllers
{
    [Route("api/[controller]")]
    [EnableCors(origins: "http://localhost:4200/", headers: "*", methods: "*")]
    [ApiController]
    public class ConfirmacaoPresencaController : Controller
    {

        //[HttpGet]
        //public ActionResult<IEnumerable<ConfirmacaoPresenca>> Get()
        //{
        //    return new ConfirmacaoPresenca[] { new ConfirmacaoPresenca() {
        //        data = DateTime.Now,
        //        email = "Teste@teste.com.br",
        //        mensagem = "Mensagem de teste.",
        //        nome = "Nome teste",
        //        quantidadeAdultos = 1,
        //        quantidadeCriancas = 2
        //    } };
        //}

        //[HttpGet]
        //[EnableCors(origins: "*", headers: "*", methods: "*")]
        //public ActionResult<string> Get(int meuParam = 0, int meuParam2 = 0, int meuParam3 = 0)
        //{
        //    if (meuParam != 234 || meuParam2 != 17801 || meuParam3 != 11029)
        //    {
        //        return " Uhull ! ";
        //    }

        //    return ConfirmacaoPresenca._GetListaConfirmaPresenca();
        //}

        [HttpGet]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public async Task<ActionResult<string>> Get(int meuParam = 0, int meuParam2 = 0, int meuParam3 = 0)
        {
            if (meuParam != 234 || meuParam2 != 17801 || meuParam3 != 11029)
            {
                return new ConfirmacaoPresenca()
                    {
                        data = DateTime.Now,
                        email = "Teste@teste.com.br",
                        mensagem = "Mensagem de teste.",
                        nome = "Nome teste",
                        quantidadeAdultos = 1,
                        quantidadeCriancas = 2
                    }
                    .ToString();
            }

            ConfirmacaoPresenca.SetaConfirmacoesEnviadas();

            return Ok(ConfirmacaoPresenca._getEmailMensagem());

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
            catch (Exception e)
            {
                var error = new Dictionary<string, string>
                {
                    { "Type", e.GetType().ToString()},
                    { "Message", e.Message},
                    { "StackTrace", e.StackTrace}
                };

                foreach (DictionaryEntry data in e.Data)
                    error.Add(data.Key.ToString(), data.Value.ToString());

                string json = JsonConvert.SerializeObject(error, Formatting.Indented);
                
                return Ok(json);
            }
        }
    }
}
