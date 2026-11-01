using BLL_23DB;
using GUI_23DB.Reportes;
using Services_23DB;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace GUI_23DB
{
    public partial class AnaliticaMantenimiento_62FD : Form, IIdiomaObserver_23DB
    {
        private AeronaveBLL_62FD aeronaveBLL_62FD = new AeronaveBLL_62FD();
        private IntervencionBLL_62FD intervencionBLL_62FD = new IntervencionBLL_62FD();
        private RepuestoBLL_62FD repuestoBLL_62FD = new RepuestoBLL_62FD();

        // Valor que devuelve la BLL (el de la base) y clave del texto que se muestra en la leyenda
        private static readonly Dictionary<string, string> clavesValorGrafico_62FD = new Dictionary<string, string>
        {
            { "En Servicio", "Msg_GraficoEnServicio" },
            { "En Mantenimiento", "Msg_GraficoEnMantenimiento" },
            { "Fuera de Servicio", "Msg_GraficoFueraDeServicio" },
            { "Preventivo", "Msg_GraficoPreventivo" },
            { "Correctivo", "Msg_GraficoCorrectivo" }
        };

        private bool hayDatos_62FD = false;
        private bool sinDatosReposicion_62FD = false;
        private string claveEstado_62FD = null;
        private int? cantidadAeronaves_62FD = null;
        public AnaliticaMantenimiento_62FD()
        {
            InitializeComponent();
            Observer_23DB.ObtenerInstancia_23DB().Suscribir_23DB(this);
            this.FormClosing += AnaliticaMantenimiento_62FD_FormClosing;
            AplicarIdiomaActual_23DB();
            Ayuda_62FD.Vincular_23DB(this, "analitica-mantenimiento");
        }

        public void ActualizarIdioma_23DB(Dictionary<string, string> configuracion_23DB)
        {
            Traductor_23DB.AplicarIdioma_23DB(this, configuracion_23DB);
            TraducirTitulosGraficos_62FD();
            TraducirLeyendas_62FD(chart1);
            TraducirLeyendas_62FD(chart2);

            if (sinDatosReposicion_62FD)
            {
                lblDiasRepo.Text = IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_SinDatos");
            }

            MostrarTextosEstado_62FD();
        }

        public void AplicarIdiomaActual_23DB()
        {
            string idiomaActual_23DB = SessionManager_23DB.ObtenerInstancia_23DB().UltimoIdioma_23DB;
            if (!string.IsNullOrEmpty(idiomaActual_23DB))
            {
                IdiomaBLL_23DB idiomaBLL_23DB = new IdiomaBLL_23DB();
                ActualizarIdioma_23DB(idiomaBLL_23DB.CargarConfiguracion_23DB(idiomaActual_23DB));
            }
        }

        private void AnaliticaMantenimiento_62FD_FormClosing(object sender, FormClosingEventArgs e)
        {
            Observer_23DB.ObtenerInstancia_23DB().Desuscribir_23DB(this);
        }

        private void MostrarTextosEstado_62FD()
        {
            if (claveEstado_62FD != null)
            {
                toolStripStatusLabel1.Text = string.Format(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_BarraEstado"), IdiomaBLL_23DB.ObtenerTexto_23DB(claveEstado_62FD));
            }

            if (cantidadAeronaves_62FD.HasValue)
            {
                toolStripStatusLabel2.Text = string.Format(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_BarraAeronaves"), cantidadAeronaves_62FD.Value);
            }

            toolStripStatusLabel3.Text = string.Format(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_BarraUsuario"), SessionManager_23DB.ObtenerInstancia_23DB().Login_23DB);
        }


        private void TraducirTitulosGraficos_62FD()
        {
            if (chart1.Titles.Count > 0) chart1.Titles[0].Text = IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_GraficoEstadoOperativo");
            if (chart2.Titles.Count > 0) chart2.Titles[0].Text = IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_GraficoIntervencionesTipo");
            if (chart3.Titles.Count > 0) chart3.Titles[0].Text = IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_GraficoRevisionesProyectadas");
        }

        private void TraducirLeyendas_62FD(Chart grafico_62FD)
        {
            if (grafico_62FD.Series.Count == 0) return;

            foreach (DataPoint punto_62FD in grafico_62FD.Series[0].Points)
            {
                if (punto_62FD.Tag != null)
                {
                    punto_62FD.LegendText = ObtenerTextoValor_62FD(punto_62FD.Tag.ToString());
                }
            }
        }

        private string ObtenerTextoValor_62FD(string valor_62FD)
        {
            string clave_62FD;
            if (clavesValorGrafico_62FD.TryGetValue(valor_62FD, out clave_62FD))
            {
                return IdiomaBLL_23DB.ObtenerTexto_23DB(clave_62FD);
            }
            return valor_62FD;
        }

        private void dtpDesde_ValueChanged(object sender, EventArgs e)
        {
            dtpHasta.MinDate = dtpDesde.Value.Date;
        }

        private void ConfigurarParametros_62FD()
        {
            dtpDesde.Format = DateTimePickerFormat.Short;
            dtpHasta.Format = DateTimePickerFormat.Short;
            dtpDesde.ValueChanged += dtpDesde_ValueChanged;

            dtpDesde.MaxDate = DateTime.Today;
            dtpHasta.MaxDate = DateTime.Today;

            numericUpDown1.Minimum = 3;
            numericUpDown1.Maximum = 12;
            numericUpDown1.Value = 6;
        }

        private void AnaliticaMantenimiento_62FD_Load(object sender, EventArgs e)
        {
            ConfigurarParametros_62FD();

            dtpDesde.Value = DateTime.Today.AddMonths(-12);
            dtpHasta.Value = DateTime.Today;

            btnGenerarPDF.Enabled = false;
            MostrarTextosEstado_62FD();

            ConfigurarGraficos_62FD();
            ActualizarAnalitica_62FD();
        }

        private void ActualizarAnalitica_62FD()
        {
            try
            {
                hayDatos_62FD = false;
                btnGenerarPDF.Enabled = false;

                if (dtpHasta.Value.Date < dtpDesde.Value.Date)
                {
                    MessageBox.Show(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_FechaHastaAnteriorDesde"), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Informacion"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                int vencidas_62FD = aeronaveBLL_62FD.ContarRevisionesVencidas_62FD();
                int proximas_62FD = aeronaveBLL_62FD.ContarRevisionesProximas_62FD(30);
                decimal diasRepo_62FD = repuestoBLL_62FD.ObtenerPromedioDiasReposicion_62FD();

                lblNroRevicionVencida.Text = vencidas_62FD.ToString();
                lblVence30dias.Text = proximas_62FD.ToString();

                sinDatosReposicion_62FD = diasRepo_62FD == RepuestoBLL_62FD.SinDatos_62FD;

                if (sinDatosReposicion_62FD)
                {
                    lblDiasRepo.Text = IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_SinDatos");
                }
                else
                {
                    lblDiasRepo.Text = diasRepo_62FD.ToString("N1");
                }

                CargarTorta_62FD(chart1, aeronaveBLL_62FD.ObtenerDistribucionEstadoOperativo_62FD());
                CargarTorta_62FD(chart2, intervencionBLL_62FD.ContarIntervencionesPorTipo_62FD(dtpDesde.Value, dtpHasta.Value));
                CargarProyeccion_62FD(aeronaveBLL_62FD.ProyectarRevisionesPorMes_62FD((int)numericUpDown1.Value));

                claveEstado_62FD = "Msg_EstadoAnaliticaActualizada";
                cantidadAeronaves_62FD = aeronaveBLL_62FD.ObtenerAeronaves_62FD("", "", "", "", false).Count;
                MostrarTextosEstado_62FD();

                hayDatos_62FD = true;
                btnGenerarPDF.Enabled = true;
            }
            catch (Exception ex_62FD)
            {
                MessageBox.Show(string.Format(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_ErrorCalcularAnalitica"), ex_62FD.Message), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarProyeccion_62FD(List<KeyValuePair<string, int>> datos_62FD)
        {
            Series serie_62FD = chart3.Series[0];
            serie_62FD.Points.Clear();

            for (int i_62FD = datos_62FD.Count - 1; i_62FD >= 0; i_62FD--)
            {
                serie_62FD.Points.AddXY(datos_62FD[i_62FD].Key, datos_62FD[i_62FD].Value);
            }
        }

        private void CargarTorta_62FD(Chart grafico_62FD, List<KeyValuePair<string, int>> datos_62FD)
        {
            Series serie_62FD = grafico_62FD.Series[0];
            serie_62FD.Points.Clear();

            foreach (KeyValuePair<string, int> dato_62FD in datos_62FD)
            {
                int indice_62FD = serie_62FD.Points.AddY(dato_62FD.Value);
                serie_62FD.Points[indice_62FD].Tag = dato_62FD.Key;
                serie_62FD.Points[indice_62FD].LegendText = ObtenerTextoValor_62FD(dato_62FD.Key);
                serie_62FD.Points[indice_62FD].Label = dato_62FD.Value.ToString();
            }
        }

        private void ConfigurarGraficos_62FD()
        {
            ConfigurarTorta_62FD(chart1, IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_GraficoEstadoOperativo"));
            ConfigurarTorta_62FD(chart2, IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_GraficoIntervencionesTipo"));

            chart3.Series.Clear();
            chart3.Titles.Clear();
            chart3.Titles.Add(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_GraficoRevisionesProyectadas"));
            chart3.Legends[0].Enabled = false;

            Series serie_62FD = chart3.Series.Add("Revisiones");
            serie_62FD.ChartType = SeriesChartType.Bar;
            serie_62FD.IsValueShownAsLabel = true;

            chart3.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
            chart3.ChartAreas[0].AxisX.Interval = 1;
            chart3.ChartAreas[0].AxisY.Minimum = 0;
        }

        private void ConfigurarTorta_62FD(Chart grafico_62FD, string titulo_62FD)
        {
            grafico_62FD.Series.Clear();
            grafico_62FD.Titles.Clear();
            grafico_62FD.Titles.Add(titulo_62FD);

            grafico_62FD.Legends[0].Enabled = true;
            grafico_62FD.Legends[0].Docking = Docking.Right;

            Series serie_62FD = grafico_62FD.Series.Add("Datos");
            serie_62FD.ChartType = SeriesChartType.Pie;
            serie_62FD.IsValueShownAsLabel = true;
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            ActualizarAnalitica_62FD();

        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();

        }

        private void btnGenerarPDF_Click(object sender, EventArgs e)
        {
            try
            {
                btnGenerarPDF.Enabled = false;

                if (!hayDatos_62FD)
                {
                    MessageBox.Show(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_ActualiceAnaliticaAntesGenerar"), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Informacion"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                string carpeta_62FD = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "AeroManager", "Analitica");

                if (!Directory.Exists(carpeta_62FD))
                {
                    Directory.CreateDirectory(carpeta_62FD);
                }

                string ruta_62FD = Path.Combine(carpeta_62FD, "Analitica_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".pdf");

                GeneradorAnaliticaPDF_62FD generador_62FD = new GeneradorAnaliticaPDF_62FD();
                generador_62FD.Generar_62FD(ruta_62FD, dtpDesde.Value, dtpHasta.Value, (int)numericUpDown1.Value, lblNroRevicionVencida.Text, lblVence30dias.Text, lblDiasRepo.Text, ObtenerImagen_62FD(chart1), ObtenerImagen_62FD(chart2), ObtenerImagen_62FD(chart3));

                System.Diagnostics.Process.Start(ruta_62FD);

                claveEstado_62FD = "Msg_EstadoDocumentoGenerado";
                MostrarTextosEstado_62FD();
            }
            catch (Exception ex_62FD)
            {
                MessageBox.Show(string.Format(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_ErrorGenerarDocumento"), ex_62FD.Message), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnGenerarPDF.Enabled = hayDatos_62FD;
            }
        }

        private System.Drawing.Image ObtenerImagen_62FD(Chart grafico_62FD)
        {
            MemoryStream memoria_62FD = new MemoryStream();
            grafico_62FD.SaveImage(memoria_62FD, ChartImageFormat.Png);
            memoria_62FD.Position = 0;
            return System.Drawing.Image.FromStream(memoria_62FD);
        }
    }
}
