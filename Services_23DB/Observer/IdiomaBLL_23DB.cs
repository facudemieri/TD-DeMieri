using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Services_23DB
{
    public class IdiomaBLL_23DB
    {
        private static string rutaIdiomas_23DB = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Idiomas");


        private const string idiomaBase_23DB = "español";


        private static Dictionary<string, string> textosEnCache_23DB = null;
        private static string idiomaEnCache_23DB = null;

        public List<string> ObtenerIdiomas_23DB()
        {
            List<string> idiomas_23DB = new List<string>();
            foreach(string archivo_23DB in Directory.GetFiles(rutaIdiomas_23DB, "*.json"))
            {
                idiomas_23DB.Add(Path.GetFileNameWithoutExtension(archivo_23DB));
            }
            return idiomas_23DB;
        }

        public Dictionary<string, string> CargarConfiguracion_23DB(string idioma_23DB)
        {
            string ruta_23DB = Path.Combine(rutaIdiomas_23DB, idioma_23DB + ".json");
            string json_23DB = File.ReadAllText(ruta_23DB);
            return JsonConvert.DeserializeObject<Dictionary<string, string>>(json_23DB);
        }

        public void CambiarIdioma_23DB(string idioma_23DB)
        {
            textosEnCache_23DB = null; 
            Dictionary<string, string> configuracion_23DB = CargarConfiguracion_23DB(idioma_23DB);
            Observer_23DB.ObtenerInstancia_23DB().Notificar_23DB(configuracion_23DB);
        }

        public static string ObtenerTexto_23DB(string clave_23DB)
        {
            if(string.IsNullOrEmpty(clave_23DB)) return string.Empty;

            string texto_23DB;
            if(ObtenerConfiguracionActual_23DB().TryGetValue(clave_23DB, out texto_23DB) && !string.IsNullOrEmpty(texto_23DB))
            {
                return texto_23DB;
            }
            return clave_23DB;
        }


        public static Dictionary<string, string> ObtenerConfiguracionActual_23DB()
        {
            string idioma_23DB = SessionManager_23DB.ObtenerInstancia_23DB().UltimoIdioma_23DB;
            if(string.IsNullOrEmpty(idioma_23DB))
            {
                idioma_23DB = idiomaBase_23DB;
            }

            if(textosEnCache_23DB == null || idioma_23DB != idiomaEnCache_23DB)
            {
                textosEnCache_23DB = CargarConRespaldo_23DB(idioma_23DB);
                idiomaEnCache_23DB = idioma_23DB;
            }
            return textosEnCache_23DB;
        }


        private static Dictionary<string, string> CargarConRespaldo_23DB(string idioma_23DB)
        {
            IdiomaBLL_23DB idiomaBLL_23DB = new IdiomaBLL_23DB();
            Dictionary<string, string> resultado_23DB = new Dictionary<string, string>();

            try
            {
                resultado_23DB = idiomaBLL_23DB.CargarConfiguracion_23DB(idiomaBase_23DB) ?? new Dictionary<string, string>();
            }
            catch(Exception)
            {
                // sin archivo base se devuelven las claves crudas
            }

            if(idioma_23DB != idiomaBase_23DB)
            {
                try
                {
                    Dictionary<string, string> activo_23DB = idiomaBLL_23DB.CargarConfiguracion_23DB(idioma_23DB);
                    if(activo_23DB != null)
                    {
                        foreach(KeyValuePair<string, string> par_23DB in activo_23DB)
                        {
                            if(!string.IsNullOrEmpty(par_23DB.Value))
                            {
                                resultado_23DB[par_23DB.Key] = par_23DB.Value;
                            }
                        }
                    }
                }
                catch(Exception)
                {
                    // idioma inexistente o ilegible: queda el español
                }
            }

            return resultado_23DB;
        }
    }
}
