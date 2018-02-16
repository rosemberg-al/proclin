using Clinicas.Application.Services.Interfaces;
using Clinicas.Domain.Model;
using Clinicas.Infrastructure.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinicas.Application.Services
{
    public class FinanceiroService : IFinanceiroService
    {
        private readonly IFinanceiroRepository _financeiroRepository;

        public FinanceiroService(IFinanceiroRepository rp)
        {
            _financeiroRepository = rp;
        }

        #region Conta 
        public void SalvarConta(Conta conta)
        {
            _financeiroRepository.SalvarConta(conta);
        }

        public void ExcluirConta(Conta conta)
        {
            _financeiroRepository.ExcluirConta(conta);
        }

        public ICollection<Conta> ListarContas(int idclinica)
        {
            return _financeiroRepository.ListarContas(idclinica);
        }

        public ICollection<Conta> PesquisarContas(int idclinica)
        {
            return _financeiroRepository.ListarContas(idclinica);
        }

        public Conta ObterContaPorId(int id)
        {
            return _financeiroRepository.ObterContaPorId(id);
        }

        public ICollection<Conta> PesquisarContas(string nome, int? codigo,int idclinica)
        {
            return _financeiroRepository.PesquisarContas(nome,codigo, idclinica);
        }
        #endregion

        #region Plano Conta]
        public PlanoConta ObterPlanodeContasPorId(int id)
        {
            return _financeiroRepository.ObterPlanodeContasPorId(id);
        }

        public ICollection<PlanoConta> ListarPlanoContas(int idclinica)
        {
            return _financeiroRepository.ListarPlanoContas(idclinica);
        }

        public ICollection<PlanoConta> PesquisarPlanoContas(string nome, int? codigo, string tipo,int idclinica)
        {
            return _financeiroRepository.PesquisarPlanoContas(nome,codigo,tipo,idclinica);
        }

        public void SalvarPlanoConta(PlanoConta planoConta)
        {
            _financeiroRepository.SalvarPlanoConta(planoConta);
        }

        public void ExcluirPlanoConta(PlanoConta planoConta)
        {
            _financeiroRepository.ExcluirPlanoConta(planoConta);
        }
        #endregion
        
        public void SalvarFinanceiro(Financeiro financeiro)
        {
            _financeiroRepository.SalvarFinanceiro(financeiro);
        }

        public void ExcluirFinanceiro(Financeiro financeiro)
        {
            _financeiroRepository.ExcluirFinanceiro(financeiro);
        }

        public Financeiro ObterFinanceiroPorId(int id)
        {
            return _financeiroRepository.ObterFinanceiroPorId(id);
        }

        public MeioPagamento GetMeioPagamentoPorId(int id)
        {
            return _financeiroRepository.GetMeioPagamentoPorId(id);
        }

        public ICollection<FinanceiroParcela> ListarContasaReceber(int idclinica, int idunidadeatendimento)
        {
            return _financeiroRepository.ListarContasaReceber(idclinica, idunidadeatendimento);
        }

        public ICollection<FinanceiroParcela> ListarContasaReceberAberto(int idclinica, int idunidadeatendimento)
        {
            return _financeiroRepository.ListarContasaReceberAberto(idclinica, idunidadeatendimento);
        }

        public ICollection<FinanceiroParcela> ListarContasaReceberBaixado(int idclinica, int idunidadeatendimento)
        {
            return _financeiroRepository.ListarContasaReceberBaixado(idclinica, idunidadeatendimento);
        }

        public decimal TotalContasaReceber(int idclinica, int idunidadeatendimento)
        {
            return _financeiroRepository.TotalContasaReceber(idclinica, idunidadeatendimento);
        }

        public decimal TotalContasaReceberHoje(int idclinica, int idunidadeatendimento)
        {
            return _financeiroRepository.TotalContasaReceberHoje(idclinica, idunidadeatendimento);
        }

        public decimal TotalContasaReceberAmanha(int idclinica, int idunidadeatendimento)
        {
            return _financeiroRepository.TotalContasaReceberAmanha(idclinica,idunidadeatendimento);
        }

        public ICollection<FinanceiroParcela> ListarContasaPagar(int idclinica, int idunidadeatendimento)
        {
            return _financeiroRepository.ListarContasaPagar(idclinica, idunidadeatendimento);
        }

        public ICollection<FinanceiroParcela> ListarContasaPagarAberto(int idclinica, int idunidadeatendimento)
        {
            return _financeiroRepository.ListarContasaPagarAberto(idclinica, idunidadeatendimento);
        }

        public ICollection<FinanceiroParcela> ListarContasaPagarBaixado(int idclinica,int idunidadeatendimento)
        {
            return _financeiroRepository.ListarContasaPagarBaixado(idclinica,idunidadeatendimento);
        }

        public decimal TotalContasaPagar(int idclinica, int idunidadeatendimento)
        {
            return _financeiroRepository.TotalContasaPagar(idclinica, idunidadeatendimento);
        }

        public decimal TotalContasaPagarHoje(int idclinica, int idunidadeatendimento)
        {
            return _financeiroRepository.TotalContasaPagarHoje(idclinica, idunidadeatendimento);
        }

        public decimal TotalContasaPagarAmanha(int idclinica, int idunidadeatendimento)
        {
            return _financeiroRepository.TotalContasaPagarAmanha(idclinica, idunidadeatendimento);
        }
        
        public void SalvarCheque(Cheque cheque)
        {
            _financeiroRepository.SalvarCheque(cheque);
        }

        public void ExcluirCheque(Cheque cheque)
        {
            _financeiroRepository.ExcluirCheque(cheque);
        }

        public Cheque ObterChequePorId(int id)
        {
            return _financeiroRepository.ObterChequePorId(id);
        }

        public ICollection<Cheque> ListarCheques(int idClinica)
        {
            return _financeiroRepository.ListarCheques(idClinica);
        }

        public ICollection<Cheque> PesquisarCheques(string emitente, string banco, int idClinica)
        {
            return _financeiroRepository.PesquisarCheques(emitente, banco, idClinica);
        }

        public void SalvarBanco(Banco banco)
        {
            _financeiroRepository.SalvarBanco(banco);
        }

        public void ExcluirBanco(Banco banco)
        {
            _financeiroRepository.ExcluirBanco(banco);
        }

        public ICollection<Banco> ListarBancos()
        {
            return _financeiroRepository.ListarBancos();
        }

        public Banco ObterBancoPorId(int id)
        {
            return _financeiroRepository.ObterBancoPorId(id);
        }

        public void SalvarCentroCusto(CentroCusto centroCusto)
        {
            _financeiroRepository.SalvarCentroCusto(centroCusto);
        }

        public void ExcluirCentroCusto(CentroCusto centroCusto)
        {
            _financeiroRepository.ExcluirCentroCusto(centroCusto);
        }

        public CentroCusto ObterCentroCustoPorId(int id)
        {
            return _financeiroRepository.ObterCentroCustoPorId(id);
        }

        public ICollection<CentroCusto> ListarCentroCustos()
        {
            return _financeiroRepository.ListarCentroCustos();
        }
        
        public void TransferenciaConta(TransferenciaConta transferenciaConta)
        {
            _financeiroRepository.TransferenciaConta(transferenciaConta);
        }

        public void SalvarCaixaDiario(CaixaDiario caixaDiario)
        {
            throw new NotImplementedException();
        }

        public void ExcluirCaixaDiario(CaixaDiario caixaDiario)
        {
            _financeiroRepository.ExcluirCaixaDiario(caixaDiario);
        }

        public void SalvarMovimentacaoCaixa(MovimentoCaixa movimentoCaixa)
        {
            _financeiroRepository.SalvarMovimentacaoCaixa(movimentoCaixa);
        }

        public int QuantidadeRegistrosContasaReceber(int idclinica, int idunidadeatendimento)
        {
            return _financeiroRepository.QuantidadeRegistrosContasaReceber(idclinica, idunidadeatendimento);
        }

        public ICollection<FinanceiroParcela> ListarContasaPagarPorId(int id, int idclinica, int idunidadeatendimento)
        {
            return _financeiroRepository.ListarContasaPagarPorId(id,idclinica,idunidadeatendimento);
        }

        public ICollection<FinanceiroParcela> ListarContasaPagarPorCredor(string credor, int idclinica, int idunidadeatendimento)
        {
            return _financeiroRepository.ListarContasaPagarPorCredor(credor, idclinica, idunidadeatendimento);
        }

        public ICollection<MeioPagamento> ListarMeiosdePagamentos()
        {
            return _financeiroRepository.ListarMeiosdePagamentos();
        }

        public MeioPagamento ObterMeioPagamentoPorId(int id)
        {
            return _financeiroRepository.ObterMeioPagamentoPorId(id);
        }


        #region [Tranferencias entre contas]

        public List<TransferenciaConta> GetTransferencias()
        {
            return _financeiroRepository.GetTransferencias();
        }

        public TransferenciaConta GetTransferenciaById(int id)
        {
            return _financeiroRepository.GetTransferenciaById(id);
        }

        public void SalvarTransferencia(TransferenciaConta model)
        {
            _financeiroRepository.SalvarTransferencia(model);
        }

        public void ExcluirTransferencia(TransferenciaConta model)
        {
            _financeiroRepository.ExcluirTransferencia(model);
        }

        public FinanceiroParcela ObterFinanceiroParcelaPorId(int id)
        {
            return _financeiroRepository.ObterFinanceiroParcelaPorId(id);
        }

        public FinanceiroParcela AlterarParcela(FinanceiroParcela parcela)
        {
            return _financeiroRepository.AlterarParcela(parcela);
        }

        public ICollection<FinanceiroParcela> ListarParcelasPesquisa(string tipo, string descricao, string tipoConta, int idclinica, int idunidadeatendimento)
        {
            return _financeiroRepository.ListarParcelasPesquisa(tipo, descricao, tipoConta,idclinica,idunidadeatendimento);
        }

       
        #endregion
    }
}