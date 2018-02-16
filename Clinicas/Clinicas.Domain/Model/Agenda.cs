using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinicas.Domain.Model
{
    public class Agenda
    {
        public int IdAgenda { get; private set; }

        public int? IdPaciente { get; set; }
        public virtual Paciente Paciente { get; set; }

        public int? IdTipoAtendimento { get; private set; }
        public TipoAtendimento TipoAtendimento { get; private set; }

        public DateTime Data { get; private set; }
        public TimeSpan Hora { get; private set; }
        public string Situacao { get; private set; }
        public string Observacao { get; private set; }
        public string SalaEspera { get; private set; }
        public string Avulsa { get; private set; }


        public int? IdProcedimento { get; private set; }
        public Procedimento Procedimento { get; private set; }

        public int IdFuncionario { get; private set; }
        public Funcionario ProfissionalSaude { get; private set; }

        public decimal? Valor { get; private set; }
        public decimal? ValorProfissional { get; private set; }
        public string Tipo { get; private set; }

        public int? IdConvenio { get; private set; }
        public Convenio Convenio { get; private set; }

        public int? IdEspecialidade { get; private set; }
        public Especialidade Especialidade { get; private set; }

        public string Solicitante { get; private set; }

        public int IdClinica { get; private set; }
        public Clinica Clinica { get; private set; }

        public int IdUnidadeAtendimento { get; private set; }
        public UnidadeAtendimento UnidadeAtendimento { get; private set; }

        public int? IdConsultorio { get; private set; }
        public Consultorio Consultorio { get; private set; }

        public string ConvocarPaciente { get; private set; }
        public DateTime? DataConvocacaoPaciente { get; private set; }

        #region [Auditoria]
        public int? IdUsuarioInclusao { get; private set; }
        public Usuario UsuarioInclusao { get; private set; }
        public DateTime DataInclusao { get; set; }

        public int? IdUsuarioMarcado { get; private set; }
        public Usuario UsuarioMarcado { get; private set; }
        public DateTime? DataMarcado { get; set; }

        public int? IdUsuarioCancelado { get; private set; }
        public Usuario UsuarioCancelado { get; private set; }
        public DateTime? DataCancelado { get; private set; }

        public int? IdUsuarioRealizado { get; private set; }
        public Usuario UsuarioRealizado { get; private set; }
        public DateTime? DataRealizado { get; private set; }
        #endregion

        public Agenda() { }

        public Agenda(DateTime data, TimeSpan hora, Funcionario profissional, Clinica clinica, Usuario usuarioInclusao, UnidadeAtendimento unidade)
        {

            #region validação
            if (profissional == null)
                throw new Exception("O Campo Profissional de Saúde é Obrigatório");

            if (usuarioInclusao == null)
                throw new Exception("Usuário não encontrado!");

            if (data == null)
                throw new Exception("O Campo Data é Obrigatório! ");

            if (hora == null)
                throw new Exception(" O Campo Hora é Obrigatório! ");

            if (clinica == null)
                throw new Exception("Não foi possivel recuperar dados da Clínica! ");
            #endregion

            SetData(data);
            SetHora(hora);
            SetProfissionalSaude(profissional);
            SetClinica(clinica);
            SetAvulsa("Nao");
            SetSalaEspera("Nao");
            SetUnidadeAtendimento(unidade);

            this.Situacao = "Aguardando";
            this.UsuarioInclusao = usuarioInclusao;
            this.DataInclusao = DateTime.Now;
        }

        public Agenda(DateTime data, TimeSpan hora, Funcionario profissional, Clinica clinica, Usuario usuarioInclusao)
        {

            #region validação
            if (profissional == null)
                throw new Exception("O Campo Profissional de Saúde é Obrigatório");

            if (usuarioInclusao == null)
                throw new Exception("Usuário não encontrado!");

            if (data == null)
                throw new Exception("O Campo Data é Obrigatório! ");

            if (hora == null)
                throw new Exception(" O Campo Hora é Obrigatório! ");

            if (clinica == null)
                throw new Exception("Não foi possivel recuperar dados da Clínica! ");
            #endregion

            SetData(data);
            SetHora(hora);
            SetProfissionalSaude(profissional);
            SetClinica(clinica);
            SetAvulsa("Nao");
            SetSalaEspera("Nao");

            this.Situacao = "Aguardando";
            this.UsuarioInclusao = usuarioInclusao;
            this.DataInclusao = DateTime.Now;
        }

        public void SetClinica(Clinica clinica)
        {
            if (clinica != null)
                Clinica = clinica;
        }

        public void SetUnidadeAtendimento(UnidadeAtendimento unidade)
        {
            if (unidade == null)
                throw new Exception("A unidadde de atendimento é obrigatória.");

                UnidadeAtendimento = unidade;
        }

        public void SetUsuarioMarcado(Usuario usuario)
        {
            if (usuario != null)
                UsuarioMarcado = usuario;
        }

        public void SetSituacao(string situacao)
        {
            if (!String.IsNullOrEmpty(situacao))
                Situacao = situacao;
        }

        public void SetPaciente(Paciente paciente)
        {
            if (paciente != null)
                Paciente = paciente;
        }

        public void Aguardando(DateTime data,
            TimeSpan hora,
            Usuario usuarioInclusao,
            Funcionario profissionalsaude,
            Clinica clinica,
            string observacao)
        {
            SetData(data);
            SetHora(hora);
            SetObservacao(observacao);
            SetAvulsa("Nao");
            SetSalaEspera("Nao");
            SetProfissionalSaude(profissionalsaude);

            this.Situacao = "Aguardando";
            this.UsuarioInclusao = usuarioInclusao;
            this.DataInclusao = DateTime.Now;
            this.Clinica = clinica;

            #region validação
            if (this.ProfissionalSaude == null)
                throw new Exception("O Campo Profissional de Saúde é Obrigatório");

            if (this.UsuarioInclusao == null)
                throw new Exception("Usuário não encontrado");

            if (this.Data == null)
                throw new Exception("O Campo Data é Obrigatório ");

            if (this.Hora == null)
                throw new Exception(" O Campo Hora é Obrigatório ");

            if (this.Clinica == null)
                throw new Exception("O Campo Clínica é Obrigatório ");
            #endregion

        }

        public void SetConvocarPaciente()
        {
            this.ConvocarPaciente = "Sim";
            this.DataConvocacaoPaciente = DateTime.Now;
        }


        public void Marcado(Usuario usuarioMarcado, Paciente paciente,
            Funcionario profissionaldesaude,
            Especialidade especialidade,
            Procedimento procedimento,
            Convenio convenio,
            string tipo,
            string Observacoes,
            string Solicitante,
            decimal valor,
            decimal valorProfissional,
            TipoAtendimento tipoAtendimento
            )
        {
            this.Situacao = "Marcado";
            this.UsuarioMarcado = usuarioMarcado;
            this.DataMarcado = DateTime.Now;
            this.Paciente = paciente;
            this.SetProfissionalSaude(profissionaldesaude);
            this.SetEspecialidade(especialidade);
            this.SetProcedimento(procedimento);
            this.SetValor(valor);
            this.SetTipoAgendamento(tipo);
            this.SetValorProfissional(valorProfissional);
            this.SetTipoAtendimento(tipoAtendimento);
            this.SetObservacao(Observacoes);
            this.SetConvenio(convenio);
            this.SetSalaEspera("Nao");


            // se tipo convênio
            if ((this.Tipo == "C") && (this.Convenio == null))
                throw new Exception("O Campo Convênio é Obrigatório ");

            if (this.Paciente == null)
                throw new Exception("O Campo Paciente é Obrigatório ");

            if (this.Especialidade == null)
                throw new Exception("O Campo Especialidade é Obrigatório ");

            if (this.Procedimento == null)
                throw new Exception("O Campo Procedimento é Obrigatório ");

            if (this.ProfissionalSaude == null)
                throw new Exception("O Campo Profissional de Saúde é Obrigatório ");
        }

        public void ConfirmarAtendimento(Usuario usuario)
        {
            this.UsuarioRealizado = usuario;
            this.DataRealizado = DateTime.Now;
            this.Situacao = "Realizado";
            this.SalaEspera = "Nao";


            if (this.UsuarioRealizado == null)
            {
                throw new Exception("Usuário não identificado");
            }
        }

        public void CancelarAtendimento(Usuario usuario)
        {
            this.UsuarioCancelado = usuario;
            this.DataCancelado = DateTime.Now;
            this.Situacao = "Cancelado";
            this.SalaEspera = "Nao";

            if (this.UsuarioCancelado == null)
            {
                throw new Exception("Usuário não identificado");
            }
        }


        #region [SETS]
        public void SetValorProfissional(decimal valorProfissional)
        {
            this.ValorProfissional = valorProfissional;
        }

        public void SetTipoAgendamento(string tipo)
        {
            this.Tipo = tipo;
        }

        public void SetValor(decimal valor)
        {
            this.Valor = valor;
        }

        public void SetEspecialidade(Especialidade especialidade)
        {
            this.Especialidade = especialidade;
        }

        public void SetProfissionalSaude(Funcionario profissionaldesaude)
        {
            this.ProfissionalSaude = profissionaldesaude;
        }

        public void SetTipoAtendimento(TipoAtendimento tipoAtendimento)
        {
            this.TipoAtendimento = tipoAtendimento;
        }

        public void SetProcedimento(Procedimento procedimento)
        {
            this.Procedimento = procedimento;
        }

        public void SetAvulsa(string avulsa)
        {
            this.Avulsa = avulsa;
        }

        public void SetSalaEspera(string salaEspera)
        {
            this.SalaEspera = salaEspera;
        }

        public void SetObservacao(string observacao)
        {
            this.Observacao = observacao;
        }

        public void SetHora(TimeSpan hora)
        {
            this.Hora = hora;
        }

        public void SetData(DateTime data)
        {
            this.Data = data;
        }

        public void SetConvenio(Convenio convenio)
        {
            this.Convenio = convenio;
        }

        #endregion
    }
}