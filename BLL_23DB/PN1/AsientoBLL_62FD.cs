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
    public class AsientoBLL_62FD
    {
        mapperAsiento_62FD mapperAsiento_62FD = new mapperAsiento_62FD();
        DVBLL_23DB dvBLL_23DB = new DVBLL_23DB();

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

        public List<Asiento_62FD> ObtenerTodosPorVuelo_62FD(string nroVuelo_62FD)
        {
            return mapperAsiento_62FD.ObtenerAsientosPorVuelo_62FD(nroVuelo_62FD);
        }

        public int AgregarAsiento_62FD(Asiento_62FD asiento_62FD)
        {
            asiento_62FD.IdAsiento_62FD = mapperAsiento_62FD.ObtenerProximoId_62FD();
            asiento_62FD.EstadoAsiento_62FD = "Disponible";

            int filas_62FD = mapperAsiento_62FD.Insertar_62FD(asiento_62FD);

            if (filas_62FD == 0)
            {
                return 0;
            }

            if (filas_62FD != 1)
            {
                return -1;
            }

            dvBLL_23DB.RecalcularDVTabla_23DB("Asiento_62FD");

            return asiento_62FD.IdAsiento_62FD;
        }

        public bool ModificarAsiento_62FD(Asiento_62FD asiento_62FD)
        {
            if (mapperAsiento_62FD.Actualizar_62FD(asiento_62FD) != 1)
            {
                return false;
            }

            dvBLL_23DB.RecalcularDVTabla_23DB("Asiento_62FD");

            return true;
        }

        public int EliminarAsiento_62FD(int idAsiento_62FD)
        {
            int filas_62FD = mapperAsiento_62FD.Eliminar_62FD(idAsiento_62FD);

            if (filas_62FD == 0)
            {
                return 0;
            }

            if (filas_62FD != 1)
            {
                return -1;
            }

            dvBLL_23DB.RecalcularDVTabla_23DB("Asiento_62FD");

            return 1;
        }
    }
}
