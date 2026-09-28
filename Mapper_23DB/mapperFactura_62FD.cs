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
    public class mapperFactura_62FD
    {
        Acceso_23DB acc_62FD = new Acceso_23DB();

        public int InsertarFactura_62FD(Factura_62FD factura_62FD)
        {
            SqlParameter[] sp_62FD = new SqlParameter[4];
            sp_62FD[0] = new SqlParameter("@NroFactura", factura_62FD.NroFactura_62FD);
            sp_62FD[1] = new SqlParameter("@CodPago", factura_62FD.CodPago_62FD);
            sp_62FD[2] = new SqlParameter("@FechaEmision", factura_62FD.FechaEmision_62FD);
            sp_62FD[3] = new SqlParameter("@Total", factura_62FD.Total_62FD);

            return acc_62FD.Escribir_23DB("InsertarFactura_62FD", sp_62FD);
        }
    }
}
