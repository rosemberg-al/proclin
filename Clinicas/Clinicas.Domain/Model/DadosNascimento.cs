using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinicas.Domain.Model
{
    public class DadosNascimento
    {
        public int IdDadosNascimento { get; private set; }
        public Decimal PesoNascimento { get; private set; }
        public Decimal AlturaNascimento { get; private set; }
        public Decimal PerimetroCraniano { get; private set; }
        public Decimal PerimetroAbdominal { get; private set; }
        public string IdadeGestacional { get; private set; }
        public int IdHospital { get; private set; }
        public DateTime DataAlta { get; private set; }
        public Decimal PesoAlta { get; private set; }
        public string TipoParto { get; private set; }
        public string TipoSanguineoRN { get; private set; }
        public DateTime DataNascimento { get; private set; }
        public DateTime HoraNascimento { get; private set; }
        public string AssistiuRecemNascidoRN { get; private set; }
        public string AleitamentoMaternoPrimeiraHoraVida { get; private set; }
        public string TipoSanguineoMae { get; private set; }
        public string TestePezinho { get; private set; }
        public DateTime DataTestePezinho { get; private set; }
        public string TesteReflexoVermelho { get; private set; }
        public string Condutas { get; private set; }

        public virtual Hospital Hospital { get; private set; }

        private DadosNascimento() { }
        public DadosNascimento(decimal pesoNascimento, decimal alturaNascimento, decimal perimetroCraniano, decimal perimetroAbdominal,
            string idadeGestacional, Hospital hospital, DateTime dataAlta, decimal peso, string tipoParto, string tipoSanguinioRN, DateTime dataNascimento,
            DateTime horaNascimento, string assistiuRecemNascidoRN, string aleitamentoMaternoPrimeiraHoraVida, string tipoSanguineoMae, string testePezinho,
            DateTime dataTestePezinho, string testeReflexoVermelho, string condutas)
        {
            SetTesteReflexoVermelho(testeReflexoVermelho);
            SetCondutas(condutas);
            SetTestePezinho(testePezinho);
            SetTipoSanguineoMae(tipoSanguineoMae);
            SetAleitamentoMaternoPrimeiraHoraVida(aleitamentoMaternoPrimeiraHoraVida);
            SetAssistiuRecemNascidoRN(assistiuRecemNascidoRN);
            SetTipoSanguineoRN(tipoSanguinioRN);
            SetTipoParto(tipoParto);
            SetIdadeGestacional(idadeGestacional);
            SetDataAlta(dataAlta);
            SetDataNascimento(dataNascimento);
            SetDataTestePezinhoo(dataTestePezinho);
            SetHoraNascimento(horaNascimento);
            SetHospital(hospital);
            SetPesoAlta(peso);
            SetPerimetroAbdominal(perimetroAbdominal);
            SetAlturaNascimento(alturaNascimento);
            SetPerimetroCraniano(perimetroCraniano);
            SetPesoNascimento(pesoNascimento);
        }
        public void SetTesteReflexoVermelho(string testeReflexoVermelho)
        {
            if (!String.IsNullOrEmpty(testeReflexoVermelho))
                TesteReflexoVermelho = testeReflexoVermelho;
        }
        //public void SetPaciente(paciente paciente)
        //{
        //    if (paciente != null)
        //        Paciente = paciente;
        //}
        public void SetCondutas(string condutas)
        {
            if (!String.IsNullOrEmpty(condutas))
                Condutas = condutas;
        }
        public void SetTestePezinho(string testePezinho)
        {
            if (!String.IsNullOrEmpty(testePezinho))
                TestePezinho = testePezinho;
        }
        public void SetTipoSanguineoMae(string tipoSanguineoMae)
        {
            if (!String.IsNullOrEmpty(tipoSanguineoMae))
                TipoSanguineoMae = tipoSanguineoMae;
        }
        public void SetAleitamentoMaternoPrimeiraHoraVida(string aleitamentoMaternoPrimeiraHoraVida)
        {
            if (!String.IsNullOrEmpty(aleitamentoMaternoPrimeiraHoraVida))
                AleitamentoMaternoPrimeiraHoraVida = aleitamentoMaternoPrimeiraHoraVida;
        }
        public void SetAssistiuRecemNascidoRN(string assistiuRecemNascidoRN)
        {
            if (!String.IsNullOrEmpty(assistiuRecemNascidoRN))
                AssistiuRecemNascidoRN = assistiuRecemNascidoRN;
        }
        public void SetTipoSanguineoRN(string tipoSanguinioRN)
        {
            if (!String.IsNullOrEmpty(tipoSanguinioRN))
                TipoSanguineoRN = tipoSanguinioRN;
        }
        public void SetTipoParto(string tipoParto)
        {
            if (!String.IsNullOrEmpty(tipoParto))
                TipoParto = tipoParto;
        }
        public void SetIdadeGestacional(string idadeGestacional)
        {
            if (!String.IsNullOrEmpty(idadeGestacional))
                IdadeGestacional = idadeGestacional;
        }
        public void SetDataAlta(DateTime dataAlta)
        {
            if (dataAlta != null)
                DataAlta = dataAlta;
        }
        public void SetDataNascimento(DateTime dataNascimento)
        {
            if (dataNascimento != null)
                DataNascimento = dataNascimento;
        }
        public void SetDataTestePezinhoo(DateTime dataTestePezinho)
        {
            if (dataTestePezinho != null)
                DataTestePezinho = dataTestePezinho;
        }
        public void SetHoraNascimento(DateTime horaNascimento)
        {
            if (horaNascimento != null)
                HoraNascimento = horaNascimento;
        }
        public void SetHospital(Hospital hospital)
        {
            if (hospital != null)
                Hospital = hospital;
        }
        public void SetPesoAlta(decimal peso)
        {
            if (peso > 0)
                PesoAlta = peso;
        }
        public void SetPerimetroAbdominal(decimal perimetroAbdominal)
        {
            if (perimetroAbdominal > 0)
                PerimetroAbdominal = perimetroAbdominal;
        }
        public void SetAlturaNascimento(decimal alturaNascimento)
        {
            if (alturaNascimento > 0)
                AlturaNascimento = alturaNascimento;
        }
        public void SetPerimetroCraniano(decimal perimetroCraniano)
        {
            if (perimetroCraniano > 0)
                PerimetroCraniano = perimetroCraniano;
        }

        public void SetPesoNascimento(decimal pesoNascimento)
        {
            if (pesoNascimento > 0)
                PesoNascimento = pesoNascimento;
        }
    }
}