using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services_23DB
{
    public class FamiliaBLL_23DB
    {
        private mapperFamilia_23DB mapperFamilia_23DB = new mapperFamilia_23DB();
        private DVBLL_23DB dvBLL_23DB = new DVBLL_23DB();
        private EventoBLL_23DB eventoBLL_23DB = new EventoBLL_23DB();

        // Detalle del último registro en bitácora que falló: lo necesita el aviso de la pantalla
        public string ErrorBitacora_62FD { get; private set; }

        public List<Familia_23DB> ObtenerFamilias_23DB()
        {
            return mapperFamilia_23DB.ObtenerFamilias_23DB();
        }

        public void CrearFamilia_23DB(string nombreFamilia_23DB, List<Rol_23DB> componentes_23DB, out bool bitacoraRegistrada_62FD)
        {
            mapperFamilia_23DB.InsertarFamilia_23DB(nombreFamilia_23DB, componentes_23DB);
            dvBLL_23DB.RecalcularDVTabla_23DB("Familia_23DB");
            dvBLL_23DB.RecalcularDVTabla_23DB("FamPat_23DB");
            dvBLL_23DB.RecalcularDVTabla_23DB("FamFam_23DB");

            bitacoraRegistrada_62FD = RegistrarEventoBitacora_62FD("Crear Familia", 2);
        }

        public bool ValidarNombre_23DB(string nombreFamilia_23DB, List<Familia_23DB> familias_23DB)
        {
            foreach(Familia_23DB familia_23DB in familias_23DB)
            {
                if(familia_23DB.NombreFamilia_23DB.ToLower() == nombreFamilia_23DB.ToLower())
                {
                    return false;
                }
            }
            return true;
        }

        public bool TienePatentesRepetidas_23DB(Rol_23DB componente_23DB, List<string> patentesExistentes_23DB)
        {
            foreach(Rol_23DB hijo_23DB in componente_23DB.ObtenerHijos_23DB())
            {
                if(hijo_23DB is Patente_23DB)
                {
                    if(patentesExistentes_23DB.Contains(hijo_23DB.ObtenerNombre_23DB())) return true;
                }
                else
                {
                    if(TienePatentesRepetidas_23DB(hijo_23DB, patentesExistentes_23DB)) return true;
                }
            }
            return false;
        }

        public bool TieneComponentes_23DB(int idFamilia_23DB)
        {
            Familia_23DB familia_23DB = ObtenerFamiliaCompleta_23DB(idFamilia_23DB);
            return familia_23DB.ObtenerHijos_23DB().Count > 0;
        }

        public bool ValidarElementos_23DB(List<Rol_23DB> componentes_23DB)
        {
            return componentes_23DB != null && componentes_23DB.Count > 0;
        }

        public void ModificarFamilia_23DB(int idFamilia_23DB, string nombreFamilia_23DB, List<Rol_23DB> componentes_23DB, out bool bitacoraRegistrada_62FD)
        {
            mapperFamilia_23DB.ModificarFamilia_23DB(idFamilia_23DB, nombreFamilia_23DB, componentes_23DB);
            dvBLL_23DB.RecalcularDVTabla_23DB("Familia_23DB");
            dvBLL_23DB.RecalcularDVTabla_23DB("FamPat_23DB");
            dvBLL_23DB.RecalcularDVTabla_23DB("FamFam_23DB");

            bitacoraRegistrada_62FD = RegistrarEventoBitacora_62FD("Modificar Familia", 2);
        }

        public void EliminarFamilia_23DB(int idFamilia_23DB, out bool bitacoraRegistrada_62FD)
        {
            mapperFamilia_23DB.EliminarFamilia_23DB(idFamilia_23DB);
            dvBLL_23DB.RecalcularDVTabla_23DB("Familia_23DB");
            dvBLL_23DB.RecalcularDVTabla_23DB("FamPat_23DB");
            dvBLL_23DB.RecalcularDVTabla_23DB("FamFam_23DB");
            // El mapper tambien borra de RolFam_23DB (saca la familia de los roles
            // que la tenian asignada), asi que su DV hay que recalcularlo aca.
            dvBLL_23DB.RecalcularDVTabla_23DB("RolFam_23DB");

            bitacoraRegistrada_62FD = RegistrarEventoBitacora_62FD("Eliminar Familia", 2);
        }

        private bool RegistrarEventoBitacora_62FD(string evento_62FD, int criticidad_62FD)
        {
            try
            {
                eventoBLL_23DB.RegistrarEvento_23DB(SessionManager_23DB.ObtenerInstancia_23DB().DNI_23DB, "Administrador", evento_62FD, criticidad_62FD);
                ErrorBitacora_62FD = null;
                return true;
            }
            catch (Exception ex_62FD)
            {
                ErrorBitacora_62FD = ex_62FD.Message;
                return false;
            }
        }

        public Familia_23DB ObtenerFamiliaCompleta_23DB(int idFamilia_23DB)
        {
            return mapperFamilia_23DB.ObtenerFamiliaCompleta_23DB(idFamilia_23DB);
        }

        public bool FamiliaEstaEnRol_23DB(int idFamilia_23DB)
        {
            return mapperFamilia_23DB.FamiliaEstaEnRol_23DB(idFamilia_23DB);
        }

        public bool FamiliaEstaEnFamilia_23DB(int idFamilia_23DB)
        {
            return mapperFamilia_23DB.FamiliaEstaEnFamilia_23DB(idFamilia_23DB);
        }
    }
}
