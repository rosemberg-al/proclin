using Clinicas.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinicas.Application.Services.Interfaces
{
    public interface IMedicamentoService
    {
        Medicamento SalvarMedicamento(Medicamento model);
        Medicamento GetMedicamentoById(int id);
        ICollection<Medicamento> PesqusiarMedicamentos(string nome);
        ICollection<Medicamento> ListarMedicamentos();
        void ExcluirMedicamento(int id);
    }
}
