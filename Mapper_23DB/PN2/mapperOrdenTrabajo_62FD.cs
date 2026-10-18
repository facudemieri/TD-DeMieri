using BE_23DB;
using DAL_23DB;
using Services_23DB;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mapper_23DB
{
    public class mapperOrdenTrabajo_62FD
    {
        Acceso_23DB acc_62FD = new Acceso_23DB();

        public List<Usuario_23DB> ObtenerTecnicos_62FD()
        {
            List<Usuario_23DB> lista_62FD = new List<Usuario_23DB>();
            DataTable dt_62FD = acc_62FD.Leer_23DB("ObtenerTecnicos_62FD", null);

            foreach (DataRow dr_62FD in dt_62FD.Rows)
            {
                lista_62FD.Add(new Usuario_23DB
                {
                    DNI_23DB = dr_62FD["DNI"].ToString(),
                    Apellido_23DB = dr_62FD["Apellido"].ToString(),
                    Nombre_23DB = dr_62FD["Nombre"].ToString()
                });
            }

            return lista_62FD;
        }

        public int ObtenerProximoId_62FD()
        {
            DataTable dt_62FD = acc_62FD.Leer_23DB("ObtenerProximoIdOrden_62FD", null);

            if (dt_62FD.Rows.Count == 0)
            {
                return 1;
            }

            return Convert.ToInt32(dt_62FD.Rows[0]["ProximoId"]);
        }

        public int ContarOrdenesAbiertas_62FD(string matricula_62FD)
        {
            SqlParameter[] sp_62FD = new SqlParameter[1];
            sp_62FD[0] = new SqlParameter("@Matricula", matricula_62FD);

            DataTable dt_62FD = acc_62FD.Leer_23DB("ExisteOrdenAbierta_62FD", sp_62FD);

            if (dt_62FD.Rows.Count == 0)
            {
                return 0;
            }

            return Convert.ToInt32(dt_62FD.Rows[0]["Cantidad"]);
        }

        public int InsertarOrdenTrabajo_62FD(OrdenTrabajo_62FD orden_62FD)
        {
            SqlParameter[] sp_62FD = new SqlParameter[9];
            sp_62FD[0] = new SqlParameter("@IdOrden", orden_62FD.IdOrden_62FD);
            sp_62FD[1] = new SqlParameter("@Matricula", orden_62FD.Matricula_62FD);
            sp_62FD[2] = new SqlParameter("@TipoIntervencion", orden_62FD.TipoIntervencion_62FD);
            sp_62FD[3] = new SqlParameter("@Descripcion", orden_62FD.Descripcion_62FD);
            sp_62FD[4] = new SqlParameter("@EstadoOrden", orden_62FD.EstadoOrden_62FD);
            sp_62FD[5] = new SqlParameter("@FechaInicioPlanificada", orden_62FD.FechaInicioPlanificada_62FD);
            sp_62FD[6] = new SqlParameter("@FechaLimite", orden_62FD.FechaLimite_62FD);
            sp_62FD[7] = new SqlParameter("@Hangar", orden_62FD.Hangar_62FD);
            sp_62FD[8] = new SqlParameter("@LegajoTecnico", orden_62FD.LegajoTecnico_62FD);

            return acc_62FD.Escribir_23DB("InsertarOrdenTrabajo_62FD", sp_62FD);
        }

        public List<OrdenTrabajo_62FD> ObtenerOrdenesPorAeronave_62FD(string matricula_62FD)
        {
            List<OrdenTrabajo_62FD> lista_62FD = new List<OrdenTrabajo_62FD>();

            SqlParameter[] sp_62FD = new SqlParameter[1];
            sp_62FD[0] = new SqlParameter("@Matricula", matricula_62FD);

            DataTable dt_62FD = acc_62FD.Leer_23DB("ObtenerOrdenesPorAeronave_62FD", sp_62FD);

            foreach (DataRow dr_62FD in dt_62FD.Rows)
            {
                lista_62FD.Add(new OrdenTrabajo_62FD
                {
                    IdOrden_62FD = Convert.ToInt32(dr_62FD["IdOrden_62FD"]),
                    Matricula_62FD = matricula_62FD,
                    TipoIntervencion_62FD = dr_62FD["TipoIntervencion_62FD"].ToString(),
                    EstadoOrden_62FD = dr_62FD["EstadoOrden_62FD"].ToString(),
                    FechaInicioPlanificada_62FD = Convert.ToDateTime(dr_62FD["FechaInicioPlanificada_62FD"]),
                    FechaLimite_62FD = Convert.ToDateTime(dr_62FD["FechaLimite_62FD"]),
                    Hangar_62FD = dr_62FD["Hangar_62FD"].ToString(),
                    LegajoTecnico_62FD = dr_62FD["LegajoTecnico_62FD"].ToString()
                });
            }

            return lista_62FD;
        }

        public List<OrdenTrabajo_62FD> ObtenerOrdenesAsignadas_62FD(string legajoTecnico_62FD)
        {
            List<OrdenTrabajo_62FD> lista_62FD = new List<OrdenTrabajo_62FD>();

            SqlParameter[] sp_62FD = new SqlParameter[1];
            sp_62FD[0] = new SqlParameter("@LegajoTecnico", legajoTecnico_62FD);

            DataTable dt_62FD = acc_62FD.Leer_23DB("ObtenerOrdenesAsignadas_62FD", sp_62FD);

            foreach (DataRow dr_62FD in dt_62FD.Rows)
            {
                lista_62FD.Add(new OrdenTrabajo_62FD
                {
                    IdOrden_62FD = Convert.ToInt32(dr_62FD["IdOrden_62FD"]),
                    Matricula_62FD = dr_62FD["Matricula_62FD"].ToString(),
                    TipoIntervencion_62FD = dr_62FD["TipoIntervencion_62FD"].ToString(),
                    FechaLimite_62FD = Convert.ToDateTime(dr_62FD["FechaLimite_62FD"]),
                    Hangar_62FD = dr_62FD["Hangar_62FD"].ToString(),
                    LegajoTecnico_62FD = legajoTecnico_62FD,
                    EstadoOrden_62FD = dr_62FD["EstadoOrden_62FD"].ToString(),
                    FechaInicioPlanificada_62FD = Convert.ToDateTime(dr_62FD["FechaInicioPlanificada_62FD"])
                });
            }

            return lista_62FD;
        }

        public OrdenTrabajo_62FD ObtenerOrdenPorId_62FD(int idOrden_62FD)
        {
            OrdenTrabajo_62FD orden_62FD = null;

            SqlParameter[] sp_62FD = new SqlParameter[1];
            sp_62FD[0] = new SqlParameter("@IdOrden", idOrden_62FD);

            DataTable dt_62FD = acc_62FD.Leer_23DB("ObtenerOrdenPorId_62FD", sp_62FD);

            if (dt_62FD.Rows.Count > 0)
            {
                DataRow dr_62FD = dt_62FD.Rows[0];
                orden_62FD = new OrdenTrabajo_62FD
                {
                    IdOrden_62FD = Convert.ToInt32(dr_62FD["IdOrden_62FD"]),
                    Matricula_62FD = dr_62FD["Matricula_62FD"].ToString(),
                    TipoIntervencion_62FD = dr_62FD["TipoIntervencion_62FD"].ToString(),
                    Descripcion_62FD = dr_62FD["Descripcion_62FD"].ToString(),
                    EstadoOrden_62FD = dr_62FD["EstadoOrden_62FD"].ToString(),
                    FechaInicioPlanificada_62FD = Convert.ToDateTime(dr_62FD["FechaInicioPlanificada_62FD"]),
                    FechaLimite_62FD = Convert.ToDateTime(dr_62FD["FechaLimite_62FD"]),
                    Hangar_62FD = dr_62FD["Hangar_62FD"].ToString(),
                    LegajoTecnico_62FD = dr_62FD["LegajoTecnico_62FD"].ToString()
                };
            }

            return orden_62FD;
        }

        public int ActualizarEstadoOrden_62FD(int idOrden_62FD, string estadoOrden_62FD)
        {
            SqlParameter[] sp_62FD = new SqlParameter[2];
            sp_62FD[0] = new SqlParameter("@IdOrden", idOrden_62FD);
            sp_62FD[1] = new SqlParameter("@EstadoOrden", estadoOrden_62FD);

            return acc_62FD.Escribir_23DB("ActualizarEstadoOrden_62FD", sp_62FD);
        }
    }
}
