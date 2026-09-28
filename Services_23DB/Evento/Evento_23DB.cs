using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services_23DB
{
    public class Evento_23DB
    {
        private int id_Evento_;

        public int Id_Evento_23DB
        {
            get { return id_Evento_; }
            set { id_Evento_ = value; }
        }

        private string DNI_;

        public string DNI_23DB
        {
            get { return DNI_; }
            set { DNI_ = value; }
        }

        private DateTime fecha_;

        public DateTime Fecha_23DB
        {
            get { return fecha_; }
            set { fecha_ = value; }
        }

        private TimeSpan Hora_;

        public TimeSpan Hora_23DB
        {
            get { return Hora_; }
            set { Hora_ = value; }
        }

        private string Modulo_;

        public string Modulo_23DB
        {
            get { return Modulo_; }
            set { Modulo_ = value; }
        }

        private string Evento_;

        public string Evento23DB
        {
            get { return Evento_; }
            set { Evento_ = value; }
        }

        private int criticidad_;

        public int Criticidad_23DB
        {
            get { return criticidad_; }
            set { criticidad_ = value; }
        }

    }
}
