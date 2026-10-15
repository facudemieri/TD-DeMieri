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
    public class mapperReserva_62FD
    {
        Acceso_23DB acc_62FD = new Acceso_23DB();

        public int InsertarReserva_62FD(Reserva_62FD reserva_62FD)
        {
            SqlParameter[] sp_62FD = new SqlParameter[5];
            sp_62FD[0] = new SqlParameter("@CodReserva", reserva_62FD.CodReserva_62FD);
            sp_62FD[1] = new SqlParameter("@NroVuelo", reserva_62FD.NroVuelo_62FD);
            sp_62FD[2] = new SqlParameter("@FechaReserva", reserva_62FD.FechaReserva_62FD);
            sp_62FD[3] = new SqlParameter("@MontoTotal", reserva_62FD.MontoTotal_62FD);
            sp_62FD[4] = new SqlParameter("@EstadoReserva", reserva_62FD.EstadoReserva_62FD);

            return acc_62FD.Escribir_23DB("InsertarReserva_62FD", sp_62FD);
        }

        public int InsertarDetalleReserva_62FD(ReservaDetalle_62FD detalle_62FD)
        {
            SqlParameter[] sp_62FD = new SqlParameter[4];
            sp_62FD[0] = new SqlParameter("@CodReserva", detalle_62FD.CodReserva_62FD);
            sp_62FD[1] = new SqlParameter("@NumeroDocumento", detalle_62FD.NumeroDocumento_62FD);
            sp_62FD[2] = new SqlParameter("@IdAsiento", detalle_62FD.IdAsiento_62FD);
            sp_62FD[3] = new SqlParameter("@Precio", detalle_62FD.Precio_62FD);

            return acc_62FD.Escribir_23DB("InsertarDetalleReserva_62FD", sp_62FD);
        }

        public Reserva_62FD ObtenerReservaPorCodigo_62FD(string codReserva_62FD)
        {
            Reserva_62FD reserva_62FD = null;

            SqlParameter[] sp_62FD = new SqlParameter[1];
            sp_62FD[0] = new SqlParameter("@CodReserva", codReserva_62FD);

            DataTable dt_62FD = acc_62FD.Leer_23DB("ObtenerReservaPorCodigo_62FD", sp_62FD);

            if (dt_62FD.Rows.Count > 0)
            {
                DataRow dr_62FD = dt_62FD.Rows[0];

                reserva_62FD = new Reserva_62FD();
                reserva_62FD.CodReserva_62FD = dr_62FD["CodReserva_62FD"].ToString();
                reserva_62FD.NroVuelo_62FD = dr_62FD["NroVuelo_62FD"].ToString();
                reserva_62FD.FechaReserva_62FD = DateTime.Parse(dr_62FD["FechaReserva_62FD"].ToString());
                reserva_62FD.MontoTotal_62FD = decimal.Parse(dr_62FD["MontoTotal_62FD"].ToString());
                reserva_62FD.EstadoReserva_62FD = dr_62FD["EstadoReserva_62FD"].ToString();
                reserva_62FD.Ruta_62FD = dr_62FD["Origen_62FD"].ToString() + " -> " + dr_62FD["Destino_62FD"].ToString();
                reserva_62FD.FechaHoraSalida_62FD = DateTime.Parse(dr_62FD["FechaHoraSalida_62FD"].ToString());
            }

            return reserva_62FD;
        }

        public List<ReservaDetalle_62FD> ObtenerDetallesReserva_62FD(string codReserva_62FD)
        {
            List<ReservaDetalle_62FD> lista_62FD = new List<ReservaDetalle_62FD>();

            SqlParameter[] sp_62FD = new SqlParameter[1];
            sp_62FD[0] = new SqlParameter("@CodReserva", codReserva_62FD);

            DataTable dt_62FD = acc_62FD.Leer_23DB("ObtenerDetallesReserva_62FD", sp_62FD);

            foreach (DataRow dr_62FD in dt_62FD.Rows)
            {
                ReservaDetalle_62FD detalle_62FD = new ReservaDetalle_62FD();

                detalle_62FD.IdDetalle_62FD = int.Parse(dr_62FD["IdDetalle_62FD"].ToString());
                detalle_62FD.CodReserva_62FD = dr_62FD["CodReserva_62FD"].ToString();
                detalle_62FD.NumeroDocumento_62FD = dr_62FD["numeroDocumento_62FD"].ToString();
                detalle_62FD.IdAsiento_62FD = int.Parse(dr_62FD["IdAsiento_62FD"].ToString());
                detalle_62FD.Precio_62FD = decimal.Parse(dr_62FD["Precio_62FD"].ToString());
                detalle_62FD.NombreCompleto_62FD = dr_62FD["Apellido_62FD"].ToString() + ", " + dr_62FD["Nombre_62FD"].ToString();
                detalle_62FD.NroAsiento_62FD = dr_62FD["NroAsiento_62FD"].ToString();
                detalle_62FD.Clase_62FD = dr_62FD["Clase_62FD"].ToString();

                lista_62FD.Add(detalle_62FD);
            }

            return lista_62FD;
        }

        public int ActualizarEstadoReserva_62FD(string codReserva_62FD, string estadoReserva_62FD)
        {
            SqlParameter[] sp_62FD = new SqlParameter[2];
            sp_62FD[0] = new SqlParameter("@CodReserva", codReserva_62FD);
            sp_62FD[1] = new SqlParameter("@EstadoReserva", estadoReserva_62FD);

            return acc_62FD.Escribir_23DB("ActualizarEstadoReserva_62FD", sp_62FD);
        }

        public List<Reserva_62FD> ObtenerReservasPendientes_62FD()
        {
            List<Reserva_62FD> lista_62FD = new List<Reserva_62FD>();

            DataTable dt_62FD = acc_62FD.Leer_23DB("ObtenerReservasPendientes_62FD", null);

            foreach (DataRow dr_62FD in dt_62FD.Rows)
            {
                Reserva_62FD reserva_62FD = new Reserva_62FD();

                reserva_62FD.CodReserva_62FD = dr_62FD["CodReserva_62FD"].ToString();
                reserva_62FD.NroVuelo_62FD = dr_62FD["NroVuelo_62FD"].ToString();
                reserva_62FD.FechaReserva_62FD = DateTime.Parse(dr_62FD["FechaReserva_62FD"].ToString());
                reserva_62FD.MontoTotal_62FD = decimal.Parse(dr_62FD["MontoTotal_62FD"].ToString());
                reserva_62FD.EstadoReserva_62FD = dr_62FD["EstadoReserva_62FD"].ToString();
                reserva_62FD.Ruta_62FD = dr_62FD["Origen_62FD"].ToString() + " -> " + dr_62FD["Destino_62FD"].ToString();
                reserva_62FD.FechaHoraSalida_62FD = DateTime.Parse(dr_62FD["FechaHoraSalida_62FD"].ToString());
                reserva_62FD.CantidadPasajeros_62FD = int.Parse(dr_62FD["CantidadPasajeros_62FD"].ToString());

                lista_62FD.Add(reserva_62FD);
            }

            return lista_62FD;
        }
    }
}
