using Clinicas.Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinicas.Application.Services.Interfaces
{
    public interface IBuscaService
    {
        ICollection<BuscaViewModel> Busca(string search);
    }
}
