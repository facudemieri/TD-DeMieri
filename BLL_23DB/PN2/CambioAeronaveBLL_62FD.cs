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

        public bool ActivarCambio_62FD(string matricula_62FD, DateTime fechaHoraCambio_62FD)
        {
            if (mapperAeronaveCambio_62FD.Activar_62FD(matricula_62FD, fechaHoraCambio_62FD) <= 0)
            {
                return false;
            }

            dvBLL_23DB.RecalcularDVTabla_23DB("Aeronave_62FD");
            dvBLL_23DB.RecalcularDVTabla_23DB("Aeronave_62FD_C");

            return true;
        }
    }
}
