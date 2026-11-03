using BE_23DB;
using Mapper_23DB;
using Services_23DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_23DB
{
    public class OrdenTrabajoBLL_62FD
    {
        mapperOrdenTrabajo_62FD mapperOrdenTrabajo_62FD = new mapperOrdenTrabajo_62FD();
        DVBLL_23DB dvBLL_23DB = new DVBLL_23DB();
        EventoBLL_23DB eventoBLL_23DB = new EventoBLL_23DB();

        // Detalle del ultimo registro en bitacora que fallo
        public string ErrorBitacora_62FD { get; private set; }

        public List<Usuario_23DB> ObtenerTecnicos_62FD()
        {
            return mapperOrdenTrabajo_62FD.ObtenerTecnicos_62FD();
        }

        public int GenerarOrdenTrabajo_62FD(OrdenTrabajo_62FD orden_62FD, out bool bitacoraRegistrada_62FD)
        {
            bitacoraRegistrada_62FD = false;

            if (mapperOrdenTrabajo_62FD.ContarOrdenesAbiertas_62FD(orden_62FD.Matricula_62FD) > 0)
            {
                return 0;
            }

            orden_62FD.IdOrden_62FD = mapperOrdenTrabajo_62FD.ObtenerProximoId_62FD();
            orden_62FD.EstadoOrden_62FD = "Asignada";

            int fa_62FD = mapperOrdenTrabajo_62FD.InsertarOrdenTrabajo_62FD(orden_62FD);

            if (fa_62FD != 1)
            {
                return -1;
            }

            dvBLL_23DB.RecalcularDVTabla_23DB("OrdenTrabajo_62FD");

            bitacoraRegistrada_62FD = RegistrarEventoBitacora_62FD("Generar Orden de Trabajo", 3);

            return orden_62FD.IdOrden_62FD;
        }

        private bool RegistrarEventoBitacora_62FD(string evento_62FD, int criticidad_62FD)
        {
            try
            {
                eventoBLL_23DB.RegistrarEvento_23DB(SessionManager_23DB.ObtenerInstancia_23DB().DNI_23DB, "Mantenimiento", evento_62FD, criticidad_62FD);
                ErrorBitacora_62FD = null;
                return true;
            }
            catch (Exception ex_62FD)
            {
                ErrorBitacora_62FD = ex_62FD.Message;
                return false;
            }
        }

        public List<OrdenTrabajo_62FD> ObtenerOrdenesPorAeronave_62FD(string matricula_62FD)
        {
            return mapperOrdenTrabajo_62FD.ObtenerOrdenesPorAeronave_62FD(matricula_62FD);
        }
    }
}
