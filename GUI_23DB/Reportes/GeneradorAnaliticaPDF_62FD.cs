using iTextSharp.text;
using iTextSharp.text.pdf;
using Services_23DB;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI_23DB.Reportes
{
    public class GeneradorAnaliticaPDF_62FD
    {
        public void Generar_62FD(string ruta_62FD, DateTime desde_62FD, DateTime hasta_62FD, int meses_62FD, string revisionesVencidas_62FD, string revisionesProximas_62FD, string diasReposicion_62FD, System.Drawing.Image graficoEstado_62FD, System.Drawing.Image graficoTipo_62FD, System.Drawing.Image graficoProyeccion_62FD)
        {
            BaseColor azul_62FD = new BaseColor(31, 73, 125);
            BaseColor gris_62FD = new BaseColor(242, 242, 242);

            iTextSharp.text.Font fuenteTitulo_62FD = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 18, azul_62FD);
            iTextSharp.text.Font fuenteSubtitulo_62FD = FontFactory.GetFont(FontFactory.HELVETICA, 10, BaseColor.DARK_GRAY);
            iTextSharp.text.Font fuenteIndicador_62FD = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 24, azul_62FD);
            iTextSharp.text.Font fuenteEtiqueta_62FD = FontFactory.GetFont(FontFactory.HELVETICA, 9, BaseColor.DARK_GRAY);
            iTextSharp.text.Font fuenteChica_62FD = FontFactory.GetFont(FontFactory.HELVETICA, 8, BaseColor.GRAY);

            Document doc_62FD = new Document(PageSize.A4, 40f, 40f, 40f, 50f);

            using (FileStream archivo_62FD = new FileStream(ruta_62FD, FileMode.Create))
            {
                PdfWriter writer_62FD = PdfWriter.GetInstance(doc_62FD, archivo_62FD);
                doc_62FD.Open();

                Paragraph titulo_62FD = new Paragraph(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_AnaliticaPdfTitulo"), fuenteTitulo_62FD);
                titulo_62FD.Alignment = Element.ALIGN_CENTER;
                doc_62FD.Add(titulo_62FD);

                Paragraph subtitulo_62FD = new Paragraph(string.Format(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_AnaliticaPdfSubtitulo"), desde_62FD.ToString("dd/MM/yyyy"), hasta_62FD.ToString("dd/MM/yyyy"), meses_62FD.ToString()), fuenteSubtitulo_62FD);
                subtitulo_62FD.Alignment = Element.ALIGN_CENTER;
                subtitulo_62FD.SpacingAfter = 20f;
                doc_62FD.Add(subtitulo_62FD);

                PdfPTable indicadores_62FD = new PdfPTable(3);
                indicadores_62FD.WidthPercentage = 100;
                indicadores_62FD.SpacingAfter = 25f;

                AgregarIndicador_62FD(indicadores_62FD, revisionesVencidas_62FD, IdiomaBLL_23DB.ObtenerTexto_23DB("AnaliticaMantenimiento_label3"), fuenteIndicador_62FD, fuenteEtiqueta_62FD, gris_62FD);
                AgregarIndicador_62FD(indicadores_62FD, revisionesProximas_62FD, IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_AnaliticaPdfVencen30Dias"), fuenteIndicador_62FD, fuenteEtiqueta_62FD, gris_62FD);
                AgregarIndicador_62FD(indicadores_62FD, diasReposicion_62FD, IdiomaBLL_23DB.ObtenerTexto_23DB("AnaliticaMantenimiento_label7"), fuenteIndicador_62FD, fuenteEtiqueta_62FD, gris_62FD);

                doc_62FD.Add(indicadores_62FD);

                PdfPTable tortas_62FD = new PdfPTable(2);
                tortas_62FD.WidthPercentage = 100;
                tortas_62FD.SpacingAfter = 20f;

                tortas_62FD.AddCell(CeldaImagen_62FD(graficoEstado_62FD, 250f));
                tortas_62FD.AddCell(CeldaImagen_62FD(graficoTipo_62FD, 250f));

                doc_62FD.Add(tortas_62FD);

                iTextSharp.text.Image proyeccion_62FD = iTextSharp.text.Image.GetInstance(graficoProyeccion_62FD, System.Drawing.Imaging.ImageFormat.Png);
                proyeccion_62FD.ScaleToFit(515f, 300f);
                proyeccion_62FD.Alignment = Element.ALIGN_CENTER;
                doc_62FD.Add(proyeccion_62FD);

                PdfContentByte cb_62FD = writer_62FD.DirectContent;
                ColumnText.ShowTextAligned(cb_62FD, Element.ALIGN_CENTER, new Phrase(string.Format(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_AnaliticaPdfPie"), DateTime.Now.ToString("dd/MM/yyyy HH:mm")), fuenteChica_62FD), doc_62FD.PageSize.Width / 2f, 30f, 0f);

                doc_62FD.Close();
            }
        }

        private void AgregarIndicador_62FD(PdfPTable tabla_62FD, string valor_62FD, string etiqueta_62FD, iTextSharp.text.Font fuenteValor_62FD, iTextSharp.text.Font fuenteEtiqueta_62FD, BaseColor fondo_62FD)
        {
            PdfPCell celda_62FD = new PdfPCell();
            celda_62FD.BackgroundColor = fondo_62FD;
            celda_62FD.Border = iTextSharp.text.Rectangle.NO_BORDER;
            celda_62FD.Padding = 12f;
            celda_62FD.HorizontalAlignment = Element.ALIGN_CENTER;

            Paragraph valor_p_62FD = new Paragraph(valor_62FD, fuenteValor_62FD);
            valor_p_62FD.Alignment = Element.ALIGN_CENTER;
            celda_62FD.AddElement(valor_p_62FD);

            Paragraph etiqueta_p_62FD = new Paragraph(etiqueta_62FD, fuenteEtiqueta_62FD);
            etiqueta_p_62FD.Alignment = Element.ALIGN_CENTER;
            celda_62FD.AddElement(etiqueta_p_62FD);

            tabla_62FD.AddCell(celda_62FD);
        }

        private PdfPCell CeldaImagen_62FD(System.Drawing.Image imagen_62FD, float ancho_62FD)
        {
            iTextSharp.text.Image img_62FD = iTextSharp.text.Image.GetInstance(imagen_62FD, System.Drawing.Imaging.ImageFormat.Png);
            img_62FD.ScaleToFit(ancho_62FD, 220f);

            PdfPCell celda_62FD = new PdfPCell(img_62FD, false);
            celda_62FD.Border = iTextSharp.text.Rectangle.NO_BORDER;
            celda_62FD.HorizontalAlignment = Element.ALIGN_CENTER;
            celda_62FD.Padding = 5f;

            return celda_62FD;
        }
    }
}
