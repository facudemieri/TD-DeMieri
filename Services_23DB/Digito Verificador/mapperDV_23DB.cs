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
    public class mapperDV_23DB
    {
        Acceso_23DB acc_23DB = new Acceso_23DB();

        public int ActualizarDV_23DB(int idTabla_23DB, long dvh_23DB, long dvv_23DB)
        {
            int fa_23DB = 0;
            SqlParameter[] sp_23DB = new SqlParameter[3];
            sp_23DB[0] = new SqlParameter("@IdTabla", idTabla_23DB);
            sp_23DB[1] = new SqlParameter("@DVH", dvh_23DB);
            sp_23DB[2] = new SqlParameter("@DVV", dvv_23DB);
            fa_23DB = acc_23DB.Escribir_23DB("ActualizarDV", sp_23DB);
            return fa_23DB;
        }

        public DV_23DB ObtenerDV_23DB(int idTabla_23DB)
        {
            DV_23DB dv_23DB = null;
            SqlParameter[] sp_23DB = new SqlParameter[1];
            sp_23DB[0] = new SqlParameter("@IdTabla", idTabla_23DB);
            DataTable dt_23DB = acc_23DB.Leer_23DB("ObtenerDV", sp_23DB);
            if(dt_23DB.Rows.Count > 0)
            {
                DataRow dr_23DB = dt_23DB.Rows[0];
                dv_23DB = new DV_23DB
                {
                    IdTabla_23DB = (int)dr_23DB["IdTabla"],
                    NombreTabla_23DB = dr_23DB["NombreTabla"].ToString(),
                    DVH_23DB = (long)dr_23DB["DVH"],
                    DVV_23DB = (long)dr_23DB["DVV"]
                };
            }
            return dv_23DB;
        }

        public List<DV_23DB> ObtenerTodosDV_23DB()
        {
            List<DV_23DB> lista_23DB = new List<DV_23DB>();
            DataTable dt_23DB = acc_23DB.Leer_23DB("ObtenerTodosDV", null);
            foreach(DataRow dr_23DB in dt_23DB.Rows)
            {
                lista_23DB.Add(new DV_23DB
                {
                    IdTabla_23DB = (int)dr_23DB["IdTabla"],
                    NombreTabla_23DB = dr_23DB["NombreTabla"].ToString(),
                    DVH_23DB = (long)dr_23DB["DVH"],
                    DVV_23DB = (long)dr_23DB["DVV"]
                });
            }
            return lista_23DB;
        }

        public DataTable ObtenerDatosTabla_23DB(string nombreTabla_23DB)
        {
            SqlParameter[] sp_23DB = new SqlParameter[1];
            sp_23DB[0] = new SqlParameter("@NombreTabla", (object)nombreTabla_23DB ?? DBNull.Value);
            return acc_23DB.Leer_23DB("ObtenerDatosTabla", sp_23DB);
        }
    }
}
