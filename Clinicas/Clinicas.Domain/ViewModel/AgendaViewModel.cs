﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinicas.Domain.ViewModel
{
    public class AgendaViewModel
    {
        public int IdAgenda { get; set; }
        public DateTime Data { get; set; }
        public TimeSpan Hora { get; set; }
        public string Situacao { get; set; }
        public string UnidadeAtendimento { get; set; }
        public int? IdPaciente { get; set; }
        public string NmPaciente { get; set; }
        public DateTime? DtNascimento { get; set; }
        public string Mae { get; set; }
        public string CPF { get; set; }
        public string Telefone1 { get; set; }
        public string Telefone2 { get; set; }
        public string Email { get; set; }
        public string RG { get; set; }
        public string Endereco { get; set; }
        public string Sexo { get; set; }
        public string NmProcedimento { get; set; }
        public decimal? Valor { get; set; }
        public decimal? ValorProfissional { get; set; }
        public string SalaEspera { get; set; }
        public string NmProfissionalSaude { get; set; }
        public string Tipo { get; set; }
        public string NmConvenio { get; set; }
        public string TipoAtendimento { get; set; }
        public byte[] Foto { get; set; }
    }
}