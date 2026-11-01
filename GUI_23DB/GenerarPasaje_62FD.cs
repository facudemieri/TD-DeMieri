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
using System.IO;

namespace GUI_23DB
{
    public partial class GenerarPasaje_62FD : Form, IIdiomaObserver_23DB
    {
        ReservaBLL_62FD reservaBLL_62FD = new ReservaBLL_62FD();
        PasajeBLL_62FD pasajeBLL_62FD = new PasajeBLL_62FD();
        EventoBLL_23DB eventoBLL_23DB = new EventoBLL_23DB();
        private Factura_62FD facturaEmitida_62FD = null;
        private List<Pasaje_62FD> pasajesEmitidos_62FD = new List<Pasaje_62FD>();
        private Reserva_62FD reserva_62FD = null;
        private List<Reserva_62FD> listaReservas_62FD = new List<Reserva_62FD>();
        private string claveEstado_62FD = null;
        private string claveModo_62FD = null;
        private DateTime fechaModo_62FD = DateTime.Now;
        private string nroFacturaMostrada_62FD = null;

        public GenerarPasaje_62FD()
        {
            InitializeComponent();
            Observer_23DB.ObtenerInstancia_23DB().Suscribir_23DB(this);
            AplicarIdiomaActual_23DB();
            dataGridView1.AutoGenerateColumns = false;
            dataGridView2.AutoGenerateColumns = false;
            ConfigurarControles_62FD();
            CargarCombos_62FD();
            ModoConsulta_62FD();
        }

        private List<ReservaDetalle_62FD> listaDetalles_62FD = new List<ReservaDetalle_62FD>();
        private string columnaOrdenDetalles_62FD = null;
        private bool ordenAscendenteDetalles_62FD = true;
        private string columnaOrdenReservas_62FD = null;
        private bool ordenAscendenteReservas_62FD = true;

        private void ConfigurarControles_62FD()
        {
            // Vencimiento de la tarjeta (MM/yyyy): no puede estar vencida. Se admite desde el primer día del mes
            // actual porque una tarjeta vale hasta el último día de su mes de vencimiento.
            // El control nunca carga un pago existente, así que el límite no puede pisar un valor guardado.
            dateTimePicker1.MinDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);

            // Los valores de los combos se comparan en el código y se guardan en la base: no se traducen, solo se eligen
            cmbMedioPago.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbEFinanciera.DropDownStyle = ComboBoxStyle.DropDownList;

            // El código de reserva, el vuelo y el estado se igualan a sus columnas (Reserva_62FD, Vuelo_62FD).
            // El número de tarjeta ya es una MaskedTextBox "0000-0000-0000-0000": solo dígitos y 16 como máximo.
            // El código de seguridad no se guarda en la base: 4 dígitos es el máximo de la industria (Amex usa 4, el resto 3).
            txtCodReserva.MaxLength = 20;
            txtNroVuelo.MaxLength = 10;
            txtEstadoReserva.MaxLength = 20;
            txtCodSeguridad.MaxLength = 4;
            txtCodSeguridad.KeyPress += SoloDigitos_62FD;

            // dataGridView1: pasajeros de la reserva elegida. dataGridView2: reservas pendientes de pago.
            ConfigurarGrilla_62FD(dataGridView1);
            ConfigurarGrilla_62FD(dataGridView2);

            NumeroDocumento_62FD.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            NombreCompleto_62FD.FillWeight = 160;
            NroAsiento_62FD.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            Clase_62FD.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            Precio_62FD.DefaultCellStyle.Format = "C2";
            Precio_62FD.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            CodReserva_62FD.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            NroVuelo_62FD.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            FechaHoraSalida_62FD.DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
            FechaHoraSalida_62FD.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            CantidadPasajeros_62FD.DefaultCellStyle.Format = "N0";
            CantidadPasajeros_62FD.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            CantidadPasajeros_62FD.FillWeight = 60;
            MontoTotal_62FD.DefaultCellStyle.Format = "C2";
            MontoTotal_62FD.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            dataGridView1.Paint += Grilla_Paint_62FD;
            dataGridView2.Paint += Grilla_Paint_62FD;
            dataGridView1.ColumnHeaderMouseClick += dataGridView1_ColumnHeaderMouseClick;
            dataGridView2.ColumnHeaderMouseClick += dataGridView2_ColumnHeaderMouseClick;
            this.Shown += (s_62FD, e_62FD) =>
            {
                LimpiarSeleccionGrilla_62FD(dataGridView1);
                LimpiarSeleccionGrilla_62FD(dataGridView2);
            };
        }

        // Se ordena una copia: reserva_62FD.Detalles_62FD lo sigue usando el PDF tal como vino
        private void dataGridView1_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            // Con la grilla vaciada (DataSource en null) no se reordena: volvería a mostrar los pasajeros de la reserva anterior
            if (dataGridView1.DataSource == null) return;

            DataGridViewColumn columna_62FD = dataGridView1.Columns[e.ColumnIndex];
            ordenAscendenteDetalles_62FD = columnaOrdenDetalles_62FD == columna_62FD.DataPropertyName ? !ordenAscendenteDetalles_62FD : true;
            columnaOrdenDetalles_62FD = columna_62FD.DataPropertyName;

            listaDetalles_62FD = Ordenar_62FD(listaDetalles_62FD, columnaOrdenDetalles_62FD, ordenAscendenteDetalles_62FD);
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = listaDetalles_62FD;
            LimpiarSeleccionGrilla_62FD(dataGridView1);

            // El glifo se pierde al reenlazar, por eso se pone después
            dataGridView1.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection = ordenAscendenteDetalles_62FD ? SortOrder.Ascending : SortOrder.Descending;
        }

        private void dataGridView2_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            // Con la grilla vaciada (DataSource en null) no se reordena: volvería a mostrar la lista anterior
            if (dataGridView2.DataSource == null) return;

            DataGridViewColumn columna_62FD = dataGridView2.Columns[e.ColumnIndex];
            ordenAscendenteReservas_62FD = columnaOrdenReservas_62FD == columna_62FD.DataPropertyName ? !ordenAscendenteReservas_62FD : true;
            columnaOrdenReservas_62FD = columna_62FD.DataPropertyName;

            listaReservas_62FD = Ordenar_62FD(listaReservas_62FD, columnaOrdenReservas_62FD, ordenAscendenteReservas_62FD);
            dataGridView2.DataSource = null;
            dataGridView2.DataSource = listaReservas_62FD;
            LimpiarSeleccionGrilla_62FD(dataGridView2);

            // El glifo se pierde al reenlazar, por eso se pone después
            dataGridView2.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection = ordenAscendenteReservas_62FD ? SortOrder.Ascending : SortOrder.Descending;
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
            grilla_62FD.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font(grilla_62FD.Font, FontStyle.Bold);
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

            System.Drawing.Rectangle area_62FD = new System.Drawing.Rectangle(0, grilla_62FD.ColumnHeadersHeight,
                grilla_62FD.ClientSize.Width, grilla_62FD.ClientSize.Height - grilla_62FD.ColumnHeadersHeight);
            TextRenderer.DrawText(e.Graphics, IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_GrillaSinDatos"),
                grilla_62FD.Font, area_62FD, Color.Gray,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.WordBreak);
        }

        private void ModoConsulta_62FD()
        {
            claveEstado_62FD = "Msg_ModoConsulta";
            ActualizarBarraEstado_62FD("Msg_NombreModoConsulta"); 

            reserva_62FD = null;

            txtNroVuelo.Clear();
            txtRuta.Clear();
            txtFechaHoraSalid.Clear();
            txtEstadoReserva.Clear();
            dataGridView1.DataSource = null;
            lblMontoTotal.Text = "$ 0,00";
            nroFacturaMostrada_62FD = null; // label4 es el número de factura, se vacía al volver a consulta
            MostrarTextosEstado_62FD();

            LimpiarDatosPago_62FD();
            groupBox2.Enabled = false;
            btnAplicar.Enabled = false;
            btnImprimir.Enabled = false;
            CargarReservasPendientes_62FD();
        }

        private void LimpiarDatosPago_62FD()
        {
            cmbMedioPago.SelectedIndex = -1;
            cmbEFinanciera.SelectedIndex = -1;
            txtNroTarjeta.Clear();
            txtCodSeguridad.Clear();
            dateTimePicker1.Value = DateTime.Now;
            // Asignar -1 cuando ya era -1 no dispara SelectedIndexChanged: se fuerza el estado de los campos de tarjeta
            HabilitarDatosTarjeta_62FD();
        }

        // Los datos de tarjeta solo se habilitan con un medio de pago elegido que no sea efectivo
        private void HabilitarDatosTarjeta_62FD()
        {
            bool esTarjeta_62FD = cmbMedioPago.SelectedIndex >= 0 && cmbMedioPago.Text != "Efectivo";

            cmbEFinanciera.Enabled = esTarjeta_62FD;
            txtNroTarjeta.Enabled = esTarjeta_62FD;
            txtCodSeguridad.Enabled = esTarjeta_62FD;
            dateTimePicker1.Enabled = esTarjeta_62FD;

            if (!esTarjeta_62FD)
            {
                cmbEFinanciera.SelectedIndex = -1;
                txtNroTarjeta.Clear();
                txtCodSeguridad.Clear();
            }
        }

        private void SoloDigitos_62FD(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

 
        private void ActualizarBarraEstado_62FD(string claveModo_62FD)
        {
            this.claveModo_62FD = claveModo_62FD;
            fechaModo_62FD = DateTime.Now; 
            MostrarTextosEstado_62FD();
        }

        private void MostrarTextosEstado_62FD()
        {
            if(claveEstado_62FD != null)
            {
                lblEstado.Text = IdiomaBLL_23DB.ObtenerTexto_23DB(claveEstado_62FD);
            }

            label4.Text = nroFacturaMostrada_62FD == null ? string.Empty : string.Format(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_EtiquetaFactura"), nroFacturaMostrada_62FD);

            if(claveModo_62FD != null)
            {
                toolStripStatusLabel1.Text = string.Format(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_BarraModo"), IdiomaBLL_23DB.ObtenerTexto_23DB(claveModo_62FD));
                toolStripStatusLabel2.Text = string.Format(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_BarraUsuario"), SessionManager_23DB.ObtenerInstancia_23DB().Login_23DB);
                toolStripStatusLabel3.Text = string.Format(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_BarraFecha"), fechaModo_62FD.ToString("dd/MM/yyyy HH:mm"));
            }
        }

        private void CargarCombos_62FD()
        {
            cmbMedioPago.Items.Clear();
            cmbMedioPago.Items.Add("Efectivo");
            cmbMedioPago.Items.Add("Débito");
            cmbMedioPago.Items.Add("Crédito");

            cmbEFinanciera.Items.Clear();
            cmbEFinanciera.Items.Add("Visa");
            cmbEFinanciera.Items.Add("Mastercard");
            cmbEFinanciera.Items.Add("American Express");
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

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtCodReserva.Text))
                {
                    MessageBox.Show(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_IngreseCodigoReserva"), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Informacion"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                reserva_62FD = reservaBLL_62FD.ObtenerReserva_62FD(txtCodReserva.Text);

                if (reserva_62FD == null)
                {
                    MessageBox.Show(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_ReservaInvalida"), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    ModoConsulta_62FD();
                    return;
                }

                if (reserva_62FD.EstadoReserva_62FD != "Pendiente de Pago")
                {
                    MessageBox.Show(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_ReservaNoPendiente"), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    ModoConsulta_62FD();
                    return;
                }

                CargarDatosReserva_62FD();
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_ErrorBuscarReserva"), ex.Message), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarDatosReserva_62FD()
        {
            // Al cargar una reserva nueva no puede quedar nada de una emisión anterior a la vista ni en memoria
            nroFacturaMostrada_62FD = null;
            facturaEmitida_62FD = null;
            pasajesEmitidos_62FD = new List<Pasaje_62FD>();
            btnBuscar.Enabled = true;
            LimpiarDatosPago_62FD();

            txtNroVuelo.Text = reserva_62FD.NroVuelo_62FD;
            txtEstadoReserva.Text = reserva_62FD.EstadoReserva_62FD;
            txtRuta.Text = reserva_62FD.Ruta_62FD;
            txtFechaHoraSalid.Text = reserva_62FD.FechaHoraSalida_62FD.ToString("dd/MM/yyyy HH:mm");

            txtNroVuelo.ReadOnly = true;
            txtEstadoReserva.ReadOnly = true;
            txtRuta.ReadOnly = true;
            txtFechaHoraSalid.ReadOnly = true;

            listaDetalles_62FD = new List<ReservaDetalle_62FD>(reserva_62FD.Detalles_62FD);
            columnaOrdenDetalles_62FD = null;
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = listaDetalles_62FD;
            LimpiarSeleccionGrilla_62FD(dataGridView1);

            lblMontoTotal.Text = reserva_62FD.MontoTotal_62FD.ToString("C2");

            ModoCobro_62FD();
        }

        private void ModoCobro_62FD()
        {
            claveEstado_62FD = "Msg_ModoCobro";
            ActualizarBarraEstado_62FD("Msg_NombreModoCobro"); 

            groupBox2.Enabled = true;
            btnAplicar.Enabled = true;
            btnImprimir.Enabled = false;
        }

        private void cmbMedioPago_SelectedIndexChanged(object sender, EventArgs e)
        {
            HabilitarDatosTarjeta_62FD();
        }

        private void btnAplicar_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbMedioPago.SelectedIndex < 0)
                {
                    MessageBox.Show(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_SeleccioneMedioPago"), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Informacion"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (reserva_62FD.EstadoReserva_62FD != "Pendiente de Pago")
                {
                    MessageBox.Show(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_ReservaNoPendiente"), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    ModoConsulta_62FD();
                    return;
                }

                Pago_62FD pago_62FD = new Pago_62FD();
                pago_62FD.CodReserva_62FD = reserva_62FD.CodReserva_62FD;
                pago_62FD.MedioPago_62FD = cmbMedioPago.Text;
                pago_62FD.MontoTotal_62FD = reserva_62FD.MontoTotal_62FD;

                if (cmbMedioPago.Text != "Efectivo")
                {
                    pago_62FD.EntidadFinanciera_62FD = cmbEFinanciera.Text;
                    pago_62FD.NumeroTarjeta_62FD = txtNroTarjeta.Text.Replace("-", "");
                    pago_62FD.CodigoSeguridad_62FD = txtCodSeguridad.Text;
                    pago_62FD.FechaVencimiento_62FD = dateTimePicker1.Value;
                }


                Factura_62FD factura_62FD = pasajeBLL_62FD.GenerarPasaje_62FD(pago_62FD, reserva_62FD);

                if (factura_62FD == null)
                {
                    MessageBox.Show(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_PagoRechazado"), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Advertencia"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                eventoBLL_23DB.RegistrarEvento_23DB(SessionManager_23DB.ObtenerInstancia_23DB().DNI_23DB, "Pasajes", "Generar Pasaje", 3);

                nroFacturaMostrada_62FD = factura_62FD.NroFactura_62FD; // label4 es el de Numero de Factura
                MostrarTextosEstado_62FD();
                txtEstadoReserva.Text = "Confirmada";

                MessageBox.Show(string.Format(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_PasajesGenerados"), factura_62FD.NroFactura_62FD), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Informacion"), MessageBoxButtons.OK, MessageBoxIcon.Information);

                facturaEmitida_62FD = factura_62FD;
                pasajesEmitidos_62FD = pasajeBLL_62FD.PasajesEmitidos_62FD;
                ModoEmitido_62FD();

                // La reserva pagada deja de estar pendiente: se saca de la grilla sin el aviso de "no hay pendientes"
                CargarReservasPendientes_62FD(false);
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_ErrorGenerarPasaje"), ex.Message), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                ModoConsulta_62FD();
            }
        }

        private void ModoEmitido_62FD()
        {
            claveEstado_62FD = "Msg_PasajesEmitidos";
            ActualizarBarraEstado_62FD("Msg_NombreModoPasajesEmitidos"); 

            groupBox2.Enabled = false;
            btnAplicar.Enabled = false;
            btnImprimir.Enabled = true;
            btnBuscar.Enabled = false;
        }

        private void GenerarPasaje_62FD_FormClosing(object sender, FormClosingEventArgs e)
        {
            Observer_23DB.ObtenerInstancia_23DB().Desuscribir_23DB(this);
        }

        private void btnImprimir_Click(object sender, EventArgs e)
        {
            try
            {
                if (facturaEmitida_62FD == null)
                {
                    MessageBox.Show(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_SinComprobanteEmitido"), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Informacion"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                string carpeta_62FD = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "AeroManager", "Comprobantes");

                if (!Directory.Exists(carpeta_62FD))
                {
                    Directory.CreateDirectory(carpeta_62FD);
                }

                string ruta_62FD = Path.Combine(carpeta_62FD, "Comprobante_" + facturaEmitida_62FD.NroFactura_62FD + ".pdf");

                GeneradorPasajePDF_62FD generador_62FD = new GeneradorPasajePDF_62FD();
                generador_62FD.Generar_62FD(ruta_62FD, reserva_62FD, facturaEmitida_62FD, pasajesEmitidos_62FD, cmbMedioPago.Text);

                System.Diagnostics.Process.Start(ruta_62FD);
            }
            catch (Exception ex_62FD)
            {
                MessageBox.Show(string.Format(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_ErrorGenerarDocumento"), ex_62FD.Message), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            ModoConsulta_62FD();
            txtCodReserva.Clear();
            btnBuscar.Enabled = true;
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void GenerarPasaje_62FD_Load(object sender, EventArgs e)
        {
            CargarReservasPendientes_62FD();

        }

        private void CargarReservasPendientes_62FD()
        {
            CargarReservasPendientes_62FD(true);
        }

        private void CargarReservasPendientes_62FD(bool avisarSiVacia_62FD)
        {
            try
            {
                listaReservas_62FD = reservaBLL_62FD.ObtenerReservasPendientes_62FD();

                dataGridView2.DataSource = null;
                dataGridView2.DataSource = listaReservas_62FD;
                columnaOrdenReservas_62FD = null;
                LimpiarSeleccionGrilla_62FD(dataGridView2);

                if (avisarSiVacia_62FD && listaReservas_62FD.Count == 0)
                {
                    MessageBox.Show(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_SinReservasPendientes"), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Informacion"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_ErrorCargarReservas"), ex.Message), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }

            try
            {
                Reserva_62FD seleccionada_62FD = (Reserva_62FD)dataGridView2.Rows[e.RowIndex].DataBoundItem;

                txtCodReserva.Text = seleccionada_62FD.CodReserva_62FD;
                reserva_62FD = reservaBLL_62FD.ObtenerReserva_62FD(seleccionada_62FD.CodReserva_62FD);

                CargarDatosReserva_62FD();
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_ErrorCargarReserva"), ex.Message), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
