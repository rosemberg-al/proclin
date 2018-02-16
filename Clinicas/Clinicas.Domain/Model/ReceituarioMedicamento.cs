using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinicas.Domain.Model
{
    public class ReceituarioMedicamento
    {
        public int IdReceituario { get; private set; }
        public int IdMedicamento { get; private set; }
        public string Nome { get; private set; }
        public string Posologia { get; private set; }
        public int Quantidade { get; private set; }

        private ReceituarioMedicamento() { }

        public ReceituarioMedicamento(int idReceituario, int idMedicamento, string nome, string posologia, int qtde) {
            SetIdReceituario(idReceituario); ;
            SetIdMedicamento(idMedicamento);
            SetNome(nome);
            SetPosologia(posologia);
            SetQuantidade(qtde);
        }

        public void SetQuantidade(int qtde)
        {
            Quantidade = qtde;
        }

        public void SetPosologia(string posologia)
        {
            Posologia = posologia;
        }

        public void SetNome(string nome)
        {
            Nome = nome;
        }

        public void SetIdMedicamento(int idMedicamento)
        {
            IdMedicamento = idMedicamento;
        }

        public void SetIdReceituario(int idReceituario)
        {
            IdReceituario = idReceituario;
        }
    }
}