using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE_23DB
{
    public class Intervencion_62FD
    {
        private int idIntervencion_62FD;

        public int IdIntervencion_62FD
        {
            get { return idIntervencion_62FD; }
            set { idIntervencion_62FD = value; }
        }

        private string _estadoIntervencion_62FD;

        public string estadoIntervencion_62FD
        {
            get { return _estadoIntervencion_62FD; }
            set { _estadoIntervencion_62FD = value; }
        }

        private DateTime _fechaHoraInicio_62FD;

        public DateTime fechaHoraInicio_62FD
        {
            get { return _fechaHoraInicio_62FD; }
            set { _fechaHoraInicio_62FD = value; }
        }

        private DateTime? _fechaHoraFin_62FD;

        public DateTime? fechaHoraFin_62FD
        {
            get { return _fechaHoraFin_62FD; }
            set { _fechaHoraFin_62FD = value; }
        }

        private decimal _totalHorasInsumidas_62FD;

        public decimal totalHorasInsumidas_62FD
        {
            get { return _totalHorasInsumidas_62FD; }
            set { _totalHorasInsumidas_62FD = value; }
        }

        // asociaciones del diagrama conceptual
        private int idOrden_62FD;

        public int IdOrden_62FD
        {
            get { return idOrden_62FD; }
            set { idOrden_62FD = value; }
        }

        private string matricula_62FD;

        public string Matricula_62FD
        {
            get { return matricula_62FD; }
            set { matricula_62FD = value; }
        }
        private List<Tarea_62FD> tareas_62FD = new List<Tarea_62FD>();

        public List<Tarea_62FD> Tareas_62FD
        {
            get { return tareas_62FD; }
            set { tareas_62FD = value; }
        }
    }
}
