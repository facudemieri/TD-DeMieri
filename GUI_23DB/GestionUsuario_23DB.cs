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


        private string claveMensaje_23DB = null;
        private int? cantidadUsuarios_23DB = null;



        private List<Usuario_23DB> listaUsuarios_23DB = new List<Usuario_23DB>();
        private string columnaOrden_23DB = null;
        private bool ordenAscendente_23DB = true;

        public GestionUsuario_23DB()
        {
            InitializeComponent();
            Observer_23DB.ObtenerInstancia_23DB().Suscribir_23DB(this);
            AplicarIdiomaActual_23DB();

            ConfigurarGrilla_62FD(dataGridUsuarios);
            // Once columnas visibles: en Fill quedan ilegibles, cada una toma el ancho de su contenido con barra horizontal
            dataGridUsuarios.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridUsuarios.ScrollBars = ScrollBars.Both;
            dataGridUsuarios.Paint += Grilla_Paint_62FD;
            dataGridUsuarios.ColumnHeaderMouseClick += dataGridUsuarios_ColumnHeaderMouseClick;
            this.Shown += (s_62FD, e_62FD) => LimpiarSeleccionGrilla_62FD(dataGridUsuarios);

            // El rol se elige de la lista: con texto libre SelectedValue quedaba en null y el casteo a int fallaba
            cmbRol.DropDownStyle = ComboBoxStyle.DropDownList;

            // Largos iguales a las columnas de Usuario_23DB y a los parámetros de Insertar / Modificar
            txtDni.MaxLength = 8;
            txtApellido.MaxLength = 50;
            txtNombre.MaxLength = 50;
            txtEmail.MaxLength = 100;
            txtLogin.MaxLength = 20;
        }

        // Avisos al usuario con el mismo criterio de íconos y títulos que el resto del sistema:
        // información para indicaciones, advertencia para datos incompletos o inválidos, éxito para operaciones terminadas.
        private void MostrarInformacion_23DB(string clave_23DB)
        {
            MessageBox.Show(IdiomaBLL_23DB.ObtenerTexto_23DB(clave_23DB), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Informacion"), MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void MostrarAdvertencia_23DB(string clave_23DB)
        {
            MessageBox.Show(IdiomaBLL_23DB.ObtenerTexto_23DB(clave_23DB), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Advertencia"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void MostrarExito_23DB(string clave_23DB)
        {
            MessageBox.Show(IdiomaBLL_23DB.ObtenerTexto_23DB(clave_23DB), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Exito"), MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void MostrarError_23DB(string clave_23DB, Exception ex_23DB)
        {
            MessageBox.Show(string.Format(IdiomaBLL_23DB.ObtenerTexto_23DB(clave_23DB), ex_23DB.Message), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            MostrarMensaje_23DB("Msg_ModoAnadir");
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
            MostrarMensaje_23DB("Msg_ModoModificar");
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
            MostrarMensaje_23DB("Msg_ModoConsulta");
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
            EnlazarGrilla_23DB(lista_23DB);

            cantidadUsuarios_23DB = lista_23DB.Count;
            MostrarTextos_23DB();
        }

        // Las columnas son autogeneradas y se recrean en cada enlace: traducción, columna oculta, formatos y modo de orden se aplican cada vez
        private void EnlazarGrilla_23DB(List<Usuario_23DB> lista_23DB)
        {
            listaUsuarios_23DB = lista_23DB;
            columnaOrden_23DB = null;

            dataGridUsuarios.DataSource = null;
            dataGridUsuarios.DataSource = listaUsuarios_23DB;
            Traductor_23DB.TraducirGrilla_23DB(dataGridUsuarios, "GestionUsuario");

            if (dataGridUsuarios.Columns["Password_23DB"] != null)
            {
                dataGridUsuarios.Columns["Password_23DB"].Visible = false;
            }

            ConfigurarColumnas_23DB();
            LimpiarSeleccionGrilla_62FD(dataGridUsuarios);
        }

        private void ConfigurarColumnas_23DB()
        {
            foreach (DataGridViewColumn columna_23DB in dataGridUsuarios.Columns)
            {
                columna_23DB.SortMode = DataGridViewColumnSortMode.Programmatic;
                columna_23DB.MinimumWidth = 60;

                switch (columna_23DB.DataPropertyName)
                {
                    case "DNI_23DB":
                    case "IdRol_23DB":
                    case "Bloqueado_23DB":
                    case "Activo_23DB":
                        columna_23DB.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        break;
                    case "IntentosFallidos_23DB":
                        columna_23DB.DefaultCellStyle.Format = "N0";
                        columna_23DB.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                        break;
                    case "FechaUltimoIntento_23DB":
                        columna_23DB.DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
                        columna_23DB.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        columna_23DB.DefaultCellStyle.NullValue = "-";
                        break;
                }
            }
        }

        private void dataGridUsuarios_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string propiedad_23DB = dataGridUsuarios.Columns[e.ColumnIndex].DataPropertyName;
            bool ascendente_23DB = columnaOrden_23DB == propiedad_23DB ? !ordenAscendente_23DB : true;

            EnlazarGrilla_23DB(Ordenar_62FD(listaUsuarios_23DB, propiedad_23DB, ascendente_23DB));
            columnaOrden_23DB = propiedad_23DB;
            ordenAscendente_23DB = ascendente_23DB;

            // Las columnas se recrearon al reenlazar: el glifo se pone sobre la columna nueva
            dataGridUsuarios.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection = ascendente_23DB ? SortOrder.Ascending : SortOrder.Descending;
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
                MostrarInformacion_23DB("Msg_SeleccioneUsuario");
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
                MostrarAdvertencia_23DB("Msg_CompletarCamposObligatorios");
                return;
            }

            if(cmbRol.SelectedValue == null)
            {
                MostrarAdvertencia_23DB("Msg_SeleccioneRol");
                return;
            }

            Usuario_23DB usuario_23DB = new Usuario_23DB
            {
                DNI_23DB = txtDni.Text,
                Apellido_23DB = txtApellido.Text,
                Nombre_23DB = txtNombre.Text,
                Email_23DB = txtEmail.Text,
                IdRol_23DB = Convert.ToInt32(cmbRol.SelectedValue)
            };

            try
            {
                usuarioBLL_23DB.ModificarUsuario_23DB(usuario_23DB);
                eventoBLL_23DB.RegistrarEvento_23DB(SessionManager_23DB.ObtenerInstancia_23DB().DNI_23DB, "Administrador", "Modificar Usuario", 2);
                MostrarExito_23DB("Msg_UsuarioModificado");
                CargarGrilla_23DB(ObtenerFiltroActual_23DB());
                ModoConsulta_23DB();
            }
            catch (Exception ex_23DB)
            {
                MostrarError_23DB("Msg_ErrorModificarUsuario", ex_23DB);
            }
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
                MostrarAdvertencia_23DB("Msg_CompletarCamposObligatorios");
                return;
            }

            if (txtDni.Text.Length != 8)
            {
                MostrarAdvertencia_23DB("Msg_DniOchoDigitos");
                return;
            }

            if (!txtDni.Text.All(char.IsDigit))
            {
                MostrarAdvertencia_23DB("Msg_DniSoloNumeros");
                return;
            }

            if (cmbRol.SelectedValue == null)
            {
                MostrarAdvertencia_23DB("Msg_SeleccioneRol");
                return;
            }

            Usuario_23DB usuario_23DB = new Usuario_23DB
            {
                DNI_23DB = txtDni.Text,
                Apellido_23DB = txtApellido.Text,
                Nombre_23DB = txtNombre.Text,
                Email_23DB = txtEmail.Text,
                IdRol_23DB = Convert.ToInt32(cmbRol.SelectedValue)
            };

            try
            {
                usuarioBLL_23DB.CrearUsuario_23DB(usuario_23DB);
                eventoBLL_23DB.RegistrarEvento_23DB(SessionManager_23DB.ObtenerInstancia_23DB().DNI_23DB, "Administrador", "Crear Usuario", 2);
                MostrarExito_23DB("Msg_UsuarioCreado");
                CargarGrilla_23DB(ObtenerFiltroActual_23DB());
                ModoConsulta_23DB();
            }
            catch (Exception ex_23DB)
            {
                MostrarError_23DB("Msg_ErrorCrearUsuario", ex_23DB);
            }
        }

        private void btnAct_Des_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(txtDni.Text))
            {
                MostrarInformacion_23DB("Msg_SeleccioneUsuario");
                return;
            }
            bool estadoActual_23DB = txtActivo.Text == "True";
            usuarioBLL_23DB.CambiarEstado_23DB(txtDni.Text, !estadoActual_23DB);
            eventoBLL_23DB.RegistrarEvento_23DB(SessionManager_23DB.ObtenerInstancia_23DB().DNI_23DB, "Administrador", "Activar / Desactivar Usuario", 1);
            MostrarExito_23DB(estadoActual_23DB ? "Msg_UsuarioDesactivado" : "Msg_UsuarioActivado");
            CargarGrilla_23DB(ObtenerFiltroActual_23DB());
            ModoConsulta_23DB();
        }

        private void btnDesbloquear_Click(object sender, EventArgs e)
        {           
            if(string.IsNullOrEmpty(txtDni.Text))
            {
                MostrarInformacion_23DB("Msg_SeleccioneUsuario");
                return;
            }
            if(txtBloqueado.Text == "False")
            {
                MostrarInformacion_23DB("Msg_UsuarioNoBloqueado");
                return;
            }
            usuarioBLL_23DB.DesbloquearUsuario_23DB(txtDni.Text, txtApellido.Text);
            eventoBLL_23DB.RegistrarEvento_23DB(SessionManager_23DB.ObtenerInstancia_23DB().DNI_23DB, "Administrador", "Desbloquear Usuario", 1);
            MostrarExito_23DB("Msg_UsuarioDesbloqueado");
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
            Traductor_23DB.AplicarIdioma_23DB(this, configuracion_23DB);
            MostrarTextos_23DB();
        }

        // La etiqueta Mensaje queda solo para el modo de la pantalla; los avisos van por MessageBox
        private void MostrarMensaje_23DB(string claveMensaje_23DB)
        {
            this.claveMensaje_23DB = claveMensaje_23DB;
            MostrarTextos_23DB();
        }

        private void MostrarTextos_23DB()
        {
            if(claveMensaje_23DB != null)
            {
                Mensaje.Text = IdiomaBLL_23DB.ObtenerTexto_23DB(claveMensaje_23DB);
            }

            if(cantidadUsuarios_23DB.HasValue)
            {
                lblnroUsuario.Text = string.Format(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_NumeroUsuarios"), cantidadUsuarios_23DB.Value);
            }
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
