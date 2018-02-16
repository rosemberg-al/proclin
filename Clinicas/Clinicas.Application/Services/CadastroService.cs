using Clinicas.Application.Services.Interfaces;
using Clinicas.Infrastructure.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Clinicas.Domain.ViewModel;
using Clinicas.Domain.Model;
using iTextSharp.text;
using System.IO;
using iTextSharp.text.pdf;

namespace Clinicas.Application.Services
{
    public class CadastroService : ICadastroService
    {
        private readonly ICadastroRepository _repository;

        public CadastroService(ICadastroRepository repository)
        {
            _repository = repository;
        }

        public Pessoa ObterPessoaPorId(int id)
        {
            return _repository.ObterPessoaPorId(id);
        }
        public List<Pessoa> ListarPessoaPorNome(string nome, int idClinica)
        {
            return _repository.ListarPessoaPorNome(nome, idClinica);
        }

        #region [Especialidades]
        public Especialidade GetEspecialidadeById(int id)
        {
            return _repository.GetEspecialidadeById(id);
        }

        public List<Especialidade> ListarEspecialidades()
        {
            return _repository.ListarEspecialidades();
        }

        public Especialidade SalvarEspecialidade(Especialidade model)
        {
            return _repository.SalvarEspecialidade(model);
        }

        public List<Especialidade> ListarEspecialidadesPorNome(string nome)
        {
            return _repository.ListarEspecialidadesPorNome(nome);
        }

        public void ExcluirEspecialidade(int id)
        {
            _repository.ExcluirEspecialidade(id);
        }
        #endregion

        #region [Procedimentos]
        public List<Procedimento> ListarProcedimentos()
        {
            return _repository.ListarProcedimentos();
        }
        public List<Procedimento> ListarProcedimentosPorNome(string nome)
        {
            return _repository.ListarProcedimentosPorNome(nome);
        }

        public List<Procedimento> ListarProcedimentosPorNomeOuCodigo(string search)
        {
            return _repository.ListarProcedimentosPorNomeOuCodigo(search);
        }
        public Procedimento ObterProcedimentoById(int id)
        {
            return _repository.ObterProcedimentoById(id);
        }
        public Procedimento SalvarProcedimento(Procedimento model)
        {
            return _repository.SalvarProcedimento(model);
        }

        #endregion

        #region [Convenios]
        public Convenio SalvarConvenio(Convenio model)
        {
            //TODO: verificar se o convenio ja existe cadastrado na clínica
            return _repository.SalvarConvenio(model);
        }

        public Convenio ExcluirConvenio(Convenio model)
        {
            //TODO: verificar restrições exclusão convenio
            return _repository.ExcluirConvenio(model);
        }
        public Convenio ObterConvenioById(int id)
        {
            return _repository.ObterConvenioById(id);
        }
        public ICollection<Convenio> ListarConvenios(int idclinica)
        {
            return _repository.ListarConvenios(idclinica);
        }
        public ICollection<Convenio> PesquisarConvenios(string nome, int? idConvenio,int idclinica)
        {
            return _repository.PesquisarConvenios(nome, idConvenio,idclinica);
        }

        #endregion

        #region [CID]
        public Cid SalvarCid(Cid model, bool atualiza)
        {
            return _repository.SalvarCid(model, atualiza);
        }
        public Cid GetCidByCodigo(string codigo)
        {
            return _repository.GetCidByCodigo(codigo);
        }
        public List<Cid> ListarCids()
        {
            return _repository.ListarCids();
        }
        public List<Cid> ListarCidsPorNome(string nome)
        {
            return _repository.ListarCidsPorNome(nome);
        }
        #endregion

        #region [Ocupacao]
        public Ocupacao SalvarOcupacao(Ocupacao model)
        {
            return _repository.SalvarOcupacao(model);
        }
        public Ocupacao GetOcupacaoById(int id)
        {
            return _repository.GetOcupacaoById(id);
        }
        public List<Ocupacao> ListarOcupacoes()
        {
            return _repository.ListarOcupacoes();
        }
        public List<Ocupacao> ListarOcupacoesPorNome(string nome)
        {
            return _repository.ListarOcupacoesPorNome(nome);
        }
        #endregion

        #region [Fornecedor]
        public Fornecedor SalvarFornecedor(Fornecedor model)
        {
            //TODO: verificar se o fornecedor ja existe cadastrado na clínica
            return _repository.SalvarFornecedor(model);
        }

        public Fornecedor ExcluirFornecedor(Fornecedor model)
        {
            //TODO: verificar restrições exclusão fornecedor
            return _repository.ExcluirFornecedor(model);
        }
        public Fornecedor ObterFornecedorById(int id)
        {
            return _repository.ObterFornecedorById(id);
        }
        public ICollection<Fornecedor> ListarFornecedores(int idclinica)
        {
            return _repository.ListarFornecedores(idclinica);
        }
        public ICollection<Fornecedor> PesquisarFornecedores(string nome, int? idFornecedor,int idclinica)
        {
            return _repository.PesquisarFornecedores(nome, idFornecedor,idclinica);
        }
        #endregion

        #region [Funcionario]
        public Funcionario SalvarFuncionario(Funcionario model)
        {
            //TODO: verificar se o funcionario ja existe cadastrado na clínica
            return _repository.SalvarFuncionario(model);
        }

        public Funcionario ExcluirFuncionario(Funcionario model)
        {
            //TODO: verificar restrições exclusão funcionario
            return _repository.ExcluirFuncionario(model);
        }
        public Funcionario ObterFuncionarioById(int id)
        {
            return _repository.ObterFuncionarioById(id);
        }
        public ICollection<Funcionario> ListarFuncionarios(int idclinica)
        {
            return _repository.ListarFuncionarios(idclinica);
        }
        public ICollection<Funcionario> ListarProfissionaisSaude(int idclinica)
        {
            return _repository.ListarProfissionaisSaude(idclinica);
        }
        public ICollection<Funcionario> PesquisarFuncionarios(string nome, int? idFuncionario,int idclinica)
        {
            return _repository.PesquisarFuncionarios(nome, idFuncionario, idclinica);
        }
        public int QuantidadeAgendasDisponiveis(int idprofissional)
        {
            return _repository.QuantidadeAgendasDisponiveis(idprofissional);
        }

        public void ExcluirEspecialidades(List<Especialidade> especialidades, int idFuncionario)
        {
            _repository.ExcluirEspecialidades(especialidades, idFuncionario);
        }

        #endregion

        #region [Paciente]
        public Paciente SalvarPaciente(Paciente model)
        {
            return _repository.SalvarPaciente(model);
        }

        public Paciente ExcluirPaciente(Paciente model)
        {
            //TODO: verificar restrições exclusão paciente
            return _repository.ExcluirPaciente(model);
        }

        public Paciente ObterPacienteById(int id)
        {
            return _repository.ObterPacienteById(id);
        }

        public ICollection<Paciente> ListarPacientes(int idclinica)
        {
            return _repository.ListarPacientes(idclinica);
        }

        public ICollection<Paciente> PesquisarPacientes(string nome, int? idPaciente,int idclinica)
        {
            return _repository.PesquisarPacientes(nome, idPaciente,idclinica);
        }
        public void ExcluirCarteiraPaciente(List<Carteira> carteiras)
        {
            _repository.ExcluirCarteiraPaciente(carteiras);
        }
        #endregion

        #region [Estado - Cidade]
        public Estado ObterEstadoPorId(int idEstado)
        {
            return _repository.ObterEstadoPorId(idEstado);
        }
        public Cidade ObterCidadePorId(int idCidade)
        {
            return _repository.ObterCidadePorId(idCidade);
        }



        #endregion

        #region Clinica
        public Clinica ObterClinicaById(int id)
        {
            return _repository.ObterClinicaById(id);
        }
        public UnidadeAtendimento ObterUnidadeDeAtendimentoByCnpj(string cnpj)
        {
            return _repository.ObterUnidadeDeAtendimentoByCnpj(cnpj);
        }

        public Clinica SalvarClinica(Clinica clinica)
        {
            return _repository.SalvarClinica(clinica);
        }

        public void ExcluirUnidadesAtendimento(List<UnidadeAtendimento> unidades, int idClinica)
        {
            _repository.ExcluirUnidadesAtendimento(unidades, idClinica);
        }

        public void ExcluirUnidadeAtendimento(UnidadeAtendimento unidade) {
            _repository.ExcluirUnidadeAtendimento(unidade);
        }

        public UnidadeAtendimento ObterUnidadeAtendimentoPorId(int id)
        {
            return _repository.ObterUnidadeAtendimentoPorId(id);
        }
        public Consultorio ObterConsultorioPorId(int id)
        {
            return _repository.ObterConsultorioPorId(id);
        }

        public Consultorio ExcluirConsultorio(int id)
        {
            return _repository.ExcluirConsultorio(id);
        }

        public Consultorio SalvarConsultorio(Consultorio consultorio)
        {
            return _repository.SalvarConsultorio(consultorio);
        }

        public ICollection<Consultorio> ListarConsultorios(int idclinica)
        {
            return _repository.ListarConsultorios(idclinica);
        }
        #endregion

        #region [Tabela de preço]

        public TabelaPreco ObterTabelaById(int idTabelaPreco)
        {
            return _repository.ObterTabelaById(idTabelaPreco);
        }

        public ICollection<TabelaPreco> ListarTabelasPreco(int idClinica)
        {
            return _repository.ListarTabelasPreco(idClinica);
        }
        public ICollection<TabelaPreco> ListarTabelasPrecoPorConvenio(string tipo, int id, int idClinica)
        {
            return _repository.ListarTabelasPrecoPorConvenio(tipo, id, idClinica);
        }

        public ICollection<TabelaPreco> ListarTabelasPrecoPorConvenioAtivas(string tipo, int id, int idClinica)
        {
            return _repository.ListarTabelasPrecoPorConvenioAtivas(tipo, id, idClinica);
        }

        public TabelaPrecoItens GetProcedimentosByTabela(int idproc, int id)
        {
            return _repository.GetProcedimentosByTabela(idproc, id);
        }

        public TabelaPreco SalvarTabelaPreco(TabelaPreco tabela)
        {
            return _repository.SalvarTabelaPreco(tabela);
        }

        public void ExcluirTabelaPreco(TabelaPreco tabela)
        {
            _repository.ExcluirTabelaPreco(tabela);
        }

        public void ExcluirItensTabela(List<TabelaPrecoItens> itens, int idTabela)
        {
            _repository.ExcluirItensTabela(itens, idTabela);
        }

        public ICollection<TabelaPrecoItens> ListarItensTabelasPreco(int idTabela)
        {
            return _repository.ListarItensTabelasPreco(idTabela);
        }

        #endregion

        public ICollection<EspecialidadeViewModel> ListarEspecialidadesPorProfissionalSaude(int idprofissional)
        {
            return  _repository.ListarEspecialidadesPorProfissionalSaude(idprofissional);
        }

        public ICollection<ProcedimentosViewModel> ListarProcedimentosPorEspecialidade(int idespecialidade)
        {
           return _repository.ListarProcedimentosPorEspecialidade(idespecialidade);
        }

        public void RemoverEspecialidadeUnidadeAtendimento(int idUnidadeAtendimento)
        {
            _repository.RemoverEspecialidadeUnidadeAtendimento(idUnidadeAtendimento);
        }
    }
}