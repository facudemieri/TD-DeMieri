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
    public partial class CambiarIdioma_23DB : Form, IIdiomaObserver_23DB
    {
        private IdiomaBLL_23DB idiomaBLL_23DB = new IdiomaBLL_23DB();

        
        public CambiarIdioma_23DB()
        {
            InitializeComponent();
            Observer_23DB.ObtenerInstancia_23DB().Suscribir_23DB(this);
            AplicarIdiomaActual_23DB();
        }

        public void ActualizarIdioma_23DB(Dictionary<string, string> configuracion_23DB)
        {
            Traductor_23DB.AplicarIdioma_23DB(this, configuracion_23DB);
        }

        private void CambiarIdioma_23DB_FormClosing(object sender, FormClosingEventArgs e)
        {
            Observer_23DB.ObtenerInstancia_23DB().Desuscribir_23DB(this);
        }
        private void btnLogin_Click(object sender, EventArgs e)
        {
            if(cmbIdioma.SelectedIndex < 0)
            {
                MessageBox.Show(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_SeleccioneIdioma"), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Advertencia"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string idiomaSeleccionado_23DB = cmbIdioma.SelectedItem.ToString();
            string idiomaActual_23DB = SessionManager_23DB.ObtenerInstancia_23DB().UltimoIdioma_23DB;

            if(idiomaSeleccionado_23DB == idiomaActual_23DB)
            {
                MessageBox.Show(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_IdiomaEnUso"), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Advertencia"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // la sesión se actualiza ANTES de notificar, para que ObtenerTexto_23DB ya
            // devuelva el idioma nuevo cuando los formularios reconstruyen sus textos dinámicos
            SessionManager_23DB.ObtenerInstancia_23DB().UltimoIdioma_23DB = idiomaSeleccionado_23DB;
            idiomaBLL_23DB.CambiarIdioma_23DB(idiomaSeleccionado_23DB);

            UsuarioBLL_23DB usuarioBLL_23DB = new UsuarioBLL_23DB();
            usuarioBLL_23DB.ActualizarUltimoIdioma_23DB(SessionManager_23DB.ObtenerInstancia_23DB().DNI_23DB, idiomaSeleccionado_23DB);

            EventoBLL_23DB eventoBLL_23DB = new EventoBLL_23DB();
            eventoBLL_23DB.RegistrarEvento_23DB(SessionManager_23DB.ObtenerInstancia_23DB().DNI_23DB, "Usuarios", "Cambiar Idioma", 2);

            MessageBox.Show(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_IdiomaCambiado"), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Exito"), MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CambiarIdioma_23DB_Load(object sender, EventArgs e)
        {
            List<string> idiomas_23DB = idiomaBLL_23DB.ObtenerIdiomas_23DB();
            cmbIdioma.DataSource = idiomas_23DB;

            string idiomaActual_23DB = SessionManager_23DB.ObtenerInstancia_23DB().UltimoIdioma_23DB;
            if(!string.IsNullOrEmpty(idiomaActual_23DB) && idiomas_23DB.Contains(idiomaActual_23DB))
            {
                cmbIdioma.SelectedItem = idiomaActual_23DB;
            }
            else
            {
                cmbIdioma.SelectedIndex = -1;
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
    }
}
