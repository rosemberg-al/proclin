using Clinicas.Domain.DTO;
using Clinicas.Domain.Mail;
using Clinicas.Domain.Model;
using Clinicas.Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinicas.Infrastructure.Repository.Interfaces
{
    public interface IBuscaRepository
    {
        ICollection<BuscaViewModel> Busca(string search);
    }
}
