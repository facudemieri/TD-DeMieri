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
    public partial class RegistrarPasajero_62FD : Form, IIdiomaObserver_23DB
    {
        PasajeroBLL_62FD pasajeroBLL_62FD = new PasajeroBLL_62FD();
        EventoBLL_23DB eventoBLL_23DB = new EventoBLL_23DB();
        private Pasajero_62FD pasajeroRegistrado_62FD = null;
        public Pasajero_62FD PasajeroRegistrado_62FD
        {
            get { return pasajeroRegistrado_62FD; }
        }
        public RegistrarPasajero_62FD(string DNI_62FD)
        {
            InitializeComponent();
            Observer_23DB.ObtenerInstancia_23DB().Suscribir_23DB(this);
            AplicarIdiomaActual_23DB();

            txtDNI.Text = DNI_62FD;
            txtDNI.Enabled = false;

            ModoAñadir();
        }

        private void ModoAñadir()
        {
            lblEstado.Text = "Modo Añadir";
            dateTimePicker1.MaxDate = DateTime.Now.Date.AddDays(-1);
        }

        public void ActualizarIdioma_23DB(Dictionary<string, string> configuracion_23DB)
        {
            string formName_23DB = "RegistrarPasajero";
            foreach (Control control_23DB in ObtenerTodosControles_23DB(this))
            {
                string clave_23DB = formName_23DB + "_" + control_23DB.Name;
                if (configuracion_23DB.ContainsKey(clave_23DB))
                {
                    control_23DB.Text = configuracion_23DB[clave_23DB];
                }

            }
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
        private void btnAplicar_Click(object sender, EventArgs e)
        {
            try
            {
                Pasajero_62FD pasajero_62FD = new Pasajero_62FD();
                pasajero_62FD.NumeroDocumento_62FD = txtDNI.Text;
                pasajero_62FD.Apellido_62FD = txtApellido.Text;
                pasajero_62FD.Nombre_62FD = txtNombre.Text;
                pasajero_62FD.FechaNacimiento_62FD = dateTimePicker1.Value;
                pasajero_62FD.CorreoElectronico_62FD = txtCorreo.Text;
                pasajero_62FD.Telefono_62FD = txtTelefono.Text;
                pasajero_62FD.ContactoEmergencia_62FD = txtContactoEmergencia.Text;

                pasajeroBLL_62FD.RegistrarPasajero_62FD(pasajero_62FD);

                eventoBLL_23DB.RegistrarEvento_23DB(SessionManager_23DB.ObtenerInstancia_23DB().DNI_23DB, "Pasajes", "Registrar Pasajero", 2);

                pasajeroRegistrado_62FD = pasajero_62FD;

                MessageBox.Show("Pasajero registrado correctamente.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al registrar el pasajero: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void RegistrarPasajero_62FD_Load(object sender, EventArgs e)
        {

        }

        private void RegistrarPasajero_62FD_FormClosing(object sender, FormClosingEventArgs e)
        {
            Observer_23DB.ObtenerInstancia_23DB().Desuscribir_23DB(this);
        }
    }
}
