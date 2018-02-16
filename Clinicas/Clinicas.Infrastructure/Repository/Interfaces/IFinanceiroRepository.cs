using Clinicas.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinicas.Infrastructure.Repository.Interfaces
{
    public interface IFinanceiroRepository
    {
        #region [Contas a Receber]
        void SalvarFinanceiro(Financeiro financeiro);
        void ExcluirFinanceiro(Financeiro financeiro);

        ICollection<FinanceiroParcela> ListarContasaReceber(int idclinica,int idunidadeatendimento);
        ICollection<FinanceiroParcela> ListarContasaReceberAberto(int idclinica, int idunidadeatendimento);
        ICollection<FinanceiroParcela> ListarContasaReceberBaixado(int idclinica, int idunidadeatendimento);

        int QuantidadeRegistrosContasaReceber(int idclinica, int idunidadeatendimento);
        decimal TotalContasaReceber(int idclinica, int idunidadeatendimento);
        decimal TotalContasaReceberHoje(int idclinica, int idunidadeatendimento);
        decimal TotalContasaReceberAmanha(int idclinica, int idunidadeatendimento);
        #endregion

        #region [Financeiro]
        MeioPagamento GetMeioPagamentoPorId(int id);
        Financeiro ObterFinanceiroPorId(int id);
        FinanceiroParcela AlterarParcela(FinanceiroParcela parcela);
        ICollection<FinanceiroParcela> ListarParcelasPesquisa(string tipo, string descricao, string tipoConta, int idclinica, int idunidadeatendimento);
        #endregion

        #region [Contas a Pagar]
        ICollection<FinanceiroParcela> ListarContasaPagar(int idclinica, int idunidadeatendimento);
        ICollection<FinanceiroParcela> ListarContasaPagarPorId(int id, int idclinica, int idunidadeatendimento);
        ICollection<FinanceiroParcela> ListarContasaPagarPorCredor(string credor, int idclinica, int idunidadeatendimento);
        ICollection<FinanceiroParcela> ListarContasaPagarAberto(int idclinica, int idunidadeatendimento);
        ICollection<FinanceiroParcela> ListarContasaPagarBaixado(int idclinica, int idunidadeatendimento);
        FinanceiroParcela ObterFinanceiroParcelaPorId(int id);
        decimal TotalContasaPagar(int idclinica, int idunidadeatendimento);
        decimal TotalContasaPagarHoje(int idclinica, int idunidadeatendimento);
        decimal TotalContasaPagarAmanha(int idclinica, int idunidadeatendimento);
        #endregion

        #region [Cheque]
        void SalvarCheque(Cheque cheque);
        void ExcluirCheque(Cheque cheque);
        Cheque ObterChequePorId(int id);
        ICollection<Cheque> ListarCheques(int idClinica);
        ICollection<Cheque> PesquisarCheques(string emitente, string banco, int idClinica);

        #endregion

        #region [Plano de Conta]
        void SalvarPlanoConta(PlanoConta planoConta);
        void ExcluirPlanoConta(PlanoConta planoConta);
        PlanoConta ObterPlanodeContasPorId(int id);
        ICollection<PlanoConta> ListarPlanoContas(int idclinica);
        ICollection<PlanoConta> PesquisarPlanoContas(string nome, int? codigo, string tipo,int idclinica);
        #endregion

        #region [Banco]
        void SalvarBanco(Banco banco);
        void ExcluirBanco(Banco banco);
        ICollection<Banco> ListarBancos();
        Banco ObterBancoPorId(int id);

        #endregion

        #region [Centro de Custo]
        void SalvarCentroCusto(CentroCusto centroCusto);
        void ExcluirCentroCusto(CentroCusto centroCusto);
        CentroCusto ObterCentroCustoPorId(int id);
        ICollection<CentroCusto> ListarCentroCustos();
        #endregion

        #region [Conta]
        void SalvarConta(Conta conta);
        void ExcluirConta(Conta conta);
        ICollection<Conta> ListarContas(int idclinica);
        Conta ObterContaPorId(int id);
        void TransferenciaConta(TransferenciaConta transferenciaConta);
        ICollection<Conta> PesquisarContas(string nome, int? codigo,int idclinica);
        #endregion

        #region [Caixa Diario]

        void SalvarCaixaDiario(CaixaDiario caixaDiario);
        void ExcluirCaixaDiario(CaixaDiario caixaDiario);
        void SalvarMovimentacaoCaixa(MovimentoCaixa movimentoCaixa);

        #endregion

        #region [MeioPagamento]
        ICollection<MeioPagamento> ListarMeiosdePagamentos();
        MeioPagamento ObterMeioPagamentoPorId(int id);
        #endregion

        #region[Transferencias entre contas]
        List<TransferenciaConta> GetTransferencias();
        TransferenciaConta GetTransferenciaById(int id);
        void SalvarTransferencia(TransferenciaConta model);
        void ExcluirTransferencia(TransferenciaConta model);
        #endregion

    }
}
