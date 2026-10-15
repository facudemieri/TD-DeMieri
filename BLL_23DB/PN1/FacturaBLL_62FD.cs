using BE_23DB;
using Mapper_23DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_23DB
{
    public class FacturaBLL_62FD
    {
        mapperFactura_62FD mapperFactura_62FD = new mapperFactura_62FD();

        public Factura_62FD GenerarFactura_62FD(Pago_62FD pago_62FD)
        {
            Factura_62FD factura_62FD = new Factura_62FD();

            factura_62FD.NroFactura_62FD = GenerarNumeroFactura_62FD();
            factura_62FD.CodPago_62FD = pago_62FD.CodPago_62FD;
            factura_62FD.FechaEmision_62FD = DateTime.Now;
            factura_62FD.Total_62FD = pago_62FD.MontoTotal_62FD;

            int fa_62FD = mapperFactura_62FD.InsertarFactura_62FD(factura_62FD);

            if (fa_62FD < 0)
            {
                throw new Exception("No se pudo generar la factura.");
            }

            return factura_62FD;
        }

        public string GenerarNumeroFactura_62FD()
        {
            return "0001-" + DateTime.Now.ToString("yyMMddHHmmss");
        }
    }
}
