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
        }

        private EventoBLL_23DB eventoBLL_23DB = new EventoBLL_23DB();

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
            dgvBitacoraEventos.DataSource = lista_23DB;
        }


        private void btnImprimir_Click(object sender, EventArgs e)
        {
            if(dgvBitacoraEventos.Rows.Count == 0)
            {
                MessageBox.Show("No hay datos para imprimir.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

                    MessageBox.Show("PDF generado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    System.Diagnostics.Process.Start(saveDialog_23DB.FileName);
                }
                catch(Exception ex_23DB)
                {
                    MessageBox.Show("Error al generar el PDF: " + ex_23DB.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnAplicar_Click(object sender, EventArgs e)
        {
            if(dtpFechaInicio.Value > dtpFechaFin.Value)
            {
                MessageBox.Show("La fecha de inicio no puede ser mayor a la fecha fin.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if(dtpFechaFin.Value > DateTime.Now.Date)
            {
                MessageBox.Show("La fecha fin no puede ser posterior a la fecha actual.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string dni_23DB = cmbLogin.SelectedIndex >= 0 ? cmbLogin.SelectedValue.ToString() : string.Empty;
            string modulo_23DB = cmbModulo.SelectedIndex >= 0 ? cmbModulo.SelectedItem.ToString() : string.Empty;
            string evento_23DB = cmbEvento.SelectedIndex >= 0 ? cmbEvento.SelectedItem.ToString() : string.Empty;
            int criticidad_23DB = cmbCriticidad.SelectedIndex >= 0 ? cmbCriticidad.SelectedIndex + 1 : 0;

            List<Evento_23DB> lista_23DB = eventoBLL_23DB.FiltrarEventos_23DB(dni_23DB, dtpFechaInicio.Value.Date, dtpFechaFin.Value.Date, modulo_23DB, evento_23DB, criticidad_23DB);

            dgvBitacoraEventos.DataSource = lista_23DB;

            if(lista_23DB.Count == 0)
            { 
                MessageBox.Show("No se encontraron eventos con los criterios ingresados.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information); 
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

        private bool actualizandoFecha_23DB = false;

        private void dtpFechaInicio_ValueChanged(object sender, EventArgs e)
        {
         
        }

        private void dtpFechaFin_ValueChanged(object sender, EventArgs e)
        {
    
        }

        public void ActualizarIdioma_23DB(Dictionary<string, string> configuracion_23DB)
        {
            string formName_23DB = "AuditoriaEventos";
            foreach(Control control_23DB in ObtenerTodosControles_23DB(this))
            {
                string clave_23DB = formName_23DB + "_" + control_23DB.Name;
                if(configuracion_23DB.ContainsKey(clave_23DB))
                {
                    control_23DB.Text = configuracion_23DB[clave_23DB];
                }                    
            }
        }

        private List<Control> ObtenerTodosControles_23DB(Control control_23DB)
        {
            List<Control> lista_23DB = new List<Control>();
            foreach(Control c_23DB in control_23DB.Controls)
            {
                lista_23DB.Add(c_23DB);
                lista_23DB.AddRange(ObtenerTodosControles_23DB(c_23DB));
            }
            return lista_23DB;
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
    }
}
