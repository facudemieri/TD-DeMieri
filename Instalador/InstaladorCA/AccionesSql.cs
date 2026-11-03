using Microsoft.Deployment.WindowsInstaller;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;

namespace InstaladorCA
{
    // Acción personalizada del instalador. Corre en el asistente, antes de copiar archivos,
    // y otra vez al comienzo de la ejecución para cubrir la instalación silenciosa.
    public static class AccionesSql
    {
        private const string Sqlcmd = "SQLCMD.EXE";

        [CustomAction]
        public static ActionResult ValidarServidorSql(Session session)
        {
            session["SQLCHECK_OK"] = "";
            session["SQLCHECK_MSG"] = "";
            session["SQLLOGDIR"] = Path.GetTempPath();

            string rutaSqlcmd = BuscarSqlcmd();

            if (rutaSqlcmd == null)
            {
                session["SQLCHECK_MSG"] = "No se encontró sqlcmd en este equipo. Instale Microsoft Command Line Utilities for SQL Server (sqlcmd), versión 17 o 18, y vuelva a ejecutar el instalador.";
                return ActionResult.Success;
            }

            session["SQLCMDEXE"] = rutaSqlcmd;
            session.Log("sqlcmd encontrado en: " + rutaSqlcmd);

            string servidor = session["SQLSERVERNAME"];

            if (string.IsNullOrWhiteSpace(servidor))
            {
                session["SQLCHECK_MSG"] = "Ingrese el nombre del servidor SQL.";
                return ActionResult.Success;
            }

            try
            {
                SqlConnectionStringBuilder cadena = new SqlConnectionStringBuilder();
                cadena.DataSource = servidor.Trim();
                cadena.InitialCatalog = "master";
                cadena.IntegratedSecurity = true;
                cadena.ConnectTimeout = 8;
                cadena.Pooling = false;

                using (SqlConnection conexion = new SqlConnection(cadena.ConnectionString))
                {
                    conexion.Open();
                }
            }
            catch (Exception ex)
            {
                session.Log("No se pudo conectar con " + servidor + ": " + ex.Message);
                session["SQLCHECK_MSG"] = "No se pudo conectar con el servidor SQL \"" + servidor + "\".\r\n\r\n" + ex.Message;
                return ActionResult.Success;
            }

            session["SQLCHECK_OK"] = "1";
            return ActionResult.Success;
        }

        // Toma la versión más nueva de las instaladas por ODBC (170, 180 y las que vengan),
        // después las herramientas que trae SQL Server y por último lo que esté en el PATH.
        private static string BuscarSqlcmd()
        {
            List<string> candidatos = new List<string>();

            foreach (string programas in CarpetasDeProgramas())
            {
                string odbc = Path.Combine(programas, "Microsoft SQL Server", "Client SDK", "ODBC");

                foreach (string version in SubcarpetasNumericas(odbc))
                {
                    candidatos.Add(Path.Combine(version, "Tools", "Binn", Sqlcmd));
                }
            }

            foreach (string programas in CarpetasDeProgramas())
            {
                string raiz = Path.Combine(programas, "Microsoft SQL Server");

                foreach (string version in SubcarpetasNumericas(raiz))
                {
                    candidatos.Add(Path.Combine(version, "Tools", "Binn", Sqlcmd));
                }

                candidatos.Add(Path.Combine(raiz, "Tools", "Binn", Sqlcmd));
            }

            string path = Environment.GetEnvironmentVariable("PATH") ?? "";

            foreach (string carpeta in path.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries))
            {
                try
                {
                    candidatos.Add(Path.Combine(carpeta.Trim().Trim('"'), Sqlcmd));
                }
                catch (ArgumentException)
                {
                    // Entrada inválida del PATH: se ignora
                }
            }

            return candidatos.FirstOrDefault(File.Exists);
        }

        // ProgramW6432 es la carpeta real de 64 bits: este proceso es de 32 bits
        private static IEnumerable<string> CarpetasDeProgramas()
        {
            string[] variables = { "ProgramW6432", "ProgramFiles", "ProgramFiles(x86)" };

            return variables
                .Select(Environment.GetEnvironmentVariable)
                .Where(ruta => !string.IsNullOrEmpty(ruta))
                .Distinct(StringComparer.OrdinalIgnoreCase);
        }

        private static IEnumerable<string> SubcarpetasNumericas(string carpeta)
        {
            if (!Directory.Exists(carpeta))
            {
                return Enumerable.Empty<string>();
            }

            int numero;

            return Directory.GetDirectories(carpeta)
                .Where(ruta => int.TryParse(Path.GetFileName(ruta), out numero))
                .OrderByDescending(ruta => int.Parse(Path.GetFileName(ruta)));
        }
    }
}
