using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services_23DB
{
    public class RolBLL_23DB
    {
        private mapperRol_23DB mapperRol_23DB = new mapperRol_23DB();
        private DVBLL_23DB dvBLL_23DB = new DVBLL_23DB();

        public List<Rol_23DB> ObtenerRoles_23DB()
        {
            return mapperRol_23DB.ObtenerRoles_23DB();
        }

        public Rol_23DB ObtenerRol_23DB(int idRol_23DB)
        {
            return mapperRol_23DB.ObtenerRol_23DB(idRol_23DB);
        }

        public void CrearRol_23DB(string nombreRol_23DB, List<Rol_23DB> componentes_23DB)
        {
            mapperRol_23DB.InsertarRol_23DB(nombreRol_23DB, componentes_23DB);
            dvBLL_23DB.RecalcularDVTabla_23DB("Rol_23DB");
            dvBLL_23DB.RecalcularDVTabla_23DB("RolPat_23DB");
            dvBLL_23DB.RecalcularDVTabla_23DB("RolFam_23DB");
        }

        public bool ValidarNombre_23DB(string nombreRol_23DB, List<Rol_23DB> roles_23DB)
        {
            foreach(Rol_23DB rol_23DB in roles_23DB)
            {
                if(rol_23DB.NombreRol_23DB.ToLower() == nombreRol_23DB.ToLower())
                {
                    return false;
                }
            }
            return true;
        }

        public bool ValidarElementos_23DB(List<Rol_23DB> componentes_23DB)
        {
            return componentes_23DB != null && componentes_23DB.Count > 0;
        }

        public void ModificarRol_23DB(int idRol_23DB, string nombreRol_23DB, List<Rol_23DB> componentes_23DB)
        {
            mapperRol_23DB.ModificarRol_23DB(idRol_23DB, nombreRol_23DB, componentes_23DB);
            dvBLL_23DB.RecalcularDVTabla_23DB("Rol_23DB");
            dvBLL_23DB.RecalcularDVTabla_23DB("RolPat_23DB");
            dvBLL_23DB.RecalcularDVTabla_23DB("RolFam_23DB");
        }

        public void EliminarRol_23DB(int idRol_23DB)
        {
            mapperRol_23DB.EliminarRol_23DB(idRol_23DB);
            dvBLL_23DB.RecalcularDVTabla_23DB("Rol_23DB");
            dvBLL_23DB.RecalcularDVTabla_23DB("RolPat_23DB");
            dvBLL_23DB.RecalcularDVTabla_23DB("RolFam_23DB");
        }

        public Rol_23DB ObtenerRolCompleto_23DB(int idRol_23DB)
        {
            return mapperRol_23DB.ObtenerRolCompleto_23DB(idRol_23DB);
        }

        public List<string> ObtenerPatentesDeRol_23DB(int idRol_23DB)
        {
            return mapperRol_23DB.ObtenerPatentesDeRol_23DB(idRol_23DB);
        }

        public Rol_23DB ObtenerRolPorNombre_23DB(string nombreRol_23DB)
        {
            return mapperRol_23DB.ObtenerRoles_23DB().FirstOrDefault(r => r.NombreRol_23DB == nombreRol_23DB);
        }
        public bool RolEstaAsignado_23DB(int idRol_23DB)
        {
            return mapperRol_23DB.RolEstaAsignado_23DB(idRol_23DB);
        }

        public bool EsPropioRol_23DB(string nombreRol_23DB)
        {
            return nombreRol_23DB == SessionManager_23DB.ObtenerInstancia_23DB().Rol_23DB;
        }
    }
}
