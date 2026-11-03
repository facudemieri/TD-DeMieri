using BE_23DB.PN2;
using Mapper_23DB.PN2;
using Services_23DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_23DB.PN2
{
    public class CambioAeronaveBLL_62FD
    {
        mapperAeronaveCambio_62FD mapperAeronaveCambio_62FD = new mapperAeronaveCambio_62FD();
        DVBLL_23DB dvBLL_23DB = new DVBLL_23DB();
        EventoBLL_23DB eventoBLL_23DB = new EventoBLL_23DB();

        // Detalle del ultimo registro en bitacora que fallo
        public string ErrorBitacora_62FD { get; private set; }

        public List<AeronaveCambio_62FD> ObtenerCambios_62FD(string matricula_62FD, string modelo_62FD, DateTime? fechaDesde_62FD, DateTime? fechaHasta_62FD)
        {
            return mapperAeronaveCambio_62FD.ObtenerCambios_62FD(matricula_62FD, modelo_62FD, fechaDesde_62FD, fechaHasta_62FD);
        }

        public List<string> ObtenerMatriculas_62FD()
        {
            return mapperAeronaveCambio_62FD.ObtenerMatriculas_62FD();
        }

        public List<string> ObtenerModelos_62FD()
        {
            return mapperAeronaveCambio_62FD.ObtenerModelos_62FD();
        }

        public bool ActivarCambio_62FD(string matricula_62FD, DateTime fechaHoraCambio_62FD, out bool bitacoraRegistrada_62FD)
        {
            bitacoraRegistrada_62FD = false;

            if (mapperAeronaveCambio_62FD.Activar_62FD(matricula_62FD, fechaHoraCambio_62FD) <= 0)
            {
                return false;
            }

            try
            {
                dvBLL_23DB.RecalcularDVTabla_23DB("Aeronave_62FD");
                dvBLL_23DB.RecalcularDVTabla_23DB("Aeronave_62FD_C");
            }
            catch (Exception ex_62FD)
            {
                System.Diagnostics.Debug.WriteLine(ex_62FD);
            }

            bitacoraRegistrada_62FD = RegistrarEventoBitacora_62FD("Activar Cambio Aeronave");

            return true;
        }

        private bool RegistrarEventoBitacora_62FD(string evento_62FD)
        {
            try
            {
                eventoBLL_23DB.RegistrarEvento_23DB(SessionManager_23DB.ObtenerInstancia_23DB().DNI_23DB, "Maestros", evento_62FD, 2);
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
