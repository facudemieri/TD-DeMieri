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
    public class mapperAeronave_62FD
    {
        Acceso_23DB acc_62FD = new Acceso_23DB();

        public List<Aeronave_62FD> ObtenerAeronaves_62FD(string matricula_62FD, string modelo_62FD, string fabricante_62FD, string estadoOperativo_62FD)
        {
            List<Aeronave_62FD> lista_62FD = new List<Aeronave_62FD>();

            SqlParameter[] sp_62FD = new SqlParameter[4];
            sp_62FD[0] = new SqlParameter("@Matricula", string.IsNullOrEmpty(matricula_62FD) ? (object)DBNull.Value : matricula_62FD);
            sp_62FD[1] = new SqlParameter("@Modelo", string.IsNullOrEmpty(modelo_62FD) ? (object)DBNull.Value : modelo_62FD);
            sp_62FD[2] = new SqlParameter("@Fabricante", string.IsNullOrEmpty(fabricante_62FD) ? (object)DBNull.Value : fabricante_62FD);
            sp_62FD[3] = new SqlParameter("@EstadoOperativo", string.IsNullOrEmpty(estadoOperativo_62FD) ? (object)DBNull.Value : estadoOperativo_62FD);

            DataTable dt_62FD = acc_62FD.Leer_23DB("ObtenerAeronaves_62FD", sp_62FD);

            foreach (DataRow dr_62FD in dt_62FD.Rows)
            {
                lista_62FD.Add(new Aeronave_62FD
                {
                    Matricula_62FD = dr_62FD["Matricula_62FD"].ToString(),
                    NroSerie_62FD = dr_62FD["NroSerie_62FD"].ToString(),
                    Modelo_62FD = dr_62FD["Modelo_62FD"].ToString(),
                    Fabricante_62FD = dr_62FD["Fabricante_62FD"].ToString(),
                    EstadoOperativo_62FD = dr_62FD["EstadoOperativo_62FD"].ToString(),
                    HorasVueloAcumuladas_62FD = Convert.ToInt32(dr_62FD["HorasVueloAcumuladas_62FD"]),
                    IntervaloRevision_62FD = Convert.ToInt32(dr_62FD["IntervaloRevision_62FD"]),
                    FechaProximaRevision_62FD = Convert.ToDateTime(dr_62FD["FechaProximaRevision_62FD"])
                });
            }

            return lista_62FD;
        }

        public int ActualizarPorCertificacion_62FD(string matricula_62FD, string estadoOperativo_62FD, bool actualizarRevision_62FD, DateTime fechaCertificacion_62FD)
        {
            SqlParameter[] parametros_62FD = new SqlParameter[]
            {
                new SqlParameter("@Matricula", matricula_62FD),
                new SqlParameter("@EstadoOperativo", estadoOperativo_62FD),
                new SqlParameter("@ActualizarRevision", actualizarRevision_62FD),
                new SqlParameter("@fechaCertificacion", fechaCertificacion_62FD)
            };

            return acc_62FD.Escribir_23DB("ActualizarAeronavePorCertificacion_62FD", parametros_62FD);
        }

        public int ActualizarEstadoOperativo_62FD(string matricula_62FD, string estadoOperativo_62FD)
        {
            SqlParameter[] parametros_62FD = new SqlParameter[]
            {
                new SqlParameter("@Matricula", matricula_62FD),
                new SqlParameter("@EstadoOperativo", estadoOperativo_62FD)
            };

            return acc_62FD.Escribir_23DB("ActualizarEstadoOperativoAeronave_62FD", parametros_62FD);
        }

        public List<Aeronave_62FD> ObtenerTodas_62FD()
        {
            DataTable tabla_62FD = acc_62FD.Leer_23DB("ObtenerAeronavesMaestro_62FD", new SqlParameter[] { });

            List<Aeronave_62FD> lista_62FD = new List<Aeronave_62FD>();

            foreach (DataRow fila_62FD in tabla_62FD.Rows)
            {
                lista_62FD.Add(new Aeronave_62FD
                {
                    Matricula_62FD = fila_62FD["Matricula_62FD"].ToString(),
                    NroSerie_62FD = fila_62FD["NroSerie_62FD"].ToString(),
                    Modelo_62FD = fila_62FD["Modelo_62FD"].ToString(),
                    Fabricante_62FD = fila_62FD["Fabricante_62FD"].ToString(),
                    EstadoOperativo_62FD = fila_62FD["EstadoOperativo_62FD"].ToString(),
                    HorasVueloAcumuladas_62FD = Convert.ToInt32(fila_62FD["HorasVueloAcumuladas_62FD"]),
                    IntervaloRevision_62FD = Convert.ToInt32(fila_62FD["IntervaloRevision_62FD"]),
                    FechaProximaRevision_62FD = Convert.ToDateTime(fila_62FD["FechaProximaRevision_62FD"]),
                    Activo_62FD = Convert.ToBoolean(fila_62FD["Activo_62FD"])
                });
            }

            return lista_62FD;
        }

        public int Insertar_62FD(Aeronave_62FD aeronave_62FD)
        {
            return acc_62FD.Escribir_23DB("InsertarAeronave_62FD", ArmarParametros_62FD(aeronave_62FD));
        }

        public int Actualizar_62FD(Aeronave_62FD aeronave_62FD)
        {
            return acc_62FD.Escribir_23DB("ActualizarAeronave_62FD", ArmarParametros_62FD(aeronave_62FD));
        }

        private SqlParameter[] ArmarParametros_62FD(Aeronave_62FD aeronave_62FD)
        {
            return new SqlParameter[]
            {
                new SqlParameter("@Matricula", aeronave_62FD.Matricula_62FD),
                new SqlParameter("@NroSerie", aeronave_62FD.NroSerie_62FD),
                new SqlParameter("@Modelo", aeronave_62FD.Modelo_62FD),
                new SqlParameter("@Fabricante", aeronave_62FD.Fabricante_62FD),
                new SqlParameter("@EstadoOperativo", aeronave_62FD.EstadoOperativo_62FD),
                new SqlParameter("@HorasVueloAcumuladas", aeronave_62FD.HorasVueloAcumuladas_62FD),
                new SqlParameter("@IntervaloRevision", aeronave_62FD.IntervaloRevision_62FD),
                new SqlParameter("@FechaProximaRevision", aeronave_62FD.FechaProximaRevision_62FD)
            };
        }

        public int CambiarEstadoAlta_62FD(string matricula_62FD, bool activo_62FD)
        {
            SqlParameter[] parametros_62FD = new SqlParameter[]
            {
                new SqlParameter("@Matricula", matricula_62FD),
                new SqlParameter("@Activo", activo_62FD)
            };

            return acc_62FD.Escribir_23DB("CambiarEstadoAltaAeronave_62FD", parametros_62FD);
        }
    }
}
