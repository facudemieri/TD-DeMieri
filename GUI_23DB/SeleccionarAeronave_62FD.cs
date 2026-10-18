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
    public partial class SeleccionarAeronave_62FD : Form, IIdiomaObserver_23DB
    {

        AeronaveBLL_62FD aeronaveBLL_62FD = new AeronaveBLL_62FD();

        private Aeronave_62FD aeronaveSeleccionada_62FD = null;
        private List<Aeronave_62FD> listaAeronaves_62FD = new List<Aeronave_62FD>();
        private string columnaOrden_62FD = null;
        private bool ordenAscendente_62FD = true;
        private string claveModo_62FD = null;
        private DateTime fechaModo_62FD = DateTime.Now;
        public Aeronave_62FD AeronaveSeleccionada_62FD
        {
            get { return aeronaveSeleccionada_62FD; }
        }
        public SeleccionarAeronave_62FD()
        {
            InitializeComponent();
            Observer_23DB.ObtenerInstancia_23DB().Suscribir_23DB(this);
            AplicarIdiomaActual_23DB();
            dataGridView1.AutoGenerateColumns = false;
            ConfigurarControles_62FD();
            CargarCombos_62FD();
        }

        private void ConfigurarControles_62FD()
        {
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox2.DropDownStyle = ComboBoxStyle.DropDownList;

            textBox1.MaxLength = 20;
            textBox2.MaxLength = 50;

            ConfigurarGrilla_62FD(dataGridView1);

            colMatricula.FillWeight = 80;
            colNroSerie.FillWeight = 80;
            colEstadoOperativo.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colHorasVuelo.DefaultCellStyle.Format = "N0";
            colHorasVuelo.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            colIntervalo.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            colIntervalo.FillWeight = 50;
            colProximaRevision.DefaultCellStyle.Format = "dd/MM/yyyy";
            colProximaRevision.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dataGridView1.Paint += Grilla_Paint_62FD;
            dataGridView1.ColumnHeaderMouseClick += dataGridView1_ColumnHeaderMouseClick;
            this.Shown += (s_62FD, e_62FD) => LimpiarSeleccionGrilla_62FD(dataGridView1);

            this.AcceptButton = btnBuscar;
            this.CancelButton = btnSalir;
        }

        private void CargarCombos_62FD()
        {
            comboBox1.Items.Clear();
            comboBox1.Items.Add("En Servicio");
            comboBox1.Items.Add("En Mantenimiento");
            comboBox1.Items.Add("Fuera de Servicio");
            comboBox1.SelectedIndex = -1;

            comboBox2.Items.Clear();
            comboBox2.Items.Add("Boeing");
            comboBox2.Items.Add("Airbus");
            comboBox2.Items.Add("Embraer");
            comboBox2.SelectedIndex = -1;
        }

        private void btnContinuar_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0 || dataGridView1.SelectedRows[0].DataBoundItem == null)
            {
                MessageBox.Show(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_SeleccioneAeronave"), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Informacion"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            aeronaveSeleccionada_62FD = (Aeronave_62FD)dataGridView1.SelectedRows[0].DataBoundItem;

            GenerarOrdenTrabajo_62FD generarOrden_62FD = new GenerarOrdenTrabajo_62FD(aeronaveSeleccionada_62FD);
            generarOrden_62FD.ShowDialog();

            CargarGrilla_62FD(false);
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {

            this.Close();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            CargarGrilla_62FD(true);

        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox2.Clear();
            comboBox1.SelectedIndex = -1;
            comboBox2.SelectedIndex = -1;
            checkBox1.Checked = false;
            CargarGrilla_62FD(false);
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

        private void SeleccionarAeronave_62FD_Load(object sender, EventArgs e)
        {
            ActualizarBarraEstado_62FD("Msg_NombreModoConsulta");
            CargarGrilla_62FD(false);
            this.ActiveControl = textBox1;
        }

        private void CargarGrilla_62FD(bool esBusqueda_62FD)
        {
            try
            {
                string matricula_62FD = textBox1.Text.Trim();
                string modelo_62FD = textBox2.Text.Trim();
                string estadoOperativo_62FD = comboBox1.SelectedIndex >= 0 ? comboBox1.Text : string.Empty;
                string fabricante_62FD = comboBox2.SelectedIndex >= 0 ? comboBox2.Text : string.Empty;

                listaAeronaves_62FD = aeronaveBLL_62FD.ObtenerAeronaves_62FD(matricula_62FD, modelo_62FD, fabricante_62FD, estadoOperativo_62FD, checkBox1.Checked);
                columnaOrden_62FD = null;

                dataGridView1.DataSource = null;
                dataGridView1.DataSource = listaAeronaves_62FD;
                Traductor_23DB.TraducirGrilla_23DB(dataGridView1, "SeleccionarAeronave");
                LimpiarSeleccionGrilla_62FD(dataGridView1);

                if (listaAeronaves_62FD.Count == 0)
                {
                    if (esBusqueda_62FD)
                    {
                        MessageBox.Show(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_SinAeronavesCriterios"), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Informacion"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_SinAeronavesFlota"), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Informacion"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex_62FD)
            {
                MessageBox.Show(string.Format(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_ErrorRecuperarAeronaves"), ex_62FD.Message), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView1_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridViewColumn columna_62FD = dataGridView1.Columns[e.ColumnIndex];
            ordenAscendente_62FD = columnaOrden_62FD == columna_62FD.DataPropertyName ? !ordenAscendente_62FD : true;
            columnaOrden_62FD = columna_62FD.DataPropertyName;

            listaAeronaves_62FD = Ordenar_62FD(listaAeronaves_62FD, columnaOrden_62FD, ordenAscendente_62FD);
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = listaAeronaves_62FD;
            Traductor_23DB.TraducirGrilla_23DB(dataGridView1, "SeleccionarAeronave");
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

        private void SeleccionarAeronave_62FD_FormClosing(object sender, FormClosingEventArgs e)
        {
            Observer_23DB.ObtenerInstancia_23DB().Desuscribir_23DB(this);
        }
    }
}
