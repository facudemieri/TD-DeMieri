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

    // El restore se conecta a master, no a INGSW_23DB, porque una base no
    //   puede restaurarse a si misma mientras esta en uso. Acceso_23DB siempre
    //   toma la cadena de Conexion_23DB.ObtenerCadena_23DB(), que apunta a
    //   INGSW_23DB, asi que no sirve para este caso.
    // Ambas necesitan CommandTimeout muy por encima de los 30 segundos por
    //   defecto de SqlCommand, y Acceso_23DB no expone forma de configurarlo.
 
    // Por eso las dos mantienen SqlConnection / SqlCommand directo. El BACKUP
    // igual pasa por el stored procedure GenerarBackup, para que la ruta viaje
    // como parametro y no interpolada en el texto del comando.
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
            // Conectarse a master para poder restaurar INGSW_23DB.

            SqlConnectionStringBuilder constructor_23DB = new SqlConnectionStringBuilder(Conexion_23DB.ObtenerCadena_23DB());
            constructor_23DB.InitialCatalog = "master";
            string cadenamaster_23DB = constructor_23DB.ConnectionString;

            string query_23DB = $@"ALTER DATABASE INGSW_23DB SET SINGLE_USER WITH ROLLBACK IMMEDIATE; RESTORE DATABASE INGSW_23DB FROM DISK = '{rutaArchivo_23DB}' WITH REPLACE; ALTER DATABASE INGSW_23DB SET MULTI_USER;";

            using(SqlConnection conn_23DB = new SqlConnection(cadenamaster_23DB))
            {
                conn_23DB.Open();
                SqlCommand cmd_23DB = new SqlCommand(query_23DB, conn_23DB);
                cmd_23DB.CommandTimeout = TimeoutRestoreSegundos_23DB;
                cmd_23DB.ExecuteNonQuery();
            }

        }
    }
}
