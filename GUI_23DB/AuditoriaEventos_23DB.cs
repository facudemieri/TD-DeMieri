using Services_23DB;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI_23DB
{
    public partial class AuditoriaEventos_23DB : Form, IIdiomaObserver_23DB
    {
        
        public AuditoriaEventos_23DB()
        {
            InitializeComponent();
            Observer_23DB.ObtenerInstancia_23DB().Suscribir_23DB(this);
            AplicarIdiomaActual_23DB();


            cmbLogin.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbCriticidad.DropDownStyle = ComboBoxStyle.DropDownList;


            txtNombre.MaxLength = 50;
            txtApellido.MaxLength = 50;

            ConfigurarGrilla_62FD(dgvBitacoraEventos);
            dgvBitacoraEventos.Paint += Grilla_Paint_62FD;
            dgvBitacoraEventos.ColumnHeaderMouseClick += dgvBitacoraEventos_ColumnHeaderMouseClick;
            this.Shown += (s_62FD, e_62FD) => LimpiarSeleccionGrilla_62FD(dgvBitacoraEventos);
        }

        private EventoBLL_23DB eventoBLL_23DB = new EventoBLL_23DB();
        private List<Evento_23DB> listaEventos_23DB = new List<Evento_23DB>();
        private string columnaOrden_23DB = null;
        private bool ordenAscendente_23DB = true;

        private void CargarLogins_23DB()
        {
            List<Usuario_23DB> logins_23DB = eventoBLL_23DB.ObtenerLogins_23DB();
            cmbLogin.DataSource = logins_23DB;
            cmbLogin.DisplayMember = "Login_23DB";
            cmbLogin.ValueMember = "DNI_23DB";
            cmbLogin.SelectedIndex = -1;
        }

        private void CargarModulos_23DB()
        {
            cmbModulo.Items.Clear();
            cmbModulo.Items.Add("Usuarios");
            cmbModulo.Items.Add("Administrador");
            cmbModulo.Items.Add("Pasajes");
            cmbModulo.Items.Add("Mantenimiento");
            cmbModulo.Items.Add("Maestros");
            cmbModulo.SelectedIndex = -1;
        }

        private void CargarEventos_23DB()
        {
            cmbEvento.Items.Clear();
            cmbEvento.Items.Add("Login");
            cmbEvento.Items.Add("Logout");
            cmbEvento.Items.Add("Bloqueo Automático por Intentos");
            cmbEvento.Items.Add("Crear Usuario");
            cmbEvento.Items.Add("Modificar Usuario");
            cmbEvento.Items.Add("Activar / Desactivar Usuario");
            cmbEvento.Items.Add("Desbloquear Usuario");
            cmbEvento.Items.Add("Cambiar Clave");
            cmbEvento.Items.Add("Cambiar Idioma");
            cmbEvento.Items.Add("Crear Familia");
            cmbEvento.Items.Add("Modificar Familia");
            cmbEvento.Items.Add("Eliminar Familia");
            cmbEvento.Items.Add("Crear Rol");
            cmbEvento.Items.Add("Modificar Rol");
            cmbEvento.Items.Add("Eliminar Rol");
            cmbEvento.Items.Add("Backup BD");
            cmbEvento.Items.Add("Restore BD");
            cmbEvento.Items.Add("Recalcular DV");
            cmbEvento.Items.Add("Generar Reserva");
            cmbEvento.Items.Add("Registrar Pasajero");
            cmbEvento.Items.Add("Generar Pasaje");
            cmbEvento.Items.Add("Generar Orden de Trabajo");
            cmbEvento.Items.Add("Registrar Intervención");
            cmbEvento.Items.Add("Solicitar Repuestos");
            cmbEvento.Items.Add("Registrar Recepción de Repuestos");
            cmbEvento.Items.Add("Certificar Intervención");
            cmbEvento.Items.Add("Crear Aeronave");
            cmbEvento.Items.Add("Modificar Aeronave");
            cmbEvento.Items.Add("Dar de Baja Aeronave");
            cmbEvento.Items.Add("Reactivar Aeronave");
            cmbEvento.Items.Add("Crear Vuelo");
            cmbEvento.Items.Add("Modificar Vuelo");
            cmbEvento.Items.Add("Cancelar Vuelo");
            cmbEvento.Items.Add("Agregar Asiento");
            cmbEvento.Items.Add("Modificar Asiento");
            cmbEvento.Items.Add("Eliminar Asiento");
            cmbEvento.Items.Add("Serializar Vuelos");
            cmbEvento.Items.Add("Deserializar Vuelos");
            cmbEvento.Items.Add("Activar Cambio Aeronave");
            cmbEvento.SelectedIndex = -1;
        }

        private void CargarCriticidades_23DB()
        {
            cmbCriticidad.Items.Clear();
            cmbCriticidad.Items.Add("1 - Crítico");
            cmbCriticidad.Items.Add("2 - Alto");
            cmbCriticidad.Items.Add("3 - Medio");
            cmbCriticidad.Items.Add("4 - Bajo");
            cmbCriticidad.SelectedIndex = -1;
        }

        private void CargarGrilla_23DB()
        {
            List<Evento_23DB> lista_23DB = eventoBLL_23DB.ObtenerEventos_23DB();
            EnlazarGrilla_23DB(lista_23DB);
        }

        private void EnlazarGrilla_23DB(List<Evento_23DB> lista_23DB)
        {
            listaEventos_23DB = lista_23DB;
            columnaOrden_23DB = null;

            dgvBitacoraEventos.DataSource = null;
            dgvBitacoraEventos.DataSource = listaEventos_23DB;
            Traductor_23DB.TraducirGrilla_23DB(dgvBitacoraEventos, "AuditoriaEventos");
            ConfigurarColumnas_23DB();
            LimpiarSeleccionGrilla_62FD(dgvBitacoraEventos);
        }

        private void ConfigurarColumnas_23DB()
        {
            foreach (DataGridViewColumn columna_23DB in dgvBitacoraEventos.Columns)
            {
                columna_23DB.SortMode = DataGridViewColumnSortMode.Programmatic;
                columna_23DB.MinimumWidth = 60;

                switch (columna_23DB.DataPropertyName)
                {
                    case "Id_Evento_23DB":
                        columna_23DB.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                        columna_23DB.FillWeight = 50;
                        break;
                    case "DNI_23DB":
                        columna_23DB.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        columna_23DB.FillWeight = 70;
                        break;
                    case "Fecha_23DB":
                        columna_23DB.DefaultCellStyle.Format = "dd/MM/yyyy";
                        columna_23DB.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        columna_23DB.FillWeight = 70;
                        break;
                    case "Hora_23DB":
                        columna_23DB.DefaultCellStyle.Format = @"hh\:mm\:ss";
                        columna_23DB.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        columna_23DB.FillWeight = 60;
                        break;
                    case "Evento23DB":
                        columna_23DB.FillWeight = 180;
                        break;
                    case "Criticidad_23DB":
                        columna_23DB.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        columna_23DB.FillWeight = 60;
                        break;
                }
            }
        }

        private void dgvBitacoraEventos_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string propiedad_23DB = dgvBitacoraEventos.Columns[e.ColumnIndex].DataPropertyName;
            bool ascendente_23DB = columnaOrden_23DB == propiedad_23DB ? !ordenAscendente_23DB : true;

            EnlazarGrilla_23DB(Ordenar_62FD(listaEventos_23DB, propiedad_23DB, ascendente_23DB));
            columnaOrden_23DB = propiedad_23DB;
            ordenAscendente_23DB = ascendente_23DB;

            dgvBitacoraEventos.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection = ascendente_23DB ? SortOrder.Ascending : SortOrder.Descending;
        }


        private void btnImprimir_Click(object sender, EventArgs e)
        {
            if(dgvBitacoraEventos.Rows.Count == 0)
            {
                MessageBox.Show(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_SinDatosImprimir"), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Advertencia"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SaveFileDialog saveDialog_23DB = new SaveFileDialog();
            saveDialog_23DB.Filter = "PDF files (*.pdf)|*.pdf";
            saveDialog_23DB.FileName = "BitacoraEventos_" + DateTime.Now.ToString("yyyyMMdd_HHmmss");

            if(saveDialog_23DB.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    iTextSharp.text.Document doc_23DB = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4.Rotate());
                    iTextSharp.text.pdf.PdfWriter.GetInstance(doc_23DB, new System.IO.FileStream(saveDialog_23DB.FileName, System.IO.FileMode.Create));
                    doc_23DB.Open();

                    // titulo
                    iTextSharp.text.Font titleFont_23DB = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 16, iTextSharp.text.Font.BOLD);
                    iTextSharp.text.Paragraph title_23DB = new iTextSharp.text.Paragraph("Bitácora de Eventos", titleFont_23DB);
                    title_23DB.Alignment = iTextSharp.text.Element.ALIGN_CENTER;
                    doc_23DB.Add(title_23DB);

                    // fecha
                    iTextSharp.text.Font dateFont_23DB = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 10);
                    iTextSharp.text.Paragraph date_23DB = new iTextSharp.text.Paragraph("Generado el: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"), dateFont_23DB);
                    date_23DB.Alignment = iTextSharp.text.Element.ALIGN_CENTER;
                    doc_23DB.Add(date_23DB);
                    doc_23DB.Add(new iTextSharp.text.Paragraph(" "));

                    // tabla
                    iTextSharp.text.pdf.PdfPTable tabla_23DB = new iTextSharp.text.pdf.PdfPTable(dgvBitacoraEventos.Columns.Count);
                    tabla_23DB.WidthPercentage = 100;

                    // encabezados
                    iTextSharp.text.Font headerFont_23DB = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 10, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.WHITE);
                    foreach(DataGridViewColumn col_23DB in dgvBitacoraEventos.Columns)
                    {
                        iTextSharp.text.pdf.PdfPCell cell_23DB = new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase(col_23DB.HeaderText, headerFont_23DB));
                        cell_23DB.BackgroundColor = new iTextSharp.text.BaseColor(31, 78, 121);
                        cell_23DB.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                        cell_23DB.Padding = 5;
                        tabla_23DB.AddCell(cell_23DB);
                    }

                    // datos
                    iTextSharp.text.Font dataFont_23DB = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 9);
                    foreach(DataGridViewRow row_23DB in dgvBitacoraEventos.Rows)
                    {
                        foreach(DataGridViewCell cell_23DB in row_23DB.Cells)
                        {
                            iTextSharp.text.pdf.PdfPCell pdfCell_23DB = new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase(cell_23DB.Value?.ToString() ?? string.Empty, dataFont_23DB));
                            pdfCell_23DB.Padding = 4;
                            tabla_23DB.AddCell(pdfCell_23DB);
                        }
                    }

                    doc_23DB.Add(tabla_23DB);
                    doc_23DB.Close();

                    MessageBox.Show(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_PdfGenerado"), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Exito"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    System.Diagnostics.Process.Start(saveDialog_23DB.FileName);
                }
                catch(Exception ex_23DB)
                {
                    MessageBox.Show(string.Format(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_ErrorGenerarPdf"), ex_23DB.Message), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnAplicar_Click(object sender, EventArgs e)
        {

            if(dtpFechaInicio.Value.Date > dtpFechaFin.Value.Date)
            {
                MessageBox.Show(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_FechaInicioMayorFin"), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Advertencia"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if(dtpFechaFin.Value > DateTime.Now.Date)
            {
                MessageBox.Show(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_FechaFinPosterior"), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Advertencia"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string dni_23DB = cmbLogin.SelectedIndex >= 0 ? cmbLogin.SelectedValue.ToString() : string.Empty;
            string modulo_23DB = cmbModulo.SelectedIndex >= 0 ? cmbModulo.SelectedItem.ToString() : string.Empty;
            string evento_23DB = cmbEvento.SelectedIndex >= 0 ? cmbEvento.SelectedItem.ToString() : string.Empty;
            int criticidad_23DB = cmbCriticidad.SelectedIndex >= 0 ? cmbCriticidad.SelectedIndex + 1 : 0;

            List<Evento_23DB> lista_23DB = eventoBLL_23DB.FiltrarEventos_23DB(dni_23DB, dtpFechaInicio.Value.Date, dtpFechaFin.Value.Date, modulo_23DB, evento_23DB, criticidad_23DB);

            EnlazarGrilla_23DB(lista_23DB);

            if(lista_23DB.Count == 0)
            { 
                MessageBox.Show(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_SinEventos"), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Informacion"), MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void dgvBitacoraEventos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.RowIndex >= 0)
            {
                string dni_23DB = dgvBitacoraEventos.Rows[e.RowIndex].Cells["DNI_23DB"].Value.ToString();
                Usuario_23DB usuario_23DB = eventoBLL_23DB.ObtenerUsuarioPorDNI_23DB(dni_23DB);
                if(usuario_23DB != null)
                {
                    txtNombre.Text = usuario_23DB.Nombre_23DB;
                    txtApellido.Text = usuario_23DB.Apellido_23DB;
                }
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            cmbLogin.SelectedIndex = -1;
            cmbModulo.SelectedIndex = -1;
            cmbEvento.SelectedIndex = -1;
            cmbCriticidad.SelectedIndex = -1;
            dtpFechaInicio.MaxDate = DateTime.Now.Date;
            dtpFechaFin.MaxDate = DateTime.Now.Date;
            dtpFechaInicio.Value = DateTime.Now.AddDays(-3).Date;
            dtpFechaFin.Value = DateTime.Now.Date;
            txtNombre.Text = string.Empty;
            txtApellido.Text = string.Empty;
            CargarGrilla_23DB();
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void AuditoriaEventos_23DB_Load(object sender, EventArgs e)
        {
            CargarLogins_23DB();
            CargarModulos_23DB();
            CargarEventos_23DB();
            CargarCriticidades_23DB();
            CargarGrilla_23DB();
            dtpFechaInicio.MaxDate = DateTime.Now.Date;
            dtpFechaFin.MaxDate = DateTime.Now.Date;
        }

        private void dtpFechaInicio_ValueChanged(object sender, EventArgs e)
        {
         
        }

        private void dtpFechaFin_ValueChanged(object sender, EventArgs e)
        {
    
        }

        public void ActualizarIdioma_23DB(Dictionary<string, string> configuracion_23DB)
        {
            Traductor_23DB.AplicarIdioma_23DB(this, configuracion_23DB);
        }

        private void AuditoriaEventos_23DB_FormClosing(object sender, FormClosingEventArgs e)
        {
            Observer_23DB.ObtenerInstancia_23DB().Desuscribir_23DB(this);
        }

        public void AplicarIdiomaActual_23DB()
        {
            string idiomaActual_23DB = SessionManager_23DB.ObtenerInstancia_23DB().UltimoIdioma_23DB;
            if(!string.IsNullOrEmpty(idiomaActual_23DB))
            {
                IdiomaBLL_23DB idiomaBLL_23DB = new IdiomaBLL_23DB();
                ActualizarIdioma_23DB(idiomaBLL_23DB.CargarConfiguracion_23DB(idiomaActual_23DB));
            }
        }

        private List<T> Ordenar_62FD<T>(List<T> lista_62FD, string propiedad_62FD, bool ascendente_62FD)
        {
            System.Reflection.PropertyInfo prop_62FD = typeof(T).GetProperty(propiedad_62FD);
            if (prop_62FD == null) return lista_62FD;
            return ascendente_62FD
                ? lista_62FD.OrderBy(x_62FD => prop_62FD.GetValue(x_62FD, null)).ToList()
                : lista_62FD.OrderByDescending(x_62FD => prop_62FD.GetValue(x_62FD, null)).ToList();
        }

        private void ConfigurarGrilla_62FD(DataGridView grilla_62FD)
        {
            // Comportamiento
            grilla_62FD.ReadOnly = true;
            grilla_62FD.AllowUserToAddRows = false;
            grilla_62FD.AllowUserToDeleteRows = false;
            grilla_62FD.AllowUserToResizeRows = false;
            grilla_62FD.AllowUserToOrderColumns = false;
            grilla_62FD.MultiSelect = false;
            grilla_62FD.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grilla_62FD.StandardTab = true;
            grilla_62FD.RowHeadersVisible = false;
            grilla_62FD.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Aspecto general
            grilla_62FD.BackgroundColor = Color.White;
            grilla_62FD.BorderStyle = BorderStyle.None;
            grilla_62FD.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            grilla_62FD.GridColor = Color.FromArgb(220, 220, 220);
            grilla_62FD.RowTemplate.Height = 26;

            // Encabezados (mismo azul que el certificado PDF)
            grilla_62FD.EnableHeadersVisualStyles = false;
            grilla_62FD.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            grilla_62FD.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            grilla_62FD.ColumnHeadersHeight = 32;
            grilla_62FD.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(31, 78, 121);
            grilla_62FD.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            grilla_62FD.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(31, 78, 121);
            grilla_62FD.ColumnHeadersDefaultCellStyle.Font = new Font(grilla_62FD.Font, FontStyle.Bold);
            grilla_62FD.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.False;

            // Filas
            grilla_62FD.DefaultCellStyle.Padding = new Padding(4, 0, 4, 0);
            grilla_62FD.DefaultCellStyle.SelectionBackColor = Color.FromArgb(204, 224, 245);
            grilla_62FD.DefaultCellStyle.SelectionForeColor = Color.Black;
            grilla_62FD.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(242, 246, 250);

            foreach (DataGridViewColumn columna_62FD in grilla_62FD.Columns)
            {
                columna_62FD.SortMode = DataGridViewColumnSortMode.Programmatic;
                columna_62FD.MinimumWidth = 60;
            }
        }

        private void LimpiarSeleccionGrilla_62FD(DataGridView grilla_62FD)
        {
            grilla_62FD.ClearSelection();
            grilla_62FD.CurrentCell = null;
            grilla_62FD.Invalidate();
        }

        private void Grilla_Paint_62FD(object sender, PaintEventArgs e)
        {
            DataGridView grilla_62FD = (DataGridView)sender;
            if (grilla_62FD.Rows.Count > 0) return;

            Rectangle area_62FD = new Rectangle(0, grilla_62FD.ColumnHeadersHeight,
                grilla_62FD.ClientSize.Width, grilla_62FD.ClientSize.Height - grilla_62FD.ColumnHeadersHeight);
            TextRenderer.DrawText(e.Graphics, IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_GrillaSinDatos"),
                grilla_62FD.Font, area_62FD, Color.Gray,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.WordBreak);
        }
    }
}
