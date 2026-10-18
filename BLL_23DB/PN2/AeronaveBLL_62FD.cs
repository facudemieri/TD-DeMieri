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
    public class AeronaveBLL_62FD
    {
        mapperAeronave_62FD mapperAeronave_62FD = new mapperAeronave_62FD();
        mapperOrdenTrabajo_62FD mapperOrdenTrabajo_62FD = new mapperOrdenTrabajo_62FD();
        DVBLL_23DB dvBLL_23DB = new DVBLL_23DB();

        public List<Aeronave_62FD> ObtenerAeronaves_62FD(string matricula_62FD, string modelo_62FD, string fabricante_62FD, string estadoOperativo_62FD, bool soloRevisionVencida_62FD)
        {
            List<Aeronave_62FD> lista_62FD = mapperAeronave_62FD.ObtenerAeronaves_62FD(matricula_62FD, modelo_62FD, fabricante_62FD, estadoOperativo_62FD);

            if (soloRevisionVencida_62FD)
            {
                lista_62FD = lista_62FD.FindAll(a_62FD => a_62FD.FechaProximaRevision_62FD.Date <= DateTime.Now.Date);
            }

            return lista_62FD;
        }

        public List<Aeronave_62FD> ObtenerTodas_62FD()
        {
            return mapperAeronave_62FD.ObtenerTodas_62FD();
        }

        public int CrearAeronave_62FD(Aeronave_62FD aeronave_62FD)
        {
            int filas_62FD = mapperAeronave_62FD.Insertar_62FD(aeronave_62FD);

            if (filas_62FD == 0)
            {
                return 0;
            }

            if (filas_62FD != 1)
            {
                return -1;
            }

            dvBLL_23DB.RecalcularDVTabla_23DB("Aeronave_62FD");
            dvBLL_23DB.RecalcularDVTabla_23DB("Aeronave_62FD_C");
            return 1;
        }

        public bool ModificarAeronave_62FD(Aeronave_62FD aeronave_62FD)
        {
            if (mapperAeronave_62FD.Actualizar_62FD(aeronave_62FD) != 1)
            {
                return false;
            }

            dvBLL_23DB.RecalcularDVTabla_23DB("Aeronave_62FD");
            dvBLL_23DB.RecalcularDVTabla_23DB("Aeronave_62FD_C");
            return true;
        }

        public int CambiarEstadoAlta_62FD(string matricula_62FD, bool activo_62FD)
        {
            if (!activo_62FD && mapperOrdenTrabajo_62FD.ContarOrdenesAbiertas_62FD(matricula_62FD) > 0)
            {
                return 0;
            }

            if (mapperAeronave_62FD.CambiarEstadoAlta_62FD(matricula_62FD, activo_62FD) != 1)
            {
                return -1;
            }

            dvBLL_23DB.RecalcularDVTabla_23DB("Aeronave_62FD");
            dvBLL_23DB.RecalcularDVTabla_23DB("Aeronave_62FD_C");
            return 1;
        }
    }
}
