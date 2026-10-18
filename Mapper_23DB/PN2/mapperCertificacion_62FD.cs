using BE_23DB;
using DAL_23DB;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Mapper_23DB
{
    public class mapperCertificacion_62FD
    {
        Acceso_23DB acceso_23DB = new Acceso_23DB();

        public List<Intervencion_62FD> ObtenerPendientes_62FD()
        {
            DataTable tabla_62FD = acceso_23DB.Leer_23DB("ObtenerIntervencionesPendientesCertificacion_62FD", new SqlParameter[] { });

            List<Intervencion_62FD> lista_62FD = new List<Intervencion_62FD>();

            foreach (DataRow fila_62FD in tabla_62FD.Rows)
            {
                Intervencion_62FD intervencion_62FD = new Intervencion_62FD();
                intervencion_62FD.IdIntervencion_62FD = Convert.ToInt32(fila_62FD["IdIntervencion_62FD"]);
                intervencion_62FD.IdOrden_62FD = Convert.ToInt32(fila_62FD["IdOrden_62FD"]);
                intervencion_62FD.Matricula_62FD = fila_62FD["Matricula_62FD"].ToString();
                intervencion_62FD.fechaHoraInicio_62FD = Convert.ToDateTime(fila_62FD["fechaHoraInicio_62FD"]);
                intervencion_62FD.estadoIntervencion_62FD = fila_62FD["estadoIntervencion_62FD"].ToString();
                intervencion_62FD.totalHorasInsumidas_62FD = Convert.ToDecimal(fila_62FD["totalHorasInsumidas_62FD"]);

                if (fila_62FD["fechaHoraFin_62FD"] != DBNull.Value)
                {
                    intervencion_62FD.fechaHoraFin_62FD = Convert.ToDateTime(fila_62FD["fechaHoraFin_62FD"]);
                }
                else
                {
                    intervencion_62FD.fechaHoraFin_62FD = null;
                }

                lista_62FD.Add(intervencion_62FD);
            }

            return lista_62FD;
        }

        public bool ExisteCertificacion_62FD(int idIntervencion_62FD)
        {
            SqlParameter[] parametros_62FD = new SqlParameter[]
            {
                new SqlParameter("@IdIntervencion", idIntervencion_62FD)
            };

            DataTable tabla_62FD = acceso_23DB.Leer_23DB("ExisteCertificacion_62FD", parametros_62FD);

            if (tabla_62FD.Rows.Count == 0)
            {
                return false;
            }

            return Convert.ToInt32(tabla_62FD.Rows[0][0]) > 0;
        }

        public int InsertarCertificacion_62FD(Certificacion_62FD certificacion_62FD)
        {
            SqlParameter[] parametros_62FD = new SqlParameter[]
            {
                new SqlParameter("@IdIntervencion", certificacion_62FD.IdIntervencion_62FD),
                new SqlParameter("@codHabilitante", certificacion_62FD.codHabilitante_62FD),
                new SqlParameter("@fechaHoraCertificacion", certificacion_62FD.fechaHoraCertificacion_62FD),
                new SqlParameter("@resultadoInspeccion", certificacion_62FD.resultadoInspeccion_62FD),
                new SqlParameter("@Observaciones", certificacion_62FD.Observaciones_62FD)
            };

            return acceso_23DB.Escribir_23DB("InsertarCertificacion_62FD", parametros_62FD);
        }
    }
}
