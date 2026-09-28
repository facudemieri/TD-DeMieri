using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Services_23DB
{
    public class DVBLL_23DB
    {
        private mapperDV_23DB mapperDV_23DB = new mapperDV_23DB();

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

        private (long dvh, long dvv) CalcularDV_23DB(DataTable tabla_23DB)
        {
            long dvh_23DB = 0;
            long dvv_23DB = 0;

            // Suma por fila
            foreach(DataRow fila_23DB in tabla_23DB.Rows)
            {
                long dvhFila_23DB = 0;
                foreach(DataColumn col_23DB in tabla_23DB.Columns)
                { 
                    dvhFila_23DB += ConvertirANumero_23DB(fila_23DB[col_23DB]); 
                }
                dvh_23DB += dvhFila_23DB;
            }

            // Suma por columna
            foreach(DataColumn col_23DB in tabla_23DB.Columns)
            {
                long dvvCol_23DB = 0;
                foreach(DataRow fila_23DB in tabla_23DB.Rows)
                {
                    dvvCol_23DB += ConvertirANumero_23DB(fila_23DB[col_23DB]);
                }
                    
                dvv_23DB += dvvCol_23DB;
            }

            return (dvh_23DB, dvv_23DB);
        }

        public void RecalcularDV_23DB()
        {
            List<DV_23DB> tablas_23DB = mapperDV_23DB.ObtenerTodosDV_23DB();
            foreach(DV_23DB tabla_23DB in tablas_23DB)
            {
                DataTable datos_23DB = mapperDV_23DB.ObtenerDatosTabla_23DB(tabla_23DB.NombreTabla_23DB);
                var (dvh, dvv) = CalcularDV_23DB(datos_23DB);
                int fa_23DB = mapperDV_23DB.ActualizarDV_23DB(tabla_23DB.IdTabla_23DB, dvh, dvv);
                if(fa_23DB < 0) throw new Exception($"No se pudo actualizar el digito verificador de {tabla_23DB.NombreTabla_23DB}.");
            }
        }

        public void RecalcularDVTabla_23DB(string nombreTabla_23DB)
        {
            List<DV_23DB> tablas_23DB = mapperDV_23DB.ObtenerTodosDV_23DB();
            DV_23DB tabla_23DB = tablas_23DB.Find(t => t.NombreTabla_23DB == nombreTabla_23DB);
            if (tabla_23DB == null) return;
            DataTable datos_23DB = mapperDV_23DB.ObtenerDatosTabla_23DB(nombreTabla_23DB);
            var (dvh, dvv) = CalcularDV_23DB(datos_23DB);
            int fa_23DB = mapperDV_23DB.ActualizarDV_23DB(tabla_23DB.IdTabla_23DB, dvh, dvv);
            if(fa_23DB < 0) throw new Exception($"No se pudo actualizar el digito verificador de {nombreTabla_23DB}.");
        }

        public DV_23DB VerificarConsistencia_23DB(string nombreTabla_23DB)
        {
            List<DV_23DB> tablas_23DB = mapperDV_23DB.ObtenerTodosDV_23DB();
            DV_23DB dvAlmacenado_23DB = tablas_23DB.Find(t => t.NombreTabla_23DB == nombreTabla_23DB);
            if(dvAlmacenado_23DB == null) return null;

            DataTable datos_23DB = mapperDV_23DB.ObtenerDatosTabla_23DB(nombreTabla_23DB);
            var (dvh, dvv) = CalcularDV_23DB(datos_23DB);

            if(dvh != dvAlmacenado_23DB.DVH_23DB || dvv != dvAlmacenado_23DB.DVV_23DB)
            {
                return new DV_23DB
                {
                    NombreTabla_23DB = nombreTabla_23DB,
                    DVH_23DB = dvh,
                    DVV_23DB = dvv
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

        public bool EsPrimeraInstalacion_23DB()
        {
            List<DV_23DB> tablas_23DB = mapperDV_23DB.ObtenerTodosDV_23DB();
            return tablas_23DB.All(t => t.DVH_23DB == 0 && t.DVV_23DB == 0);
        }
    }
}