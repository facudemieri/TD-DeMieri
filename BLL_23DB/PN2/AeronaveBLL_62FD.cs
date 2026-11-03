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
        EventoBLL_23DB eventoBLL_23DB = new EventoBLL_23DB();

        // Detalle del ultimo registro en bitacora que fallo
        public string ErrorBitacora_62FD { get; private set; }

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

        public int CrearAeronave_62FD(Aeronave_62FD aeronave_62FD, out bool bitacoraRegistrada_62FD)
        {
            bitacoraRegistrada_62FD = false;

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

            bitacoraRegistrada_62FD = RegistrarEventoBitacora_62FD("Crear Aeronave");

            return 1;
        }

        public bool ModificarAeronave_62FD(Aeronave_62FD aeronave_62FD, out bool bitacoraRegistrada_62FD)
        {
            bitacoraRegistrada_62FD = false;

            if (mapperAeronave_62FD.Actualizar_62FD(aeronave_62FD) != 1)
            {
                return false;
            }

            dvBLL_23DB.RecalcularDVTabla_23DB("Aeronave_62FD");
            dvBLL_23DB.RecalcularDVTabla_23DB("Aeronave_62FD_C");

            bitacoraRegistrada_62FD = RegistrarEventoBitacora_62FD("Modificar Aeronave");

            return true;
        }

        public int CambiarEstadoAlta_62FD(string matricula_62FD, bool activo_62FD, out bool bitacoraRegistrada_62FD)
        {
            bitacoraRegistrada_62FD = false;

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

            bitacoraRegistrada_62FD = RegistrarEventoBitacora_62FD(activo_62FD ? "Reactivar Aeronave" : "Dar de Baja Aeronave");

            return 1;
        }

        private bool RegistrarEventoBitacora_62FD(string evento_62FD)
        {
            try
            {
                eventoBLL_23DB.RegistrarEvento_23DB(SessionManager_23DB.ObtenerInstancia_23DB().DNI_23DB, "Maestros", evento_62FD, 3);
                ErrorBitacora_62FD = null;
                return true;
            }
            catch (Exception ex_62FD)
            {
                ErrorBitacora_62FD = ex_62FD.Message;
                return false;
            }
        }

        public int ContarRevisionesVencidas_62FD()
        {
            List<Aeronave_62FD> flota_62FD = mapperAeronave_62FD.ObtenerAeronaves_62FD("", "", "", "");
            int contador_62FD = 0;

            foreach (Aeronave_62FD aeronave_62FD in flota_62FD)
            {
                if (aeronave_62FD.FechaProximaRevision_62FD.Date <= DateTime.Today)
                {
                    contador_62FD++;
                }
            }

            return contador_62FD;
        }

        public int ContarRevisionesProximas_62FD(int dias_62FD)
        {
            List<Aeronave_62FD> flota_62FD = mapperAeronave_62FD.ObtenerAeronaves_62FD("", "", "", "");
            DateTime limite_62FD = DateTime.Today.AddDays(dias_62FD);
            int contador_62FD = 0;

            foreach (Aeronave_62FD aeronave_62FD in flota_62FD)
            {
                if (aeronave_62FD.FechaProximaRevision_62FD.Date > DateTime.Today && aeronave_62FD.FechaProximaRevision_62FD.Date <= limite_62FD)
                {
                    contador_62FD++;
                }
            }

            return contador_62FD;
        }

        public List<KeyValuePair<string, int>> ObtenerDistribucionEstadoOperativo_62FD()
        {
            string[] estados_62FD = new string[] { "En Servicio", "En Mantenimiento", "Fuera de Servicio" };
            int[] conteo_62FD = new int[estados_62FD.Length];

            List<Aeronave_62FD> flota_62FD = mapperAeronave_62FD.ObtenerAeronaves_62FD("", "", "", "");

            foreach (Aeronave_62FD aeronave_62FD in flota_62FD)
            {
                for (int i_62FD = 0; i_62FD < estados_62FD.Length; i_62FD++)
                {
                    if (aeronave_62FD.EstadoOperativo_62FD == estados_62FD[i_62FD])
                    {
                        conteo_62FD[i_62FD]++;
                    }
                }
            }

            List<KeyValuePair<string, int>> distribucion_62FD = new List<KeyValuePair<string, int>>();

            for (int i_62FD = 0; i_62FD < estados_62FD.Length; i_62FD++)
            {
                distribucion_62FD.Add(new KeyValuePair<string, int>(estados_62FD[i_62FD], conteo_62FD[i_62FD]));
            }

            return distribucion_62FD;
        }

        public List<KeyValuePair<string, int>> ProyectarRevisionesPorMes_62FD(int meses_62FD)
        {
            int[] conteo_62FD = new int[meses_62FD];
            DateTime primerMes_62FD = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            DateTime limite_62FD = primerMes_62FD.AddMonths(meses_62FD);

            List<Aeronave_62FD> flota_62FD = mapperAeronave_62FD.ObtenerAeronaves_62FD("", "", "", "");

            foreach (Aeronave_62FD aeronave_62FD in flota_62FD)
            {
                DateTime fecha_62FD = aeronave_62FD.FechaProximaRevision_62FD.Date;

                if (fecha_62FD < DateTime.Today)
                {
                    fecha_62FD = DateTime.Today;
                }

                while (fecha_62FD < limite_62FD)
                {
                    int indice_62FD = ((fecha_62FD.Year - primerMes_62FD.Year) * 12) + fecha_62FD.Month - primerMes_62FD.Month;

                    if (indice_62FD >= 0 && indice_62FD < meses_62FD)
                    {
                        conteo_62FD[indice_62FD]++;
                    }

                    if (aeronave_62FD.IntervaloRevision_62FD <= 0)
                    {
                        break;
                    }

                    fecha_62FD = fecha_62FD.AddDays(aeronave_62FD.IntervaloRevision_62FD);
                }
            }

            List<KeyValuePair<string, int>> proyeccion_62FD = new List<KeyValuePair<string, int>>();

            for (int i_62FD = 0; i_62FD < meses_62FD; i_62FD++)
            {
                proyeccion_62FD.Add(new KeyValuePair<string, int>(primerMes_62FD.AddMonths(i_62FD).ToString("MMM yyyy"), conteo_62FD[i_62FD]));
            }

            return proyeccion_62FD;
        }
    }
}
