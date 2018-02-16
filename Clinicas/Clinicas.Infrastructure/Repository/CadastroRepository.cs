using Clinicas.Infrastructure.Base;
using Clinicas.Infrastructure.Context;
using Clinicas.Infrastructure.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Clinicas.Domain.ViewModel;
using Clinicas.Domain.Model;
using System.Data.Entity;
using System.Linq.Expressions;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Validation;

namespace Clinicas.Infrastructure.Repository
{
    public class CadastroRepository : RepositoryBase<ClinicasContext>, ICadastroRepository
    {
        public CadastroRepository(IUnitOfWork<ClinicasContext> unit)
            : base(unit)
        {
        }

        public Pessoa ObterPessoaPorId(int id)
        {
            return Context.Pessoa.Find(id);
        }
        public List<Pessoa> ListarPessoaPorNome(string nome, int idClinica)
        {
            return Context.Pessoa.Where(x => x.Situacao != "Excluido" && x.IdClinica == idClinica && x.Nome.ToUpper().Contains(nome.ToUpper())).Take(200).ToList();
        }

        #region [Especialidades]

        public Especialidade GetEspecialidadeById(int id)
        {
            return Context.Especialidade.Include(x => x.Procedimentos).FirstOrDefault(x => x.IdEspecialidade == id);
        }

        public List<Especialidade> ListarEspecialidades()
        {
            return Context.Especialidade.Include(m=>m.Procedimentos).Where(x => x.Situacao == "Ativo").ToList();
        }

        public List<Especialidade> ListarEspecialidadesPorNome(string nome)
        {
            return Context.Especialidade.Where(x => x.NmEspecialidade.ToUpper().Contains(nome.ToUpper())).ToList();
        }

        public Especialidade SalvarEspecialidade(Especialidade model)
        {
            if (model.IdEspecialidade > 0)
            {
                Context.Entry(model).State = EntityState.Modified;
            }
            else
            {
                Context.Especialidade.Add(model);
            }
            Context.SaveChanges();
            return model;
        }

        public void ExcluirEspecialidade(int id)
        {
            var especialidade = Context.Especialidade.Find(id);
            especialidade.SetSituacao("Excluido");
            Context.Entry(especialidade).State = EntityState.Modified;
            Context.SaveChanges();
        }
        #endregion

        #region [Procedimentos]
        public List<Procedimento> ListarProcedimentos()
        {
            return Context.Procedimento.Include(x=>x.Especialidades).ToList();
        }

        public List<Procedimento> ListarProcedimentosPorNome(string nome)
        {
            return Context.Procedimento.Where(x => x.NomeProcedimento.ToUpper().Contains(nome.ToUpper())).ToList();
        }

        public List<Procedimento> ListarProcedimentosPorNomeOuCodigo(string search)
        {
            var codigo = search.Substring(0, 2);
            bool isNumber = true;
            foreach (var cod in codigo.ToCharArray())
            {
                if (!Char.IsDigit(cod))
                    isNumber = false;
            }

            if (isNumber)
                return Context.Procedimento.Where(x => x.Codigo == Convert.ToInt32(search)).ToList();
            else
                return Context.Procedimento.Where(x => x.NomeProcedimento.Contains(search.ToUpper())).ToList();
        }
        public Procedimento ObterProcedimentoById(int id)
        {
            return Context.Procedimento.Include(m=>m.Especialidades).FirstOrDefault(x => x.IdProcedimento == id);
        }
        public Procedimento SalvarProcedimento(Procedimento model)
        {
            if (model.IdProcedimento > 0)
            {
                Context.Entry(model).State = EntityState.Modified;
            }
            else
            {
                Context.Procedimento.Add(model);
            }
            Context.SaveChanges();
            return model;
        }

        #endregion

        #region [CID]
        public Cid SalvarCid(Cid model, bool atualiza)
        {
            if (atualiza)
            {
                Context.Entry(model).State = EntityState.Modified;
            }
            else
            {
                Context.Cid.Add(model);
            }
            Context.SaveChanges();
            return model;
        }
        public Cid GetCidByCodigo(string codigo)
        {
            return Context.Cid.FirstOrDefault(x => x.Codigo == codigo);
        }
        public List<Cid> ListarCids()
        {
            return Context.Cid.Take(200).ToList();
        }
        public List<Cid> ListarCidsPorNome(string nome)
        {
            return Context.Cid.Where(x => x.Descricao.ToUpper().Contains(nome.ToUpper())).Take(200).ToList();
        }
        #endregion

        #region [Ocupacao]
        public Ocupacao SalvarOcupacao(Ocupacao model)
        {
            if (model.IdOcupacao > 0)
            {
                Context.Entry(model).State = EntityState.Modified;
            }
            else
            {
                Context.Ocupacao.Add(model);
            }
            Context.SaveChanges();
            return model;
        }
        public Ocupacao GetOcupacaoById(int id)
        {
            return Context.Ocupacao.FirstOrDefault(x => x.IdOcupacao == id);
        }
        public List<Ocupacao> ListarOcupacoes()
        {
            return Context.Ocupacao.Take(200).ToList();
        }
        public List<Ocupacao> ListarOcupacoesPorNome(string nome)
        {
            return Context.Ocupacao.Where(x => x.NmOcupacao.ToUpper().Contains(nome.ToUpper())).Take(200).ToList();
        }
        #endregion

        #region [Fornecedor]
        public Fornecedor SalvarFornecedor(Fornecedor model)
        {
            if (model.IdFornecedor > 0)
            {
                Context.Entry(model).State = EntityState.Modified;
            }
            else
            {
                Context.Fornecedor.Add(model);
            }
            Context.SaveChanges();
            return model;
        }

        public Fornecedor ExcluirFornecedor(Fornecedor model)
        {
            if (model.IdFornecedor > 0)
            {
                model.Pessoa.SetSituacao("Excluido");
                Context.Entry(model).State = EntityState.Modified;

            }
            Context.SaveChanges();
            return model;
        }

        public Fornecedor ObterFornecedorById(int id)
        {
            return Context.Fornecedor.Include(x => x.Pessoa).FirstOrDefault(x => x.IdFornecedor == id);
        }

        public ICollection<Fornecedor> ListarFornecedores(int idclinica)
        {
            return Context.Fornecedor.Include(x => x.Pessoa).Where(x => x.Pessoa.Situacao != "Excluido" && x.Pessoa.IdClinica == idclinica).Take(200).ToList();
        }

        public ICollection<Fornecedor> PesquisarFornecedores(string nome, int? idFornecedor, int idclinica)
        {
            ICollection<Fornecedor> resultado = null;

            Expression<Func<Fornecedor, bool>> filtroNome = registro => true;
            Expression<Func<Fornecedor, bool>> filtroIdFornecedor = registro => true;

            if (!string.IsNullOrEmpty(nome))
                filtroNome = (Fornecedor registro) =>
                           registro.Pessoa.Nome.Contains(nome.ToUpper()); //  || registro.Pessoa.RazaoSocial.ToUpper().Contains(nome.ToUpper());

            if (idFornecedor > 0)
                filtroIdFornecedor = (Fornecedor registro) =>
                                          registro.IdFornecedor == idFornecedor;


            resultado = Context.Fornecedor.Include(x => x.Pessoa)
                .Where(filtroNome).Where(filtroIdFornecedor).Where(x => x.Pessoa.Situacao != "Excluido" && x.Pessoa.IdClinica == idclinica).ToList();

            return resultado;
        }
        #endregion

        #region [Funcionario]
        public Funcionario SalvarFuncionario(Funcionario model)
        {
            if (model.IdFuncionario > 0)
            {
                Context.Entry(model).State = EntityState.Modified;
            }
            else
            {
                Context.Funcionario.Add(model);
            }
            Context.SaveChanges();
            return model;
        }

        public Funcionario ExcluirFuncionario(Funcionario model)
        {
            if (model.IdFuncionario > 0)
            {
                model.Pessoa.SetSituacao("Excluido");
                Context.Entry(model).State = EntityState.Modified;

            }
            Context.SaveChanges();
            return model;
        }

        public Funcionario ObterFuncionarioById(int id)
        {
            return Context.Funcionario.Include(x => x.Pessoa).Include(x => x.Especialidades).FirstOrDefault(x => x.IdFuncionario == id);
        }

        public ICollection<Funcionario> ListarFuncionarios(int idclinica)
        {
            return Context.Funcionario.Include(x => x.Pessoa).Where(x => x.Pessoa.Situacao != "Excluido" && x.Pessoa.IdClinica == idclinica).Take(1000).ToList();
        }

        public ICollection<Funcionario> ListarProfissionaisSaude(int idclinica)
        {
            return Context.Funcionario.Include(x => x.Especialidades).Include(x => x.Pessoa).Where(x => x.Pessoa.Situacao != "Excluido" && x.Tipo == "Profissional de Saúde" && x.Pessoa.IdClinica == idclinica).Take(1000).ToList();
        }

        public int QuantidadeAgendasDisponiveis(int idprofissional)
        {

            var data = DateTime.Now.Date;
            return Context.Agenda.Where(x => x.Situacao == "Aguardando" && x.IdFuncionario == idprofissional && x.Data >= data).Count();
        }

        public void ExcluirEspecialidades(List<Especialidade> especialidades, int idFuncionario)
        {
            var func = Context.Funcionario.Where(x => x.IdFuncionario == idFuncionario).FirstOrDefault();
            if (func != null)
            {
                foreach (var item in especialidades)
                {
                    func.Especialidades.Remove(item);
                }
                Context.SaveChanges();
            }

        }

        public ICollection<Funcionario> PesquisarFuncionarios(string nome, int? idFuncionario, int idclinica)
        {
            ICollection<Funcionario> resultado = null;

            Expression<Func<Funcionario, bool>> filtroNome = registro => true;
            Expression<Func<Funcionario, bool>> filtroId = registro => true;

            if (!string.IsNullOrEmpty(nome))
                filtroNome = (Funcionario registro) =>
                           registro.Pessoa.Nome.Contains(nome.ToUpper()); //  || registro.Pessoa.RazaoSocial.ToUpper().Contains(nome.ToUpper());

            if (idFuncionario > 0)
                filtroId = (Funcionario registro) =>
                                          registro.IdFuncionario == idFuncionario;


            resultado = Context.Funcionario.Include(x => x.Pessoa)
                .Where(filtroNome).Where(filtroId).Where(x => x.Pessoa.Situacao != "Excluido").ToList();

            return resultado;
        }
        #endregion

        #region [Convenio]
        public Convenio SalvarConvenio(Convenio model)
        {
            if (model.IdConvenio > 0)
            {
                Context.Entry(model).State = EntityState.Modified;
            }
            else
            {
                Context.Convenio.Add(model);
            }
            Context.SaveChanges();
            return model;
        }

        public Convenio ExcluirConvenio(Convenio model)
        {
            if (model.IdConvenio > 0)
            {
                model.Pessoa.SetSituacao("Excluido");
                Context.Entry(model).State = EntityState.Modified;
            }
            Context.SaveChanges();
            return model;
        }

        public Convenio ObterConvenioById(int id)
        {
            return Context.Convenio.Include(x => x.Pessoa).FirstOrDefault(x => x.IdConvenio == id);
        }

        public ICollection<Convenio> ListarConvenios(int idclinica)
        {
            return Context.Convenio.Include(x => x.Pessoa).Where(x => x.Pessoa.Situacao != "Excluido" && x.Pessoa.IdClinica == idclinica).Take(200).OrderBy(x=>x.Pessoa.Nome).ToList();
        }

        public ICollection<Convenio> PesquisarConvenios(string nome, int? idConvenio, int idclinica)
        {
            ICollection<Convenio> resultado = null;

            Expression<Func<Convenio, bool>> filtroNome = registro => true;
            Expression<Func<Convenio, bool>> filtroId = registro => true;

            if (!string.IsNullOrEmpty(nome))
                filtroNome = (Convenio registro) =>
                           registro.Pessoa.Nome.Contains(nome.ToUpper()); //  || registro.Pessoa.RazaoSocial.ToUpper().Contains(nome.ToUpper());

            if (idConvenio > 0)
                filtroId = (Convenio registro) =>
                                          registro.IdConvenio == idConvenio;


            resultado = Context.Convenio.Include(x => x.Pessoa)
                .Where(filtroNome).Where(filtroId).Where(x => x.Pessoa.Situacao != "Excluido" && x.Pessoa.IdClinica == idclinica).ToList();

            return resultado;
        }

        #endregion

        #region [Paciente]
        public Paciente SalvarPaciente(Paciente model)
        {
            var paciente = Context.Paciente.Include(a => a.Pessoa)
                .FirstOrDefault(x => x.Pessoa.CpfCnpj == model.Pessoa.CpfCnpj && x.Pessoa.IdClinica == model.Pessoa.IdClinica && x.Pessoa.Situacao == "Ativo");

            if ((model.IdPaciente == 0) && (paciente != null))
            {
                throw new Exception("Paciente em Duplicidade");
            }

            if (model.IdPaciente > 0)
            {
                Context.Entry(model).State = EntityState.Modified;
            }
            else
            {
                Context.Paciente.Add(model);
            }
            Context.SaveChanges();
            return model;
        }

        public Paciente ExcluirPaciente(Paciente model)
        {
            if (model.IdPaciente > 0)
            {
                model.Pessoa.SetSituacao("Excluido");
                Context.Entry(model).State = EntityState.Modified;

            }
            Context.SaveChanges();
            return model;
        }

        public Paciente ObterPacienteById(int id)
        {
            return Context.Paciente.Include(x => x.Pessoa).Include(x => x.Carteiras).Include(x => x.Carteiras.Select(a => a.Convenio.Pessoa)).FirstOrDefault(x => x.IdPaciente == id);
        }

        public ICollection<Paciente> ListarPacientes(int idclinica)
        {
            return Context.Paciente.Include(x => x.Pessoa).Where(x => x.Pessoa.Situacao != "Excluido" && x.Pessoa.IdClinica == idclinica).Take(200).ToList();
        }

        public ICollection<Paciente> PesquisarPacientes(string nome, int? idPaciente, int idclinica)
        {
            ICollection<Paciente> resultado = null;

            Expression<Func<Paciente, bool>> filtroNome = registro => true;
            Expression<Func<Paciente, bool>> filtroId = registro => true;

            if (!string.IsNullOrEmpty(nome))
                filtroNome = (Paciente registro) =>
                           registro.Pessoa.Nome.Contains(nome.ToUpper()); //  || registro.Pessoa.RazaoSocial.ToUpper().Contains(nome.ToUpper());

            if (idPaciente > 0)
                filtroId = (Paciente registro) =>
                                          registro.IdPaciente == idPaciente;


            resultado = Context.Paciente.Include(x => x.Pessoa)
                .Where(filtroNome).Where(filtroId).Where(x => x.Pessoa.Situacao != "Excluido" && x.Pessoa.IdClinica == idclinica).ToList();

            return resultado;
        }

        public void ExcluirCarteiraPaciente(List<Carteira> carteiras)
        {
            Context.Carteira.RemoveRange(carteiras);
            Context.SaveChanges();
        }
        #endregion

        #region [Estado - Cidade]
        public Estado ObterEstadoPorId(int idEstado)
        {
            return Context.Estados.Find(idEstado);
        }

        public Cidade ObterCidadePorId(int idCidade)
        {
            return Context.Cidades.Find(idCidade);
        }
        #endregion

        #region Clinicas
        public Clinica ObterClinicaById(int id)
        {
            var clinica = Context.Clinica.Include(x=>x.Unidades.Select(m=>m.Especialidades)).FirstOrDefault(x => x.IdClinica == id);

            return clinica;
        }

        public UnidadeAtendimento ObterUnidadeDeAtendimentoByCnpj(string cnpj)
        {
            return Context.Unidades.FirstOrDefault(x=>x.CpfCnpj==cnpj);
        }

        public Clinica SalvarClinica(Clinica clinica)
        {
            try
            {
                if (clinica.IdClinica > 0)
                {
                    Context.Entry(clinica).State = EntityState.Modified;
                }
                else
                {
                    Context.Clinica.Add(clinica);
                }
                Context.SaveChanges();
                return clinica;
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
        }

        public void ExcluirUnidadesAtendimento(List<UnidadeAtendimento> unidades, int idClinica)
        {
            foreach (var item in unidades)
            {
                item.SetSituacao("Excluido");
                Context.Entry(item).State = EntityState.Modified;
                Context.SaveChanges();
            }
        }

        public UnidadeAtendimento ObterUnidadeAtendimentoPorId(int id)
        {
            return Context.Unidades.Include(m => m.Cidade).Include(m=>m.Estado).FirstOrDefault(x => x.IdUnidadeAtendimento == id);
        }

        public void ExcluirUnidadeAtendimento(UnidadeAtendimento unidade)
        {
            unidade.SetSituacao("Excluido");
            Context.Entry(unidade).State = EntityState.Modified;
            Context.SaveChanges();
        }

        public ICollection<Consultorio> ListarConsultorios(int idclinica)
        {
            return Context.Consultorios.Include(x => x.UnidadeAtendimento).Include(x => x.Clinica).Where(x => x.IdClinica == idclinica).ToList();
        }

        public Consultorio ObterConsultorioPorId(int id)
        {
            return Context.Consultorios.Include(x => x.Clinica).FirstOrDefault(x => x.IdConsultorio == id);
        }

        public Consultorio SalvarConsultorio(Consultorio consultorio)
        {
            if (consultorio.IdConsultorio > 0)
            {
                Context.Entry(consultorio).State = EntityState.Modified;
            }
            else
            {
                Context.Consultorios.Add(consultorio);
            }
            Context.SaveChanges();
            return consultorio;
        }

        public Consultorio ExcluirConsultorio(int id)
        {
            var consultorio = Context.Consultorios.Find(id);
            Context.Consultorios.Remove(consultorio);
            Context.SaveChanges();
            return consultorio;
        }
        #endregion

        #region [Tabela de preço]

        public TabelaPreco ObterTabelaById(int idTabelaPreco)
        {
            return Context.TabelaPreco.Include(x => x.Clinica).Include(x => x.Convenio).Include(x => x.Itens).Include(x => x.Convenio.Pessoa).FirstOrDefault(x => x.IdTabelaPreco == idTabelaPreco);
        }

        public ICollection<TabelaPreco> ListarTabelasPreco(int idClinica)
        {
            return Context.TabelaPreco.Include(x => x.Clinica).Include(x => x.Convenio).Include(x => x.Convenio.Pessoa).Where(x => x.IdClinica == idClinica && x.Situacao == "Ativo").Take(200).ToList();
        }

        public ICollection<TabelaPrecoItens> ListarItensTabelasPreco(int idTabela)
        {
            return Context.Itens.Include(x => x.Procedimento).Include(x => x.TabelaPreco).Where(x => x.IdTabelaPreco == idTabela).ToList();
        }

        public ICollection<TabelaPreco> ListarTabelasPrecoPorConvenio(string tipo, int id, int idClinica)
        {
            if (tipo == "P")
                return Context.TabelaPreco.Include(x => x.Clinica).Include(x => x.Convenio).Include(x => x.Convenio.Pessoa).Where(x => x.Tipo == "P" && x.IdClinica == idClinica).Take(200).ToList();
            else
                return Context.TabelaPreco.Include(x => x.Clinica).Include(x => x.Convenio).Include(x => x.Convenio.Pessoa).Where(x => x.IdConvenio == id && x.IdClinica == idClinica).Take(200).ToList();
        }

        public ICollection<TabelaPreco> ListarTabelasPrecoPorConvenioAtivas(string tipo, int id, int idClinica)
        {
            if (tipo == "P")
                return Context.TabelaPreco.Include(x => x.Clinica).Include(x => x.Convenio).Include(x => x.Convenio.Pessoa).Where(x => x.Tipo == "P" && x.IdClinica == idClinica && x.Situacao == "Ativo").Take(200).ToList();
            else
                return Context.TabelaPreco.Include(x => x.Clinica).Include(x => x.Convenio).Include(x => x.Convenio.Pessoa).Where(x => x.IdConvenio == id && x.IdClinica == idClinica && x.Situacao == "Ativo").Take(200).ToList();
        }
        public TabelaPrecoItens GetProcedimentosByTabela(int idproc, int id)
        {
            return Context.Itens.Include(x => x.Procedimento).Include(x => x.TabelaPreco).FirstOrDefault(x => x.IdTabelaPreco == id && x.IdProcedimento == idproc);
        }

        public TabelaPreco SalvarTabelaPreco(TabelaPreco tabela)
        {
            if (tabela.IdTabelaPreco > 0)
            {
                Context.Entry(tabela).State = EntityState.Modified;
            }
            else
            {
                Context.TabelaPreco.Add(tabela);
            }
            Context.SaveChanges();
            return tabela;
        }

        public void ExcluirTabelaPreco(TabelaPreco tabela)
        {
            tabela.SetSituacao("Excluido");
            Context.Entry(tabela).State = EntityState.Modified;
            Context.SaveChanges();
        }

        public void ExcluirItensTabela(List<TabelaPrecoItens> itens, int idTabela)
        {
            var tabela = Context.TabelaPreco.Where(x => x.IdTabelaPreco == idTabela).FirstOrDefault();
            if (tabela != null)
            {
                foreach (var item in new List<TabelaPrecoItens>(itens))
                {
                    tabela.Itens.Remove(item);
                }
                Context.SaveChanges();
            }
        }

        #endregion

        /****  Alterar    ****/
        public ICollection<EspecialidadeViewModel> ListarEspecialidadesPorProfissionalSaude(int idprofissional)
        {
            return Context.Database.SqlQuery<EspecialidadeViewModel>(@" SELECT
                                    EspecialidadeFuncionario.IdEspecialidade,
                                    Especialidade.NmEspecialidade
                                    FROM
                                    EspecialidadeFuncionario
                                    INNER JOIN Especialidade ON EspecialidadeFuncionario.IdEspecialidade = Especialidade.IdEspecialidade
                                    WHERE EspecialidadeFuncionario.IdFuncionario = '" + idprofissional + "' ORDER BY Especialidade.NmEspecialidade ").ToList();
        }

        public ICollection<ProcedimentosViewModel> ListarProcedimentosPorEspecialidade(int idespecialidade)
        {
            return Context.Database.SqlQuery<ProcedimentosViewModel>(@" SELECT
                                            EspecialidadeProcedimento.IdProcedimento,
                                            Procedimento.NmProcedimento
                                            FROM
                                            Procedimento
                                            INNER JOIN EspecialidadeProcedimento ON Procedimento.IdProcedimento = EspecialidadeProcedimento.IdProcedimento
                                            WHERE EspecialidadeProcedimento.IdEspecialidade = '" + idespecialidade + "' ORDER BY Procedimento.NmProcedimento   ").ToList();
        }

        public void RemoverEspecialidadeUnidadeAtendimento(int idUnidadeAtendimento)
        {
            Context.Database.ExecuteSqlCommand(" DELETE FROM `EspecialidadeUnidadeAtendimento` WHERE (`IdUnidadeAtendimento`='" + idUnidadeAtendimento + "')  ");
        }
    }
}