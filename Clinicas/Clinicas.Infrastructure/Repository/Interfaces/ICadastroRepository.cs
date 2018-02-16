using Clinicas.Domain.Model;
using Clinicas.Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinicas.Infrastructure.Repository.Interfaces
{
    public interface ICadastroRepository
    {
        Pessoa ObterPessoaPorId(int id);
        List<Pessoa> ListarPessoaPorNome(string nome, int idClinica);

        #region [Especialidades]
        Especialidade SalvarEspecialidade(Especialidade model);
        Especialidade GetEspecialidadeById(int id);
        List<Especialidade> ListarEspecialidades();
        List<Especialidade> ListarEspecialidadesPorNome(string nome);
        void ExcluirEspecialidade(int id);
        #endregion

        #region [Procedimentos]
        List<Procedimento> ListarProcedimentos();
        List<Procedimento> ListarProcedimentosPorNome(string nome);
        List<Procedimento> ListarProcedimentosPorNomeOuCodigo(string search);
        Procedimento ObterProcedimentoById(int id);
        Procedimento SalvarProcedimento(Procedimento model);
        #endregion

        #region [CID]
        Cid SalvarCid(Cid model, bool atualiza);
        Cid GetCidByCodigo(string codigo);
        List<Cid> ListarCids();
        List<Cid> ListarCidsPorNome(string nome);
        #endregion

        #region [Ocupacao]
        Ocupacao SalvarOcupacao(Ocupacao model);
        Ocupacao GetOcupacaoById(int id);
        List<Ocupacao> ListarOcupacoes();
        List<Ocupacao> ListarOcupacoesPorNome(string nome);
        #endregion

        #region [Fornecedor]
        Fornecedor SalvarFornecedor(Fornecedor model);
        Fornecedor ExcluirFornecedor(Fornecedor model);
        Fornecedor ObterFornecedorById(int id);
        ICollection<Fornecedor> ListarFornecedores(int idclinica);
        ICollection<Fornecedor> PesquisarFornecedores(string nome, int? idFornecedor,int idclinica);
        #endregion

        #region [Funcionario]
        Funcionario SalvarFuncionario(Funcionario model);
        Funcionario ExcluirFuncionario(Funcionario model);
        Funcionario ObterFuncionarioById(int id);
        ICollection<Funcionario> ListarFuncionarios(int idclinica);
        ICollection<Funcionario> ListarProfissionaisSaude(int idclinica);
        int QuantidadeAgendasDisponiveis(int idprofissional);
        ICollection<Funcionario> PesquisarFuncionarios(string nome, int? idFuncionario,int idclinica);
        void ExcluirEspecialidades(List<Especialidade> especialidades, int idFuncionario);
        #endregion

        #region [Convenio]
        Convenio SalvarConvenio(Convenio model);
        Convenio ExcluirConvenio(Convenio model);
        Convenio ObterConvenioById(int id);
        ICollection<Convenio> ListarConvenios(int idclinica);
        ICollection<Convenio> PesquisarConvenios(string nome, int? idConvenio,int idclinica);
        #endregion

        #region [Paciente]
        Paciente SalvarPaciente(Paciente model);
        Paciente ExcluirPaciente(Paciente model);
        Paciente ObterPacienteById(int id);
        ICollection<Paciente> ListarPacientes(int idclinica);
        ICollection<Paciente> PesquisarPacientes(string nome, int? idPaciente,int idclinica);
        void ExcluirCarteiraPaciente(List<Carteira> carteiras);
        #endregion

        #region [Estado - Cidade]
        Estado ObterEstadoPorId(int idEstado);
        Cidade ObterCidadePorId(int idCidade);
        #endregion

        #region Clinica
        Clinica ObterClinicaById(int id);
        Clinica SalvarClinica(Clinica clinica);
        UnidadeAtendimento ObterUnidadeDeAtendimentoByCnpj(string cnpj);
        void ExcluirUnidadesAtendimento(List<UnidadeAtendimento> unidades, int idClinica);
        void ExcluirUnidadeAtendimento(UnidadeAtendimento unidade);
        UnidadeAtendimento ObterUnidadeAtendimentoPorId(int id);
        Consultorio ObterConsultorioPorId(int id);
        Consultorio SalvarConsultorio(Consultorio clinica);
        Consultorio ExcluirConsultorio(int id);
        ICollection<Consultorio> ListarConsultorios(int idclinica);
        void RemoverEspecialidadeUnidadeAtendimento(int idUnidadeAtendimento);

        #endregion

        #region [Tabela de preço]
        TabelaPreco ObterTabelaById(int idTabelaPreco);
        TabelaPreco SalvarTabelaPreco(TabelaPreco tabela);
        ICollection<TabelaPreco> ListarTabelasPreco(int idClinica);
        void ExcluirItensTabela(List<TabelaPrecoItens> itens, int idTabela);
        ICollection<TabelaPrecoItens> ListarItensTabelasPreco(int idTabela);
        ICollection<TabelaPreco> ListarTabelasPrecoPorConvenio(string tipo, int id, int idClinica);
        ICollection<TabelaPreco> ListarTabelasPrecoPorConvenioAtivas(string tipo, int id, int idClinica);
        TabelaPrecoItens GetProcedimentosByTabela(int idproc, int id);
        void ExcluirTabelaPreco(TabelaPreco tabela);
        #endregion

        /**********/

        ICollection<EspecialidadeViewModel> ListarEspecialidadesPorProfissionalSaude(int idprofissional);
        ICollection<ProcedimentosViewModel> ListarProcedimentosPorEspecialidade(int idespecialidade);
    }
}
