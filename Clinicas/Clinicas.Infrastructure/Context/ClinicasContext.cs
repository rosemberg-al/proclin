using Clinicas.Domain.Mail;
using Clinicas.Domain.Model;
using Clinicas.Domain.Tiss;
using Clinicas.Infrastructure.Models.Mapping;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace Clinicas.Infrastructure.Context
{
    public partial class ClinicasContext : DbContext
    {
        static ClinicasContext()
        {
            Database.SetInitializer<ClinicasContext>(null);
        }

        public DbSet<MailQueue> MailQueue { get; set; }
        public DbSet<MailTemplate> MailTemplates { get; set; }
        public DbSet<Pessoa> Pessoa { get; set; }
        public DbSet<Paciente> Paciente { get; set; }
        public DbSet<Carteira> Carteira { get; set; }
        public DbSet<Anamnese> Anamnese { get; set; }
        public DbSet<Atestado> Atestado { get; set; }
        public DbSet<ReceituarioMedicamento> ReceituarioMedicamento { get; set; }
        public DbSet<TabelaPreco> TabelaPreco { get; set; }
        public DbSet<TabelaPrecoItens> Itens { get; set; }
        public DbSet<Cidade> Cidades { get; set; }
        public DbSet<Procedimento> Procedimento { get; set; }
        public DbSet<Especialidade> Especialidade { get; set; }
        public DbSet<Medicamento> Medicamentos { get; set; }
        public DbSet<Estado> Estados { get; set; }
        public DbSet<UnidadeAtendimento> Unidades { get; set; }
        public DbSet<Convenio> Convenio { get; set; }
        public DbSet<Vacina> Vacina { get; set; }
        public DbSet<Fornecedor> Fornecedor { get; set; }
        public DbSet<Medico> Medico { get; set; }
        public DbSet<Ocupacao> Ocupacao { get; set; }
        public DbSet<Receituario> Receituario { get; set; }
        public DbSet<Funcionario> Funcionario { get; set; }
        public DbSet<RegistroVacina> RegistroVacina { get; set; }
        public DbSet<RequisicaoExames> RequisicaoExames { get; set; }
        public DbSet<DadosNascimento> DadosNascimento { get; set; }
        public DbSet<HistoriaPregressa> HistoriaPregressa { get; set; }
        public DbSet<Hospital> Hospital { get; set; }
        public DbSet<MedidasAntropometricas> MedidasAntropometricas { get; set; }
        public DbSet<ModeloProntuario> ModeloProntuario { get; set; }
        public DbSet<Cid> Cid { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Funcionalidade> Funcionalidade { get; set; }
        public DbSet<GrupoUsuario> GrupoUsuario { get; set; }
        public DbSet<Guia> Guia { get; set; }
        public DbSet<Lote> Lote { get; set; }
        public DbSet<TipoAtendimento> TipoAtendimento { get; set; }
        public DbSet<Agenda> Agenda { get; set; }
        public DbSet<Banco> Bancos { get; set; }
        public DbSet<Cheque> Cheques { get; set; }
        public DbSet<Financeiro> Financeiroes { get; set; }
        public DbSet<FinanceiroParcela> FinanceiroParcelas { get; set; }
        public DbSet<Remessa> Remessa { get; set; }
        public DbSet<MeioPagamento> MeioPagamentoes { get; set; }
        public DbSet<PlanoConta> PlanoContas { get; set; }
        public DbSet<Conta> Contas { get; set; }
        public DbSet<TransferenciaConta> Transferencias { get; set; }
        public DbSet<Modulo> Moduloes { get; set; }
        public DbSet<Clinica> Clinica { get; set; }
        public DbSet<Odontograma> Odontogramas { get; set; }
        public DbSet<Consultorio> Consultorios { get; set; }
        public DbSet<BloqueioAgenda> BloqueioAgenda { get; set; }

        public DbSet<MovimentoEstoque> MovimentoEstoque { get; set; }
        public DbSet<Material> Material { get; set; }
        public DbSet<TipoMaterial> TipoMaterial { get; set; }

        public ClinicasContext()
            : base("ConectClinicas")
        {
            this.Configuration.LazyLoadingEnabled = false;
            this.Configuration.ProxyCreationEnabled = false;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Entity<MailTemplate>().ToTable("MailTemplate");
            modelBuilder.Entity<MailTemplate>().HasKey(p => p.Id);
            modelBuilder.Entity<MailTemplate>().Property(p => p.Filename)
                .HasMaxLength(100)
                .IsRequired();
            modelBuilder.Entity<MailTemplate>().Property(p => p.Folder)
               .HasMaxLength(50)
               .IsRequired();
            modelBuilder.Entity<MailTemplate>().Property(p => p.Subject)
                .HasMaxLength(120)
                .IsRequired();
            modelBuilder.Entity<MailTemplate>().Property(p => p.Description)
                .HasMaxLength(300);
            modelBuilder.Entity<MailTemplate>().Property(p => p.Identifier)
                .HasMaxLength(50);

            modelBuilder.Entity<MailQueue>().ToTable("MailQueue");
            modelBuilder.Entity<MailQueue>().HasKey(p => p.Id);
            modelBuilder.Entity<MailQueue>()
                .HasRequired(p => p.Template)
                .WithMany()
                .HasForeignKey(p => p.TemplateId);

            modelBuilder.Configurations.Add(new PessoaMap());
            modelBuilder.Configurations.Add(new BloqueioAgendaMap());
            modelBuilder.Configurations.Add(new TabelaPrecoItensMap());
            modelBuilder.Configurations.Add(new TabelaPrecoMap());
            modelBuilder.Configurations.Add(new AnamneseMap());
            modelBuilder.Configurations.Add(new PacienteMap());
            modelBuilder.Configurations.Add(new AtestadoMap());
            modelBuilder.Configurations.Add(new ReceituarioMedicamentoMap());
            modelBuilder.Configurations.Add(new DadosNascimentoMap());
            modelBuilder.Configurations.Add(new HistoriaPregressaMap());
            modelBuilder.Configurations.Add(new HospitalMap());
            modelBuilder.Configurations.Add(new MedidasAntropometricasMap());
            modelBuilder.Configurations.Add(new ModeloProntuarioMap());
            modelBuilder.Configurations.Add(new RequisicaoExamesMap());
            modelBuilder.Configurations.Add(new VacinaMap());
            modelBuilder.Configurations.Add(new RegistroVacinaMap());
            modelBuilder.Configurations.Add(new FuncionarioMap());
            modelBuilder.Configurations.Add(new ReceituarioMap());
            modelBuilder.Configurations.Add(new MedicoMap());
            modelBuilder.Configurations.Add(new EstadosMap());
            modelBuilder.Configurations.Add(new CidadesMap());
            modelBuilder.Configurations.Add(new EspecialidadeMap());
            modelBuilder.Configurations.Add(new ProcedimentoMap());
            modelBuilder.Configurations.Add(new ConvenioMap());
            modelBuilder.Configurations.Add(new CidMap());
            modelBuilder.Configurations.Add(new OcupacaoMap());
            modelBuilder.Configurations.Add(new FornecedorMap());
            modelBuilder.Configurations.Add(new GuiaMap());
            modelBuilder.Configurations.Add(new CarteiraMap());
            modelBuilder.Configurations.Add(new UsuarioMap());
            modelBuilder.Configurations.Add(new FuncionalidadeMap());
            modelBuilder.Configurations.Add(new GrupoUsuarioMap());
            modelBuilder.Configurations.Add(new ModuloMap());
            modelBuilder.Configurations.Add(new UnidadeAtendimentoMap());
            modelBuilder.Configurations.Add(new AgendaMap());
            modelBuilder.Configurations.Add(new TipoAtendimentoMap());
            modelBuilder.Configurations.Add(new ContaMap());
            modelBuilder.Configurations.Add(new FinanceiroMap());
            modelBuilder.Configurations.Add(new FinanceiroParcelaMap());
            modelBuilder.Configurations.Add(new MeioPagamentoMap());
            modelBuilder.Configurations.Add(new RemessaMap());
            modelBuilder.Configurations.Add(new ChequeMap());
            modelBuilder.Configurations.Add(new PlanoContaMap());
            modelBuilder.Configurations.Add(new BancoMap());
            modelBuilder.Configurations.Add(new TransferenciaContaMap());
            modelBuilder.Configurations.Add(new ClinicaMap());
            modelBuilder.Configurations.Add(new MedicamentoMap());
            modelBuilder.Configurations.Add(new LoteMap());
            modelBuilder.Configurations.Add(new OdontogramaMap());
            modelBuilder.Configurations.Add(new ConsultorioMap());

            modelBuilder.Configurations.Add(new TipoMaterialMap());
            modelBuilder.Configurations.Add(new MaterialMap());
            modelBuilder.Configurations.Add(new MovimentoEstoqueMap());

        }
    }
}