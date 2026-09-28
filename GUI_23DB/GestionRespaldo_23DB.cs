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
    public partial class GestionRespaldo_23DB : Form, IIdiomaObserver_23DB
    {
        private EventoBLL_23DB eventoBLL_23DB = new EventoBLL_23DB();
        private RespaldoBLL_23DB respaldoBLL_23DB = new RespaldoBLL_23DB();
        

        public bool SoloRestore_23DB { get; set; } = false;
        public GestionRespaldo_23DB()
        {
            InitializeComponent();
            Observer_23DB.ObtenerInstancia_23DB().Suscribir_23DB(this);
            AplicarIdiomaActual_23DB();
        }

        void LimpiarCampos()
        {
            txtRutaBackup.Text = string.Empty;
            txtRutaRestore.Text = string.Empty;
        }
        private void btnSeleccionarRuta_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog_23DB = new FolderBrowserDialog();
            dialog_23DB.Description = "Seleccione la carpeta donde guardar el backup";
            if(dialog_23DB.ShowDialog() == DialogResult.OK)
            {
                txtRutaBackup.Text = dialog_23DB.SelectedPath;
            }            
        }

        private void btnSeleccionarArchivo_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog_23DB = new OpenFileDialog();
            dialog_23DB.Filter = "Backup files (*.bak)|*.bak|All files (*.*)|*.*";
            dialog_23DB.Title = "Seleccione el archivo de backup";
            if(dialog_23DB.ShowDialog() == DialogResult.OK)
            {
                txtRutaRestore.Text = dialog_23DB.FileName;
            }                
        }

        private void btnBackup_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(txtRutaBackup.Text))
            {
                MessageBox.Show("Debe seleccionar una ruta para el backup.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                string rutaCompleta_23DB = respaldoBLL_23DB.GenerarBackup_23DB(txtRutaBackup.Text);
                // El backup ya se genero: un fallo de la bitacora no debe informarse como error del backup.
                try
                {
                    eventoBLL_23DB.RegistrarEvento_23DB(SessionManager_23DB.ObtenerInstancia_23DB().DNI_23DB, "Administrador", "Backup BD", 2);
                }
                catch(Exception exEvento_23DB)
                {
                    System.Diagnostics.Debug.WriteLine(exEvento_23DB);
                }
                MessageBox.Show($"Backup generado correctamente:\n{rutaCompleta_23DB}", "Exito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LimpiarCampos();
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error al generar el backup: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRestore_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(txtRutaRestore.Text))
            {
                MessageBox.Show("Debe seleccionar un archivo de backup.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult confirmar_23DB = MessageBox.Show("¿Está seguro que desea restaurar la base de datos? Se perderán los datos posteriores al backup.", "Confirmar Restore", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if(confirmar_23DB != DialogResult.Yes) return;

            try
            {
                respaldoBLL_23DB.RestaurarBackup_23DB(txtRutaRestore.Text);

                if(SoloRestore_23DB)
                {
                    MessageBox.Show("Base de datos restaurada correctamente. Inicie sesión nuevamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    LimpiarCampos();
                    this.Close();
                }
                else
                {
                    // La base ya se restauro: un fallo de la bitacora no debe informarse como error del restore.
                    try
                    {
                        eventoBLL_23DB.RegistrarEvento_23DB(SessionManager_23DB.ObtenerInstancia_23DB().DNI_23DB, "Administrador", "Restore BD", 2);
                    }
                    catch(Exception exEvento_23DB)
                    {
                        System.Diagnostics.Debug.WriteLine(exEvento_23DB);
                    }
                    MessageBox.Show("Base de datos restaurada correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LimpiarCampos();                    
                }
            }
            catch(Exception ex_23DB)
            {
                MessageBox.Show("Error al restaurar la base de datos: " + ex_23DB.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void GestionRespaldo_23DB_Load(object sender, EventArgs e)
        {
            if(SoloRestore_23DB)
            {
                txtRutaBackup.Enabled = false;
                btnSeleccionarRuta.Enabled = false;
                btnBackup.Enabled = false;
            }
        }

        public void ActualizarIdioma_23DB(Dictionary<string, string> configuracion_23DB)
        {
            string formName_23DB = "GestionRespaldo";
            foreach(Control control_23DB in ObtenerTodosControles_23DB(this))
            {
                string clave_23DB = formName_23DB + "_" + control_23DB.Name;
                if(configuracion_23DB.ContainsKey(clave_23DB))
                {
                    control_23DB.Text = configuracion_23DB[clave_23DB];
                }
            }
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

        private void GestionRespaldo_23DB_FormClosing(object sender, FormClosingEventArgs e)
        {
            Observer_23DB.ObtenerInstancia_23DB().Desuscribir_23DB(this);
        }
    }
}
