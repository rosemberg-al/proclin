using Clinicas.Domain.DTO;
using Clinicas.Domain.Model;
using Clinicas.Domain.Tiss;
using Clinicas.Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinicas.Infrastructure.Repository.Interfaces
{
    public interface IGuiaRepository
    {
        Guia ObterGuiaPorId(int idguia);
        void Salvar(Guia guia);
        void Excluir(Guia guia);
        void CancelarGuia(int idguia);
        List<Guia> ListarGuiasPorTipo(string tipoGuia, int idClinica);
        List<Guia> ListarGuiasBuscaAvancada(BuscaGuiaViewModel model);
        List<Guia> ListarGuiasPorConvenio(int idConvenio, int idClinica, string tipoGuia);
        List<Guia> ListarGuias();
        ICollection<Guia> Pesquisar(int? idGuia, string nome);

        #region[Lotes guias]
        Lote SalvarLote(Lote lote);
        void ExcluirLote(Lote lote);
        List<Lote> ListarLotes(int idClinica);
        Lote ObterLotePorId(int idLote);
        List<Lote> ListarLotesPorConvenio(int idClinica, int idConvenio);
        #endregion
    }
}
