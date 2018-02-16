using Clinicas.Infrastructure.Base;
using Clinicas.Infrastructure.Context;
using Clinicas.Infrastructure.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Clinicas.Domain.Model;
using System.Data.Entity;

namespace Clinicas.Infrastructure.Repository
{
    public class ProntuarioRepository : RepositoryBase<ClinicasContext>, IProntuarioRepository
    {
        public ProntuarioRepository(IUnitOfWork<ClinicasContext> unit)
            : base(unit)
        {
        }

        #region [Ultimos Atendimentos]
        public ICollection<Agenda> UltimosAtendimentos(int idpaciente)
        {
            return Context.Agenda.Include(a=>a.ProfissionalSaude.Pessoa).Include(a=>a.Paciente.Pessoa).Where(x => x.IdPaciente == idpaciente).ToList();
        }
        #endregion

        #region [Anamnese]
        public Anamnese ObterAnamnesePorId(int id)
        {
            return Context.Anamnese.Include(x => x.Paciente.Pessoa).Include(x=>x.ProfissionalSaude.Pessoa).First(x => x.IdAnamnese == id);
        }

        public ICollection<Anamnese> ListarAnamnese(int id)
        {
            return Context.Anamnese
                .Include(x => x.Paciente.Pessoa)
                .Include(x => x.ProfissionalSaude.Pessoa)
                .Where(x => x.IdPaciente == id).ToList();
        }

        public void SalvarAnamnese(Anamnese anamnese)
        {
            if (anamnese.IdAnamnese > 0)
            {
                Context.Entry(anamnese).State = EntityState.Modified;
            }
            else
            {
                Context.Anamnese.Add(anamnese);
            }
            Context.SaveChanges();
        }

        public void ExcluirAnamnese(Anamnese anamnese)
        {
            anamnese.SetSituacao("Excluido");
            Context.Entry(anamnese).State = EntityState.Modified;
            Context.SaveChanges();
        }
        #endregion

        #region [Atestado]

        public Atestado GetAtestadoById(int id)
        {
            return Context.Atestado.FirstOrDefault(x => x.IdAtestado == id);
        }

        public List<Atestado> ListarAtestadoByPaciente(int id)
        {
            return Context.Atestado.Include(x => x.Paciente.Pessoa).Include(x => x.Funcionario.Pessoa).Where(x => x.IdPaciente == id).ToList();
        }

        public ModeloProntuario ObterModeloAtestado(int id)
        {
            return Context.ModeloProntuario.FirstOrDefault(x => x.IdModeloProntuario == id);
        }

        public List<ModeloProntuario> ListarModelosAtestadosAtivos()
        {
            return Context.ModeloProntuario.Where(x => x.Tipo == "Atestado" && x.Situacao == "Ativo").ToList();
        }

        public Atestado SalvarAtestadoPaciente(Atestado atestado)
        {
            if (atestado.IdAtestado > 0)
            {
                Context.Entry(atestado).State = EntityState.Modified;
            }
            else
            {
                Context.Atestado.Add(atestado);
            }
            Context.SaveChanges();
            return atestado;
        }

        public void ExcluirAtestado(int id)
        {
            var ates = Context.Atestado.Find(id);
            ates.SetSituacao("Excluido");
            Context.Entry(ates).State = EntityState.Modified;
            Context.SaveChanges();
        }
        #endregion

        #region [Modelo de Prontuário]
        public ModeloProntuario ObterModeloProntuarioPorId(int id)
        {
            return Context.ModeloProntuario.FirstOrDefault(x => x.IdModeloProntuario == id);
        }

        public ModeloProntuario SalvarModeloProntuario(ModeloProntuario modelo)
        {
            if (modelo.IdModeloProntuario > 0)
            {
                Context.Entry(modelo).State = EntityState.Modified;
            }
            else
            {
                Context.ModeloProntuario.Add(modelo);
            }
            Context.SaveChanges();
            return modelo;
        }
        public ICollection<ModeloProntuario> ListarModelosProntuario()
        {
            return Context.ModeloProntuario.ToList();
        }

        public ICollection<ModeloProntuario> ListarModeloProntuarioPorTipo(string tipo)
        {
            return Context.ModeloProntuario.Where(x=>x.Tipo==tipo).ToList();
        }

        public void ExcluirModelo(ModeloProntuario modelo)
        {
            Context.ModeloProntuario.Remove(modelo);
            Context.SaveChanges();
        }

        public ICollection<ModeloProntuario> PesquisarModelos(string nome)
        {
            if (!string.IsNullOrEmpty(nome))
            {
                return Context.ModeloProntuario.Where(x => x.NomeModelo.ToUpper().Contains(nome.ToUpper())).ToList();
            }else
            {
                return Context.ModeloProntuario.ToList();
            }
            
        }
        #endregion

        #region [História Pregressa]

        public HistoriaPregressa SalvarHistoriaPregressa(HistoriaPregressa historia)
        {
            if (historia.IdHistoriaPregressa > 0)
                Context.Entry(historia).State = EntityState.Modified;
            else
                Context.HistoriaPregressa.Add(historia);

            Context.SaveChanges();
            return historia;
        }

        public HistoriaPregressa ObterHistoriaPregressaPorId(int id)
        {
            return Context.HistoriaPregressa.Include(x => x.Paciente.Pessoa).Include(x => x.ProfissionalSaude.Pessoa).FirstOrDefault(x => x.IdHistoriaPregressa == id);
        }

        public void ExcluirHistoriaPregressa(int id)
        {
            var historia = Context.HistoriaPregressa.Find(id);
            historia.SetSituacao("Excluido");
            Context.Entry(historia).State = EntityState.Modified;
            Context.SaveChanges();
        }

        public ICollection<HistoriaPregressa> ListarHistoriaPregressa(int id)
        {
            return Context.HistoriaPregressa.Include(x=>x.Paciente.Pessoa).Include(x=>x.ProfissionalSaude.Pessoa).Where(x => x.IdPaciente == id).OrderByDescending(x=>x.IdHistoriaPregressa).ToList();
        }

        #endregion
               
        #region [Hospital]
        public Hospital ObterHospitalPorId(int id)
        {
            return Context.Hospital.FirstOrDefault(x => x.IdHospital == id);
        }

        public Hospital SalvarHospital(Hospital hospital)
        {
            if (hospital.IdHospital > 0)
            {
                Context.Entry(hospital).State = EntityState.Modified;
            }
            else
            {
                Context.Hospital.Add(hospital);
            }
            Context.SaveChanges();
            return hospital;
        }

        public ICollection<Hospital> ObterHospitais()
        {
            return Context.Hospital.ToList();
        }

        public List<Hospital> ListarHospitaisPorNome(string nome)
        {
             return Context.Hospital.Where(z => z.Nome.ToUpper().Contains(nome.ToUpper())).ToList();
        }

        #endregion

        #region [Medida Antropométrica]

        public ICollection<MedidasAntropometricas> ObterMedidasPorPaciente(int id)
        {
            return Context.MedidasAntropometricas.Where(x => x.IdPaciente == id).ToList();
        }

        public MedidasAntropometricas ObterMedidasPorId(int id)
        {
            return Context.MedidasAntropometricas.Include(x=>x.Paciente.Pessoa).Include(x=>x.ProfissionalSaude.Pessoa).FirstOrDefault(x => x.IdMedida == id);
        }

        public MedidasAntropometricas SalvarMedidaAntropometrica(MedidasAntropometricas medida)
        {
            if (medida.IdMedida > 0)
            {
                Context.Entry(medida).State = EntityState.Modified;
            }
            else
            {
                Context.MedidasAntropometricas.Add(medida);
            }
            Context.SaveChanges();
            return medida;
        }
        public void ExcluirMedidas(int id)
        {
            var medida = Context.MedidasAntropometricas.Find(id);
            Context.MedidasAntropometricas.Remove(medida);
            Context.SaveChanges();
        }
        #endregion

        #region [Receituário]

        public Receituario ObterReceituarioPorId(int id)
        {
            return Context.Receituario.FirstOrDefault(x => x.IdReceituario == id);
        }

        public List<Receituario> ListarReceituariosByPaciente(int id)
        {
            return Context.Receituario.Include(x => x.Paciente.Pessoa).Include(x => x.Funcionario.Pessoa).Where(x => x.IdPaciente == id).ToList();
        }

        public List<ModeloProntuario> ListarModelosReceituariosAtivos()
        {
            return Context.ModeloProntuario.Where(x => x.Tipo == "Receituario" && x.Situacao == "Ativo").ToList();
        }

        public List<ReceituarioMedicamento> ObterMedicamentosReceituario(int id)
        {
            return Context.ReceituarioMedicamento.Where(x => x.IdReceituario == id).ToList();
        }

        public Receituario SalvarReceituario(Receituario receituario)
        {
            if (receituario.IdReceituario > 0)
            {
                Context.Entry(receituario).State = EntityState.Modified;
            }
            else
            {
                Context.Receituario.Add(receituario);
            }
            Context.SaveChanges();
            return receituario;
        }

        public void ExcluirReceituario(int id)
        {
            var rec = Context.Receituario.Find(id);
            rec.SetSituacao("Excluido");
            Context.Entry(rec).State = EntityState.Modified;
            Context.SaveChanges();
        }

        #endregion

        #region [Cadastro de Vacinas]
        public Vacina ObterVacinaPorId(int id)
        {
            return Context.Vacina.FirstOrDefault(x => x.IdVacina == id);
        }
        public List<Vacina> ListarVacinas()
        {
            return Context.Vacina.ToList();
        }
        public Vacina SalvarVacina(Vacina vacina)
        {
            if (vacina.IdVacina > 0)
            {
                Context.Entry(vacina).State = EntityState.Modified;
            }
            else
            {
                Context.Vacina.Add(vacina);
            }
            Context.SaveChanges();
            return vacina;
        }
        #endregion

        #region [Requisições de Exames]
        public RequisicaoExames ObterRequisicaoExameById(int id)
        {
            return Context.RequisicaoExames.Include(p => p.Paciente).Include(x => x.Medico).FirstOrDefault(x => x.IdRequisicao == id);
        }
        public List<RequisicaoExames> ListarRequisicoesByPaciente(int id)
        {
            return Context.RequisicaoExames.Include(x => x.Medico).Include(x => x.Paciente).Where(x => x.IdPaciente == id).OrderByDescending(x => x.Data).ToList();
        }
        public RequisicaoExames SalvarRequisicaoExame(RequisicaoExames requisicao)
        {
            if (requisicao.IdRequisicao > 0)
            {
                Context.Entry(requisicao).State = EntityState.Modified;
            }
            else
            {
                Context.RequisicaoExames.Add(requisicao);
            }
            Context.SaveChanges();
            return requisicao;
        }
        #endregion

        #region [Odontograma]
        public Odontograma ObterOdontogramaPorId(int id)
        {
            return Context.Odontogramas.Include(p => p.Paciente.Pessoa).Include(p=>p.Funcionario.Pessoa).Where(x=>x.Situacao!="Excluido").FirstOrDefault(x => x.IdOdontograma == id);
        }
        public ICollection<Odontograma> ListarOdontogramaPorIdPaciente(int id)
        {
            return Context.Odontogramas.Include(p => p.Paciente.Pessoa).Include(p => p.Funcionario.Pessoa).Where(x => x.Situacao != "Excluido" && x.IdPaciente == id).ToList();
        }
        public Odontograma SalvarOdontograma(Odontograma odontograma)
        {
            if (odontograma.IdOdontograma > 0)
            {
                Context.Entry(odontograma).State = EntityState.Modified;
            }
            else
            {
                Context.Odontogramas.Add(odontograma);
            }
            Context.SaveChanges();
            return odontograma;
        }

        public Odontograma ExcluirOdontograma(Odontograma odontograma)
        {
            odontograma.SetSituacao("Excluido");
            Context.Entry(odontograma).State = EntityState.Modified;
            Context.SaveChanges();
            return odontograma;
        }
        #endregion

    }
}