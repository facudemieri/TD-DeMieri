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
    public class mapperRepuesto_62FD
    {
        Acceso_23DB acceso_23DB = new Acceso_23DB();

        public int ObtenerProximoNroSolicitud_62FD()
        {
            DataTable tabla_62FD = acceso_23DB.Leer_23DB("ObtenerProximoNroSolicitud_62FD", new SqlParameter[] { });

            if (tabla_62FD.Rows.Count == 0)
            {
                return 1;
            }

            return Convert.ToInt32(tabla_62FD.Rows[0][0]);
        }

        public int InsertarRepuesto_62FD(Repuesto_62FD repuesto_62FD)
        {
            SqlParameter[] parametros_62FD = new SqlParameter[]
            {
                new SqlParameter("@nroSolicitud", repuesto_62FD.nroSolicitud_62FD),
                new SqlParameter("@IdTarea", repuesto_62FD.IdTarea_62FD),
                new SqlParameter("@nroParte", repuesto_62FD.nroParte_62FD),
                new SqlParameter("@nombreRepuesto", repuesto_62FD.nombreRepuesto_62FD),
                new SqlParameter("@cantidadSolicitada", repuesto_62FD.cantidadSolicitada_62FD),
                new SqlParameter("@estadoPedido", repuesto_62FD.estadoPedido_62FD),
                new SqlParameter("@fechaSolicitud", repuesto_62FD.fechaSolicitud_62FD)
            };

            return acceso_23DB.Escribir_23DB("InsertarRepuesto_62FD", parametros_62FD);
        }

        public List<Repuesto_62FD> ObtenerRepuestosPorTarea_62FD(int idTarea_62FD)
        {
            SqlParameter[] parametros_62FD = new SqlParameter[]
            {
                new SqlParameter("@IdTarea", idTarea_62FD)
            };

            DataTable tabla_62FD = acceso_23DB.Leer_23DB("ObtenerRepuestosPorTarea_62FD", parametros_62FD);

            List<Repuesto_62FD> lista_62FD = new List<Repuesto_62FD>();

            foreach (DataRow fila_62FD in tabla_62FD.Rows)
            {
                Repuesto_62FD repuesto_62FD = new Repuesto_62FD();
                repuesto_62FD.nroSolicitud_62FD = Convert.ToInt32(fila_62FD["nroSolicitud_62FD"]);
                repuesto_62FD.IdTarea_62FD = Convert.ToInt32(fila_62FD["IdTarea_62FD"]);
                repuesto_62FD.nroParte_62FD = fila_62FD["nroParte_62FD"].ToString();
                repuesto_62FD.nombreRepuesto_62FD = fila_62FD["nombreRepuesto_62FD"].ToString();
                repuesto_62FD.cantidadSolicitada_62FD = Convert.ToInt32(fila_62FD["cantidadSolicitada_62FD"]);
                repuesto_62FD.estadoPedido_62FD = fila_62FD["estadoPedido_62FD"].ToString();
                repuesto_62FD.fechaSolicitud_62FD = Convert.ToDateTime(fila_62FD["fechaSolicitud_62FD"]);

                if (fila_62FD["fechaRecepcion_62FD"] != DBNull.Value)
                {
                    repuesto_62FD.fechaRecepcion_62FD = Convert.ToDateTime(fila_62FD["fechaRecepcion_62FD"]);
                }
                else
                {
                    repuesto_62FD.fechaRecepcion_62FD = null;
                }

                lista_62FD.Add(repuesto_62FD);
            }

            return lista_62FD;
        }

        public int RegistrarRecepcion_62FD(int nroSolicitud_62FD, DateTime fechaRecepcion_62FD)
        {
            SqlParameter[] parametros_62FD = new SqlParameter[]
            {
                new SqlParameter("@nroSolicitud", nroSolicitud_62FD),
                new SqlParameter("@fechaRecepcion", fechaRecepcion_62FD)
            };

            return acceso_23DB.Escribir_23DB("ActualizarRecepcionRepuesto_62FD", parametros_62FD);
        }

        public List<Repuesto_62FD> ObtenerNombres_62FD()
        {
            DataTable tabla_62FD = acceso_23DB.Leer_23DB("ObtenerNombresRepuesto_62FD", new SqlParameter[] { });

            List<Repuesto_62FD> lista_62FD = new List<Repuesto_62FD>();

            foreach (DataRow fila_62FD in tabla_62FD.Rows)
            {
                Repuesto_62FD repuesto_62FD = new Repuesto_62FD();
                repuesto_62FD.nombreRepuesto_62FD = fila_62FD["nombreRepuesto_62FD"].ToString();
                repuesto_62FD.nroParte_62FD = fila_62FD["nroParte_62FD"].ToString();
                lista_62FD.Add(repuesto_62FD);
            }

            return lista_62FD;
        }
    }
}
