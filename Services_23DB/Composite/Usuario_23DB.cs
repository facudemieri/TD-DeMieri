using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services_23DB
{
    public class Usuario_23DB
    {
        private string DNI_;

        public string DNI_23DB
        {
            get { return DNI_; }
            set { DNI_ = value; }
        }

        private string Apellido_;

        public string Apellido_23DB
        {
            get { return Apellido_; }
            set { Apellido_ = value; }
        }

        private string nombre_;

        public string Nombre_23DB
        {
            get { return nombre_; }
            set { nombre_ = value; }
        }

        private string Email_;

        public string Email_23DB
        {
            get { return Email_; }
            set { Email_ = value; }
        }

        private string Login_;

        public string Login_23DB
        {
            get { return Login_; }
            set { Login_ = value; }
        }

        private string Password_;

        public string Password_23DB
        {
            get { return Password_; }
            set { Password_ = value; }
        }

        private int Rol_;

        public int IdRol_23DB
        {
            get { return Rol_; }
            set { Rol_ = value; }
        }

        private bool Bloqueado_;

        public bool Bloqueado_23DB
        {
            get { return Bloqueado_; }
            set { Bloqueado_ = value; }
        }

        private bool Activo_;

        public bool Activo_23DB
        {
            get { return Activo_; }
            set { Activo_ = value; }
        }

        private int intentosFallidos_;

        public int IntentosFallidos_23DB
        {
            get { return intentosFallidos_; }
            set { intentosFallidos_ = value; }
        }

        private string ultimoIdioma_;

        public string UltimoIdioma_23DB
        {
            get { return ultimoIdioma_; }
            set { ultimoIdioma_ = value; }
        }

        private DateTime? fechaUltimoIntento_;

        public DateTime? FechaUltimoIntento_23DB
        {
            get { return fechaUltimoIntento_; }
            set { fechaUltimoIntento_ = value; }
        }

    }
}
