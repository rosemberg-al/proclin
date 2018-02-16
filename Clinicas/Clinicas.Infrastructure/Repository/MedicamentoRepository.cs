using Clinicas.Infrastructure.Base;
using Clinicas.Infrastructure.Context;
using Clinicas.Infrastructure.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Clinicas.Domain.Model;

namespace Clinicas.Infrastructure.Repository
{
    public class MedicamentoRepository : RepositoryBase<ClinicasContext>, IMedicamentoRepository
    {
        public MedicamentoRepository(IUnitOfWork<ClinicasContext> unit)
            : base(unit)
        {
        }

        public void ExcluirMedicamento(int id)
        {
            throw new NotImplementedException();
        }

        public Medicamento GetMedicamentoById(int id)
        {
            return Context.Medicamentos.Find(id);
        }

        public ICollection<Medicamento> ListarMedicamentos()
        {
            return Context.Medicamentos.ToList();
        }

        public ICollection<Medicamento> PesqusiarMedicamentos(string nome)
        {
            return Context.Medicamentos.Where(x => x.Nome.ToUpper().Contains(nome.ToUpper())).ToList();
        }

        public Medicamento SalvarMedicamento(Medicamento model)
        {
            throw new NotImplementedException();
        }
    }
}