using Clinicas.Application.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Clinicas.Domain.Model;
using Clinicas.Infrastructure.Repository.Interfaces;

namespace Clinicas.Application.Services
{
    public class MedicamentoService : IMedicamentoService
    {
        private readonly IMedicamentoRepository _repository;

        public MedicamentoService(IMedicamentoRepository repository)
        {
            _repository = repository;
        }

        public void ExcluirMedicamento(int id)
        {
            _repository.ExcluirMedicamento(id);
        }

        public Medicamento GetMedicamentoById(int id)
        {
            return _repository.GetMedicamentoById(id);
        }

        public ICollection<Medicamento> ListarMedicamentos()
        {
            return _repository.ListarMedicamentos();
        }

        public ICollection<Medicamento> PesqusiarMedicamentos(string nome)
        {
            return _repository.PesqusiarMedicamentos(nome);
        }

        public Medicamento SalvarMedicamento(Medicamento model)
        {
            return _repository.SalvarMedicamento(model);
        }
    }
}