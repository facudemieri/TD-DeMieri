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
    public class mapperPago_62FD
    {
        Acceso_23DB acc_62FD = new Acceso_23DB();

        public int InsertarPago_62FD(Pago_62FD pago_62FD)
        {
            SqlParameter[] sp_62FD = new SqlParameter[9];
            sp_62FD[0] = new SqlParameter("@CodPago", pago_62FD.CodPago_62FD);
            sp_62FD[1] = new SqlParameter("@CodReserva", pago_62FD.CodReserva_62FD);
            sp_62FD[2] = new SqlParameter("@MedioPago", pago_62FD.MedioPago_62FD);
            sp_62FD[3] = new SqlParameter("@EntidadFinanciera", pago_62FD.EntidadFinanciera_62FD ?? "");
            sp_62FD[4] = new SqlParameter("@UltimosDigitos", pago_62FD.UltimosDigitos_62FD ?? "");
            sp_62FD[5] = new SqlParameter("@CodigoAutorizacion", pago_62FD.CodigoAutorizacion_62FD ?? "");
            sp_62FD[6] = new SqlParameter("@FechaOperacion", pago_62FD.FechaOperacion_62FD);
            sp_62FD[7] = new SqlParameter("@MontoTotal", pago_62FD.MontoTotal_62FD);
            sp_62FD[8] = new SqlParameter("@EstadoPago", pago_62FD.EstadoPago_62FD);

            return acc_62FD.Escribir_23DB("InsertarPago_62FD", sp_62FD);
        }
    }
}
