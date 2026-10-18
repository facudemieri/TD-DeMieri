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

        public List<Usuario_23DB> ObtenerTecnicos_62FD()
        {
            return mapperOrdenTrabajo_62FD.ObtenerTecnicos_62FD();
        }

        public int GenerarOrdenTrabajo_62FD(OrdenTrabajo_62FD orden_62FD)
        {
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

            return orden_62FD.IdOrden_62FD;
        }

        public List<OrdenTrabajo_62FD> ObtenerOrdenesPorAeronave_62FD(string matricula_62FD)
        {
            return mapperOrdenTrabajo_62FD.ObtenerOrdenesPorAeronave_62FD(matricula_62FD);
        }
    }
}
