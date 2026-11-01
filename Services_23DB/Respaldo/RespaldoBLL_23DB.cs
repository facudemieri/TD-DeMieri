using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services_23DB
{
    public class RespaldoBLL_23DB
    {
        private mapperRespaldo_23DB mapperRespaldo_23DB = new mapperRespaldo_23DB();
        private DVBLL_23DB dvBLL_23DB = new DVBLL_23DB();
        private EventoBLL_23DB eventoBLL_23DB = new EventoBLL_23DB();

        // Detalle del último registro en bitácora que falló: lo necesita el aviso de la pantalla
        public string ErrorBitacora_62FD { get; private set; }

        public string GenerarBackup_23DB(string rutaDestino_23DB, out bool bitacoraRegistrada_62FD)
        {
            string nombreArchivo_23DB = $"INGSW_23DB_{DateTime.Now:yyyyMMdd_HHmmss}.bak";
            string rutaCompleta_23DB = Path.Combine(rutaDestino_23DB, nombreArchivo_23DB);
            mapperRespaldo_23DB.GenerarBackup_23DB(rutaCompleta_23DB);

            bitacoraRegistrada_62FD = RegistrarEventoBitacora_62FD(SessionManager_23DB.ObtenerInstancia_23DB().DNI_23DB, "Backup BD", 2);

            return rutaCompleta_23DB;
        }

        // El DNI se recibe porque desde la pantalla de reparación todavía no hay sesión
        public void RestaurarBackup_23DB(string rutaArchivo_23DB, string dniOperador_62FD, out bool bitacoraRegistrada_62FD)
        {
            if (!string.IsNullOrEmpty(dniOperador_62FD))
            {
                eventoBLL_23DB.RegistrarEvento_23DB(dniOperador_62FD, "Seguridad", "Restore BD", 3);
            }

            mapperRespaldo_23DB.RestaurarBackup_23DB(rutaArchivo_23DB);

            // La restauracion reemplaza la base completa, incluida la tabla DV_23DB.
            // El .bak puede venir de otra instalacion o de un estado anterior, asi
            // que los DV que trae no tienen por que coincidir con los datos
            // restaurados. Se recalculan todos aca; sin esto la inconsistencia
            // recien aparece en el proximo login.
            dvBLL_23DB.RecalcularDV_23DB();

            bitacoraRegistrada_62FD = RegistrarEventoBitacora_62FD(dniOperador_62FD, "Restore BD", 1);
        }

        private bool RegistrarEventoBitacora_62FD(string dniOperador_62FD, string evento_62FD, int criticidad_62FD)
        {
            // Sin DNI no se intenta el insert: la clave foránea de Eventos_23DB hacia Usuario_23DB lo rechazaría
            if(string.IsNullOrEmpty(dniOperador_62FD))
            {
                ErrorBitacora_62FD = "No se pudo identificar al usuario que realizó la operación.";
                return false;
            }

            try
            {
                eventoBLL_23DB.RegistrarEvento_23DB(dniOperador_62FD, "Administrador", evento_62FD, criticidad_62FD);
                ErrorBitacora_62FD = null;
                return true;
            }
            catch(Exception ex_62FD)
            {
                ErrorBitacora_62FD = ex_62FD.Message;
                return false;
            }
        }
    }
}
