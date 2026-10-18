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
    public partial class CertificarIntervencion_62FD : Form, IIdiomaObserver_23DB
    {
        CertificacionBLL_62FD certificacionBLL_62FD = new CertificacionBLL_62FD();
        IntervencionBLL_62FD intervencionBLL_62FD = new IntervencionBLL_62FD();
        AeronaveBLL_62FD aeronaveBLL_62FD = new AeronaveBLL_62FD();
        RepuestoBLL_62FD repuestoBLL_62FD = new RepuestoBLL_62FD();
        UsuarioBLL_23DB usuarioBLL_23DB = new UsuarioBLL_23DB();
        EventoBLL_23DB eventoBLL_23DB = new EventoBLL_23DB();

        private Intervencion_62FD intervencionSeleccionada_62FD = null;
        private Certificacion_62FD certificacionRegistrada_62FD = null;
        private List<Intervencion_62FD> listaPendientes_62FD = new List<Intervencion_62FD>();
        private List<Tarea_62FD> listaTareas_62FD = new List<Tarea_62FD>();
        private string columnaOrdenPendientes_62FD = null;
        private bool ordenAscendentePendientes_62FD = true;
        private string columnaOrdenTareas_62FD = null;
        private bool ordenAscendenteTareas_62FD = true;
        private string claveModo_62FD = null;
        private DateTime fechaModo_62FD = DateTime.Now;
        public CertificarIntervencion_62FD()
        {
            InitializeComponent();
            Observer_23DB.ObtenerInstancia_23DB().Suscribir_23DB(this);
            AplicarIdiomaActual_23DB();

            dataGridView1.AutoGenerateColumns = false;
            dataGridView2.AutoGenerateColumns = false;
            ConfigurarControles_62FD();
        }

        private void ConfigurarControles_62FD()
        {
            cmbResultadoInspeccion.DropDownStyle = ComboBoxStyle.DropDownList;

            // Largos iguales a las columnas y a los parámetros de InsertarCertificacion_62FD
            txtCodigoHabilitante.MaxLength = 50;
            txtObservaciones.MaxLength = 500;
            // Es multilínea: Enter tiene que hacer salto de línea y no disparar el botón Aplicar
            txtObservaciones.AcceptsReturn = true;

            ConfigurarGrilla_62FD(dataGridView1);
            ConfigurarGrilla_62FD(dataGridView2);

            colIdIntervencion.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            colIdIntervencion.FillWeight = 50;
            colIdOrden.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            colIdOrden.FillWeight = 50;
            colFechaHoraFin.DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
            colFechaHoraFin.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colFechaHoraFin.DefaultCellStyle.NullValue = "-";
            colTotalHoras.DefaultCellStyle.Format = "N2";
            colTotalHoras.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            colDescripcionTarea.FillWeight = 200;
            colObservacionesTarea.FillWeight = 200;
            colHorasTarea.DefaultCellStyle.Format = "N2";
            colHorasTarea.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            colRepuestosTarea.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dataGridView1.Paint += Grilla_Paint_62FD;
            dataGridView2.Paint += Grilla_Paint_62FD;
            dataGridView1.ColumnHeaderMouseClick += dataGridView1_ColumnHeaderMouseClick;
            dataGridView2.ColumnHeaderMouseClick += dataGridView2_ColumnHeaderMouseClick;
            this.Shown += (s_62FD, e_62FD) =>
            {
                LimpiarSeleccionGrilla_62FD(dataGridView1);
                LimpiarSeleccionGrilla_62FD(dataGridView2);
            };

            this.AcceptButton = btnAplicar;
            this.CancelButton = btnSalir;
        }

        public void ActualizarIdioma_23DB(Dictionary<string, string> configuracion_23DB)
        {
            Traductor_23DB.AplicarIdioma_23DB(this, configuracion_23DB, statusStrip1);
            MostrarTextosEstado_62FD();
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

        private void CertificarIntervencion_62FD_Load(object sender, EventArgs e)
        {
            CargarCombos_62FD();
            CargarGrilla_62FD();
            LimpiarSeleccion_62FD();
            ActualizarBarraEstado_62FD("Msg_NombreModoConsulta");
            this.ActiveControl = dataGridView1;
        }

        private void CargarCombos_62FD()
        {
            cmbResultadoInspeccion.Items.Clear();
            cmbResultadoInspeccion.Items.Add("Satisfactorio");
            cmbResultadoInspeccion.Items.Add("No Satisfactorio");
            cmbResultadoInspeccion.SelectedIndex = -1;

            dtpFechaHoraCertificacion.Enabled = false;

            txtNroIntervencion.ReadOnly = true;
            txtNroOrden.ReadOnly = true;
            txtMatricula.ReadOnly = true;
            txtFechaHoraFin.ReadOnly = true;
            txtHorasInsumidas.ReadOnly = true;
        }

        private void CargarGrilla_62FD()
        {
            try
            {
                listaPendientes_62FD = certificacionBLL_62FD.ObtenerPendientes_62FD();
                columnaOrdenPendientes_62FD = null;

                dataGridView1.DataSource = null;
                dataGridView1.DataSource = listaPendientes_62FD;
                Traductor_23DB.TraducirGrilla_23DB(dataGridView1, "CertificarIntervencion");
                LimpiarSeleccionGrilla_62FD(dataGridView1);
            }
            catch (Exception ex_62FD)
            {
                MessageBox.Show(string.Format(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_ErrorRecuperarPendientesCertificacion"), ex_62FD.Message), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }

            try
            {
                intervencionSeleccionada_62FD = (Intervencion_62FD)dataGridView1.Rows[e.RowIndex].DataBoundItem;

                txtNroIntervencion.Text = intervencionSeleccionada_62FD.IdIntervencion_62FD.ToString();
                txtNroOrden.Text = intervencionSeleccionada_62FD.IdOrden_62FD.ToString();
                txtMatricula.Text = intervencionSeleccionada_62FD.Matricula_62FD;
                txtHorasInsumidas.Text = intervencionSeleccionada_62FD.totalHorasInsumidas_62FD.ToString("N2");

                if (intervencionSeleccionada_62FD.fechaHoraFin_62FD.HasValue)
                {
                    txtFechaHoraFin.Text = intervencionSeleccionada_62FD.fechaHoraFin_62FD.Value.ToString("dd/MM/yyyy HH:mm");
                }
                else
                {
                    txtFechaHoraFin.Clear();
                }

                CargarGrillaTareas_62FD();

                dtpFechaHoraCertificacion.Value = DateTime.Now;
                certificacionRegistrada_62FD = null;
                btnImprimir.Enabled = false;

                ActualizarBarraEstado_62FD("Msg_NombreModoCertificacion");
            }
            catch (Exception ex_62FD)
            {
                MessageBox.Show(string.Format(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_ErrorRecuperarTareas"), ex_62FD.Message), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarGrillaTareas_62FD()
        {
            listaTareas_62FD = intervencionBLL_62FD.ObtenerTareas_62FD(intervencionSeleccionada_62FD.IdIntervencion_62FD);
            columnaOrdenTareas_62FD = null;

            dataGridView2.DataSource = null;
            dataGridView2.DataSource = listaTareas_62FD;
            Traductor_23DB.TraducirGrilla_23DB(dataGridView2, "CertificarIntervencion");
            LimpiarSeleccionGrilla_62FD(dataGridView2);
        }

        private void btnAplicar_Click(object sender, EventArgs e)
        {
            // Evita que un doble clic intente certificar dos veces
            btnAplicar.Enabled = false;

            try
            {
                if (intervencionSeleccionada_62FD == null)
                {
                    MessageBox.Show(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_SeleccioneIntervencion"), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Informacion"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtCodigoHabilitante.Text) || cmbResultadoInspeccion.SelectedIndex < 0)
                {
                    MessageBox.Show(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_CompletarDatosCertificacion"), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Informacion"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                string estadoResultante_62FD = cmbResultadoInspeccion.Text == "Satisfactorio" ? "En Servicio" : "Fuera de Servicio";

                DialogResult confirmacion_62FD = MessageBox.Show(string.Format(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_ConfirmarCertificacion"), intervencionSeleccionada_62FD.IdIntervencion_62FD, intervencionSeleccionada_62FD.Matricula_62FD, cmbResultadoInspeccion.Text, estadoResultante_62FD), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Confirmar"), MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (confirmacion_62FD != DialogResult.Yes)
                {
                    return;
                }

                Certificacion_62FD certificacion_62FD = new Certificacion_62FD();
                certificacion_62FD.IdIntervencion_62FD = intervencionSeleccionada_62FD.IdIntervencion_62FD;
                certificacion_62FD.codHabilitante_62FD = txtCodigoHabilitante.Text.Trim();
                certificacion_62FD.resultadoInspeccion_62FD = cmbResultadoInspeccion.Text;
                certificacion_62FD.Observaciones_62FD = txtObservaciones.Text.Trim();

                int resultado_62FD = certificacionBLL_62FD.CertificarIntervencion_62FD(certificacion_62FD, intervencionSeleccionada_62FD.IdOrden_62FD);

                if (resultado_62FD == CertificacionBLL_62FD.YaCertificada_62FD)
                {
                    MessageBox.Show(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_IntervencionYaCertificada"), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Advertencia"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (resultado_62FD == CertificacionBLL_62FD.Autocertificacion_62FD)
                {
                    MessageBox.Show(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_AutocertificacionNoPermitida"), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Advertencia"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (resultado_62FD == CertificacionBLL_62FD.DatoModificado_62FD)
                {
                    MessageBox.Show(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_DatoModificado"), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Advertencia"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    CargarGrilla_62FD();
                    LimpiarSeleccion_62FD();
                    return;
                }

                // A partir de acá la certificación quedó registrada (con o sin la aeronave actualizada)
                RegistrarEventoBitacora_62FD("Certificar Intervención", 3);
                certificacionRegistrada_62FD = certificacion_62FD;
                dtpFechaHoraCertificacion.Value = certificacion_62FD.fechaHoraCertificacion_62FD;

                if (resultado_62FD == CertificacionBLL_62FD.AeronaveNoActualizada_62FD)
                {
                    MessageBox.Show(string.Format(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_CertificacionSinActualizarAeronave"), certificacion_62FD.IdIntervencion_62FD), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Advertencia"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else if (certificacion_62FD.resultadoInspeccion_62FD == "Satisfactorio")
                {
                    MessageBox.Show(string.Format(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_CertificacionSatisfactoria"), certificacion_62FD.IdIntervencion_62FD), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Informacion"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show(string.Format(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_CertificacionNoSatisfactoria"), certificacion_62FD.IdIntervencion_62FD), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Advertencia"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                btnImprimir.Enabled = true;
                CargarGrilla_62FD();
            }
            catch (Exception ex_62FD)
            {
                MessageBox.Show(string.Format(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_ErrorRegistrarCertificacion"), ex_62FD.Message), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnAplicar.Enabled = true;
            }
        }

        // La Bitácora va en su propio try/catch: si falla, la operación de negocio ya está hecha y no se informa como error
        private void RegistrarEventoBitacora_62FD(string evento_62FD, int criticidad_62FD)
        {
            try
            {
                eventoBLL_23DB.RegistrarEvento_23DB(SessionManager_23DB.ObtenerInstancia_23DB().DNI_23DB, "Mantenimiento", evento_62FD, criticidad_62FD);
            }
            catch (Exception ex_62FD)
            {
                MessageBox.Show(string.Format(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_OperacionSinBitacora"), ex_62FD.Message), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Advertencia"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnImprimir_Click(object sender, EventArgs e)
        {
            if (certificacionRegistrada_62FD == null || intervencionSeleccionada_62FD == null)
            {
                MessageBox.Show(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_RegistreCertificacionAntesImprimir"), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Informacion"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                string carpeta_62FD = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "AeroManager", "Certificados");

                if (!Directory.Exists(carpeta_62FD))
                {
                    Directory.CreateDirectory(carpeta_62FD);
                }

                string ruta_62FD = Path.Combine(carpeta_62FD, "Certificado_" + certificacionRegistrada_62FD.IdIntervencion_62FD + ".pdf");

                GenerarCertificadoPDF_62FD(ruta_62FD);

                System.Diagnostics.Process.Start(ruta_62FD);
            }
            catch (Exception ex_62FD)
            {
                MessageBox.Show(string.Format(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_ErrorGenerarDocumento"), ex_62FD.Message), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void GenerarCertificadoPDF_62FD(string ruta_62FD)
        {
            // Datos que el formulario no tiene cargados se recuperan con las BLL existentes
            Aeronave_62FD aeronave_62FD = aeronaveBLL_62FD.ObtenerAeronaves_62FD(intervencionSeleccionada_62FD.Matricula_62FD, string.Empty, string.Empty, string.Empty, false)
                .Find(a_62FD => a_62FD.Matricula_62FD == intervencionSeleccionada_62FD.Matricula_62FD);
            OrdenTrabajo_62FD orden_62FD = intervencionBLL_62FD.ObtenerOrdenPorId_62FD(intervencionSeleccionada_62FD.IdOrden_62FD);
            List<Tarea_62FD> tareas_62FD = intervencionBLL_62FD.ObtenerTareas_62FD(intervencionSeleccionada_62FD.IdIntervencion_62FD);
            Usuario_23DB inspector_62FD = usuarioBLL_23DB.ObtenerUsuarioPorLogin_23DB(SessionManager_23DB.ObtenerInstancia_23DB().Login_23DB);

            bool apto_62FD = certificacionRegistrada_62FD.resultadoInspeccion_62FD == "Satisfactorio";

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

                PdfPCell celdaNumero_62FD = new PdfPCell(new Phrase(string.Format(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_CertificadoNumero"), certificacionRegistrada_62FD.IdIntervencion_62FD), fuenteDato_62FD));
                celdaNumero_62FD.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                celdaNumero_62FD.BorderColor = grisBorde_62FD;
                celdaNumero_62FD.PaddingBottom = 8f;
                identificacion_62FD.AddCell(celdaNumero_62FD);

                PdfPCell celdaFecha_62FD = new PdfPCell(new Phrase(string.Format(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_CertificadoFechaHora"), certificacionRegistrada_62FD.fechaHoraCertificacion_62FD.ToString("dd/MM/yyyy HH:mm")), fuenteTexto_62FD));
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
                AgregarDato_62FD(datosAeronave_62FD, IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_CertificadoMatricula"), intervencionSeleccionada_62FD.Matricula_62FD, fuenteEtiqueta_62FD, fuenteDato_62FD);
                AgregarDato_62FD(datosAeronave_62FD, IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_CertificadoModelo"), aeronave_62FD != null ? aeronave_62FD.Modelo_62FD : "-", fuenteEtiqueta_62FD, fuenteDato_62FD);
                AgregarDato_62FD(datosAeronave_62FD, IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_CertificadoFabricante"), aeronave_62FD != null ? aeronave_62FD.Fabricante_62FD : "-", fuenteEtiqueta_62FD, fuenteDato_62FD);
                doc_62FD.Add(datosAeronave_62FD);

                // Datos de la intervención
                AgregarSeccion_62FD(doc_62FD, IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_CertificadoSeccionIntervencion"), fuenteSeccion_62FD, azul_62FD);

                string fechaFin_62FD = intervencionSeleccionada_62FD.fechaHoraFin_62FD.HasValue ? intervencionSeleccionada_62FD.fechaHoraFin_62FD.Value.ToString("dd/MM/yyyy HH:mm") : "-";

                PdfPTable datosIntervencion_62FD = new PdfPTable(3);
                datosIntervencion_62FD.WidthPercentage = 100;
                AgregarDato_62FD(datosIntervencion_62FD, IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_CertificadoNroOrden"), intervencionSeleccionada_62FD.IdOrden_62FD.ToString(), fuenteEtiqueta_62FD, fuenteDato_62FD);
                AgregarDato_62FD(datosIntervencion_62FD, IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_CertificadoTipoIntervencion"), orden_62FD != null ? orden_62FD.TipoIntervencion_62FD : "-", fuenteEtiqueta_62FD, fuenteDato_62FD);
                AgregarDato_62FD(datosIntervencion_62FD, IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_CertificadoTotalHoras"), intervencionSeleccionada_62FD.totalHorasInsumidas_62FD.ToString("N2"), fuenteEtiqueta_62FD, fuenteDato_62FD);
                AgregarDato_62FD(datosIntervencion_62FD, IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_CertificadoFechaInicio"), intervencionSeleccionada_62FD.fechaHoraInicio_62FD.ToString("dd/MM/yyyy HH:mm"), fuenteEtiqueta_62FD, fuenteDato_62FD);
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

                string observaciones_62FD = string.IsNullOrWhiteSpace(certificacionRegistrada_62FD.Observaciones_62FD)
                    ? IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_CertificadoSinObservaciones")
                    : certificacionRegistrada_62FD.Observaciones_62FD;
                doc_62FD.Add(new Paragraph(observaciones_62FD, fuenteTexto_62FD));

                // Pie con datos del inspector y línea para firma
                string nombreInspector_62FD = inspector_62FD != null
                    ? inspector_62FD.Apellido_23DB + ", " + inspector_62FD.Nombre_23DB
                    : SessionManager_23DB.ObtenerInstancia_23DB().Login_23DB;

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
                fraseCodigo_62FD.Add(new Chunk(certificacionRegistrada_62FD.codHabilitante_62FD, fuenteTexto_62FD));
                celdaInspector_62FD.AddElement(new Paragraph(fraseCodigo_62FD));
                Phrase fraseInspector_62FD = new Phrase();
                fraseInspector_62FD.Add(new Chunk(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_CertificadoInspector") + " ", fuenteTextoNegrita_62FD));
                fraseInspector_62FD.Add(new Chunk(nombreInspector_62FD, fuenteTexto_62FD));
                celdaInspector_62FD.AddElement(new Paragraph(fraseInspector_62FD));
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

                ColumnText.ShowTextAligned(cb_62FD, Element.ALIGN_CENTER,
                    new Phrase(string.Format(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_CertificadoNotaSistema"), DateTime.Now.ToString("dd/MM/yyyy HH:mm")), fuenteChica_62FD),
                    doc_62FD.PageSize.Width / 2f, 30f, 0f);

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

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            LimpiarSeleccion_62FD();
            ActualizarBarraEstado_62FD("Msg_NombreModoConsulta");
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void LimpiarSeleccion_62FD()
        {
            intervencionSeleccionada_62FD = null;
            certificacionRegistrada_62FD = null;

            txtNroIntervencion.Clear();
            txtNroOrden.Clear();
            txtMatricula.Clear();
            txtFechaHoraFin.Clear();
            txtHorasInsumidas.Clear();
            txtCodigoHabilitante.Clear();
            txtObservaciones.Clear();
            cmbResultadoInspeccion.SelectedIndex = -1;
            dtpFechaHoraCertificacion.Value = DateTime.Now;

            dataGridView2.DataSource = null;
            listaTareas_62FD = new List<Tarea_62FD>();
            LimpiarSeleccionGrilla_62FD(dataGridView1);

            btnImprimir.Enabled = false;
        }

        private void dataGridView1_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridViewColumn columna_62FD = dataGridView1.Columns[e.ColumnIndex];
            ordenAscendentePendientes_62FD = columnaOrdenPendientes_62FD == columna_62FD.DataPropertyName ? !ordenAscendentePendientes_62FD : true;
            columnaOrdenPendientes_62FD = columna_62FD.DataPropertyName;

            listaPendientes_62FD = Ordenar_62FD(listaPendientes_62FD, columnaOrdenPendientes_62FD, ordenAscendentePendientes_62FD);
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = listaPendientes_62FD;
            Traductor_23DB.TraducirGrilla_23DB(dataGridView1, "CertificarIntervencion");
            LimpiarSeleccionGrilla_62FD(dataGridView1);

            // El glifo se pierde al reenlazar, por eso se pone después
            dataGridView1.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection = ordenAscendentePendientes_62FD ? SortOrder.Ascending : SortOrder.Descending;
        }

        private void dataGridView2_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridViewColumn columna_62FD = dataGridView2.Columns[e.ColumnIndex];
            ordenAscendenteTareas_62FD = columnaOrdenTareas_62FD == columna_62FD.DataPropertyName ? !ordenAscendenteTareas_62FD : true;
            columnaOrdenTareas_62FD = columna_62FD.DataPropertyName;

            listaTareas_62FD = Ordenar_62FD(listaTareas_62FD, columnaOrdenTareas_62FD, ordenAscendenteTareas_62FD);
            dataGridView2.DataSource = null;
            dataGridView2.DataSource = listaTareas_62FD;
            Traductor_23DB.TraducirGrilla_23DB(dataGridView2, "CertificarIntervencion");
            LimpiarSeleccionGrilla_62FD(dataGridView2);

            // El glifo se pierde al reenlazar, por eso se pone después
            dataGridView2.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection = ordenAscendenteTareas_62FD ? SortOrder.Ascending : SortOrder.Descending;
        }

        private List<T> Ordenar_62FD<T>(List<T> lista_62FD, string propiedad_62FD, bool ascendente_62FD)
        {
            System.Reflection.PropertyInfo prop_62FD = typeof(T).GetProperty(propiedad_62FD);
            if (prop_62FD == null) return lista_62FD;
            return ascendente_62FD
                ? lista_62FD.OrderBy(x_62FD => prop_62FD.GetValue(x_62FD, null)).ToList()
                : lista_62FD.OrderByDescending(x_62FD => prop_62FD.GetValue(x_62FD, null)).ToList();
        }

        private void ConfigurarGrilla_62FD(DataGridView grilla_62FD)
        {
            // Comportamiento
            grilla_62FD.ReadOnly = true;
            grilla_62FD.AllowUserToAddRows = false;
            grilla_62FD.AllowUserToDeleteRows = false;
            grilla_62FD.AllowUserToResizeRows = false;
            grilla_62FD.AllowUserToOrderColumns = false;
            grilla_62FD.MultiSelect = false;
            grilla_62FD.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grilla_62FD.StandardTab = true;
            grilla_62FD.RowHeadersVisible = false;
            grilla_62FD.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Aspecto general
            grilla_62FD.BackgroundColor = Color.White;
            grilla_62FD.BorderStyle = BorderStyle.None;
            grilla_62FD.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            grilla_62FD.GridColor = Color.FromArgb(220, 220, 220);
            grilla_62FD.RowTemplate.Height = 26;

            // Encabezados (mismo azul que el certificado PDF)
            grilla_62FD.EnableHeadersVisualStyles = false;
            grilla_62FD.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            grilla_62FD.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            grilla_62FD.ColumnHeadersHeight = 32;
            grilla_62FD.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(31, 78, 121);
            grilla_62FD.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            grilla_62FD.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(31, 78, 121);
            grilla_62FD.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font(grilla_62FD.Font, FontStyle.Bold);
            grilla_62FD.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.False;

            // Filas
            grilla_62FD.DefaultCellStyle.Padding = new Padding(4, 0, 4, 0);
            grilla_62FD.DefaultCellStyle.SelectionBackColor = Color.FromArgb(204, 224, 245);
            grilla_62FD.DefaultCellStyle.SelectionForeColor = Color.Black;
            grilla_62FD.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(242, 246, 250);

            foreach (DataGridViewColumn columna_62FD in grilla_62FD.Columns)
            {
                columna_62FD.SortMode = DataGridViewColumnSortMode.Programmatic;
                columna_62FD.MinimumWidth = 60;
            }
        }

        private void LimpiarSeleccionGrilla_62FD(DataGridView grilla_62FD)
        {
            grilla_62FD.ClearSelection();
            grilla_62FD.CurrentCell = null;
            grilla_62FD.Invalidate();
        }

        private void Grilla_Paint_62FD(object sender, PaintEventArgs e)
        {
            DataGridView grilla_62FD = (DataGridView)sender;
            if (grilla_62FD.Rows.Count > 0) return;

            System.Drawing.Rectangle area_62FD = new System.Drawing.Rectangle(0, grilla_62FD.ColumnHeadersHeight,
                grilla_62FD.ClientSize.Width, grilla_62FD.ClientSize.Height - grilla_62FD.ColumnHeadersHeight);
            TextRenderer.DrawText(e.Graphics, IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_GrillaSinDatos"),
                grilla_62FD.Font, area_62FD, Color.Gray,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.WordBreak);
        }

        private void ActualizarBarraEstado_62FD(string claveModo_62FD)
        {
            this.claveModo_62FD = claveModo_62FD;
            fechaModo_62FD = DateTime.Now;
            MostrarTextosEstado_62FD();
        }

        private void MostrarTextosEstado_62FD()
        {
            if (claveModo_62FD == null) return;

            toolStripStatusLabel1.Text = string.Format(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_BarraModo"), IdiomaBLL_23DB.ObtenerTexto_23DB(claveModo_62FD));
            toolStripStatusLabel2.Text = string.Format(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_BarraUsuario"), SessionManager_23DB.ObtenerInstancia_23DB().Login_23DB);
            toolStripStatusLabel3.Text = string.Format(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_BarraFecha"), fechaModo_62FD.ToString("dd/MM/yyyy HH:mm"));
        }

        private void CertificarIntervencion_62FD_FormClosing(object sender, FormClosingEventArgs e)
        {
            Observer_23DB.ObtenerInstancia_23DB().Desuscribir_23DB(this);

        }
    }
}
