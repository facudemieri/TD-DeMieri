using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Services_23DB
{
    public class DVBLL_23DB
    {
        private mapperDV_23DB mapperDV_23DB = new mapperDV_23DB();

        private const string columnaDVH_23DB = "DVH_23DB";

        private long ConvertirANumero_23DB(object valor_23DB)
        {
            if(valor_23DB == null || valor_23DB == DBNull.Value)
            {
                return 0;
            }

            string texto_23DB = valor_23DB.ToString();
            long resultado_23DB = 0;
            foreach(char c_23DB in texto_23DB)
            {
                resultado_23DB += (long)c_23DB;
            }

            return resultado_23DB;
        }

        // Calcula el DVH de una fila sumando todas sus columnas excepto DVH_23DB.
        private long CalcularDV_23DB(DataRow fila_23DB)
        {
            long dvh_23DB = 0;
            foreach(DataColumn col_23DB in fila_23DB.Table.Columns)
            {
                if(string.Equals(col_23DB.ColumnName, columnaDVH_23DB, StringComparison.OrdinalIgnoreCase)) continue;
                dvh_23DB += ConvertirANumero_23DB(fila_23DB[col_23DB]);
            }
            return dvh_23DB;
        }

        // Separa NombrePK en sus columnas y valida que existan, junto con DVH_23DB, en los datos de la tabla.
        private string[] ObtenerColumnasPK_23DB(DV_23DB dv_23DB, DataTable datos_23DB)
        {
            if(string.IsNullOrWhiteSpace(dv_23DB.NombrePK_23DB))
            {
                throw new Exception($"La tabla {dv_23DB.NombreTabla_23DB} no tiene cargado NombrePK en DV_23DB.");
            }
            if(!datos_23DB.Columns.Contains(columnaDVH_23DB))
            {
                throw new Exception($"La tabla {dv_23DB.NombreTabla_23DB} no tiene la columna {columnaDVH_23DB}.");
            }

            string[] columnas_23DB = dv_23DB.NombrePK_23DB
                .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(c => c.Trim())
                .ToArray();

            foreach(string columna_23DB in columnas_23DB)
            {
                if(!datos_23DB.Columns.Contains(columna_23DB))
                {
                    throw new Exception($"La columna de clave {columna_23DB} no existe en la tabla {dv_23DB.NombreTabla_23DB}.");
                }
            }
            return columnas_23DB;
        }

        // Devuelve los valores de la clave de una fila como string.
        private string[] ObtenerValoresPK_23DB(DataRow fila_23DB, string[] columnasPK_23DB)
        {
            return columnasPK_23DB.Select(c => fila_23DB[c].ToString()).ToArray();
        }

        // Arma la descripción de una clave: "Columna=Valor" o "Col1=Valor1, Col2=Valor2".
        private string DescribirClave_23DB(string[] columnasPK_23DB, string[] valoresPK_23DB)
        {
            List<string> partes_23DB = new List<string>();
            for(int i_23DB = 0; i_23DB < columnasPK_23DB.Length; i_23DB++)
            {
                partes_23DB.Add($"{columnasPK_23DB[i_23DB]}={valoresPK_23DB[i_23DB]}");
            }
            return string.Join(", ", partes_23DB);
        }

        // Recalcula los DVH y el DVV de todas las tablas verificadas.
        public void RecalcularDV_23DB()
        {
            List<DV_23DB> tablas_23DB = mapperDV_23DB.ObtenerTodosDV_23DB();
            foreach(DV_23DB tabla_23DB in tablas_23DB)
            {
                RecalcularDVTabla_23DB(tabla_23DB.NombreTabla_23DB);
            }
        }

        // Actualiza el DVH de las filas que cambiaron y guarda el DVV de la tabla.
        public void RecalcularDVTabla_23DB(string nombreTabla_23DB)
        {
            DV_23DB tabla_23DB = mapperDV_23DB.ObtenerDV_23DB(nombreTabla_23DB);
            if (tabla_23DB == null) return;
            DataTable datos_23DB = mapperDV_23DB.ObtenerDatosTabla_23DB(nombreTabla_23DB);
            string[] columnasPK_23DB = ObtenerColumnasPK_23DB(tabla_23DB, datos_23DB);

            long dvv_23DB = 0;
            foreach(DataRow fila_23DB in datos_23DB.Rows)
            {
                long dvhCalculado_23DB = CalcularDV_23DB(fila_23DB);
                long dvhAlmacenado_23DB = Convert.ToInt64(fila_23DB[columnaDVH_23DB]);
                if(dvhCalculado_23DB != dvhAlmacenado_23DB)
                {
                    string[] valoresPK_23DB = ObtenerValoresPK_23DB(fila_23DB, columnasPK_23DB);
                    int faFila_23DB = mapperDV_23DB.ActualizarDVHRegistro_23DB(nombreTabla_23DB, columnasPK_23DB, valoresPK_23DB, dvhCalculado_23DB);
                    if(faFila_23DB != 1) throw new Exception($"No se pudo actualizar el DVH de {nombreTabla_23DB} ({DescribirClave_23DB(columnasPK_23DB, valoresPK_23DB)}): filas afectadas {faFila_23DB}.");
                }
                dvv_23DB += dvhCalculado_23DB;
            }

            int fa_23DB = mapperDV_23DB.ActualizarDVV_23DB(nombreTabla_23DB, dvv_23DB);
            if(fa_23DB < 0) throw new Exception($"No se pudo actualizar el digito verificador de {nombreTabla_23DB}.");
        }

        // Verifica los DVH de cada fila y el DVV de la tabla. Devuelve null si está consistente;
        // si no, las claves de las filas adulteradas (lista vacía = filas agregadas o borradas).
        public DV_23DB VerificarConsistencia_23DB(string nombreTabla_23DB)
        {
            DV_23DB dvAlmacenado_23DB = mapperDV_23DB.ObtenerDV_23DB(nombreTabla_23DB);
            if(dvAlmacenado_23DB == null) return null;

            DataTable datos_23DB = mapperDV_23DB.ObtenerDatosTabla_23DB(nombreTabla_23DB);
            string[] columnasPK_23DB = ObtenerColumnasPK_23DB(dvAlmacenado_23DB, datos_23DB);

            List<string> registrosInconsistentes_23DB = new List<string>();
            long sumaDVHAlmacenados_23DB = 0;
            foreach(DataRow fila_23DB in datos_23DB.Rows)
            {
                long dvhAlmacenado_23DB = Convert.ToInt64(fila_23DB[columnaDVH_23DB]);
                sumaDVHAlmacenados_23DB += dvhAlmacenado_23DB;
                if(CalcularDV_23DB(fila_23DB) != dvhAlmacenado_23DB)
                {
                    registrosInconsistentes_23DB.Add(DescribirClave_23DB(columnasPK_23DB, ObtenerValoresPK_23DB(fila_23DB, columnasPK_23DB)));
                }
            }

            if(registrosInconsistentes_23DB.Count > 0 || sumaDVHAlmacenados_23DB != dvAlmacenado_23DB.DVV_23DB)
            {
                return new DV_23DB
                {
                    NombreTabla_23DB = nombreTabla_23DB,
                    RegistrosInconsistentes_23DB = registrosInconsistentes_23DB
                };
            }
            return null;
        }

        public List<DV_23DB> VerificarTodasLasTablas_23DB()
        {
            List<DV_23DB> inconsistencias_23DB = new List<DV_23DB>();
            List<DV_23DB> tablas_23DB = mapperDV_23DB.ObtenerTodosDV_23DB();
            foreach(DV_23DB tabla_23DB in tablas_23DB)
            {
                DV_23DB inconsistencia_23DB = VerificarConsistencia_23DB(tabla_23DB.NombreTabla_23DB);
                if(inconsistencia_23DB != null)
                {
                    inconsistencias_23DB.Add(inconsistencia_23DB);
                }

            }
            return inconsistencias_23DB;
        }

        // Devuelve true si todas las tablas tienen DVV en cero.
        public bool EsPrimeraInstalacion_23DB()
        {
            List<DV_23DB> tablas_23DB = mapperDV_23DB.ObtenerTodosDV_23DB();
            return tablas_23DB.All(t => t.DVV_23DB == 0);
        }
    }
}
