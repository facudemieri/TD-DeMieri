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
    public partial class GestionUsuario_23DB : Form, IIdiomaObserver_23DB
    {
        private UsuarioBLL_23DB usuarioBLL_23DB = new UsuarioBLL_23DB();
        private RolBLL_23DB rolBLL_23DB = new RolBLL_23DB();
        private EventoBLL_23DB eventoBLL_23DB = new EventoBLL_23DB();
        private string modoActual_23DB = "Consulta";
        


        public GestionUsuario_23DB()
        {
            InitializeComponent();
            Observer_23DB.ObtenerInstancia_23DB().Suscribir_23DB(this);
            AplicarIdiomaActual_23DB();
            
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnCrear_Click(object sender, EventArgs e)
        {
            ModoAnadir_23DB();
        }

        private void ModoAnadir_23DB()
        {
            modoActual_23DB = "Anadir";
            LimpiarCampos_23DB();
            HabilitarCampos_23DB(true);
            txtLogin.Enabled = false;
            txtBloqueado.Enabled = false;
            txtActivo.Enabled = false;
            btnCrear.Enabled = false;
            btnModificar.Enabled = false;
            btnAct_Des.Enabled = false;
            btnDesbloquear.Enabled = false;
            btnAplicar.Enabled = true;
            btnCancelar.Enabled = true;
            Mensaje.Text = "Modo Añadir";
        }

        private void ModoModificar_23DB()
        {
            modoActual_23DB = "Modificar";
            HabilitarCampos_23DB(true);
            txtDni.Enabled = false;
            txtLogin.Enabled = false;
            txtBloqueado.Enabled = false;
            txtActivo.Enabled = false;
            txtApellido.Enabled = false;
            txtNombre.Enabled = false;
            btnCrear.Enabled = false;
            btnModificar.Enabled = false;
            btnAct_Des.Enabled = false;
            btnDesbloquear.Enabled = false;
            btnAplicar.Enabled = true;
            btnCancelar.Enabled = true;
            Mensaje.Text = "Modo Modificar";
        }
        private void GestionUsuario_23DB_Load(object sender, EventArgs e)
        {
            ckTodos.Checked = true;
            CargarRoles_23DB();
            CargarGrilla_23DB(ObtenerFiltroActual_23DB());
            ModoConsulta_23DB();
        }

        private void ModoConsulta_23DB()
        {
            modoActual_23DB = "Consulta";
            LimpiarCampos_23DB();
            HabilitarCampos_23DB(false);
            btnCrear.Enabled = true;
            btnModificar.Enabled = true;
            btnAct_Des.Enabled = true;
            btnDesbloquear.Enabled = true;
            btnAplicar.Enabled = false;
            btnCancelar.Enabled = false;
            Mensaje.Text = "Modo Consulta";
        }

        private void HabilitarCampos_23DB(bool habilitar_23DB)
        {
            txtDni.Enabled = habilitar_23DB;
            txtApellido.Enabled = habilitar_23DB;
            txtNombre.Enabled = habilitar_23DB;
            txtEmail.Enabled = habilitar_23DB;
            cmbRol.Enabled = habilitar_23DB;
            txtActivo.Enabled = habilitar_23DB;
            txtLogin.Enabled = habilitar_23DB;
            txtBloqueado.Enabled = habilitar_23DB;
        }

        private void LimpiarCampos_23DB()
        {
            txtDni.Text = string.Empty;
            txtApellido.Text = string.Empty;
            txtNombre.Text = string.Empty;
            txtEmail.Text = string.Empty;
            txtLogin.Text = string.Empty;
            txtBloqueado.Text = string.Empty;
            txtActivo.Text = string.Empty;
            cmbRol.SelectedIndex = 0;
        }

        private void CargarGrilla_23DB(string filtro_23DB)
        {
            List<Usuario_23DB> lista_23DB = usuarioBLL_23DB.ObtenerTodos_23DB(filtro_23DB);
            dataGridUsuarios.DataSource = lista_23DB;
            lblnroUsuario.Text = "Numero de usuarios: " + lista_23DB.Count;
        }

        private void CargarRoles_23DB()
        {
            List<Rol_23DB> roles_23DB = rolBLL_23DB.ObtenerRoles_23DB();
            cmbRol.DataSource = roles_23DB;
            cmbRol.DisplayMember = "NombreRol_23DB";
            cmbRol.ValueMember = "IdRol_23DB";
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(txtDni.Text))
            {
                Mensaje.Text = "Debe seleccionar un usuario.";
                return;
            }
            ModoModificar_23DB();
        }

        private void dataGridUsuarios_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.RowIndex >= 0)
            {
                DataGridViewRow fila_23DB = dataGridUsuarios.Rows[e.RowIndex];
                txtDni.Text = fila_23DB.Cells["DNI_23DB"].Value.ToString();
                txtApellido.Text = fila_23DB.Cells["Apellido_23DB"].Value.ToString();
                txtNombre.Text = fila_23DB.Cells["Nombre_23DB"].Value.ToString();
                txtEmail.Text = fila_23DB.Cells["Email_23DB"].Value.ToString();
                txtLogin.Text = fila_23DB.Cells["Login_23DB"].Value.ToString();
                txtBloqueado.Text = fila_23DB.Cells["Bloqueado_23DB"].Value.ToString();
                txtActivo.Text = fila_23DB.Cells["Activo_23DB"].Value.ToString();
                cmbRol.SelectedValue = fila_23DB.Cells["IdRol_23DB"].Value;
            }
        }

        private void btnAplicar_Click(object sender, EventArgs e)
        {
            if(modoActual_23DB == "Anadir")
                Crear_23DB();
            else if(modoActual_23DB == "Modificar")
                Modificar_23DB();
        }

        private void Modificar_23DB()
        {
            if(string.IsNullOrEmpty(txtEmail.Text))
            {
                Mensaje.Text = "Debe completar todos los campos obligatorios.";
                return;
            }
            Usuario_23DB usuario_23DB = new Usuario_23DB
            {
                DNI_23DB = txtDni.Text,
                Apellido_23DB = txtApellido.Text,
                Nombre_23DB = txtNombre.Text,
                Email_23DB = txtEmail.Text,
                IdRol_23DB = (int)cmbRol.SelectedValue
            };

            usuarioBLL_23DB.ModificarUsuario_23DB(usuario_23DB);
            eventoBLL_23DB.RegistrarEvento_23DB(SessionManager_23DB.ObtenerInstancia_23DB().DNI_23DB, "Administrador", "Modificar Usuario", 2);
            Mensaje.Text = "Usuario modificado correctamente.";
            CargarGrilla_23DB(ObtenerFiltroActual_23DB());
            ModoConsulta_23DB();
        }

        private string ObtenerFiltroActual_23DB()
        {
            if(ckActivos.Checked) return "Activos";
            if(ckInactivos.Checked) return "Inactivos";
            return "Todos";
        }

        private void Crear_23DB()
        {
            
            if(string.IsNullOrEmpty(txtDni.Text) || string.IsNullOrEmpty(txtApellido.Text) || string.IsNullOrEmpty(txtNombre.Text) || string.IsNullOrEmpty(txtEmail.Text))
            {
                Mensaje.Text = "Debe completar todos los campos obligatorios.";
                return;
            }

            if (txtDni.Text.Length != 8)
            {
                Mensaje.Text = "El DNI debe tener exactamente 8 dígitos.";
                return;
            }

            if (!txtDni.Text.All(char.IsDigit))
            {
                Mensaje.Text = "El DNI solo puede contener números.";
                return;
            }

            Usuario_23DB usuario_23DB = new Usuario_23DB
            {
                DNI_23DB = txtDni.Text,
                Apellido_23DB = txtApellido.Text,
                Nombre_23DB = txtNombre.Text,
                Email_23DB = txtEmail.Text,
                IdRol_23DB = (int)cmbRol.SelectedValue
            };

            try
            {
                usuarioBLL_23DB.CrearUsuario_23DB(usuario_23DB);
                eventoBLL_23DB.RegistrarEvento_23DB(SessionManager_23DB.ObtenerInstancia_23DB().DNI_23DB, "Administrador", "Crear Usuario", 2);
                Mensaje.Text = "Usuario creado correctamente.";
                CargarGrilla_23DB(ObtenerFiltroActual_23DB());
                ModoConsulta_23DB();
            }
            catch (Exception ex_23DB)
            {
                Mensaje.Text = ex_23DB.Message;
            }
        }

        private void btnAct_Des_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(txtDni.Text))
            {
                Mensaje.Text = "Debe seleccionar un usuario.";
                return;
            }
            bool estadoActual_23DB = txtActivo.Text == "True";
            usuarioBLL_23DB.CambiarEstado_23DB(txtDni.Text, !estadoActual_23DB);
            eventoBLL_23DB.RegistrarEvento_23DB(SessionManager_23DB.ObtenerInstancia_23DB().DNI_23DB, "Administrador", "Activar / Desactivar Usuario", 2);
            Mensaje.Text = estadoActual_23DB ? "Usuario desactivado correctamente." : "Usuario activado correctamente.";
            CargarGrilla_23DB(ObtenerFiltroActual_23DB());
            ModoConsulta_23DB();
        }

        private void btnDesbloquear_Click(object sender, EventArgs e)
        {           
            if(string.IsNullOrEmpty(txtDni.Text))
            {
                Mensaje.Text = "Debe seleccionar un usuario.";
                return;
            }
            if(txtBloqueado.Text == "False")
            {
                Mensaje.Text = "El usuario no está bloqueado.";
                return;
            }
            usuarioBLL_23DB.DesbloquearUsuario_23DB(txtDni.Text, txtApellido.Text);
            eventoBLL_23DB.RegistrarEvento_23DB(SessionManager_23DB.ObtenerInstancia_23DB().DNI_23DB, "Administrador", "Desbloquear Usuario", 2);
            Mensaje.Text = "Usuario desbloqueado y contraseña restablecida correctamente.";
            CargarGrilla_23DB(ObtenerFiltroActual_23DB());
            ModoConsulta_23DB();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            ModoConsulta_23DB();
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ckActivos_CheckedChanged(object sender, EventArgs e)
        {
            if(ckActivos.Checked)
            {
                ckInactivos.Checked = false;
                ckTodos.Checked = false;
                CargarGrilla_23DB("Activos");
            }
        }

        private void ckInactivos_CheckedChanged(object sender, EventArgs e)
        {
            if(ckInactivos.Checked)
            {
                ckActivos.Checked = false;
                ckTodos.Checked = false;
                CargarGrilla_23DB("Inactivos");
            }
        }

        private void ckTodos_CheckedChanged(object sender, EventArgs e)
        {
            if(ckTodos.Checked)
            {
                ckActivos.Checked = false;
                ckInactivos.Checked = false;
                CargarGrilla_23DB("Todos");
            }
        }

        public void ActualizarIdioma_23DB(Dictionary<string, string> configuracion_23DB)
        {
            string formName_23DB = "GestionUsuario";
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

        private void GestionUsuario_23DB_FormClosing(object sender, FormClosingEventArgs e)
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
