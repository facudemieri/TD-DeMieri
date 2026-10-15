using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE_23DB
{
    public class OrdenTrabajo_62FD
    {
        private int idOrden_62FD;

        public int IdOrden_62FD
        {
            get { return idOrden_62FD; }
            set { idOrden_62FD = value; }
        }

        private string tipoIntervencion_62FD;

        public string TipoIntervencion_62FD
        {
            get { return tipoIntervencion_62FD; }
            set { tipoIntervencion_62FD = value; }
        }

        private string descripcion_62FD;

        public string Descripcion_62FD
        {
            get { return descripcion_62FD; }
            set { descripcion_62FD = value; }
        }

        private string estadoOrden_62FD;

        public string EstadoOrden_62FD
        {
            get { return estadoOrden_62FD; }
            set { estadoOrden_62FD = value; }
        }

        private DateTime fechaInicioPlanificada_62FD;

        public DateTime FechaInicioPlanificada_62FD
        {
            get { return fechaInicioPlanificada_62FD; }
            set { fechaInicioPlanificada_62FD = value; }
        }

        private DateTime fechaLimite_62FD;

        public DateTime FechaLimite_62FD
        {
            get { return fechaLimite_62FD; }
            set { fechaLimite_62FD = value; }
        }

        private string hangar_62FD;

        public string Hangar_62FD
        {
            get { return hangar_62FD; }
            set { hangar_62FD = value; }
        }

        private string legajoTecnico_62FD;

        public string LegajoTecnico_62FD
        {
            get { return legajoTecnico_62FD; }
            set { legajoTecnico_62FD = value; }
        }

        // asociaciones del diagrama conceptual
        private string matricula_62FD;

        public string Matricula_62FD
        {
            get { return matricula_62FD; }
            set { matricula_62FD = value; }
        }
    }
}
