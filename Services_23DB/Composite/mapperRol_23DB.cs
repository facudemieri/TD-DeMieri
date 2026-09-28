using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL_23DB;

namespace Services_23DB
{
    public class mapperRol_23DB
    {
        Acceso_23DB acc_23DB = new Acceso_23DB();

        private Rol_23DB MapearRol_23DB(DataRow dr_23DB)
        {
            return new Rol_23DB
            {
                IdRol_23DB = (int)dr_23DB["IdRol"],
                NombreRol_23DB = dr_23DB["NombreRol"].ToString()
            };
        }

        public List<Rol_23DB> ObtenerRoles_23DB()
        {
            List<Rol_23DB> lista_23DB = new List<Rol_23DB>();
            DataTable dt_23DB = acc_23DB.Leer_23DB("ObtenerRoles", null);
            foreach(DataRow dr_23DB in dt_23DB.Rows)
            {
                lista_23DB.Add(MapearRol_23DB(dr_23DB));
            }
            return lista_23DB;
        }

        public Rol_23DB ObtenerRol_23DB(int idRol_23DB)
        {
            Rol_23DB rol_23DB = null;
            SqlParameter[] sp_23DB = new SqlParameter[1];
            sp_23DB[0] = new SqlParameter("@IdRol", idRol_23DB);
            DataTable dt_23DB = acc_23DB.Leer_23DB("ObtenerRol", sp_23DB);
            if(dt_23DB.Rows.Count > 0)
            {
                rol_23DB = MapearRol_23DB(dt_23DB.Rows[0]);
            }
            return rol_23DB;
        }

        public void InsertarRol_23DB(string nombreRol_23DB, List<Rol_23DB> componentes_23DB)
        {

            SqlParameter[] spRol_23DB = new SqlParameter[1];
            spRol_23DB[0] = new SqlParameter("@NombreRol", nombreRol_23DB);
            DataTable dtId_23DB = acc_23DB.Leer_23DB("InsertarRol", spRol_23DB);

            if(dtId_23DB.Rows.Count == 0)
            {
                throw new Exception("No se pudo registrar el rol.");
            }

            int nuevoId_23DB = (int)dtId_23DB.Rows[0]["IdRol"];

            AgregarComponentes_23DB(nuevoId_23DB, componentes_23DB);
        }

        public void ModificarRol_23DB(int idRol_23DB, string nombreRol_23DB, List<Rol_23DB> componentes_23DB)
        {
            SqlParameter[] spRol_23DB = new SqlParameter[2];
            spRol_23DB[0] = new SqlParameter("@IdRol", idRol_23DB);
            spRol_23DB[1] = new SqlParameter("@NombreRol", nombreRol_23DB);
            if(acc_23DB.Escribir_23DB("ModificarRol", spRol_23DB) < 0)
            {
                throw new Exception("No se pudo modificar el rol.");
            }

            SqlParameter[] spDelPat_23DB = new SqlParameter[1];
            spDelPat_23DB[0] = new SqlParameter("@IdRol", idRol_23DB);
            if(acc_23DB.Escribir_23DB("EliminarRolPatPorRol", spDelPat_23DB) < 0)
            {
                throw new Exception("No se pudieron quitar las patentes del rol.");
            }

            SqlParameter[] spDelFam_23DB = new SqlParameter[1];
            spDelFam_23DB[0] = new SqlParameter("@IdRol", idRol_23DB);
            if(acc_23DB.Escribir_23DB("EliminarRolFamPorRol", spDelFam_23DB) < 0)
            {
                throw new Exception("No se pudieron quitar las familias del rol.");
            }

            AgregarComponentes_23DB(idRol_23DB, componentes_23DB);
        }

        private void AgregarComponentes_23DB(int idRol_23DB, List<Rol_23DB> componentes_23DB)
        {
            foreach(Rol_23DB componente_23DB in componentes_23DB)
            {
                if(componente_23DB is Patente_23DB)
                {
                    SqlParameter[] spRel_23DB = new SqlParameter[2];
                    spRel_23DB[0] = new SqlParameter("@IdRol", idRol_23DB);
                    spRel_23DB[1] = new SqlParameter("@IdPatente", ((Patente_23DB)componente_23DB).IdPatente_23DB);

                    if(acc_23DB.Escribir_23DB("InsertarRolPat", spRel_23DB) < 0)
                    {
                        throw new Exception($"No se pudo asignar la patente '{componente_23DB.ObtenerNombre_23DB()}' al rol.");
                    }
                }
                else if(componente_23DB is Familia_23DB)
                {
                    SqlParameter[] spRel_23DB = new SqlParameter[2];
                    spRel_23DB[0] = new SqlParameter("@IdRol", idRol_23DB);
                    spRel_23DB[1] = new SqlParameter("@IdFamilia", ((Familia_23DB)componente_23DB).IdFamilia_23DB);
                    if(acc_23DB.Escribir_23DB("InsertarRolFam", spRel_23DB) < 0)
                    {
                        throw new Exception($"No se pudo asignar la familia '{componente_23DB.ObtenerNombre_23DB()}' al rol.");
                    }
                }
            }
        }

        public void EliminarRol_23DB(int idRol_23DB)
        {
            SqlParameter[] spDelPat_23DB = new SqlParameter[1];
            spDelPat_23DB[0] = new SqlParameter("@IdRol", idRol_23DB);
            if(acc_23DB.Escribir_23DB("EliminarRolPatPorRol", spDelPat_23DB) < 0)
            {
                throw new Exception("No se pudieron quitar las patentes del rol.");
            }

            SqlParameter[] spDelFam_23DB = new SqlParameter[1];
            spDelFam_23DB[0] = new SqlParameter("@IdRol", idRol_23DB);
            if(acc_23DB.Escribir_23DB("EliminarRolFamPorRol", spDelFam_23DB) < 0)
            {
                throw new Exception("No se pudieron quitar las familias del rol.");
            }

            SqlParameter[] spRol_23DB = new SqlParameter[1];
            spRol_23DB[0] = new SqlParameter("@IdRol", idRol_23DB);
            if(acc_23DB.Escribir_23DB("EliminarRol", spRol_23DB) < 0)
            {
                throw new Exception("No se pudo eliminar el rol.");
            }
        }

        public Rol_23DB ObtenerRolCompleto_23DB(int idRol_23DB)
        {
            SqlParameter[] spRol_23DB = new SqlParameter[1];
            spRol_23DB[0] = new SqlParameter("@IdRol", idRol_23DB);
            DataTable dtRol_23DB = acc_23DB.Leer_23DB("ObtenerRol", spRol_23DB);

            if(dtRol_23DB.Rows.Count == 0)
            {
                return null;
            }

            Rol_23DB rol_23DB = MapearRol_23DB(dtRol_23DB.Rows[0]);

            SqlParameter[] spPatentes_23DB = new SqlParameter[1];
            spPatentes_23DB[0] = new SqlParameter("@IdRol", idRol_23DB);
            DataTable dtPatentes_23DB = acc_23DB.Leer_23DB("ObtenerPatentesDeRolDirectas", spPatentes_23DB);
            foreach(DataRow dr_23DB in dtPatentes_23DB.Rows)
            {
                rol_23DB.Agregar_23DB(new Patente_23DB
                {
                    IdPatente_23DB = (int)dr_23DB["IdPatente"],
                    NombrePatente_23DB = dr_23DB["NombrePatente"].ToString(),
                    Descripcion_23DB = dr_23DB["Descripcion"].ToString()
                });
            }

            SqlParameter[] spFamilias_23DB = new SqlParameter[1];
            spFamilias_23DB[0] = new SqlParameter("@IdRol", idRol_23DB);
            DataTable dtFamilias_23DB = acc_23DB.Leer_23DB("ObtenerFamiliasDeRol", spFamilias_23DB);
            List<int> idsFamilias_23DB = new List<int>();
            foreach(DataRow dr_23DB in dtFamilias_23DB.Rows)
            {
                idsFamilias_23DB.Add((int)dr_23DB["IdFamilia"]);
            }

    
            mapperFamilia_23DB mapperFam_23DB = new mapperFamilia_23DB();
            foreach(int idFam_23DB in idsFamilias_23DB)
            {
                Familia_23DB familia_23DB = mapperFam_23DB.ObtenerFamiliaCompleta_23DB(idFam_23DB);
                if(familia_23DB != null)
                {
                    rol_23DB.Agregar_23DB(familia_23DB);
                }
            }

            return rol_23DB;
        }

        public bool RolEstaAsignado_23DB(int idRol_23DB)
        {
            SqlParameter[] sp_23DB = new SqlParameter[1];
            sp_23DB[0] = new SqlParameter("@IdRol", idRol_23DB);
            DataTable dt_23DB = acc_23DB.Leer_23DB("RolEstaAsignado", sp_23DB);
            if(dt_23DB.Rows.Count == 0)
            {
                return false;
            }
            return (int)dt_23DB.Rows[0]["Cantidad"] > 0;
        }

        public List<string> ObtenerPatentesDeRol_23DB(int idRol_23DB)
        {
            List<string> patentes_23DB = new List<string>();

            SqlParameter[] spPat_23DB = new SqlParameter[1];
            spPat_23DB[0] = new SqlParameter("@IdRol", idRol_23DB);
            DataTable dtPat_23DB = acc_23DB.Leer_23DB("ObtenerNombresPatentesDeRol", spPat_23DB);
            foreach(DataRow dr_23DB in dtPat_23DB.Rows)
            {
                patentes_23DB.Add(dr_23DB["NombrePatente"].ToString());
            }

            // trae las patentes de las familias que tenga el rol y las patentes de las familias de las familias
            SqlParameter[] spFamPat_23DB = new SqlParameter[1];
            spFamPat_23DB[0] = new SqlParameter("@IdRol", idRol_23DB);
            DataTable dtFamPat_23DB = acc_23DB.Leer_23DB("ObtenerNombresPatentesDeFamiliasDeRol", spFamPat_23DB);
            foreach(DataRow dr_23DB in dtFamPat_23DB.Rows)
            {
                string patente_23DB = dr_23DB["NombrePatente"].ToString();
                if(!patentes_23DB.Contains(patente_23DB))
                {
                    patentes_23DB.Add(patente_23DB);
                }
            }

            return patentes_23DB;
        }
    }
}
