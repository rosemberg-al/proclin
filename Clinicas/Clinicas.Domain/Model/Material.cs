using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinicas.Domain.Model
{
    public class Material
    {

        public int IdMaterial { get; private set; }
        public string Nome { get; private set; }
        public int EstoqueMinimo { get; private set; }
        public string Marca { get; private set; }
        public string Modelo { get; private set; }
        public string Observacao { get; private set; }
        public string EstadoConservacao { get; private set; }
        public string Identificador { get; private set; }
        public decimal ValorVenda { get; private set; }
        public decimal ValorCompra { get; private set; }
        public string Situacao { get; private set; }
        public int EstoqueAtual { get; private set; }

        public int IdTipoMaterial { get; private set; }
        public virtual TipoMaterial TipoMaterial { get; private set; }

        public int IdUnidadeAtendimento { get; private set; }
        public virtual UnidadeAtendimento UnidadeAtendimento { get; private set; }

        public virtual ICollection<MovimentoEstoque> MovimentoEstoque { get; private set; }

        public Material() {
            this.MovimentoEstoque = new List<MovimentoEstoque>();
        }

        public Material(string nome,int estoqueMinimo,string marca,string modelo,string observacao,
                string estadoConservacao,string identificador,decimal valorVenda, decimal valorCompra,int estoqueAtual,
                TipoMaterial tipoMaterial,UnidadeAtendimento unidadeAtendimento)
        {
            SetNome(nome);
            SetEstoqueMinimo(estoqueMinimo);
            SetMarca(marca);
            SetModelo(modelo);
            SetObservacao(observacao);
            SetEstadoConservacao(estadoConservacao);
            SetIdentificador(identificador);
            SetValorVenda(valorVenda);
            SetValorCompra(valorCompra);
            SetSituacao("Ativo");
            SetEstoqueAtual(estoqueAtual);
            SetTipoMaterial(tipoMaterial);
            SetUnidadeAtendimento(unidadeAtendimento);
        }

        public void SetSituacao(string situacao)
        {
            this.Situacao = situacao;
        }

        public void SetEstadoConservacao(string estadoConservacao)
        {
            this.EstadoConservacao = estadoConservacao;
        }

        public void SetIdentificador(string identificador)
        {
            this.Identificador = identificador;
        }

        public void SetValorVenda(decimal valorVenda)
        {
            this.ValorVenda = valorVenda;
        }

        public void SetValorCompra(decimal valorCompra)
        {
            this.ValorCompra = valorCompra;
        }

        public void SetEstoqueAtual(int estoqueAtual)
        {
            this.EstoqueAtual = estoqueAtual;
        }

        public void SetTipoMaterial(TipoMaterial tipoMaterial)
        {
            this.TipoMaterial = tipoMaterial;
        }

        public void SetUnidadeAtendimento(UnidadeAtendimento unidadeAtendimento)
        {
            this.UnidadeAtendimento = unidadeAtendimento;
        }

        public void SetNome(string nome)
        {
            if (!string.IsNullOrEmpty(nome))
                this.Nome = nome;
            else
                throw new Exception("Campo Nome é Obrigatório");
        }

        public void SetEstoqueMinimo(int estoqueMinimo)
        {
            this.EstoqueMinimo = estoqueMinimo;
        }

        public void SetMarca(string marca)
        {
            this.Marca = marca;
        }

        public void SetModelo(string modelo)
        {
            this.Modelo = modelo;
        }

        public void SetObservacao(string observacao)
        {
            this.Observacao = observacao;
        }

        public void GerarMovimentoEstoque(int qtd, string tipo, UnidadeAtendimento unidade)
        {
            if (qtd > this.EstoqueAtual)
            {
                throw new Exception("A Quantidade informada e maior que o estoque atual");
            }

            if (tipo == "Saida")
            {
                this.EstoqueAtual = this.EstoqueAtual - qtd;
            }
            else if (tipo == "Entrada")
            {
                this.EstoqueAtual = this.EstoqueAtual + qtd;
            }

            // gera movimento de estoque 
            MovimentoEstoque.Add(new Model.MovimentoEstoque(DateTime.Now, tipo, qtd, unidade));
        }
    }
}