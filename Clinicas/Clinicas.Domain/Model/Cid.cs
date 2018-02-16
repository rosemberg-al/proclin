using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinicas.Domain.Model
{
    public class Cid
    {
        public string Codigo { get; private set; }
        public string Descricao { get; private set; }
        public string Agravo { get; private set; }
        public string SexoOcorrencia { get; private set; }
        public string Estadio { get; private set; }
        public string CamposIrradiados { get; private set; }

        private Cid() { }

        public Cid(string codigo, string descricao, string agravo, string sexo, string estadio, string campo)
        {
            SetCodigo(codigo);
            SetDescricao(descricao);
            SetAgravo(agravo);
            SetSexoOcorrencia(sexo);
            SetEstadio(estadio);
            SetCamposIrradiados(campo);

        }

        public void SetCamposIrradiados(string campo)
        {
            if (!string.IsNullOrEmpty(campo))
                CamposIrradiados = campo;
        }

        public void SetEstadio(string estadio)
        {
            if (!string.IsNullOrEmpty(estadio))
                Estadio = estadio;
        }

        public void SetSexoOcorrencia(string sexo)
        {
            if (!string.IsNullOrEmpty(sexo))
                SexoOcorrencia = sexo;
        }

        public void SetAgravo(string agravo)
        {
            if (!string.IsNullOrEmpty(agravo))
                Agravo = agravo;
        }

        public void SetDescricao(string descricao)
        {
            if (!string.IsNullOrEmpty(descricao))
                Descricao = descricao;
        }

        public void SetCodigo(string codigo)
        {
            if (!string.IsNullOrEmpty(codigo))
                Codigo = codigo;
        }
    }
}