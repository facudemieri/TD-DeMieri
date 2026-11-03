using BE_23DB;
using Services_23DB;
using System;
using System.Collections.Generic;
using System.Linq;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;

namespace GUI_23DB
{
    public class GeneradorPasajePDF_62FD
    {
        public void Generar_62FD(string ruta_62FD, Reserva_62FD reserva_62FD, Factura_62FD factura_62FD, List<Pasaje_62FD> pasajes_62FD, string medioPago_62FD)
        {
            using (FileStream stream_62FD = new FileStream(ruta_62FD, FileMode.Create))
            {
                Document doc_62FD = new Document(PageSize.A4.Rotate(), 36f, 36f, 36f, 36f);
                PdfWriter writer_62FD = PdfWriter.GetInstance(doc_62FD, stream_62FD);
                doc_62FD.Open();
                PdfContentByte cb_62FD = writer_62FD.DirectContent;

                BaseColor azul_62FD = new BaseColor(31, 78, 121);
                BaseColor grisTexto_62FD = new BaseColor(110, 110, 110);
                BaseColor grisBorde_62FD = new BaseColor(200, 200, 200);
                BaseColor grisFondo_62FD = new BaseColor(230, 230, 230);

                iTextSharp.text.Font fuenteMarca_62FD = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 20f, BaseColor.WHITE);
                iTextSharp.text.Font fuenteBanda_62FD = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12f, BaseColor.WHITE);
                iTextSharp.text.Font fuenteEtiqueta_62FD = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 8f, grisTexto_62FD);
                iTextSharp.text.Font fuenteVuelo_62FD = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 30f, azul_62FD);
                iTextSharp.text.Font fuenteRuta_62FD = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 30f, BaseColor.BLACK);
                BaseFont baseSimbolos_62FD = BaseFont.CreateFont(BaseFont.SYMBOL, BaseFont.SYMBOL, BaseFont.NOT_EMBEDDED);
                iTextSharp.text.Font fuenteFlecha_62FD = new iTextSharp.text.Font(baseSimbolos_62FD, 30f, iTextSharp.text.Font.NORMAL, azul_62FD);
                iTextSharp.text.Font fuenteDato_62FD = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 13f, BaseColor.BLACK);
                iTextSharp.text.Font fuenteCodigo_62FD = FontFactory.GetFont(FontFactory.COURIER_BOLD, 18f, azul_62FD);
                iTextSharp.text.Font fuenteChica_62FD = FontFactory.GetFont(FontFactory.HELVETICA, 8f, grisTexto_62FD);

                // Documento 1: un boarding pass apaisado por pasajero
                bool primeraPagina_62FD = true;
                foreach (Pasaje_62FD pasaje_62FD in pasajes_62FD)
                {
                    if (!primeraPagina_62FD)
                    {
                        doc_62FD.NewPage();
                    }
                    primeraPagina_62FD = false;

                    ReservaDetalle_62FD detalleReserva_62FD = reserva_62FD.Detalles_62FD .First(d => d.IdDetalle_62FD == pasaje_62FD.IdDetalle_62FD);

                    PdfPTable banda_62FD = new PdfPTable(2);
                    banda_62FD.WidthPercentage = 100;
                    banda_62FD.SpacingAfter = 24f;

                    PdfPCell celdaMarca_62FD = new PdfPCell(new Phrase("AeroManager", fuenteMarca_62FD));
                    celdaMarca_62FD.BackgroundColor = azul_62FD;
                    celdaMarca_62FD.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    celdaMarca_62FD.Padding = 14f;
                    celdaMarca_62FD.VerticalAlignment = Element.ALIGN_MIDDLE;
                    banda_62FD.AddCell(celdaMarca_62FD);

                    PdfPCell celdaTipo_62FD = new PdfPCell(new Phrase(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_PasajeTitulo"), fuenteBanda_62FD));
                    celdaTipo_62FD.BackgroundColor = azul_62FD;
                    celdaTipo_62FD.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    celdaTipo_62FD.Padding = 14f;
                    celdaTipo_62FD.HorizontalAlignment = Element.ALIGN_RIGHT;
                    celdaTipo_62FD.VerticalAlignment = Element.ALIGN_MIDDLE;
                    banda_62FD.AddCell(celdaTipo_62FD);

                    doc_62FD.Add(banda_62FD);

                    PdfPTable vuelo_62FD = new PdfPTable(2);
                    vuelo_62FD.WidthPercentage = 100;
                    vuelo_62FD.SetWidths(new float[] { 35f, 65f });
                    vuelo_62FD.SpacingAfter = 20f;

                    PdfPCell celdaVuelo_62FD = new PdfPCell();
                    celdaVuelo_62FD.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    celdaVuelo_62FD.AddElement(new Paragraph(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_PasajeVuelo"), fuenteEtiqueta_62FD));
                    celdaVuelo_62FD.AddElement(new Paragraph(reserva_62FD.NroVuelo_62FD, fuenteVuelo_62FD));
                    vuelo_62FD.AddCell(celdaVuelo_62FD);

                    // Mismo leading que el valor de VUELO (1,5 x tamaño); el constructor vacío deja 16pt y el texto se monta sobre la etiqueta
                    Paragraph parrafoRuta_62FD = new Paragraph();
                    parrafoRuta_62FD.Leading = fuenteRuta_62FD.Size * 1.5f;
                    string[] partesRuta_62FD = reserva_62FD.Ruta_62FD.Split(new string[] { "->" }, StringSplitOptions.None);
                    if (partesRuta_62FD.Length == 2)
                    {
                        parrafoRuta_62FD.Add(new Chunk(partesRuta_62FD[0].Trim() + "  ", fuenteRuta_62FD));
                        parrafoRuta_62FD.Add(new Chunk("→", fuenteFlecha_62FD));
                        parrafoRuta_62FD.Add(new Chunk("  " + partesRuta_62FD[1].Trim(), fuenteRuta_62FD));
                    }
                    else
                    {
                        parrafoRuta_62FD.Add(new Chunk(reserva_62FD.Ruta_62FD, fuenteRuta_62FD));
                    }

                    PdfPCell celdaRuta_62FD = new PdfPCell();
                    celdaRuta_62FD.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    celdaRuta_62FD.AddElement(new Paragraph(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_PasajeRuta"), fuenteEtiqueta_62FD));
                    celdaRuta_62FD.AddElement(parrafoRuta_62FD);
                    vuelo_62FD.AddCell(celdaRuta_62FD);

                    doc_62FD.Add(vuelo_62FD);

                    string[,] datosPasaje_62FD = new string[,]
                    {
                        { IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_PasajePasajero"), detalleReserva_62FD.NombreCompleto_62FD, IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_PasajeDocumento"), detalleReserva_62FD.NumeroDocumento_62FD },
                        { IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_PasajeFechaSalida"), reserva_62FD.FechaHoraSalida_62FD.ToString("dd/MM/yyyy"), IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_PasajeHoraSalida"), reserva_62FD.FechaHoraSalida_62FD.ToString("HH:mm") },
                        { IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_PasajeAsiento"), detalleReserva_62FD.NroAsiento_62FD, IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_PasajeClase"), detalleReserva_62FD.Clase_62FD }
                    };

                    PdfPTable bloque_62FD = new PdfPTable(3);
                    bloque_62FD.WidthPercentage = 100;
                    bloque_62FD.SpacingAfter = 24f;

                    for (int i_62FD = 0; i_62FD < 3; i_62FD++)
                    {
                        PdfPCell celdaBloque_62FD = new PdfPCell();
                        celdaBloque_62FD.Border = iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER;
                        if (i_62FD > 0)
                        {
                            celdaBloque_62FD.Border |= iTextSharp.text.Rectangle.LEFT_BORDER;
                        }
                        celdaBloque_62FD.BorderColor = grisBorde_62FD;
                        celdaBloque_62FD.Padding = 10f;
                        celdaBloque_62FD.PaddingBottom = 14f;

                        celdaBloque_62FD.AddElement(new Paragraph(datosPasaje_62FD[i_62FD, 0], fuenteEtiqueta_62FD));
                        celdaBloque_62FD.AddElement(new Paragraph(datosPasaje_62FD[i_62FD, 1], fuenteDato_62FD));
                        Paragraph segundaEtiqueta_62FD = new Paragraph(datosPasaje_62FD[i_62FD, 2], fuenteEtiqueta_62FD);
                        segundaEtiqueta_62FD.SpacingBefore = 8f;
                        celdaBloque_62FD.AddElement(segundaEtiqueta_62FD);
                        celdaBloque_62FD.AddElement(new Paragraph(datosPasaje_62FD[i_62FD, 3], fuenteDato_62FD));

                        bloque_62FD.AddCell(celdaBloque_62FD);
                    }

                    doc_62FD.Add(bloque_62FD);

                    PdfPTable recuadro_62FD = new PdfPTable(1);
                    recuadro_62FD.WidthPercentage = 40;
                    recuadro_62FD.HorizontalAlignment = Element.ALIGN_CENTER;

                    PdfPCell celdaCodigo_62FD = new PdfPCell();
                    celdaCodigo_62FD.BorderColor = azul_62FD;
                    celdaCodigo_62FD.BorderWidth = 1.5f;
                    celdaCodigo_62FD.Padding = 10f;
                    celdaCodigo_62FD.PaddingBottom = 14f;

                    Paragraph etiquetaCodigo_62FD = new Paragraph(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_PasajeCodigo"), fuenteEtiqueta_62FD);
                    etiquetaCodigo_62FD.Alignment = Element.ALIGN_CENTER;
                    celdaCodigo_62FD.AddElement(etiquetaCodigo_62FD);
                    Paragraph valorCodigo_62FD = new Paragraph(pasaje_62FD.CodPasaje_62FD, fuenteCodigo_62FD);
                    valorCodigo_62FD.Alignment = Element.ALIGN_CENTER;
                    celdaCodigo_62FD.AddElement(valorCodigo_62FD);

                    recuadro_62FD.AddCell(celdaCodigo_62FD);
                    doc_62FD.Add(recuadro_62FD);

                    float anchoPagina_62FD = doc_62FD.PageSize.Width;
                    float yCorte_62FD = 70f;

                    Barcode128 barcode_62FD = new Barcode128();
                    barcode_62FD.Code = pasaje_62FD.CodPasaje_62FD;
                    barcode_62FD.BarHeight = 45f;
                    barcode_62FD.X = 1.2f;
                    iTextSharp.text.Image imagenBarcode_62FD = barcode_62FD.CreateImageWithBarcode(cb_62FD, BaseColor.BLACK, BaseColor.BLACK);
                    imagenBarcode_62FD.SetAbsolutePosition((anchoPagina_62FD - imagenBarcode_62FD.ScaledWidth) / 2f, yCorte_62FD + 25f);
                    cb_62FD.AddImage(imagenBarcode_62FD);

                    cb_62FD.SaveState();
                    cb_62FD.SetLineDash(4f, 3f, 0f);
                    cb_62FD.SetLineWidth(0.8f);
                    cb_62FD.SetColorStroke(grisTexto_62FD);
                    cb_62FD.MoveTo(doc_62FD.LeftMargin, yCorte_62FD);
                    cb_62FD.LineTo(anchoPagina_62FD - doc_62FD.RightMargin, yCorte_62FD);
                    cb_62FD.Stroke();
                    cb_62FD.RestoreState();

                    ColumnText.ShowTextAligned(cb_62FD, Element.ALIGN_CENTER, new Phrase(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_PasajeConserve"), fuenteChica_62FD), anchoPagina_62FD / 2f, yCorte_62FD - 14f, 0f);
                }

                // Documento 2: factura en A4 vertical, última página
                doc_62FD.SetPageSize(PageSize.A4);
                doc_62FD.SetMargins(40f, 40f, 40f, 40f);
                doc_62FD.NewPage();

                iTextSharp.text.Font fuenteEmpresa_62FD = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16f, azul_62FD);
                iTextSharp.text.Font fuenteFactura_62FD = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 26f, azul_62FD);
                iTextSharp.text.Font fuenteTexto_62FD = FontFactory.GetFont(FontFactory.HELVETICA, 9f, BaseColor.BLACK);
                iTextSharp.text.Font fuenteTextoNegrita_62FD = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10f, BaseColor.BLACK);
                iTextSharp.text.Font fuenteTabla_62FD = FontFactory.GetFont(FontFactory.HELVETICA, 10f, BaseColor.BLACK);
                iTextSharp.text.Font fuenteTotal_62FD = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 11f, BaseColor.BLACK);

                PdfPTable cabecera_62FD = new PdfPTable(2);
                cabecera_62FD.WidthPercentage = 100;
                cabecera_62FD.SetWidths(new float[] { 55f, 45f });
                cabecera_62FD.SpacingAfter = 12f;

                PdfPCell celdaEmisor_62FD = new PdfPCell();
                celdaEmisor_62FD.Border = iTextSharp.text.Rectangle.BOX;
                celdaEmisor_62FD.BorderColor = azul_62FD;
                celdaEmisor_62FD.Padding = 14f;
                celdaEmisor_62FD.AddElement(new Paragraph("AeroManager S.A.", fuenteEmpresa_62FD));
                celdaEmisor_62FD.AddElement(new Paragraph("Av. Corrientes 1234, Piso 5", fuenteTexto_62FD));
                celdaEmisor_62FD.AddElement(new Paragraph("C1043AAZ - Ciudad Autónoma de Buenos Aires", fuenteTexto_62FD));
                celdaEmisor_62FD.AddElement(new Paragraph("CUIT: 30-71234567-8", fuenteTexto_62FD));
                cabecera_62FD.AddCell(celdaEmisor_62FD);

                PdfPCell celdaComprobante_62FD = new PdfPCell();
                celdaComprobante_62FD.Border = iTextSharp.text.Rectangle.BOX;
                celdaComprobante_62FD.BorderColor = azul_62FD;
                celdaComprobante_62FD.Padding = 14f;
                celdaComprobante_62FD.AddElement(new Paragraph(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_PasajeFactura"), fuenteFactura_62FD));
                celdaComprobante_62FD.AddElement(new Paragraph(string.Format(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_PasajeNroFactura"), factura_62FD.NroFactura_62FD), fuenteTextoNegrita_62FD));
                celdaComprobante_62FD.AddElement(new Paragraph(string.Format(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_PasajeFechaEmision"), factura_62FD.FechaEmision_62FD.ToString("dd/MM/yyyy HH:mm")), fuenteTexto_62FD));
                cabecera_62FD.AddCell(celdaComprobante_62FD);

                doc_62FD.Add(cabecera_62FD);

                PdfPTable operacion_62FD = new PdfPTable(2);
                operacion_62FD.WidthPercentage = 100;
                operacion_62FD.SpacingAfter = 16f;

                Phrase fraseReserva_62FD = new Phrase();
                fraseReserva_62FD.Add(new Chunk(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_PasajeCodigoReserva") + " ", fuenteTextoNegrita_62FD));
                fraseReserva_62FD.Add(new Chunk(reserva_62FD.CodReserva_62FD, fuenteTabla_62FD));
                PdfPCell celdaReserva_62FD = new PdfPCell(fraseReserva_62FD);
                celdaReserva_62FD.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                celdaReserva_62FD.BorderColor = grisBorde_62FD;
                celdaReserva_62FD.PaddingBottom = 8f;
                operacion_62FD.AddCell(celdaReserva_62FD);

                Phrase fraseMedioPago_62FD = new Phrase();
                fraseMedioPago_62FD.Add(new Chunk(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_PasajeMedioPago") + " ", fuenteTextoNegrita_62FD));
                fraseMedioPago_62FD.Add(new Chunk(medioPago_62FD, fuenteTabla_62FD));
                PdfPCell celdaMedioPago_62FD = new PdfPCell(fraseMedioPago_62FD);
                celdaMedioPago_62FD.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                celdaMedioPago_62FD.BorderColor = grisBorde_62FD;
                celdaMedioPago_62FD.PaddingBottom = 8f;
                celdaMedioPago_62FD.HorizontalAlignment = Element.ALIGN_RIGHT;
                operacion_62FD.AddCell(celdaMedioPago_62FD);

                doc_62FD.Add(operacion_62FD);

                PdfPTable detalle_62FD = new PdfPTable(5);
                detalle_62FD.WidthPercentage = 100;
                detalle_62FD.SetWidths(new float[] { 32f, 22f, 12f, 14f, 20f });
                detalle_62FD.HeaderRows = 1;

                AgregarEncabezado_62FD(detalle_62FD, IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_PasajeColPasajero"), azul_62FD, Element.ALIGN_LEFT);
                AgregarEncabezado_62FD(detalle_62FD, IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_PasajeColCodigo"), azul_62FD, Element.ALIGN_LEFT);
                AgregarEncabezado_62FD(detalle_62FD, IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_PasajeColAsiento"), azul_62FD, Element.ALIGN_LEFT);
                AgregarEncabezado_62FD(detalle_62FD, IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_PasajeColClase"), azul_62FD, Element.ALIGN_LEFT);
                AgregarEncabezado_62FD(detalle_62FD, IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_PasajeColPrecio"), azul_62FD, Element.ALIGN_RIGHT);

                foreach (Pasaje_62FD pasaje_62FD in pasajes_62FD)
                {
                    ReservaDetalle_62FD detalleReserva_62FD = reserva_62FD.Detalles_62FD.First(d => d.IdDetalle_62FD == pasaje_62FD.IdDetalle_62FD);

                    AgregarFila_62FD(detalle_62FD, detalleReserva_62FD, pasaje_62FD.CodPasaje_62FD, fuenteTabla_62FD, grisBorde_62FD);
                }

                PdfPCell celdaTotalTexto_62FD = new PdfPCell(new Phrase(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_PasajeTotal"), fuenteTotal_62FD));
                celdaTotalTexto_62FD.Colspan = 4;
                celdaTotalTexto_62FD.BackgroundColor = grisFondo_62FD;
                celdaTotalTexto_62FD.Border = iTextSharp.text.Rectangle.NO_BORDER;
                celdaTotalTexto_62FD.Padding = 8f;
                celdaTotalTexto_62FD.HorizontalAlignment = Element.ALIGN_RIGHT;
                detalle_62FD.AddCell(celdaTotalTexto_62FD);

                PdfPCell celdaTotal_62FD = new PdfPCell(new Phrase(factura_62FD.Total_62FD.ToString("C2"), fuenteTotal_62FD));
                celdaTotal_62FD.BackgroundColor = grisFondo_62FD;
                celdaTotal_62FD.Border = iTextSharp.text.Rectangle.NO_BORDER;
                celdaTotal_62FD.Padding = 8f;
                celdaTotal_62FD.HorizontalAlignment = Element.ALIGN_RIGHT;
                detalle_62FD.AddCell(celdaTotal_62FD);

                doc_62FD.Add(detalle_62FD);

                ColumnText.ShowTextAligned(cb_62FD, Element.ALIGN_CENTER, new Phrase(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_PasajeNotaSistema"), fuenteChica_62FD), doc_62FD.PageSize.Width / 2f, 30f, 0f);

                doc_62FD.Close();
            }
        }

        private void AgregarFila_62FD(PdfPTable tabla_62FD, ReservaDetalle_62FD detalleReserva_62FD, string codPasaje_62FD, iTextSharp.text.Font fuente_62FD, BaseColor colorBorde_62FD)
        {
            string[] valores_62FD = new string[]
            {
                detalleReserva_62FD.NombreCompleto_62FD,
                codPasaje_62FD,
                detalleReserva_62FD.NroAsiento_62FD,
                detalleReserva_62FD.Clase_62FD,
                detalleReserva_62FD.Precio_62FD.ToString("C2")
            };

            for (int i_62FD = 0; i_62FD < valores_62FD.Length; i_62FD++)
            {
                PdfPCell celda_62FD = new PdfPCell(new Phrase(valores_62FD[i_62FD], fuente_62FD));
                celda_62FD.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                celda_62FD.BorderColor = colorBorde_62FD;
                celda_62FD.Padding = 6f;
                celda_62FD.HorizontalAlignment = i_62FD == valores_62FD.Length - 1 ? Element.ALIGN_RIGHT : Element.ALIGN_LEFT;
                tabla_62FD.AddCell(celda_62FD);
            }
        }

        private void AgregarEncabezado_62FD(PdfPTable tabla_62FD, string texto_62FD, BaseColor color_62FD, int alineacion_62FD)
        {
            iTextSharp.text.Font fuente_62FD = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10f, BaseColor.WHITE);
            PdfPCell celda_62FD = new PdfPCell(new Phrase(texto_62FD, fuente_62FD));
            celda_62FD.BackgroundColor = color_62FD;
            celda_62FD.Border = iTextSharp.text.Rectangle.NO_BORDER;
            celda_62FD.Padding = 6f;
            celda_62FD.HorizontalAlignment = alineacion_62FD;
            tabla_62FD.AddCell(celda_62FD);
        }
    }
}
