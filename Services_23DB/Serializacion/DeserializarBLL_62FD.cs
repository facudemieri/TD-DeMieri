using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Services_23DB.Serializacion
{
    public class DeserializarBLL_62FD
    {
        public List<T> Deserializar_62DB<T>(string rutaArchivo_62DB)
        {
            XmlSerializer serializador_62DB = new XmlSerializer(typeof(List<T>));

            using (StreamReader lector_62DB = new StreamReader(rutaArchivo_62DB))
            {
                return (List<T>)serializador_62DB.Deserialize(lector_62DB);
            }
        }
    }
}
