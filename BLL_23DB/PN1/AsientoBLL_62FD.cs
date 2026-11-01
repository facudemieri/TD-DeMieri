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
        EventoBLL_23DB eventoBLL_23DB = new EventoBLL_23DB();

        // Detalle del último registro en bitácora que falló: lo necesita el aviso de la pantalla
        public string ErrorBitacora_62FD { get; private set; }

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

        public int AgregarAsiento_62FD(Asiento_62FD asiento_62FD, out bool bitacoraRegistrada_62FD)
        {
            bitacoraRegistrada_62FD = false;

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

            bitacoraRegistrada_62FD = RegistrarEventoBitacora_62FD("Agregar Asiento", 3);

            return asiento_62FD.IdAsiento_62FD;
        }

        public bool ModificarAsiento_62FD(Asiento_62FD asiento_62FD, out bool bitacoraRegistrada_62FD)
        {
            bitacoraRegistrada_62FD = false;

            if (mapperAsiento_62FD.Actualizar_62FD(asiento_62FD) != 1)
            {
                return false;
            }

            dvBLL_23DB.RecalcularDVTabla_23DB("Asiento_62FD");

            bitacoraRegistrada_62FD = RegistrarEventoBitacora_62FD("Modificar Asiento", 3);

            return true;
        }

        public int EliminarAsiento_62FD(int idAsiento_62FD, out bool bitacoraRegistrada_62FD)
        {
            bitacoraRegistrada_62FD = false;

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

            bitacoraRegistrada_62FD = RegistrarEventoBitacora_62FD("Eliminar Asiento", 3);

            return 1;
        }

        private bool RegistrarEventoBitacora_62FD(string evento_62FD, int criticidad_62FD)
        {
            try
            {
                eventoBLL_23DB.RegistrarEvento_23DB(SessionManager_23DB.ObtenerInstancia_23DB().DNI_23DB, "Maestros", evento_62FD, criticidad_62FD);
                ErrorBitacora_62FD = null;
                return true;
            }
            catch (Exception ex_62FD)
            {
                ErrorBitacora_62FD = ex_62FD.Message;
                return false;
            }
        }
    }
}
