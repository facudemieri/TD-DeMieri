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
    public class mapperIntervencion_62FD
    {
        Acceso_23DB acc_62FD = new Acceso_23DB();

        public int ObtenerProximoId_62FD()
        {
            DataTable dt_62FD = acc_62FD.Leer_23DB("ObtenerProximoIdIntervencion_62FD", null);

            if (dt_62FD.Rows.Count == 0)
            {
                return 1;
            }

            return Convert.ToInt32(dt_62FD.Rows[0]["ProximoId"]);
        }

        public int InsertarIntervencion_62FD(Intervencion_62FD intervencion_62FD)
        {
            SqlParameter[] sp_62FD = new SqlParameter[6];
            sp_62FD[0] = new SqlParameter("@IdIntervencion", intervencion_62FD.IdIntervencion_62FD);
            sp_62FD[1] = new SqlParameter("@IdOrden", intervencion_62FD.IdOrden_62FD);
            sp_62FD[2] = new SqlParameter("@EstadoIntervencion", intervencion_62FD.estadoIntervencion_62FD);
            sp_62FD[3] = new SqlParameter("@FechaHoraInicio", intervencion_62FD.fechaHoraInicio_62FD);
            sp_62FD[4] = new SqlParameter("@FechaHoraFin", (object)intervencion_62FD.fechaHoraFin_62FD ?? DBNull.Value);
            sp_62FD[5] = new SqlParameter("@TotalHorasInsumidas", intervencion_62FD.totalHorasInsumidas_62FD);

            return acc_62FD.Escribir_23DB("InsertarIntervencion_62FD", sp_62FD);
        }

        public Intervencion_62FD ObtenerEnCursoPorOrden_62FD(int idOrden_62FD)
        {
            Intervencion_62FD intervencion_62FD = null;

            SqlParameter[] sp_62FD = new SqlParameter[1];
            sp_62FD[0] = new SqlParameter("@IdOrden", idOrden_62FD);

            DataTable dt_62FD = acc_62FD.Leer_23DB("ObtenerIntervencionEnCursoPorOrden_62FD", sp_62FD);

            if (dt_62FD.Rows.Count > 0)
            {
                DataRow dr_62FD = dt_62FD.Rows[0];
                intervencion_62FD = new Intervencion_62FD
                {
                    IdIntervencion_62FD = Convert.ToInt32(dr_62FD["IdIntervencion_62FD"]),
                    IdOrden_62FD = Convert.ToInt32(dr_62FD["IdOrden_62FD"]),
                    estadoIntervencion_62FD = dr_62FD["estadoIntervencion_62FD"].ToString(),
                    fechaHoraInicio_62FD = Convert.ToDateTime(dr_62FD["fechaHoraInicio_62FD"]),
                    totalHorasInsumidas_62FD = Convert.ToDecimal(dr_62FD["totalHorasInsumidas_62FD"])
                };

                if (dr_62FD["fechaHoraFin_62FD"] != DBNull.Value)
                {
                    intervencion_62FD.fechaHoraFin_62FD = Convert.ToDateTime(dr_62FD["fechaHoraFin_62FD"]);
                }
            }

            return intervencion_62FD;
        }

        public int ActualizarCierre_62FD(Intervencion_62FD intervencion_62FD)
        {
            SqlParameter[] sp_62FD = new SqlParameter[4];
            sp_62FD[0] = new SqlParameter("@IdIntervencion", intervencion_62FD.IdIntervencion_62FD);
            sp_62FD[1] = new SqlParameter("@FechaHoraFin", (object)intervencion_62FD.fechaHoraFin_62FD ?? DBNull.Value);
            sp_62FD[2] = new SqlParameter("@TotalHorasInsumidas", intervencion_62FD.totalHorasInsumidas_62FD);
            sp_62FD[3] = new SqlParameter("@EstadoIntervencion", intervencion_62FD.estadoIntervencion_62FD);

            return acc_62FD.Escribir_23DB("ActualizarCierreIntervencion_62FD", sp_62FD);
        }
    }
}
