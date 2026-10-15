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
    public class mapperAsiento_62FD
    {
        Acceso_23DB acc_62FD = new Acceso_23DB();

        public List<Asiento_62FD> ObtenerAsientosPorVuelo_62FD(string nroVuelo_62FD)
        {
            List<Asiento_62FD> lista_62FD = new List<Asiento_62FD>();

            SqlParameter[] sp_62FD = new SqlParameter[1];
            sp_62FD[0] = new SqlParameter("@NroVuelo", nroVuelo_62FD);

            DataTable dt_62FD = acc_62FD.Leer_23DB("ObtenerAsientosPorVuelo_62FD", sp_62FD);

            foreach (DataRow dr_62FD in dt_62FD.Rows)
            {
                lista_62FD.Add(MapearAsiento_62FD(dr_62FD));
            }

            return lista_62FD;
        }

        public int ActualizarEstadoAsiento_62FD(int idAsiento_62FD, string estadoAsiento_62FD)
        {
            SqlParameter[] sp_62FD = new SqlParameter[2];
            sp_62FD[0] = new SqlParameter("@IdAsiento", idAsiento_62FD);
            sp_62FD[1] = new SqlParameter("@EstadoAsiento", estadoAsiento_62FD);

            return acc_62FD.Escribir_23DB("ActualizarEstadoAsiento_62FD", sp_62FD);
        }

        private Asiento_62FD MapearAsiento_62FD(DataRow dr_62FD)
        {
            Asiento_62FD asiento_62FD = new Asiento_62FD();

            asiento_62FD.IdAsiento_62FD = int.Parse(dr_62FD["IdAsiento_62FD"].ToString());
            asiento_62FD.NroVuelo_62FD = dr_62FD["NroVuelo_62FD"].ToString();
            asiento_62FD.NroAsiento_62FD = dr_62FD["NroAsiento_62FD"].ToString();
            asiento_62FD.Clase_62FD = dr_62FD["Clase_62FD"].ToString();
            asiento_62FD.EstadoAsiento_62FD = dr_62FD["EstadoAsiento_62FD"].ToString();

            return asiento_62FD;
        }
    }
}
