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
    public class PasajeroBLL_62FD
    {
        mapperPasajero_62FD mapperPasajero_62FD = new mapperPasajero_62FD();

        DVBLL_23DB dVBLL_23DB = new DVBLL_23DB();

        public Pasajero_62FD ObtenerPasajero_62FD(string numeroDocumento_62FD)
        {
            return mapperPasajero_62FD.ObtenerPasajeroPorDocumento_62FD(numeroDocumento_62FD);
        }

        public void RegistrarPasajero_62FD(Pasajero_62FD pasajero_62FD)
        {
            if (!ValidaDatos_62FD(pasajero_62FD))
            {
                throw new Exception("Los datos del pasajero son inválidos.");
            }

            if (ObtenerPasajero_62FD(pasajero_62FD.NumeroDocumento_62FD) != null)
            {
                throw new Exception("El pasajero ya se encuentra registrado.");
            }

            int fa_62FD = mapperPasajero_62FD.InsertarPasajero_62FD(pasajero_62FD);

            if (fa_62FD < 0)
            {
                throw new Exception("No se pudo registrar el pasajero.");
            }

            dVBLL_23DB.RecalcularDVTabla_23DB("Pasajero_62FD");
        }

        public bool ValidaDatos_62FD(Pasajero_62FD pasajero_62FD)
        {
                        
            if (string.IsNullOrEmpty(pasajero_62FD.NumeroDocumento_62FD) || string.IsNullOrEmpty(pasajero_62FD.Apellido_62FD) || string.IsNullOrEmpty(pasajero_62FD.Nombre_62FD) || string.IsNullOrEmpty(pasajero_62FD.CorreoElectronico_62FD) || string.IsNullOrEmpty(pasajero_62FD.Telefono_62FD))
            {
                return false;
            }

            if (pasajero_62FD.NumeroDocumento_62FD.Length != 8)
            {
                return false;
            }

            if (!pasajero_62FD.CorreoElectronico_62FD.Contains("@"))
            {
                return false;
            }

            if (pasajero_62FD.FechaNacimiento_62FD.Date >= DateTime.Now.Date)
            {
                return false;
            }

            return true;
        }
    }
}
