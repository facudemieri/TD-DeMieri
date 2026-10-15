using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE_23DB
{
    public class Vuelo_62FD
    {
        private string nroVuelo_62FD;

        public string NroVuelo_62FD
        {
            get { return nroVuelo_62FD; }
            set { nroVuelo_62FD = value; }
        }

        private string origen_62FD;

        public string Origen_62FD
        {
            get { return origen_62FD; }
            set { origen_62FD = value; }
        }

        private string destino_62FD;

        public string Destino_62FD
        {
            get { return destino_62FD; }
            set { destino_62FD = value; }
        }

        private DateTime fechaHoraSalida_62FD;

        public DateTime FechaHoraSalida_62FD
        {
            get { return fechaHoraSalida_62FD; }
            set { fechaHoraSalida_62FD = value; }
        }

        private string matricula_62FD;

        public string Matricula_62FD
        {
            get { return matricula_62FD; }
            set { matricula_62FD = value; }
        }

        private decimal precio_62FD;

        public decimal Precio_62FD
        {
            get { return precio_62FD; }
            set { precio_62FD = value; }
        }

        private string estadoVuelo_62FD;

        public string EstadoVuelo_62FD
        {
            get { return estadoVuelo_62FD; }
            set { estadoVuelo_62FD = value; }
        }

        private int asientosDisponibles_62FD;

        public int AsientosDisponibles_62FD
        {
            get { return asientosDisponibles_62FD; }
            set { asientosDisponibles_62FD = value; }
        }

        public string Ruta_62FD
        {
            get { return origen_62FD + " -> " + destino_62FD; }
        }

    }
}
