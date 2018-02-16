using AutoMapper;
using Clinicas.Application.Services.Interfaces;
using Clinicas.Domain.Model;
using Clinicas.Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Clinicas.Api.Controllers
{
    [RoutePrefix("funcionario")]
    public class FuncionarioController : ApiController
    {
        private readonly IFuncionarioService _service;

        public FuncionarioController(IFuncionarioService service)
        {
            _service = service;
        }
        
        //#region [Cadastro de Funcionarios]
        //[HttpGet]
        //[Route("getFuncionarioById")]
        //public HttpResponseMessage ObterFuncionarioPorId(int id)
        //{
        //    try
        //    {
        //        var model = new FuncionarioViewModel();

        //        var result = _service.ObterFuncionariPorId(id);

        //        if (result != null)
        //        {
        //            Mapper.CreateMap<Funcionario, FuncionarioViewModel>();
        //            model = Mapper.Map<Funcionario, FuncionarioViewModel>(result);

        //            if (!string.IsNullOrEmpty(model.TelResidencial))
        //                model.TelResidencial = model.TelResidencial.Trim().Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "");

        //            if (!string.IsNullOrEmpty(model.TelCelular))
        //                model.TelCelular = model.TelCelular.Trim().Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "");

        //            if (!string.IsNullOrEmpty(model.Cep))
        //                model.Cep = model.Cep.Replace("-", "").Trim();

        //            if (!string.IsNullOrEmpty(model.CPF))
        //                model.CPF = model.CPF.Replace(".", "").Replace("-", "").Trim();

        //        }
        //        else
        //            return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Não foi possível recuperar dados do funcionário.");

        //        return Request.CreateResponse(HttpStatusCode.OK, model);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
        //    }
        //}

        //[HttpGet]
        //[Route("listarProfissionaisAtivos")]
        //public HttpResponseMessage ListarProfissionaisSaudeAtivos()
        //{
        //    try
        //    {
        //        var model = new List<FuncionarioViewModel>();
        //        var result = _service.ListarFuncionariosDeSaudeAtivos();

        //        if (result != null)
        //        {
        //            foreach(var item in result)
        //            {
        //                model.Add(new FuncionarioViewModel()
        //                {
        //                    NmFuncionario = item.Pessoa.Nome,
        //                    IdFuncionario = item.IdFuncionario
        //                });
        //            }
        //            //Mapper.CreateMap<Funcionario, FuncionarioViewModel>();
        //            //model = Mapper.Map< List<Funcionario>, List<FuncionarioViewModel>>(result);
        //        }
        //        else
        //            return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Não foi possível recuperar dados do funcionário.");

        //        return Request.CreateResponse(HttpStatusCode.OK, model);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
        //    }
        //}

        //[HttpGet]
        //[Route("listarProfissionais")]
        //public HttpResponseMessage ListarProfissionaisSaude()
        //{
        //    try
        //    {
        //        var model = new List<FuncionarioViewModel>();

        //        var result = _service.ListarFuncionariosDeSaude();

        //        if (result != null)
        //        {
        //            Mapper.CreateMap<Funcionario, FuncionarioViewModel>();
        //            model = Mapper.Map<List<Funcionario>, List<FuncionarioViewModel>>(result);

        //        }
        //        else
        //            return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Não foi possível recuperar dados do funcionário.");

        //        return Request.CreateResponse(HttpStatusCode.OK, model);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
        //    }
        //}

        //[HttpPost]
        //[Route("saveFuncionario")]
        //public HttpResponseMessage SalvarFuncionario(FuncionarioViewModel model)
        //{

        //    return Request.CreateResponse(HttpStatusCode.OK, model);

        //    /* try
        //    {
        //        string situacao = "";
        //        if (model.IdFuncionario > 0)
        //        {
        //            var funcionario = _service.ObterFuncionariPorId(model.IdFuncionario);
        //            if (funcionario == null)
        //                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Não foi possível obter dados do funcionário.");

        //            funcionario.Pessoa.SetCpfCnpj(model.CPF);
        //            funcionario.Pessoa.SetDataNascimento(model.DtNascimento);
        //            funcionario.Pessoa.SetMae(model.Mae);
        //            funcionario.Pessoa.SetNome(model.NmFuncionario.ToUpper());
        //            funcionario.SetRegistroConselho(model.RegistroConselho);
        //            funcionario.Pessoa.SetRg(model.RG);
        //            funcionario.Pessoa.SetSexo(model.Sexo);

        //            if (model.Situacao == "A")
        //                situacao = "ATIVO";
        //            else
        //                situacao = "INATIVO";

        //            funcionario.Pessoa.SetSituacao(situacao);
        //            funcionario.Pessoa.SetTipo(model.Tipo);
        //            funcionario.Pessoa.SetEndereco(model.Cep, null, null, model.Bairro, model.Logradouro, model.Nr, model.Complemento, model.Referencia);
                       
        //            funcionario.Pessoa.SetObservacoes(model.Observacao);
        //            funcionario.Pessoa.SetPai(model.Pai);
        //            funcionario.Pessoa.SetTelefoneCelular(model.TelCelular);
        //            funcionario.Pessoa.SetTelefoneComercial(model.TelResidencial);


        //            _service.SalvarFuncionario(funcionario);
        //        }
        //        else
        //        {

        //            if (model.Situacao == "A")
        //                situacao = "ATIVO";
        //            else
        //                situacao = "INATIVO";

        //            var funcionario = new Funcionario(model.NmFuncionario.ToUpper(), model.Sexo, model.RG, model.DtNascimento, model.CPF, model.Mae, situacao, model.Tipo);
        //            funcionario.SetRegistroConselho(model.RegistroConselho);

        //            funcionario.Pessoa.SetEndereco(model.Cep, null, null, model.Bairro, model.Logradouro, model.Nr, model.Complemento, model.Referencia);


        //            funcionario.Pessoa.SetObservacoes(model.Observacao);
        //            funcionario.Pessoa.SetPai(model.Pai);
        //            funcionario.SetTelCelular(model.TelCelular);
        //            funcionario.SetTelResidencial(model.TelResidencial);

        //            _service.SalvarFuncionario(funcionario);
        //        }

        //        return Request.CreateResponse(HttpStatusCode.OK, model);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
        //    }  */
        //}
        //[HttpGet]
        //[Route("listarfuncionarios")]
        //public HttpResponseMessage ListarFuncionarios()
        //{
        //    try
        //    {
        //        var model = new List<FuncionarioViewModel>();

        //        var result = _service.ListarFuncionarios();

        //        if (result != null)
        //        {
        //            Mapper.CreateMap<Funcionario, FuncionarioViewModel>();
        //            model = Mapper.Map<List<Funcionario>, List<FuncionarioViewModel>>(result);

        //        }
        //        else
        //            return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Não foi possível recuperar dados do funcionário.");

        //        return Request.CreateResponse(HttpStatusCode.OK, model);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
        //    }
        //}
        //[HttpGet]
        //[Route("listarfuncionariosPorNome")]
        //public HttpResponseMessage ListarFuncionariosPorNome(string nome)
        //{
        //    try
        //    {
        //        var model = new List<FuncionarioViewModel>();

        //        var result = _service.ListarFuncionariosPorNome(nome);

        //        if (result != null)
        //        {
        //            Mapper.CreateMap<Funcionario, FuncionarioViewModel>();
        //            model = Mapper.Map<List<Funcionario>, List<FuncionarioViewModel>>(result);

        //        }
        //        else
        //            return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Não foi possível recuperar dados do funcionário.");

        //        return Request.CreateResponse(HttpStatusCode.OK, model);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
        //    }
        //}

        //#endregion

        //#region [Cadastro de Médicos]

        //[HttpGet]
        //[Route("getMedicoById")]
        //public HttpResponseMessage ObterMedicoPorId(int id)
        //{
        //    try
        //    {
        //        var model = new MedicoViewModel();

        //        var medico = _service.ObterMedicoPorId(id);

        //        if (medico == null)
        //            return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Erro ao recuperar dados do médico.");

        //        Mapper.CreateMap<Medico, MedicoViewModel>();
        //        model = Mapper.Map<Medico, MedicoViewModel>(medico);

        //        return Request.CreateResponse(HttpStatusCode.OK, model);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
        //    }
        //}

        //[HttpGet]
        //[Route("listarMedicos")]
        //public HttpResponseMessage ListarMedicos()
        //{
        //    try
        //    {
        //        var model = new List<MedicoViewModel>();

        //        var medicos = _service.ListarMedicos();

        //        if (medicos.Count > 0)
        //        {
        //            Mapper.CreateMap<Medico, MedicoViewModel>();
        //            model = Mapper.Map<List<Medico>, List<MedicoViewModel>>(medicos);
        //        }
        //        return Request.CreateResponse(HttpStatusCode.OK, model);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
        //    }
        //}

        //[HttpGet]
        //[Route("listarMedicosPorNome")]
        //public HttpResponseMessage ListarMedicosPorNome(string nome)
        //{
        //    try
        //    {
        //        var model = new List<MedicoViewModel>();

        //        var medicos = _service.ListarMedicosPorNome(nome);

        //        if (medicos.Count > 0)
        //        {
        //            Mapper.CreateMap<Medico, MedicoViewModel>();
        //            model = Mapper.Map<List<Medico>, List<MedicoViewModel>>(medicos);
        //        }
        //        return Request.CreateResponse(HttpStatusCode.OK, model);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
        //    }
        //}

        //[HttpPost]
        //[Route("saveMedico")]
        //public HttpResponseMessage SalvarMedico(MedicoViewModel model)
        //{
        //    try
        //    {
        //        if (model.IdMedico > 0)
        //        {
        //            var medico = _service.ObterMedicoPorId(model.IdMedico);
        //            if (medico == null)
        //                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Não foi possível obter dados do médico.");


        //            medico.SetNomeMedico(model.NomeMedico);
        //            medico.SetCrm(model.Crm);

        //            _service.SalvarMedico(medico);
        //        }
        //        else
        //        {
        //            var medico = new Medico(model.NomeMedico, model.Crm);
        //            _service.SalvarMedico(medico);
        //        }

        //        return Request.CreateResponse(HttpStatusCode.OK, model);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
        //    }
        //}

        //#endregion
    }
}