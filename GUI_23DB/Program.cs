using Services_23DB.Singleton;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI_23DB
{
    internal static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            RegistroManager_23DB.GarantizarUsuarioRecuperacion_23DB();  

            Application.Run(new InicioSesion_23DB());
        }
    }
}
