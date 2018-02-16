using Clinicas.Domain.Model;
using Clinicas.Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinicas.Application.Services.Interfaces
{
    public interface IEstoqueService
    {
        ICollection<Material> ListaMateriais(int idunidade);
        Material ObterMaterialPorId(int idmaterial);
        void ExcluirMaterial(Material material);
        Material SalvarMaterial(Material material);

        ICollection<MovimentoEstoque> ListarMovimentoEstoque(int idunidade);
        MovimentoEstoque ObterMovimentoEstoquePorId(int idmovimentoestoque);
        void ExcluirMovimentoEstoque(MovimentoEstoque mov);
        MovimentoEstoque SalvarMovimentoEstoque(MovimentoEstoque movimento);

        ICollection<TipoMaterial> ListaTipoMateriais(int idunidade);
        TipoMaterial ObterTipoMaterialPorId(int idtipomaterial);
        void ExcluirTipoMaterial(TipoMaterial tipo);
        TipoMaterial SalvarTipoMaterial(TipoMaterial tipomaterial);
    }
}
