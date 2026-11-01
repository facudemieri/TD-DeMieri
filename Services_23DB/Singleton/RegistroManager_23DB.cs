using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services_23DB.Singleton
{
    public static class RegistroManager_23DB
    {
        private const string Clave_23DB = @"Software\AeroManager";
        private const string ValLogin_23DB = "LoginRecuperacion";
        private const string ValPass_23DB = "PasswordRecuperacion";

        private const string LoginInicial_23DB = "recuperacion";
        private const string PassInicial_23DB = "Recuperacion.2026";

        public static void GarantizarUsuarioRecuperacion_23DB()
        {
            try
            {
                using (RegistryKey clave_23DB = Registry.CurrentUser.CreateSubKey(Clave_23DB))
                {
                    if (clave_23DB.GetValue(ValLogin_23DB) == null)
                    {
                        clave_23DB.SetValue(ValLogin_23DB, LoginInicial_23DB);
                        clave_23DB.SetValue(ValPass_23DB,
                            CryptoManager_23DB.EncriptarHash_23DB(PassInicial_23DB));
                    }
                }
            }
            catch (Exception)
            {
                // Si el registro no se puede escribir, el sistema sigue funcionando
                // normalmente contra la base. Solo se pierde el modo de recuperación.
            }
        }

        public static bool ValidarUsuarioRecuperacion_23DB(string login_23DB, string password_23DB)
        {
            try
            {
                using (RegistryKey clave_23DB = Registry.CurrentUser.OpenSubKey(Clave_23DB))
                {
                    if (clave_23DB == null) return false;

                    string loginGuardado_23DB = clave_23DB.GetValue(ValLogin_23DB) as string;
                    string passGuardado_23DB = clave_23DB.GetValue(ValPass_23DB) as string;

                    if (string.IsNullOrEmpty(loginGuardado_23DB)) return false;
                    if (string.IsNullOrEmpty(passGuardado_23DB)) return false;

                    return login_23DB == loginGuardado_23DB
                        && CryptoManager_23DB.EncriptarHash_23DB(password_23DB) == passGuardado_23DB;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
