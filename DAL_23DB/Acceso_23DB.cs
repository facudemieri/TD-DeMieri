using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL_23DB
{
    public class Acceso_23DB
    {
        SqlConnection conn_23DB = new SqlConnection();

        void Conectar_23DB()
        {
            conn_23DB.ConnectionString = Conexion_23DB.ObtenerCadena_23DB();
            conn_23DB.Open();
        }

        void Desconectar_23DB()
        {
            conn_23DB.Close();
        }

        public int Escribir_23DB(string st_23DB, SqlParameter[] p_23DB)
        {
            int filas_23DB;

            try
            {
                Conectar_23DB();

                SqlCommand cmd_23DB = new SqlCommand();
                cmd_23DB.Connection = conn_23DB;
                cmd_23DB.CommandType = CommandType.StoredProcedure;
                cmd_23DB.CommandText = st_23DB;

                if (p_23DB != null)
                {
                    cmd_23DB.Parameters.AddRange(p_23DB);
                }

                filas_23DB = cmd_23DB.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al ejecutar '{st_23DB}': {ex.Message}", ex);
            }
            finally
            {
                Desconectar_23DB();
            }

            return filas_23DB;
        }

        public DataTable Leer_23DB(string st_23DB, SqlParameter[] p_23DB)
        {
            DataTable tabla_23DB = new DataTable();

            try
            {
                Conectar_23DB();

                SqlCommand cmd_23DB = new SqlCommand();
                cmd_23DB.Connection = conn_23DB;
                cmd_23DB.CommandType = CommandType.StoredProcedure;
                cmd_23DB.CommandText = st_23DB;

                if (p_23DB != null)
                {
                    cmd_23DB.Parameters.AddRange(p_23DB);
                }

                SqlDataAdapter adapter_23DB = new SqlDataAdapter(cmd_23DB);
                adapter_23DB.Fill(tabla_23DB);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al ejecutar '{st_23DB}': {ex.Message}", ex);
            }
            finally
            {
                Desconectar_23DB();
            }

            return tabla_23DB;
        }
    }
}
