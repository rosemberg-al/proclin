using Clinicas.Application.Services.Interfaces;
using Clinicas.Domain.Model;
using Clinicas.Infrastructure.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Clinicas.Domain.DTO;
using Clinicas.Domain.ViewModel;
using Clinicas.Domain.Mail;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Configuration;
using System.IO;
using System.Net.Mail;

namespace Clinicas.Application.Services
{
    public class EstoqueService : IEstoqueService
    {
        private readonly IEstoqueRepository _repository;

        public EstoqueService(IEstoqueRepository repository)
        {
            _repository = repository;
        }

        public void ExcluirMaterial(Material material)
        {
            _repository.ExcluirMaterial(material);
        }

        public void ExcluirMovimentoEstoque(MovimentoEstoque mov)
        {
            _repository.ExcluirMovimentoEstoque(mov);
        }

        public void ExcluirTipoMaterial(TipoMaterial tipo)
        {
            _repository.ExcluirTipoMaterial(tipo);
        }

        public ICollection<Material> ListaMateriais(int idunidade)
        {
            return _repository.ListaMateriais(idunidade);
        }

        public ICollection<MovimentoEstoque> ListarMovimentoEstoque(int idunidade)
        {
            return _repository.ListarMovimentoEstoque(idunidade);
        }

        public ICollection<TipoMaterial> ListaTipoMateriais(int idunidade)
        {
            return _repository.ListaTipoMateriais(idunidade);
        }

        public Material ObterMaterialPorId(int idmaterial)
        {
            return _repository.ObterMaterialPorId(idmaterial);
        }

        public MovimentoEstoque ObterMovimentoEstoquePorId(int idmovimentoestoque)
        {
            return _repository.ObterMovimentoEstoquePorId(idmovimentoestoque);
        }

        public TipoMaterial ObterTipoMaterialPorId(int idtipomaterial)
        {
            return _repository.ObterTipoMaterialPorId(idtipomaterial);
        }

        public Material SalvarMaterial(Material material)
        {
            return _repository.SalvarMaterial(material);
        }

        public MovimentoEstoque SalvarMovimentoEstoque(MovimentoEstoque movimento)
        {
            return _repository.SalvarMovimentoEstoque(movimento);
        }

        public TipoMaterial SalvarTipoMaterial(TipoMaterial tipomaterial)
        {
            return _repository.SalvarTipoMaterial(tipomaterial);
        }
    }
}