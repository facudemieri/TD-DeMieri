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
    public class mapperUsuario_23DB
    {
        Acceso_23DB acc_23DB = new Acceso_23DB();

        private Usuario_23DB MapearUsuario_23DB(DataRow dr_23DB)
        {
            return new Usuario_23DB
            {
                DNI_23DB = dr_23DB["DNI"].ToString(),
                Apellido_23DB = dr_23DB["Apellido"].ToString(),
                Nombre_23DB = dr_23DB["Nombre"].ToString(),
                Email_23DB = dr_23DB["Email"].ToString(),
                Login_23DB = dr_23DB["Login"].ToString(),
                IdRol_23DB = (int)dr_23DB["IdRol"],
                Bloqueado_23DB = (bool)dr_23DB["Bloqueado"],
                Activo_23DB = (bool)dr_23DB["Activo"],
                IntentosFallidos_23DB = dr_23DB["IntentosFallidos"] == DBNull.Value ? 0 : (int)dr_23DB["IntentosFallidos"],
                FechaUltimoIntento_23DB = dr_23DB["FechaUltimoIntento"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr_23DB["FechaUltimoIntento"]),
                UltimoIdioma_23DB = dr_23DB["UltimoIdioma"] == DBNull.Value ? string.Empty : dr_23DB["UltimoIdioma"].ToString()
            };
        }

        public List<Usuario_23DB> ObtenerTodos_23DB(string filtro_23DB)
        {
            List<Usuario_23DB> lista_23DB = new List<Usuario_23DB>();
            SqlParameter[] sp_23DB = new SqlParameter[1];
            sp_23DB[0] = new SqlParameter("@Filtro", (object)filtro_23DB ?? DBNull.Value);
            DataTable dt_23DB = acc_23DB.Leer_23DB("ObtenerTodos", sp_23DB);
            foreach(DataRow dr_23DB in dt_23DB.Rows)
            {
                lista_23DB.Add(MapearUsuario_23DB(dr_23DB));
            }
            return lista_23DB;
        }

        public int Insertar_23DB(Usuario_23DB usuario_23DB)
        {
            int fa_23DB = 0;
            SqlParameter[] sp_23DB = new SqlParameter[9];
            sp_23DB[0] = new SqlParameter("@DNI", usuario_23DB.DNI_23DB);
            sp_23DB[1] = new SqlParameter("@Apellido", usuario_23DB.Apellido_23DB);
            sp_23DB[2] = new SqlParameter("@Nombre", usuario_23DB.Nombre_23DB);
            sp_23DB[3] = new SqlParameter("@Email", usuario_23DB.Email_23DB);
            sp_23DB[4] = new SqlParameter("@Login", usuario_23DB.Login_23DB);
            sp_23DB[5] = new SqlParameter("@Password", usuario_23DB.Password_23DB);
            sp_23DB[6] = new SqlParameter("@IdRol", usuario_23DB.IdRol_23DB);
            sp_23DB[7] = new SqlParameter("@Bloqueado", usuario_23DB.Bloqueado_23DB);
            sp_23DB[8] = new SqlParameter("@Activo", usuario_23DB.Activo_23DB);
            fa_23DB = acc_23DB.Escribir_23DB("Insertar", sp_23DB);
            return fa_23DB;
        }

        public int Modificar_23DB(Usuario_23DB usuario_23DB)
        {
            int fa_23DB = 0;
            SqlParameter[] sp_23DB = new SqlParameter[3];
            sp_23DB[0] = new SqlParameter("@DNI", usuario_23DB.DNI_23DB);
            sp_23DB[1] = new SqlParameter("@Email", usuario_23DB.Email_23DB);
            sp_23DB[2] = new SqlParameter("@IdRol", usuario_23DB.IdRol_23DB);
            fa_23DB = acc_23DB.Escribir_23DB("Modificar", sp_23DB);
            return fa_23DB;
        }

        public int CambiarEstado_23DB(string dni_23DB, bool activo_23DB)
        {
            int fa_23DB = 0;
            SqlParameter[] sp_23DB = new SqlParameter[2];
            sp_23DB[0] = new SqlParameter("@DNI", dni_23DB);
            sp_23DB[1] = new SqlParameter("@Activo", activo_23DB);
            fa_23DB = acc_23DB.Escribir_23DB("CambiarEstado", sp_23DB);
            return fa_23DB;
        }

        public int Desbloquear_23DB(string dni_23DB, string passwordInicial_23DB)
        {
            int fa_23DB = 0;
            SqlParameter[] sp_23DB = new SqlParameter[2];
            sp_23DB[0] = new SqlParameter("@DNI", dni_23DB);
            sp_23DB[1] = new SqlParameter("@Password", passwordInicial_23DB);
            fa_23DB = acc_23DB.Escribir_23DB("Desbloquear", sp_23DB);
            return fa_23DB;
        }

        public int ActualizarPassword_23DB(string dni_23DB, string passwordEncriptado_23DB)
        {
            int fa_23DB = 0;
            SqlParameter[] sp_23DB = new SqlParameter[2];
            sp_23DB[0] = new SqlParameter("@DNI", dni_23DB);
            sp_23DB[1] = new SqlParameter("@Password", passwordEncriptado_23DB);
            fa_23DB = acc_23DB.Escribir_23DB("ActualizarPassword", sp_23DB);
            return fa_23DB;
        }

        public Usuario_23DB ObtenerUsuario_23DB(string login_23DB, string password_23DB) // valido credenciales
        {
            Usuario_23DB usuario_23DB = null;
            SqlParameter[] sp_23DB = new SqlParameter[2];
            sp_23DB[0] = new SqlParameter("@Login", login_23DB);
            sp_23DB[1] = new SqlParameter("@Password", password_23DB);
            DataTable dt_23DB = acc_23DB.Leer_23DB("ObtenerUsuario", sp_23DB);
            foreach(DataRow dr_23DB in dt_23DB.Rows)
            {
                usuario_23DB = MapearUsuario_23DB(dr_23DB);
            }
            return usuario_23DB;
        }

        public Usuario_23DB ObtenerUsuarioPorLogin_23DB(string login_23DB) // traigo el dni del usuario antes de validar el password
        {
            Usuario_23DB usuario_23DB = null;
            SqlParameter[] sp_23DB = new SqlParameter[1];
            sp_23DB[0] = new SqlParameter("@Login", login_23DB);
            DataTable dt_23DB = acc_23DB.Leer_23DB("ObtenerUsuarioPorLogin", sp_23DB);
            foreach(DataRow dr_23DB in dt_23DB.Rows)
            {
                usuario_23DB = MapearUsuario_23DB(dr_23DB);
            }
            return usuario_23DB;
        }

        public Usuario_23DB ObtenerUsuarioPorDNI_23DB(string dni_23DB, string password_23DB) // para cambiar clave
        {
            Usuario_23DB usuario_23DB = null;
            SqlParameter[] sp_23DB = new SqlParameter[2];
            sp_23DB[0] = new SqlParameter("@DNI", dni_23DB);
            sp_23DB[1] = new SqlParameter("@Password", password_23DB);
            DataTable dt_23DB = acc_23DB.Leer_23DB("ObtenerUsuarioPorDNI", sp_23DB);
            if(dt_23DB.Rows.Count > 0)
            {
                usuario_23DB = MapearUsuario_23DB(dt_23DB.Rows[0]);
            }
            return usuario_23DB;
        }

        public Usuario_23DB ObtenerUsuarioDNI_23DB(string dni_23DB)
        {
            Usuario_23DB usuario_23DB = null;
            SqlParameter[] sp_23DB = new SqlParameter[1];
            sp_23DB[0] = new SqlParameter("@DNI", dni_23DB);
            DataTable dt_23DB = acc_23DB.Leer_23DB("ObtenerUsuarioDNI", sp_23DB);
            if(dt_23DB.Rows.Count > 0)
            {
                usuario_23DB = MapearUsuario_23DB(dt_23DB.Rows[0]);
            }
            return usuario_23DB;
        }

        public int BloquearUsuario_23DB(string dni)
        {
            int fa_23DB = 0;
            SqlParameter[] sp_23DB = new SqlParameter[1];
            sp_23DB[0] = new SqlParameter("@DNI", dni);
            fa_23DB = acc_23DB.Escribir_23DB("BloquearUsuario", sp_23DB);
            return fa_23DB;
        }

        public int IncrementarIntentos_23DB(string dni_23DB)
        {
            int fa_23DB = 0;
            SqlParameter[] sp_23DB = new SqlParameter[2];
            sp_23DB[0] = new SqlParameter("@DNI", dni_23DB);
            sp_23DB[1] = new SqlParameter("@Fecha", DateTime.Now);
            fa_23DB = acc_23DB.Escribir_23DB("IncrementarIntentos", sp_23DB);
            return fa_23DB;
        }

        public int ResetearIntentos_23DB(string dni_23DB)
        {
            int fa_23DB = 0;
            SqlParameter[] sp_23DB = new SqlParameter[1];
            sp_23DB[0] = new SqlParameter("@DNI", dni_23DB);
            fa_23DB = acc_23DB.Escribir_23DB("ResetearIntentos", sp_23DB);
            return fa_23DB;
        }

        public int ActualizarUltimoIdioma_23DB(string dni_23DB, string idioma_23DB)
        {
            int fa_23DB = 0;
            SqlParameter[] sp_23DB = new SqlParameter[2];
            sp_23DB[0] = new SqlParameter("@DNI", dni_23DB);
            sp_23DB[1] = new SqlParameter("@Idioma", (object)idioma_23DB ?? DBNull.Value);
            fa_23DB = acc_23DB.Escribir_23DB("ActualizarUltimoIdioma", sp_23DB);
            return fa_23DB;
        }
    }
}
