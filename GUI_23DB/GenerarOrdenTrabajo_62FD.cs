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

namespace GUI_23DB
{
    public partial class GenerarOrdenTrabajo_62FD : Form, IIdiomaObserver_23DB
    {

        OrdenTrabajoBLL_62FD ordenTrabajoBLL_62FD = new OrdenTrabajoBLL_62FD();
        EventoBLL_23DB eventoBLL_23DB = new EventoBLL_23DB();

        private Aeronave_62FD aeronave_62FD = null;
        private List<Usuario_23DB> listaTecnicos_62FD = new List<Usuario_23DB>();
        private List<OrdenTrabajo_62FD> listaOrdenes_62FD = new List<OrdenTrabajo_62FD>();
        private string columnaOrden_62FD = null;
        private bool ordenAscendente_62FD = true;
        private string claveModo_62FD = null;
        private DateTime fechaModo_62FD = DateTime.Now;
        public GenerarOrdenTrabajo_62FD(Aeronave_62FD aeronaveSeleccionada_62FD)
        {
            InitializeComponent();
            Observer_23DB.ObtenerInstancia_23DB().Suscribir_23DB(this);
            AplicarIdiomaActual_23DB();
            dataGridView1.AutoGenerateColumns = false;
            aeronave_62FD = aeronaveSeleccionada_62FD;
            ConfigurarControles_62FD();
        }

        private void ConfigurarControles_62FD()
        {
            cmbTipoIntervencion.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbHangar.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbLegajoTecnico.DropDownStyle = ComboBoxStyle.DropDownList;

            txtDescripcion.MaxLength = 500;
            // Es multilínea: Enter tiene que hacer salto de línea y no disparar el botón Aplicar
            txtDescripcion.AcceptsReturn = true;

            ConfigurarGrilla_62FD(dataGridView1);

            dtpFechaInicio.MinDate = DateTime.Today;
            dtpFechaLimite.MinDate = DateTime.Today;

            colIdOrden.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            colIdOrden.FillWeight = 50;
            colEstadoOrden.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colFechaInicio.DefaultCellStyle.Format = "dd/MM/yyyy";
            colFechaInicio.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colFechaLimite.DefaultCellStyle.Format = "dd/MM/yyyy";
            colFechaLimite.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dataGridView1.Paint += Grilla_Paint_62FD;
            dataGridView1.ColumnHeaderMouseClick += dataGridView1_ColumnHeaderMouseClick;
            this.Shown += (s_62FD, e_62FD) => LimpiarSeleccionGrilla_62FD(dataGridView1);

            this.AcceptButton = btnAplicar;
            this.CancelButton = btnSalir;
        }


        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }

        private void GenerarOrdenTrabajo_62FD_Load(object sender, EventArgs e)
        {
            MostrarAeronave_62FD();
            CargarCombos_62FD();
            CargarTecnicos_62FD();
            CargarGrilla_62FD();
            ModoAlta_62FD();
            this.ActiveControl = cmbTipoIntervencion;
  
        }

        private void MostrarAeronave_62FD()
        {
            txtMatricula.Text = aeronave_62FD.Matricula_62FD;
            txtModelo.Text = aeronave_62FD.Modelo_62FD;
            txtFabricante.Text = aeronave_62FD.Fabricante_62FD;
            txtHoras.Text = aeronave_62FD.HorasVueloAcumuladas_62FD.ToString();
            txtFechaProxRevision.Text = aeronave_62FD.FechaProximaRevision_62FD.ToString("dd/MM/yyyy");

            txtMatricula.ReadOnly = true;
            txtModelo.ReadOnly = true;
            txtFabricante.ReadOnly = true;
            txtHoras.ReadOnly = true;
            txtFechaProxRevision.ReadOnly = true;
        }

        private void CargarCombos_62FD()
        {
            cmbTipoIntervencion.Items.Clear();
            cmbTipoIntervencion.Items.Add("Preventivo");
            cmbTipoIntervencion.Items.Add("Correctivo");
            cmbTipoIntervencion.SelectedIndex = -1;

            cmbHangar.Items.Clear();
            cmbHangar.Items.Add("Hangar 1");
            cmbHangar.Items.Add("Hangar 2");
            cmbHangar.Items.Add("Hangar 3");
            cmbHangar.SelectedIndex = -1;
        }

        private void CargarTecnicos_62FD()
        {
            try
            {
                listaTecnicos_62FD = ordenTrabajoBLL_62FD.ObtenerTecnicos_62FD();

                cmbLegajoTecnico.Items.Clear();

                foreach (Usuario_23DB tecnico_62FD in listaTecnicos_62FD)
                {
                    cmbLegajoTecnico.Items.Add(tecnico_62FD.Apellido_23DB + ", " + tecnico_62FD.Nombre_23DB + " (" + tecnico_62FD.DNI_23DB + ")");
                }

                cmbLegajoTecnico.SelectedIndex = -1;

                if (listaTecnicos_62FD.Count == 0)
                {
                    MessageBox.Show(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_SinTecnicosActivos"), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Informacion"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex_62FD)
            {
                MessageBox.Show(string.Format(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_ErrorRecuperarTecnicos"), ex_62FD.Message), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarGrilla_62FD()
        {
            try
            {
                listaOrdenes_62FD = ordenTrabajoBLL_62FD.ObtenerOrdenesPorAeronave_62FD(aeronave_62FD.Matricula_62FD);
                columnaOrden_62FD = null;

                dataGridView1.DataSource = null;
                dataGridView1.DataSource = listaOrdenes_62FD;
                Traductor_23DB.TraducirGrilla_23DB(dataGridView1, "GenerarOrdenTrabajo");
                LimpiarSeleccionGrilla_62FD(dataGridView1);
            }
            catch (Exception ex_62FD)
            {
                MessageBox.Show(string.Format(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_ErrorRecuperarOrdenes"), ex_62FD.Message), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ModoAlta_62FD()
        {
            groupBox2.Enabled = true;
            btnAplicar.Enabled = true;
            btnCancelar.Enabled = true;
            ActualizarBarraEstado_62FD("Msg_NombreModoAlta");
        }

        private void ModoConsulta_62FD()
        {
            groupBox2.Enabled = false;
            btnAplicar.Enabled = false;
            btnCancelar.Enabled = false;
            ActualizarBarraEstado_62FD("Msg_NombreModoConsulta");
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

        private void LimpiarDatosOrden_62FD()
        {
            cmbTipoIntervencion.SelectedIndex = -1;
            cmbHangar.SelectedIndex = -1;
            cmbLegajoTecnico.SelectedIndex = -1;
            txtDescripcion.Clear();
            dtpFechaInicio.Value = DateTime.Now;
            dtpFechaLimite.Value = DateTime.Now;
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

        private void btnAplicar_Click(object sender, EventArgs e)
        {
            // Evita que un doble clic ejecute la operación dos veces
            btnAplicar.Enabled = false;

            try
            {
                if (cmbTipoIntervencion.SelectedIndex < 0 || cmbHangar.SelectedIndex < 0 || cmbLegajoTecnico.SelectedIndex < 0 || string.IsNullOrWhiteSpace(txtDescripcion.Text))
                {
                    MessageBox.Show(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_CompletarCamposOrden"), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Informacion"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (dtpFechaLimite.Value.Date < dtpFechaInicio.Value.Date)
                {
                    MessageBox.Show(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_FechaLimiteAnteriorInicio"), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Informacion"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                OrdenTrabajo_62FD orden_62FD = new OrdenTrabajo_62FD();
                orden_62FD.Matricula_62FD = aeronave_62FD.Matricula_62FD;
                orden_62FD.TipoIntervencion_62FD = cmbTipoIntervencion.Text;
                orden_62FD.Descripcion_62FD = txtDescripcion.Text.Trim();
                orden_62FD.FechaInicioPlanificada_62FD = dtpFechaInicio.Value.Date;
                orden_62FD.FechaLimite_62FD = dtpFechaLimite.Value.Date;
                orden_62FD.Hangar_62FD = cmbHangar.Text;
                orden_62FD.LegajoTecnico_62FD = listaTecnicos_62FD[cmbLegajoTecnico.SelectedIndex].DNI_23DB;

                int resultado_62FD = ordenTrabajoBLL_62FD.GenerarOrdenTrabajo_62FD(orden_62FD);

                if (resultado_62FD == 0)
                {
                    MessageBox.Show(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_OrdenAbiertaExistente"), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Advertencia"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (resultado_62FD < 0)
                {
                    MessageBox.Show(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_DatoModificado"), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Advertencia"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    CargarGrilla_62FD();
                    return;
                }

                RegistrarEventoBitacora_62FD("Generar Orden de Trabajo", 2);

                MessageBox.Show(string.Format(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_OrdenGenerada"), resultado_62FD), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Informacion"), MessageBoxButtons.OK, MessageBoxIcon.Information);

                CargarGrilla_62FD();
                ModoConsulta_62FD();
            }
            catch (Exception ex_62FD)
            {
                MessageBox.Show(string.Format(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_ErrorGenerarOrden"), ex_62FD.Message), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Solo queda habilitado si el formulario sigue en modo alta
                btnAplicar.Enabled = groupBox2.Enabled;
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

        private void dataGridView1_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridViewColumn columna_62FD = dataGridView1.Columns[e.ColumnIndex];
            ordenAscendente_62FD = columnaOrden_62FD == columna_62FD.DataPropertyName ? !ordenAscendente_62FD : true;
            columnaOrden_62FD = columna_62FD.DataPropertyName;

            listaOrdenes_62FD = Ordenar_62FD(listaOrdenes_62FD, columnaOrden_62FD, ordenAscendente_62FD);
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = listaOrdenes_62FD;
            Traductor_23DB.TraducirGrilla_23DB(dataGridView1, "GenerarOrdenTrabajo");
            LimpiarSeleccionGrilla_62FD(dataGridView1);

            // El glifo se pierde al reenlazar, por eso se pone después
            dataGridView1.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection = ordenAscendente_62FD ? SortOrder.Ascending : SortOrder.Descending;
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
            grilla_62FD.ColumnHeadersDefaultCellStyle.Font = new Font(grilla_62FD.Font, FontStyle.Bold);
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

            Rectangle area_62FD = new Rectangle(0, grilla_62FD.ColumnHeadersHeight,
                grilla_62FD.ClientSize.Width, grilla_62FD.ClientSize.Height - grilla_62FD.ColumnHeadersHeight);
            TextRenderer.DrawText(e.Graphics, IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_GrillaSinDatos"),
                grilla_62FD.Font, area_62FD, Color.Gray,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.WordBreak);
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            LimpiarDatosOrden_62FD();
            ModoAlta_62FD();
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void GenerarOrdenTrabajo_62FD_FormClosing(object sender, FormClosingEventArgs e)
        {
            Observer_23DB.ObtenerInstancia_23DB().Desuscribir_23DB(this);

        }
    }
}
