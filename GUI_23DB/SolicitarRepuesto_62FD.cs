using BE_23DB;
using BLL_23DB;
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
    public partial class SolicitarRepuesto_62FD : Form, IIdiomaObserver_23DB
    {
        RepuestoBLL_62FD repuestoBLL_62FD = new RepuestoBLL_62FD();
        EventoBLL_23DB eventoBLL_23DB = new EventoBLL_23DB();

        private Tarea_62FD tarea_62FD = null;
        private string matricula_62FD = "";
        private List<Repuesto_62FD> listaNombres_62FD = new List<Repuesto_62FD>();
        private List<Repuesto_62FD> listaRepuestos_62FD = new List<Repuesto_62FD>();
        private string columnaOrden_62FD = null;
        private bool ordenAscendente_62FD = true;
        private string claveModo_62FD = null;
        private DateTime fechaModo_62FD = DateTime.Now;
        public SolicitarRepuesto_62FD(Tarea_62FD tareaSeleccionada_62FD, string matriculaAeronave_62FD)
        {
            InitializeComponent();
            Observer_23DB.ObtenerInstancia_23DB().Suscribir_23DB(this);
            AplicarIdiomaActual_23DB();

            tarea_62FD = tareaSeleccionada_62FD;
            matricula_62FD = matriculaAeronave_62FD;

            dataGridView1.AutoGenerateColumns = false;
            ConfigurarControles_62FD();
        }

        private void ConfigurarControles_62FD()
        {
            // cmbNombreRepuesto queda editable a propósito: tiene que admitir repuestos que todavía no están en el catálogo
            cmbNombreRepuesto.MaxLength = 100;
            txtNroParte.MaxLength = 50;
            cmbNombreRepuesto.TextUpdate += cmbNombreRepuesto_TextUpdate;

            numericUpDown1.Maximum = 999;
            numericUpDown1.Minimum = 1;

            ConfigurarGrilla_62FD(dataGridView1);

            colNroSolicitud.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            colNroSolicitud.FillWeight = 50;
            colCantidad.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            colCantidad.FillWeight = 50;
            colEstadoPedido.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colFechaSolicitud.DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
            colFechaSolicitud.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colFechaRecepcion.DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
            colFechaRecepcion.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colFechaRecepcion.DefaultCellStyle.NullValue = "-";

            dataGridView1.Paint += Grilla_Paint_62FD;
            dataGridView1.ColumnHeaderMouseClick += dataGridView1_ColumnHeaderMouseClick;
            this.Shown += (s_62FD, e_62FD) => LimpiarSeleccionGrilla_62FD(dataGridView1);

            this.AcceptButton = btnAplicar;
            this.CancelButton = btnSalir;
        }

        public void ActualizarIdioma_23DB(Dictionary<string, string> configuracion_23DB)
        {
            Traductor_23DB.AplicarIdioma_23DB(this, configuracion_23DB, statusStrip1);
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

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void SolicitarRepuesto_62FD_Load(object sender, EventArgs e)
        {
            txtNroTarea.Text = tarea_62FD.IdTarea_62FD.ToString();
            txtDescripcion.Text = tarea_62FD.Descripcion_62FD;
            txtNroIntervencion.Text = tarea_62FD.IdIntervencion_62FD.ToString();
            txtMatricula.Text = matricula_62FD;

            txtNroTarea.ReadOnly = true;
            txtDescripcion.ReadOnly = true;
            txtNroIntervencion.ReadOnly = true;
            txtMatricula.ReadOnly = true;

            CargarNombres_62FD();
            CargarGrilla_62FD();
            ActualizarBarraEstado_62FD("Msg_NombreModoRegistro");
            this.ActiveControl = cmbNombreRepuesto;
        }

        private void CargarNombres_62FD()
        {
            try
            {
                listaNombres_62FD = repuestoBLL_62FD.ObtenerNombres_62FD();

                cmbNombreRepuesto.Items.Clear();

                foreach (Repuesto_62FD r_62FD in listaNombres_62FD)
                {
                    cmbNombreRepuesto.Items.Add(r_62FD.nombreRepuesto_62FD);
                }

                cmbNombreRepuesto.SelectedIndex = -1;
            }
            catch (Exception ex_62FD)
            {
                MessageBox.Show(string.Format(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_ErrorRecuperarRepuestos"), ex_62FD.Message), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cmbNombreRepuesto_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbNombreRepuesto.SelectedIndex >= 0 && cmbNombreRepuesto.SelectedIndex < listaNombres_62FD.Count)
            {
                txtNroParte.Text = listaNombres_62FD[cmbNombreRepuesto.SelectedIndex].nroParte_62FD;
            }
        }

        // Si el usuario edita a mano el nombre, el nro de parte que trajo la selección anterior deja de corresponder
        private void cmbNombreRepuesto_TextUpdate(object sender, EventArgs e)
        {
            int indice_62FD = cmbNombreRepuesto.SelectedIndex;

            if (indice_62FD < 0 || indice_62FD >= listaNombres_62FD.Count || cmbNombreRepuesto.Text != listaNombres_62FD[indice_62FD].nombreRepuesto_62FD)
            {
                txtNroParte.Clear();
            }
        }

        private void CargarGrilla_62FD()
        {
            try
            {
                listaRepuestos_62FD = repuestoBLL_62FD.ObtenerRepuestosPorTarea_62FD(tarea_62FD.IdTarea_62FD);
                columnaOrden_62FD = null;

                dataGridView1.DataSource = null;
                dataGridView1.DataSource = listaRepuestos_62FD;
                Traductor_23DB.TraducirGrilla_23DB(dataGridView1, "SolicitarRepuesto");
                LimpiarSeleccionGrilla_62FD(dataGridView1);
            }
            catch (Exception ex_62FD)
            {
                MessageBox.Show(string.Format(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_ErrorRecuperarRepuestos"), ex_62FD.Message), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAplicar_Click(object sender, EventArgs e)
        {
            // Evita que un doble clic registre la solicitud dos veces
            btnAplicar.Enabled = false;

            try
            {
                if (string.IsNullOrWhiteSpace(cmbNombreRepuesto.Text) || string.IsNullOrWhiteSpace(txtNroParte.Text))
                {
                    MessageBox.Show(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_CompletarDatosRepuesto"), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Informacion"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (numericUpDown1.Value <= 0)
                {
                    MessageBox.Show(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_CantidadMayorCero"), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Informacion"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                string nombre_62FD = cmbNombreRepuesto.Text.Trim();
                string nroParte_62FD = txtNroParte.Text.Trim();

                // Si el nombre ya existe en el catálogo, el nro de parte tiene que ser uno de los suyos
                List<Repuesto_62FD> delCatalogo_62FD = listaNombres_62FD.FindAll(r_62FD => string.Equals(r_62FD.nombreRepuesto_62FD, nombre_62FD, StringComparison.OrdinalIgnoreCase));

                if (delCatalogo_62FD.Count > 0 && !delCatalogo_62FD.Exists(r_62FD => string.Equals(r_62FD.nroParte_62FD, nroParte_62FD, StringComparison.OrdinalIgnoreCase)))
                {
                    MessageBox.Show(string.Format(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_NroParteNoCoincide"), nombre_62FD), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Advertencia"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                Repuesto_62FD repuesto_62FD = new Repuesto_62FD();
                repuesto_62FD.IdTarea_62FD = tarea_62FD.IdTarea_62FD;
                repuesto_62FD.nombreRepuesto_62FD = nombre_62FD;
                repuesto_62FD.nroParte_62FD = nroParte_62FD;
                repuesto_62FD.cantidadSolicitada_62FD = Convert.ToInt32(numericUpDown1.Value);

                int nroSolicitud_62FD = repuestoBLL_62FD.SolicitarRepuesto_62FD(repuesto_62FD);

                if (nroSolicitud_62FD < 0)
                {
                    MessageBox.Show(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_DatoModificado"), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Advertencia"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    CargarGrilla_62FD();
                    return;
                }

                RegistrarEventoBitacora_62FD("Solicitar Repuestos", 2);

                MessageBox.Show(string.Format(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_SolicitudRepuestoRegistrada"), nroSolicitud_62FD), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Informacion"), MessageBoxButtons.OK, MessageBoxIcon.Information);

                LimpiarCampos_62FD();
                CargarNombres_62FD();
                CargarGrilla_62FD();
            }
            catch (Exception ex_62FD)
            {
                MessageBox.Show(string.Format(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_ErrorRegistrarSolicitud"), ex_62FD.Message), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnAplicar.Enabled = true;
            }
        }

        private void btnRegistrarRecepcion_Click(object sender, EventArgs e)
        {
            // Evita que un doble clic intente registrar la recepción dos veces
            btnRegistrarRecepcion.Enabled = false;

            try
            {
                if (dataGridView1.SelectedRows.Count == 0 || dataGridView1.SelectedRows[0].DataBoundItem == null)
                {
                    MessageBox.Show(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_SeleccioneSolicitud"), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Informacion"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                Repuesto_62FD repuesto_62FD = (Repuesto_62FD)dataGridView1.SelectedRows[0].DataBoundItem;

                if (repuesto_62FD.estadoPedido_62FD == "Recibido")
                {
                    MessageBox.Show(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_SolicitudYaRecibida"), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Advertencia"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DialogResult confirmacion_62FD = MessageBox.Show(string.Format(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_ConfirmarRecepcion"), repuesto_62FD.nroSolicitud_62FD, repuesto_62FD.nombreRepuesto_62FD, repuesto_62FD.cantidadSolicitada_62FD), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Confirmar"), MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (confirmacion_62FD != DialogResult.Yes)
                {
                    return;
                }

                if (!repuestoBLL_62FD.RegistrarRecepcion_62FD(repuesto_62FD.nroSolicitud_62FD))
                {
                    MessageBox.Show(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_DatoModificado"), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Advertencia"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    CargarGrilla_62FD();
                    return;
                }

                RegistrarEventoBitacora_62FD("Registrar Recepción de Repuestos", 2);
                MessageBox.Show(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_RecepcionRegistrada"), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Informacion"), MessageBoxButtons.OK, MessageBoxIcon.Information);

                CargarGrilla_62FD();
            }
            catch (Exception ex_62FD)
            {
                MessageBox.Show(string.Format(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_ErrorRegistrarRecepcion"), ex_62FD.Message), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnRegistrarRecepcion.Enabled = true;
            }
        }

        // La Bitácora va en su propio try/catch: si falla, la operación de negocio ya está hecha y no se informa como error
        private void RegistrarEventoBitacora_62FD(string evento_62FD, int criticidad_62FD)
        {
            try
            {
                eventoBLL_23DB.RegistrarEvento_23DB(SessionManager_23DB.ObtenerInstancia_23DB().DNI_23DB, "Mantenimiento", evento_62FD, criticidad_62FD);
            }
            catch (Exception ex_62FD)
            {
                MessageBox.Show(string.Format(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_OperacionSinBitacora"), ex_62FD.Message), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Advertencia"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            LimpiarCampos_62FD();
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void LimpiarCampos_62FD()
        {
            cmbNombreRepuesto.SelectedIndex = -1;
            cmbNombreRepuesto.Text = "";
            txtNroParte.Clear();
            numericUpDown1.Value = numericUpDown1.Minimum;
        }

        private void dataGridView1_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridViewColumn columna_62FD = dataGridView1.Columns[e.ColumnIndex];
            ordenAscendente_62FD = columnaOrden_62FD == columna_62FD.DataPropertyName ? !ordenAscendente_62FD : true;
            columnaOrden_62FD = columna_62FD.DataPropertyName;

            listaRepuestos_62FD = Ordenar_62FD(listaRepuestos_62FD, columnaOrden_62FD, ordenAscendente_62FD);
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = listaRepuestos_62FD;
            Traductor_23DB.TraducirGrilla_23DB(dataGridView1, "SolicitarRepuesto");
            LimpiarSeleccionGrilla_62FD(dataGridView1);

            // El glifo se pierde al reenlazar, por eso se pone después
            dataGridView1.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection = ordenAscendente_62FD ? SortOrder.Ascending : SortOrder.Descending;
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

        private void ActualizarBarraEstado_62FD(string claveModo_62FD)
        {
            this.claveModo_62FD = claveModo_62FD;
            fechaModo_62FD = DateTime.Now;
            MostrarTextosEstado_62FD();
        }

        private void MostrarTextosEstado_62FD()
        {
            if (claveModo_62FD == null) return;

            toolStripStatusLabel1.Text = string.Format(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_BarraModo"), IdiomaBLL_23DB.ObtenerTexto_23DB(claveModo_62FD));
            toolStripStatusLabel2.Text = string.Format(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_BarraUsuario"), SessionManager_23DB.ObtenerInstancia_23DB().Login_23DB);
            toolStripStatusLabel3.Text = string.Format(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_BarraFecha"), fechaModo_62FD.ToString("dd/MM/yyyy HH:mm"));
        }

        private void SolicitarRepuesto_62FD_FormClosing(object sender, FormClosingEventArgs e)
        {
            Observer_23DB.ObtenerInstancia_23DB().Desuscribir_23DB(this);

        }
    }
}
