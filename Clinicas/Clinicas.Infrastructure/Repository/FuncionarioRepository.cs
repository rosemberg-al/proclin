using Clinicas.Infrastructure.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Clinicas.Domain.Model;
using Clinicas.Infrastructure.Base;
using Clinicas.Infrastructure.Context;
using System.Data.Entity;

namespace Clinicas.Infrastructure.Repository
{
    public class FuncionarioRepository : RepositoryBase<ClinicasContext>, IFuncionarioRepository
    {
        public FuncionarioRepository(IUnitOfWork<ClinicasContext> unit)
            : base(unit)
        {
        }

        #region [Funcionário]

        public List<Funcionario> ListarFuncionariosDeSaude()
        {   
            return Context.Funcionario.Where(x => x.Tipo == "Profissional de Saude").ToList();
        }

        public List<Funcionario> ListarFuncionariosDeSaudeAtivos()
        {
            return Context.Funcionario.Include(x=>x.Pessoa).Where(x => x.Tipo == "Profissional de Saude" && x.Pessoa.Situacao == "ATIVO").OrderBy(x=>x.Pessoa.Nome).ToList();
        }

        public Funcionario ObterFuncionariPorId(int id)
        {
            return Context.Funcionario.FirstOrDefault(x => x.IdFuncionario == id);
        }
        public List<Funcionario> ListarFuncionariosPorNome(string nome)
        {
            return Context.Funcionario.Where(x => x.Pessoa.Nome.ToUpper().Contains(nome.ToUpper())).ToList();
        }

        public List<Funcionario> ListarFuncionarios()
        {
            return Context.Funcionario.ToList();
        }

        public List<Especialidade> ListarEspecialidadesFuncionario(int idFuncionario)
        {
            var sql = @"select E.CdEspecialidade, E.NmEspecialidade from especialidade E, especialidadefuncionario F
                            where E.CdEspecialidade = F.CdEspecialidade
                            and F.CdFuncionario = "+ idFuncionario + "";
            return Context.Database.SqlQuery<Especialidade>(sql).ToList();
        }

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

        #endregion

        #region [Cadastro de Médicos]

        public Medico ObterMedicoPorId(int id)
        {
            return Context.Medico.FirstOrDefault(x => x.IdMedico == id);
        }

        public List<Medico> ListarMedicos()
        {
            return Context.Medico.ToList();
        }
        public List<Medico> ListarMedicosPorNome(string nome)
        {
            return Context.Medico.Where(x => x.NomeMedico.ToUpper().Contains(nome.ToUpper())).ToList();
        }

        public Medico SalvarMedico(Medico medico)
        {
            if (medico.IdMedico > 0)
            {
                Context.Entry(medico).State = EntityState.Modified;
            }
            else
            {
                Context.Medico.Add(medico);
            }
            Context.SaveChanges();
            return medico;
        }

        #endregion
    }
}