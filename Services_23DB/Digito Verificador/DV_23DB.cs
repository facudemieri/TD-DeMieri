using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services_23DB
{
    public class DV_23DB
    {
        private int idTabla_;
        public int IdTabla_23DB
        {
            get { return idTabla_; }
            set { idTabla_ = value; }
        }

        private string nombreTabla_;
        public string NombreTabla_23DB
        {
            get { return nombreTabla_; }
            set { nombreTabla_ = value; }
        }

        private string nombrePK_;
        public string NombrePK_23DB
        {
            get { return nombrePK_; }
            set { nombrePK_ = value; }
        }

        private long dvv_;
        public long DVV_23DB
        {
            get { return dvv_; }
            set { dvv_ = value; }
        }

        private List<string> registrosInconsistentes_ = new List<string>();
        public List<string> RegistrosInconsistentes_23DB
        {
            get { return registrosInconsistentes_; }
            set { registrosInconsistentes_ = value; }
        }
    }
}
