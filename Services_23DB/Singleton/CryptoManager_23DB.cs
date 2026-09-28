using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Services_23DB
{
    public class CryptoManager_23DB
    {
        public static string EncriptarHash_23DB(string texto_23DB)
        {
            using (SHA256 sha256_23DB = SHA256.Create())
            {
                byte[] bytes_23DB = sha256_23DB.ComputeHash(Encoding.UTF8.GetBytes(texto_23DB));
                StringBuilder resultado_23DB = new StringBuilder();
                foreach (byte b in bytes_23DB)
                {
                    resultado_23DB.Append(b.ToString("x2"));
                }
                return resultado_23DB.ToString();
            }
        }

        private static readonly string clave_23DB = "ClaveSecreta23DB!ClaveSecreta23D";
        public static string Encriptar_23DB(string texto_23DB)
        {
            using (Aes aes_23DB = Aes.Create())
            {
                aes_23DB.Key = Encoding.UTF8.GetBytes(clave_23DB);
                aes_23DB.IV = new byte[16];
                ICryptoTransform encriptador_23DB = aes_23DB.CreateEncryptor();
                byte[] bytesTexto_23DB = Encoding.UTF8.GetBytes(texto_23DB);
                byte[] bytesEncriptados_23DB = encriptador_23DB.TransformFinalBlock(bytesTexto_23DB, 0, bytesTexto_23DB.Length);
                return Convert.ToBase64String(bytesEncriptados_23DB);
            }
        }

        public static string Desencriptar_23DB(string textoEncriptado_23DB)
        {
            using (Aes aes_23DB = Aes.Create())
            {
                aes_23DB.Key = Encoding.UTF8.GetBytes(clave_23DB);
                aes_23DB.IV = new byte[16];
                ICryptoTransform desencriptador_23DB = aes_23DB.CreateDecryptor();
                byte[] bytesEncriptados_23DB = Convert.FromBase64String(textoEncriptado_23DB);
                byte[] bytesDesencriptados_23DB = desencriptador_23DB.TransformFinalBlock(bytesEncriptados_23DB, 0, bytesEncriptados_23DB.Length);
                return Encoding.UTF8.GetString(bytesDesencriptados_23DB);
            }
        }

        public static string GenerarLogin_23DB(string nombre_23DB, string apellido_23DB)
        {
            return nombre_23DB.Trim() + "." + apellido_23DB.Trim();
        }

        public static string GenerarPassword_23DB(string dni_23DB, string apellido_23DB)
        {
            string passwordInicial_23DB = dni_23DB.Trim() + apellido_23DB.Trim();
            return EncriptarHash_23DB(passwordInicial_23DB);
        }
    }
}
