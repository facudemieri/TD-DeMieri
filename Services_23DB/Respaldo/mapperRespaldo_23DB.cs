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


    public class mapperRespaldo_23DB
    {
        private const int TimeoutBackupSegundos_23DB = 120;
        private const int TimeoutRestoreSegundos_23DB = 300;

        public void GenerarBackup_23DB(string rutaCompleta_23DB)
        {
            using(SqlConnection conn_23DB = new SqlConnection(Conexion_23DB.ObtenerCadena_23DB()))
            {
                conn_23DB.Open();
                SqlCommand cmd_23DB = new SqlCommand("GenerarBackup", conn_23DB);
                cmd_23DB.CommandType = CommandType.StoredProcedure;
                cmd_23DB.CommandTimeout = TimeoutBackupSegundos_23DB;
                cmd_23DB.Parameters.AddWithValue("@Ruta", rutaCompleta_23DB);
                cmd_23DB.ExecuteNonQuery();
            }
        }

        public void RestaurarBackup_23DB(string rutaArchivo_23DB)
        {
            // Conectarse a master para poder restaurar TD_62FD.

            SqlConnectionStringBuilder constructor_23DB = new SqlConnectionStringBuilder(Conexion_23DB.ObtenerCadena_23DB());
            constructor_23DB.InitialCatalog = "master";
            string cadenamaster_23DB = constructor_23DB.ConnectionString;

            string query_23DB = @"IF DB_ID('TD_62FD') IS NOT NULL ALTER DATABASE TD_62FD SET SINGLE_USER WITH ROLLBACK IMMEDIATE; 
                                    BEGIN TRY RESTORE DATABASE TD_62FD FROM DISK = @Ruta WITH REPLACE; 
                                    END TRY
                                    BEGIN CATCH
                                        IF DB_ID('TD_62FD') IS NOT NULL ALTER DATABASE TD_62FD SET MULTI_USER;
                                            THROW;
                                            END CATCH
                                                IF DB_ID('TD_62FD') IS NOT NULL ALTER DATABASE TD_62FD SET MULTI_USER;";

            using (SqlConnection conn_23DB = new SqlConnection(cadenamaster_23DB))
            {
                conn_23DB.Open();
                SqlCommand cmd_23DB = new SqlCommand(query_23DB, conn_23DB);
                cmd_23DB.CommandTimeout = TimeoutRestoreSegundos_23DB;
                cmd_23DB.Parameters.Add(new SqlParameter("@Ruta", SqlDbType.NVarChar, 260) { Value = rutaArchivo_23DB });
                cmd_23DB.ExecuteNonQuery();
            }
        }
    }
}
