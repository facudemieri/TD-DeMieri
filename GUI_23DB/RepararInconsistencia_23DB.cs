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
    public partial class RepararInconsistencia_23DB : Form
    {
        private DVBLL_23DB dvBLL_23DB = new DVBLL_23DB();
        private EventoBLL_23DB eventoBLL_23DB = new EventoBLL_23DB();
        private List<DV_23DB> inconsistencias_23DB;

        public RepararInconsistencia_23DB(List<DV_23DB> inconsistencias_23DB)
        {
            InitializeComponent();
            this.inconsistencias_23DB = inconsistencias_23DB;

        }
        private void btnRecalcular_Click(object sender, EventArgs e)
        {
            dvBLL_23DB.RecalcularDV_23DB();

            string dni_23DB = SessionManager_23DB.ObtenerInstancia_23DB().DNI_23DB;
            if(!string.IsNullOrEmpty(dni_23DB))
            {
                eventoBLL_23DB.RegistrarEvento_23DB(dni_23DB, "Administrador", "Recalcular DV", 1);
            }

            MessageBox.Show("DV recalculado correctamente. Inicie sesión nuevamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            Application.Exit();
        }

        private void btnRestore_Click(object sender, EventArgs e)
        {
            GestionRespaldo_23DB gestionRespaldo_23DB = new GestionRespaldo_23DB();
            gestionRespaldo_23DB.SoloRestore_23DB = true;
            DialogResult resultado_23DB = gestionRespaldo_23DB.ShowDialog();
            
            if(resultado_23DB == DialogResult.OK)
            {               
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void RepararInconsistencia_23DB_Load(object sender, EventArgs e)
        {
            string tablas_23DB = string.Join(", ", inconsistencias_23DB.ConvertAll(i => i.NombreTabla_23DB));
            string mensaje = $"Se ha detectado una inconsistencia en: {tablas_23DB}. Seleccione un camino.";
            MessageBox.Show(mensaje, "Informacion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }


        private void RepararInconsistencia_23DB_FormClosing(object sender, FormClosingEventArgs e)
        {
            
        }
    }
}
