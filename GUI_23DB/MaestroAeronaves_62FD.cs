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
    public partial class MaestroAeronaves_62FD : Form, IIdiomaObserver_23DB
    {
        AeronaveBLL_62FD aeronaveBLL_62FD = new AeronaveBLL_62FD();
        EventoBLL_23DB eventoBLL_23DB = new EventoBLL_23DB();

        // modoActual_62FD se compara en el código: sus valores quedan literales y solo se traduce lo que muestra la barra de estado
        private string modoActual_62FD = "Consulta";
        private Aeronave_62FD aeronaveSeleccionada_62FD = null;
        private List<Aeronave_62FD> listaAeronaves_62FD = new List<Aeronave_62FD>();
        private string columnaOrden_62FD = null;
        private bool ordenAscendente_62FD = true;
        private DateTime fechaModo_62FD = DateTime.Now;
        public MaestroAeronaves_62FD()
        {
            InitializeComponent();
            Observer_23DB.ObtenerInstancia_23DB().Suscribir_23DB(this);
            AplicarIdiomaActual_23DB();

            dataGridView1.AutoGenerateColumns = false;
            ConfigurarControles_62FD();
        }

        private void ConfigurarControles_62FD()
        {
            // Los valores del combo se guardan en la base y se comparan en el PN2: no se traducen
            cmbEstadoOperativo.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbEstadoOperativo.Items.Clear();
            cmbEstadoOperativo.Items.Add("En Servicio");
            cmbEstadoOperativo.Items.Add("En Mantenimiento");
            cmbEstadoOperativo.Items.Add("Fuera de Servicio");

            // Largos iguales a las columnas de Aeronave_62FD; los numéricos alcanzan para un int
            txtMatricula.MaxLength = 20;
            txtNroSerie.MaxLength = 50;
            txtModelo.MaxLength = 50;
            txtFabricante.MaxLength = 50;
            txtHorasVueloAcumuladas.MaxLength = 9;
            txtIntervaloProxRevision.MaxLength = 9;

            // Los campos numéricos no aceptan letras al tipear; lo pegado lo sigue frenando la validación
            txtHorasVueloAcumuladas.KeyPress += SoloDigitos_62FD;
            txtIntervaloProxRevision.KeyPress += SoloDigitos_62FD;

            ConfigurarGrilla_62FD(dataGridView1);

            colMatricula.FillWeight = 80;
            colNroSerie.FillWeight = 80;
            colEstadoOperativo.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colHorasVuelo.DefaultCellStyle.Format = "N0";
            colHorasVuelo.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            colIntervaloRevision.DefaultCellStyle.Format = "N0";
            colIntervaloRevision.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            colIntervaloRevision.FillWeight = 50;
            colProximaRevision.DefaultCellStyle.Format = "dd/MM/yyyy";
            colProximaRevision.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colActivo.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colActivo.FillWeight = 50;

            dataGridView1.Paint += Grilla_Paint_62FD;
            dataGridView1.ColumnHeaderMouseClick += dataGridView1_ColumnHeaderMouseClick;
            dataGridView1.CellFormatting += Grilla_CellFormatting_62FD;
            this.Shown += (s_62FD, e_62FD) => LimpiarSeleccionGrilla_62FD(dataGridView1);

            this.CancelButton = btnSalir;
        }

        public void ActualizarIdioma_23DB(Dictionary<string, string> configuracion_23DB)
        {
            Traductor_23DB.AplicarIdioma_23DB(this, configuracion_23DB, statusStrip1);
            // Va después del traductor: si no, el texto fijo del JSON pisa el que corresponde al modo
            ActualizarTextoBotonBaja_62FD();
            MostrarTextosEstado_62FD();
        }

        // Con las inactivas a la vista el botón reactiva; con las activas, da de baja
        private void ActualizarTextoBotonBaja_62FD()
        {
            btnEliminar.Text = IdiomaBLL_23DB.ObtenerTexto_23DB(checkBox1.Checked ? "Msg_ReactivarAeronave" : "MaestroAeronaves_btnEliminar");
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

        private void btnBitacoraCambios_Click(object sender, EventArgs e)
        {
            AuditoriaDeCambios_62FD auditoriaCambios_62FD = new AuditoriaDeCambios_62FD();
            auditoriaCambios_62FD.ShowDialog();
        }

        private void MaestroAeronaves_62FD_FormClosing(object sender, FormClosingEventArgs e)
        {
            Observer_23DB.ObtenerInstancia_23DB().Desuscribir_23DB(this);

        }

        private void MaestroAeronaves_62FD_Load(object sender, EventArgs e)
        {
            CargarGrilla_62FD();
            ModoConsulta_62FD();
        }

        private Aeronave_62FD ObtenerSeleccionada_62FD()
        {
            if (dataGridView1.SelectedRows.Count == 0 || dataGridView1.SelectedRows[0].DataBoundItem == null)
            {
                MessageBox.Show(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_SeleccioneAeronave"), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Informacion"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return null;
            }

            return (Aeronave_62FD)dataGridView1.SelectedRows[0].DataBoundItem;
        }

        private void ModoConsulta_62FD()
        {
            modoActual_62FD = "Consulta";

            groupBox1.Enabled = false;
            btnAplicar.Enabled = false;
            btnCancelar.Enabled = false;

            btnActualizar.Enabled = true;
            btnAñadir.Enabled = true;
            btnModificar.Enabled = true;
            btnEliminar.Enabled = true;
            dataGridView1.Enabled = true;

            ActualizarBarraEstado_62FD();
        }

        private void CargarGrilla_62FD()
        {
            try
            {
                List<Aeronave_62FD> lista_62FD = aeronaveBLL_62FD.ObtenerTodas_62FD();

                if (checkBox1.Checked)
                {
                    lista_62FD = lista_62FD.FindAll(a_62FD => !a_62FD.Activo_62FD);
                }
                else
                {
                    lista_62FD = lista_62FD.FindAll(a_62FD => a_62FD.Activo_62FD);
                }

                listaAeronaves_62FD = lista_62FD;
                columnaOrden_62FD = null;

                dataGridView1.DataSource = null;
                dataGridView1.DataSource = listaAeronaves_62FD;
                Traductor_23DB.TraducirGrilla_23DB(dataGridView1, "MaestroAeronaves");
                LimpiarSeleccionGrilla_62FD(dataGridView1);
            }
            catch (Exception ex_62FD)
            {
                MessageBox.Show(string.Format(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_ErrorRecuperarAeronaves"), ex_62FD.Message), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            CargarGrilla_62FD();
            ModoConsulta_62FD();
        }

        private void btnAñadir_Click(object sender, EventArgs e)
        {
            LimpiarCampos_62FD();
            ModoAlta_62FD();
        }

        private void ModoAlta_62FD()
        {
            modoActual_62FD = "Alta";

            groupBox1.Enabled = true;
            txtMatricula.ReadOnly = false;
            btnAplicar.Enabled = true;
            btnCancelar.Enabled = true;

            btnActualizar.Enabled = false;
            btnAñadir.Enabled = false;
            btnModificar.Enabled = false;
            btnEliminar.Enabled = false;
            dataGridView1.Enabled = false;

            ActualizarBarraEstado_62FD();
            txtMatricula.Focus();
        }

        private void LimpiarCampos_62FD()
        {
            aeronaveSeleccionada_62FD = null;

            txtMatricula.Clear();
            txtNroSerie.Clear();
            txtModelo.Clear();
            txtFabricante.Clear();
            txtHorasVueloAcumuladas.Clear();
            txtIntervaloProxRevision.Clear();
            cmbEstadoOperativo.SelectedIndex = -1;
            dateTimePicker1.Value = DateTime.Today;
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            aeronaveSeleccionada_62FD = ObtenerSeleccionada_62FD();

            if (aeronaveSeleccionada_62FD == null)
            {
                return;
            }

            txtMatricula.Text = aeronaveSeleccionada_62FD.Matricula_62FD;
            txtNroSerie.Text = aeronaveSeleccionada_62FD.NroSerie_62FD;
            txtModelo.Text = aeronaveSeleccionada_62FD.Modelo_62FD;
            txtFabricante.Text = aeronaveSeleccionada_62FD.Fabricante_62FD;
            cmbEstadoOperativo.Text = aeronaveSeleccionada_62FD.EstadoOperativo_62FD;
            txtHorasVueloAcumuladas.Text = aeronaveSeleccionada_62FD.HorasVueloAcumuladas_62FD.ToString();
            txtIntervaloProxRevision.Text = aeronaveSeleccionada_62FD.IntervaloRevision_62FD.ToString();
            dateTimePicker1.Value = aeronaveSeleccionada_62FD.FechaProximaRevision_62FD;

            ModoModificacion_62FD();
        }

        private void ModoModificacion_62FD()
        {
            modoActual_62FD = "Modificación";

            groupBox1.Enabled = true;
            txtMatricula.ReadOnly = true;
            btnAplicar.Enabled = true;
            btnCancelar.Enabled = true;

            btnActualizar.Enabled = false;
            btnAñadir.Enabled = false;
            btnModificar.Enabled = false;
            btnEliminar.Enabled = false;
            dataGridView1.Enabled = false;

            ActualizarBarraEstado_62FD();
            txtNroSerie.Focus();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                Aeronave_62FD aeronave_62FD = ObtenerSeleccionada_62FD();

                if (aeronave_62FD == null)
                {
                    return;
                }

                bool nuevoEstado_62FD = !aeronave_62FD.Activo_62FD;
                string claveConfirmacion_62FD = nuevoEstado_62FD ? "Msg_ConfirmarReactivarAeronave" : "Msg_ConfirmarBajaAeronave";

                if (MessageBox.Show(string.Format(IdiomaBLL_23DB.ObtenerTexto_23DB(claveConfirmacion_62FD), aeronave_62FD.Matricula_62FD), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Confirmar"), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    return;
                }

                int resultado_62FD = aeronaveBLL_62FD.CambiarEstadoAlta_62FD(aeronave_62FD.Matricula_62FD, nuevoEstado_62FD);

                if (resultado_62FD == 0)
                {
                    MessageBox.Show(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_AeronaveConOrdenAbierta"), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Advertencia"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (resultado_62FD < 0)
                {
                    MessageBox.Show(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_NoSePudoCambiarEstadoAeronave"), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                RegistrarEventoBitacora_62FD(nuevoEstado_62FD ? "Reactivar Aeronave" : "Dar de Baja Aeronave");

                CargarGrilla_62FD();
                ModoConsulta_62FD();
            }
            catch (Exception ex_62FD)
            {
                MessageBox.Show(string.Format(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_ErrorCambiarEstadoAeronave"), ex_62FD.Message), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAplicar_Click(object sender, EventArgs e)
        {
            try
            {
                btnAplicar.Enabled = false;

                if (!ValidarCampos_62FD())
                {
                    return;
                }

                Aeronave_62FD aeronave_62FD = new Aeronave_62FD();
                aeronave_62FD.Matricula_62FD = txtMatricula.Text.Trim();
                aeronave_62FD.NroSerie_62FD = txtNroSerie.Text.Trim();
                aeronave_62FD.Modelo_62FD = txtModelo.Text.Trim();
                aeronave_62FD.Fabricante_62FD = txtFabricante.Text.Trim();
                aeronave_62FD.EstadoOperativo_62FD = cmbEstadoOperativo.Text;
                aeronave_62FD.HorasVueloAcumuladas_62FD = Convert.ToInt32(txtHorasVueloAcumuladas.Text.Trim());
                aeronave_62FD.IntervaloRevision_62FD = Convert.ToInt32(txtIntervaloProxRevision.Text.Trim());
                aeronave_62FD.FechaProximaRevision_62FD = dateTimePicker1.Value.Date;

                if (modoActual_62FD == "Alta")
                {
                    int resultado_62FD = aeronaveBLL_62FD.CrearAeronave_62FD(aeronave_62FD);

                    if (resultado_62FD == 0)
                    {
                        MessageBox.Show(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_AeronaveMatriculaExistente"), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Advertencia"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    if (resultado_62FD < 0)
                    {
                        MessageBox.Show(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_NoSePudoRegistrarAeronave"), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    RegistrarEventoBitacora_62FD("Crear Aeronave");
                    MessageBox.Show(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_AeronaveRegistrada"), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Informacion"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    if (!aeronaveBLL_62FD.ModificarAeronave_62FD(aeronave_62FD))
                    {
                        MessageBox.Show(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_NoSePudoModificarAeronave"), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    RegistrarEventoBitacora_62FD("Modificar Aeronave");
                    MessageBox.Show(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_AeronaveModificada"), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Informacion"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                CargarGrilla_62FD();
                ModoConsulta_62FD();
            }
            catch (Exception ex_62FD)
            {
                MessageBox.Show(string.Format(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_ErrorGuardarAeronave"), ex_62FD.Message), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnAplicar.Enabled = modoActual_62FD != "Consulta";
            }
        }

        private bool ValidarCampos_62FD()
        {
            if (string.IsNullOrWhiteSpace(txtMatricula.Text) || string.IsNullOrWhiteSpace(txtNroSerie.Text) || string.IsNullOrWhiteSpace(txtModelo.Text) || string.IsNullOrWhiteSpace(txtFabricante.Text) || cmbEstadoOperativo.SelectedIndex < 0)
            {
                MessageBox.Show(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_CompletarDatosAeronave"), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Informacion"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            int horas_62FD;

            if (!int.TryParse(txtHorasVueloAcumuladas.Text.Trim(), out horas_62FD) || horas_62FD < 0)
            {
                MessageBox.Show(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_HorasVueloInvalidas"), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Informacion"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            int intervalo_62FD;

            if (!int.TryParse(txtIntervaloProxRevision.Text.Trim(), out intervalo_62FD) || intervalo_62FD <= 0)
            {
                MessageBox.Show(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_IntervaloRevisionInvalido"), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Informacion"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            return true;
        }

        private void SoloDigitos_62FD(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void RegistrarEventoBitacora_62FD(string evento_62FD)
        {
            try
            {
                eventoBLL_23DB.RegistrarEvento_23DB(SessionManager_23DB.ObtenerInstancia_23DB().DNI_23DB, "Maestros", evento_62FD, 2);
            }
            catch (Exception ex_62FD)
            {
                MessageBox.Show(string.Format(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_OperacionSinBitacora"), ex_62FD.Message), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Advertencia"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void ActualizarBarraEstado_62FD()
        {
            fechaModo_62FD = DateTime.Now;
            MostrarTextosEstado_62FD();
        }

        private void MostrarTextosEstado_62FD()
        {
            string claveModo_62FD = "Msg_NombreModoConsulta";

            if (modoActual_62FD == "Alta")
            {
                claveModo_62FD = "Msg_NombreModoAlta";
            }
            else if (modoActual_62FD == "Modificación")
            {
                claveModo_62FD = "Msg_NombreModoModificacion";
            }

            toolStripStatusLabel1.Text = string.Format(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_BarraModo"), IdiomaBLL_23DB.ObtenerTexto_23DB(claveModo_62FD));
            toolStripStatusLabel2.Text = string.Format(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_BarraUsuario"), SessionManager_23DB.ObtenerInstancia_23DB().Login_23DB);
            toolStripStatusLabel3.Text = string.Format(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_BarraFecha"), fechaModo_62FD.ToString("dd/MM/yyyy HH:mm"));
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            LimpiarCampos_62FD();
            ModoConsulta_62FD();
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            ActualizarTextoBotonBaja_62FD();
            CargarGrilla_62FD();
        }

        private void dataGridView1_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridViewColumn columna_62FD = dataGridView1.Columns[e.ColumnIndex];
            ordenAscendente_62FD = columnaOrden_62FD == columna_62FD.DataPropertyName ? !ordenAscendente_62FD : true;
            columnaOrden_62FD = columna_62FD.DataPropertyName;

            listaAeronaves_62FD = Ordenar_62FD(listaAeronaves_62FD, columnaOrden_62FD, ordenAscendente_62FD);
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = listaAeronaves_62FD;
            Traductor_23DB.TraducirGrilla_23DB(dataGridView1, "MaestroAeronaves");
            LimpiarSeleccionGrilla_62FD(dataGridView1);

            // El glifo se pierde al reenlazar, por eso se pone después
            dataGridView1.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection = ordenAscendente_62FD ? SortOrder.Ascending : SortOrder.Descending;
        }

        // Muestra los bool como Sí / No traducidos en lugar de True / False
        private void Grilla_CellFormatting_62FD(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.Value is bool)
            {
                e.Value = IdiomaBLL_23DB.ObtenerTexto_23DB((bool)e.Value ? "Msg_Si" : "Msg_No");
                e.FormattingApplied = true;
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
