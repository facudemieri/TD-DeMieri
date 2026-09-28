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
    public partial class CambiarContraseña_23DB : Form, IIdiomaObserver_23DB
    {
        
        public CambiarContraseña_23DB()
        {
            InitializeComponent();
            Observer_23DB.ObtenerInstancia_23DB().Suscribir_23DB(this);
            AplicarIdiomaActual_23DB();
        }

        private UsuarioBLL_23DB usuarioBLL_23DB = new UsuarioBLL_23DB();
        private EventoBLL_23DB eventoBLL_23DB = new EventoBLL_23DB();

        private void btnConfirmar_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(txtPasswordActual.Text) || string.IsNullOrEmpty(txtPasswordNueva.Text) || string.IsNullOrEmpty(txtConfirmarPassword.Text))
            {
                MessageBox.Show("Debe completar todos los campos.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if(txtPasswordNueva.Text != txtConfirmarPassword.Text)
            {
                MessageBox.Show("Las contraseñas nuevas no coinciden.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if(txtPasswordActual.Text == txtPasswordNueva.Text)
            {
                MessageBox.Show("La nueva contraseña debe ser distinta a la actual.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string dni_23DB = SessionManager_23DB.ObtenerInstancia_23DB().DNI_23DB;
            bool resultado_23DB = usuarioBLL_23DB.CambiarClave_23DB(dni_23DB, txtPasswordActual.Text, txtPasswordNueva.Text, txtConfirmarPassword.Text);

            if(!resultado_23DB)
            {
                MessageBox.Show("La contraseña actual es incorrecta.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            eventoBLL_23DB.RegistrarEvento_23DB(dni_23DB, "Usuarios", "Cambiar Clave", 2);
            MessageBox.Show("Contraseña actualizada correctamente. Debe iniciar sesión nuevamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            SessionManager_23DB.ObtenerInstancia_23DB().CerrarSesion_23DB();
            this.Close();
            
            Form menuPrincipal_23DB = Application.OpenForms["MenuPrincipal_23DB"];
            if(menuPrincipal_23DB != null)
            {
                menuPrincipal_23DB.Close();
            }
                
            eventoBLL_23DB.RegistrarEvento_23DB(dni_23DB, "Usuarios", "Logout", 1);
            InicioSesion_23DB login_23DB = new InicioSesion_23DB();
            login_23DB.Show();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void chkMostrarActual_CheckedChanged(object sender, EventArgs e)
        {
            txtPasswordActual.PasswordChar = chkMostrarActual.Checked ? '\0' : '*';
        }

        private void chkMostarNueva_CheckedChanged(object sender, EventArgs e)
        {
            txtPasswordNueva.PasswordChar = chkMostrarNueva.Checked ? '\0' : '*';
        }

        private void chkMostrarConfirmacion_CheckedChanged(object sender, EventArgs e)
        {
            txtConfirmarPassword.PasswordChar = chkMostrarConfirmacion.Checked ? '\0' : '*';
        }

        private void lblLogin_Click(object sender, EventArgs e)
        {
            
        }

        private void CambiarContraseña_23DB_Load(object sender, EventArgs e)
        {
            lblLogin.Text = "LOGIN: " + SessionManager_23DB.ObtenerInstancia_23DB().Login_23DB;
        }

        public void ActualizarIdioma_23DB(Dictionary<string, string> configuracion_23DB)
        {
            string formName_23DB = "CambiarContraseña";
            foreach(Control control_23DB in ObtenerTodosControles_23DB(this))
            {
                string clave_23DB = formName_23DB + "_" + control_23DB.Name;
                if(configuracion_23DB.ContainsKey(clave_23DB))
                {
                    control_23DB.Text = configuracion_23DB[clave_23DB];
                }                    
            }
        }

        private List<Control> ObtenerTodosControles_23DB(Control control_23DB)
        {
            List<Control> lista_23DB = new List<Control>();
            foreach(Control c_23DB in control_23DB.Controls)
            {
                lista_23DB.Add(c_23DB);
                lista_23DB.AddRange(ObtenerTodosControles_23DB(c_23DB));
            }
            return lista_23DB;
        }

        private void CambiarContraseña_23DB_FormClosing(object sender, FormClosingEventArgs e)
        {
            Observer_23DB.ObtenerInstancia_23DB().Desuscribir_23DB(this);
        }

        public void AplicarIdiomaActual_23DB()
        {
            string idiomaActual_23DB = SessionManager_23DB.ObtenerInstancia_23DB().UltimoIdioma_23DB;
            if(!string.IsNullOrEmpty(idiomaActual_23DB))
            {
                IdiomaBLL_23DB idiomaBLL_23DB = new IdiomaBLL_23DB();
                ActualizarIdioma_23DB(idiomaBLL_23DB.CargarConfiguracion_23DB(idiomaActual_23DB));
            }
        }
    }
}
