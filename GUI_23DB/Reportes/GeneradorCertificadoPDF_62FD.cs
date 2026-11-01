using BE_23DB;
using BLL_23DB;
using Services_23DB;
using System;
using System.Collections.Generic;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;

namespace GUI_23DB
{
    public class GeneradorCertificadoPDF_62FD
    {
        RepuestoBLL_62FD repuestoBLL_62FD = new RepuestoBLL_62FD();

        // Con nombreInspector_62FD vacío o nulo se omite la línea del inspector: la base no guarda quién certificó,
        // así que una reimpresión no puede afirmar un nombre.
        public void Generar_62FD(string ruta_62FD, Intervencion_62FD intervencion_62FD, Certificacion_62FD certificacion_62FD, Aeronave_62FD aeronave_62FD, OrdenTrabajo_62FD orden_62FD, List<Tarea_62FD> tareas_62FD, string nombreInspector_62FD)
        {
            bool apto_62FD = certificacion_62FD.resultadoInspeccion_62FD == "Satisfactorio";

            using (FileStream stream_62FD = new FileStream(ruta_62FD, FileMode.Create))
            {
                Document doc_62FD = new Document(PageSize.A4, 45f, 45f, 40f, 50f);
                PdfWriter writer_62FD = PdfWriter.GetInstance(doc_62FD, stream_62FD);
                doc_62FD.Open();
                PdfContentByte cb_62FD = writer_62FD.DirectContent;

                BaseColor azul_62FD = new BaseColor(31, 78, 121);
                BaseColor grisTexto_62FD = new BaseColor(110, 110, 110);
                BaseColor grisBorde_62FD = new BaseColor(200, 200, 200);
                BaseColor verde_62FD = new BaseColor(30, 120, 60);
                BaseColor rojo_62FD = new BaseColor(170, 30, 30);
                BaseColor colorResultado_62FD = apto_62FD ? verde_62FD : rojo_62FD;
                BaseColor fondoResultado_62FD = apto_62FD ? new BaseColor(232, 245, 236) : new BaseColor(250, 232, 232);

                iTextSharp.text.Font fuenteMarca_62FD = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16f, azul_62FD);
                iTextSharp.text.Font fuenteTitulo_62FD = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 22f, BaseColor.BLACK);
                iTextSharp.text.Font fuenteSubtitulo_62FD = FontFactory.GetFont(FontFactory.HELVETICA, 10f, grisTexto_62FD);
                iTextSharp.text.Font fuenteSeccion_62FD = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 11f, azul_62FD);
                iTextSharp.text.Font fuenteEtiqueta_62FD = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 8f, grisTexto_62FD);
                iTextSharp.text.Font fuenteDato_62FD = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 11f, BaseColor.BLACK);
                iTextSharp.text.Font fuenteTexto_62FD = FontFactory.GetFont(FontFactory.HELVETICA, 10f, BaseColor.BLACK);
                iTextSharp.text.Font fuenteTextoNegrita_62FD = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10f, BaseColor.BLACK);
                iTextSharp.text.Font fuenteTabla_62FD = FontFactory.GetFont(FontFactory.HELVETICA, 9f, BaseColor.BLACK);
                iTextSharp.text.Font fuenteResultado_62FD = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12f, colorResultado_62FD);
                iTextSharp.text.Font fuenteLeyenda_62FD = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 22f, colorResultado_62FD);
                iTextSharp.text.Font fuenteChica_62FD = FontFactory.GetFont(FontFactory.HELVETICA, 8f, grisTexto_62FD);

                // Encabezado enmarcado
                PdfPTable encabezado_62FD = new PdfPTable(1);
                encabezado_62FD.WidthPercentage = 100;
                encabezado_62FD.SpacingAfter = 14f;

                PdfPCell celdaEncabezado_62FD = new PdfPCell();
                celdaEncabezado_62FD.Border = iTextSharp.text.Rectangle.BOX;
                celdaEncabezado_62FD.BorderColor = azul_62FD;
                celdaEncabezado_62FD.BorderWidth = 2f;
                celdaEncabezado_62FD.Padding = 14f;
                celdaEncabezado_62FD.PaddingBottom = 18f;

                Paragraph marca_62FD = new Paragraph("AeroManager", fuenteMarca_62FD);
                marca_62FD.Alignment = Element.ALIGN_CENTER;
                celdaEncabezado_62FD.AddElement(marca_62FD);

                Paragraph titulo_62FD = new Paragraph(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_CertificadoTitulo"), fuenteTitulo_62FD);
                titulo_62FD.Alignment = Element.ALIGN_CENTER;
                titulo_62FD.SpacingBefore = 4f;
                celdaEncabezado_62FD.AddElement(titulo_62FD);

                Paragraph subtitulo_62FD = new Paragraph(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_CertificadoSubtitulo"), fuenteSubtitulo_62FD);
                subtitulo_62FD.Alignment = Element.ALIGN_CENTER;
                celdaEncabezado_62FD.AddElement(subtitulo_62FD);

                encabezado_62FD.AddCell(celdaEncabezado_62FD);
                doc_62FD.Add(encabezado_62FD);

                // Número y fecha de certificación
                PdfPTable identificacion_62FD = new PdfPTable(2);
                identificacion_62FD.WidthPercentage = 100;
                identificacion_62FD.SpacingAfter = 6f;

                PdfPCell celdaNumero_62FD = new PdfPCell(new Phrase(string.Format(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_CertificadoNumero"), certificacion_62FD.IdIntervencion_62FD), fuenteDato_62FD));
                celdaNumero_62FD.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                celdaNumero_62FD.BorderColor = grisBorde_62FD;
                celdaNumero_62FD.PaddingBottom = 8f;
                identificacion_62FD.AddCell(celdaNumero_62FD);

                PdfPCell celdaFecha_62FD = new PdfPCell(new Phrase(string.Format(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_CertificadoFechaHora"), certificacion_62FD.fechaHoraCertificacion_62FD.ToString("dd/MM/yyyy HH:mm")), fuenteTexto_62FD));
                celdaFecha_62FD.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                celdaFecha_62FD.BorderColor = grisBorde_62FD;
                celdaFecha_62FD.PaddingBottom = 8f;
                celdaFecha_62FD.HorizontalAlignment = Element.ALIGN_RIGHT;
                identificacion_62FD.AddCell(celdaFecha_62FD);

                doc_62FD.Add(identificacion_62FD);

                // Datos de la aeronave
                AgregarSeccion_62FD(doc_62FD, IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_CertificadoSeccionAeronave"), fuenteSeccion_62FD, azul_62FD);

                PdfPTable datosAeronave_62FD = new PdfPTable(3);
                datosAeronave_62FD.WidthPercentage = 100;
                AgregarDato_62FD(datosAeronave_62FD, IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_CertificadoMatricula"), intervencion_62FD.Matricula_62FD, fuenteEtiqueta_62FD, fuenteDato_62FD);
                AgregarDato_62FD(datosAeronave_62FD, IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_CertificadoModelo"), aeronave_62FD != null ? aeronave_62FD.Modelo_62FD : "-", fuenteEtiqueta_62FD, fuenteDato_62FD);
                AgregarDato_62FD(datosAeronave_62FD, IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_CertificadoFabricante"), aeronave_62FD != null ? aeronave_62FD.Fabricante_62FD : "-", fuenteEtiqueta_62FD, fuenteDato_62FD);
                doc_62FD.Add(datosAeronave_62FD);

                // Datos de la intervención
                AgregarSeccion_62FD(doc_62FD, IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_CertificadoSeccionIntervencion"), fuenteSeccion_62FD, azul_62FD);

                string fechaFin_62FD = intervencion_62FD.fechaHoraFin_62FD.HasValue ? intervencion_62FD.fechaHoraFin_62FD.Value.ToString("dd/MM/yyyy HH:mm") : "-";

                PdfPTable datosIntervencion_62FD = new PdfPTable(3);
                datosIntervencion_62FD.WidthPercentage = 100;
                AgregarDato_62FD(datosIntervencion_62FD, IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_CertificadoNroOrden"), intervencion_62FD.IdOrden_62FD.ToString(), fuenteEtiqueta_62FD, fuenteDato_62FD);
                AgregarDato_62FD(datosIntervencion_62FD, IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_CertificadoTipoIntervencion"), orden_62FD != null ? orden_62FD.TipoIntervencion_62FD : "-", fuenteEtiqueta_62FD, fuenteDato_62FD);
                AgregarDato_62FD(datosIntervencion_62FD, IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_CertificadoTotalHoras"), intervencion_62FD.totalHorasInsumidas_62FD.ToString("N2"), fuenteEtiqueta_62FD, fuenteDato_62FD);
                AgregarDato_62FD(datosIntervencion_62FD, IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_CertificadoFechaInicio"), intervencion_62FD.fechaHoraInicio_62FD.ToString("dd/MM/yyyy HH:mm"), fuenteEtiqueta_62FD, fuenteDato_62FD);
                AgregarDato_62FD(datosIntervencion_62FD, IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_CertificadoFechaFin"), fechaFin_62FD, fuenteEtiqueta_62FD, fuenteDato_62FD);
                AgregarDato_62FD(datosIntervencion_62FD, string.Empty, string.Empty, fuenteEtiqueta_62FD, fuenteDato_62FD);
                doc_62FD.Add(datosIntervencion_62FD);

                // Detalle de tareas
                AgregarSeccion_62FD(doc_62FD, IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_CertificadoSeccionTareas"), fuenteSeccion_62FD, azul_62FD);

                PdfPTable tablaTareas_62FD = new PdfPTable(5);
                tablaTareas_62FD.WidthPercentage = 100;
                tablaTareas_62FD.SetWidths(new float[] { 28f, 14f, 10f, 22f, 26f });
                tablaTareas_62FD.HeaderRows = 1;

                AgregarEncabezado_62FD(tablaTareas_62FD, IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_CertificadoColDescripcion"), azul_62FD, Element.ALIGN_LEFT);
                AgregarEncabezado_62FD(tablaTareas_62FD, IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_CertificadoColResultado"), azul_62FD, Element.ALIGN_LEFT);
                AgregarEncabezado_62FD(tablaTareas_62FD, IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_CertificadoColHoras"), azul_62FD, Element.ALIGN_RIGHT);
                AgregarEncabezado_62FD(tablaTareas_62FD, IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_CertificadoColRepuestos"), azul_62FD, Element.ALIGN_LEFT);
                AgregarEncabezado_62FD(tablaTareas_62FD, IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_CertificadoColObservaciones"), azul_62FD, Element.ALIGN_LEFT);

                foreach (Tarea_62FD tarea_62FD in tareas_62FD)
                {
                    string[] valores_62FD = new string[]
                    {
                        tarea_62FD.Descripcion_62FD,
                        tarea_62FD.ResultadoObtenido_62FD,
                        tarea_62FD.HorasInsumidas_62FD.ToString("N2"),
                        ObtenerRepuestosUtilizados_62FD(tarea_62FD.IdTarea_62FD),
                        string.IsNullOrWhiteSpace(tarea_62FD.observacionesTecnicas_62FD) ? "-" : tarea_62FD.observacionesTecnicas_62FD
                    };

                    for (int i_62FD = 0; i_62FD < valores_62FD.Length; i_62FD++)
                    {
                        PdfPCell celda_62FD = new PdfPCell(new Phrase(valores_62FD[i_62FD], fuenteTabla_62FD));
                        celda_62FD.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                        celda_62FD.BorderColor = grisBorde_62FD;
                        celda_62FD.Padding = 5f;
                        celda_62FD.HorizontalAlignment = i_62FD == 2 ? Element.ALIGN_RIGHT : Element.ALIGN_LEFT;
                        tablaTareas_62FD.AddCell(celda_62FD);
                    }
                }

                doc_62FD.Add(tablaTareas_62FD);

                // Resultado de la inspección, destacado
                AgregarSeccion_62FD(doc_62FD, IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_CertificadoSeccionResultado"), fuenteSeccion_62FD, azul_62FD);

                PdfPTable resultado_62FD = new PdfPTable(1);
                resultado_62FD.WidthPercentage = 100;
                resultado_62FD.SpacingBefore = 4f;

                PdfPCell celdaResultado_62FD = new PdfPCell();
                celdaResultado_62FD.Border = iTextSharp.text.Rectangle.BOX;
                celdaResultado_62FD.BorderColor = colorResultado_62FD;
                celdaResultado_62FD.BorderWidth = 2f;
                celdaResultado_62FD.BackgroundColor = fondoResultado_62FD;
                celdaResultado_62FD.Padding = 12f;
                celdaResultado_62FD.PaddingBottom = 16f;

                Paragraph valorResultado_62FD = new Paragraph(IdiomaBLL_23DB.ObtenerTexto_23DB(apto_62FD ? "Msg_CertificadoResultadoSatisfactorio" : "Msg_CertificadoResultadoNoSatisfactorio"), fuenteResultado_62FD);
                valorResultado_62FD.Alignment = Element.ALIGN_CENTER;
                celdaResultado_62FD.AddElement(valorResultado_62FD);

                Paragraph leyenda_62FD = new Paragraph(IdiomaBLL_23DB.ObtenerTexto_23DB(apto_62FD ? "Msg_CertificadoApto" : "Msg_CertificadoNoApto"), fuenteLeyenda_62FD);
                leyenda_62FD.Alignment = Element.ALIGN_CENTER;
                leyenda_62FD.SpacingBefore = 2f;
                celdaResultado_62FD.AddElement(leyenda_62FD);

                resultado_62FD.AddCell(celdaResultado_62FD);
                doc_62FD.Add(resultado_62FD);

                // Observaciones del inspector
                AgregarSeccion_62FD(doc_62FD, IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_CertificadoSeccionObservaciones"), fuenteSeccion_62FD, azul_62FD);

                string observaciones_62FD = string.IsNullOrWhiteSpace(certificacion_62FD.Observaciones_62FD) ? IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_CertificadoSinObservaciones") : certificacion_62FD.Observaciones_62FD;
                doc_62FD.Add(new Paragraph(observaciones_62FD, fuenteTexto_62FD));

                // Pie con datos del inspector y línea para firma
                PdfPTable firma_62FD = new PdfPTable(2);
                firma_62FD.WidthPercentage = 100;
                firma_62FD.SetWidths(new float[] { 55f, 45f });
                firma_62FD.SpacingBefore = 40f;
                firma_62FD.KeepTogether = true;

                PdfPCell celdaInspector_62FD = new PdfPCell();
                celdaInspector_62FD.Border = iTextSharp.text.Rectangle.NO_BORDER;
                celdaInspector_62FD.Rowspan = 2;
                celdaInspector_62FD.VerticalAlignment = Element.ALIGN_BOTTOM;
                Phrase fraseCodigo_62FD = new Phrase();
                fraseCodigo_62FD.Add(new Chunk(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_CertificadoCodigoHabilitante") + " ", fuenteTextoNegrita_62FD));
                fraseCodigo_62FD.Add(new Chunk(certificacion_62FD.codHabilitante_62FD, fuenteTexto_62FD));
                celdaInspector_62FD.AddElement(new Paragraph(fraseCodigo_62FD));
                if (!string.IsNullOrWhiteSpace(nombreInspector_62FD))
                {
                    Phrase fraseInspector_62FD = new Phrase();
                    fraseInspector_62FD.Add(new Chunk(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_CertificadoInspector") + " ", fuenteTextoNegrita_62FD));
                    fraseInspector_62FD.Add(new Chunk(nombreInspector_62FD, fuenteTexto_62FD));
                    celdaInspector_62FD.AddElement(new Paragraph(fraseInspector_62FD));
                }
                firma_62FD.AddCell(celdaInspector_62FD);

                PdfPCell celdaEspacioFirma_62FD = new PdfPCell(new Phrase(" "));
                celdaEspacioFirma_62FD.Border = iTextSharp.text.Rectangle.NO_BORDER;
                celdaEspacioFirma_62FD.FixedHeight = 45f;
                firma_62FD.AddCell(celdaEspacioFirma_62FD);

                PdfPCell celdaLineaFirma_62FD = new PdfPCell(new Phrase(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_CertificadoFirma"), fuenteChica_62FD));
                celdaLineaFirma_62FD.Border = iTextSharp.text.Rectangle.TOP_BORDER;
                celdaLineaFirma_62FD.BorderColor = BaseColor.BLACK;
                celdaLineaFirma_62FD.BorderWidth = 0.8f;
                celdaLineaFirma_62FD.PaddingTop = 4f;
                celdaLineaFirma_62FD.HorizontalAlignment = Element.ALIGN_CENTER;
                firma_62FD.AddCell(celdaLineaFirma_62FD);

                doc_62FD.Add(firma_62FD);

                ColumnText.ShowTextAligned(cb_62FD, Element.ALIGN_CENTER, new Phrase(string.Format(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_CertificadoNotaSistema"), certificacion_62FD.fechaHoraCertificacion_62FD.ToString("dd/MM/yyyy HH:mm")), fuenteChica_62FD), doc_62FD.PageSize.Width / 2f, 30f, 0f);
                doc_62FD.Close();
            }
        }

        private string ObtenerRepuestosUtilizados_62FD(int idTarea_62FD)
        {
            List<string> repuestos_62FD = new List<string>();

            foreach (Repuesto_62FD repuesto_62FD in repuestoBLL_62FD.ObtenerRepuestosPorTarea_62FD(idTarea_62FD))
            {
                if (repuesto_62FD.estadoPedido_62FD == "Recibido")
                {
                    repuestos_62FD.Add(repuesto_62FD.nombreRepuesto_62FD + " x" + repuesto_62FD.cantidadSolicitada_62FD);
                }
            }

            if (repuestos_62FD.Count == 0)
            {
                return IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_CertificadoSinRepuestos");
            }

            return string.Join("\n", repuestos_62FD);
        }

        private void AgregarSeccion_62FD(Document doc_62FD, string titulo_62FD, iTextSharp.text.Font fuente_62FD, BaseColor color_62FD)
        {
            PdfPTable seccion_62FD = new PdfPTable(1);
            seccion_62FD.WidthPercentage = 100;
            seccion_62FD.SpacingBefore = 14f;
            seccion_62FD.SpacingAfter = 4f;

            PdfPCell celda_62FD = new PdfPCell(new Phrase(titulo_62FD, fuente_62FD));
            celda_62FD.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            celda_62FD.BorderColor = color_62FD;
            celda_62FD.BorderWidth = 1.2f;
            celda_62FD.PaddingBottom = 4f;
            celda_62FD.PaddingLeft = 0f;
            seccion_62FD.AddCell(celda_62FD);

            doc_62FD.Add(seccion_62FD);
        }

        private void AgregarDato_62FD(PdfPTable tabla_62FD, string etiqueta_62FD, string valor_62FD, iTextSharp.text.Font fuenteEtiqueta_62FD, iTextSharp.text.Font fuenteDato_62FD)
        {
            PdfPCell celda_62FD = new PdfPCell();
            celda_62FD.Border = iTextSharp.text.Rectangle.NO_BORDER;
            celda_62FD.PaddingLeft = 0f;
            celda_62FD.PaddingTop = 4f;
            celda_62FD.PaddingBottom = 6f;
            celda_62FD.AddElement(new Paragraph(etiqueta_62FD, fuenteEtiqueta_62FD));
            celda_62FD.AddElement(new Paragraph(valor_62FD, fuenteDato_62FD));
            tabla_62FD.AddCell(celda_62FD);
        }

        private void AgregarEncabezado_62FD(PdfPTable tabla_62FD, string texto_62FD, BaseColor color_62FD, int alineacion_62FD)
        {
            iTextSharp.text.Font fuente_62FD = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 9f, BaseColor.WHITE);
            PdfPCell celda_62FD = new PdfPCell(new Phrase(texto_62FD, fuente_62FD));
            celda_62FD.BackgroundColor = color_62FD;
            celda_62FD.Border = iTextSharp.text.Rectangle.NO_BORDER;
            celda_62FD.Padding = 5f;
            celda_62FD.HorizontalAlignment = alineacion_62FD;
            tabla_62FD.AddCell(celda_62FD);
        }
    }
}
