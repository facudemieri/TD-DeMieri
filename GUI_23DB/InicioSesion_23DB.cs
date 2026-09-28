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
    public partial class InicioSesion_23DB : Form
    {
        private UsuarioBLL_23DB usuarioBLL_23DB = new UsuarioBLL_23DB();
        private EventoBLL_23DB eventoBLL_23DB = new EventoBLL_23DB();
        private RolBLL_23DB rolBLL_23DB = new RolBLL_23DB();
        DVBLL_23DB dvBLL_23DB = new DVBLL_23DB();
        public bool EsRelogin_23DB { get; set; } = false;

        
        public InicioSesion_23DB()
        {
            InitializeComponent();
                    
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(txtUsuario.Text) || string.IsNullOrEmpty(txtContraseña.Text))
            {
                MessageBox.Show("Debe completar todos los campos.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Usuario_23DB usuarioPorLogin_23DB = usuarioBLL_23DB.ObtenerUsuarioPorLogin_23DB(txtUsuario.Text);

            List<DV_23DB> inconsistencias_23DB = usuarioBLL_23DB.VerificarConsistenciaDV_23DB();
            if(inconsistencias_23DB.Count > 0)
            {
                
                bool primeraInstalacion_23DB = dvBLL_23DB.EsPrimeraInstalacion_23DB();

                if(primeraInstalacion_23DB)
                {
                    new DVBLL_23DB().RecalcularDV_23DB();
                    
                }
                else if(usuarioPorLogin_23DB != null && usuarioBLL_23DB.ObtenerNombreRol_23DB(usuarioPorLogin_23DB.IdRol_23DB) == "Administrador")
                {
                    RepararInconsistencia_23DB repararForm_23DB = new RepararInconsistencia_23DB(inconsistencias_23DB);
                    repararForm_23DB.ShowDialog();
                    return;
                }
                else
                {
                    MessageBox.Show("El sistema no está disponible en este momento. Contacte al administrador.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                
            }

            


            if(usuarioPorLogin_23DB == null)
            {
                MessageBox.Show("Credenciales incorrectas.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            if(usuarioBLL_23DB.VerificarTiempoReset_23DB(usuarioPorLogin_23DB.FechaUltimoIntento_23DB))
            {
                usuarioBLL_23DB.ResetearIntentos_23DB(usuarioPorLogin_23DB.DNI_23DB);
            }

            if(usuarioPorLogin_23DB.Bloqueado_23DB)
            {
                MessageBox.Show("Su cuenta está bloqueada. Contacte al administrador.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if(!usuarioPorLogin_23DB.Activo_23DB)
            {
                MessageBox.Show("Su cuenta está deshabilitada. Contacte al administrador.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Usuario_23DB usuarioAutenticado_23DB = usuarioBLL_23DB.AutenticarUsuario_23DB(txtUsuario.Text, txtContraseña.Text);

            if(usuarioAutenticado_23DB == null)
            {
                usuarioBLL_23DB.IncrementarIntentos_23DB(usuarioPorLogin_23DB.DNI_23DB);
                usuarioPorLogin_23DB = usuarioBLL_23DB.ObtenerUsuarioPorLogin_23DB(txtUsuario.Text);

                if(usuarioPorLogin_23DB.IntentosFallidos_23DB >= 3)
                {
                    usuarioBLL_23DB.BloquearUsuario_23DB(usuarioPorLogin_23DB.DNI_23DB);
                    eventoBLL_23DB.RegistrarEvento_23DB(usuarioPorLogin_23DB.DNI_23DB, "Usuarios", "Bloqueo Automático por Intentos", 1);
                    MessageBox.Show("Su cuenta ha sido bloqueada por demasiados intentos fallidos. Contacte al administrador.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                MessageBox.Show($"Credenciales incorrectas. Intentos restantes: {3 - usuarioPorLogin_23DB.IntentosFallidos_23DB}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string nombreRol_23DB = usuarioBLL_23DB.ObtenerNombreRol_23DB(usuarioAutenticado_23DB.IdRol_23DB);

            // El singleton rechaza la sesion si ya hay una activa (caso Relogin).
            // Va antes que las operaciones accesorias para no registrar un Login
            // que no ocurrio.
            try
            {
                SessionManager_23DB.ObtenerInstancia_23DB().InicializarSesion_23DB(
                    usuarioAutenticado_23DB.DNI_23DB,
                    usuarioAutenticado_23DB.Login_23DB,
                    nombreRol_23DB
                );
            }
            catch(Exception ex_23DB)
            {
                MessageBox.Show(ex_23DB.Message, "Sesión activa", MessageBoxButtons.OK, MessageBoxIcon.Error);
                if(EsRelogin_23DB)
                {
                    this.DialogResult = DialogResult.Cancel;
                    this.Close();
                }
                return;
            }

            // Operaciones accesorias: un fallo no impide entrar, porque la
            // autenticacion ya fue correcta.
            try
            {
                usuarioBLL_23DB.ResetearIntentos_23DB(usuarioAutenticado_23DB.DNI_23DB);
            }
            catch(Exception ex_23DB)
            {
                System.Diagnostics.Debug.WriteLine(ex_23DB);
            }

            try
            {
                eventoBLL_23DB.RegistrarEvento_23DB(usuarioAutenticado_23DB.DNI_23DB, "Usuarios", "Login", 1);
            }
            catch(Exception ex_23DB)
            {
                System.Diagnostics.Debug.WriteLine(ex_23DB);
            }

            string ultimoIdioma_23DB = usuarioAutenticado_23DB.UltimoIdioma_23DB;
            if(!string.IsNullOrEmpty(ultimoIdioma_23DB))
            {
                SessionManager_23DB.ObtenerInstancia_23DB().UltimoIdioma_23DB = ultimoIdioma_23DB;
            }

            if(EsRelogin_23DB)
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                List<string> patentes_23DB = rolBLL_23DB.ObtenerPatentesDeRol_23DB(usuarioAutenticado_23DB.IdRol_23DB);
                MenuPrincipal_23DB menuPrincipal_23DB = new MenuPrincipal_23DB(patentes_23DB);
                menuPrincipal_23DB.Show();
                this.Hide();
            }
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox1.Checked)
            { 
                txtContraseña.PasswordChar = '\0';
            }
            else
                txtContraseña.PasswordChar = '*';
        }
                
        private void InicioSesion_23DB_FormClosing(object sender, FormClosingEventArgs e)
        {
            
        }

        private void InicioSesion_23DB_Load(object sender, EventArgs e)
        {

        }
    }
}
