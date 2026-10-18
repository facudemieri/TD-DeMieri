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
    public class RepuestoBLL_62FD
    {
        mapperRepuesto_62FD mapperRepuesto_62FD = new mapperRepuesto_62FD();
        DVBLL_23DB dvBLL_23DB = new DVBLL_23DB();

        public int SolicitarRepuesto_62FD(Repuesto_62FD repuesto_62FD)
        {
            repuesto_62FD.nroSolicitud_62FD = mapperRepuesto_62FD.ObtenerProximoNroSolicitud_62FD();
            repuesto_62FD.estadoPedido_62FD = "Solicitado";
            repuesto_62FD.fechaSolicitud_62FD = DateTime.Now;
            repuesto_62FD.fechaRecepcion_62FD = null;

            if (mapperRepuesto_62FD.InsertarRepuesto_62FD(repuesto_62FD) != 1)
            {
                return -1;
            }

            dvBLL_23DB.RecalcularDVTabla_23DB("Repuesto_62FD");

            return repuesto_62FD.nroSolicitud_62FD;
        }

        public bool RegistrarRecepcion_62FD(int nroSolicitud_62FD)
        {
            if (mapperRepuesto_62FD.RegistrarRecepcion_62FD(nroSolicitud_62FD, DateTime.Now) != 1)
            {
                return false;
            }

            dvBLL_23DB.RecalcularDVTabla_23DB("Repuesto_62FD");

            return true;
        }

        public List<Repuesto_62FD> ObtenerRepuestosPorTarea_62FD(int idTarea_62FD)
        {
            return mapperRepuesto_62FD.ObtenerRepuestosPorTarea_62FD(idTarea_62FD);
        }

        public List<Repuesto_62FD> ObtenerNombres_62FD()
        {
            return mapperRepuesto_62FD.ObtenerNombres_62FD();
        }
    }
}
