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
    public class mapperFamilia_23DB
    {
        Acceso_23DB acc_23DB = new Acceso_23DB();

        private Familia_23DB MapearFamilia_23DB(DataRow dr_23DB)
        {
            return new Familia_23DB
            {
                IdFamilia_23DB = (int)dr_23DB["IdFamilia"],
                NombreFamilia_23DB = dr_23DB["NombreFamilia"].ToString()
            };
        }

        public List<Familia_23DB> ObtenerFamilias_23DB()
        {
            List<Familia_23DB> lista_23DB = new List<Familia_23DB>();
            DataTable dt_23DB = acc_23DB.Leer_23DB("ObtenerFamilias", null);
            foreach(DataRow dr_23DB in dt_23DB.Rows)
            {
                lista_23DB.Add(MapearFamilia_23DB(dr_23DB));
            }
            return lista_23DB;
        }

        public Familia_23DB ObtenerFamiliaCompleta_23DB(int idFamilia_23DB)
        {
            return ObtenerFamiliaRecursiva_23DB(idFamilia_23DB);
        }

        public Familia_23DB ObtenerFamiliaRecursiva_23DB(int idFamilia_23DB)
        {
            SqlParameter[] spFamilia_23DB = new SqlParameter[1];
            spFamilia_23DB[0] = new SqlParameter("@IdFamilia", idFamilia_23DB);
            DataTable dtFamilia_23DB = acc_23DB.Leer_23DB("ObtenerFamilia", spFamilia_23DB);

            Familia_23DB familia_23DB = null;
            if(dtFamilia_23DB.Rows.Count > 0)
            {
                familia_23DB = MapearFamilia_23DB(dtFamilia_23DB.Rows[0]);
            }

            if(familia_23DB == null) return null;

            SqlParameter[] spPatentes_23DB = new SqlParameter[1];
            spPatentes_23DB[0] = new SqlParameter("@IdFamilia", idFamilia_23DB);
            DataTable dtPatentes_23DB = acc_23DB.Leer_23DB("ObtenerPatentesDeFamilia", spPatentes_23DB);
            foreach(DataRow dr_23DB in dtPatentes_23DB.Rows)
            {
                familia_23DB.Agregar_23DB(new Patente_23DB
                {
                    IdPatente_23DB = (int)dr_23DB["IdPatente"],
                    NombrePatente_23DB = dr_23DB["NombrePatente"].ToString(),
                    Descripcion_23DB = dr_23DB["Descripcion"].ToString()
                });
            }

            // trae las familias de la familia
            SqlParameter[] spFamilias_23DB = new SqlParameter[1];
            spFamilias_23DB[0] = new SqlParameter("@IdFamilia", idFamilia_23DB);
            DataTable dtFamilias_23DB = acc_23DB.Leer_23DB("ObtenerSubFamilias", spFamilias_23DB);
            List<int> idsFamilias_23DB = new List<int>();
            foreach(DataRow dr_23DB in dtFamilias_23DB.Rows)
            {
                idsFamilias_23DB.Add((int)dr_23DB["IdFamiliaHija"]);
            }

            foreach(int idSubFamilia_23DB in idsFamilias_23DB)
            {
                familia_23DB.Agregar_23DB(ObtenerFamiliaRecursiva_23DB(idSubFamilia_23DB));
            }
            return familia_23DB;
        }

        public void InsertarFamilia_23DB(string nombreFamilia_23DB, List<Rol_23DB> componentes_23DB)
        {

            SqlParameter[] spFamilia_23DB = new SqlParameter[1];
            spFamilia_23DB[0] = new SqlParameter("@NombreFamilia", nombreFamilia_23DB);
            DataTable dtId_23DB = acc_23DB.Leer_23DB("InsertarFamilia", spFamilia_23DB);


            if(dtId_23DB.Rows.Count == 0)
            {
                throw new Exception("No se pudo registrar la familia.");
            }

            int nuevoId_23DB = (int)dtId_23DB.Rows[0]["IdFamilia"];

            AgregarComponentes_23DB(nuevoId_23DB, componentes_23DB);
        }

        public void ModificarFamilia_23DB(int idFamilia_23DB, string nombreFamilia_23DB, List<Rol_23DB> componentes_23DB)
        {
            SqlParameter[] spFamilia_23DB = new SqlParameter[2];
            spFamilia_23DB[0] = new SqlParameter("@IdFamilia", idFamilia_23DB);
            spFamilia_23DB[1] = new SqlParameter("@NombreFamilia", nombreFamilia_23DB);
            if(acc_23DB.Escribir_23DB("ModificarFamilia", spFamilia_23DB) < 0)
            {
                throw new Exception("No se pudo modificar la familia.");
            }

            SqlParameter[] spDelPat_23DB = new SqlParameter[1];
            spDelPat_23DB[0] = new SqlParameter("@IdFamilia", idFamilia_23DB);
            if(acc_23DB.Escribir_23DB("EliminarFamPatPorFamilia", spDelPat_23DB) < 0)
            {
                throw new Exception("No se pudieron quitar las patentes de la familia.");
            }

            SqlParameter[] spDelFam_23DB = new SqlParameter[1];
            spDelFam_23DB[0] = new SqlParameter("@IdFamilia", idFamilia_23DB);
            if(acc_23DB.Escribir_23DB("EliminarFamFamPorPadre", spDelFam_23DB) < 0)
            {
                throw new Exception("No se pudieron quitar las subfamilias de la familia.");
            }

            AgregarComponentes_23DB(idFamilia_23DB, componentes_23DB);
        }

        private void AgregarComponentes_23DB(int idFamilia_23DB, List<Rol_23DB> componentes_23DB)
        {
            foreach(Rol_23DB componente_23DB in componentes_23DB)
            {
                if(componente_23DB is Patente_23DB)
                {
                    SqlParameter[] spRel_23DB = new SqlParameter[2];
                    spRel_23DB[0] = new SqlParameter("@IdFamilia", idFamilia_23DB);
                    spRel_23DB[1] = new SqlParameter("@IdPatente", ((Patente_23DB)componente_23DB).IdPatente_23DB);

                    if(acc_23DB.Escribir_23DB("InsertarFamPat", spRel_23DB) < 0)
                    {
                        throw new Exception($"No se pudo asignar la patente '{componente_23DB.ObtenerNombre_23DB()}' a la familia.");
                    }
                }
                else if(componente_23DB is Familia_23DB)
                {
                    SqlParameter[] spRel_23DB = new SqlParameter[2];
                    spRel_23DB[0] = new SqlParameter("@IdFamiliaPadre", idFamilia_23DB);
                    spRel_23DB[1] = new SqlParameter("@IdFamiliaHija", ((Familia_23DB)componente_23DB).IdFamilia_23DB);
                    if(acc_23DB.Escribir_23DB("InsertarFamFam", spRel_23DB) < 0)
                    {
                        throw new Exception($"No se pudo asignar la subfamilia '{componente_23DB.ObtenerNombre_23DB()}' a la familia.");
                    }
                }
            }
        }

        public void EliminarFamilia_23DB(int idFamilia_23DB)
        {
            SqlParameter[] spDelRolFam_23DB = new SqlParameter[1];
            spDelRolFam_23DB[0] = new SqlParameter("@IdFamilia", idFamilia_23DB);
            if(acc_23DB.Escribir_23DB("EliminarRolFamPorFamilia", spDelRolFam_23DB) < 0)
            {
                throw new Exception("No se pudo quitar la familia de los roles que la tenian asignada.");
            }

            SqlParameter[] spDelPat_23DB = new SqlParameter[1];
            spDelPat_23DB[0] = new SqlParameter("@IdFamilia", idFamilia_23DB);
            if(acc_23DB.Escribir_23DB("EliminarFamPatPorFamilia", spDelPat_23DB) < 0)
            {
                throw new Exception("No se pudieron quitar las patentes de la familia.");
            }

            SqlParameter[] spDelFam_23DB = new SqlParameter[1];
            spDelFam_23DB[0] = new SqlParameter("@IdFamilia", idFamilia_23DB);
            if(acc_23DB.Escribir_23DB("EliminarFamFamPorFamilia", spDelFam_23DB) < 0)
            {
                throw new Exception("No se pudieron quitar los vinculos de la familia con otras familias.");
            }

            // Eliminar Familia
            SqlParameter[] spFamilia_23DB = new SqlParameter[1];
            spFamilia_23DB[0] = new SqlParameter("@IdFamilia", idFamilia_23DB);
            if(acc_23DB.Escribir_23DB("EliminarFamilia", spFamilia_23DB) < 0)
            {
                throw new Exception("No se pudo eliminar la familia.");
            }
        }

        public bool FamiliaEstaEnRol_23DB(int idFamilia_23DB)
        {
            SqlParameter[] sp_23DB = new SqlParameter[1];
            sp_23DB[0] = new SqlParameter("@IdFamilia", idFamilia_23DB);
            DataTable dt_23DB = acc_23DB.Leer_23DB("FamiliaEstaEnRol", sp_23DB);
            if(dt_23DB.Rows.Count == 0)
            {
                return false;
            }
            return (int)dt_23DB.Rows[0]["Cantidad"] > 0;
        }

        public bool FamiliaEstaEnFamilia_23DB(int idFamilia_23DB)
        {
            SqlParameter[] sp_23DB = new SqlParameter[1];
            sp_23DB[0] = new SqlParameter("@IdFamilia", idFamilia_23DB);
            DataTable dt_23DB = acc_23DB.Leer_23DB("FamiliaEstaEnFamilia", sp_23DB);
            if(dt_23DB.Rows.Count == 0)
            {
                return false;
            }
            return (int)dt_23DB.Rows[0]["Cantidad"] > 0;
        }
    }
}
