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

        public string GenerarBackup_23DB(string rutaDestino_23DB)
        {
            string nombreArchivo_23DB = $"INGSW_23DB_{DateTime.Now:yyyyMMdd_HHmmss}.bak";
            string rutaCompleta_23DB = Path.Combine(rutaDestino_23DB, nombreArchivo_23DB);
            mapperRespaldo_23DB.GenerarBackup_23DB(rutaCompleta_23DB);
            return rutaCompleta_23DB;
        }

        public void RestaurarBackup_23DB(string rutaArchivo_23DB)
        {
            mapperRespaldo_23DB.RestaurarBackup_23DB(rutaArchivo_23DB);

            // La restauracion reemplaza la base completa, incluida la tabla DV_23DB.
            // El .bak puede venir de otra instalacion o de un estado anterior, asi
            // que los DV que trae no tienen por que coincidir con los datos
            // restaurados. Se recalculan todos aca; sin esto la inconsistencia
            // recien aparece en el proximo login.
            dvBLL_23DB.RecalcularDV_23DB();
        }
    }
}
