using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL_23DB;

namespace Services_23DB
{
    public class mapperPatente_23DB
    {
        Acceso_23DB acc_23DB = new Acceso_23DB();

        public List<Patente_23DB> ObtenerPatentes_23DB()
        {
            List<Patente_23DB> lista_23DB = new List<Patente_23DB>();
            DataTable dt_23DB = acc_23DB.Leer_23DB("ObtenerPatentes", null);
            foreach(DataRow dr_23DB in dt_23DB.Rows)
            {
                lista_23DB.Add(new Patente_23DB
                {
                    IdPatente_23DB = (int)dr_23DB["IdPatente"],
                    NombrePatente_23DB = dr_23DB["NombrePatente"].ToString(),
                    Descripcion_23DB = dr_23DB["Descripcion"].ToString()
                });
            }
            return lista_23DB;
        }
    }
}
