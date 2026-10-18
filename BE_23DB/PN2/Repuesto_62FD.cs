using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE_23DB
{
    public class Repuesto_62FD
    {
        private int _nroSolicitud_62FD;

        public int nroSolicitud_62FD
        {
            get { return _nroSolicitud_62FD; }
            set { _nroSolicitud_62FD = value; }
        }

        private string _nroParte_62FD;

        public string nroParte_62FD
        {
            get { return _nroParte_62FD; }
            set { _nroParte_62FD = value; }
        }

        private string _nombreRepuesto_62FD;

        public string nombreRepuesto_62FD
        {
            get { return _nombreRepuesto_62FD; }
            set { _nombreRepuesto_62FD = value; }
        }

        private int _cantidadSolicitada_62FD;

        public int cantidadSolicitada_62FD
        {
            get { return _cantidadSolicitada_62FD; }
            set { _cantidadSolicitada_62FD = value; }
        }

        private string _estadoPedido_62FD;

        public string estadoPedido_62FD
        {
            get { return _estadoPedido_62FD; }
            set { _estadoPedido_62FD = value; }
        }

        private DateTime _fechaSolicitud_62FD;

        public DateTime fechaSolicitud_62FD
        {
            get { return _fechaSolicitud_62FD; }
            set { _fechaSolicitud_62FD = value; }
        }

        private DateTime? _fechaRecepcion_62FD;

        public DateTime? fechaRecepcion_62FD
        {
            get { return _fechaRecepcion_62FD; }
            set { _fechaRecepcion_62FD = value; }
        }


        // asociaciones del diagrama conceptual
        private int idTarea_62FD;

        public int IdTarea_62FD
        {
            get { return idTarea_62FD; }
            set { idTarea_62FD = value; }
        }
    }
}
