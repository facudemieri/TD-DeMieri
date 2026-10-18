using BE_23DB.PN2;
using DAL_23DB;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mapper_23DB.PN2
{
    public class mapperAeronaveCambio_62FD
    {
        Acceso_23DB acceso_23DB = new Acceso_23DB();

        public List<AeronaveCambio_62FD> ObtenerCambios_62FD(string matricula_62FD, string modelo_62FD, DateTime? fechaDesde_62FD, DateTime? fechaHasta_62FD)
        {
            SqlParameter[] parametros_62FD = new SqlParameter[]
            {
                new SqlParameter("@Matricula", string.IsNullOrEmpty(matricula_62FD) ? (object)DBNull.Value : matricula_62FD),
                new SqlParameter("@Modelo", string.IsNullOrEmpty(modelo_62FD) ? (object)DBNull.Value : modelo_62FD),
                new SqlParameter("@FechaDesde", fechaDesde_62FD.HasValue ? (object)fechaDesde_62FD.Value : DBNull.Value),
                new SqlParameter("@FechaHasta", fechaHasta_62FD.HasValue ? (object)fechaHasta_62FD.Value : DBNull.Value)
            };

            DataTable tabla_62FD = acceso_23DB.Leer_23DB("ObtenerCambiosAeronave_62FD", parametros_62FD);

            List<AeronaveCambio_62FD> lista_62FD = new List<AeronaveCambio_62FD>();

            foreach (DataRow fila_62FD in tabla_62FD.Rows)
            {
                lista_62FD.Add(new AeronaveCambio_62FD
                {
                    Matricula_62FD = fila_62FD["Matricula_62FD"].ToString(),
                    FechaHoraCambio_62FD = Convert.ToDateTime(fila_62FD["FechaHoraCambio_62FD"]),
                    Fecha_62FD = Convert.ToDateTime(fila_62FD["Fecha_62FD"]),
                    Hora_62FD = fila_62FD["Hora_62FD"].ToString(),
                    NroSerie_62FD = fila_62FD["NroSerie_62FD"].ToString(),
                    Modelo_62FD = fila_62FD["Modelo_62FD"].ToString(),
                    Fabricante_62FD = fila_62FD["Fabricante_62FD"].ToString(),
                    EstadoOperativo_62FD = fila_62FD["EstadoOperativo_62FD"].ToString(),
                    HorasVueloAcumuladas_62FD = Convert.ToInt32(fila_62FD["HorasVueloAcumuladas_62FD"]),
                    IntervaloRevision_62FD = Convert.ToInt32(fila_62FD["IntervaloRevision_62FD"]),
                    FechaProximaRevision_62FD = Convert.ToDateTime(fila_62FD["FechaProximaRevision_62FD"]),
                    Activo_62FD = Convert.ToBoolean(fila_62FD["Activo_62FD"]),
                    Act_62FD = Convert.ToBoolean(fila_62FD["Act_62FD"])
                });
            }

            return lista_62FD;
        }

        public List<string> ObtenerMatriculas_62FD()
        {
            return ObtenerColumna_62FD("ObtenerMatriculasCambios_62FD");
        }

        public List<string> ObtenerModelos_62FD()
        {
            return ObtenerColumna_62FD("ObtenerModelosCambios_62FD");
        }

        private List<string> ObtenerColumna_62FD(string nombreSP_62FD)
        {
            DataTable tabla_62FD = acceso_23DB.Leer_23DB(nombreSP_62FD, new SqlParameter[] { });

            List<string> lista_62FD = new List<string>();

            foreach (DataRow fila_62FD in tabla_62FD.Rows)
            {
                lista_62FD.Add(fila_62FD[0].ToString());
            }

            return lista_62FD;
        }

        public int Activar_62FD(string matricula_62FD, DateTime fechaHoraCambio_62FD)
        {
            SqlParameter[] parametros_62FD = new SqlParameter[]
            {
                new SqlParameter("@Matricula", matricula_62FD),
                new SqlParameter("@FechaHoraCambio", fechaHoraCambio_62FD)
            };

            return acceso_23DB.Escribir_23DB("ActivarCambioAeronave_62FD", parametros_62FD);
        }
    }
}
