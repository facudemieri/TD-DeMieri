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

        private long dvh_;
        public long DVH_23DB
        {
            get { return dvh_; }
            set { dvh_ = value; }
        }

        private long dvv_;
        public long DVV_23DB
        {
            get { return dvv_; }
            set { dvv_ = value; }
        }
    }
}
