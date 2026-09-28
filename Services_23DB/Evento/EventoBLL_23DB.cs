using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services_23DB
{
    public class EventoBLL_23DB
    {
        private mapperEvento_23DB mapperEvento_23DB = new mapperEvento_23DB();
        private DVBLL_23DB dvBLL_23DB = new DVBLL_23DB();


        public void RegistrarEvento_23DB(string dni_23DB, string modulo_23DB, string evento_23DB, int criticidad_23DB)
        {
            int fa_23DB = mapperEvento_23DB.InsertarEvento_23DB(dni_23DB, modulo_23DB, evento_23DB, criticidad_23DB);
            if(fa_23DB < 0) throw new Exception("No se pudo registrar el evento en la bitacora.");
            dvBLL_23DB.RecalcularDVTabla_23DB("Eventos_23DB");
        }

        public List<Evento_23DB> ObtenerEventos_23DB()
        {
            DateTime fechaInicio_23DB = DateTime.Now.AddDays(-3).Date;
            DateTime fechaFin_23DB = DateTime.Now.Date;
            return mapperEvento_23DB.ObtenerEventos_23DB(fechaInicio_23DB, fechaFin_23DB);
        }

        public List<Evento_23DB> FiltrarEventos_23DB(string dni_23DB, DateTime fechaInicio_23DB, DateTime fechaFin_23DB, string modulo_23DB, string evento_23DB, int criticidad_23DB)
        {
            return mapperEvento_23DB.FiltrarEventos_23DB(dni_23DB, fechaInicio_23DB, fechaFin_23DB, modulo_23DB, evento_23DB, criticidad_23DB);
        }

        public List<Usuario_23DB> ObtenerLogins_23DB()
        {
            return mapperEvento_23DB.ObtenerLogins_23DB();
        }

        public Usuario_23DB ObtenerUsuarioPorDNI_23DB(string dni_23DB)
        {
            return mapperEvento_23DB.ObtenerUsuarioPorDNI_23DB(dni_23DB);
        }

    }
}
