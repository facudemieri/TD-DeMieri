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
            CargarCombos_62FD();
            ModoConsulta_62FD();
            ActualizarBarraEstado_62FD("Msg_NombreModoConsulta"); // se pasa la clave del nombre del modo
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
