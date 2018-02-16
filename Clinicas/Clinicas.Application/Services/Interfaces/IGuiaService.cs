using Clinicas.Domain.DTO;
using Clinicas.Domain.Model;
using Clinicas.Domain.Tiss;
using Clinicas.Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinicas.Application.Services.Interfaces
{
    public interface IGuiaService
    {
        Guia ObterGuiaPorId(int idguia);
        void Salvar(Guia guia);
        void Excluir(Guia guia);
        void CancelarGuia(int idguia);
        List<Guia> ListarGuiasPorTipo(string tipoGuia, int idClinica);
        List<Guia> ListarGuiasBuscaAvancada(BuscaGuiaViewModel model);
        List<Guia> ListarGuiasPorConvenio(int idConvenio, int idClinica, string tipoGuia);

        ICollection<Guia> Pesqusiar(int? idguia, string nome);

        List<Guia> ListarGuias();
        byte[] GetGuiaConsultaPdf(Int32 idGuia);
        byte[] GetGuiaSpSadtPdf(Int32 idGuia);

        #region[Lotes guias]
        Lote SalvarLote(Lote lote);
        void ExcluirLote(Lote lote);
        List<Lote> ListarLotes(int idClinica);
        Lote ObterLotePorId(int idLote);
        List<Lote> ListarLotesPorConvenio(int idClinica, int idConvenio);
        #endregion
    }
}