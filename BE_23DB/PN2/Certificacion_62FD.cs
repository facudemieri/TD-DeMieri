using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE_23DB
{
    public class Certificacion_62FD
    {
        private string _codHabilitante_62FD;

        public string codHabilitante_62FD
        {
            get { return _codHabilitante_62FD; }
            set { _codHabilitante_62FD = value; }
        }

        private DateTime _fechaHoraCertificacion_62FD;

        public DateTime fechaHoraCertificacion_62FD
        {
            get { return _fechaHoraCertificacion_62FD; }
            set { _fechaHoraCertificacion_62FD = value; }
        }

        private string _resultadoInspeccion_62FD;

        public string resultadoInspeccion_62FD
        {
            get { return _resultadoInspeccion_62FD; }
            set { _resultadoInspeccion_62FD = value; }
        }

        private string observaciones_62FD;

        public string Observaciones_62FD
        {
            get { return observaciones_62FD; }
            set { observaciones_62FD = value; }
        }

        // asociaciones del diagrama conceptual
        private int idIntervencion_62FD;

        public int IdIntervencion_62FD
        {
            get { return idIntervencion_62FD; }
            set { idIntervencion_62FD = value; }
        }
    }
}
