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
    public partial class RegistrarIntervencion_62FD : Form, IIdiomaObserver_23DB
    {
        IntervencionBLL_62FD intervencionBLL_62FD = new IntervencionBLL_62FD();
        EventoBLL_23DB eventoBLL_23DB = new EventoBLL_23DB();

        private List<OrdenTrabajo_62FD> listaOrdenes_62FD = new List<OrdenTrabajo_62FD>();
        private List<Tarea_62FD> listaTareas_62FD = new List<Tarea_62FD>();
        private Intervencion_62FD intervencion_62FD = null;
        private OrdenTrabajo_62FD orden_62FD = null;
        private string columnaOrden_62FD = null;
        private bool ordenAscendente_62FD = true;
        private string claveModo_62FD = null;
        private DateTime fechaModo_62FD = DateTime.Now;
        public RegistrarIntervencion_62FD()
        {
            InitializeComponent();
            Observer_23DB.ObtenerInstancia_23DB().Suscribir_23DB(this);
            AplicarIdiomaActual_23DB();
            dataGridView1.AutoGenerateColumns = false;
            ConfigurarControles_62FD();
        }

        private void ConfigurarControles_62FD()
        {
            cmbNroOrden.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbResultadoObtenido.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbEstadoIntervencion.DropDownStyle = ComboBoxStyle.DropDownList;

            txtDescripcion.MaxLength = 500;
            txtObservaciones.MaxLength = 500;
            // Es multilínea: Enter tiene que hacer salto de línea y no disparar el botón por defecto
            txtObservaciones.AcceptsReturn = true;

            // Horas con decimales; el máximo respeta decimal(10,2) de la columna
            nudHorasInsumidas.DecimalPlaces = 2;
            nudHorasInsumidas.Increment = 0.25m;
            nudHorasInsumidas.Maximum = 999.99m;
            nudHorasInsumidas.Minimum = 0.25m;

            ConfigurarGrilla_62FD(dataGridView1);

            colIdTarea.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            colIdTarea.FillWeight = 50;
            colDescripcionTarea.FillWeight = 200;
            colObservacionesTarea.FillWeight = 200;
            colHorasTarea.DefaultCellStyle.Format = "N2";
            colHorasTarea.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            colRepuestos.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dataGridView1.Paint += Grilla_Paint_62FD;
            dataGridView1.ColumnHeaderMouseClick += dataGridView1_ColumnHeaderMouseClick;
            this.Shown += (s_62FD, e_62FD) => LimpiarSeleccionGrilla_62FD(dataGridView1);

            this.CancelButton = btnSalir;
        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }

        private void RegistrarIntervencion_62FD_Load(object sender, EventArgs e)
        {
            CargarCombos_62FD();
            CargarOrdenes_62FD();
            ModoConsulta_62FD();
            this.ActiveControl = cmbNroOrden;
        }

        private void CargarCombos_62FD()
        {
            cmbResultadoObtenido.Items.Clear();
            cmbResultadoObtenido.Items.Add("Conforme");
            cmbResultadoObtenido.Items.Add("No Conforme");
            cmbResultadoObtenido.Items.Add("Pendiente");
            cmbResultadoObtenido.SelectedIndex = -1;

            cmbEstadoIntervencion.Items.Clear();
            cmbEstadoIntervencion.Items.Add("En Curso");
            cmbEstadoIntervencion.Items.Add("Finalizada");
            cmbEstadoIntervencion.SelectedIndex = -1;
            cmbEstadoIntervencion.Enabled = false;

            dtpFechaInicio.Enabled = false;

            txtMatricula.ReadOnly = true;
            txtTipoIntervencion.ReadOnly = true;
            txtEstadoOrden.ReadOnly = true;
            txtFechaLimite.ReadOnly = true;
            txtHorasInsumidas.ReadOnly = true;
        }

        private void CargarOrdenes_62FD()
        {
            try
            {
                listaOrdenes_62FD = intervencionBLL_62FD.ObtenerOrdenesAsignadas_62FD(SessionManager_23DB.ObtenerInstancia_23DB().DNI_23DB);

                cmbNroOrden.Items.Clear();

                foreach (OrdenTrabajo_62FD o_62FD in listaOrdenes_62FD)
                {
                    cmbNroOrden.Items.Add(o_62FD.IdOrden_62FD + " - " + o_62FD.Matricula_62FD + " (" + o_62FD.TipoIntervencion_62FD + ")");
                }

                cmbNroOrden.SelectedIndex = -1;

                if (listaOrdenes_62FD.Count == 0)
                {
                    MessageBox.Show(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_SinOrdenesAsignadas"), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Informacion"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex_62FD)
            {
                MessageBox.Show(string.Format(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_ErrorRecuperarOrdenes"), ex_62FD.Message), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbNroOrden.SelectedIndex < 0)
                {
                    MessageBox.Show(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_SeleccioneOrden"), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Informacion"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                OrdenTrabajo_62FD ordenElegida_62FD = listaOrdenes_62FD[cmbNroOrden.SelectedIndex];

                // Solo se confirma cuando se va a crear una intervención nueva; retomar una En Curso no escribe nada
                if (intervencionBLL_62FD.ObtenerIntervencionEnCurso_62FD(ordenElegida_62FD.IdOrden_62FD) == null)
                {
                    DialogResult confirmacion_62FD = MessageBox.Show(string.Format(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_ConfirmarIniciarIntervencion"), ordenElegida_62FD.IdOrden_62FD, ordenElegida_62FD.Matricula_62FD), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Confirmar"), MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (confirmacion_62FD != DialogResult.Yes)
                    {
                        return;
                    }
                }

                Intervencion_62FD intervencionObtenida_62FD;
                int resultado_62FD = intervencionBLL_62FD.IniciarIntervencion_62FD(ordenElegida_62FD.IdOrden_62FD, out intervencionObtenida_62FD);

                if (resultado_62FD == IntervencionBLL_62FD.DatoModificado_62FD || intervencionObtenida_62FD == null)
                {
                    MessageBox.Show(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_DatoModificado"), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Advertencia"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    CargarOrdenes_62FD();
                    ModoConsulta_62FD();
                    return;
                }

                if (resultado_62FD == IntervencionBLL_62FD.AeronaveNoActualizada_62FD)
                {
                    MessageBox.Show(string.Format(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_IntervencionSinActualizarAeronave"), intervencionObtenida_62FD.IdIntervencion_62FD), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Advertencia"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                orden_62FD = ordenElegida_62FD;
                intervencion_62FD = intervencionObtenida_62FD;

                txtMatricula.Text = orden_62FD.Matricula_62FD;
                txtTipoIntervencion.Text = orden_62FD.TipoIntervencion_62FD;
                txtEstadoOrden.Text = orden_62FD.EstadoOrden_62FD;
                txtFechaLimite.Text = orden_62FD.FechaLimite_62FD.ToString("dd/MM/yyyy");

                dtpFechaInicio.Value = intervencion_62FD.fechaHoraInicio_62FD;
                dtpFechaFin.Value = DateTime.Now;
                cmbEstadoIntervencion.Text = intervencion_62FD.estadoIntervencion_62FD;

                CargarGrillaTareas_62FD();
                ModoRegistro_62FD();
            }
            catch (Exception ex_62FD)
            {
                MessageBox.Show(string.Format(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_ErrorIniciarIntervencion"), ex_62FD.Message), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarGrillaTareas_62FD()
        {
            listaTareas_62FD = intervencionBLL_62FD.ObtenerTareas_62FD(intervencion_62FD.IdIntervencion_62FD);
            columnaOrden_62FD = null;

            dataGridView1.DataSource = null;
            dataGridView1.DataSource = listaTareas_62FD;
            Traductor_23DB.TraducirGrilla_23DB(dataGridView1, "RegistrarIntervencion");
            LimpiarSeleccionGrilla_62FD(dataGridView1);

            decimal total_62FD = 0;

            foreach (Tarea_62FD t_62FD in listaTareas_62FD)
            {
                total_62FD += t_62FD.HorasInsumidas_62FD;
            }

            txtHorasInsumidas.Text = total_62FD.ToString("N2");
        }

        private void btnAgregarTarea_Click(object sender, EventArgs e)
        {
            // Evita que un doble clic cargue la tarea dos veces
            btnAgregarTarea.Enabled = false;

            try
            {
                if (string.IsNullOrWhiteSpace(txtDescripcion.Text) || cmbResultadoObtenido.SelectedIndex < 0)
                {
                    MessageBox.Show(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_CompletarDatosTarea"), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Informacion"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (nudHorasInsumidas.Value <= 0)
                {
                    MessageBox.Show(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_HorasMayorCero"), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Informacion"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                Tarea_62FD tarea_62FD = new Tarea_62FD();
                tarea_62FD.IdIntervencion_62FD = intervencion_62FD.IdIntervencion_62FD;
                tarea_62FD.Descripcion_62FD = txtDescripcion.Text.Trim();
                tarea_62FD.HorasInsumidas_62FD = nudHorasInsumidas.Value;
                tarea_62FD.ResultadoObtenido_62FD = cmbResultadoObtenido.Text;
                tarea_62FD.observacionesTecnicas_62FD = txtObservaciones.Text.Trim();

                if (intervencionBLL_62FD.AgregarTarea_62FD(tarea_62FD) < 0)
                {
                    MessageBox.Show(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_DatoModificado"), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Advertencia"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    CargarGrillaTareas_62FD();
                    return;
                }

                LimpiarTarea_62FD();
                CargarGrillaTareas_62FD();
            }
            catch (Exception ex_62FD)
            {
                MessageBox.Show(string.Format(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_ErrorRegistrarTarea"), ex_62FD.Message), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // El botón está dentro de groupBox3: fuera del modo registro lo deshabilita el contenedor
                btnAgregarTarea.Enabled = true;
            }
        }

        private void LimpiarTarea_62FD()
        {
            txtDescripcion.Clear();
            txtObservaciones.Clear();
            nudHorasInsumidas.Value = nudHorasInsumidas.Minimum;
            cmbResultadoObtenido.SelectedIndex = -1;
        }

        private Tarea_62FD ObtenerTareaSeleccionada_62FD()
        {
            if (dataGridView1.SelectedRows.Count == 0 || dataGridView1.SelectedRows[0].DataBoundItem == null)
            {
                MessageBox.Show(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_SeleccioneTarea"), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Informacion"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return null;
            }

            return (Tarea_62FD)dataGridView1.SelectedRows[0].DataBoundItem;
        }

        private void btnQuitar_Click(object sender, EventArgs e)
        {
            try
            {
                Tarea_62FD tarea_62FD = ObtenerTareaSeleccionada_62FD();

                if (tarea_62FD == null)
                {
                    return;
                }

                if (tarea_62FD.EstadoRepuestos_62FD != "-")
                {
                    MessageBox.Show(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_TareaConRepuestos"), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Advertencia"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DialogResult confirmacion_62FD = MessageBox.Show(string.Format(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_ConfirmarQuitarTarea"), tarea_62FD.IdTarea_62FD, tarea_62FD.Descripcion_62FD), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Confirmar"), MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (confirmacion_62FD != DialogResult.Yes)
                {
                    return;
                }

                if (!intervencionBLL_62FD.QuitarTarea_62FD(tarea_62FD.IdTarea_62FD))
                {
                    MessageBox.Show(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_DatoModificado"), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Advertencia"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                CargarGrillaTareas_62FD();
            }
            catch (Exception ex_62FD)
            {
                MessageBox.Show(string.Format(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_ErrorQuitarTarea"), ex_62FD.Message), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                Tarea_62FD tarea_62FD = ObtenerTareaSeleccionada_62FD();

                if (tarea_62FD == null)
                {
                    return;
                }

                SolicitarRepuesto_62FD solicitarRepuesto_62FD = new SolicitarRepuesto_62FD(tarea_62FD, orden_62FD.Matricula_62FD);
                solicitarRepuesto_62FD.ShowDialog();

                CargarGrillaTareas_62FD();
            }
            catch (Exception ex_62FD)
            {
                MessageBox.Show(string.Format(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_ErrorRecuperarTareas"), ex_62FD.Message), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAplicar_Click(object sender, EventArgs e)
        {
            // Evita que un doble clic intente cerrar la intervención dos veces
            btnAplicar.Enabled = false;

            try
            {
                if (listaTareas_62FD.Count == 0)
                {
                    MessageBox.Show(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_RegistreTareaAntesCerrar"), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Informacion"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (dtpFechaFin.Value < intervencion_62FD.fechaHoraInicio_62FD)
                {
                    MessageBox.Show(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_FechaFinAnteriorInicio"), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Informacion"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                DialogResult confirmacion_62FD = MessageBox.Show(string.Format(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_ConfirmarCerrarIntervencion"), intervencion_62FD.IdIntervencion_62FD, orden_62FD.IdOrden_62FD, orden_62FD.Matricula_62FD), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Confirmar"), MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (confirmacion_62FD != DialogResult.Yes)
                {
                    return;
                }

                intervencion_62FD.fechaHoraFin_62FD = dtpFechaFin.Value;

                int resultado_62FD = intervencionBLL_62FD.CerrarIntervencion_62FD(intervencion_62FD);

                if (resultado_62FD == IntervencionBLL_62FD.SinTareas_62FD)
                {
                    MessageBox.Show(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_RegistreTareaAntesCerrar"), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Informacion"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CargarGrillaTareas_62FD();
                    return;
                }

                if (resultado_62FD == IntervencionBLL_62FD.TareasPendientes_62FD)
                {
                    MessageBox.Show(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_CierreTareasPendientes"), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Advertencia"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (resultado_62FD == IntervencionBLL_62FD.RepuestosPendientes_62FD)
                {
                    MessageBox.Show(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_CierreRepuestosPendientes"), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Advertencia"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (resultado_62FD != IntervencionBLL_62FD.IntervencionCerrada_62FD)
                {
                    MessageBox.Show(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_DatoModificado"), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Advertencia"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    CargarOrdenes_62FD();
                    ModoConsulta_62FD();
                    return;
                }

                RegistrarEventoBitacora_62FD("Registrar Intervención", 2);
                MessageBox.Show(string.Format(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_IntervencionRegistrada"), intervencion_62FD.IdIntervencion_62FD, intervencion_62FD.totalHorasInsumidas_62FD.ToString("N2")), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Informacion"), MessageBoxButtons.OK, MessageBoxIcon.Information);

                cmbEstadoIntervencion.Text = "Finalizada";
                txtHorasInsumidas.Text = intervencion_62FD.totalHorasInsumidas_62FD.ToString("N2");

                CargarOrdenes_62FD();
                ModoConsulta_62FD();
            }
            catch (Exception ex_62FD)
            {
                MessageBox.Show(string.Format(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_ErrorCerrarIntervencion"), ex_62FD.Message), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Solo queda habilitado si sigue habiendo una intervención en registro
                btnAplicar.Enabled = intervencion_62FD != null;
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
            MessageBox.Show(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_IntervencionQuedaEnCurso"), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Informacion"), MessageBoxButtons.OK, MessageBoxIcon.Information);
            CargarOrdenes_62FD();
            ModoConsulta_62FD();
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ModoConsulta_62FD()
        {
            intervencion_62FD = null;
            orden_62FD = null;

            txtMatricula.Clear();
            txtTipoIntervencion.Clear();
            txtEstadoOrden.Clear();
            txtFechaLimite.Clear();
            txtHorasInsumidas.Clear();
            cmbEstadoIntervencion.SelectedIndex = -1;
            LimpiarTarea_62FD();

            dataGridView1.DataSource = null;
            listaTareas_62FD = new List<Tarea_62FD>();

            groupBox3.Enabled = false;
            groupBox4.Enabled = false;
            btnAplicar.Enabled = false;
            btnCancelar.Enabled = false;
            dtpFechaFin.Enabled = false;

            // Enter busca o inicia la intervención de la orden elegida
            this.AcceptButton = button1;

            ActualizarBarraEstado_62FD("Msg_NombreModoConsulta");
        }

        private void ModoRegistro_62FD()
        {
            groupBox3.Enabled = true;
            groupBox4.Enabled = true;
            btnAplicar.Enabled = true;
            btnCancelar.Enabled = true;
            dtpFechaFin.Enabled = true;

            // Mientras se cargan tareas, Enter agrega la tarea
            this.AcceptButton = btnAgregarTarea;
            this.ActiveControl = txtDescripcion;

            ActualizarBarraEstado_62FD("Msg_NombreModoRegistro");
        }

        private void dataGridView1_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridViewColumn columna_62FD = dataGridView1.Columns[e.ColumnIndex];
            ordenAscendente_62FD = columnaOrden_62FD == columna_62FD.DataPropertyName ? !ordenAscendente_62FD : true;
            columnaOrden_62FD = columna_62FD.DataPropertyName;

            listaTareas_62FD = Ordenar_62FD(listaTareas_62FD, columnaOrden_62FD, ordenAscendente_62FD);
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = listaTareas_62FD;
            Traductor_23DB.TraducirGrilla_23DB(dataGridView1, "RegistrarIntervencion");
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

        private void RegistrarIntervencion_62FD_FormClosing(object sender, FormClosingEventArgs e)
        {
            Observer_23DB.ObtenerInstancia_23DB().Desuscribir_23DB(this);

        }
    }
}
