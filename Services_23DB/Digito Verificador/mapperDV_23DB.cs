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

        // Guarda el DVV de una tabla en DV_23DB.
        public int ActualizarDVV_23DB(string nombreTabla_23DB, long dvv_23DB)
        {
            int fa_23DB = 0;
            SqlParameter[] sp_23DB = new SqlParameter[2];
            sp_23DB[0] = new SqlParameter("@NombreTabla", nombreTabla_23DB);
            sp_23DB[1] = new SqlParameter("@DVV", SqlDbType.BigInt) { Value = dvv_23DB };
            fa_23DB = acc_23DB.Escribir_23DB("ActualizarDVV_62FD", sp_23DB);
            return fa_23DB;
        }

        // Guarda el DVH de un registro identificado por su clave (simple o compuesta de hasta 2 columnas).
        public int ActualizarDVHRegistro_23DB(string nombreTabla_23DB, string[] columnasPK_23DB, string[] valoresPK_23DB, long dvh_23DB)
        {
            if(columnasPK_23DB == null || valoresPK_23DB == null || columnasPK_23DB.Length == 0 || columnasPK_23DB.Length != valoresPK_23DB.Length)
            {
                throw new Exception($"La clave informada para la tabla {nombreTabla_23DB} no es válida: la cantidad de columnas no coincide con la cantidad de valores.");
            }
            if(columnasPK_23DB.Length > 2)
            {
                throw new Exception($"La tabla {nombreTabla_23DB} tiene {columnasPK_23DB.Length} columnas en la clave primaria y solo se admiten hasta 2.");
            }

            int fa_23DB = 0;
            SqlParameter[] sp_23DB = new SqlParameter[columnasPK_23DB.Length == 2 ? 6 : 4];
            sp_23DB[0] = new SqlParameter("@NombreTabla", nombreTabla_23DB);
            sp_23DB[1] = new SqlParameter("@Columna1", columnasPK_23DB[0]);
            sp_23DB[2] = new SqlParameter("@Valor1", valoresPK_23DB[0]);
            sp_23DB[3] = new SqlParameter("@DVH", SqlDbType.BigInt) { Value = dvh_23DB };
            if(columnasPK_23DB.Length == 2)
            {
                sp_23DB[4] = new SqlParameter("@Columna2", columnasPK_23DB[1]);
                sp_23DB[5] = new SqlParameter("@Valor2", valoresPK_23DB[1]);
            }
            fa_23DB = acc_23DB.Escribir_23DB("ActualizarDVHRegistro_62FD", sp_23DB);
            return fa_23DB;
        }

        // Obtiene la fila de DV_23DB de una tabla, o null si no está registrada.
        public DV_23DB ObtenerDV_23DB(string nombreTabla_23DB)
        {
            DV_23DB dv_23DB = null;
            SqlParameter[] sp_23DB = new SqlParameter[1];
            sp_23DB[0] = new SqlParameter("@NombreTabla", nombreTabla_23DB);
            DataTable dt_23DB = acc_23DB.Leer_23DB("ObtenerDVConPK_62FD", sp_23DB);
            if(dt_23DB.Rows.Count > 0)
            {
                DataRow dr_23DB = dt_23DB.Rows[0];
                dv_23DB = new DV_23DB
                {
                    IdTabla_23DB = Convert.ToInt32(dr_23DB["IdTabla"]),
                    NombreTabla_23DB = dr_23DB["NombreTabla"].ToString(),
                    NombrePK_23DB = dr_23DB["NombrePK"].ToString(),
                    DVV_23DB = Convert.ToInt64(dr_23DB["DVV"])
                };
            }
            return dv_23DB;
        }

        // Obtiene todas las tablas verificadas con su clave primaria y su DVV.
        public List<DV_23DB> ObtenerTodosDV_23DB()
        {
            List<DV_23DB> lista_23DB = new List<DV_23DB>();
            DataTable dt_23DB = acc_23DB.Leer_23DB("ObtenerTodosDVConPK_62FD", null);
            foreach(DataRow dr_23DB in dt_23DB.Rows)
            {
                lista_23DB.Add(new DV_23DB
                {
                    IdTabla_23DB = Convert.ToInt32(dr_23DB["IdTabla"]),
                    NombreTabla_23DB = dr_23DB["NombreTabla"].ToString(),
                    NombrePK_23DB = dr_23DB["NombrePK"].ToString(),
                    DVV_23DB = Convert.ToInt64(dr_23DB["DVV"])
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
