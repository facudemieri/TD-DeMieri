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
            CargarCombos_62FD();
            ModoConsulta_62FD();
            ActualizarBarraEstado_62FD("Consulta");
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
            lblEstado.Text = "Modo Consulta";
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
                    MessageBox.Show("No se encontraron vuelos disponibles con los criterios ingresados.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al buscar vuelos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarGrilla_62FD(List<Vuelo_62FD> lista_62FD)
        {
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = lista_62FD;
            lblEstado.Text = "Modo Consulta - " + lista_62FD.Count + " vuelo/s encontrado/s";
            ActualizarBarraEstado_62FD("Consulta");
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

        private void ActualizarBarraEstado_62FD(string modo_62FD)
        {
            toolStripStatusLabel1.Text = "Modo: " + modo_62FD;
            toolStripStatusLabel2.Text = "Usuario: " + SessionManager_23DB.ObtenerInstancia_23DB().Login_23DB;
            toolStripStatusLabel3.Text = "Última consulta: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm");
        }

        private void btnContinuar_Click(object sender, EventArgs e)
        {
            if(vueloSeleccionado_62FD == null)
            {
                MessageBox.Show("Debe seleccionar un vuelo.", "Informacion", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        public void ActualizarIdioma_23DB(Dictionary<string, string> configuracion_23DB)
        {
            string formName_23DB = "SeleccionarVuelo";
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
                MessageBox.Show("Error al cargar los vuelos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
