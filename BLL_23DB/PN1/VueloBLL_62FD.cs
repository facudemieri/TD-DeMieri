using BE_23DB;
using Mapper_23DB;
using Services_23DB;
using Services_23DB.Serializacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_23DB
{
    public class VueloBLL_62FD
    {
        mapperVuelo_62FD mapperVuelo_62FD = new mapperVuelo_62FD();
        mapperAeronave_62FD mapperAeronave_62FD = new mapperAeronave_62FD();
        DVBLL_23DB dvBLL_23DB = new DVBLL_23DB();
        EventoBLL_23DB eventoBLL_23DB = new EventoBLL_23DB();

        public const int HorasMinimasEntreVuelos_62FD = 3;
        public const int AeronaveConVueloCercano_62FD = -3;

        // Detalle del último registro en bitácora que falló: lo necesita el aviso de la pantalla
        public string ErrorBitacora_62FD { get; private set; }

        public List<Vuelo_62FD> BuscarVuelos_62FD(string origen_62FD, string destino_62FD, DateTime fechaPartida_62FD, int cantidadPasajeros_62FD)
        {
            if (!ValidarCriterios_62FD(origen_62FD, destino_62FD, fechaPartida_62FD, cantidadPasajeros_62FD))
            {
                throw new Exception("Los criterios de búsqueda son inválidos.");
            }

            List<Vuelo_62FD> vuelos_62FD = mapperVuelo_62FD.ObtenerVuelos_62FD(origen_62FD, destino_62FD, fechaPartida_62FD);
            List<Vuelo_62FD> resultado_62FD = new List<Vuelo_62FD>();

            foreach(Vuelo_62FD vuelo_62FD in vuelos_62FD)
            {
                vuelo_62FD.AsientosDisponibles_62FD = mapperVuelo_62FD.ContarAsientosDisponibles_62FD(vuelo_62FD.NroVuelo_62FD);

                if(vuelo_62FD.AsientosDisponibles_62FD >= cantidadPasajeros_62FD)
                {
                    resultado_62FD.Add(vuelo_62FD);
                }
            }

            return resultado_62FD;
        }

        public bool ValidarCriterios_62FD(string origen_62FD, string destino_62FD, DateTime fechaPartida_62FD, int cantidadPasajeros_62FD)
        {
            if(string.IsNullOrEmpty(origen_62FD) || string.IsNullOrEmpty(destino_62FD))
            {
                return false;
            }

            if(origen_62FD == destino_62FD)
            {
                return false;
            }

            if(fechaPartida_62FD.Date < DateTime.Now.Date)
            {
                return false;
            }

            if(cantidadPasajeros_62FD < 1)
            {
                return false;
            }

            return true;
        }

        public List<string> ObtenerOrigenes_62FD()
        {

            List<string> lista_62FD = new List<string>();

            foreach (Vuelo_62FD vuelo_62FD in mapperVuelo_62FD.ObtenerTodosLosVuelos_62FD())
            {
                if (!lista_62FD.Contains(vuelo_62FD.Origen_62FD))
                {
                    lista_62FD.Add(vuelo_62FD.Origen_62FD);
                }
            }

            lista_62FD.Sort();
            return lista_62FD;
        }

        public List<string> ObtenerDestinos_62FD()
        {
            List<string> lista_62FD = new List<string>();

            foreach (Vuelo_62FD vuelo_62FD in mapperVuelo_62FD.ObtenerTodosLosVuelos_62FD())
            {
                if (!lista_62FD.Contains(vuelo_62FD.Destino_62FD))
                {
                    lista_62FD.Add(vuelo_62FD.Destino_62FD);
                }
            }

            lista_62FD.Sort();
            return lista_62FD;
        }

        public List<Vuelo_62FD> ObtenerTodosLosVuelos_62FD() 
        {
            List<Vuelo_62FD> vuelos_62FD = mapperVuelo_62FD.ObtenerTodosLosVuelos_62FD();
            List<Vuelo_62FD> resultado_62FD = new List<Vuelo_62FD>();

            foreach (Vuelo_62FD vuelo_62FD in vuelos_62FD)
            {
                if (vuelo_62FD.EstadoVuelo_62FD == "Cancelado")
                {
                    continue;
                }

                if (vuelo_62FD.FechaHoraSalida_62FD <= DateTime.Now)
                {
                    continue;
                }

                vuelo_62FD.AsientosDisponibles_62FD = mapperVuelo_62FD.ContarAsientosDisponibles_62FD(vuelo_62FD.NroVuelo_62FD);

                if (vuelo_62FD.AsientosDisponibles_62FD > 0)
                {
                    resultado_62FD.Add(vuelo_62FD);
                }
            }

            return resultado_62FD;
        }

        public List<Vuelo_62FD> ObtenerVuelosMaestro_62FD()
        {
            return mapperVuelo_62FD.ObtenerTodosLosVuelos_62FD();
        }

        public void SerializarVuelos_62FD(List<Vuelo_62FD> listaVuelos_62FD, string rutaArchivo_62FD, out bool bitacoraRegistrada_62FD)
        {
            SerializarBLL_62FD serializarBLL_62FD = new SerializarBLL_62FD();
            serializarBLL_62FD.Serializar_62DB(listaVuelos_62FD, rutaArchivo_62FD);

            bitacoraRegistrada_62FD = RegistrarEventoSerializacion_62FD("Serializar Vuelos");
        }

        public List<Vuelo_62FD> DeserializarVuelos_62FD(string rutaArchivo_62FD, out bool bitacoraRegistrada_62FD)
        {
            DeserializarBLL_62FD deserializarBLL_62FD = new DeserializarBLL_62FD();
            List<Vuelo_62FD> listaVuelos_62FD = deserializarBLL_62FD.Deserializar_62DB<Vuelo_62FD>(rutaArchivo_62FD);

            bitacoraRegistrada_62FD = RegistrarEventoSerializacion_62FD("Deserializar Vuelos");

            return listaVuelos_62FD;
        }

        private bool RegistrarEventoSerializacion_62FD(string evento_62FD)
        {
            return RegistrarEventoBitacora_62FD(evento_62FD, 4);
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

        public bool ExisteAeronave_62FD(string matricula_62FD)
        {
            return mapperAeronave_62FD.ObtenerTodas_62FD().Exists(a_62FD => a_62FD.Matricula_62FD == matricula_62FD && a_62FD.Activo_62FD);
        }

        public int CrearVuelo_62FD(Vuelo_62FD vuelo_62FD, out bool bitacoraRegistrada_62FD)
        {
            bitacoraRegistrada_62FD = false;

            if (!ExisteAeronave_62FD(vuelo_62FD.Matricula_62FD))
            {
                return -2;
            }

            if (mapperVuelo_62FD.ContarVuelosEnVentana_62FD(vuelo_62FD.Matricula_62FD, vuelo_62FD.FechaHoraSalida_62FD, HorasMinimasEntreVuelos_62FD, null) > 0)
            {
                return AeronaveConVueloCercano_62FD;
            }

            vuelo_62FD.EstadoVuelo_62FD = "Programado";

            int filas_62FD = mapperVuelo_62FD.Insertar_62FD(vuelo_62FD);

            if (filas_62FD == 0)
            {
                return 0;
            }

            if (filas_62FD != 1)
            {
                return -1;
            }

            dvBLL_23DB.RecalcularDVTabla_23DB("Vuelo_62FD");

            bitacoraRegistrada_62FD = RegistrarEventoBitacora_62FD("Crear Vuelo", 3);

            return 1;
        }

        public int ModificarVuelo_62FD(Vuelo_62FD vuelo_62FD, out bool bitacoraRegistrada_62FD)
        {
            bitacoraRegistrada_62FD = false;

            if (!ExisteAeronave_62FD(vuelo_62FD.Matricula_62FD))
            {
                return -2;
            }

            if (vuelo_62FD.EstadoVuelo_62FD == "Programado" && mapperVuelo_62FD.ContarVuelosEnVentana_62FD(vuelo_62FD.Matricula_62FD, vuelo_62FD.FechaHoraSalida_62FD, HorasMinimasEntreVuelos_62FD, vuelo_62FD.NroVuelo_62FD) > 0)
            {
                return AeronaveConVueloCercano_62FD;
            }

            if (mapperVuelo_62FD.Actualizar_62FD(vuelo_62FD) != 1)
            {
                return -1;
            }

            dvBLL_23DB.RecalcularDVTabla_23DB("Vuelo_62FD");

            bitacoraRegistrada_62FD = RegistrarEventoBitacora_62FD("Modificar Vuelo", 3);

            return 1;
        }

        public bool CancelarVuelo_62FD(string nroVuelo_62FD, out bool bitacoraRegistrada_62FD)
        {
            bitacoraRegistrada_62FD = false;

            if (mapperVuelo_62FD.ActualizarEstado_62FD(nroVuelo_62FD, "Cancelado") != 1)
            {
                return false;
            }

            dvBLL_23DB.RecalcularDVTabla_23DB("Vuelo_62FD");

            bitacoraRegistrada_62FD = RegistrarEventoBitacora_62FD("Cancelar Vuelo", 3);

            return true;
        }


    }
}
