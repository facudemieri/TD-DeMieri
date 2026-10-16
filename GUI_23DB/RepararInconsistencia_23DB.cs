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

            MessageBox.Show(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_DvRecalculado"), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Exito"), MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        // Muestra las tablas inconsistentes con hasta 10 claves de registros adulterados por tabla.
        private void RepararInconsistencia_23DB_Load(object sender, EventArgs e)
        {
            const int maxRegistros_23DB = 10;
            StringBuilder detalle_23DB = new StringBuilder();
            foreach(DV_23DB inconsistencia_23DB in inconsistencias_23DB)
            {
                List<string> registros_23DB = inconsistencia_23DB.RegistrosInconsistentes_23DB;
                if(registros_23DB.Count == 0)
                {
                    detalle_23DB.AppendLine($"- {inconsistencia_23DB.NombreTabla_23DB}: {IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_InconsistenciaSoloDVV")}");
                    continue;
                }
                detalle_23DB.AppendLine($"- {inconsistencia_23DB.NombreTabla_23DB}: {string.Join("; ", registros_23DB.Take(maxRegistros_23DB))}");
                if(registros_23DB.Count > maxRegistros_23DB)
                {
                    detalle_23DB.AppendLine("   " + string.Format(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_InconsistenciaMasRegistros"), registros_23DB.Count - maxRegistros_23DB));
                }
            }
            string mensaje = string.Format(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_InconsistenciaDetectada"), detalle_23DB.ToString().TrimEnd());
            MessageBox.Show(mensaje, IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Informacion"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }


        private void RepararInconsistencia_23DB_FormClosing(object sender, FormClosingEventArgs e)
        {
            
        }
    }
}
