using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE_23DB
{
    public class Reserva_62FD
    {
        private string codReserva_62FD;

        public string CodReserva_62FD
        {
            get { return codReserva_62FD; }
            set { codReserva_62FD = value; }
        }

        private string nroVuelo_62FD;

        public string NroVuelo_62FD
        {
            get { return nroVuelo_62FD; }
            set { nroVuelo_62FD = value; }
        }

        private DateTime fechaReserva_62FD;

        public DateTime FechaReserva_62FD
        {
            get { return fechaReserva_62FD; }
            set { fechaReserva_62FD = value; }
        }

        private decimal montoTotal_62FD;

        public decimal MontoTotal_62FD
        {
            get { return montoTotal_62FD; }
            set { montoTotal_62FD = value; }
        }

        private string estadoReserva_62FD;

        public string EstadoReserva_62FD
        {
            get { return estadoReserva_62FD; }
            set { estadoReserva_62FD = value; }
        }

        private List<ReservaDetalle_62FD> detalles_62FD = new List<ReservaDetalle_62FD>();

        public List<ReservaDetalle_62FD> Detalles_62FD
        {
            get { return detalles_62FD; }
            set { detalles_62FD = value; }
        }

        // solo de visualizacion
        private string ruta_62FD;

        public string Ruta_62FD
        {
            get { return ruta_62FD; }
            set { ruta_62FD = value; }
        }

        private DateTime fechaHoraSalida_62FD;

        public DateTime FechaHoraSalida_62FD
        {
            get { return fechaHoraSalida_62FD; }
            set { fechaHoraSalida_62FD = value; }
        }

        private int cantidadPasajeros_62FD;

        public int CantidadPasajeros_62FD
        {
            get { return cantidadPasajeros_62FD; }
            set { cantidadPasajeros_62FD = value; }
        }
    }
}
