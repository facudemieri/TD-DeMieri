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
using System.IO;

namespace GUI_23DB
{
    public partial class CertificarIntervencion_62FD : Form, IIdiomaObserver_23DB
    {
        CertificacionBLL_62FD certificacionBLL_62FD = new CertificacionBLL_62FD();
        IntervencionBLL_62FD intervencionBLL_62FD = new IntervencionBLL_62FD();
        AeronaveBLL_62FD aeronaveBLL_62FD = new AeronaveBLL_62FD();
        UsuarioBLL_23DB usuarioBLL_23DB = new UsuarioBLL_23DB();

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
            Ayuda_62FD.Vincular_23DB(this, "certificar-intervencion");

            dataGridView1.AutoGenerateColumns = false;
            dataGridView2.AutoGenerateColumns = false;
            ConfigurarControles_62FD();
        }

        private void ConfigurarControles_62FD()
        {
            cmbResultadoInspeccion.DropDownStyle = ComboBoxStyle.DropDownList;

            txtCodigoHabilitante.MaxLength = 50;
            txtObservaciones.MaxLength = 500;
            txtObservaciones.AcceptsReturn = true;

            dtpFechaHoraCertificacion.Format = DateTimePickerFormat.Custom;
            dtpFechaHoraCertificacion.CustomFormat = "dd/MM/yyyy HH:mm";

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
                HabilitarEdicionCertificacion_62FD(true);
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

        private void HabilitarEdicionCertificacion_62FD(bool habilitar_62FD)
        {
            txtCodigoHabilitante.Enabled = habilitar_62FD;
            cmbResultadoInspeccion.Enabled = habilitar_62FD;
            txtObservaciones.Enabled = habilitar_62FD;
            dtpFechaHoraCertificacion.Enabled = habilitar_62FD;
            btnAplicar.Enabled = habilitar_62FD;
        }
        private void btnAplicar_Click(object sender, EventArgs e)
        {
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

                bool bitacoraRegistrada_62FD;
                int resultado_62FD = certificacionBLL_62FD.CertificarIntervencion_62FD(certificacion_62FD, intervencionSeleccionada_62FD.IdOrden_62FD, out bitacoraRegistrada_62FD);

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
                if (!bitacoraRegistrada_62FD)
                {
                    MessageBox.Show(string.Format(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_OperacionSinBitacora"), certificacionBLL_62FD.ErrorBitacora_62FD), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Advertencia"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

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
                HabilitarEdicionCertificacion_62FD(false);
                CargarGrilla_62FD();
            }
            catch (Exception ex_62FD)
            {
                MessageBox.Show(string.Format(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_ErrorRegistrarCertificacion"), ex_62FD.Message), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (certificacionRegistrada_62FD == null)
                {
                    btnAplicar.Enabled = true;
                }
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

                Aeronave_62FD aeronave_62FD = aeronaveBLL_62FD.ObtenerAeronaves_62FD(intervencionSeleccionada_62FD.Matricula_62FD, string.Empty, string.Empty, string.Empty, false).Find(a_62FD => a_62FD.Matricula_62FD == intervencionSeleccionada_62FD.Matricula_62FD);
                OrdenTrabajo_62FD orden_62FD = intervencionBLL_62FD.ObtenerOrdenPorId_62FD(intervencionSeleccionada_62FD.IdOrden_62FD);
                List<Tarea_62FD> tareas_62FD = intervencionBLL_62FD.ObtenerTareas_62FD(intervencionSeleccionada_62FD.IdIntervencion_62FD);
                string nombreInspector_62FD = ObtenerNombreInspector_62FD();

                GeneradorCertificadoPDF_62FD generador_62FD = new GeneradorCertificadoPDF_62FD();
                generador_62FD.Generar_62FD(ruta_62FD, intervencionSeleccionada_62FD, certificacionRegistrada_62FD, aeronave_62FD, orden_62FD, tareas_62FD, nombreInspector_62FD);

                System.Diagnostics.Process.Start(ruta_62FD);
            }
            catch (Exception ex_62FD)
            {
                MessageBox.Show(string.Format(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_ErrorGenerarDocumento"), ex_62FD.Message), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string ObtenerNombreInspector_62FD()
        {
            Usuario_23DB inspector_62FD = usuarioBLL_23DB.ObtenerUsuarioPorLogin_23DB(SessionManager_23DB.ObtenerInstancia_23DB().Login_23DB);

            return inspector_62FD != null ? inspector_62FD.Apellido_23DB + ", " + inspector_62FD.Nombre_23DB : SessionManager_23DB.ObtenerInstancia_23DB().Login_23DB;
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

            dataGridView2.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection = ordenAscendenteTareas_62FD ? SortOrder.Ascending : SortOrder.Descending;
        }

        private List<T> Ordenar_62FD<T>(List<T> lista_62FD, string propiedad_62FD, bool ascendente_62FD)
        {
            System.Reflection.PropertyInfo prop_62FD = typeof(T).GetProperty(propiedad_62FD);
            if (prop_62FD == null) return lista_62FD;
            return ascendente_62FD ? lista_62FD.OrderBy(x_62FD => prop_62FD.GetValue(x_62FD, null)).ToList() : lista_62FD.OrderByDescending(x_62FD => prop_62FD.GetValue(x_62FD, null)).ToList();
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

            System.Drawing.Rectangle area_62FD = new System.Drawing.Rectangle(0, grilla_62FD.ColumnHeadersHeight, grilla_62FD.ClientSize.Width, grilla_62FD.ClientSize.Height - grilla_62FD.ColumnHeadersHeight);
            TextRenderer.DrawText(e.Graphics, IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_GrillaSinDatos"), grilla_62FD.Font, area_62FD, Color.Gray, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.WordBreak);
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
