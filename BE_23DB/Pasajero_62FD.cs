using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE_23DB
{
    public class Pasajero_62FD
    {
        private string numeroDocumento_62FD;

        public string NumeroDocumento_62FD
        {
            get { return numeroDocumento_62FD; }
            set { numeroDocumento_62FD = value; }
        }

        private string apellido_62FD;

        public string Apellido_62FD
        {
            get { return apellido_62FD; }
            set { apellido_62FD = value; }
        }

        private string nombre_62FD;

        public string Nombre_62FD
        {
            get { return nombre_62FD; }
            set { nombre_62FD = value; }
        }

        private DateTime fechaNacimiento_62FD;

        public DateTime FechaNacimiento_62FD
        {
            get { return fechaNacimiento_62FD; }
            set { fechaNacimiento_62FD = value; }
        }

        private string correoElectronico_62FD;

        public string CorreoElectronico_62FD
        {
            get { return correoElectronico_62FD; }
            set { correoElectronico_62FD = value; }
        }

        private string telefono_62FD;

        public string Telefono_62FD
        {
            get { return telefono_62FD; }
            set { telefono_62FD = value; }
        }

        private string contactoEmergencia_62FD;

        public string ContactoEmergencia_62FD
        {
            get { return contactoEmergencia_62FD; }
            set { contactoEmergencia_62FD = value; }
        }

        public string NombreCompleto_62FD
        {
            get { return apellido_62FD + ", " + nombre_62FD; }
        }
    }
}
