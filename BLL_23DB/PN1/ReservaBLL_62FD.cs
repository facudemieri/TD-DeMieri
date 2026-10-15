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
    public class ReservaBLL_62FD
    {
        mapperReserva_62FD mapperReserva_62FD = new mapperReserva_62FD();
        AsientoBLL_62FD asientoBLL_62FD = new AsientoBLL_62FD();
        DVBLL_23DB dvBLL_23DB = new DVBLL_23DB();

        public string GenerarReserva_62FD(Reserva_62FD reserva_62FD)
        {
            if (!ValidarDatos_62FD(reserva_62FD))
            {
                throw new Exception("Debe seleccionar al menos un pasajero con su asiento.");
            }

            reserva_62FD.CodReserva_62FD = GenerarCodigoReserva_62FD();
            reserva_62FD.FechaReserva_62FD = DateTime.Now;
            reserva_62FD.MontoTotal_62FD = CalcularMontoTotal_62FD(reserva_62FD.Detalles_62FD);
            reserva_62FD.EstadoReserva_62FD = "Pendiente de Pago";

            int fa_62FD = mapperReserva_62FD.InsertarReserva_62FD(reserva_62FD);

            if (fa_62FD < 0)
            {
                throw new Exception("No se pudo registrar la reserva.");
            }

            foreach (ReservaDetalle_62FD detalle_62FD in reserva_62FD.Detalles_62FD)
            {
                detalle_62FD.CodReserva_62FD = reserva_62FD.CodReserva_62FD;

                int faDetalle_62FD = mapperReserva_62FD.InsertarDetalleReserva_62FD(detalle_62FD);

                if (faDetalle_62FD < 0)
                {
                    throw new Exception("No se pudo registrar el detalle de la reserva.");
                }

                asientoBLL_62FD.ActualizarEstado_62FD(detalle_62FD.IdAsiento_62FD, "Reservado");
            }

            dvBLL_23DB.RecalcularDVTabla_23DB("Reserva_62FD");
            dvBLL_23DB.RecalcularDVTabla_23DB("ReservaDetalle_62FD");
            dvBLL_23DB.RecalcularDVTabla_23DB("Asiento_62FD");

            return reserva_62FD.CodReserva_62FD;
        }

        public bool ValidarDatos_62FD(Reserva_62FD reserva_62FD)
        {
            if (reserva_62FD == null || string.IsNullOrEmpty(reserva_62FD.NroVuelo_62FD))
            {
                return false;
            }

            if (reserva_62FD.Detalles_62FD == null || reserva_62FD.Detalles_62FD.Count == 0)
            {
                return false;
            }

            return true;
        }

        public string GenerarCodigoReserva_62FD()
        {
            return "R" + DateTime.Now.ToString("yyMMddHHmmss");
        }

        public decimal CalcularMontoTotal_62FD(List<ReservaDetalle_62FD> detalles_62FD)
        {
            decimal total_62FD = 0;

            foreach (ReservaDetalle_62FD detalle_62FD in detalles_62FD)
            {
                total_62FD = total_62FD + detalle_62FD.Precio_62FD;
            }

            return total_62FD;
        }

        public Reserva_62FD ObtenerReserva_62FD(string codReserva_62FD)
        {
            Reserva_62FD reserva_62FD = mapperReserva_62FD.ObtenerReservaPorCodigo_62FD(codReserva_62FD);

            if (reserva_62FD != null)
            {
                reserva_62FD.Detalles_62FD = mapperReserva_62FD.ObtenerDetallesReserva_62FD(codReserva_62FD);
            }

            return reserva_62FD;
        }

        public void ActualizarEstado_62FD(string codReserva_62FD, string estadoReserva_62FD)
        {
            int fa_62FD = mapperReserva_62FD.ActualizarEstadoReserva_62FD(codReserva_62FD, estadoReserva_62FD);

            if (fa_62FD < 0)
            {
                throw new Exception("No se pudo actualizar el estado de la reserva.");
            }
        }

        public List<Reserva_62FD> ObtenerReservasPendientes_62FD()
        {
            return mapperReserva_62FD.ObtenerReservasPendientes_62FD();
        }
    }
}
