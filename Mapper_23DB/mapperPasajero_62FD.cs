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
    public class mapperPasajero_62FD
    {
        Acceso_23DB acc_62FD = new Acceso_23DB();

        public Pasajero_62FD ObtenerPasajeroPorDocumento_62FD(string numeroDocumento_62FD)
        {
            Pasajero_62FD pasajero_62FD = null;

            SqlParameter[] sp_62FD = new SqlParameter[1];
            sp_62FD[0] = new SqlParameter("@NumeroDocumento", numeroDocumento_62FD);

            DataTable dt_62FD = acc_62FD.Leer_23DB("ObtenerPasajeroPorDocumento_62FD", sp_62FD);

            if (dt_62FD.Rows.Count > 0)
            {
                pasajero_62FD = MapearPasajero_62FD(dt_62FD.Rows[0]);
            }

            return pasajero_62FD;
        }

        private Pasajero_62FD MapearPasajero_62FD(DataRow dr_62FD)
        {
            Pasajero_62FD pasajero_62FD = new Pasajero_62FD();

            pasajero_62FD.NumeroDocumento_62FD = dr_62FD["numeroDocumento_62FD"].ToString();
            pasajero_62FD.Apellido_62FD = dr_62FD["Apellido_62FD"].ToString();
            pasajero_62FD.Nombre_62FD = dr_62FD["Nombre_62FD"].ToString();
            pasajero_62FD.FechaNacimiento_62FD = DateTime.Parse(dr_62FD["FechaNacimiento_62FD"].ToString());
            pasajero_62FD.CorreoElectronico_62FD = dr_62FD["CorreoElectronico_62FD"].ToString();
            pasajero_62FD.Telefono_62FD = dr_62FD["Telefono_62FD"].ToString();
            pasajero_62FD.ContactoEmergencia_62FD = dr_62FD["ContactoEmergencia_62FD"].ToString();

            return pasajero_62FD;
        }

        public int InsertarPasajero_62FD(Pasajero_62FD pasajero_62FD)
        {
            SqlParameter[] sp_62FD = new SqlParameter[7];
            sp_62FD[0] = new SqlParameter("@NumeroDocumento", pasajero_62FD.NumeroDocumento_62FD);
            sp_62FD[1] = new SqlParameter("@Apellido", pasajero_62FD.Apellido_62FD);
            sp_62FD[2] = new SqlParameter("@Nombre", pasajero_62FD.Nombre_62FD);
            sp_62FD[3] = new SqlParameter("@FechaNacimiento", pasajero_62FD.FechaNacimiento_62FD.Date);
            sp_62FD[4] = new SqlParameter("@CorreoElectronico", pasajero_62FD.CorreoElectronico_62FD);
            sp_62FD[5] = new SqlParameter("@Telefono", pasajero_62FD.Telefono_62FD);
            sp_62FD[6] = new SqlParameter("@ContactoEmergencia", pasajero_62FD.ContactoEmergencia_62FD);

            return acc_62FD.Escribir_23DB("InsertarPasajero_62FD", sp_62FD);
        }
    }
}
