using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services_23DB
{
    public class PatenteBLL_23DB
    {
        private mapperPatente_23DB mapperPatente_23DB = new mapperPatente_23DB();

        public List<Patente_23DB> ObtenerPatentes_23DB()
        {
            return mapperPatente_23DB.ObtenerPatentes_23DB();
        }
    }
}
