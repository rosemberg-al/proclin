using Clinicas.Domain.Model;
using Clinicas.Infrastructure.Base;
using Clinicas.Infrastructure.Context;
using Clinicas.Infrastructure.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace Clinicas.Infrastructure.Repository
{
    public class PacienteRepository : RepositoryBase<ClinicasContext>, IPacienteRepository
    {
        public PacienteRepository(IUnitOfWork<ClinicasContext> unit)
            : base(unit)
        {
        }
        public Paciente ObterPacientePorId(int id)
        {
            var paciente = Context.Paciente.Include(x=>x.Pessoa).FirstOrDefault(x => x.IdPaciente == id);

            if (paciente.IdDadosNascimento != null)
            {
                paciente.SetDadosNascimento(Context.DadosNascimento.Find(paciente.IdDadosNascimento));
            }
            paciente.SetCarteiras(Context.Carteira.Include(y => y.Convenio).Where(x => x.IdPaciente == paciente.IdPaciente).ToList());
            return paciente;
        }

        public void ExcluirCarteiraPaciente(List<Carteira> carteiras)
        {
            Context.Carteira.RemoveRange(carteiras);
            Context.SaveChanges();
        }

        public IEnumerable<Paciente> ListarPacientes()
        {
            return Context.Paciente.Include(x => x.Pessoa).Take(200).ToList();
        }

        public List<Paciente> ListarPacientesPorNome(string nome)
        {
            var paciente = Context.Paciente.Include(x=>x.Pessoa)
                .Where(x => x.Pessoa.Nome.ToUpper().StartsWith(nome.ToUpper()) && x.Pessoa.Situacao == "Ativo").Take(400).ToList();
            return paciente;
        }

        public List<Carteira> ListarCarteirasPaciente(int id)
        {
            var carteiras = Context.Carteira.Include(x=>x.Convenio.Pessoa).Where(x => x.IdPaciente == id && x.ValidadeCarteira >= DateTime.Now).ToList();
            return carteiras;
        }

        public Paciente SalvarPaciente(Paciente paciente)
        {
            if (paciente.IdPaciente > 0)
            {
                Context.Entry(paciente).State = EntityState.Modified;
            }
            else
            {
                Context.Paciente.Add(paciente);
            }
            Context.SaveChanges();
            return paciente;
        }

        public Anamnese SalvarAnamnese(Anamnese model)
        {
            if (model.IdAnamnese > 0)
            {
                Context.Entry(model).State = EntityState.Modified;
            }
            else
            {
                Context.Anamnese.Add(model);
            }
            Context.SaveChanges();
            return model;
        }

        public Anamnese ObterAnamnesePorId(int id)
        {
            return Context.Anamnese.FirstOrDefault(x => x.IdAnamnese == id);
        }

        public List<Anamnese> ListarAnamnesesPorPaciente(int idPaciente)
        {
            return Context.Anamnese.Where(x => x.IdPaciente == idPaciente).ToList();
        }

        public List<Vacina> ObterVacinasAtivas()
        {
            return Context.Vacina.Where(x => x.Situacao == "Ativo").ToList();
        }

        public Vacina ObterVacinaPorId(int id)
        {
            return Context.Vacina.FirstOrDefault(x => x.IdVacina == id);
        }

        public RegistroVacina ObterRegistroVacinaPorId(int id)
        {
            return Context.RegistroVacina.FirstOrDefault(x => x.IdRegistroVacina == id);
        }

        public RegistroVacina SalvarRegistroVacina(RegistroVacina model)
        {
            if (model.IdRegistroVacina > 0)
            {
                Context.Entry(model).State = EntityState.Modified;
            }
            else
            {
                Context.RegistroVacina.Add(model);
            }
            Context.SaveChanges();
            return model;
        }

        public List<RegistroVacina> ListarRegistroVacinaPorPaciente(int idPaciente)
        {
            return Context.RegistroVacina.Include(x => x.Vacina).Include(x => x.Paciente).Where(x => x.IdPaciente == idPaciente).ToList();
        }

        public DadosNascimento ObterDadosNascimentoPorIdPaciente(int id)
        {
            return Context.DadosNascimento.FirstOrDefault(x => x.IdDadosNascimento == id);
        }

        public DadosNascimento SalvarDadosNascimento(DadosNascimento dados)
        {
            if (dados.IdDadosNascimento > 0)
            {
                Context.Entry(dados).State = EntityState.Modified;
            }
            else
            {
                Context.DadosNascimento.Add(dados);
            }
            Context.SaveChanges();
            return dados;
        }

        #region [Estados e Cidades]

        public List<Estado> ListarEstados()
        {
            return Context.Estados.ToList();
        }
        public List<Cidade> ListarCidadesByEstado(int idEstado)
        {
            return Context.Cidades.Include(x => x.Estado).Where(x => x.IdEstado == idEstado).ToList();
        }

        public Estado ObterEstadoPorId(int idEstado)
        {
            return Context.Estados.FirstOrDefault(x => x.IdEstado == idEstado);
        }
        public Cidade ObterCidadePorId(int idCidade)
        {
            return Context.Cidades.FirstOrDefault(x => x.IdCidade == idCidade);
        }

        #endregion
    }
}