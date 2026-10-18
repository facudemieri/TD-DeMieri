using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Services_23DB.Serializacion
{
    public class SerializarBLL_62FD
    {
        public void Serializar_62DB<T>(List<T> lista_62DB, string rutaArchivo_62DB)
        {
            XmlSerializer serializador_62DB = new XmlSerializer(typeof(List<T>));

            using (StreamWriter escritor_62DB = new StreamWriter(rutaArchivo_62DB))
            {
                serializador_62DB.Serialize(escritor_62DB, lista_62DB);
            }
        }
    }
}
