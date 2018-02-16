using Clinicas.Domain.Model;
using Clinicas.Infrastructure.Base;
using Clinicas.Infrastructure.Context;
using Clinicas.Infrastructure.Repository;
using Clinicas.Infrastructure.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace Clinicas.Infrastructure.Repository
{
    public class FinanceiroRepository : RepositoryBase<ClinicasContext>, IFinanceiroRepository
    {
        public FinanceiroRepository(IUnitOfWork<ClinicasContext> unit)
            : base(unit)
        {
        }
       
        #region [Conta]
        public void SalvarConta(Conta conta)
        {
            if (conta.IdConta > 0)
            {
                Context.Entry(conta).State = EntityState.Modified;
            }
            else
            {
                Context.Contas.Add(conta);
            }
            Context.SaveChanges();
        }

        public void ExcluirConta(Conta conta)
        {
            conta.Situacao = "Excluido";
            Context.Entry(conta).State = EntityState.Modified;
            Context.SaveChanges();
        }

        public ICollection<Conta> ListarContas(int idclinica)
        {
            return Context.Contas.Where(x=>x.Situacao!="Excluido").Where(x=>x.IdClinica==idclinica).ToList();
        }

        public Conta ObterContaPorId(int id)
        {
            return Context.Contas.Find(id);
        }

        public ICollection<Conta> PesquisarContas(string nome,int? codigo,int idclinica)
        {
            ICollection<Conta> resultado = null;

            Expression<Func<Conta, bool>> filtroNome = registro => true;
            Expression<Func<Conta, bool>> filtroCodigo = registro => true;

            if (!string.IsNullOrEmpty(nome))
                filtroNome = (Conta registro) =>
                           registro.NmConta.ToUpper().Contains(nome.ToUpper());

            if (codigo > 0)
                filtroCodigo = (Conta registro) => registro.IdConta == codigo;


            resultado = Context.Contas.Where(x=>x.Situacao!="Excluido")
                .Where(filtroNome).Where(filtroCodigo).Where(x=>x.IdClinica==idclinica).ToList();

            return resultado;
        }
        #endregion

        #region [Plano Conta]

        public ICollection<PlanoConta> ListarPlanoContas(int idclinica)
        {
            return Context.PlanoContas.Where(x => x.Situacao != "Excluido").Where(x => x.IdClinica==idclinica).ToList();
        }

        public ICollection<PlanoConta> PesquisarPlanoContas(string nome, int? codigo, string tipo,int idclinica)
        {
            ICollection<PlanoConta> resultado = null;

            Expression<Func<PlanoConta, bool>> filtroNome = registro => true;
            Expression<Func<PlanoConta, bool>> filtroCodigo = registro => true;
            Expression<Func<PlanoConta, bool>> filtroTipo= registro => true;

            if (!string.IsNullOrEmpty(nome))
                filtroNome = (PlanoConta registro) =>
                           registro.NmPlanoConta.ToUpper().Contains(nome.ToUpper());

            if (codigo > 0)
                filtroCodigo = (PlanoConta registro) => registro.IdPlanoConta == codigo;


            if (!string.IsNullOrEmpty(tipo))
                filtroTipo = (PlanoConta registro) =>
                           registro.Tipo.ToUpper().Contains(tipo.ToUpper());


            resultado = Context.PlanoContas.Where(x => x.Situacao != "Excluido")
                .Where(filtroNome).Where(filtroCodigo).Where(filtroTipo).Where(x=>x.IdClinica==idclinica).ToList();

            return resultado;
        }

        public void SalvarPlanoConta(PlanoConta planoConta)
        {
            if (planoConta.IdPlanoConta > 0)
            {
                Context.Entry(planoConta).State = EntityState.Modified;
            }
            else
            {
                Context.PlanoContas.Add(planoConta);
            }
            Context.SaveChanges();
        }

        public void ExcluirPlanoConta(PlanoConta planoConta)
        {
            Context.PlanoContas.Remove(planoConta);
            Context.SaveChanges();
        }

        public PlanoConta ObterPlanodeContasPorId(int id)
        {
            return Context.PlanoContas.FirstOrDefault(x => x.IdPlanoConta == id);
        }
        #endregion

        #region [Contas a Pagar - Contas a Receber]

        public void SalvarFinanceiro(Financeiro financeiro)
        {
            if (financeiro.IdFinanceiro > 0)
            {
                Context.Entry(financeiro).State = EntityState.Modified;
            }
            else
            {
                Context.Financeiroes.Add(financeiro);
            }
            Context.SaveChanges();
        }

        public void ExcluirFinanceiro(Financeiro financeiro)
        {
            Context.Entry(financeiro).State = EntityState.Modified;
            Context.SaveChanges();
        }

        public MeioPagamento GetMeioPagamentoPorId(int id)
        {
            return Context.MeioPagamentoes.FirstOrDefault(x => x.IdMeioPagamento == id);
        }

        public FinanceiroParcela AlterarParcela(FinanceiroParcela parcela)
        {
            Context.Entry(parcela).State = EntityState.Modified;
            Context.SaveChanges();
            return parcela;
        }

        public Financeiro ObterFinanceiroPorId(int id)
        {
            return Context.Financeiroes
                .Include(x => x.Pessoa)
                .Include(m => m.Parcelas)
                .Include(m=>m.UnidadeAtendimento)
                .Include(m=>m.UnidadeAtendimento)
                .FirstOrDefault(x => x.IdFinanceiro == id);
        }

        public ICollection<FinanceiroParcela> ListarContasaReceber(int idclinica,int idunidadeatendimento)
        {
            return Context.FinanceiroParcelas.Include(m => m.Financeiro)
                .Include(m => m.Financeiro.Pessoa)
                .Include(m=>m.Financeiro.UnidadeAtendimento)
                .Include(m=>m.Financeiro.Clinica)
                .Include(m => m.PlanoConta)
                .Where(x => x.Financeiro.Tipo == "Contas a Receber" && x.Financeiro.IdClinica == idclinica && x.Financeiro.IdUnidadeAtendimento == idunidadeatendimento).ToList();
        }

        public bool IsNumeric(string data)
        {
            bool isnumeric = false;
            char[] datachars = data.ToCharArray();

            foreach (var datachar in datachars)
                isnumeric = isnumeric ? isnumeric : char.IsDigit(datachar);
            return isnumeric;
        }

        public ICollection<FinanceiroParcela> ListarParcelasPesquisa(string tipo, string descricao, string tipoConta,int idclinica, int idunidadeatendimento)
        {
            tipoConta = tipoConta == "R" ? "Contas a Receber" : "Contas a Pagar";

            //  Expression<Func<Contrato, bool>> filtro = registro => true;
            var q = Context.FinanceiroParcelas.Include(m => m.Financeiro)
                .Include(m => m.Financeiro.Pessoa)
                .Include(m => m.PlanoConta);

            if (!string.IsNullOrEmpty(descricao))
            {
                switch (tipo)
                {
                    case "Codigo":
                        if (this.IsNumeric(descricao))
                        {
                            int id = Convert.ToInt32(descricao);
                            if (id > 0)
                                q = q.Where(x => x.Financeiro.Tipo == tipoConta && x.IdFinanceiro == id);
                        }
                    break;
                    case "CpfCnpj":
                        q = q.Where(x => x.Financeiro.Tipo == tipoConta && x.Financeiro.Pessoa.CpfCnpj.ToUpper() == descricao.ToUpper());
                    break;
                    case "Nome":
                        q = q.Where(x => x.Financeiro.Tipo == tipoConta && x.Financeiro.Pessoa.Nome.ToUpper().Contains(descricao.ToUpper()));
                    break;
                }
            }
            return q.Where(x=>x.Financeiro.IdClinica==idclinica && x.Financeiro.IdUnidadeAtendimento == idunidadeatendimento).ToList();
        }

        public ICollection<FinanceiroParcela> ListarContasaReceberAberto(int idclinica,int idunidadeatendimento)
        {
            return
                Context.FinanceiroParcelas.Include(m => m.Financeiro)
                .Include(m => m.PlanoConta)
                .Include(m => m.Financeiro.Pessoa)
                .Include(m=>m.Financeiro.UnidadeAtendimento)
                .Include(m=>m.Financeiro.Clinica)
                .Where(x => x.Situacao == "Aberto" && x.Financeiro.Tipo == "Contas a Receber" 
                 && x.Financeiro.IdClinica == idclinica 
                 && x.Financeiro.IdUnidadeAtendimento == idunidadeatendimento)
                .ToList();
        }

        public ICollection<FinanceiroParcela> ListarContasaReceberBaixado(int idclinica, int idunidadeatendimento)
        {
            return
                Context.FinanceiroParcelas.Include(m => m.Financeiro)
                  .Include(m => m.PlanoConta)
                .Include(m => m.Financeiro.Pessoa)
                 .Include(m => m.Financeiro.UnidadeAtendimento)
                 .Include(m => m.Financeiro.Clinica)
                 .Where(x => x.Situacao == "Baixado" && x.Financeiro.Tipo == "Contas a Receber" 
                 && x.Financeiro.IdClinica == idclinica 
                 && x.Financeiro.IdUnidadeAtendimento == idunidadeatendimento)
                .ToList();
        }

        public decimal TotalContasaReceber(int idclinica, int idunidadeatendimento)
        {
            var sum = Context.FinanceiroParcelas.Include(m => m.Financeiro)
                .Where(x => x.Situacao == "Aberto" 
                && x.Financeiro.IdClinica==idclinica 
                && x.Financeiro.IdUnidadeAtendimento == idunidadeatendimento 
                && x.Financeiro.Tipo == "Contas a Receber").Sum(x => x.TotalAcerto);
            if (sum != null)
            {
                decimal total = (decimal)sum;

                return total;
            }
            else
            {
                return 0;
            }
        }

        public decimal TotalContasaReceberHoje(int idclinica, int idunidadeatendimento)
        {
            var sum = Context.FinanceiroParcelas.Include(m => m.Financeiro)
                 .Where(x => x.Situacao == "Aberto" 
                 && x.Financeiro.IdClinica == idclinica 
                 && x.Financeiro.IdUnidadeAtendimento == idunidadeatendimento
                 && x.Financeiro.Tipo == "Contas a Receber" 
                 && x.DataVencimento == DateTime.Now).Sum(x => x.TotalAcerto);
            if (sum != null)
            {
                decimal total = (decimal)sum;

                return total;
            }
            else
            {
                return 0;
            }
        }

        public decimal TotalContasaReceberAmanha(int idclinica, int idunidadeatendimento)
        {
            var sum = Context.FinanceiroParcelas.Include(m => m.Financeiro)
                  .Where(x => x.Situacao == "Aberto" 
                  && x.Financeiro.IdClinica == idclinica 
                  && x.Financeiro.IdUnidadeAtendimento == idunidadeatendimento 
                  && x.Financeiro.Tipo == "Contas a Receber" && x.DataVencimento == DateTime.Now.AddDays(1)).Sum(x => x.TotalAcerto);
            if (sum != null)
            {
                decimal total = (decimal)sum;

                return total;
            }
            else
            {
                return 0;
            }
        }

        public ICollection<FinanceiroParcela> ListarContasaPagar(int idclinica,int idunidadeatendimento)
        {
            return Context.FinanceiroParcelas.Include(m => m.Financeiro)
                .Include(m => m.Financeiro.Pessoa)
                .Include(m => m.PlanoConta)
                .Where(x => x.Financeiro.Tipo == "Contas a Pagar" && x.Financeiro.IdClinica == idclinica && x.Financeiro.IdUnidadeAtendimento == idunidadeatendimento).ToList();
        }

        public ICollection<FinanceiroParcela> ListarContasaPagarAberto(int idclinica, int idunidadeatendimento)
        {
            return Context.FinanceiroParcelas.Include(m => m.Financeiro)
                .Where(x => x.Financeiro.Tipo == "Contas a Pagar" 
                && x.Situacao == "Aberto" && x.Financeiro.IdClinica == idclinica && x.Financeiro.IdUnidadeAtendimento == idunidadeatendimento).ToList();
        }

        public ICollection<FinanceiroParcela> ListarContasaPagarBaixado(int idclinica, int idunidadeatendimento)
        {
            return Context.FinanceiroParcelas.Include(m => m.Financeiro)
                .Where(x => x.Financeiro.Tipo == "Contas a Pagar" 
                && x.Situacao == "Baixado" && x.Financeiro.IdClinica == idclinica && x.Financeiro.IdUnidadeAtendimento==idunidadeatendimento).ToList();
        }

        public decimal TotalContasaPagar(int idclinica, int idunidadeatendimento)
        {
            var sum = Context.FinanceiroParcelas.Include(m => m.Financeiro)
                  .Where(x => x.Situacao == "Aberto" && x.Financeiro.Tipo == "Contas a Pagar" 
                  && x.Financeiro.IdClinica==idclinica 
                  && x.Financeiro.IdUnidadeAtendimento==idunidadeatendimento).Sum(x => x.Valor);
            if (sum != null)
            {
                decimal total = (decimal)sum;

                return total;
            }
            else
            {
                return 0;
            }
        }

        public decimal TotalContasaPagarHoje(int idclinica, int idunidadeatendimento)
        {
            var sum = Context.FinanceiroParcelas.Include(m => m.Financeiro)
                  .Where(x => x.Situacao == "Aberto" && x.Financeiro.Tipo == "Contas a Pagar" && x.DataVencimento == DateTime.Now).Sum(x => x.Valor);
            if (sum != null)
            {
                decimal total = (decimal)sum;

                return total;
            }
            else
            {
                return 0;
            }
        }

        public decimal TotalContasaPagarAmanha(int idclinica, int idunidadeatendimento)
        {

            var sum = Context.FinanceiroParcelas.Include(m => m.Financeiro)
                  .Where(x => x.Situacao == "Aberto" && x.Financeiro.Tipo == "Contas a Pagar" 
                  && x.DataVencimento == DateTime.Now.AddDays(1)
                  && x.Financeiro.IdUnidadeAtendimento == idunidadeatendimento
                  && x.Financeiro.IdClinica == idclinica).Sum(x => x.Valor);
            if (sum != null)
            {
                decimal total = (decimal)sum;

                return total;
            }
            else
            {
                return 0;
            }
        }

        #endregion

        #region [Cheque]

        public void SalvarCheque(Cheque cheque)
        {
            if (cheque.IdCheque > 0)
            {
                Context.Entry(cheque).State = EntityState.Modified;
            }
            else
            {
                Context.Cheques.Add(cheque);
            }
            Context.SaveChanges();
        }

        public void ExcluirCheque(Cheque cheque)
        {
            cheque.SetSituacao("Excluido");
            Context.Entry(cheque).State = EntityState.Modified;
            Context.SaveChanges();
        }

        public Cheque ObterChequePorId(int id)
        {
            return Context.Cheques.Include(x => x.Pessoa).FirstOrDefault(x => x.IdCheque == id);
        }

        public ICollection<Cheque> ListarCheques(int idClinica)
        {
            return Context.Cheques.Where(x => x.IdClinica == idClinica && x.Situacao!="Excluido").ToList();
        }

        public ICollection<Cheque> PesquisarCheques(string emitente, string banco, int idClinica)
        {
            ICollection<Cheque> resultado = null;

            Expression<Func<Cheque, bool>> filtroEmitente = registro => true;
            Expression<Func<Cheque, bool>> filtroBanco = registro => true;

            if (!string.IsNullOrEmpty(emitente))
                filtroEmitente = (Cheque registro) =>
                           registro.Emitente.ToUpper().Contains(emitente.ToUpper());

            if (!string.IsNullOrEmpty(banco))
                filtroBanco = (Cheque registro) =>
                           registro.Banco.ToUpper().Contains(banco.ToUpper());


            resultado = Context.Cheques.Where(x => x.Situacao != "Excluido")
                .Where(filtroEmitente).Where(filtroBanco).Where(x => x.IdClinica == idClinica).ToList();

            return resultado;
        }

        #endregion

        #region [Banco]

        public void SalvarBanco(Banco banco)
        {
            if (banco.IdBanco > 0)
            {
                Context.Entry(banco).State = EntityState.Modified;
            }
            else
            {
                Context.Bancos.Add(banco);
            }
            Context.SaveChanges();
        }

        public void ExcluirBanco(Banco banco)
        {
            Context.Bancos.Remove(banco);
            Context.SaveChanges();
        }

        public ICollection<Banco> ListarBancos()
        {
            return Context.Bancos.OrderBy(x=>x.NomeBanco).ToList();
        }

        public Banco ObterBancoPorId(int id)
        {
            return Context.Bancos.FirstOrDefault(x => x.IdBanco == id);
        }

        #endregion

        #region [Centro de Custo]

        public void SalvarCentroCusto(CentroCusto centroCusto)
        {
            if (centroCusto.IdCentroCusto > 0)
            {
                Context.Entry(centroCusto).State = EntityState.Modified;
            }
            else
            {
                //  Context.CentroCustos.Add(centroCusto);
            }
            Context.SaveChanges();
        }

        public void ExcluirCentroCusto(CentroCusto centroCusto)
        {
            // Context.CentroCustos.Remove(centroCusto);
            Context.SaveChanges();
        }

        public CentroCusto ObterCentroCustoPorId(int id)
        {
            //return Context.CentroCustos.Find(id);   
            return null;
        }

        public ICollection<CentroCusto> ListarCentroCustos()
        {
            return null; // return Context.CentroCustos.ToList();
        }

        #endregion

        public void TransferenciaConta(TransferenciaConta transferenciaConta)
        {
            throw new NotImplementedException();
        }

        public void SalvarCaixaDiario(CaixaDiario caixaDiario)
        {
            throw new NotImplementedException();
        }

        public void ExcluirCaixaDiario(CaixaDiario caixaDiario)
        {
            throw new NotImplementedException();
        }

        public void SalvarMovimentacaoCaixa(MovimentoCaixa movimentoCaixa)
        {
            throw new NotImplementedException();
        }

        public int QuantidadeRegistrosContasaReceber(int idclinica,int idunidadeatendimento)
        {
            return Context.FinanceiroParcelas.Include(x=>x.Financeiro).Where(x=>x.Financeiro.IdClinica==idclinica && x.Financeiro.IdUnidadeAtendimento==idunidadeatendimento).Count();
        }

        public ICollection<FinanceiroParcela> ListarContasaPagarPorId(int id,int idclinica,int idunidadeatendimento)
        {
            return Context.FinanceiroParcelas.Include(m => m.Financeiro).Include(m => m.Financeiro.Pessoa).Where(x => x.Financeiro.Tipo == "Contas a Pagar"
            && x.IdFinanceiro == id && x.Financeiro.IdClinica==idclinica && x.Financeiro.IdUnidadeAtendimento==idunidadeatendimento).ToList();
        }

        public ICollection<FinanceiroParcela> ListarContasaPagarPorCredor(string credor,int idclinica,int idunidadeatendimento)
        {
            return Context.FinanceiroParcelas.Include(m => m.Financeiro)
                .Include(m => m.Financeiro.Pessoa)
                .Where(x => x.Financeiro.Tipo == "Contas a Pagar"
                && x.Financeiro.Pessoa.Nome.Contains(credor) && x.Financeiro.IdClinica == idclinica && x.Financeiro.IdUnidadeAtendimento == idunidadeatendimento).ToList();
        }

        public ICollection<MeioPagamento> ListarMeiosdePagamentos()
        {
            return Context.MeioPagamentoes.ToList();
        }

        public MeioPagamento ObterMeioPagamentoPorId(int id)
        {
            return Context.MeioPagamentoes.FirstOrDefault(x => x.IdMeioPagamento == id);
        }

        #region [Tranferencias entre contas]
        public List<TransferenciaConta> GetTransferencias()
        {
            return Context.Transferencias.Include(x => x.ContaDestino).Include(x => x.ContaOrigem).ToList();
        }

        public TransferenciaConta GetTransferenciaById(int id)
        {
            return Context.Transferencias.Include(x => x.ContaDestino).Include(x => x.ContaOrigem).FirstOrDefault(x => x.IdTransferenciaConta == id);
        }

        public void ExcluirTransferencia(TransferenciaConta model)
        {
            Context.Transferencias.Remove(model);
            Context.SaveChanges();
        }

        public void SalvarTransferencia(TransferenciaConta model)
        {
            if (model.IdTransferenciaConta > 0)
            {
                Context.Entry(model).State = EntityState.Modified;
            }
            else
            {
                Context.Transferencias.Add(model);
            }
            Context.SaveChanges();
        }

        public FinanceiroParcela ObterFinanceiroParcelaPorId(int id)
        {
            return Context.FinanceiroParcelas.Include(x => x.Financeiro).Include(x => x.Conta).Include(x => x.MeioPagamento).Include(x => x.Financeiro.Pessoa).FirstOrDefault(x => x.IdParcela == id);
        }

        #endregion
    }
}