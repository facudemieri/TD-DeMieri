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
    public partial class SeleccionarVuelo_62FD : Form, IIdiomaObserver_23DB
    {
        VueloBLL_62FD vueloBLL_62FD = new VueloBLL_62FD();
        private List<Vuelo_62FD> listaVuelos_62FD = new List<Vuelo_62FD>();
        private Vuelo_62FD vueloSeleccionado_62FD = null;

        // estado de los textos que escribe el código. Se guardan claves, no textos traducidos
        private string claveEstado_62FD = null;
        private int cantidadVuelos_62FD = 0;
        private string claveModo_62FD = null;
        private DateTime fechaModo_62FD = DateTime.Now;

        public Vuelo_62FD VueloSeleccionado_62FD
        {
            get { return vueloSeleccionado_62FD; }
        }
        public SeleccionarVuelo_62FD()
        {
            InitializeComponent();
            Observer_23DB.ObtenerInstancia_23DB().Suscribir_23DB(this);
            AplicarIdiomaActual_23DB();
            dataGridView1.AutoGenerateColumns = false;
            ConfigurarControles_62FD();
            CargarCombos_62FD();
            ModoConsulta_62FD();
            ActualizarBarraEstado_62FD("Msg_NombreModoConsulta"); // se pasa la clave del nombre del modo
        }

        private void ConfigurarControles_62FD()
        {
            // Se buscan vuelos para viajar: la fecha de partida no puede estar en el pasado.
            // El control nunca carga un registro existente, así que el límite no puede pisar un valor guardado.
            dateTimePicker1.MinDate = DateTime.Today;

            ConfigurarGrilla_62FD(dataGridView1);

            NroVuelo_62FD.FillWeight = 70;
            FechaHoraSalida_62FD.DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
            FechaHoraSalida_62FD.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            AsientosDisponibles_62FD.DefaultCellStyle.Format = "N0";
            AsientosDisponibles_62FD.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            AsientosDisponibles_62FD.FillWeight = 70;
            // Precio ya trae formato moneda (C2) del diseñador; se mantiene
            Precio_62FD.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            dataGridView1.Paint += Grilla_Paint_62FD;
            dataGridView1.ColumnHeaderMouseClick += dataGridView1_ColumnHeaderMouseClick;
            this.Shown += (s_62FD, e_62FD) => LimpiarSeleccionGrilla_62FD(dataGridView1);
        }

        private string columnaOrden_62FD = null;
        private bool ordenAscendente_62FD = true;

        private void dataGridView1_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            // Con la grilla vaciada (DataSource en null) no se reordena: volvería a mostrar la lista anterior
            if (dataGridView1.DataSource == null) return;

            DataGridViewColumn columna_62FD = dataGridView1.Columns[e.ColumnIndex];
            ordenAscendente_62FD = columnaOrden_62FD == columna_62FD.DataPropertyName ? !ordenAscendente_62FD : true;
            columnaOrden_62FD = columna_62FD.DataPropertyName;

            listaVuelos_62FD = Ordenar_62FD(listaVuelos_62FD, columnaOrden_62FD, ordenAscendente_62FD);
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = listaVuelos_62FD;
            LimpiarSeleccionVuelo_62FD();

            // El glifo se pierde al reenlazar, por eso se pone después
            dataGridView1.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection = ordenAscendente_62FD ? SortOrder.Ascending : SortOrder.Descending;
        }

        // Al reenlazar la grilla no queda ninguna fila resaltada: el vuelo elegido también se descarta,
        // para que Continuar no avance con un vuelo que el usuario ya no ve seleccionado
        private void LimpiarSeleccionVuelo_62FD()
        {
            LimpiarSeleccionGrilla_62FD(dataGridView1);
            vueloSeleccionado_62FD = null;
            btnContinuar.Enabled = false;
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

        private void CargarCombos_62FD()
        {
            cmbOrigen.DataSource = vueloBLL_62FD.ObtenerOrigenes_62FD();
            cmbDestino.DataSource = vueloBLL_62FD.ObtenerDestinos_62FD();

            cmbOrigen.SelectedIndex = -1;
            cmbDestino.SelectedIndex = -1;
        }
        private void ModoConsulta_62FD()
        {
            claveEstado_62FD = "Msg_ModoConsulta";
            MostrarTextosEstado_62FD();
            dataGridView1.DataSource = null;
            vueloSeleccionado_62FD = null;
            btnContinuar.Enabled = false;
        }

        

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            

            try
            {
                listaVuelos_62FD = vueloBLL_62FD.BuscarVuelos_62FD(cmbOrigen.Text, cmbDestino.Text, dateTimePicker1.Value, (int)numericUpDown1.Value);

                CargarGrilla_62FD(listaVuelos_62FD);
                if (listaVuelos_62FD.Count == 0)
                {
                    MessageBox.Show(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_SinVuelosDisponibles"), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Informacion"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_ErrorBuscarVuelos"), ex.Message), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarGrilla_62FD(List<Vuelo_62FD> lista_62FD)
        {
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = lista_62FD;
            columnaOrden_62FD = null;
            LimpiarSeleccionVuelo_62FD();
            claveEstado_62FD = "Msg_ModoConsultaVuelosEncontrados";
            cantidadVuelos_62FD = lista_62FD.Count;
            ActualizarBarraEstado_62FD("Msg_NombreModoConsulta"); // se pasa la clave del nombre del modo
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                vueloSeleccionado_62FD = (Vuelo_62FD)dataGridView1.Rows[e.RowIndex].DataBoundItem;
                btnContinuar.Enabled = true;
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            cmbOrigen.SelectedIndex = -1;
            cmbDestino.SelectedIndex = -1;
            dateTimePicker1.Value = DateTime.Now;
            numericUpDown1.Value = 1;
            ModoConsulta_62FD();
            listaVuelos_62FD = vueloBLL_62FD.ObtenerTodosLosVuelos_62FD();
            CargarGrilla_62FD(listaVuelos_62FD);
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
          
        }

        // Recibe la clave del nombre del modo (solo se muestra, nunca se compara)
        private void ActualizarBarraEstado_62FD(string claveModo_62FD)
        {
            this.claveModo_62FD = claveModo_62FD;
            fechaModo_62FD = DateTime.Now; // se guarda al escribir la barra; al cambiar de idioma se reescribe la misma fecha
            MostrarTextosEstado_62FD();
        }

        // Reconstruye lblEstado y la barra de estado a partir de las claves guardadas, en el idioma activo
        private void MostrarTextosEstado_62FD()
        {
            if(claveEstado_62FD != null)
            {
                lblEstado.Text = string.Format(IdiomaBLL_23DB.ObtenerTexto_23DB(claveEstado_62FD), cantidadVuelos_62FD);
            }

            if(claveModo_62FD != null)
            {
                toolStripStatusLabel1.Text = string.Format(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_BarraModo"), IdiomaBLL_23DB.ObtenerTexto_23DB(claveModo_62FD));
                toolStripStatusLabel2.Text = string.Format(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_BarraUsuario"), SessionManager_23DB.ObtenerInstancia_23DB().Login_23DB);
                toolStripStatusLabel3.Text = string.Format(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_BarraUltimaConsulta"), fechaModo_62FD.ToString("dd/MM/yyyy HH:mm"));
            }
        }

        private void btnContinuar_Click(object sender, EventArgs e)
        {
            if(vueloSeleccionado_62FD == null)
            {
                MessageBox.Show(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_SeleccioneVuelo"), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Informacion"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        public void ActualizarIdioma_23DB(Dictionary<string, string> configuracion_23DB)
        {
            Traductor_23DB.AplicarIdioma_23DB(this, configuracion_23DB);
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

        private void SeleccionarVuelo_62FD_FormClosing(object sender, FormClosingEventArgs e)
        {
            Observer_23DB.ObtenerInstancia_23DB().Desuscribir_23DB(this);
        }

        private void btnCancelar_Click_1(object sender, EventArgs e)
        {
  
        }

        private void SeleccionarVuelo_62FD_Load(object sender, EventArgs e)
        {
            try
            {
                listaVuelos_62FD = vueloBLL_62FD.ObtenerTodosLosVuelos_62FD();
                CargarGrilla_62FD(listaVuelos_62FD);
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_ErrorCargarVuelos"), ex.Message), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
