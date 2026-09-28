using BE_23DB;
using Mapper_23DB;
using Services_23DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_23DB
{
    public class PasajeBLL_62FD
    {
        mapperPasaje_62FD mapperPasaje_62FD = new mapperPasaje_62FD();
        PagoBLL_62FD pagoBLL_62FD = new PagoBLL_62FD();
        FacturaBLL_62FD facturaBLL_62FD = new FacturaBLL_62FD();
        ReservaBLL_62FD reservaBLL_62FD = new ReservaBLL_62FD();
        AsientoBLL_62FD asientoBLL_62FD = new AsientoBLL_62FD();
        DVBLL_23DB dvBLL_23DB = new DVBLL_23DB();

        public Factura_62FD GenerarPasaje_62FD(Pago_62FD pago_62FD, Reserva_62FD reserva_62FD)
        {
            if (!ValidaDatosPago_62FD(pago_62FD))
            {
                throw new Exception("Los datos de pago son inválidos.");
            }

            bool aprobado_62FD = pagoBLL_62FD.RegistrarPago_62FD(pago_62FD);

            if (!aprobado_62FD)
            {
                throw new Exception("Pago rechazado por la entidad financiera.");
            }

            reservaBLL_62FD.ActualizarEstado_62FD(reserva_62FD.CodReserva_62FD, "Confirmada");

            foreach (ReservaDetalle_62FD detalle_62FD in reserva_62FD.Detalles_62FD)
            {
                Pasaje_62FD pasaje_62FD = new Pasaje_62FD();
                pasaje_62FD.CodPasaje_62FD = GenerarCodigoPasaje_62FD(detalle_62FD.IdDetalle_62FD);
                pasaje_62FD.IdDetalle_62FD = detalle_62FD.IdDetalle_62FD;
                pasaje_62FD.FechaEmision_62FD = DateTime.Now;
                pasaje_62FD.EstadoPasaje_62FD = "Emitido";

                int fa_62FD = mapperPasaje_62FD.InsertarPasaje_62FD(pasaje_62FD);

                if (fa_62FD < 0)
                {
                    throw new Exception("No se pudo emitir el pasaje.");
                }

                asientoBLL_62FD.ActualizarEstado_62FD(detalle_62FD.IdAsiento_62FD, "Ocupado");
            }

            Factura_62FD factura_62FD = facturaBLL_62FD.GenerarFactura_62FD(pago_62FD);

            dvBLL_23DB.RecalcularDVTabla_23DB("Pago_62FD");
            dvBLL_23DB.RecalcularDVTabla_23DB("Reserva_62FD");
            dvBLL_23DB.RecalcularDVTabla_23DB("Pasaje_62FD");
            dvBLL_23DB.RecalcularDVTabla_23DB("Asiento_62FD");
            dvBLL_23DB.RecalcularDVTabla_23DB("Factura_62FD");

            return factura_62FD;
        }

        public bool ValidaDatosPago_62FD(Pago_62FD pago_62FD)
        {
            if (pago_62FD == null || string.IsNullOrEmpty(pago_62FD.MedioPago_62FD))
            {
                return false;
            }

            if (pago_62FD.MedioPago_62FD == "Efectivo")
            {
                return true;
            }

            if (string.IsNullOrEmpty(pago_62FD.EntidadFinanciera_62FD) || string.IsNullOrEmpty(pago_62FD.NumeroTarjeta_62FD) || string.IsNullOrEmpty(pago_62FD.CodigoSeguridad_62FD))
            {
                return false;
            }

            if (pago_62FD.NumeroTarjeta_62FD.Length != 16)
            {
                return false;
            }

            foreach (char c_62FD in pago_62FD.NumeroTarjeta_62FD)
            {
                if (!char.IsDigit(c_62FD))
                {
                    return false;
                }
            }

            if (pago_62FD.FechaVencimiento_62FD.Date < DateTime.Now.Date)
            {
                return false;
            }

            return true;
        }

        public string GenerarCodigoPasaje_62FD(int idDetalle_62FD)
        {
            return "T" + DateTime.Now.ToString("yyMMddHHmmss") + idDetalle_62FD.ToString();
        }
    }
}
