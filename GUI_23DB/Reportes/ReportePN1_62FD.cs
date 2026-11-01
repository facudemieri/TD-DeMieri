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
    public partial class ComprobantePasajes_62FD : Form, IIdiomaObserver_23DB
    {
        private FacturaBLL_62FD facturaBLL_62FD = new FacturaBLL_62FD();
        private ReservaBLL_62FD reservaBLL_62FD = new ReservaBLL_62FD();
        private PasajeBLL_62FD pasajeBLL_62FD = new PasajeBLL_62FD();

        private List<Factura_62FD> listaComprobantes_62FD = new List<Factura_62FD>();
        private Factura_62FD comprobanteSeleccionado_62FD = null;
        private string claveEstado_62FD = null;
        private int? cantidadRegistros_62FD = null;
        public ComprobantePasajes_62FD()
        {
            InitializeComponent();
            Observer_23DB.ObtenerInstancia_23DB().Suscribir_23DB(this);
            this.FormClosing += ComprobantePasajes_62FD_FormClosing;
            AplicarIdiomaActual_23DB();
        }

        public void ActualizarIdioma_23DB(Dictionary<string, string> configuracion_23DB)
        {
            Traductor_23DB.AplicarIdioma_23DB(this, configuracion_23DB);
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

        private void ComprobantePasajes_62FD_FormClosing(object sender, FormClosingEventArgs e)
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

        private void ComprobantePasajes_62FD_Load(object sender, EventArgs e)
        {
            ConfigurarControles_62FD();
            LimitarFechas_62FD();

            dtpDesde.Value = DateTime.Today.AddMonths(-1);
            dtpHasta.Value = DateTime.Today;

            btnImprimir.Enabled = false;
            MostrarTextosEstado_62FD();

            CargarComprobantes_62FD();
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
            // Las columnas son las del diseñador: sin esto el enlace agrega una por cada propiedad de Factura_62FD
            dtpDesde.Format = DateTimePickerFormat.Short;
            dtpHasta.Format = DateTimePickerFormat.Short;
            dtpDesde.ValueChanged += dtpDesde_ValueChanged;

            dataGridView1.AutoGenerateColumns = false;
            ConfigurarGrilla_62FD(dataGridView1);

            colCodReserva.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colDocumentos.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colPasajeros.FillWeight = 160;
            colFechaEmision.DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
            colFechaEmision.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colTotal.DefaultCellStyle.Format = "C2";
            colTotal.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

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

            System.Drawing.Rectangle area_62FD = new System.Drawing.Rectangle(0, grilla_62FD.ColumnHeadersHeight,
                grilla_62FD.ClientSize.Width, grilla_62FD.ClientSize.Height - grilla_62FD.ColumnHeadersHeight);
            TextRenderer.DrawText(e.Graphics, IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_GrillaSinDatos"),
                grilla_62FD.Font, area_62FD, Color.Gray,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.WordBreak);
        }

        private void CargarComprobantes_62FD()
        {
            try
            {
                listaComprobantes_62FD = facturaBLL_62FD.ObtenerComprobantes_62FD();
                AplicarFiltros_62FD();
            }
            catch (Exception ex_62FD)
            {
                MessageBox.Show(string.Format(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_ErrorRecuperarComprobantes"), ex_62FD.Message), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AplicarFiltros_62FD()
        {
            string nroFactura_62FD = txtNroFactura.Text.Trim();
            string documento_62FD = txtDocumento.Text.Trim();
            DateTime desde_62FD = dtpDesde.Value.Date;
            DateTime hasta_62FD = dtpHasta.Value.Date;

            List<Factura_62FD> filtrada_62FD = new List<Factura_62FD>();

            foreach (Factura_62FD comprobante_62FD in listaComprobantes_62FD)
            {
                if (nroFactura_62FD.Length > 0 && comprobante_62FD.NroFactura_62FD.IndexOf(nroFactura_62FD, StringComparison.OrdinalIgnoreCase) < 0)
                {
                    continue;
                }

                if (documento_62FD.Length > 0 && comprobante_62FD.Documentos_62FD.IndexOf(documento_62FD, StringComparison.OrdinalIgnoreCase) < 0)
                {
                    continue;
                }

                if (!checkBox1.Checked)
                {
                    if (comprobante_62FD.FechaEmision_62FD.Date < desde_62FD) continue;
                    if (comprobante_62FD.FechaEmision_62FD.Date > hasta_62FD) continue;
                }

                filtrada_62FD.Add(comprobante_62FD);
            }

            comprobanteSeleccionado_62FD = null;
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

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            CargarComprobantes_62FD();
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            txtNroFactura.Clear();
            txtDocumento.Clear();
            checkBox1.Checked = false;
            LimitarFechas_62FD();
            dtpDesde.Value = DateTime.Today.AddMonths(-1);
            dtpHasta.Value = DateTime.Today;

            CargarComprobantes_62FD();
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
                comprobanteSeleccionado_62FD = null;
                btnImprimir.Enabled = false;
                return;
            }

            comprobanteSeleccionado_62FD = (Factura_62FD)dataGridView1.SelectedRows[0].DataBoundItem;
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

                if (comprobanteSeleccionado_62FD == null)
                {
                    MessageBox.Show(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_SeleccioneComprobante"), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Informacion"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                Reserva_62FD reserva_62FD = reservaBLL_62FD.ObtenerReserva_62FD(comprobanteSeleccionado_62FD.CodReserva_62FD);

                if (reserva_62FD == null)
                {
                    MessageBox.Show(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_ReservaComprobanteNoRecuperada"), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Advertencia"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                List<Pasaje_62FD> pasajes_62FD = pasajeBLL_62FD.ObtenerPasajesPorReserva_62FD(comprobanteSeleccionado_62FD.CodReserva_62FD);

                if (pasajes_62FD.Count == 0)
                {
                    MessageBox.Show(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_ComprobanteSinPasajes"), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Advertencia"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string carpeta_62FD = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "AeroManager", "Comprobantes");

                if (!Directory.Exists(carpeta_62FD))
                {
                    Directory.CreateDirectory(carpeta_62FD);
                }

                string ruta_62FD = Path.Combine(carpeta_62FD, "Comprobante_" + comprobanteSeleccionado_62FD.NroFactura_62FD + ".pdf");

                GeneradorPasajePDF_62FD generador_62FD = new GeneradorPasajePDF_62FD();
                generador_62FD.Generar_62FD(ruta_62FD, reserva_62FD, comprobanteSeleccionado_62FD, pasajes_62FD, comprobanteSeleccionado_62FD.MedioPago_62FD);

                System.Diagnostics.Process.Start(ruta_62FD);

                claveEstado_62FD = "Msg_EstadoComprobanteReimpreso";
                MostrarTextosEstado_62FD();
            }
            catch (Exception ex_62FD)
            {
                MessageBox.Show(string.Format(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_ErrorGenerarComprobante"), ex_62FD.Message), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnImprimir.Enabled = comprobanteSeleccionado_62FD != null;
            }
        }
    }
}
