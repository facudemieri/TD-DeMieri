using BE_23DB;
using DAL_23DB;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mapper_23DB
{
    public class mapperTarea_62FD
    {
        Acceso_23DB acc_62FD = new Acceso_23DB();

        public int ObtenerProximoId_62FD()
        {
            DataTable dt_62FD = acc_62FD.Leer_23DB("ObtenerProximoIdTarea_62FD", null);

            if (dt_62FD.Rows.Count == 0)
            {
                return 1;
            }

            return Convert.ToInt32(dt_62FD.Rows[0]["ProximoId"]);
        }

        public int InsertarTarea_62FD(Tarea_62FD tarea_62FD)
        {
            SqlParameter[] sp_62FD = new SqlParameter[6];
            sp_62FD[0] = new SqlParameter("@IdTarea", tarea_62FD.IdTarea_62FD);
            sp_62FD[1] = new SqlParameter("@IdIntervencion", tarea_62FD.IdIntervencion_62FD);
            sp_62FD[2] = new SqlParameter("@Descripcion", tarea_62FD.Descripcion_62FD);
            sp_62FD[3] = new SqlParameter("@HorasInsumidas", tarea_62FD.HorasInsumidas_62FD);
            sp_62FD[4] = new SqlParameter("@ObservacionesTecnicas", (object)tarea_62FD.observacionesTecnicas_62FD ?? DBNull.Value);
            sp_62FD[5] = new SqlParameter("@ResultadoObtenido", tarea_62FD.ResultadoObtenido_62FD);

            return acc_62FD.Escribir_23DB("InsertarTarea_62FD", sp_62FD);
        }

        public List<Tarea_62FD> ObtenerTareasPorIntervencion_62FD(int idIntervencion_62FD)
        {
            List<Tarea_62FD> lista_62FD = new List<Tarea_62FD>();

            SqlParameter[] sp_62FD = new SqlParameter[1];
            sp_62FD[0] = new SqlParameter("@IdIntervencion", idIntervencion_62FD);

            DataTable dt_62FD = acc_62FD.Leer_23DB("ObtenerTareasPorIntervencion_62FD", sp_62FD);

            foreach (DataRow dr_62FD in dt_62FD.Rows)
            {
                lista_62FD.Add(new Tarea_62FD
                {
                    IdTarea_62FD = Convert.ToInt32(dr_62FD["IdTarea_62FD"]),
                    IdIntervencion_62FD = Convert.ToInt32(dr_62FD["IdIntervencion_62FD"]),
                    Descripcion_62FD = dr_62FD["Descripcion_62FD"].ToString(),
                    HorasInsumidas_62FD = Convert.ToDecimal(dr_62FD["HorasInsumidas_62FD"]),
                    observacionesTecnicas_62FD = dr_62FD["observacionesTecnicas_62FD"].ToString(),
                    ResultadoObtenido_62FD = dr_62FD["ResultadoObtenido_62FD"].ToString(),
                    EstadoRepuestos_62FD = dr_62FD["EstadoRepuestos"].ToString()
                });
            }

            return lista_62FD;
        }

        public int EliminarTarea_62FD(int idTarea_62FD)
        {
            SqlParameter[] sp_62FD = new SqlParameter[1];
            sp_62FD[0] = new SqlParameter("@IdTarea", idTarea_62FD);

            return acc_62FD.Escribir_23DB("EliminarTarea_62FD", sp_62FD);
        }
    }
}
