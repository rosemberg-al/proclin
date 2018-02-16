using Clinicas.Application.Services.Interfaces;
using Clinicas.Domain.Model;
using Clinicas.Infrastructure.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Clinicas.Domain.DTO;
using Clinicas.Domain.ViewModel;
using Clinicas.Domain.Tiss;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.IO;
using System.Linq.Expressions;

namespace Clinicas.Application.Services
{
    public class GuiaService : IGuiaService
    {
        private readonly IGuiaRepository _repository;
        private readonly ICadastroRepository _cadrepository;
        BaseFont f_default = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
        BaseFont f_bold = BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);

        public GuiaService(IGuiaRepository repository, ICadastroRepository cadrepository)
        {
            _repository = repository;
            _cadrepository = cadrepository;
        }

        public void Excluir(Guia guia)
        {
            _repository.Excluir(guia);
        }

        public List<Guia> ListarGuias()
        {
            return _repository.ListarGuias();
        }

        public List<Guia> ListarGuiasPorTipo(string tipoGuia, int idClinica)
        {
            return _repository.ListarGuiasPorTipo(tipoGuia, idClinica);
        }

        public List<Guia> ListarGuiasBuscaAvancada(BuscaGuiaViewModel model)
        {
            return _repository.ListarGuiasBuscaAvancada(model);
        }

        public List<Guia> ListarGuiasPorConvenio(int idConvenio, int idClinica, string tipoGuia)
        {
            return _repository.ListarGuiasPorConvenio(idConvenio, idClinica, tipoGuia);
        }

        public Guia ObterGuiaPorId(int idguia)
        {
            return _repository.ObterGuiaPorId(idguia);
        }

        public void Salvar(Guia guia)
        {
            _repository.Salvar(guia);
        }

        public class procedimentoteste
        {
            public string Tabela { get; set; }
            public string Codigo { get; set; }
            public string Descricao { get; set; }
            public string QtdeSolicitada { get; set; }
            public string QtdeAutorizada { get; set; }
        }

        public class procedimentoRealizadosteste
        {
            public string Data { get; set; }
            public string HoraInicail { get; set; }
            public string HoraFinal { get; set; }
            public string Tabela { get; set; }
            public string Codigo { get; set; }
            public string Descricao { get; set; }
            public string Qtde { get; set; }
            public string Via { get; set; }
            public string Tecnico { get; set; }
            public string ReducaoAcrescimo { get; set; }
            public string ValorUnitario { get; set; }
            public string ValorTotal { get; set; }

        }

        public byte[] GetGuiaSpSadtPdf(Int32 idGuia)
        {
            var guia = _repository.ObterGuiaPorId(idGuia);

            if (guia == null)
                throw new Exception("Não foi possível recuperar os dados da guia!");


            var convenio = _cadrepository.ObterConvenioById(guia.IdConvenio);

            if (convenio == null)
                throw new Exception("Não foi possível recuperar os dados do convênio.");


            float hSum = 0;
            Document pdf = new Document(PageSize.LETTER, 40f, 20f, 0f, 0f);
            MemoryStream stream = new MemoryStream();

            pdf.SetPageSize(PageSize.A4.Rotate());

            try
            {
                PdfWriter writer = PdfWriter.GetInstance(pdf, stream);
                writer.CloseStream = false;
                pdf.Open();
                PdfContentByte cb = writer.DirectContent;
                float topPos = pdf.PageSize.Height - pdf.TopMargin;
                float leftPos = pdf.LeftMargin;

                float caixaDados = 22f;

                float caixaLogoW = 120f;
                #region[cabeçalho PDF]

                if (convenio.LogoGuia.Count() > 0)
                {
                    //iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(System.Web.HttpContext.Current.Server.MapPath("~/imagens/logo_promed.jpg"));
                    iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(byteArrayToImage(convenio.LogoGuia), System.Drawing.Imaging.ImageFormat.Jpeg);
                    logo.ScalePercent(60f);
                    logo.SetAbsolutePosition(leftPos + 10f, topPos - hSum - 40f);
                    pdf.Add(logo);
                }

                //Cabeçalho - Header
                float caixaHeaderH = 20f;

                cb.BeginText();
                cb.SetColorFill(BaseColor.BLACK);
                cb.SetFontAndSize(f_bold, 11);
                cb.SetTextMatrix(leftPos + caixaLogoW + 10f, topPos - hSum - 20f);
                cb.ShowText("GUIA  DE SERVIÇO PROFISSIONAL / SERVIÇO AUXILIAR DE DIAGNÓSTICO E TERAPIA - SP/SADT");
                cb.EndText();
                hSum += caixaHeaderH;

                cb.BeginText();
                cb.SetColorFill(BaseColor.BLACK);
                cb.SetFontAndSize(f_bold, 10);
                cb.SetTextMatrix(leftPos + caixaLogoW + 540f, topPos - hSum - 5f);
                cb.ShowText("2 - Nº");
                cb.EndText();
                hSum += caixaHeaderH;

                //Soma de linha cabeçalho
                hSum += 10f;
                #endregion



                hSum += 10f;
                //registro ans
                cb.Rectangle(pdf.LeftMargin, topPos - hSum - 10f, 120f, caixaDados);
                cb.SetColorFill(BaseColor.WHITE);
                cb.FillStroke();
                cb.ResetCMYKColorFill();
                cb.BeginText();
                cb.SetFontAndSize(f_default, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 2f, topPos - 56f);
                cb.ShowText("1 - Registro ANS");
                cb.SetFontAndSize(f_bold, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 2f, topPos - hSum - 8f);
                cb.ShowText("545465456");
                cb.EndText();
                //numero guia principal
                cb.Rectangle(pdf.LeftMargin + 124f, topPos - hSum - 10f, 200f, caixaDados);
                cb.SetColorFill(BaseColor.WHITE);
                cb.FillStroke();
                cb.ResetCMYKColorFill();
                cb.BeginText();
                cb.SetFontAndSize(f_default, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 126f, topPos - 56f);
                cb.ShowText("3 - Nº Guia Principal");
                cb.SetFontAndSize(f_bold, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 126f, topPos - hSum - 8f);
                cb.ShowText(DateTime.Now.ToShortDateString());
                cb.EndText();
                //data da autorização
                cb.Rectangle(pdf.LeftMargin + 328f, topPos - hSum - 10f, 100f, caixaDados);
                cb.SetColorFill(BaseColor.WHITE);
                cb.FillStroke();
                cb.ResetCMYKColorFill();
                cb.BeginText();
                cb.SetFontAndSize(f_default, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 330f, topPos - 56f);
                cb.ShowText("4 - Data da Autorização");
                cb.SetFontAndSize(f_bold, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 330f, topPos - hSum - 8f);
                cb.ShowText(DateTime.Now.ToShortDateString());
                cb.EndText();
                //senha
                cb.Rectangle(pdf.LeftMargin + 432f, topPos - hSum - 10f, 70f, caixaDados);
                cb.SetColorFill(BaseColor.WHITE);
                cb.FillStroke();
                cb.ResetCMYKColorFill();
                cb.BeginText();
                cb.SetFontAndSize(f_default, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 434f, topPos - 56f);
                cb.ShowText("5 - Senha");
                cb.SetFontAndSize(f_bold, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 434f, topPos - hSum - 8f);
                cb.ShowText(DateTime.Now.ToShortDateString());
                cb.EndText();

                //data validade da senha
                cb.Rectangle(pdf.LeftMargin + 506f, topPos - hSum - 10f, 120f, caixaDados);
                cb.SetColorFill(BaseColor.WHITE);
                cb.FillStroke();
                cb.ResetCMYKColorFill();
                cb.BeginText();
                cb.SetFontAndSize(f_default, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 508f, topPos - 56f);
                cb.ShowText("6 - Data Validade da Senha");
                cb.SetFontAndSize(f_bold, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 508f, topPos - hSum - 8f);
                cb.ShowText(DateTime.Now.ToShortDateString());
                cb.EndText();

                //data emissão da guia
                cb.Rectangle(pdf.LeftMargin + 630f, topPos - hSum - 10f, 100f, caixaDados);
                cb.SetColorFill(BaseColor.WHITE);
                cb.FillStroke();
                cb.ResetCMYKColorFill();
                cb.BeginText();
                cb.SetFontAndSize(f_default, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 634f, topPos - 56f);
                cb.ShowText("7 - Data Emissão da Guia");
                cb.SetFontAndSize(f_bold, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 634f, topPos - hSum - 8f);
                cb.ShowText(DateTime.Now.ToShortDateString());
                cb.EndText();

                //cabeçalho dados do beneficiario
                cb.Rectangle(pdf.LeftMargin, topPos - hSum - 25f, 770f, 12f);
                cb.SetColorFill(BaseColor.LIGHT_GRAY);
                cb.FillStroke();
                cb.ResetCMYKColorFill();

                cb.BeginText();
                cb.SetColorFill(BaseColor.BLACK);
                cb.SetFontAndSize(f_bold, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 10f, topPos - hSum - 22f);
                cb.ShowText("Dados do Beneficiário");
                cb.EndText();
                hSum += caixaHeaderH;

                hSum += 10f;

                //tamanho da pagina 770f
                //numero da carteira
                cb.Rectangle(pdf.LeftMargin, topPos - hSum - 20f, 120f, caixaDados);
                cb.SetColorFill(BaseColor.WHITE);
                cb.FillStroke();
                cb.ResetCMYKColorFill();
                cb.BeginText();
                cb.SetFontAndSize(f_default, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 2f, topPos - hSum - 5f);
                cb.ShowText("8 - Número da Carteira");
                cb.SetFontAndSize(f_bold, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 2f, topPos - hSum - 18f);
                cb.ShowText("545465456");
                cb.EndText();
                //plano
                cb.Rectangle(pdf.LeftMargin + 124f, topPos - hSum - 20f, 140f, caixaDados);
                cb.SetColorFill(BaseColor.WHITE);
                cb.FillStroke();
                cb.ResetCMYKColorFill();
                cb.BeginText();
                cb.SetFontAndSize(f_default, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 126f, topPos - hSum - 5f);
                cb.ShowText("9 - Plano");
                cb.SetFontAndSize(f_bold, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 126f, topPos - hSum - 18f);
                cb.ShowText("PROMED ENFERMARIA GOLD");
                cb.EndText();
                //validade da certeira
                cb.Rectangle(pdf.LeftMargin + 268f, topPos - hSum - 20f, 102f, caixaDados);
                cb.SetColorFill(BaseColor.WHITE);
                cb.FillStroke();
                cb.ResetCMYKColorFill();
                cb.BeginText();
                cb.SetFontAndSize(f_default, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 270f, topPos - hSum - 5f);
                cb.ShowText("10 - Validade da Carteira");
                cb.SetFontAndSize(f_bold, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 270f, topPos - hSum - 18f);
                cb.ShowText(DateTime.Now.ToShortDateString());
                cb.EndText();
                //nome 
                cb.Rectangle(pdf.LeftMargin + 374f, topPos - hSum - 20f, 236f, caixaDados);
                cb.SetColorFill(BaseColor.WHITE);
                cb.FillStroke();
                cb.ResetCMYKColorFill();
                cb.BeginText();
                cb.SetFontAndSize(f_default, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 376f, topPos - hSum - 5f);
                cb.ShowText("11 - Nome");
                cb.SetFontAndSize(f_bold, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 376f, topPos - hSum - 18f);
                cb.ShowText("DINO DA SILVA SAURO");
                cb.EndText();
                //Número do Cartão Nacional de Saúde
                cb.Rectangle(pdf.LeftMargin + 614f, topPos - hSum - 20f, 156f, caixaDados);
                cb.SetColorFill(BaseColor.WHITE);
                cb.FillStroke();
                cb.ResetCMYKColorFill();
                cb.BeginText();
                cb.SetFontAndSize(f_default, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 616f, topPos - hSum - 5f);
                cb.ShowText("12 - Número do Cartão Nacional de Saúde");
                cb.SetFontAndSize(f_bold, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 616f, topPos - hSum - 18f);
                cb.ShowText("123456789000000");
                cb.EndText();

                //dados do contratado solicitante
                cb.Rectangle(pdf.LeftMargin, topPos - hSum - 36f, 770f, 12f);
                cb.SetColorFill(BaseColor.LIGHT_GRAY);
                cb.FillStroke();
                cb.ResetCMYKColorFill();

                cb.BeginText();
                cb.SetColorFill(BaseColor.BLACK);
                cb.SetFontAndSize(f_bold, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 10f, topPos - hSum - 32f);
                cb.ShowText("Dados do Contratado Solicitante");
                cb.EndText();
                hSum += caixaHeaderH;

                hSum += 10f;
                //codigo na operadora
                cb.Rectangle(pdf.LeftMargin, topPos - hSum - 31f, 250f, caixaDados);
                cb.SetColorFill(BaseColor.WHITE);
                cb.FillStroke();
                cb.ResetCMYKColorFill();
                cb.BeginText();
                cb.SetFontAndSize(f_default, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 2f, topPos - hSum - 17f);
                cb.ShowText("13 - Código na Operadora / CNPJ / CPF");
                cb.SetFontAndSize(f_bold, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 2f, topPos - hSum - 28f);
                cb.ShowText("56.456.0001/25");
                cb.EndText();
                //nome contartado
                cb.Rectangle(pdf.LeftMargin + 254f, topPos - hSum - 31f, 350f, caixaDados);
                cb.SetColorFill(BaseColor.WHITE);
                cb.FillStroke();
                cb.ResetCMYKColorFill();
                cb.BeginText();
                cb.SetFontAndSize(f_default, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 256f, topPos - hSum - 17f);
                cb.ShowText("14 - Nome do Contratado");
                cb.SetFontAndSize(f_bold, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 256f, topPos - hSum - 28f);
                cb.ShowText("HOSTPITAL BELO HORIZONTE");
                cb.EndText();
                //codigo CNES
                cb.Rectangle(pdf.LeftMargin + 608f, topPos - hSum - 31f, 120f, caixaDados);
                cb.SetColorFill(BaseColor.WHITE);
                cb.FillStroke();
                cb.ResetCMYKColorFill();
                cb.BeginText();
                cb.SetFontAndSize(f_default, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 610f, topPos - hSum - 17f);
                cb.ShowText("15 - Código CNES");
                cb.SetFontAndSize(f_bold, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 610f, topPos - hSum - 28f);
                cb.ShowText("123456789000000");
                cb.EndText();
                //nome do profissional solicitante
                cb.Rectangle(pdf.LeftMargin, topPos - hSum - 56f, 300f, caixaDados);
                cb.SetColorFill(BaseColor.WHITE);
                cb.FillStroke();
                cb.ResetCMYKColorFill();
                cb.BeginText();
                cb.SetFontAndSize(f_default, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 2f, topPos - hSum - 42f);
                cb.ShowText("16 - Nome do Profissonal Solicitante");
                cb.SetFontAndSize(f_bold, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 2f, topPos - hSum - 53f);
                cb.ShowText("JOAO ANTONIO SILVEIRA");
                cb.EndText();
                //conselho profissional
                cb.Rectangle(pdf.LeftMargin + 304f, topPos - hSum - 56f, 150f, caixaDados);
                cb.SetColorFill(BaseColor.WHITE);
                cb.FillStroke();
                cb.ResetCMYKColorFill();
                cb.BeginText();
                cb.SetFontAndSize(f_default, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 306f, topPos - hSum - 42f);
                cb.ShowText("17 - Conselho Profissional");
                cb.SetFontAndSize(f_bold, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 306f, topPos - hSum - 53f);
                cb.ShowText("CRM");
                cb.EndText();
                //numero do conselho
                cb.Rectangle(pdf.LeftMargin + 458f, topPos - hSum - 56f, 100f, caixaDados);
                cb.SetColorFill(BaseColor.WHITE);
                cb.FillStroke();
                cb.ResetCMYKColorFill();
                cb.BeginText();
                cb.SetFontAndSize(f_default, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 460f, topPos - hSum - 42f);
                cb.ShowText("18 - Número no Conselho");
                cb.SetFontAndSize(f_bold, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 460f, topPos - hSum - 53f);
                cb.ShowText("545465456");
                cb.EndText();
                //uf
                cb.Rectangle(pdf.LeftMargin + 562f, topPos - hSum - 56f, 70f, caixaDados);
                cb.SetColorFill(BaseColor.WHITE);
                cb.FillStroke();
                cb.ResetCMYKColorFill();
                cb.BeginText();
                cb.SetFontAndSize(f_default, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 564f, topPos - hSum - 42f);
                cb.ShowText("19 - UF");
                cb.SetFontAndSize(f_bold, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 564f, topPos - hSum - 53f);
                cb.ShowText("MG");
                cb.EndText();
                //código cbos
                cb.Rectangle(pdf.LeftMargin + 636f, topPos - hSum - 56f, 120f, caixaDados);
                cb.SetColorFill(BaseColor.WHITE);
                cb.FillStroke();
                cb.ResetCMYKColorFill();
                cb.BeginText();
                cb.SetFontAndSize(f_default, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 638f, topPos - hSum - 42f);
                cb.ShowText("20 - Código CBO S");
                cb.SetFontAndSize(f_bold, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 638f, topPos - hSum - 53f);
                cb.ShowText("545465456");
                cb.EndText();

                //Dados da Solicitação / Procedimentos e Exames Solicitados
                cb.Rectangle(pdf.LeftMargin, topPos - hSum - 72f, 770f, 12f);
                cb.SetColorFill(BaseColor.LIGHT_GRAY);
                cb.FillStroke();
                cb.ResetCMYKColorFill();

                cb.BeginText();
                cb.SetColorFill(BaseColor.BLACK);
                cb.SetFontAndSize(f_bold, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 10f, topPos - hSum - 68f);
                cb.ShowText("Dados da Solicitação / Procedimentos e Exames Solicitados");
                cb.EndText();
                hSum += caixaHeaderH;

                hSum += 10f;
                //DATA/HORA DA SOLICITAÇÃO
                cb.Rectangle(pdf.LeftMargin, topPos - hSum - 67f, 140f, caixaDados);
                cb.SetColorFill(BaseColor.WHITE);
                cb.FillStroke();
                cb.ResetCMYKColorFill();
                cb.BeginText();
                cb.SetFontAndSize(f_default, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 2f, topPos - hSum - 53f);
                cb.ShowText("21 - Data/Hora da Solicitação");
                cb.SetFontAndSize(f_bold, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 2f, topPos - hSum - 64f);
                cb.ShowText(DateTime.Now.ToString());
                cb.EndText();
                //caracter da solicitação
                cb.Rectangle(pdf.LeftMargin + 144f, topPos - hSum - 67f, 170f, caixaDados);
                cb.SetColorFill(BaseColor.WHITE);
                cb.FillStroke();
                cb.ResetCMYKColorFill();
                cb.BeginText();
                cb.SetFontAndSize(f_default, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 146f, topPos - hSum - 53f);
                cb.ShowText("22 - Carácter da Solicitação");
                cb.SetFontAndSize(f_bold, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 146f, topPos - hSum - 64f);
                cb.ShowText("[ U ] E-Eletiva  U-Urgência/Emergência");
                cb.EndText();
                //CID 10
                cb.Rectangle(pdf.LeftMargin + 318f, topPos - hSum - 67f, 70f, caixaDados);
                cb.SetColorFill(BaseColor.WHITE);
                cb.FillStroke();
                cb.ResetCMYKColorFill();
                cb.BeginText();
                cb.SetFontAndSize(f_default, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 320f, topPos - hSum - 53f);
                cb.ShowText("23 - CID 10");
                cb.SetFontAndSize(f_bold, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 320f, topPos - hSum - 64f);
                cb.ShowText("02565");
                cb.EndText();
                //indicação clinica
                cb.Rectangle(pdf.LeftMargin + 392f, topPos - hSum - 67f, 378f, caixaDados);
                cb.SetColorFill(BaseColor.WHITE);
                cb.FillStroke();
                cb.ResetCMYKColorFill();
                cb.BeginText();
                cb.SetFontAndSize(f_default, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 394f, topPos - hSum - 53f);
                cb.ShowText("24 - Indicação Clínica (obrigatório se pequena cirurgia, terapia, consulta de referência e alto custo)");
                cb.SetFontAndSize(f_bold, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 394f, topPos - hSum - 64f);
                cb.ShowText("Cirurgia ");
                cb.EndText();

                hSum += 10f;

                //tabela de procedimentos -----------------------------------------------------------------------------------------
                cb.Rectangle(pdf.LeftMargin, topPos - hSum - 130f, 770f, 70f);
                cb.SetColorFill(BaseColor.WHITE);
                cb.FillStroke();
                cb.ResetCMYKColorFill();

                PdfPTable table = new PdfPTable(5);
                table.SetWidths(new int[] { 60, 130, 380, 90, 100 });
                table.WidthPercentage = 98;
                table.SpacingBefore = 192f;


                BaseFont bf = BaseFont.CreateFont(
                        BaseFont.HELVETICA,
                        BaseFont.CP1252,
                        BaseFont.EMBEDDED);
                Font font = new Font(bf, 8);

                BaseFont bfBold = BaseFont.CreateFont(
                       BaseFont.HELVETICA_BOLD,
                       BaseFont.CP1252,
                       BaseFont.EMBEDDED);
                Font fontBold = new Font(bfBold, 8);


                PdfPCell cellTabela = new PdfPCell(new Phrase("25 - Tabela", font));
                PdfPCell cellCodigo = new PdfPCell(new Phrase("26 - Código do Procedimento", font));
                PdfPCell cellDescricao = new PdfPCell(new Phrase("27 - Descrição", font));
                PdfPCell cellQSolic = new PdfPCell(new Phrase("28 - Qtde. Solicitada", font));
                PdfPCell cellQAuto = new PdfPCell(new Phrase("29 - Qtde. Autorizada", font));

                cellTabela.Border = Rectangle.TOP_BORDER | Rectangle.BOTTOM_BORDER;
                cellTabela.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                cellTabela.Border = 0;
                table.AddCell(cellTabela);

                cellCodigo.Border = 0;
                cellCodigo.HorizontalAlignment = 0;
                table.AddCell(cellCodigo);

                cellDescricao.HorizontalAlignment = 0;
                cellDescricao.Border = 0;
                table.AddCell(cellDescricao);

                cellQSolic.HorizontalAlignment = 0;
                cellQSolic.Border = 0;
                table.AddCell(cellQSolic);

                cellQAuto.HorizontalAlignment = 0;
                cellQAuto.Border = 0;
                table.AddCell(cellQAuto);

                var lista = new List<procedimentoteste>();

                for (int i = 0; i < 5; i++)
                {
                    lista.Add(new procedimentoteste
                    {
                        Codigo = "10101012",
                        Descricao = "Consulta Eletiva",
                        QtdeAutorizada = "1",
                        QtdeSolicitada = "1",
                        Tabela = "099"
                    });
                }


                foreach (var item in lista)
                {
                    PdfPCell celulaTabela = new PdfPCell(new Phrase(item.Tabela, fontBold));
                    PdfPCell celulaCodigo = new PdfPCell(new Phrase(item.Codigo, fontBold));
                    PdfPCell celulaDescricao = new PdfPCell(new Phrase(item.Descricao, fontBold));
                    PdfPCell celulaQtdeSolic = new PdfPCell(new Phrase(item.QtdeSolicitada, fontBold));
                    PdfPCell celulaQtdeAutorizda = new PdfPCell(new Phrase(item.QtdeAutorizada, fontBold));
                    celulaTabela.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    celulaTabela.Border = 0;
                    table.AddCell(celulaTabela);

                    celulaCodigo.HorizontalAlignment = 0;
                    celulaCodigo.Border = 0;
                    table.AddCell(celulaCodigo);

                    celulaDescricao.HorizontalAlignment = 0;
                    celulaDescricao.Border = 0;
                    table.AddCell(celulaDescricao);


                    celulaQtdeSolic.HorizontalAlignment = 0;
                    celulaQtdeSolic.Border = 0;
                    table.AddCell(celulaQtdeSolic);

                    celulaQtdeAutorizda.HorizontalAlignment = 0;
                    celulaQtdeAutorizda.Border = 0;
                    table.AddCell(celulaQtdeAutorizda);
                }
                Paragraph paragraphTable2 = new Paragraph();
                paragraphTable2.SpacingBefore = 10f;
                paragraphTable2.Add(table);
                cb.PdfDocument.Add(paragraphTable2);

                //fim tabela de procedimentos -----------------------------------------------------------------------------------------

                //dados do contratado solicitante
                cb.Rectangle(pdf.LeftMargin, topPos - hSum - 145f, 770f, 12f);
                cb.SetColorFill(BaseColor.LIGHT_GRAY);
                cb.FillStroke();
                cb.ResetCMYKColorFill();

                cb.BeginText();
                cb.SetColorFill(BaseColor.BLACK);
                cb.SetFontAndSize(f_bold, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 10f, topPos - hSum - 142f);
                cb.ShowText("Dados do Contratado Executante");
                cb.EndText();
                hSum += caixaHeaderH;

                hSum += 10f;
                //codigo na operadora
                cb.Rectangle(pdf.LeftMargin, topPos - hSum - 140f, 130f, caixaDados);
                cb.SetColorFill(BaseColor.WHITE);
                cb.FillStroke();
                cb.ResetCMYKColorFill();
                cb.BeginText();
                cb.SetFontAndSize(f_default, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 2f, topPos - hSum - 126f);
                cb.ShowText("30-Cód. na Operadora/CNPJ/CPF");
                cb.SetFontAndSize(f_bold, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 2f, topPos - hSum - 138f);
                cb.ShowText("56.456.0001/25");
                cb.EndText();
                //nome contartado
                cb.Rectangle(pdf.LeftMargin + 134f, topPos - hSum - 140f, 150f, caixaDados);
                cb.SetColorFill(BaseColor.WHITE);
                cb.FillStroke();
                cb.ResetCMYKColorFill();
                cb.BeginText();
                cb.SetFontAndSize(f_default, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 136f, topPos - hSum - 126f);
                cb.ShowText("31 - Nome do Contratado");
                cb.SetFontAndSize(f_bold, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 136f, topPos - hSum - 138f);
                cb.ShowText("HOSTPITAL BELO HORIZONTE");
                cb.EndText();
                //tipo logradouro
                cb.Rectangle(pdf.LeftMargin + 288f, topPos - hSum - 140f, 30f, caixaDados);
                cb.SetColorFill(BaseColor.WHITE);
                cb.FillStroke();
                cb.ResetCMYKColorFill();
                cb.BeginText();
                cb.SetFontAndSize(f_default, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 290f, topPos - hSum - 126f);
                cb.ShowText("32-T.L.");
                cb.SetFontAndSize(f_bold, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 290f, topPos - hSum - 138f);
                cb.ShowText("Rua");
                cb.EndText();
                //Logradouro
                cb.Rectangle(pdf.LeftMargin + 322f, topPos - hSum - 140f, 175f, caixaDados);
                cb.SetColorFill(BaseColor.WHITE);
                cb.FillStroke();
                cb.ResetCMYKColorFill();
                cb.BeginText();
                cb.SetFontAndSize(f_default, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 324f, topPos - hSum - 126f);
                cb.ShowText("33-34-35-Logradouro - Número - Complemento");
                cb.SetFontAndSize(f_bold, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 324f, topPos - hSum - 138f);
                cb.ShowText("Rio de Janeiro, 189 - Loja A");
                cb.EndText();
                //Municipio
                cb.Rectangle(pdf.LeftMargin + 501f, topPos - hSum - 140f, 65f, caixaDados);
                cb.SetColorFill(BaseColor.WHITE);
                cb.FillStroke();
                cb.ResetCMYKColorFill();
                cb.BeginText();
                cb.SetFontAndSize(f_default, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 503f, topPos - hSum - 126f);
                cb.ShowText("36 - Município");
                cb.SetFontAndSize(f_bold, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 503f, topPos - hSum - 138f);
                cb.ShowText("Belo Horizonte");
                cb.EndText();
                //uf
                cb.Rectangle(pdf.LeftMargin + 570f, topPos - hSum - 140f, 30f, caixaDados);
                cb.SetColorFill(BaseColor.WHITE);
                cb.FillStroke();
                cb.ResetCMYKColorFill();
                cb.BeginText();
                cb.SetFontAndSize(f_default, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 572f, topPos - hSum - 126f);
                cb.ShowText("37- UF");
                cb.SetFontAndSize(f_bold, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 572f, topPos - hSum - 138f);
                cb.ShowText("MG");
                cb.EndText();
                //código IBGE
                cb.Rectangle(pdf.LeftMargin + 604f, topPos - hSum - 140f, 55f, caixaDados);
                cb.SetColorFill(BaseColor.WHITE);
                cb.FillStroke();
                cb.ResetCMYKColorFill();
                cb.BeginText();
                cb.SetFontAndSize(f_default, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 606f, topPos - hSum - 126f);
                cb.ShowText("38-Cód. IBGE");
                cb.SetFontAndSize(f_bold, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 606f, topPos - hSum - 138f);
                cb.ShowText("54545");
                cb.EndText();
                ////CEP
                cb.Rectangle(pdf.LeftMargin + 663f, topPos - hSum - 140f, 45f, caixaDados);
                cb.SetColorFill(BaseColor.WHITE);
                cb.FillStroke();
                cb.ResetCMYKColorFill();
                cb.BeginText();
                cb.SetFontAndSize(f_default, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 665f, topPos - hSum - 126f);
                cb.ShowText("39-CEP");
                cb.SetFontAndSize(f_bold, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 665f, topPos - hSum - 138f);
                cb.ShowText("30000-555");
                cb.EndText();
                //CNES
                cb.Rectangle(pdf.LeftMargin + 712f, topPos - hSum - 140f, 58f, caixaDados);
                cb.SetColorFill(BaseColor.WHITE);
                cb.FillStroke();
                cb.ResetCMYKColorFill();
                cb.BeginText();
                cb.SetFontAndSize(f_default, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 714f, topPos - hSum - 126f);
                cb.ShowText("40-Cód. CNES");
                cb.SetFontAndSize(f_bold, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 714f, topPos - hSum - 138f);
                cb.ShowText("3000055");
                cb.EndText();

                hSum += 10f;
                //codigo na operadora complementar
                cb.Rectangle(pdf.LeftMargin, topPos - hSum - 155f, 170f, caixaDados);
                cb.SetColorFill(BaseColor.WHITE);
                cb.FillStroke();
                cb.ResetCMYKColorFill();
                cb.BeginText();
                cb.SetFontAndSize(f_default, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 2f, topPos - hSum - 141f);
                cb.ShowText("40a-Cód. na Operadora/CPF do exec. comp.");
                cb.SetFontAndSize(f_bold, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 2f, topPos - hSum - 151f);
                cb.ShowText("56.456.0001/25");
                cb.EndText();
                //nome profissional
                cb.Rectangle(pdf.LeftMargin + 174f, topPos - hSum - 155f, 196f, caixaDados);
                cb.SetColorFill(BaseColor.WHITE);
                cb.FillStroke();
                cb.ResetCMYKColorFill();
                cb.BeginText();
                cb.SetFontAndSize(f_default, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 176f, topPos - hSum - 141f);
                cb.ShowText("41-Nome do Profissional Executante/Comp.");
                cb.SetFontAndSize(f_bold, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 176f, topPos - hSum - 151f);
                cb.ShowText("JOÃO DA SILVEIRA");
                cb.EndText();
                //CONSELHO PROFISSIONAL
                cb.Rectangle(pdf.LeftMargin + 374f, topPos - hSum - 155f, 100f, caixaDados);
                cb.SetColorFill(BaseColor.WHITE);
                cb.FillStroke();
                cb.ResetCMYKColorFill();
                cb.BeginText();
                cb.SetFontAndSize(f_default, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 376f, topPos - hSum - 141f);
                cb.ShowText("42 - Conselho Profissional");
                cb.SetFontAndSize(f_bold, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 376f, topPos - hSum - 151f);
                cb.ShowText("CRM");
                cb.EndText();
                //numero do conselho
                cb.Rectangle(pdf.LeftMargin + 478f, topPos - hSum - 155f, 100f, caixaDados);
                cb.SetColorFill(BaseColor.WHITE);
                cb.FillStroke();
                cb.ResetCMYKColorFill();
                cb.BeginText();
                cb.SetFontAndSize(f_default, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 480f, topPos - hSum - 141f);
                cb.ShowText("43 - Número no Conselho");
                cb.SetFontAndSize(f_bold, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 480f, topPos - hSum - 151f);
                cb.ShowText("545465456");
                cb.EndText();
                //uf
                cb.Rectangle(pdf.LeftMargin + 582f, topPos - hSum - 155f, 30f, caixaDados);
                cb.SetColorFill(BaseColor.WHITE);
                cb.FillStroke();
                cb.ResetCMYKColorFill();
                cb.BeginText();
                cb.SetFontAndSize(f_default, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 584f, topPos - hSum - 141f);
                cb.ShowText("44 - UF");
                cb.SetFontAndSize(f_bold, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 584f, topPos - hSum - 151f);
                cb.ShowText("MG");
                cb.EndText();
                //código cbos
                cb.Rectangle(pdf.LeftMargin + 616f, topPos - hSum - 155f, 70f, caixaDados);
                cb.SetColorFill(BaseColor.WHITE);
                cb.FillStroke();
                cb.ResetCMYKColorFill();
                cb.BeginText();
                cb.SetFontAndSize(f_default, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 618f, topPos - hSum - 141f);
                cb.ShowText("45 - Código CBOS");
                cb.SetFontAndSize(f_bold, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 618f, topPos - hSum - 151f);
                cb.ShowText("545465456");
                cb.EndText();
                // Grau de Participação
                cb.Rectangle(pdf.LeftMargin + 690f, topPos - hSum - 155f, 80f, caixaDados);
                cb.SetColorFill(BaseColor.WHITE);
                cb.FillStroke();
                cb.ResetCMYKColorFill();
                cb.BeginText();
                cb.SetFontAndSize(f_default, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 692f, topPos - hSum - 141f);
                cb.ShowText("45a - Grau de Partic.");
                cb.SetFontAndSize(f_bold, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 692f, topPos - hSum - 151f);
                cb.ShowText("1º");
                cb.EndText();

                //dados do contratado solicitante
                cb.Rectangle(pdf.LeftMargin, topPos - hSum - 170f, 770f, 12f);
                cb.SetColorFill(BaseColor.LIGHT_GRAY);
                cb.FillStroke();
                cb.ResetCMYKColorFill();

                cb.BeginText();
                cb.SetColorFill(BaseColor.BLACK);
                cb.SetFontAndSize(f_bold, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 10f, topPos - hSum - 166f);
                cb.ShowText("Dados do Atendimento");
                cb.EndText();
                hSum += caixaHeaderH;

                hSum += 10f;
                //Tipo Atendimento
                cb.Rectangle(pdf.LeftMargin, topPos - hSum - 164f, 350f, caixaDados);
                cb.SetColorFill(BaseColor.WHITE);
                cb.FillStroke();
                cb.ResetCMYKColorFill();
                cb.BeginText();
                cb.SetFontAndSize(f_default, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 2f, topPos - hSum - 150f);
                cb.ShowText("46 - Tipo Atendimento");
                cb.SetFontAndSize(f_bold, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 2f, topPos - hSum - 160f);
                cb.ShowText("01 Remoção ");
                cb.EndText();

                cb.Rectangle(pdf.LeftMargin + 354f, topPos - hSum - 164f, 200f, caixaDados);
                cb.SetColorFill(BaseColor.WHITE);
                cb.FillStroke();
                cb.ResetCMYKColorFill();
                cb.BeginText();
                cb.SetFontAndSize(f_default, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 356f, topPos - hSum - 150f);
                cb.ShowText("47 - Indicação de Acidente");
                cb.SetFontAndSize(f_bold, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 356f, topPos - hSum - 160f);
                cb.ShowText(" 0  Acidente ou doença relacionado ao trabalho");
                cb.EndText();

                cb.Rectangle(pdf.LeftMargin + 558f, topPos - hSum - 164f, 180f, caixaDados);
                cb.SetColorFill(BaseColor.WHITE);
                cb.FillStroke();
                cb.ResetCMYKColorFill();
                cb.BeginText();
                cb.SetFontAndSize(f_default, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 560f, topPos - hSum - 150f);
                cb.ShowText("48 - Tipo de Saída");
                cb.SetFontAndSize(f_bold, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 560f, topPos - hSum - 160f);
                cb.ShowText("01 Retorno");
                cb.EndText();


                //dados do contratado solicitante
                cb.Rectangle(pdf.LeftMargin, topPos - hSum - 179f, 770f, 12f);
                cb.SetColorFill(BaseColor.LIGHT_GRAY);
                cb.FillStroke();
                cb.ResetCMYKColorFill();

                cb.BeginText();
                cb.SetColorFill(BaseColor.BLACK);
                cb.SetFontAndSize(f_bold, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 10f, topPos - hSum - 176f);
                cb.ShowText("Consulta Referência");
                cb.EndText();
                hSum += caixaHeaderH;

                hSum += 10f;
                //Tipo Atendimento
                cb.Rectangle(pdf.LeftMargin, topPos - hSum - 173f, 250f, caixaDados);
                cb.SetColorFill(BaseColor.WHITE);
                cb.FillStroke();
                cb.ResetCMYKColorFill();
                cb.BeginText();
                cb.SetFontAndSize(f_default, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 2f, topPos - hSum - 159f);
                cb.ShowText("46 - Tipo Atendimento");
                cb.SetFontAndSize(f_bold, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 2f, topPos - hSum - 169f);
                cb.ShowText("01 Remoção ");
                cb.EndText();

                cb.Rectangle(pdf.LeftMargin + 254f, topPos - hSum - 173f, 200f, caixaDados);
                cb.SetColorFill(BaseColor.WHITE);
                cb.FillStroke();
                cb.ResetCMYKColorFill();
                cb.BeginText();
                cb.SetFontAndSize(f_default, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 256f, topPos - hSum - 160f);
                cb.ShowText("47 - Indicação de Acidente");
                cb.SetFontAndSize(f_bold, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 256f, topPos - hSum - 170f);
                cb.ShowText(" 0  Acidente ou doença relacionado ao trabalho");
                cb.EndText();


                //dados do contratado solicitante
                cb.Rectangle(pdf.LeftMargin, topPos - hSum - 187f, 770f, 12f);
                cb.SetColorFill(BaseColor.LIGHT_GRAY);
                cb.FillStroke();
                cb.ResetCMYKColorFill();

                cb.BeginText();
                cb.SetColorFill(BaseColor.BLACK);
                cb.SetFontAndSize(f_bold, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 10f, topPos - hSum - 184f);
                cb.ShowText("Procedimentos Realizados");
                cb.EndText();
                hSum += caixaHeaderH;



                //tabela de procedimentos realizados -----------------------------------------------------------------------------------------
                cb.Rectangle(pdf.LeftMargin, topPos - hSum - 215f, 770f, 45f);
                cb.SetColorFill(BaseColor.WHITE);
                cb.FillStroke();
                cb.ResetCMYKColorFill();

                PdfPTable tablep = new PdfPTable(11);
                tablep.SetWidths(new int[] { 7, 7, 7, 7, 22, 5, 4, 8, 8, 9, 10 });
                tablep.WidthPercentage = 100;
                tablep.SpacingBefore = 132f;

                //51-Data / 52-Hora Inicial / 53-Hora Final / 54-Tabela / 55-Código do Procedimento /
                //56 -Descrição /  57-Qtde. /  58-Via / 59-Tec. / 60% Red. / Acresc.  /  61-Valor Unitário - R$  / 62-Valor Total - R$

                PdfPCell cellTabData = new PdfPCell(new Phrase("51-Data", font));
                cellTabData.Border = Rectangle.TOP_BORDER | Rectangle.BOTTOM_BORDER;
                cellTabData.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                cellTabData.Border = 0;
                tablep.AddCell(cellTabData);


                PdfPCell cellHora = new PdfPCell(new Phrase("52-H. Inic.", font));
                cellHora.Border = 0;
                cellHora.HorizontalAlignment = 0;
                tablep.AddCell(cellHora);


                PdfPCell cellHoraF = new PdfPCell(new Phrase("53-H. Final", font));
                cellHoraF.HorizontalAlignment = 0;
                cellHoraF.Border = 0;
                tablep.AddCell(cellHoraF);


                PdfPCell cellTab = new PdfPCell(new Phrase("54-Tabela", font));
                cellTab.HorizontalAlignment = 0;
                cellTab.Border = 0;
                tablep.AddCell(cellTab);


                PdfPCell cellCod = new PdfPCell(new Phrase("55-Código do Procedimento / 56-Descrição", font));
                cellCod.HorizontalAlignment = 0;
                cellCod.Border = 0;
                tablep.AddCell(cellCod);


                //PdfPCell cellDesc = new PdfPCell(new Phrase("56 -Descrição", font));
                //cellDesc.HorizontalAlignment = 0;
                //cellDesc.Border = 0;
                //tablep.AddCell(cellDesc);


                PdfPCell cellQtde = new PdfPCell(new Phrase("57-Qtde.", font));
                cellQtde.HorizontalAlignment = 0;
                cellQtde.Border = 0;
                tablep.AddCell(cellQtde);


                PdfPCell cellVia = new PdfPCell(new Phrase("58-Via", font));
                cellVia.HorizontalAlignment = 0;
                cellVia.Border = 0;
                tablep.AddCell(cellVia);


                PdfPCell cellTec = new PdfPCell(new Phrase("59-Tec.", font));
                cellTec.HorizontalAlignment = 0;
                cellTec.Border = 0;
                tablep.AddCell(cellTec);


                PdfPCell celRed = new PdfPCell(new Phrase("60-Red./Acresc.", font));
                celRed.HorizontalAlignment = 0;
                celRed.Border = 0;
                tablep.AddCell(celRed);


                PdfPCell cellValU = new PdfPCell(new Phrase("61-V. Unitário-R$", font));
                cellValU.HorizontalAlignment = 0;
                cellValU.Border = 0;
                tablep.AddCell(cellValU);


                PdfPCell cellValT = new PdfPCell(new Phrase("62-V. Total-R$", font));
                cellValT.HorizontalAlignment = 0;
                cellValT.Border = 0;
                tablep.AddCell(cellValT);



                //51-Data / 52-Hora Inicial / 53-Hora Final / 54-Tabela / 55-Código do Procedimento /
                //56 -Descrição /  57-Qtde. /  58-Via / 59-Tec. / 60% Red. / Acresc.  /  61-Valor Unitário - R$  / 62-Valor Total - R$

                var listap = new List<procedimentoRealizadosteste>();

                for (int i = 0; i < 3; i++)
                {
                    listap.Add(new procedimentoRealizadosteste
                    {
                        Data = DateTime.Now.ToShortDateString(),
                        HoraInicail = DateTime.Now.ToShortTimeString(),
                        HoraFinal = DateTime.Now.ToShortTimeString(),
                        Tabela = "099",
                        Codigo = "10101012",
                        Descricao = "Consulta Eletiva",
                        Qtde = "1",
                        Via = "1",
                        Tecnico = "João",
                        ReducaoAcrescimo = "5%",
                        ValorUnitario = "100,00",
                        ValorTotal = "95,00"
                    });
                }

                //51-Data / 52-Hora Inicial / 53-Hora Final / 54-Tabela / 55-Código do Procedimento /
                //56 -Descrição /  57-Qtde. /  58-Via / 59-Tec. / 60% Red. / Acresc.  /  61-Valor Unitário - R$  / 62-Valor Total - R$
                foreach (var item in listap)
                {
                    PdfPCell celulaData = new PdfPCell(new Phrase(item.Data, fontBold));
                    celulaData.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    celulaData.Border = 0;
                    tablep.AddCell(celulaData);

                    PdfPCell celulaHora = new PdfPCell(new Phrase(item.HoraInicail, fontBold));
                    celulaHora.HorizontalAlignment = 0;
                    celulaHora.Border = 0;
                    tablep.AddCell(celulaHora);

                    PdfPCell celulaHoraF = new PdfPCell(new Phrase(item.HoraFinal, fontBold));
                    celulaHoraF.HorizontalAlignment = 0;
                    celulaHoraF.Border = 0;
                    tablep.AddCell(celulaHoraF);


                    PdfPCell celulatab = new PdfPCell(new Phrase(item.Tabela, fontBold));
                    celulatab.HorizontalAlignment = 0;
                    celulatab.Border = 0;
                    tablep.AddCell(celulatab);


                    PdfPCell celulaCod = new PdfPCell(new Phrase(item.Codigo + " / " + item.Descricao, fontBold));
                    celulaCod.HorizontalAlignment = 0;
                    celulaCod.Border = 0;
                    tablep.AddCell(celulaCod);

                    //PdfPCell celulaDesc = new PdfPCell(new Phrase(item.Descricao, fontBold));
                    //celulaDesc.HorizontalAlignment = 0;
                    //celulaDesc.Border = 0;
                    //tablep.AddCell(celulaDesc);


                    PdfPCell celulaQtde = new PdfPCell(new Phrase(item.Qtde, fontBold));
                    celulaQtde.HorizontalAlignment = 0;
                    celulaQtde.Border = 0;
                    tablep.AddCell(celulaQtde);

                    PdfPCell celulaVia = new PdfPCell(new Phrase(item.Via, fontBold));
                    celulaVia.HorizontalAlignment = 0;
                    celulaVia.Border = 0;
                    tablep.AddCell(celulaVia);

                    PdfPCell celulaTec = new PdfPCell(new Phrase(item.Tecnico, fontBold));
                    celulaTec.HorizontalAlignment = 0;
                    celulaTec.Border = 0;
                    tablep.AddCell(celulaTec);

                    PdfPCell celulaRed = new PdfPCell(new Phrase(item.ReducaoAcrescimo, fontBold));
                    celulaRed.HorizontalAlignment = 0;
                    celulaRed.Border = 0;
                    tablep.AddCell(celulaRed);

                    PdfPCell celulaValU = new PdfPCell(new Phrase(item.ValorUnitario, fontBold));
                    celulaValU.HorizontalAlignment = 0;
                    celulaValU.Border = 0;
                    tablep.AddCell(celulaValU);

                    PdfPCell celulaValTotal = new PdfPCell(new Phrase(item.ValorTotal, fontBold));
                    celulaValTotal.HorizontalAlignment = 0;
                    celulaValTotal.Border = 0;
                    tablep.AddCell(celulaValTotal);

                }
                Paragraph paragraphTablep = new Paragraph();
                paragraphTablep.SpacingBefore = 10f;
                paragraphTablep.Add(tablep);
                cb.PdfDocument.Add(paragraphTablep);

                //fim tabela de procedimentos realizados -----------------------------------------------------------------------------------------


                hSum += 10f;
                //DATA/HORA DA SOLICITAÇÃO
                cb.Rectangle(pdf.LeftMargin, topPos - hSum - 228f, 770f, 20f);
                cb.SetColorFill(BaseColor.WHITE);
                cb.FillStroke();
                cb.ResetCMYKColorFill();
                cb.BeginText();
                cb.SetFontAndSize(f_default, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 2f, topPos - hSum - 216f);
                cb.ShowText("63-Data e Assinatura de Procedimentos em Série");
                cb.SetFontAndSize(f_bold, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 2f, topPos - hSum - 226f);
                cb.ShowText(DateTime.Now.ToString());
                cb.EndText();

                cb.Rectangle(pdf.LeftMargin, topPos - hSum - 250f, 770f, 20f);
                cb.SetColorFill(BaseColor.WHITE);
                cb.FillStroke();
                cb.ResetCMYKColorFill();
                cb.BeginText();
                cb.SetFontAndSize(f_default, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 2f, topPos - hSum - 238f);
                cb.ShowText("64 - Observação");
                cb.SetFontAndSize(f_bold, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 2f, topPos - hSum - 248f);
                cb.ShowText(DateTime.Now.ToString());
                cb.EndText();

                cb.Rectangle(pdf.LeftMargin, topPos - hSum - 273f, 106f, 20f);
                cb.SetColorFill(BaseColor.WHITE);
                cb.FillStroke();
                cb.ResetCMYKColorFill();
                cb.BeginText();
                cb.SetFontAndSize(f_default, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 2f, topPos - hSum - 261f);
                cb.ShowText("65 - Total Procedimentos R$");
                cb.SetFontAndSize(f_bold, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 2f, topPos - hSum - 271f);
                cb.ShowText(DateTime.Now.ToString());
                cb.EndText();

                cb.Rectangle(pdf.LeftMargin + 110f, topPos - hSum - 273f, 114f, 20f);
                cb.SetColorFill(BaseColor.WHITE);
                cb.FillStroke();
                cb.ResetCMYKColorFill();
                cb.BeginText();
                cb.SetFontAndSize(f_default, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 112f, topPos - hSum - 261f);
                cb.ShowText("66 - Total Taxas e Aluguéis R$");
                cb.SetFontAndSize(f_bold, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 112f, topPos - hSum - 271f);
                cb.ShowText(DateTime.Now.ToString());
                cb.EndText();


                cb.Rectangle(pdf.LeftMargin + 228f, topPos - hSum - 273f, 98f, 20f);
                cb.SetColorFill(BaseColor.WHITE);
                cb.FillStroke();
                cb.ResetCMYKColorFill();
                cb.BeginText();
                cb.SetFontAndSize(f_default, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 230f, topPos - hSum - 261f);
                cb.ShowText("67- Total Materiais R$");
                cb.SetFontAndSize(f_bold, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 230f, topPos - hSum - 271f);
                cb.ShowText(DateTime.Now.ToString());
                cb.EndText();

                cb.Rectangle(pdf.LeftMargin + 330f, topPos - hSum - 273f, 106f, 20f);
                cb.SetColorFill(BaseColor.WHITE);
                cb.FillStroke();
                cb.ResetCMYKColorFill();
                cb.BeginText();
                cb.SetFontAndSize(f_default, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 332f, topPos - hSum - 261f);
                cb.ShowText("68 - Total Medicamentos R$");
                cb.SetFontAndSize(f_bold, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 332f, topPos - hSum - 271f);
                cb.ShowText(DateTime.Now.ToString());
                cb.EndText();


                cb.Rectangle(pdf.LeftMargin + 440f, topPos - hSum - 273f, 96f, 20f);
                cb.SetColorFill(BaseColor.WHITE);
                cb.FillStroke();
                cb.ResetCMYKColorFill();
                cb.BeginText();
                cb.SetFontAndSize(f_default, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 442f, topPos - hSum - 261f);
                cb.ShowText("69 - Total Diárias R$");
                cb.SetFontAndSize(f_bold, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 442f, topPos - hSum - 271f);
                cb.ShowText(DateTime.Now.ToString());
                cb.EndText();

                cb.Rectangle(pdf.LeftMargin + 540f, topPos - hSum - 273f, 118f, 20f);
                cb.SetColorFill(BaseColor.WHITE);
                cb.FillStroke();
                cb.ResetCMYKColorFill();
                cb.BeginText();
                cb.SetFontAndSize(f_default, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 542f, topPos - hSum - 261f);
                cb.ShowText("70 - Total Gases Medicinais R$");
                cb.SetFontAndSize(f_bold, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 542f, topPos - hSum - 271f);
                cb.ShowText(DateTime.Now.ToString());
                cb.EndText();

                cb.Rectangle(pdf.LeftMargin + 662f, topPos - hSum - 273f, 108f, 20f);
                cb.SetColorFill(BaseColor.WHITE);
                cb.FillStroke();
                cb.ResetCMYKColorFill();
                cb.BeginText();
                cb.SetFontAndSize(f_default, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 664f, topPos - hSum - 261f);
                cb.ShowText("71 - Total Geral da Guia R$");
                cb.SetFontAndSize(f_bold, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 664f, topPos - hSum - 271f);
                cb.ShowText(DateTime.Now.ToString());
                cb.EndText();

                cb.Rectangle(pdf.LeftMargin, topPos - hSum - 295f, 170f, 20f);
                cb.SetColorFill(BaseColor.WHITE);
                cb.FillStroke();
                cb.ResetCMYKColorFill();
                cb.BeginText();
                cb.SetFontAndSize(f_default, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 2f, topPos - hSum - 283f);
                cb.ShowText("86 - Data e Assinatura do Solicitante");
                cb.SetFontAndSize(f_bold, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 2f, topPos - hSum - 293f);
                cb.ShowText(DateTime.Now.ToString());
                cb.EndText();


                cb.Rectangle(pdf.LeftMargin + 174f, topPos - hSum - 295f, 215f, 20f);
                cb.SetColorFill(BaseColor.WHITE);
                cb.FillStroke();
                cb.ResetCMYKColorFill();
                cb.BeginText();
                cb.SetFontAndSize(f_default, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 176f, topPos - hSum - 283f);
                cb.ShowText("87 - Data e Assinatura do Responsável pela Autorização");
                cb.SetFontAndSize(f_bold, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 176f, topPos - hSum - 293f);
                cb.ShowText(DateTime.Now.ToString());
                cb.EndText();


                cb.Rectangle(pdf.LeftMargin + 393f, topPos - hSum - 295f, 200f, 20f);
                cb.SetColorFill(BaseColor.WHITE);
                cb.FillStroke();
                cb.ResetCMYKColorFill();
                cb.BeginText();
                cb.SetFontAndSize(f_default, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 395f, topPos - hSum - 283f);
                cb.ShowText("88-Data e Assinatura do Beneficiário ou Responsável");
                cb.SetFontAndSize(f_bold, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 395f, topPos - hSum - 293f);
                cb.ShowText(DateTime.Now.ToString());
                cb.EndText();


                cb.Rectangle(pdf.LeftMargin + 597f, topPos - hSum - 295f, 173f, 20f);
                cb.SetColorFill(BaseColor.WHITE);
                cb.FillStroke();
                cb.ResetCMYKColorFill();
                cb.BeginText();
                cb.SetFontAndSize(f_default, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 599f, topPos - hSum - 283f);
                cb.ShowText("89- Data e Assinatura do Prestador Executante");
                cb.SetFontAndSize(f_bold, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 599f, topPos - hSum - 293f);
                cb.ShowText(DateTime.Now.ToString());
                cb.EndText();



                pdf.Close();

                byte[] byteInfo = stream.ToArray();
                stream.Write(byteInfo, 0, byteInfo.Length);
                stream.Position = 0;




                return byteInfo;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public System.Drawing.Image byteArrayToImage(byte[] byteArrayIn)
        {
            MemoryStream ms = new MemoryStream(byteArrayIn);
            System.Drawing.Image returnImage = System.Drawing.Image.FromStream(ms);
            return returnImage;
        }

        public byte[] GetGuiaConsultaPdf(Int32 idGuia)
        {

            var guia = _repository.ObterGuiaPorId(idGuia);

            if (guia == null)
                throw new Exception("Não foi possível recuperar os dados da guia.");

            var convenio = _cadrepository.ObterConvenioById(guia.IdConvenio);

            if (convenio == null)
                throw new Exception("Não foi possível recuperar os dados do convênio.");


            float hSum = 0;
            Document pdf = new Document();
            MemoryStream stream = new MemoryStream();

            try
            {
                PdfWriter writer = PdfWriter.GetInstance(pdf, stream);
                writer.CloseStream = false;
                pdf.Open();
                PdfContentByte cb = writer.DirectContent;
                float topPos = pdf.PageSize.Height - pdf.TopMargin;
                float leftPos = pdf.LeftMargin;

                #region[cabeçalho PDF]

                //Cabeçalho - Logo
                float caixaLogoW = 120f;

                if (convenio.LogoGuia.Count() > 0)
                {
                    //iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(System.Web.HttpContext.Current.Server.MapPath("~/imagens/logo_promed.jpg"));
                    iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(byteArrayToImage(convenio.LogoGuia), System.Drawing.Imaging.ImageFormat.Jpeg);
                    logo.ScalePercent(60f);
                    logo.SetAbsolutePosition(leftPos + 10f, topPos - hSum - 40f);
                    pdf.Add(logo);
                }

                //Cabeçalho - Header
                float caixaHeaderH = 20f;
                cb.BeginText();
                cb.SetColorFill(BaseColor.BLACK);
                cb.SetFontAndSize(f_bold, 16);
                cb.SetTextMatrix(leftPos + caixaLogoW + 50f, topPos - hSum - 20f);
                cb.ShowText("GUIA DE CONSULTA");
                cb.EndText();
                hSum += caixaHeaderH;

                cb.BeginText();
                cb.SetColorFill(BaseColor.BLACK);
                cb.SetFontAndSize(f_bold, 10);
                cb.SetTextMatrix(leftPos + caixaLogoW + 260f, topPos - hSum - 5f);
                cb.ShowText("2 - Nº " + guia.CabecalhoGuia.NumeroGuia);
                cb.EndText();
                hSum += caixaHeaderH;

                //Soma de linha cabeçalho
                hSum += 10f;
                #endregion

                // 510f tamanho do documento


                hSum += 10f;
                //dados
                cb.Rectangle(pdf.LeftMargin, topPos - hSum - 20f, 150f, 30f);
                cb.SetColorFill(BaseColor.WHITE);
                cb.FillStroke();
                cb.ResetCMYKColorFill();
                cb.BeginText();
                cb.SetFontAndSize(f_default, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 2f, topPos - hSum);
                cb.ShowText("1 - Registro ANS");
                cb.SetFontAndSize(f_bold, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 2f, topPos - hSum - 15f);
                cb.ShowText(guia.CabecalhoGuia.RegistroANS);
                cb.EndText();

                cb.Rectangle(pdf.LeftMargin + 154f, topPos - hSum - 20f, 150f, 30f);
                cb.SetColorFill(BaseColor.WHITE);
                cb.FillStroke();
                cb.ResetCMYKColorFill();
                cb.BeginText();
                cb.SetFontAndSize(f_default, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 156f, topPos - hSum);
                cb.ShowText("3 - Data Emissão da Guia");
                cb.SetFontAndSize(f_bold, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 156f, topPos - hSum - 15f);
                cb.ShowText(guia.CabecalhoGuia.DataEmissaoGuia.ToShortDateString());
                cb.EndText();

                cb.Rectangle(pdf.LeftMargin, topPos - hSum - 40f, 510f, 15f);
                cb.SetColorFill(BaseColor.LIGHT_GRAY);
                cb.FillStroke();
                cb.ResetCMYKColorFill();

                cb.BeginText();
                cb.SetColorFill(BaseColor.BLACK);
                cb.SetFontAndSize(f_bold, 9);
                cb.SetTextMatrix(pdf.LeftMargin + 10f, topPos - hSum - 36f);
                cb.ShowText("Dados do Beneficiário");
                cb.EndText();
                hSum += caixaHeaderH;

                hSum += 10f;


                cb.Rectangle(pdf.LeftMargin, topPos - hSum - 45f, 160f, 30f);
                cb.SetColorFill(BaseColor.WHITE);
                cb.FillStroke();
                cb.ResetCMYKColorFill();
                cb.BeginText();
                cb.SetFontAndSize(f_default, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 2f, topPos - hSum - 25f);
                cb.ShowText("4 - Número da Carteira");
                cb.SetFontAndSize(f_bold, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 2f, topPos - hSum - 40f);
                cb.ShowText(guia.DadosBeneficiario.NumeroCarteira);
                cb.EndText();

                cb.Rectangle(pdf.LeftMargin + 164f, topPos - hSum - 45f, 240f, 30f);
                cb.SetColorFill(BaseColor.WHITE);
                cb.FillStroke();
                cb.ResetCMYKColorFill();
                cb.BeginText();
                cb.SetFontAndSize(f_default, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 166f, topPos - hSum - 25f);
                cb.ShowText("5 - Plano");
                cb.SetFontAndSize(f_bold, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 166f, topPos - hSum - 40f);
                cb.ShowText(guia.DadosBeneficiario.Plano);
                cb.EndText();

                cb.Rectangle(pdf.LeftMargin + 408f, topPos - hSum - 45f, 102f, 30f);
                cb.SetColorFill(BaseColor.WHITE);
                cb.FillStroke();
                cb.ResetCMYKColorFill();
                cb.BeginText();
                cb.SetFontAndSize(f_default, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 410f, topPos - hSum - 25f);
                cb.ShowText("6 - Validade da Carteira");
                cb.SetFontAndSize(f_bold, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 410f, topPos - hSum - 40f);
                cb.ShowText(guia.DadosBeneficiario.ValidadeCarteira.ToShortDateString());
                cb.EndText();

                hSum += 10f;


                cb.Rectangle(pdf.LeftMargin, topPos - hSum - 70f, 350f, 30f);
                cb.SetColorFill(BaseColor.WHITE);
                cb.FillStroke();
                cb.ResetCMYKColorFill();
                cb.BeginText();
                cb.SetFontAndSize(f_default, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 2f, topPos - hSum - 50f);
                cb.ShowText("7 - Nome");
                cb.SetFontAndSize(f_bold, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 2f, topPos - hSum - 65f);
                cb.ShowText(guia.DadosBeneficiario.Nome.ToUpper());
                cb.EndText();

                cb.Rectangle(pdf.LeftMargin + 354f, topPos - hSum - 70f, 156f, 30f);
                cb.SetColorFill(BaseColor.WHITE);
                cb.FillStroke();
                cb.ResetCMYKColorFill();
                cb.BeginText();
                cb.SetFontAndSize(f_default, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 358f, topPos - hSum - 50f);
                cb.ShowText("8 - Número do Cartão Nacional de Saúde");
                cb.SetFontAndSize(f_bold, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 358f, topPos - hSum - 65f);
                cb.ShowText(guia.DadosBeneficiario.NumeroCartaoSus ?? "");
                cb.EndText();


                cb.Rectangle(pdf.LeftMargin, topPos - hSum - 90f, 510f, 15f);
                cb.SetColorFill(BaseColor.LIGHT_GRAY);
                cb.FillStroke();
                cb.ResetCMYKColorFill();

                cb.BeginText();
                cb.SetColorFill(BaseColor.BLACK);
                cb.SetFontAndSize(f_bold, 9);
                cb.SetTextMatrix(pdf.LeftMargin + 10f, topPos - hSum - 86f);
                cb.ShowText("Dados do Contratado");
                cb.EndText();
                hSum += caixaHeaderH;

                hSum += 10f;
                //-----------------------------------------------------------------------------

                cb.Rectangle(pdf.LeftMargin, topPos - hSum - 95f, 150f, 30f);
                cb.SetColorFill(BaseColor.WHITE);
                cb.FillStroke();
                cb.ResetCMYKColorFill();
                cb.BeginText();
                cb.SetFontAndSize(f_default, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 2f, topPos - hSum - 75f);
                cb.ShowText("9 - Código na Operadora / CNPJ / CPF");
                cb.SetFontAndSize(f_bold, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 2f, topPos - hSum - 90f);
                cb.ShowText(guia.DadosContratado.CodigoNaOperadora);
                cb.EndText();

                cb.Rectangle(pdf.LeftMargin + 154f, topPos - hSum - 95f, 270f, 30f);
                cb.SetColorFill(BaseColor.WHITE);
                cb.FillStroke();
                cb.ResetCMYKColorFill();
                cb.BeginText();
                cb.SetFontAndSize(f_default, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 156f, topPos - hSum - 75f);
                cb.ShowText("10 - Nome do Contratado");
                cb.SetFontAndSize(f_bold, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 156f, topPos - hSum - 90f);
                cb.ShowText(guia.DadosContratado.NomeContratado.ToUpper());
                cb.EndText();

                cb.Rectangle(pdf.LeftMargin + 428f, topPos - hSum - 95f, 82f, 30f);
                cb.SetColorFill(BaseColor.WHITE);
                cb.FillStroke();
                cb.ResetCMYKColorFill();
                cb.BeginText();
                cb.SetFontAndSize(f_default, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 432f, topPos - hSum - 75f);
                cb.ShowText("11 - Código CNES");
                cb.SetFontAndSize(f_bold, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 432f, topPos - hSum - 90f);
                cb.ShowText(guia.DadosContratado.CodigoCnes ?? "");
                cb.EndText();

                hSum += 10f;


                cb.Rectangle(pdf.LeftMargin, topPos - hSum - 120f, 40f, 30f);
                cb.SetColorFill(BaseColor.WHITE);
                cb.FillStroke();
                cb.ResetCMYKColorFill();
                cb.BeginText();
                cb.SetFontAndSize(f_default, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 2f, topPos - hSum - 100f);
                cb.ShowText("12 - T.L");
                cb.SetFontAndSize(f_bold, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 2f, topPos - hSum - 115f);
                cb.ShowText(guia.DadosContratado.TipoLogradouro ?? "");
                cb.EndText();

                cb.Rectangle(pdf.LeftMargin + 44f, topPos - hSum - 120f, 200f, 30f);
                cb.SetColorFill(BaseColor.WHITE);
                cb.FillStroke();
                cb.ResetCMYKColorFill();
                cb.BeginText();
                cb.SetFontAndSize(f_default, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 46f, topPos - hSum - 100f);
                cb.ShowText("13-14-15 - Logradouro - Número - Complemento");
                cb.SetFontAndSize(f_bold, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 46f, topPos - hSum - 115f);
                cb.ShowText(guia.DadosContratado.Logradouro ?? "");
                cb.EndText();

                cb.Rectangle(pdf.LeftMargin + 248f, topPos - hSum - 120f, 100f, 30f);
                cb.SetColorFill(BaseColor.WHITE);
                cb.FillStroke();
                cb.ResetCMYKColorFill();
                cb.BeginText();
                cb.SetFontAndSize(f_default, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 250f, topPos - hSum - 100f);
                cb.ShowText("16 - Município");
                cb.SetFontAndSize(f_bold, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 250f, topPos - hSum - 115f);
                cb.ShowText(guia.DadosContratado.Municipio ?? "");
                cb.EndText();

                cb.Rectangle(pdf.LeftMargin + 352f, topPos - hSum - 120f, 30f, 30f);
                cb.SetColorFill(BaseColor.WHITE);
                cb.FillStroke();
                cb.ResetCMYKColorFill();
                cb.BeginText();
                cb.SetFontAndSize(f_default, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 354f, topPos - hSum - 100f);
                cb.ShowText("17 - UF");
                cb.SetFontAndSize(f_bold, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 354f, topPos - hSum - 115f);
                cb.ShowText(guia.DadosContratado.UFContratado ?? "");
                cb.EndText();

                cb.Rectangle(pdf.LeftMargin + 386f, topPos - hSum - 120f, 70f, 30f);
                cb.SetColorFill(BaseColor.WHITE);
                cb.FillStroke();
                cb.ResetCMYKColorFill();
                cb.BeginText();
                cb.SetFontAndSize(f_default, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 388f, topPos - hSum - 100f);
                cb.ShowText("18 - Código IBGE");
                cb.SetFontAndSize(f_bold, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 388f, topPos - hSum - 115f);
                cb.ShowText(guia.DadosContratado.CodigoIBGEMunicipio ?? "");
                cb.EndText();

                cb.Rectangle(pdf.LeftMargin + 460f, topPos - hSum - 120f, 50f, 30f);
                cb.SetColorFill(BaseColor.WHITE);
                cb.FillStroke();
                cb.ResetCMYKColorFill();
                cb.BeginText();
                cb.SetFontAndSize(f_default, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 462f, topPos - hSum - 100f);
                cb.ShowText("19 - CEP");
                cb.SetFontAndSize(f_bold, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 462f, topPos - hSum - 115f);
                cb.ShowText(guia.DadosContratado.CEP ?? "");
                cb.EndText();

                hSum += 10f;

                cb.Rectangle(pdf.LeftMargin, topPos - hSum - 145f, 200f, 30f);
                cb.SetColorFill(BaseColor.WHITE);
                cb.FillStroke();
                cb.ResetCMYKColorFill();
                cb.BeginText();
                cb.SetFontAndSize(f_default, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 2f, topPos - hSum - 125f);
                cb.ShowText("20 - Nome do Profissional Executante");
                cb.SetFontAndSize(f_bold, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 2f, topPos - hSum - 140f);
                string NomeProfissionalExecutante = guia.DadosContratado.NomeProfissionalExecutante;
                cb.ShowText(NomeProfissionalExecutante ?? "");


                cb.EndText();

                cb.Rectangle(pdf.LeftMargin + 204f, topPos - hSum - 145f, 100f, 30f);
                cb.SetColorFill(BaseColor.WHITE);
                cb.FillStroke();
                cb.ResetCMYKColorFill();
                cb.BeginText();
                cb.SetFontAndSize(f_default, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 206f, topPos - hSum - 125f);
                cb.ShowText("21 - Conselho Profissional");
                cb.SetFontAndSize(f_bold, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 206f, topPos - hSum - 140f);
                cb.ShowText(guia.DadosContratado.ConselhoProfissional ?? "");
                cb.EndText();

                cb.Rectangle(pdf.LeftMargin + 308f, topPos - hSum - 145f, 100f, 30f);
                cb.SetColorFill(BaseColor.WHITE);
                cb.FillStroke();
                cb.ResetCMYKColorFill();
                cb.BeginText();
                cb.SetFontAndSize(f_default, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 310f, topPos - hSum - 125f);
                cb.ShowText("22 - Número no Conselho");
                cb.SetFontAndSize(f_bold, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 310f, topPos - hSum - 140f);
                cb.ShowText(guia.DadosContratado.NumeroConselho ?? "");
                cb.EndText();

                cb.Rectangle(pdf.LeftMargin + 412f, topPos - hSum - 145f, 30f, 30f);
                cb.SetColorFill(BaseColor.WHITE);
                cb.FillStroke();
                cb.ResetCMYKColorFill();
                cb.BeginText();
                cb.SetFontAndSize(f_default, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 414f, topPos - hSum - 125f);
                cb.ShowText("23 - UF");
                cb.SetFontAndSize(f_bold, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 414f, topPos - hSum - 140f);
                cb.ShowText(guia.DadosContratado.UFConselho ?? "");
                cb.EndText();

                cb.Rectangle(pdf.LeftMargin + 446f, topPos - hSum - 145f, 65f, 30f);
                cb.SetColorFill(BaseColor.WHITE);
                cb.FillStroke();
                cb.ResetCMYKColorFill();
                cb.BeginText();
                cb.SetFontAndSize(f_default, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 448f, topPos - hSum - 125f);
                cb.ShowText("24-Código CBOS");
                cb.SetFontAndSize(f_bold, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 448f, topPos - hSum - 140f);
                cb.ShowText(guia.DadosContratado.CodigoCBOs ?? "");
                cb.EndText();

                cb.Rectangle(pdf.LeftMargin, topPos - hSum - 165f, 510f, 15f);
                cb.SetColorFill(BaseColor.LIGHT_GRAY);
                cb.FillStroke();
                cb.ResetCMYKColorFill();

                cb.BeginText();
                cb.SetColorFill(BaseColor.BLACK);
                cb.SetFontAndSize(f_bold, 9);
                cb.SetTextMatrix(pdf.LeftMargin + 10f, topPos - hSum - 162f);
                cb.ShowText("Hipóteses Diagnósticas");
                cb.EndText();
                hSum += caixaHeaderH;

                hSum += 10f;

                cb.Rectangle(pdf.LeftMargin, topPos - hSum - 170f, 90f, 30f);
                cb.SetColorFill(BaseColor.WHITE);
                cb.FillStroke();
                cb.ResetCMYKColorFill();
                cb.BeginText();
                cb.SetFontAndSize(f_default, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 2f, topPos - hSum - 150f);
                cb.ShowText("25 - Tipo de Doença");
                cb.SetFontAndSize(f_bold, 7);
                cb.SetTextMatrix(pdf.LeftMargin + 2f, topPos - hSum - 165f);
                cb.ShowText("[ " + guia.HipoteseDiagnostica.TipoDoenca + " ]  A-Aguda  C-Crônica");
                cb.EndText();

                cb.Rectangle(pdf.LeftMargin + 94f, topPos - hSum - 170f, 125f, 30f);
                cb.SetColorFill(BaseColor.WHITE);
                cb.FillStroke();
                cb.ResetCMYKColorFill();
                cb.BeginText();
                cb.SetFontAndSize(f_default, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 96f, topPos - hSum - 150f);
                cb.ShowText("26 - Tempo de Doença");
                cb.SetFontAndSize(f_bold, 7);
                cb.SetTextMatrix(pdf.LeftMargin + 96f, topPos - hSum - 165f);
                cb.ShowText("[ " + guia.HipoteseDiagnostica.TempoDoenca + " ] D A-Anos M-Meses D-Dias");
                cb.EndText();

                cb.Rectangle(pdf.LeftMargin + 223f, topPos - hSum - 170f, 287f, 30f);
                cb.SetColorFill(BaseColor.WHITE);
                cb.FillStroke();
                cb.ResetCMYKColorFill();
                cb.BeginText();
                cb.SetFontAndSize(f_default, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 225f, topPos - hSum - 150f);
                cb.ShowText("27 - Indicação de Acidente");
                cb.SetFontAndSize(f_bold, 7);
                cb.SetTextMatrix(pdf.LeftMargin + 225f, topPos - hSum - 165f);
                cb.ShowText("[ " + guia.HipoteseDiagnostica.IndicacaoAcidente + " ]  0 - Acidente ou doença relacionado ao trabalho  1 - Trânsito  2 - Outros");
                cb.EndText();



                cb.Rectangle(pdf.LeftMargin, topPos - hSum - 205f, 120f, 30f);
                cb.SetColorFill(BaseColor.WHITE);
                cb.FillStroke();
                cb.ResetCMYKColorFill();
                cb.BeginText();
                cb.SetFontAndSize(f_default, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 2f, topPos - hSum - 185f);
                cb.ShowText("28 - CID Principal");
                cb.SetFontAndSize(f_bold, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 2f, topPos - hSum - 200f);
                cb.ShowText(guia.HipoteseDiagnostica.CID10 ?? "");
                cb.EndText();

                cb.Rectangle(pdf.LeftMargin + 124f, topPos - hSum - 205f, 120f, 30f);
                cb.SetColorFill(BaseColor.WHITE);
                cb.FillStroke();
                cb.ResetCMYKColorFill();
                cb.BeginText();
                cb.SetFontAndSize(f_default, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 126f, topPos - hSum - 185f);
                cb.ShowText("29 - CID (2)");
                cb.SetFontAndSize(f_bold, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 126f, topPos - hSum - 200f);
                cb.ShowText(guia.HipoteseDiagnostica.CID102 ?? "");
                cb.EndText();

                cb.Rectangle(pdf.LeftMargin + 248f, topPos - hSum - 205f, 120f, 30f);
                cb.SetColorFill(BaseColor.WHITE);
                cb.FillStroke();
                cb.ResetCMYKColorFill();
                cb.BeginText();
                cb.SetFontAndSize(f_default, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 250f, topPos - hSum - 185f);
                cb.ShowText("30 - CID (3)");
                cb.SetFontAndSize(f_bold, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 250f, topPos - hSum - 200f);
                cb.ShowText(guia.HipoteseDiagnostica.CID103 ?? "");
                cb.EndText();

                cb.Rectangle(pdf.LeftMargin + 372f, topPos - hSum - 205f, 138f, 30f);
                cb.SetColorFill(BaseColor.WHITE);
                cb.FillStroke();
                cb.ResetCMYKColorFill();
                cb.BeginText();
                cb.SetFontAndSize(f_default, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 374f, topPos - hSum - 185f);
                cb.ShowText("31 - CID (4)");
                cb.SetFontAndSize(f_bold, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 374f, topPos - hSum - 200f);
                cb.ShowText(guia.HipoteseDiagnostica.CID104 ?? "");
                cb.EndText();

                cb.Rectangle(pdf.LeftMargin, topPos - hSum - 225f, 510f, 15f);
                cb.SetColorFill(BaseColor.LIGHT_GRAY);
                cb.FillStroke();
                cb.ResetCMYKColorFill();

                cb.BeginText();
                cb.SetColorFill(BaseColor.BLACK);
                cb.SetFontAndSize(f_bold, 9);
                cb.SetTextMatrix(pdf.LeftMargin + 10f, topPos - hSum - 221f);
                cb.ShowText("Dados do Atendimento / Procedimnto Realizado");
                cb.EndText();
                hSum += caixaHeaderH;

                hSum += 10f;

                cb.Rectangle(pdf.LeftMargin, topPos - hSum - 230f, 150f, 30f);
                cb.SetColorFill(BaseColor.WHITE);
                cb.FillStroke();
                cb.ResetCMYKColorFill();
                cb.BeginText();
                cb.SetFontAndSize(f_default, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 2f, topPos - hSum - 210f);
                cb.ShowText("32 - Data do Atendimento");
                cb.SetFontAndSize(f_bold, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 2f, topPos - hSum - 225f);
                cb.ShowText(guia.DadosAtendimento.DataAtendimento.ToShortDateString());
                cb.EndText();

                cb.Rectangle(pdf.LeftMargin + 154f, topPos - hSum - 230f, 100f, 30f);
                cb.SetColorFill(BaseColor.WHITE);
                cb.FillStroke();
                cb.ResetCMYKColorFill();
                cb.BeginText();
                cb.SetFontAndSize(f_default, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 156f, topPos - hSum - 210f);
                cb.ShowText("33 - Código Tabela");
                cb.SetFontAndSize(f_bold, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 156f, topPos - hSum - 225f);
                cb.ShowText(guia.DadosAtendimento.CodigoTabela ?? "");
                cb.EndText();

                cb.Rectangle(pdf.LeftMargin + 258f, topPos - hSum - 230f, 252f, 30f);
                cb.SetColorFill(BaseColor.WHITE);
                cb.FillStroke();
                cb.ResetCMYKColorFill();
                cb.BeginText();
                cb.SetFontAndSize(f_default, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 260f, topPos - hSum - 210f);
                cb.ShowText("34 - Código Procedimento");
                cb.SetFontAndSize(f_bold, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 260f, topPos - hSum - 225f);
                cb.ShowText(guia.DadosAtendimento.CodigoProcedimento ?? "");
                cb.EndText();


                cb.Rectangle(pdf.LeftMargin, topPos - hSum - 265f, 200f, 30f);
                cb.SetColorFill(BaseColor.WHITE);
                cb.FillStroke();
                cb.ResetCMYKColorFill();
                cb.BeginText();
                cb.SetFontAndSize(f_default, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 2f, topPos - hSum - 245f);
                cb.ShowText("35 - Tipo de Consulta");
                cb.SetFontAndSize(f_bold, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 2f, topPos - hSum - 260f);
                cb.ShowText("[ " + guia.DadosAtendimento.TipoConsulta + " ]  - 1-Primeira  2-Seguimento  3-Pré-Natal");
                cb.EndText();

                cb.Rectangle(pdf.LeftMargin + 204f, topPos - hSum - 265f, 305f, 30f);
                cb.SetColorFill(BaseColor.WHITE);
                cb.FillStroke();
                cb.ResetCMYKColorFill();
                cb.BeginText();
                cb.SetFontAndSize(f_default, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 206f, topPos - hSum - 245f);
                cb.ShowText("36 - Tipo de Saída");
                cb.SetFontAndSize(f_bold, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 206f, topPos - hSum - 260f);
                cb.ShowText("[ " + guia.DadosAtendimento.TipoSaida + " ]  - 1-Retorno    2-Retorno SADT    3-Referência    4-Internação    5-Alta");
                cb.EndText();


                cb.Rectangle(pdf.LeftMargin, topPos - hSum - 320f, 510f, 50f);
                cb.SetColorFill(BaseColor.WHITE);
                cb.FillStroke();
                cb.ResetCMYKColorFill();
                cb.BeginText();
                cb.SetFontAndSize(f_default, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 2f, topPos - hSum - 280f);
                cb.ShowText("37 - Observação");
                cb.SetFontAndSize(f_bold, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 2f, topPos - hSum - 295f);
                cb.ShowText(guia.DadosAtendimento.Observacao ?? "");
                cb.EndText();

                cb.Rectangle(pdf.LeftMargin, topPos - hSum - 355f, 250f, 30f);
                cb.SetColorFill(BaseColor.WHITE);
                cb.FillStroke();
                cb.ResetCMYKColorFill();
                cb.BeginText();
                cb.SetFontAndSize(f_default, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 2f, topPos - hSum - 335f);
                cb.ShowText("38 - Data e Assinatura do Médico");
                cb.SetFontAndSize(f_bold, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 2f, topPos - hSum - 350f);
                cb.ShowText(DateTime.Now.ToShortDateString());
                cb.EndText();


                cb.Rectangle(pdf.LeftMargin + 254f, topPos - hSum - 355f, 256f, 30f);
                cb.SetColorFill(BaseColor.WHITE);
                cb.FillStroke();
                cb.ResetCMYKColorFill();
                cb.BeginText();
                cb.SetFontAndSize(f_default, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 256f, topPos - hSum - 335f);
                cb.ShowText("39 - Data e Assinatura do Beneficiário ou Responsável");
                cb.SetFontAndSize(f_bold, 8);
                cb.SetTextMatrix(pdf.LeftMargin + 260f, topPos - hSum - 350f);
                cb.ShowText(DateTime.Now.ToShortDateString());
                cb.EndText();

                pdf.Close();

                byte[] byteInfo = stream.ToArray();
                stream.Write(byteInfo, 0, byteInfo.Length);
                stream.Position = 0;

                return byteInfo;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public ICollection<Guia> Pesqusiar(int? idguia, string nome)
        {
            return _repository.Pesquisar(idguia, nome);
        }

        public void CancelarGuia(int idguia)
        {
            _repository.CancelarGuia(idguia);
        }

        #region [Lotes guia]
        public Lote SalvarLote(Lote lote)
        {
            return _repository.SalvarLote(lote);
        }

        public void ExcluirLote(Lote lote)
        {
            _repository.ExcluirLote(lote);
        }

        public List<Lote> ListarLotes(int idClinica)
        {
            return _repository.ListarLotes(idClinica);
        }

        public Lote ObterLotePorId(int idLote)
        {
            return _repository.ObterLotePorId(idLote);
        }

        public List<Lote> ListarLotesPorConvenio(int idClinica, int idConvenio)
        {
            return _repository.ListarLotesPorConvenio(idClinica, idConvenio);
        }
        #endregion
    }
}