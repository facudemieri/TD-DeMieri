using BE_23DB;
using DAL_23DB;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mapper_23DB
{
    public class mapperPasaje_62FD
    {
        Acceso_23DB acc_62FD = new Acceso_23DB();

        public int InsertarPasaje_62FD(Pasaje_62FD pasaje_62FD)
        {
            SqlParameter[] sp_62FD = new SqlParameter[4];
            sp_62FD[0] = new SqlParameter("@CodPasaje", pasaje_62FD.CodPasaje_62FD);
            sp_62FD[1] = new SqlParameter("@IdDetalle", pasaje_62FD.IdDetalle_62FD);
            sp_62FD[2] = new SqlParameter("@FechaEmision", pasaje_62FD.FechaEmision_62FD);
            sp_62FD[3] = new SqlParameter("@EstadoPasaje", pasaje_62FD.EstadoPasaje_62FD);

            return acc_62FD.Escribir_23DB("InsertarPasaje_62FD", sp_62FD);
        }
    }
}
