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
    public class CertificacionBLL_62FD
    {
        mapperCertificacion_62FD mapperCertificacion_62FD = new mapperCertificacion_62FD();
        mapperAeronave_62FD mapperAeronave_62FD = new mapperAeronave_62FD();
        mapperOrdenTrabajo_62FD mapperOrdenTrabajo_62FD = new mapperOrdenTrabajo_62FD();
        DVBLL_23DB dvBLL_23DB = new DVBLL_23DB();

        // Códigos de resultado de CertificarIntervencion_62FD
        public const int YaCertificada_62FD = 0;
        public const int DatoModificado_62FD = -1;
        public const int AeronaveNoActualizada_62FD = -2;
        public const int Autocertificacion_62FD = -3;

        public List<Intervencion_62FD> ObtenerPendientes_62FD()
        {
            return mapperCertificacion_62FD.ObtenerPendientes_62FD();
        }

        // Devuelve el nro de intervención si todo salió bien, o uno de los códigos de arriba.
        // Con AeronaveNoActualizada_62FD la certificación SÍ quedó registrada.
        public int CertificarIntervencion_62FD(Certificacion_62FD certificacion_62FD, int idOrden_62FD)
        {
            if (mapperCertificacion_62FD.ExisteCertificacion_62FD(certificacion_62FD.IdIntervencion_62FD))
            {
                return YaCertificada_62FD;
            }

            // Una sola lectura de la orden: sirve para la autocertificación, el tipo de intervención y la matrícula
            OrdenTrabajo_62FD orden_62FD = mapperOrdenTrabajo_62FD.ObtenerOrdenPorId_62FD(idOrden_62FD);

            if (orden_62FD == null)
            {
                return DatoModificado_62FD;
            }

            if (orden_62FD.LegajoTecnico_62FD == SessionManager_23DB.ObtenerInstancia_23DB().DNI_23DB)
            {
                return Autocertificacion_62FD;
            }

            certificacion_62FD.fechaHoraCertificacion_62FD = DateTime.Now;

            if (mapperCertificacion_62FD.InsertarCertificacion_62FD(certificacion_62FD) != 1)
            {
                return DatoModificado_62FD;
            }

            dvBLL_23DB.RecalcularDVTabla_23DB("Certificacion_62FD");

            bool satisfactorio_62FD = certificacion_62FD.resultadoInspeccion_62FD == "Satisfactorio";
            bool actualizarRevision_62FD = satisfactorio_62FD && orden_62FD.TipoIntervencion_62FD == "Preventivo";
            string estadoOperativo_62FD = satisfactorio_62FD ? "En Servicio" : "Fuera de Servicio";

            int filasAeronave_62FD;

            try
            {
                filasAeronave_62FD = mapperAeronave_62FD.ActualizarPorCertificacion_62FD(orden_62FD.Matricula_62FD, estadoOperativo_62FD, actualizarRevision_62FD, certificacion_62FD.fechaHoraCertificacion_62FD);
            }
            catch (Exception)
            {
                // La certificación ya está grabada: se informa como aeronave no actualizada en lugar de como error total
                return AeronaveNoActualizada_62FD;
            }

            if (filasAeronave_62FD != 1)
            {
                return AeronaveNoActualizada_62FD;
            }

            dvBLL_23DB.RecalcularDVTabla_23DB("Aeronave_62FD");
            dvBLL_23DB.RecalcularDVTabla_23DB("Aeronave_62FD_C");
            return certificacion_62FD.IdIntervencion_62FD;
        }
    }
}
