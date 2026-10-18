using BE_23DB.PN2;
using BLL_23DB.PN2;
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
    public partial class AuditoriaDeCambios_62FD : Form, IIdiomaObserver_23DB
    {
        CambioAeronaveBLL_62FD cambioAeronaveBLL_62FD = new CambioAeronaveBLL_62FD();
        EventoBLL_23DB eventoBLL_23DB = new EventoBLL_23DB();

        private List<AeronaveCambio_62FD> listaCambios_62FD = new List<AeronaveCambio_62FD>();
        private string columnaOrden_62FD = null;
        private bool ordenAscendente_62FD = true;
        private string claveModo_62FD = null;
        private DateTime fechaModo_62FD = DateTime.Now;
        private ToolTip ayuda_62FD = new ToolTip();
        public AuditoriaDeCambios_62FD()
        {
            InitializeComponent();
            Observer_23DB.ObtenerInstancia_23DB().Suscribir_23DB(this);
            AplicarIdiomaActual_23DB();

            dataGridView1.AutoGenerateColumns = false;
            ConfigurarControles_62FD();
        }

        private void ConfigurarControles_62FD()
        {
            cmbMatricula.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbModelo.DropDownStyle = ComboBoxStyle.DropDownList;

            dtpFechaDesde.Format = DateTimePickerFormat.Short;
            dtpFechaHasta.Format = DateTimePickerFormat.Short;

            // No puede haber cambios registrados en el futuro: el tope es el último instante de hoy
            DateTime finDeHoy_62FD = DateTime.Today.AddDays(1).AddTicks(-1);
            dtpFechaDesde.MaxDate = finDeHoy_62FD;
            dtpFechaHasta.MaxDate = finDeHoy_62FD;

            ConfigurarGrilla_62FD(dataGridView1);

            // Doce columnas: en Fill quedan ilegibles, así que cada una toma el ancho de su contenido y aparece la barra horizontal
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridView1.ScrollBars = ScrollBars.Both;

            colFechaCambio.DefaultCellStyle.Format = "dd/MM/yyyy";
            colFechaCambio.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colHoraCambio.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colEstadoOperativoCambio.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colHorasVueloCambio.DefaultCellStyle.Format = "N0";
            colHorasVueloCambio.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            colIntervaloCambio.DefaultCellStyle.Format = "N0";
            colIntervaloCambio.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            colProxRevisionCambio.DefaultCellStyle.Format = "dd/MM/yyyy";
            colProxRevisionCambio.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colActivaCambio.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colVigenteCambio.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dataGridView1.Paint += Grilla_Paint_62FD;
            dataGridView1.ColumnHeaderMouseClick += dataGridView1_ColumnHeaderMouseClick;
            dataGridView1.CellFormatting += Grilla_CellFormatting_62FD;
            this.Shown += (s_62FD, e_62FD) => LimpiarSeleccionGrilla_62FD(dataGridView1);

            this.AcceptButton = btnAplicar;
            this.CancelButton = btnSalir;
        }

        public void ActualizarIdioma_23DB(Dictionary<string, string> configuracion_23DB)
        {
            Traductor_23DB.AplicarIdioma_23DB(this, configuracion_23DB, statusStrip1);
            AplicarToolTips_62FD();
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

        // Se llama desde ActualizarIdioma_23DB para que los ToolTip también cambien de idioma
        private void AplicarToolTips_62FD()
        {
            ayuda_62FD.SetToolTip(btnAplicar, IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_AyudaAplicarFiltroCambios"));
            ayuda_62FD.SetToolTip(btnLimpiar, IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_AyudaLimpiarFiltroCambios"));
            ayuda_62FD.SetToolTip(btnActivar, IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_AyudaActivarCambio"));
        }

        private void AuditoriaDeCambios_62FD_Load(object sender, EventArgs e)
        {
            CargarCombos_62FD();
            RestablecerFiltros_62FD();
            CargarGrilla_62FD();
            ActualizarBarraEstado_62FD("Msg_NombreModoConsulta");
        }

        private void CargarCombos_62FD()
        {
            try
            {
                cmbMatricula.Items.Clear();
                cmbMatricula.Items.Add("");

                foreach (string matricula_62FD in cambioAeronaveBLL_62FD.ObtenerMatriculas_62FD())
                {
                    cmbMatricula.Items.Add(matricula_62FD);
                }

                cmbModelo.Items.Clear();
                cmbModelo.Items.Add("");

                foreach (string modelo_62FD in cambioAeronaveBLL_62FD.ObtenerModelos_62FD())
                {
                    cmbModelo.Items.Add(modelo_62FD);
                }
            }
            catch (Exception ex_62FD)
            {
                MessageBox.Show(string.Format(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_ErrorCargarFiltros"), ex_62FD.Message), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RestablecerFiltros_62FD()
        {
            cmbMatricula.SelectedIndex = 0;
            cmbModelo.SelectedIndex = 0;
            dtpFechaDesde.Value = DateTime.Today.AddMonths(-1);
            dtpFechaHasta.Value = DateTime.Today;
        }

        private void CargarGrilla_62FD()
        {
            try
            {
                string matricula_62FD = cmbMatricula.SelectedIndex > 0 ? cmbMatricula.Text : "";
                string modelo_62FD = cmbModelo.SelectedIndex > 0 ? cmbModelo.Text : "";

                listaCambios_62FD = cambioAeronaveBLL_62FD.ObtenerCambios_62FD(matricula_62FD, modelo_62FD, dtpFechaDesde.Value.Date, dtpFechaHasta.Value.Date);
                columnaOrden_62FD = null;

                dataGridView1.DataSource = null;
                dataGridView1.DataSource = listaCambios_62FD;
                Traductor_23DB.TraducirGrilla_23DB(dataGridView1, "AuditoriaDeCambios");
                LimpiarSeleccionGrilla_62FD(dataGridView1);
            }
            catch (Exception ex_62FD)
            {
                MessageBox.Show(string.Format(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_ErrorRecuperarCambios"), ex_62FD.Message), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAplicar_Click(object sender, EventArgs e)
        {
            if (dtpFechaHasta.Value.Date < dtpFechaDesde.Value.Date)
            {
                MessageBox.Show(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_FechaHastaAnteriorDesde"), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Informacion"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            CargarGrilla_62FD();
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            RestablecerFiltros_62FD();
            CargarGrilla_62FD();
        }

        private void btnActivar_Click(object sender, EventArgs e)
        {
            try
            {
                btnActivar.Enabled = false;

                if (dataGridView1.SelectedRows.Count == 0 || dataGridView1.SelectedRows[0].DataBoundItem == null)
                {
                    MessageBox.Show(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_SeleccioneRegistro"), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Informacion"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                AeronaveCambio_62FD cambio_62FD = (AeronaveCambio_62FD)dataGridView1.SelectedRows[0].DataBoundItem;

                if (cambio_62FD.Act_62FD)
                {
                    MessageBox.Show(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_RegistroYaVigente"), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Advertencia"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (MessageBox.Show(string.Format(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_ConfirmarActivarCambio"), cambio_62FD.Matricula_62FD, cambio_62FD.Fecha_62FD.ToString("dd/MM/yyyy"), cambio_62FD.Hora_62FD), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Confirmar"), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    return;
                }

                if (!cambioAeronaveBLL_62FD.ActivarCambio_62FD(cambio_62FD.Matricula_62FD, cambio_62FD.FechaHoraCambio_62FD))
                {
                    MessageBox.Show(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_NoSePudoActivarCambio"), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                RegistrarEventoBitacora_62FD("Activar Cambio Aeronave");

                MessageBox.Show(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_CambioActivado"), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Informacion"), MessageBoxButtons.OK, MessageBoxIcon.Information);

                CargarCombos_62FD();
                RestablecerFiltros_62FD();
                CargarGrilla_62FD();
            }
            catch (Exception ex_62FD)
            {
                MessageBox.Show(string.Format(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_ErrorActivarCambio"), ex_62FD.Message), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnActivar.Enabled = true;
            }
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void RegistrarEventoBitacora_62FD(string evento_62FD)
        {
            try
            {
                eventoBLL_23DB.RegistrarEvento_23DB(SessionManager_23DB.ObtenerInstancia_23DB().DNI_23DB, "Maestros", evento_62FD, 3);
            }
            catch (Exception ex_62FD)
            {
                MessageBox.Show(string.Format(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_OperacionSinBitacora"), ex_62FD.Message), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Advertencia"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
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

        private void dataGridView1_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridViewColumn columna_62FD = dataGridView1.Columns[e.ColumnIndex];
            ordenAscendente_62FD = columnaOrden_62FD == columna_62FD.DataPropertyName ? !ordenAscendente_62FD : true;
            columnaOrden_62FD = columna_62FD.DataPropertyName;

            listaCambios_62FD = Ordenar_62FD(listaCambios_62FD, columnaOrden_62FD, ordenAscendente_62FD);
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = listaCambios_62FD;
            Traductor_23DB.TraducirGrilla_23DB(dataGridView1, "AuditoriaDeCambios");
            LimpiarSeleccionGrilla_62FD(dataGridView1);

            // El glifo se pierde al reenlazar, por eso se pone después
            dataGridView1.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection = ordenAscendente_62FD ? SortOrder.Ascending : SortOrder.Descending;
        }

        // Muestra los bool (Activa, Vigente) como Sí / No traducidos en lugar de True / False
        private void Grilla_CellFormatting_62FD(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.Value is bool)
            {
                e.Value = IdiomaBLL_23DB.ObtenerTexto_23DB((bool)e.Value ? "Msg_Si" : "Msg_No");
                e.FormattingApplied = true;
            }
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

        private void AuditoriaDeCambios_62FD_FormClosing(object sender, FormClosingEventArgs e)
        {
            Observer_23DB.ObtenerInstancia_23DB().Desuscribir_23DB(this);

        }
    }
}
