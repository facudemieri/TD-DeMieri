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
    public partial class GenerarReserva_62FD : Form, IIdiomaObserver_23DB
    {
        PasajeroBLL_62FD pasajeroBLL_62FD = new PasajeroBLL_62FD();
        AsientoBLL_62FD asientoBLL_62FD = new AsientoBLL_62FD();
        ReservaBLL_62FD reservaBLL_62FD = new ReservaBLL_62FD();

        private Vuelo_62FD vuelo_62FD = null;
        private Pasajero_62FD pasajeroActual_62FD = null;
        private List<Asiento_62FD> listaAsientos_62FD = new List<Asiento_62FD>();
        private List<ReservaDetalle_62FD> detalles_62FD = new List<ReservaDetalle_62FD>();
        private Asiento_62FD asientoSeleccionado_62FD = null;

        // estado de los textos que escribe el código. Se guardan claves, no textos traducidos
        private string claveEstado_62FD = null;
        private string claveModo_62FD = null;
        private DateTime fechaModo_62FD = DateTime.Now;

        public GenerarReserva_62FD()
        {
            InitializeComponent();
            dataGridView2.AutoGenerateColumns = false;
            dataGridView1.AutoGenerateColumns = false;
            Observer_23DB.ObtenerInstancia_23DB().Suscribir_23DB(this);
            AplicarIdiomaActual_23DB();
            Ayuda_62FD.Vincular_23DB(this, "generar-reserva");
            ConfigurarControles_62FD();
        }

        private string columnaOrdenAsientos_62FD = null;
        private bool ordenAscendenteAsientos_62FD = true;
        private string columnaOrdenDetalles_62FD = null;
        private bool ordenAscendenteDetalles_62FD = true;

        private void ConfigurarControles_62FD()
        {
            // Largos iguales a las columnas: documento y datos del pasajero (Pasajero_62FD), vuelo y matrícula (Vuelo_62FD).
            // Ruta, fecha y precio son textos armados por el código, sin columna propia: no llevan MaxLength.
            txtDNI.MaxLength = 8;
            txtApellido.MaxLength = 50;
            txtNombre.MaxLength = 50;
            txtCorreo.MaxLength = 100;
            txtTelefono.MaxLength = 20;
            txtNumeroVuelo.MaxLength = 10;
            txtAeronave.MaxLength = 10;

            // dataGridView2: asientos disponibles del vuelo. dataGridView1: pasajeros ya agregados a la reserva.
            ConfigurarGrilla_62FD(dataGridView2);
            ConfigurarGrilla_62FD(dataGridView1);

            NroAsiento_62FD.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            Clase_62FD.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            EstadoAsiento_62FD.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            NumeroDocumento_62FD.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            NombreCompleto_62FD.FillWeight = 160;
            NrAsiento_62FD.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            Class_62FD.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            Precio_62FD.DefaultCellStyle.Format = "C2";
            Precio_62FD.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            dataGridView2.Paint += Grilla_Paint_62FD;
            dataGridView1.Paint += Grilla_Paint_62FD;
            dataGridView2.ColumnHeaderMouseClick += dataGridView2_ColumnHeaderMouseClick;
            dataGridView1.ColumnHeaderMouseClick += dataGridView1_ColumnHeaderMouseClick;
            this.Shown += (s_62FD, e_62FD) =>
            {
                LimpiarSeleccionGrilla_62FD(dataGridView2);
                LimpiarSeleccionGrilla_62FD(dataGridView1);
            };
        }

        // Al reenlazar la grilla de asientos no queda ninguna fila resaltada: el asiento elegido también se descarta,
        // para que Agregar no use un asiento que el usuario ya no ve seleccionado
        private void LimpiarSeleccionAsiento_62FD()
        {
            LimpiarSeleccionGrilla_62FD(dataGridView2);
            asientoSeleccionado_62FD = null;
            btnAgregarReserva.Enabled = false;
        }

        private void dataGridView2_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            // Con la grilla vaciada (DataSource en null) no se reordena: volvería a mostrar asientos ya asignados
            if (dataGridView2.DataSource == null) return;

            DataGridViewColumn columna_62FD = dataGridView2.Columns[e.ColumnIndex];
            ordenAscendenteAsientos_62FD = columnaOrdenAsientos_62FD == columna_62FD.DataPropertyName ? !ordenAscendenteAsientos_62FD : true;
            columnaOrdenAsientos_62FD = columna_62FD.DataPropertyName;

            listaAsientos_62FD = Ordenar_62FD(listaAsientos_62FD, columnaOrdenAsientos_62FD, ordenAscendenteAsientos_62FD);
            dataGridView2.DataSource = null;
            dataGridView2.DataSource = listaAsientos_62FD;
            LimpiarSeleccionAsiento_62FD();

            // El glifo se pierde al reenlazar, por eso se pone después
            dataGridView2.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection = ordenAscendenteAsientos_62FD ? SortOrder.Ascending : SortOrder.Descending;
        }

        private void dataGridView1_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            // Con la grilla vaciada (DataSource en null) no se reordena: volvería a mostrar la lista anterior
            if (dataGridView1.DataSource == null) return;

            DataGridViewColumn columna_62FD = dataGridView1.Columns[e.ColumnIndex];
            ordenAscendenteDetalles_62FD = columnaOrdenDetalles_62FD == columna_62FD.DataPropertyName ? !ordenAscendenteDetalles_62FD : true;
            columnaOrdenDetalles_62FD = columna_62FD.DataPropertyName;

            detalles_62FD = Ordenar_62FD(detalles_62FD, columnaOrdenDetalles_62FD, ordenAscendenteDetalles_62FD);
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = detalles_62FD;
            LimpiarSeleccionGrilla_62FD(dataGridView1);

            // El glifo se pierde al reenlazar, por eso se pone después
            dataGridView1.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection = ordenAscendenteDetalles_62FD ? SortOrder.Ascending : SortOrder.Descending;
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

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void GenerarReserva_62FD_Load(object sender, EventArgs e)
        {
            SeleccionarVuelo_62FD formSeleccionarVuelo_62FD = new SeleccionarVuelo_62FD();

            if (formSeleccionarVuelo_62FD.ShowDialog() == DialogResult.OK)
            {
                vuelo_62FD = formSeleccionarVuelo_62FD.VueloSeleccionado_62FD;
                CargarDatosVuelo_62FD();
                ModoAnadir_62FD();
            }
            else
            {
                this.Close();
            }
        }

        private void ModoAnadir_62FD()
        {
            claveEstado_62FD = "Msg_ModoAnadir";
            ActualizarBarraEstado_62FD("Msg_NombreModoAnadir"); // se pasa la clave del nombre del modo
            btnAplicar.Enabled = false;
            btnAgregarReserva.Enabled = false;
        }

        private void CargarDatosVuelo_62FD()
        {
            txtNumeroVuelo.Text = vuelo_62FD.NroVuelo_62FD;
            txtRuta.Text = vuelo_62FD.Ruta_62FD;
            txtFechayHora.Text = vuelo_62FD.FechaHoraSalida_62FD.ToString("dd/MM/yyyy HH:mm");
            txtAeronave.Text = vuelo_62FD.Matricula_62FD;
            txtPrecio.Text = vuelo_62FD.Precio_62FD.ToString("C2");
            txtNumeroVuelo.ReadOnly = true;
            txtRuta.ReadOnly = true;
            txtFechayHora.ReadOnly = true;
            txtAeronave.ReadOnly = true;
            txtPrecio.ReadOnly = true;
        }

        private void btnBuscarPasajero_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtDNI.Text))
                {
                    MessageBox.Show(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_IngreseNroDocumento"), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Informacion"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                pasajeroActual_62FD = pasajeroBLL_62FD.ObtenerPasajero_62FD(txtDNI.Text);

                if (pasajeroActual_62FD == null)
                {
                    DialogResult respuesta_62FD = MessageBox.Show(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_PasajeroNoRegistrado"), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_PasajeroNoEncontrado"), MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (respuesta_62FD == DialogResult.Yes)
                    {
                        RegistrarPasajero_62FD formRegistrarPasajero_62FD = new RegistrarPasajero_62FD(txtDNI.Text);

                        if (formRegistrarPasajero_62FD.ShowDialog() == DialogResult.OK)
                        {
                            pasajeroActual_62FD = formRegistrarPasajero_62FD.PasajeroRegistrado_62FD;
                        }
                    }
                }

                if (pasajeroActual_62FD != null)
                {
                    MostrarDatosPasajero_62FD();
                    CargarAsientos_62FD();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_ErrorBuscarPasajero"), ex.Message), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarAsientos_62FD()
        {
            listaAsientos_62FD = asientoBLL_62FD.ObtenerAsientosDisponibles_62FD(vuelo_62FD.NroVuelo_62FD);

            QuitarAsientosYaAsignados_62FD();

            dataGridView2.DataSource = null;
            dataGridView2.DataSource = listaAsientos_62FD;
            columnaOrdenAsientos_62FD = null;
            LimpiarSeleccionAsiento_62FD();

            if (listaAsientos_62FD.Count == 0)
            {
                MessageBox.Show(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_SinAsientosDisponibles"), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Informacion"), MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void QuitarAsientosYaAsignados_62FD()
        {
            List<Asiento_62FD> disponibles_62FD = new List<Asiento_62FD>();

            foreach (Asiento_62FD asiento_62FD in listaAsientos_62FD)
            {
                bool yaAsignado_62FD = false;

                foreach (ReservaDetalle_62FD detalle_62FD in detalles_62FD)
                {
                    if (detalle_62FD.IdAsiento_62FD == asiento_62FD.IdAsiento_62FD)
                    {
                        yaAsignado_62FD = true;
                    }
                }

                if (!yaAsignado_62FD)
                {
                    disponibles_62FD.Add(asiento_62FD);
                }
            }

            listaAsientos_62FD = disponibles_62FD;
        }

        private void MostrarDatosPasajero_62FD()
        {
            txtApellido.Text = pasajeroActual_62FD.Apellido_62FD;
            txtNombre.Text = pasajeroActual_62FD.Nombre_62FD;
            txtCorreo.Text = pasajeroActual_62FD.CorreoElectronico_62FD;
            txtTelefono.Text = pasajeroActual_62FD.Telefono_62FD;
        }

        private void LimpiarDatosPasajero_62FD()
        {
            txtDNI.Clear();
            txtApellido.Clear();
            txtNombre.Clear();
            txtCorreo.Clear();
            txtTelefono.Clear();
            pasajeroActual_62FD = null;
        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                asientoSeleccionado_62FD = (Asiento_62FD)dataGridView2.Rows[e.RowIndex].DataBoundItem;
                btnAgregarReserva.Enabled = true;
            }
        }

        private void btnAgregarReserva_Click(object sender, EventArgs e)
        {
            if (pasajeroActual_62FD == null)
            {
                MessageBox.Show(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_BuscarPasajeroAntes"), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Informacion"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (asientoSeleccionado_62FD == null)
            {
                MessageBox.Show(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_SeleccioneAsiento"), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Informacion"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            foreach (ReservaDetalle_62FD d_62FD in detalles_62FD)
            {
                if (d_62FD.NumeroDocumento_62FD == pasajeroActual_62FD.NumeroDocumento_62FD)
                {
                    MessageBox.Show(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_PasajeroYaAgregado"), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Informacion"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }

            ReservaDetalle_62FD detalle_62FD = new ReservaDetalle_62FD();
            detalle_62FD.NumeroDocumento_62FD = pasajeroActual_62FD.NumeroDocumento_62FD;
            detalle_62FD.IdAsiento_62FD = asientoSeleccionado_62FD.IdAsiento_62FD;
            detalle_62FD.Precio_62FD = vuelo_62FD.Precio_62FD;
            detalle_62FD.NombreCompleto_62FD = pasajeroActual_62FD.NombreCompleto_62FD;
            detalle_62FD.NroAsiento_62FD = asientoSeleccionado_62FD.NroAsiento_62FD;
            detalle_62FD.Clase_62FD = asientoSeleccionado_62FD.Clase_62FD;

            detalles_62FD.Add(detalle_62FD);

            CargarGrillaDetalles_62FD();
            LimpiarDatosPasajero_62FD();
            asientoSeleccionado_62FD = null;
            dataGridView2.DataSource = null;
            btnAgregarReserva.Enabled = false;
        }

        private void CargarGrillaDetalles_62FD()
        {
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = detalles_62FD;
            columnaOrdenDetalles_62FD = null;
            LimpiarSeleccionGrilla_62FD(dataGridView1);

            ActualizarMontoTotal_62FD();
            btnAplicar.Enabled = detalles_62FD.Count > 0;
        }

        private void ActualizarMontoTotal_62FD()
        {
            decimal total_62FD = 0;

            foreach (ReservaDetalle_62FD d_62FD in detalles_62FD)
            {
                total_62FD = total_62FD + d_62FD.Precio_62FD;
            }

            lblMonto.Text = total_62FD.ToString("C2");
        }

        private void btnQuitar_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                MessageBox.Show(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_SeleccionePasajeroReserva"), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Informacion"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            ReservaDetalle_62FD detalle_62FD = (ReservaDetalle_62FD)dataGridView1.CurrentRow.DataBoundItem;
            detalles_62FD.Remove(detalle_62FD);

            CargarGrillaDetalles_62FD();
        }

        private void btnAplicar_Click(object sender, EventArgs e)
        {
            try
            {
                Reserva_62FD reserva_62FD = new Reserva_62FD();
                reserva_62FD.NroVuelo_62FD = vuelo_62FD.NroVuelo_62FD;
                reserva_62FD.Detalles_62FD = detalles_62FD;

                bool bitacoraRegistrada_62FD;
                string codReserva_62FD = reservaBLL_62FD.GenerarReserva_62FD(reserva_62FD, out bitacoraRegistrada_62FD);

                if (!bitacoraRegistrada_62FD)
                {
                    MessageBox.Show(string.Format(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_OperacionSinBitacora"), reservaBLL_62FD.ErrorBitacora_62FD), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Advertencia"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                MessageBox.Show(string.Format(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_ReservaCreada"), codReserva_62FD), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Informacion"), MessageBoxButtons.OK, MessageBoxIcon.Information);

                ModoConsulta_62FD();
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_ErrorGenerarReserva"), ex.Message), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ModoConsulta_62FD()
        {
            claveEstado_62FD = "Msg_ModoConsulta";
            ActualizarBarraEstado_62FD("Msg_NombreModoConsulta"); // se pasa la clave del nombre del modo

            detalles_62FD.Clear();
            listaAsientos_62FD.Clear();
            asientoSeleccionado_62FD = null;

            LimpiarDatosPasajero_62FD();
            dataGridView2.DataSource = null;
            dataGridView1.DataSource = null;
            lblMonto.Text = "$ 0,00";

            btnAplicar.Enabled = false;
            btnAgregarReserva.Enabled = false;
        }

        // Recibe la clave del nombre del modo (solo se muestra, nunca se compara)
        private void ActualizarBarraEstado_62FD(string claveModo_62FD)
        {
            this.claveModo_62FD = claveModo_62FD;
            fechaModo_62FD = DateTime.Now; // fecha de entrada al modo; al cambiar de idioma se reescribe la misma
            MostrarTextosEstado_62FD();
        }

        // Reconstruye lblEstado y la barra de estado a partir de las claves guardadas, en el idioma activo
        private void MostrarTextosEstado_62FD()
        {
            if(claveEstado_62FD != null)
            {
                lblEstado.Text = IdiomaBLL_23DB.ObtenerTexto_23DB(claveEstado_62FD);
            }

            if(claveModo_62FD != null)
            {
                toolStripStatusLabel1.Text = string.Format(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_BarraModo"), IdiomaBLL_23DB.ObtenerTexto_23DB(claveModo_62FD));
                toolStripStatusLabel2.Text = string.Format(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_BarraUsuario"), SessionManager_23DB.ObtenerInstancia_23DB().Login_23DB);
                toolStripStatusLabel3.Text = string.Format(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_BarraFecha"), fechaModo_62FD.ToString("dd/MM/yyyy HH:mm"));
            }
        }

        private void toolStripStatusLabel2_Click(object sender, EventArgs e)
        {

        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            if (detalles_62FD.Count > 0)
            {
                DialogResult respuesta_62FD = MessageBox.Show(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_ConfirmarDescartarReserva"), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Confirmar"), MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (respuesta_62FD == DialogResult.No)
                {
                    return;
                }
            }

            ModoConsulta_62FD();
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public void ActualizarIdioma_23DB(Dictionary<string, string> configuracion_23DB)
        {
            Traductor_23DB.AplicarIdioma_23DB(this, configuracion_23DB);
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
        private void GenerarReserva_62FD_FormClosing(object sender, FormClosingEventArgs e)
        {
            Observer_23DB.ObtenerInstancia_23DB().Desuscribir_23DB(this);
        }
    }
}
