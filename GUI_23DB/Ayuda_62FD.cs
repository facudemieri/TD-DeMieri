using Services_23DB;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace GUI_23DB
{
    public static class Ayuda_62FD
    {
        public static void Mostrar_23DB(string tema_23DB)
        {
            string carpeta_23DB = Path.Combine(Application.StartupPath, "Ayuda");
            string ruta_23DB = Path.Combine(carpeta_23DB, tema_23DB + ".html");

            if(!File.Exists(ruta_23DB))
            {
                ruta_23DB = Path.Combine(carpeta_23DB, "index.html");
            }

            if(!File.Exists(ruta_23DB))
            {
                MessageBox.Show(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_AyudaNoDisponible"), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Advertencia"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                ProcessStartInfo inicio_23DB = new ProcessStartInfo(ruta_23DB);
                inicio_23DB.UseShellExecute = true;
                Process.Start(inicio_23DB);
            }
            catch(Exception ex_23DB)
            {
                MessageBox.Show(string.Format(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_ErrorAbrirAyuda"), ex_23DB.Message), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static void Vincular_23DB(Form formulario_23DB, string tema_23DB)
        {
            formulario_23DB.HelpRequested += (sender_23DB, e_23DB) =>
            {
                e_23DB.Handled = true;
                Mostrar_23DB(tema_23DB);
            };
        }
    }
}
