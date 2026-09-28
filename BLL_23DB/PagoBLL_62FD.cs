using BE_23DB;
using Mapper_23DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_23DB
{
    public class PagoBLL_62FD
    {
        mapperPago_62FD mapperPago_62FD = new mapperPago_62FD();

        public bool RegistrarPago_62FD(Pago_62FD pago_62FD)
        {
            pago_62FD.CodPago_62FD = GenerarCodigoPago_62FD();
            pago_62FD.FechaOperacion_62FD = DateTime.Now;

            if (pago_62FD.MedioPago_62FD == "Efectivo")
            {
                AprobarPagoEfectivo_62FD(pago_62FD);
            }
            else
            {
                string codigoAutorizacion_62FD = ConformarPago_62FD(pago_62FD.NumeroTarjeta_62FD, pago_62FD.MontoTotal_62FD);

                if (string.IsNullOrEmpty(codigoAutorizacion_62FD))
                {
                    return false;
                }

                pago_62FD.CodigoAutorizacion_62FD = codigoAutorizacion_62FD;
                pago_62FD.UltimosDigitos_62FD = pago_62FD.NumeroTarjeta_62FD.Substring(
                pago_62FD.NumeroTarjeta_62FD.Length - 4);
                pago_62FD.EstadoPago_62FD = "Aprobado";
            }

            int fa_62FD = mapperPago_62FD.InsertarPago_62FD(pago_62FD);

            if (fa_62FD < 0)
            {
                throw new Exception("No se pudo registrar el pago.");
            }

            return true;
        }

        public void AprobarPagoEfectivo_62FD(Pago_62FD pago_62FD)
        {

            pago_62FD.EntidadFinanciera_62FD = "";
            pago_62FD.UltimosDigitos_62FD = "";
            pago_62FD.CodigoAutorizacion_62FD = "EFECTIVO";
            pago_62FD.EstadoPago_62FD = "Aprobado";
        }

        public string ConformarPago_62FD(string numeroTarjeta_62FD, decimal montoTotal_62FD)
        {
            int ultimoDigito_62FD = int.Parse(numeroTarjeta_62FD.Substring(numeroTarjeta_62FD.Length - 1));

            if (ultimoDigito_62FD % 2 != 0)
            {
                return "";
            }

            return "AUT" + DateTime.Now.ToString("yyMMddHHmmss");
        }

        public string GenerarCodigoPago_62FD()
        {
            return "P" + DateTime.Now.ToString("yyMMddHHmmss");
        }
    }
}
