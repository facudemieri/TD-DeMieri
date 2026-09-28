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
    public class mapperEvento_23DB
    {
        Acceso_23DB acc_23DB = new Acceso_23DB();

        private Evento_23DB MapearEvento_23DB(DataRow dr_23DB)
        {
            return new Evento_23DB
            {
                Id_Evento_23DB = Convert.ToInt32(dr_23DB["Id_Evento"]),
                DNI_23DB = dr_23DB["DNI"].ToString(),
                Fecha_23DB = Convert.ToDateTime(dr_23DB["Fecha"]),
                Hora_23DB = TimeSpan.Parse(dr_23DB["Hora"].ToString()),
                Modulo_23DB = dr_23DB["Modulo"].ToString(),
                Evento23DB = dr_23DB["Evento"].ToString(),
                Criticidad_23DB = Convert.ToInt32(dr_23DB["Criticidad"])
            };
        }

        public int InsertarEvento_23DB(string dni_23DB, string modulo_23DB, string evento_23DB, int criticidad_23DB)
        {
            int fa_23DB = 0;
            SqlParameter[] sp_23DB = new SqlParameter[6];
            sp_23DB[0] = new SqlParameter("@DNI", dni_23DB);
            sp_23DB[1] = new SqlParameter("@Fecha", DateTime.Now.Date);
            sp_23DB[2] = new SqlParameter("@Hora", DateTime.Now.TimeOfDay);
            sp_23DB[3] = new SqlParameter("@Modulo", modulo_23DB);
            sp_23DB[4] = new SqlParameter("@Evento", evento_23DB);
            sp_23DB[5] = new SqlParameter("@Criticidad", criticidad_23DB);
            fa_23DB = acc_23DB.Escribir_23DB("InsertarEvento", sp_23DB);
            return fa_23DB;
        }

        public List<Evento_23DB> ObtenerEventos_23DB(DateTime fechaInicio_23DB, DateTime fechaFin_23DB)
        {
            List<Evento_23DB> lista_23DB = new List<Evento_23DB>();
            SqlParameter[] sp_23DB = new SqlParameter[2];
            sp_23DB[0] = new SqlParameter("@FechaInicio", fechaInicio_23DB.Date);
            sp_23DB[1] = new SqlParameter("@FechaFin", fechaFin_23DB.Date);
            DataTable dt_23DB = acc_23DB.Leer_23DB("ObtenerEventos", sp_23DB);
            foreach(DataRow dr_23DB in dt_23DB.Rows)
            {
                lista_23DB.Add(MapearEvento_23DB(dr_23DB));
            }
            return lista_23DB;
        }

        public List<Evento_23DB> FiltrarEventos_23DB(string dni_23DB, DateTime fechaInicio_23DB, DateTime fechaFin_23DB, string modulo_23DB, string evento_23DB, int criticidad_23DB)
        {
            List<Evento_23DB> lista_23DB = new List<Evento_23DB>();
            SqlParameter[] sp_23DB = new SqlParameter[6];
            sp_23DB[0] = new SqlParameter("@FechaInicio", fechaInicio_23DB.Date);
            sp_23DB[1] = new SqlParameter("@FechaFin", fechaFin_23DB.Date);
            sp_23DB[2] = new SqlParameter("@DNI", string.IsNullOrEmpty(dni_23DB) ? (object)DBNull.Value : dni_23DB);
            sp_23DB[3] = new SqlParameter("@Modulo", string.IsNullOrEmpty(modulo_23DB) ? (object)DBNull.Value : modulo_23DB);
            sp_23DB[4] = new SqlParameter("@Evento", string.IsNullOrEmpty(evento_23DB) ? (object)DBNull.Value : evento_23DB);
            sp_23DB[5] = new SqlParameter("@Criticidad", criticidad_23DB > 0 ? (object)criticidad_23DB : DBNull.Value);
            DataTable dt_23DB = acc_23DB.Leer_23DB("FiltrarEventos", sp_23DB);
            foreach(DataRow dr_23DB in dt_23DB.Rows)
            {
                lista_23DB.Add(MapearEvento_23DB(dr_23DB));
            }
            return lista_23DB;
        }

        public List<Usuario_23DB> ObtenerLogins_23DB()
        {
            List<Usuario_23DB> lista_23DB = new List<Usuario_23DB>();
            DataTable dt_23DB = acc_23DB.Leer_23DB("ObtenerLogins", null);
            foreach(DataRow dr_23DB in dt_23DB.Rows)
            {
                lista_23DB.Add(new Usuario_23DB
                {
                    DNI_23DB = dr_23DB["DNI"].ToString(),
                    Login_23DB = dr_23DB["Login"].ToString()
                });
            }
            return lista_23DB;
        }

        public Usuario_23DB ObtenerUsuarioPorDNI_23DB(string dni_23DB)
        {
            Usuario_23DB usuario_23DB = null;
            SqlParameter[] sp_23DB = new SqlParameter[1];
            sp_23DB[0] = new SqlParameter("@DNI", dni_23DB);
            DataTable dt_23DB = acc_23DB.Leer_23DB("ObtenerUsuarioDNI", sp_23DB);
            if(dt_23DB.Rows.Count > 0)
            {
                DataRow dr_23DB = dt_23DB.Rows[0];
                usuario_23DB = new Usuario_23DB
                {
                    DNI_23DB = dr_23DB["DNI"].ToString(),
                    Nombre_23DB = dr_23DB["Nombre"].ToString(),
                    Apellido_23DB = dr_23DB["Apellido"].ToString()
                };
            }
            return usuario_23DB;
        }
    }
}
