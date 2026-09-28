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
    public class mapperVuelo_62FD
    {
        Acceso_23DB acc_62FD = new Acceso_23DB();

        public List<Vuelo_62FD> ObtenerVuelos_62FD(string origen_62FD, string destino_62FD, DateTime fechaPartida_62FD)
        {
            List<Vuelo_62FD> lista_62FD = new List<Vuelo_62FD>();

            SqlParameter[] sp_62FD = new SqlParameter[3];
            sp_62FD[0] = new SqlParameter("@Origen", origen_62FD);
            sp_62FD[1] = new SqlParameter("@Destino", destino_62FD);
            sp_62FD[2] = new SqlParameter("@FechaPartida", fechaPartida_62FD.Date);

            DataTable dt_62FD = acc_62FD.Leer_23DB("ObtenerVuelos_62FD", sp_62FD);

            foreach(DataRow dr_62FD in dt_62FD.Rows)
            {
                lista_62FD.Add(MapearVuelo_62FD(dr_62FD));
            }

            return lista_62FD;
        }

        public List<Vuelo_62FD> ObtenerTodosLosVuelos_62FD()
        {
            List<Vuelo_62FD> lista_62FD = new List<Vuelo_62FD>();

            DataTable dt_62FD = acc_62FD.Leer_23DB("ObtenerTodosLosVuelos_62FD", null);

            foreach (DataRow dr_62FD in dt_62FD.Rows)
            {
                lista_62FD.Add(MapearVuelo_62FD(dr_62FD));
            }

            return lista_62FD;
        }

        public int ContarAsientosDisponibles_62FD(string nroVuelo_62FD)
        {
            int cantidad_62FD = 0;

            SqlParameter[] sp_62FD = new SqlParameter[1];
            sp_62FD[0] = new SqlParameter("@NroVuelo", nroVuelo_62FD);

            DataTable dt_62FD = acc_62FD.Leer_23DB("ContarAsientosDisponibles_62FD", sp_62FD);

            if(dt_62FD.Rows.Count > 0)
            {
                cantidad_62FD = int.Parse(dt_62FD.Rows[0]["Cantidad_62FD"].ToString());
            }

            return cantidad_62FD;
        }

        private Vuelo_62FD MapearVuelo_62FD(DataRow dr_62FD)
        {
            Vuelo_62FD vuelo_62FD = new Vuelo_62FD();

            vuelo_62FD.NroVuelo_62FD = dr_62FD["NroVuelo_62FD"].ToString();
            vuelo_62FD.Origen_62FD = dr_62FD["Origen_62FD"].ToString();
            vuelo_62FD.Destino_62FD = dr_62FD["Destino_62FD"].ToString();
            vuelo_62FD.FechaHoraSalida_62FD = DateTime.Parse(dr_62FD["FechaHoraSalida_62FD"].ToString());
            vuelo_62FD.Matricula_62FD = dr_62FD["Matricula_62FD"].ToString();
            vuelo_62FD.Precio_62FD = decimal.Parse(dr_62FD["Precio_62FD"].ToString());
            vuelo_62FD.EstadoVuelo_62FD = dr_62FD["EstadoVuelo_62FD"].ToString();

            return vuelo_62FD;
        }
    }
}
