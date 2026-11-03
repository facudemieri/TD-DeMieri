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
    public class IntervencionBLL_62FD
    {
        mapperIntervencion_62FD mapperIntervencion_62FD = new mapperIntervencion_62FD();
        mapperTarea_62FD mapperTarea_62FD = new mapperTarea_62FD();
        mapperOrdenTrabajo_62FD mapperOrdenTrabajo_62FD = new mapperOrdenTrabajo_62FD();
        mapperAeronave_62FD mapperAeronave_62FD = new mapperAeronave_62FD();
        DVBLL_23DB dvBLL_23DB = new DVBLL_23DB();
        EventoBLL_23DB eventoBLL_23DB = new EventoBLL_23DB();

        // Detalle del ultimo registro en bitacora que fallo
        public string ErrorBitacora_62FD { get; private set; }

        // Codigos de resultado de IniciarIntervencion_62FD
        public const int IntervencionIniciada_62FD = 1;
        public const int IntervencionRetomada_62FD = 2;
        public const int DatoModificado_62FD = -1;
        public const int AeronaveNoActualizada_62FD = -2;

        // Codigos de resultado de CerrarIntervencion_62FD 
        public const int IntervencionCerrada_62FD = 1;
        public const int SinTareas_62FD = -3;
        public const int TareasPendientes_62FD = -4;
        public const int RepuestosPendientes_62FD = -5;

        public List<OrdenTrabajo_62FD> ObtenerOrdenesAsignadas_62FD(string legajoTecnico_62FD)
        {
            return mapperOrdenTrabajo_62FD.ObtenerOrdenesAsignadas_62FD(legajoTecnico_62FD);
        }

        public OrdenTrabajo_62FD ObtenerOrdenPorId_62FD(int idOrden_62FD)
        {
            return mapperOrdenTrabajo_62FD.ObtenerOrdenPorId_62FD(idOrden_62FD);
        }

        public List<Tarea_62FD> ObtenerTareas_62FD(int idIntervencion_62FD)
        {
            return mapperTarea_62FD.ObtenerTareasPorIntervencion_62FD(idIntervencion_62FD);
        }

        // Permite a la GUI saber si se va a retomar una intervención existente
        public Intervencion_62FD ObtenerIntervencionEnCurso_62FD(int idOrden_62FD)
        {
            return mapperIntervencion_62FD.ObtenerEnCursoPorOrden_62FD(idOrden_62FD);
        }


        public int IniciarIntervencion_62FD(int idOrden_62FD, out Intervencion_62FD intervencion_62FD)
        {
            intervencion_62FD = mapperIntervencion_62FD.ObtenerEnCursoPorOrden_62FD(idOrden_62FD);

            if (intervencion_62FD != null)
            {
                return IntervencionRetomada_62FD;
            }

            OrdenTrabajo_62FD orden_62FD = mapperOrdenTrabajo_62FD.ObtenerOrdenPorId_62FD(idOrden_62FD);

            if (orden_62FD == null)
            {
                return DatoModificado_62FD;
            }

            Intervencion_62FD nueva_62FD = new Intervencion_62FD();
            nueva_62FD.IdIntervencion_62FD = mapperIntervencion_62FD.ObtenerProximoId_62FD();
            nueva_62FD.IdOrden_62FD = idOrden_62FD;
            nueva_62FD.estadoIntervencion_62FD = "En Curso";
            nueva_62FD.fechaHoraInicio_62FD = DateTime.Now;
            nueva_62FD.fechaHoraFin_62FD = null;
            nueva_62FD.totalHorasInsumidas_62FD = 0;

            if (mapperIntervencion_62FD.InsertarIntervencion_62FD(nueva_62FD) != 1)
            {
                return DatoModificado_62FD;
            }

            dvBLL_23DB.RecalcularDVTabla_23DB("Intervencion_62FD");
            intervencion_62FD = nueva_62FD;

            int filasAeronave_62FD;

            try
            {
                filasAeronave_62FD = mapperAeronave_62FD.ActualizarEstadoOperativo_62FD(orden_62FD.Matricula_62FD, "En Mantenimiento");
            }
            catch (Exception)
            {
                return AeronaveNoActualizada_62FD;
            }

            if (filasAeronave_62FD != 1)
            {
                return AeronaveNoActualizada_62FD;
            }

            dvBLL_23DB.RecalcularDVTabla_23DB("Aeronave_62FD");
            dvBLL_23DB.RecalcularDVTabla_23DB("Aeronave_62FD_C");

            return IntervencionIniciada_62FD;
        }

        public int AgregarTarea_62FD(Tarea_62FD tarea_62FD)
        {
            tarea_62FD.IdTarea_62FD = mapperTarea_62FD.ObtenerProximoId_62FD();

            if (mapperTarea_62FD.InsertarTarea_62FD(tarea_62FD) != 1)
            {
                return DatoModificado_62FD;
            }

            dvBLL_23DB.RecalcularDVTabla_23DB("Tarea_62FD");

            return tarea_62FD.IdTarea_62FD;
        }

        public bool QuitarTarea_62FD(int idTarea_62FD)
        {
            if (mapperTarea_62FD.EliminarTarea_62FD(idTarea_62FD) != 1)
            {
                return false;
            }

            dvBLL_23DB.RecalcularDVTabla_23DB("Tarea_62FD");

            return true;
        }

        public int CerrarIntervencion_62FD(Intervencion_62FD intervencion_62FD, out bool bitacoraRegistrada_62FD)
        {
            bitacoraRegistrada_62FD = false;

            List<Tarea_62FD> tareas_62FD = mapperTarea_62FD.ObtenerTareasPorIntervencion_62FD(intervencion_62FD.IdIntervencion_62FD);

            if (tareas_62FD.Count == 0)
            {
                return SinTareas_62FD;
            }

            if (tareas_62FD.Exists(t_62FD => t_62FD.ResultadoObtenido_62FD == "Pendiente"))
            {
                return TareasPendientes_62FD;
            }

            if (tareas_62FD.Exists(t_62FD => t_62FD.EstadoRepuestos_62FD == "Solicitado"))
            {
                return RepuestosPendientes_62FD;
            }

            decimal total_62FD = 0;

            foreach (Tarea_62FD tarea_62FD in tareas_62FD)
            {
                total_62FD += tarea_62FD.HorasInsumidas_62FD;
            }

            intervencion_62FD.totalHorasInsumidas_62FD = total_62FD;
            intervencion_62FD.estadoIntervencion_62FD = "Finalizada";

            if (mapperIntervencion_62FD.ActualizarCierre_62FD(intervencion_62FD) != 1)
            {
                return DatoModificado_62FD;
            }

            int filasOrden_62FD = mapperOrdenTrabajo_62FD.ActualizarEstadoOrden_62FD(intervencion_62FD.IdOrden_62FD, "Ejecutada");

            dvBLL_23DB.RecalcularDVTabla_23DB("Intervencion_62FD");

            if (filasOrden_62FD != 1)
            {
                return DatoModificado_62FD;
            }

            dvBLL_23DB.RecalcularDVTabla_23DB("OrdenTrabajo_62FD");

            bitacoraRegistrada_62FD = RegistrarEventoBitacora_62FD("Registrar Intervención", 3);

            return IntervencionCerrada_62FD;
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

        public bool ModificarTarea_62FD(Tarea_62FD tarea_62FD)
        {
            if (mapperTarea_62FD.ActualizarTarea_62FD(tarea_62FD) != 1)
            {
                return false;
            }

            dvBLL_23DB.RecalcularDVTabla_23DB("Tarea_62FD");

            return true;
        }

        public List<Intervencion_62FD> ObtenerIntervencionesConOrden_62FD()
        {
            return mapperIntervencion_62FD.ObtenerIntervencionesConOrden_62FD();
        }

        public List<KeyValuePair<string, int>> ContarIntervencionesPorTipo_62FD(DateTime desde_62FD, DateTime hasta_62FD)
        {
            string[] tipos_62FD = new string[] { "Preventivo", "Correctivo" };
            int[] conteo_62FD = new int[tipos_62FD.Length];

            List<Intervencion_62FD> lista_62FD = mapperIntervencion_62FD.ObtenerIntervencionesConOrden_62FD();

            foreach (Intervencion_62FD intervencion_62FD in lista_62FD)
            {
                if (intervencion_62FD.fechaHoraInicio_62FD.Date >= desde_62FD.Date && intervencion_62FD.fechaHoraInicio_62FD.Date <= hasta_62FD.Date)
                {
                    for (int i_62FD = 0; i_62FD < tipos_62FD.Length; i_62FD++)
                    {
                        if (intervencion_62FD.TipoIntervencion_62FD == tipos_62FD[i_62FD])
                        {
                            conteo_62FD[i_62FD]++;
                        }
                    }
                }
            }

            List<KeyValuePair<string, int>> distribucion_62FD = new List<KeyValuePair<string, int>>();

            for (int i_62FD = 0; i_62FD < tipos_62FD.Length; i_62FD++)
            {
                distribucion_62FD.Add(new KeyValuePair<string, int>(tipos_62FD[i_62FD], conteo_62FD[i_62FD]));
            }

            return distribucion_62FD;
        }
    }
}
