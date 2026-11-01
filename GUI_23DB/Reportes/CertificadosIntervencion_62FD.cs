using BE_23DB;
using BLL_23DB;
using Services_23DB;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI_23DB
{
    public partial class CertificadosIntervencion_62FD : Form, IIdiomaObserver_23DB
    {
        private CertificacionBLL_62FD certificacionBLL_62FD = new CertificacionBLL_62FD();
        private IntervencionBLL_62FD intervencionBLL_62FD = new IntervencionBLL_62FD();
        private AeronaveBLL_62FD aeronaveBLL_62FD = new AeronaveBLL_62FD();

        private List<Certificacion_62FD> listaCertificaciones_62FD = new List<Certificacion_62FD>();
        private Certificacion_62FD certificacionSeleccionada_62FD = null;
        private string claveEstado_62FD = null;
        private int? cantidadRegistros_62FD = null;
        public CertificadosIntervencion_62FD()
        {
            InitializeComponent();
            Observer_23DB.ObtenerInstancia_23DB().Suscribir_23DB(this);
            this.FormClosing += CertificadosIntervencion_62FD_FormClosing;
            AplicarIdiomaActual_23DB();
            Ayuda_62FD.Vincular_23DB(this, "certificados-intervencion");
        }

        private void CertificadosIntervencion_62FD_Load(object sender, EventArgs e)
        {
            ConfigurarControles_62FD();


            cmbResultado.Items.Clear();
            cmbResultado.Items.Add(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_FiltroTodos"));
            cmbResultado.Items.Add("Satisfactorio");
            cmbResultado.Items.Add("No Satisfactorio");
            cmbResultado.SelectedIndex = 0;

            LimitarFechas_62FD();
            dtpDesde.Value = DateTime.Today.AddMonths(-1);
            dtpHasta.Value = DateTime.Today;

            btnImprimir.Enabled = false;
            MostrarTextosEstado_62FD();

            CargarCertificaciones_62FD();
        }

        private void LimitarFechas_62FD()
        {
            dtpDesde.MaxDate = DateTime.Today;
            dtpHasta.MaxDate = DateTime.Today;
        }

        private void dtpDesde_ValueChanged(object sender, EventArgs e)
        {
            dtpHasta.MinDate = dtpDesde.Value.Date;
        }

        private void ConfigurarControles_62FD()
        {
            dtpDesde.Format = DateTimePickerFormat.Short;
            dtpHasta.Format = DateTimePickerFormat.Short;
            dtpDesde.ValueChanged += dtpDesde_ValueChanged;

            dataGridView1.AutoGenerateColumns = false;
            ConfigurarGrilla_62FD(dataGridView1);

            colIdIntervencion.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            colIdIntervencion.FillWeight = 50;
            colIdOrden.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            colIdOrden.FillWeight = 50;
            colFechaCert.DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
            colFechaCert.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dataGridView1.Paint += Grilla_Paint_62FD;
            this.Shown += (s_62FD, e_62FD) => LimpiarSeleccionGrilla_62FD(dataGridView1);
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

        private void CargarCertificaciones_62FD()
        {
            try
            {
                listaCertificaciones_62FD = certificacionBLL_62FD.ObtenerCertificacionesEmitidas_62FD();
                AplicarFiltros_62FD();
            }
            catch (Exception ex_62FD)
            {
                MessageBox.Show(string.Format(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_ErrorRecuperarCertificaciones"), ex_62FD.Message), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AplicarFiltros_62FD()
        {
            string nroIntervencion_62FD = txtNroIntervencion.Text.Trim();
            string matricula_62FD = txtMatricula.Text.Trim();
            DateTime desde_62FD = dtpDesde.Value.Date;
            DateTime hasta_62FD = dtpHasta.Value.Date;

            List<Certificacion_62FD> filtrada_62FD = new List<Certificacion_62FD>();

            foreach (Certificacion_62FD certificacion_62FD in listaCertificaciones_62FD)
            {
                if (nroIntervencion_62FD.Length > 0 && certificacion_62FD.IdIntervencion_62FD.ToString().IndexOf(nroIntervencion_62FD, StringComparison.OrdinalIgnoreCase) < 0)
                {
                    continue;
                }

                if (matricula_62FD.Length > 0 && certificacion_62FD.Matricula_62FD.IndexOf(matricula_62FD, StringComparison.OrdinalIgnoreCase) < 0)
                {
                    continue;
                }

                if (cmbResultado.SelectedIndex > 0 && certificacion_62FD.resultadoInspeccion_62FD != cmbResultado.Text)
                {
                    continue;
                }

                if (!checkBox1.Checked)
                {
                    if (certificacion_62FD.fechaHoraCertificacion_62FD.Date < desde_62FD) continue;
                    if (certificacion_62FD.fechaHoraCertificacion_62FD.Date > hasta_62FD) continue;
                }

                filtrada_62FD.Add(certificacion_62FD);
            }

            certificacionSeleccionada_62FD = null;
            btnImprimir.Enabled = false;

            dataGridView1.DataSource = null;
            dataGridView1.DataSource = filtrada_62FD;
            LimpiarSeleccionGrilla_62FD(dataGridView1);

            cantidadRegistros_62FD = filtrada_62FD.Count;

            if (filtrada_62FD.Count == 0)
            {
                claveEstado_62FD = "Msg_EstadoSinResultados";
            }
            else
            {
                claveEstado_62FD = "Msg_EstadoConsulta";
            }

            MostrarTextosEstado_62FD();
        }

        public void ActualizarIdioma_23DB(Dictionary<string, string> configuracion_23DB)
        {
            Traductor_23DB.AplicarIdioma_23DB(this, configuracion_23DB);
            TraducirComboResultado_62FD();
            MostrarTextosEstado_62FD();
            dataGridView1.Invalidate();
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

        private void TraducirComboResultado_62FD()
        {
            if (cmbResultado.Items.Count == 0) return;

            int seleccionado_62FD = cmbResultado.SelectedIndex;
            cmbResultado.Items[0] = IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_FiltroTodos");
            cmbResultado.SelectedIndex = seleccionado_62FD;
        }

        private void CertificadosIntervencion_62FD_FormClosing(object sender, FormClosingEventArgs e)
        {
            Observer_23DB.ObtenerInstancia_23DB().Desuscribir_23DB(this);
        }

        private void MostrarTextosEstado_62FD()
        {
            if (claveEstado_62FD != null)
            {
                toolStripStatusLabel1.Text = string.Format(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_BarraEstado"), IdiomaBLL_23DB.ObtenerTexto_23DB(claveEstado_62FD));
            }

            if (cantidadRegistros_62FD.HasValue)
            {
                toolStripStatusLabel2.Text = string.Format(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_BarraRegistros"), cantidadRegistros_62FD.Value);
            }

            toolStripStatusLabel3.Text = string.Format(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_BarraUsuario"), SessionManager_23DB.ObtenerInstancia_23DB().Login_23DB);
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            CargarCertificaciones_62FD(); ;
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            txtNroIntervencion.Clear();
            txtMatricula.Clear();
            cmbResultado.SelectedIndex = 0;
            checkBox1.Checked = false;
            LimitarFechas_62FD();
            dtpDesde.Value = DateTime.Today.AddMonths(-1);
            dtpHasta.Value = DateTime.Today;

            CargarCertificaciones_62FD();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            dtpDesde.Enabled = !checkBox1.Checked;
            dtpHasta.Enabled = !checkBox1.Checked;
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0 || dataGridView1.SelectedRows[0].DataBoundItem == null)
            {
                certificacionSeleccionada_62FD = null;
                btnImprimir.Enabled = false;
                return;
            }

            certificacionSeleccionada_62FD = (Certificacion_62FD)dataGridView1.SelectedRows[0].DataBoundItem;
            btnImprimir.Enabled = true;
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnImprimir_Click(object sender, EventArgs e)
        {
            try
            {
                btnImprimir.Enabled = false;

                if (certificacionSeleccionada_62FD == null)
                {
                    MessageBox.Show(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_SeleccioneCertificado"), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Informacion"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                Intervencion_62FD intervencion_62FD = null;
                List<Intervencion_62FD> intervenciones_62FD = intervencionBLL_62FD.ObtenerIntervencionesConOrden_62FD();

                foreach (Intervencion_62FD item_62FD in intervenciones_62FD)
                {
                    if (item_62FD.IdIntervencion_62FD == certificacionSeleccionada_62FD.IdIntervencion_62FD)
                    {
                        intervencion_62FD = item_62FD;
                        break;
                    }
                }

                if (intervencion_62FD == null)
                {
                    MessageBox.Show(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_IntervencionCertificadoNoRecuperada"), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Advertencia"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                OrdenTrabajo_62FD orden_62FD = intervencionBLL_62FD.ObtenerOrdenPorId_62FD(intervencion_62FD.IdOrden_62FD);
                List<Tarea_62FD> tareas_62FD = intervencionBLL_62FD.ObtenerTareas_62FD(intervencion_62FD.IdIntervencion_62FD);

                Aeronave_62FD aeronave_62FD = null;
                List<Aeronave_62FD> flota_62FD = aeronaveBLL_62FD.ObtenerTodas_62FD();

                foreach (Aeronave_62FD item_62FD in flota_62FD)
                {
                    if (item_62FD.Matricula_62FD == certificacionSeleccionada_62FD.Matricula_62FD)
                    {
                        aeronave_62FD = item_62FD;
                        break;
                    }
                }

                string carpeta_62FD = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "AeroManager", "Certificados");

                if (!Directory.Exists(carpeta_62FD))
                {
                    Directory.CreateDirectory(carpeta_62FD);
                }

                string ruta_62FD = Path.Combine(carpeta_62FD, "Certificado_" + certificacionSeleccionada_62FD.IdIntervencion_62FD.ToString() + ".pdf");

                GeneradorCertificadoPDF_62FD generador_62FD = new GeneradorCertificadoPDF_62FD();
                generador_62FD.Generar_62FD(ruta_62FD, intervencion_62FD, certificacionSeleccionada_62FD, aeronave_62FD, orden_62FD, tareas_62FD, "");

                System.Diagnostics.Process.Start(ruta_62FD);

                claveEstado_62FD = "Msg_EstadoCertificadoReimpreso";
                MostrarTextosEstado_62FD();
            }
            catch (Exception ex_62FD)
            {
                MessageBox.Show(string.Format(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_ErrorGenerarCertificado"), ex_62FD.Message), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnImprimir.Enabled = certificacionSeleccionada_62FD != null;
            }
        }
    }
}
