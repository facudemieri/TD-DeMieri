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
        EventoBLL_23DB eventoBLL_23DB = new EventoBLL_23DB();

        // Detalle del ultimo registro en bitacora que fallo
        public string ErrorBitacora_62FD { get; private set; }

        public int SolicitarRepuesto_62FD(Repuesto_62FD repuesto_62FD, out bool bitacoraRegistrada_62FD)
        {
            bitacoraRegistrada_62FD = false;

            repuesto_62FD.nroSolicitud_62FD = mapperRepuesto_62FD.ObtenerProximoNroSolicitud_62FD();
            repuesto_62FD.estadoPedido_62FD = "Solicitado";
            repuesto_62FD.fechaSolicitud_62FD = DateTime.Now;
            repuesto_62FD.fechaRecepcion_62FD = null;

            if (mapperRepuesto_62FD.InsertarRepuesto_62FD(repuesto_62FD) != 1)
            {
                return -1;
            }

            dvBLL_23DB.RecalcularDVTabla_23DB("Repuesto_62FD");

            bitacoraRegistrada_62FD = RegistrarEventoBitacora_62FD("Solicitar Repuestos", 3);

            return repuesto_62FD.nroSolicitud_62FD;
        }

        public bool RegistrarRecepcion_62FD(int nroSolicitud_62FD, out bool bitacoraRegistrada_62FD)
        {
            bitacoraRegistrada_62FD = false;

            if (mapperRepuesto_62FD.RegistrarRecepcion_62FD(nroSolicitud_62FD, DateTime.Now) != 1)
            {
                return false;
            }

            dvBLL_23DB.RecalcularDVTabla_23DB("Repuesto_62FD");

            bitacoraRegistrada_62FD = RegistrarEventoBitacora_62FD("Registrar Recepción de Repuestos", 3);

            return true;
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

        public List<Repuesto_62FD> ObtenerRepuestosPorTarea_62FD(int idTarea_62FD)
        {
            return mapperRepuesto_62FD.ObtenerRepuestosPorTarea_62FD(idTarea_62FD);
        }

        public List<Repuesto_62FD> ObtenerNombres_62FD()
        {
            return mapperRepuesto_62FD.ObtenerNombres_62FD();
        }

        public List<Repuesto_62FD> ObtenerTodos_62FD()
        {
            return mapperRepuesto_62FD.ObtenerTodos_62FD();
        }

        public const decimal SinDatos_62FD = -1;

        public decimal ObtenerPromedioDiasReposicion_62FD()
        {
            List<Repuesto_62FD> lista_62FD = mapperRepuesto_62FD.ObtenerTodos_62FD();
            decimal totalDias_62FD = 0;
            int cantidad_62FD = 0;

            foreach (Repuesto_62FD repuesto_62FD in lista_62FD)
            {
                if (repuesto_62FD.fechaRecepcion_62FD != null)
                {
                    TimeSpan diferencia_62FD = repuesto_62FD.fechaRecepcion_62FD.Value.Date - repuesto_62FD.fechaSolicitud_62FD.Date;

                    if (diferencia_62FD.Days >= 0)
                    {
                        totalDias_62FD += diferencia_62FD.Days;
                        cantidad_62FD++;
                    }
                }
            }

            if (cantidad_62FD == 0)
            {
                return SinDatos_62FD;
            }

            return Math.Round(totalDias_62FD / cantidad_62FD, 1);
        }
    }
}
