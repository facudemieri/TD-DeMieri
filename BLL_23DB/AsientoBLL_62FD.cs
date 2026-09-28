using BE_23DB;
using Mapper_23DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_23DB
{
    public class AsientoBLL_62FD
    {
        mapperAsiento_62FD mapperAsiento_62FD = new mapperAsiento_62FD();

        public List<Asiento_62FD> ObtenerAsientosDisponibles_62FD(string nroVuelo_62FD)
        {
            return mapperAsiento_62FD.ObtenerAsientosPorVuelo_62FD(nroVuelo_62FD);
        }

        public void ActualizarEstado_62FD(int idAsiento_62FD, string estadoAsiento_62FD)
        {
            int fa_62FD = mapperAsiento_62FD.ActualizarEstadoAsiento_62FD(idAsiento_62FD, estadoAsiento_62FD);

            if (fa_62FD < 0)
            {
                throw new Exception("No se pudo actualizar el estado del asiento.");
            }
        }
    }
}
