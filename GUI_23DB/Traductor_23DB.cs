using Services_23DB;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace GUI_23DB
{

    public static class Traductor_23DB
    {
      
        private static readonly string[] sufijos_23DB = { "_23DB", "_62FD" };

        
        public static void AplicarIdioma_23DB(Form formulario_23DB, Dictionary<string, string> configuracion_23DB)
        {
            if(formulario_23DB == null || configuracion_23DB == null || configuracion_23DB.Count == 0) return;

            string nombreFormulario_23DB = ObtenerNombreFormulario_23DB(formulario_23DB);

            string titulo_23DB;
            if(configuracion_23DB.TryGetValue(nombreFormulario_23DB + "_Titulo", out titulo_23DB))
            {
                formulario_23DB.Text = titulo_23DB;
            }

            TraducirControles_23DB(formulario_23DB.Controls, nombreFormulario_23DB, configuracion_23DB);
        }

      
   
        public static void AplicarIdioma_23DB(Form formulario_23DB, Dictionary<string, string> configuracion_23DB, params ToolStrip[] menusContextuales_23DB)
        {
            AplicarIdioma_23DB(formulario_23DB, configuracion_23DB);

            if(formulario_23DB == null || configuracion_23DB == null || configuracion_23DB.Count == 0 || menusContextuales_23DB == null) return;

            string nombreFormulario_23DB = ObtenerNombreFormulario_23DB(formulario_23DB);
            foreach(ToolStrip menu_23DB in menusContextuales_23DB)
            {
                if(menu_23DB != null)
                {
                    TraducirItems_23DB(menu_23DB.Items, nombreFormulario_23DB, configuracion_23DB);
                }
            }
        }


        public static void TraducirGrilla_23DB(DataGridView grilla_23DB, string nombreFormulario_23DB)
        {
            if(grilla_23DB == null || string.IsNullOrEmpty(nombreFormulario_23DB)) return;

            Dictionary<string, string> configuracion_23DB;
            try
            {
                configuracion_23DB = IdiomaBLL_23DB.ObtenerConfiguracionActual_23DB();
            }
            catch(Exception)
            {
                return;
            }

            if(configuracion_23DB == null || configuracion_23DB.Count == 0) return;

            TraducirColumnas_23DB(grilla_23DB, nombreFormulario_23DB, configuracion_23DB);
        }


        private static string ObtenerNombreFormulario_23DB(Form formulario_23DB)
        {
            string nombre_23DB = formulario_23DB.GetType().Name;
            foreach(string sufijo_23DB in sufijos_23DB)
            {
                if(nombre_23DB.EndsWith(sufijo_23DB))
                {
                    return nombre_23DB.Substring(0, nombre_23DB.Length - sufijo_23DB.Length);
                }
            }
            return nombre_23DB;
        }


        private static void TraducirControles_23DB(Control.ControlCollection controles_23DB, string nombreFormulario_23DB, Dictionary<string, string> configuracion_23DB)
        {
            foreach(Control control_23DB in controles_23DB)
            {
                string texto_23DB;
                if(configuracion_23DB.TryGetValue(nombreFormulario_23DB + "_" + control_23DB.Name, out texto_23DB))
                {
                    control_23DB.Text = texto_23DB;
                }

 
                ToolStrip barra_23DB = control_23DB as ToolStrip;
                if(barra_23DB != null)
                {
                    TraducirItems_23DB(barra_23DB.Items, nombreFormulario_23DB, configuracion_23DB);
                }

                DataGridView grilla_23DB = control_23DB as DataGridView;
                if(grilla_23DB != null)
                {
                    TraducirColumnas_23DB(grilla_23DB, nombreFormulario_23DB, configuracion_23DB);
                }

                TraducirControles_23DB(control_23DB.Controls, nombreFormulario_23DB, configuracion_23DB);
            }
        }


        private static void TraducirItems_23DB(ToolStripItemCollection items_23DB, string nombreFormulario_23DB, Dictionary<string, string> configuracion_23DB)
        {
            foreach(ToolStripItem item_23DB in items_23DB)
            {
                string texto_23DB;
                if(configuracion_23DB.TryGetValue(nombreFormulario_23DB + "_" + item_23DB.Name, out texto_23DB))
                {
                    item_23DB.Text = texto_23DB;
                }

                ToolStripDropDownItem desplegable_23DB = item_23DB as ToolStripDropDownItem;
                if(desplegable_23DB != null)
                {
                    TraducirItems_23DB(desplegable_23DB.DropDownItems, nombreFormulario_23DB, configuracion_23DB);
                }
            }
        }


        private static void TraducirColumnas_23DB(DataGridView grilla_23DB, string nombreFormulario_23DB, Dictionary<string, string> configuracion_23DB)
        {
            foreach(DataGridViewColumn columna_23DB in grilla_23DB.Columns)
            {
                string texto_23DB;
                if(configuracion_23DB.TryGetValue(nombreFormulario_23DB + "_" + columna_23DB.Name, out texto_23DB))
                {
                    columna_23DB.HeaderText = texto_23DB;
                }
            }
        }
    }
}
