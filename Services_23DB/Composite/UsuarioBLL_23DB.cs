using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services_23DB
{
    public class UsuarioBLL_23DB
    {
        private mapperUsuario_23DB mapperUsuario_23DB = new mapperUsuario_23DB();
        private RolBLL_23DB rolBLL_23DB = new RolBLL_23DB();
        private DVBLL_23DB dvBLL_23DB = new DVBLL_23DB();

        public Usuario_23DB AutenticarUsuario_23DB(string login_23DB, string password_23DB)
        {
            string passwordEncriptado_23DB = CryptoManager_23DB.EncriptarHash_23DB(password_23DB);
            return mapperUsuario_23DB.ObtenerUsuario_23DB(login_23DB, passwordEncriptado_23DB);
        }

        public void BloquearUsuario_23DB(string dni_23DB)
        {
            int fa_23DB = mapperUsuario_23DB.BloquearUsuario_23DB(dni_23DB);
            if(fa_23DB < 0) throw new Exception("No se pudo bloquear el usuario.");
            dvBLL_23DB.RecalcularDVTabla_23DB("Usuario_23DB");
        }

        public string ObtenerNombreRol_23DB(int idRol_23DB)
        {
            Rol_23DB rol_23DB = rolBLL_23DB.ObtenerRol_23DB(idRol_23DB);
            return rol_23DB != null ? rol_23DB.NombreRol_23DB : string.Empty;
        }
        public Usuario_23DB ObtenerUsuarioPorLogin_23DB(string login_23DB)
        {
            return mapperUsuario_23DB.ObtenerUsuarioPorLogin_23DB(login_23DB);
        }
        public string GenerarLogin_23DB(string nombre_23DB, string apellido_23DB)
        {
            return CryptoManager_23DB.GenerarLogin_23DB(nombre_23DB, apellido_23DB);
        }

        public string GenerarPasswordInicial_23DB(string dni_23DB, string apellido_23DB)
        {
            return CryptoManager_23DB.GenerarPassword_23DB(dni_23DB, apellido_23DB);
        }

        public List<Usuario_23DB> ObtenerTodos_23DB(string filtro_23DB)
        {
            return mapperUsuario_23DB.ObtenerTodos_23DB(filtro_23DB);
        }

        public void CrearUsuario_23DB(Usuario_23DB usuario_23DB)
        {
            if(mapperUsuario_23DB.ObtenerUsuarioDNI_23DB(usuario_23DB.DNI_23DB) != null)
            {
                throw new Exception("Ya existe un usuario con ese DNI.");
            }

            string loginGenerado_23DB = GenerarLogin_23DB(usuario_23DB.Nombre_23DB, usuario_23DB.Apellido_23DB);

            if(mapperUsuario_23DB.ObtenerUsuarioPorLogin_23DB(loginGenerado_23DB) != null)
            {
                throw new Exception("Ya existe un usuario con ese nombre y apellido.");
            }
            usuario_23DB.Login_23DB = loginGenerado_23DB;
            usuario_23DB.Password_23DB = GenerarPasswordInicial_23DB(usuario_23DB.DNI_23DB, usuario_23DB.Apellido_23DB);
            usuario_23DB.Bloqueado_23DB = false;
            usuario_23DB.Activo_23DB = true;
            int fa_23DB = mapperUsuario_23DB.Insertar_23DB(usuario_23DB);
            if(fa_23DB < 0) throw new Exception("No se pudo registrar el usuario.");
            dvBLL_23DB.RecalcularDVTabla_23DB("Usuario_23DB");
        }

        public void ModificarUsuario_23DB(Usuario_23DB usuario_23DB)
        {
            int fa_23DB = mapperUsuario_23DB.Modificar_23DB(usuario_23DB);
            if(fa_23DB < 0) throw new Exception("No se pudo modificar el usuario.");
            dvBLL_23DB.RecalcularDVTabla_23DB("Usuario_23DB");
        }

        public void CambiarEstado_23DB(string dni_23DB, bool activo_23DB)
        {
            int fa_23DB = mapperUsuario_23DB.CambiarEstado_23DB(dni_23DB, activo_23DB);
            if(fa_23DB < 0) throw new Exception("No se pudo cambiar el estado del usuario.");
            dvBLL_23DB.RecalcularDVTabla_23DB("Usuario_23DB");
        }

        public void DesbloquearUsuario_23DB(string dni_23DB, string apellido_23DB)
        {
            string passwordInicial_23DB = GenerarPasswordInicial_23DB(dni_23DB, apellido_23DB);
            int fa_23DB = mapperUsuario_23DB.Desbloquear_23DB(dni_23DB, passwordInicial_23DB);
            if(fa_23DB < 0) throw new Exception("No se pudo desbloquear el usuario.");
            dvBLL_23DB.RecalcularDVTabla_23DB("Usuario_23DB");
        }

        public bool CambiarClave_23DB(string dni_23DB, string passwordActual_23DB, string passwordNuevo_23DB, string confirmarPassword_23DB)
        {
            if(passwordNuevo_23DB != confirmarPassword_23DB) return false;

            if(passwordActual_23DB == passwordNuevo_23DB) return false;
            
            string passwordActualEncriptado_23DB = CryptoManager_23DB.EncriptarHash_23DB(passwordActual_23DB);
            Usuario_23DB usuario_23DB = mapperUsuario_23DB.ObtenerUsuarioPorDNI_23DB(SessionManager_23DB.ObtenerInstancia_23DB().DNI_23DB, passwordActualEncriptado_23DB);

            if(usuario_23DB == null) return false;

            string passwordNuevoEncriptado_23DB = CryptoManager_23DB.EncriptarHash_23DB(passwordNuevo_23DB);
            int fa_23DB = mapperUsuario_23DB.ActualizarPassword_23DB(dni_23DB, passwordNuevoEncriptado_23DB);
            if(fa_23DB < 0) throw new Exception("No se pudo actualizar la contraseña del usuario.");
            dvBLL_23DB.RecalcularDVTabla_23DB("Usuario_23DB");
            return true;
        }

        public void IncrementarIntentos_23DB(string dni_23DB)
        {
            int fa_23DB = mapperUsuario_23DB.IncrementarIntentos_23DB(dni_23DB);
            if(fa_23DB < 0) throw new Exception("No se pudo registrar el intento fallido del usuario.");
            dvBLL_23DB.RecalcularDVTabla_23DB("Usuario_23DB");
        }

        public void ResetearIntentos_23DB(string dni_23DB)
        {
            int fa_23DB = mapperUsuario_23DB.ResetearIntentos_23DB(dni_23DB);
            if(fa_23DB < 0) throw new Exception("No se pudieron resetear los intentos del usuario.");
            dvBLL_23DB.RecalcularDVTabla_23DB("Usuario_23DB");
        }

        public void ActualizarUltimoIdioma_23DB(string dni_23DB, string idioma_23DB)
        {
            int fa_23DB = mapperUsuario_23DB.ActualizarUltimoIdioma_23DB(dni_23DB, idioma_23DB);
            if(fa_23DB < 0) throw new Exception("No se pudo actualizar el idioma del usuario.");
            dvBLL_23DB.RecalcularDVTabla_23DB("Usuario_23DB");
        }

        public bool VerificarTiempoReset_23DB(DateTime? fechaUltimoIntento_23DB)
        {
            if(fechaUltimoIntento_23DB == null) return false;
            
            return (DateTime.Now - fechaUltimoIntento_23DB.Value).TotalHours >= 5;
        }

        public List<DV_23DB> VerificarConsistenciaDV_23DB()
        {
            return dvBLL_23DB.VerificarTodasLasTablas_23DB();
        }
    }
}
