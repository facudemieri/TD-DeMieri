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

        public List<Factura_62FD> ObtenerComprobantes_62FD()
        {
            List<Factura_62FD> lista_62FD = new List<Factura_62FD>();

            DataTable tabla_62FD = acc_62FD.Leer_23DB("ObtenerComprobantes_62FD", null);

            foreach (DataRow fila_62FD in tabla_62FD.Rows)
            {
                Factura_62FD factura_62FD = new Factura_62FD();

                factura_62FD.NroFactura_62FD = fila_62FD["NroFactura_62FD"].ToString();
                factura_62FD.CodReserva_62FD = fila_62FD["CodReserva_62FD"].ToString();
                factura_62FD.FechaEmision_62FD = Convert.ToDateTime(fila_62FD["FechaEmision_62FD"]);
                factura_62FD.MedioPago_62FD = fila_62FD["MedioPago_62FD"].ToString();
                factura_62FD.Total_62FD = Convert.ToDecimal(fila_62FD["Total_62FD"]);

                if (fila_62FD["Documentos_62FD"] != DBNull.Value)
                {
                    factura_62FD.Documentos_62FD = fila_62FD["Documentos_62FD"].ToString();
                }
                else
                {
                    factura_62FD.Documentos_62FD = "";
                }

                if (fila_62FD["Pasajeros_62FD"] != DBNull.Value)
                {
                    factura_62FD.Pasajeros_62FD = fila_62FD["Pasajeros_62FD"].ToString();
                }
                else
                {
                    factura_62FD.Pasajeros_62FD = "";
                }

                lista_62FD.Add(factura_62FD);
            }

            return lista_62FD;
        }
    }
}
