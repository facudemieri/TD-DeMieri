using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE_23DB
{
    public class Tarea_62FD
    {
        private int idTarea_62FD;

        public int IdTarea_62FD
        {
            get { return idTarea_62FD; }
            set { idTarea_62FD = value; }
        }

        private string descripcion_62FD;

        public string Descripcion_62FD
        {
            get { return descripcion_62FD; }
            set { descripcion_62FD = value; }
        }

        private decimal horasInsumidas_62FD;

        public decimal HorasInsumidas_62FD
        {
            get { return horasInsumidas_62FD; }
            set { horasInsumidas_62FD = value; }
        }

        private string _observacionesTecnicas_62FD;

        public string observacionesTecnicas_62FD
        {
            get { return _observacionesTecnicas_62FD; }
            set { _observacionesTecnicas_62FD = value; }
        }

        private string resultadoObtenido_62FD;

        public string ResultadoObtenido_62FD
        {
            get { return resultadoObtenido_62FD; }
            set { resultadoObtenido_62FD = value; }
        }

        // asociaciones del diagrama conceptual
        private int idIntervencion_62FD;

        public int IdIntervencion_62FD
        {
            get { return idIntervencion_62FD; }
            set { idIntervencion_62FD = value; }
        }

        private List<Repuesto_62FD> repuestos_62FD = new List<Repuesto_62FD>();

        public List<Repuesto_62FD> Repuestos_62FD
        {
            get { return repuestos_62FD; }
            set { repuestos_62FD = value; }
        }

        private string estadoRepuestos_62FD;
        public string EstadoRepuestos_62FD
        {
            get { return estadoRepuestos_62FD; }
            set { estadoRepuestos_62FD = value; }
        }
    }
}
