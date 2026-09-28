using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace DAL_23DB
{
    public class Conexion_23DB
    {
        public static string ObtenerCadena_23DB()
        {
            string servidor_23DB = ".";

            using (RegistryKey baseKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32))

            using (RegistryKey key = baseKey.OpenSubKey(@"SOFTWARE\AeroManager"))
            {
                if (key != null)
                {
                    servidor_23DB = key.GetValue("Server", ".").ToString();
                }
            }

            return $"Server={servidor_23DB};Database=INGSW_23DB;Integrated Security=True;";
        }
    }
}
