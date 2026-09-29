using BE_23DB;
using BLL_23DB;
using Services_23DB;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;

namespace GUI_23DB
{
    public partial class GenerarPasaje_62FD : Form, IIdiomaObserver_23DB
    {
        ReservaBLL_62FD reservaBLL_62FD = new ReservaBLL_62FD();
        PasajeBLL_62FD pasajeBLL_62FD = new PasajeBLL_62FD();
        EventoBLL_23DB eventoBLL_23DB = new EventoBLL_23DB();
        private Factura_62FD facturaEmitida_62FD = null;
        private List<Pasaje_62FD> pasajesEmitidos_62FD = new List<Pasaje_62FD>();
        private Reserva_62FD reserva_62FD = null;
        private List<Reserva_62FD> listaReservas_62FD = new List<Reserva_62FD>();
        public GenerarPasaje_62FD()
        {
            InitializeComponent();
            Observer_23DB.ObtenerInstancia_23DB().Suscribir_23DB(this);
            AplicarIdiomaActual_23DB();
            dataGridView1.AutoGenerateColumns = false;
            dataGridView2.AutoGenerateColumns = false;
            CargarCombos_62FD();
            ModoConsulta_62FD();
        }

        private void ModoConsulta_62FD()
        {
            lblEstado.Text = "Modo Consulta";
            ActualizarBarraEstado_62FD("Consulta");

            reserva_62FD = null;

            txtNroVuelo.Clear();
            txtRuta.Clear();
            txtFechaHoraSalid.Clear();
            txtEstadoReserva.Clear();
            dataGridView1.DataSource = null;
            lblMontoTotal.Text = "$ 0,00";
            label4.Text = "";

            LimpiarDatosPago_62FD();
            groupBox2.Enabled = false;
            btnAplicar.Enabled = false;
            btnImprimir.Enabled = false;
            CargarReservasPendientes_62FD();
        }

        private void LimpiarDatosPago_62FD()
        {
            cmbMedioPago.SelectedIndex = -1;
            cmbEFinanciera.SelectedIndex = -1;
            txtNroTarjeta.Clear();
            txtCodSeguridad.Clear();
            dateTimePicker1.Value = DateTime.Now;
        }

        private void ActualizarBarraEstado_62FD(string modo_62FD)
        {
            toolStripStatusLabel1.Text = "Modo: " + modo_62FD;
            toolStripStatusLabel2.Text = "Usuario: " + SessionManager_23DB.ObtenerInstancia_23DB().Login_23DB;
            toolStripStatusLabel3.Text = "Fecha: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm");
        }

        private void CargarCombos_62FD()
        {
            cmbMedioPago.Items.Clear();
            cmbMedioPago.Items.Add("Efectivo");
            cmbMedioPago.Items.Add("Débito");
            cmbMedioPago.Items.Add("Crédito");

            cmbEFinanciera.Items.Clear();
            cmbEFinanciera.Items.Add("Visa");
            cmbEFinanciera.Items.Add("Mastercard");
            cmbEFinanciera.Items.Add("American Express");
        }

        public void ActualizarIdioma_23DB(Dictionary<string, string> configuracion_23DB)
        {
            string formName_23DB = "GenerarPasaje";
            foreach (Control control_23DB in ObtenerTodosControles_23DB(this))
            {
                string clave_23DB = formName_23DB + "_" + control_23DB.Name;
                if (configuracion_23DB.ContainsKey(clave_23DB))
                {
                    control_23DB.Text = configuracion_23DB[clave_23DB];
                }

            }
        }

        private List<Control> ObtenerTodosControles_23DB(Control control_23DB)
        {
            List<Control> lista_23DB = new List<Control>();
            foreach (Control c_23DB in control_23DB.Controls)
            {
                lista_23DB.Add(c_23DB);
                lista_23DB.AddRange(ObtenerTodosControles_23DB(c_23DB));
            }
            return lista_23DB;
        }

        public void AplicarIdiomaActual_23DB()
        {
            string idiomaActual_23DB = SessionManager_23DB.ObtenerInstancia_23DB().UltimoIdioma_23DB;
            if (!string.IsNullOrEmpty(idiomaActual_23DB))
            {
                IdiomaBLL_23DB idiomaBLL_23DB = new IdiomaBLL_23DB();
                ActualizarIdioma_23DB(idiomaBLL_23DB.CargarConfiguracion_23DB(idiomaActual_23DB));
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtCodReserva.Text))
                {
                    MessageBox.Show("Debe ingresar un código de reserva.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                reserva_62FD = reservaBLL_62FD.ObtenerReserva_62FD(txtCodReserva.Text);

                if (reserva_62FD == null)
                {
                    MessageBox.Show("Reserva no válida o inexistente.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    ModoConsulta_62FD();
                    return;
                }

                if (reserva_62FD.EstadoReserva_62FD != "Pendiente de Pago")
                {
                    MessageBox.Show("La reserva no se encuentra pendiente de pago.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    ModoConsulta_62FD();
                    return;
                }

                CargarDatosReserva_62FD();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al buscar la reserva: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarDatosReserva_62FD()
        {
            txtNroVuelo.Text = reserva_62FD.NroVuelo_62FD;
            txtEstadoReserva.Text = reserva_62FD.EstadoReserva_62FD;
            txtRuta.Text = reserva_62FD.Ruta_62FD;
            txtFechaHoraSalid.Text = reserva_62FD.FechaHoraSalida_62FD.ToString("dd/MM/yyyy HH:mm");

            txtNroVuelo.ReadOnly = true;
            txtEstadoReserva.ReadOnly = true;
            txtRuta.ReadOnly = true;
            txtFechaHoraSalid.ReadOnly = true;

            dataGridView1.DataSource = null;
            dataGridView1.DataSource = reserva_62FD.Detalles_62FD;

            lblMontoTotal.Text = reserva_62FD.MontoTotal_62FD.ToString("C2");

            ModoCobro_62FD();
        }

        private void ModoCobro_62FD()
        {
            lblEstado.Text = "Modo Cobro";
            ActualizarBarraEstado_62FD("Cobro");

            groupBox2.Enabled = true;
            btnAplicar.Enabled = true;
            btnImprimir.Enabled = false;
        }

        private void cmbMedioPago_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool esTarjeta_62FD = cmbMedioPago.Text != "Efectivo" && cmbMedioPago.SelectedIndex >= 0;

            cmbEFinanciera.Enabled = esTarjeta_62FD;
            txtNroTarjeta.Enabled = esTarjeta_62FD;
            txtCodSeguridad.Enabled = esTarjeta_62FD;
            dateTimePicker1.Enabled = esTarjeta_62FD;

            if (!esTarjeta_62FD)
            {
                cmbEFinanciera.SelectedIndex = -1;
                txtNroTarjeta.Clear();
                txtCodSeguridad.Clear();
            }
        }

        private void btnAplicar_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbMedioPago.SelectedIndex < 0)
                {
                    MessageBox.Show("Debe seleccionar un medio de pago.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (reserva_62FD.EstadoReserva_62FD != "Pendiente de Pago")
                {
                    MessageBox.Show("La reserva no se encuentra pendiente de pago.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    ModoConsulta_62FD();
                    return;
                }

                Pago_62FD pago_62FD = new Pago_62FD();
                pago_62FD.CodReserva_62FD = reserva_62FD.CodReserva_62FD;
                pago_62FD.MedioPago_62FD = cmbMedioPago.Text;
                pago_62FD.MontoTotal_62FD = reserva_62FD.MontoTotal_62FD;

                if (cmbMedioPago.Text != "Efectivo")
                {
                    pago_62FD.EntidadFinanciera_62FD = cmbEFinanciera.Text;
                    pago_62FD.NumeroTarjeta_62FD = txtNroTarjeta.Text.Replace("-", "");
                    pago_62FD.CodigoSeguridad_62FD = txtCodSeguridad.Text;
                    pago_62FD.FechaVencimiento_62FD = dateTimePicker1.Value;
                }


                Factura_62FD factura_62FD = pasajeBLL_62FD.GenerarPasaje_62FD(pago_62FD, reserva_62FD);

                eventoBLL_23DB.RegistrarEvento_23DB(SessionManager_23DB.ObtenerInstancia_23DB().DNI_23DB, "Pasajes", "Generar Pasaje", 2);

                label4.Text = "Factura: " + factura_62FD.NroFactura_62FD; // label4 es el de Numero de Factura
                txtEstadoReserva.Text = "Confirmada";

                MessageBox.Show("Pasajes generados correctamente. Factura: " + factura_62FD.NroFactura_62FD, "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);

                facturaEmitida_62FD = factura_62FD;
                pasajesEmitidos_62FD = pasajeBLL_62FD.PasajesEmitidos_62FD;
                ModoEmitido_62FD();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al generar el pasaje: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ModoConsulta_62FD();
            }
        }

        private void ModoEmitido_62FD()
        {
            lblEstado.Text = "Pasajes emitidos";
            ActualizarBarraEstado_62FD("Pasajes emitidos");

            groupBox2.Enabled = false;
            btnAplicar.Enabled = false;
            btnImprimir.Enabled = true;
            btnBuscar.Enabled = false;
        }

        private void GenerarPasaje_62FD_FormClosing(object sender, FormClosingEventArgs e)
        {
            Observer_23DB.ObtenerInstancia_23DB().Desuscribir_23DB(this);
        }

        private void btnImprimir_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialogo_62FD = new SaveFileDialog();
            dialogo_62FD.Filter = "Archivos PDF|*.pdf";
            dialogo_62FD.FileName = "Reserva_" + reserva_62FD.CodReserva_62FD + ".pdf";

            if (dialogo_62FD.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            try
            {
                GenerarPDF_62FD(dialogo_62FD.FileName);
                MessageBox.Show("Documento generado correctamente.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex_62FD)
            {
                MessageBox.Show("Error al generar el documento: " + ex_62FD.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void GenerarPDF_62FD(string ruta_62FD)
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
                foreach (Pasaje_62FD pasaje_62FD in pasajesEmitidos_62FD)
                {
                    if (!primeraPagina_62FD)
                    {
                        doc_62FD.NewPage();
                    }
                    primeraPagina_62FD = false;

                    ReservaDetalle_62FD detalleReserva_62FD = reserva_62FD.Detalles_62FD
                        .First(d => d.IdDetalle_62FD == pasaje_62FD.IdDetalle_62FD);

                    PdfPTable banda_62FD = new PdfPTable(2);
                    banda_62FD.WidthPercentage = 100;
                    banda_62FD.SpacingAfter = 24f;

                    PdfPCell celdaMarca_62FD = new PdfPCell(new Phrase("AeroManager", fuenteMarca_62FD));
                    celdaMarca_62FD.BackgroundColor = azul_62FD;
                    celdaMarca_62FD.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    celdaMarca_62FD.Padding = 14f;
                    celdaMarca_62FD.VerticalAlignment = Element.ALIGN_MIDDLE;
                    banda_62FD.AddCell(celdaMarca_62FD);

                    PdfPCell celdaTipo_62FD = new PdfPCell(new Phrase("PASAJE AÉREO / BOARDING PASS", fuenteBanda_62FD));
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
                    celdaVuelo_62FD.AddElement(new Paragraph("VUELO", fuenteEtiqueta_62FD));
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
                    celdaRuta_62FD.AddElement(new Paragraph("RUTA", fuenteEtiqueta_62FD));
                    celdaRuta_62FD.AddElement(parrafoRuta_62FD);
                    vuelo_62FD.AddCell(celdaRuta_62FD);

                    doc_62FD.Add(vuelo_62FD);

                    string[,] datosPasaje_62FD = new string[,]
                    {
                        { "PASAJERO", detalleReserva_62FD.NombreCompleto_62FD, "DOCUMENTO", detalleReserva_62FD.NumeroDocumento_62FD },
                        { "FECHA DE SALIDA", reserva_62FD.FechaHoraSalida_62FD.ToString("dd/MM/yyyy"), "HORA DE SALIDA", reserva_62FD.FechaHoraSalida_62FD.ToString("HH:mm") },
                        { "ASIENTO", detalleReserva_62FD.NroAsiento_62FD, "CLASE", detalleReserva_62FD.Clase_62FD }
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

                    Paragraph etiquetaCodigo_62FD = new Paragraph("CÓDIGO DE PASAJE", fuenteEtiqueta_62FD);
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

                    ColumnText.ShowTextAligned(cb_62FD, Element.ALIGN_CENTER, new Phrase("Conserve este comprobante", fuenteChica_62FD), anchoPagina_62FD / 2f, yCorte_62FD - 14f, 0f);
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
                celdaComprobante_62FD.AddElement(new Paragraph("FACTURA", fuenteFactura_62FD));
                celdaComprobante_62FD.AddElement(new Paragraph("N° " + facturaEmitida_62FD.NroFactura_62FD, fuenteTextoNegrita_62FD));
                celdaComprobante_62FD.AddElement(new Paragraph("Fecha de emisión: " + facturaEmitida_62FD.FechaEmision_62FD.ToString("dd/MM/yyyy HH:mm"), fuenteTexto_62FD));
                cabecera_62FD.AddCell(celdaComprobante_62FD);

                doc_62FD.Add(cabecera_62FD);

                PdfPTable operacion_62FD = new PdfPTable(2);
                operacion_62FD.WidthPercentage = 100;
                operacion_62FD.SpacingAfter = 16f;

                Phrase fraseReserva_62FD = new Phrase();
                fraseReserva_62FD.Add(new Chunk("Código de reserva: ", fuenteTextoNegrita_62FD));
                fraseReserva_62FD.Add(new Chunk(reserva_62FD.CodReserva_62FD, fuenteTabla_62FD));
                PdfPCell celdaReserva_62FD = new PdfPCell(fraseReserva_62FD);
                celdaReserva_62FD.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                celdaReserva_62FD.BorderColor = grisBorde_62FD;
                celdaReserva_62FD.PaddingBottom = 8f;
                operacion_62FD.AddCell(celdaReserva_62FD);

                Phrase fraseMedioPago_62FD = new Phrase();
                fraseMedioPago_62FD.Add(new Chunk("Medio de pago: ", fuenteTextoNegrita_62FD));
                fraseMedioPago_62FD.Add(new Chunk(cmbMedioPago.Text, fuenteTabla_62FD));
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

                AgregarEncabezado_62FD(detalle_62FD, "Pasajero", azul_62FD, Element.ALIGN_LEFT);
                AgregarEncabezado_62FD(detalle_62FD, "Código de Pasaje", azul_62FD, Element.ALIGN_LEFT);
                AgregarEncabezado_62FD(detalle_62FD, "Asiento", azul_62FD, Element.ALIGN_LEFT);
                AgregarEncabezado_62FD(detalle_62FD, "Clase", azul_62FD, Element.ALIGN_LEFT);
                AgregarEncabezado_62FD(detalle_62FD, "Precio", azul_62FD, Element.ALIGN_RIGHT);

                foreach (Pasaje_62FD pasaje_62FD in pasajesEmitidos_62FD)
                {
                    ReservaDetalle_62FD detalleReserva_62FD = reserva_62FD.Detalles_62FD.First(d => d.IdDetalle_62FD == pasaje_62FD.IdDetalle_62FD);

                    AgregarFila_62FD(detalle_62FD, detalleReserva_62FD, pasaje_62FD.CodPasaje_62FD, fuenteTabla_62FD, grisBorde_62FD);
                }

                PdfPCell celdaTotalTexto_62FD = new PdfPCell(new Phrase("TOTAL", fuenteTotal_62FD));
                celdaTotalTexto_62FD.Colspan = 4;
                celdaTotalTexto_62FD.BackgroundColor = grisFondo_62FD;
                celdaTotalTexto_62FD.Border = iTextSharp.text.Rectangle.NO_BORDER;
                celdaTotalTexto_62FD.Padding = 8f;
                celdaTotalTexto_62FD.HorizontalAlignment = Element.ALIGN_RIGHT;
                detalle_62FD.AddCell(celdaTotalTexto_62FD);

                PdfPCell celdaTotal_62FD = new PdfPCell(new Phrase(facturaEmitida_62FD.Total_62FD.ToString("C2"), fuenteTotal_62FD));
                celdaTotal_62FD.BackgroundColor = grisFondo_62FD;
                celdaTotal_62FD.Border = iTextSharp.text.Rectangle.NO_BORDER;
                celdaTotal_62FD.Padding = 8f;
                celdaTotal_62FD.HorizontalAlignment = Element.ALIGN_RIGHT;
                detalle_62FD.AddCell(celdaTotal_62FD);

                doc_62FD.Add(detalle_62FD);

                ColumnText.ShowTextAligned(cb_62FD, Element.ALIGN_CENTER,
                    new Phrase("Documento no válido como comprobante fiscal - Sistema académico", fuenteChica_62FD),
                    doc_62FD.PageSize.Width / 2f, 30f, 0f);

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
        private void btnCancelar_Click(object sender, EventArgs e)
        {
            ModoConsulta_62FD();
            txtCodReserva.Clear();
            btnBuscar.Enabled = true;
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void GenerarPasaje_62FD_Load(object sender, EventArgs e)
        {
            CargarReservasPendientes_62FD();

        }

        private void CargarReservasPendientes_62FD()
        {
            try
            {
                listaReservas_62FD = reservaBLL_62FD.ObtenerReservasPendientes_62FD();

                dataGridView2.DataSource = null;
                dataGridView2.DataSource = listaReservas_62FD;

                if (listaReservas_62FD.Count == 0)
                {
                    MessageBox.Show("No hay reservas pendientes de pago.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar las reservas: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }

            try
            {
                Reserva_62FD seleccionada_62FD = (Reserva_62FD)dataGridView2.Rows[e.RowIndex].DataBoundItem;

                txtCodReserva.Text = seleccionada_62FD.CodReserva_62FD;
                reserva_62FD = reservaBLL_62FD.ObtenerReserva_62FD(seleccionada_62FD.CodReserva_62FD);

                CargarDatosReserva_62FD();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar la reserva: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
