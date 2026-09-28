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
            Dictionary<string, string> configuracion_23DB = CargarConfiguracion_23DB(idioma_23DB);
            Observer_23DB.ObtenerInstancia_23DB().Notificar_23DB(configuracion_23DB);
        }
    }
}
