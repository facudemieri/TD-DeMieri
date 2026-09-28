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
    public partial class GenerarPasaje_62FD : Form, IIdiomaObserver_23DB
    {
        ReservaBLL_62FD reservaBLL_62FD = new ReservaBLL_62FD();
        PasajeBLL_62FD pasajeBLL_62FD = new PasajeBLL_62FD();
        EventoBLL_23DB eventoBLL_23DB = new EventoBLL_23DB();

        private Reserva_62FD reserva_62FD = null;
        public GenerarPasaje_62FD()
        {
            InitializeComponent();
            Observer_23DB.ObtenerInstancia_23DB().Suscribir_23DB(this);
            AplicarIdiomaActual_23DB();
            dataGridView1.AutoGenerateColumns = false;
            CargarCombos_62FD();
            ModoConsulta_62FD();
        }

        private void ModoConsulta_62FD()
        {
            lblEstado.Text = "Modo Consulta";
            ActualizarBarraEstado_62FD("Consulta");

            reserva_62FD = null;

            txtNroVuelo.Clear();
            txtRuta.Clear();
            txtFechaHoraSalid.Clear();
            txtEstadoReserva.Clear();
            dataGridView1.DataSource = null;
            lblMontoTotal.Text = "$ 0,00";
            label4.Text = "";

            LimpiarDatosPago_62FD();
            groupBox2.Enabled = false;
            btnAplicar.Enabled = false;
            btnImprimir.Enabled = false;
        }

        private void LimpiarDatosPago_62FD()
        {
            cmbMedioPago.SelectedIndex = -1;
            cmbEFinanciera.SelectedIndex = -1;
            txtNroTarjeta.Clear();
            txtCodSeguridad.Clear();
            dateTimePicker1.Value = DateTime.Now;
        }

        private void ActualizarBarraEstado_62FD(string modo_62FD)
        {
            toolStripStatusLabel1.Text = "Modo: " + modo_62FD;
            toolStripStatusLabel2.Text = "Usuario: " + SessionManager_23DB.ObtenerInstancia_23DB().Login_23DB;
            toolStripStatusLabel3.Text = "Fecha: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm");
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
            string formName_23DB = "GenerarPasaje";
            foreach (Control control_23DB in ObtenerTodosControles_23DB(this))
            {
                string clave_23DB = formName_23DB + "_" + control_23DB.Name;
                if (configuracion_23DB.ContainsKey(clave_23DB))
                {
                    control_23DB.Text = configuracion_23DB[clave_23DB];
                }

            }
        }

        private List<Control> ObtenerTodosControles_23DB(Control control_23DB)
        {
            List<Control> lista_23DB = new List<Control>();
            foreach (Control c_23DB in control_23DB.Controls)
            {
                lista_23DB.Add(c_23DB);
                lista_23DB.AddRange(ObtenerTodosControles_23DB(c_23DB));
            }
            return lista_23DB;
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
                    MessageBox.Show("Debe ingresar un código de reserva.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                reserva_62FD = reservaBLL_62FD.ObtenerReserva_62FD(txtCodReserva.Text);

                if (reserva_62FD == null)
                {
                    MessageBox.Show("Reserva no válida o inexistente.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    ModoConsulta_62FD();
                    return;
                }

                if (reserva_62FD.EstadoReserva_62FD != "Pendiente de Pago")
                {
                    MessageBox.Show("La reserva no se encuentra pendiente de pago.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    ModoConsulta_62FD();
                    return;
                }

                CargarDatosReserva_62FD();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al buscar la reserva: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarDatosReserva_62FD()
        {
            txtNroVuelo.Text = reserva_62FD.NroVuelo_62FD;
            txtEstadoReserva.Text = reserva_62FD.EstadoReserva_62FD;
            txtRuta.Text = reserva_62FD.Ruta_62FD;
            txtFechaHoraSalid.Text = reserva_62FD.FechaHoraSalida_62FD.ToString("dd/MM/yyyy HH:mm");

            txtNroVuelo.ReadOnly = true;
            txtEstadoReserva.ReadOnly = true;
            txtRuta.ReadOnly = true;
            txtFechaHoraSalid.ReadOnly = true;

            dataGridView1.DataSource = null;
            dataGridView1.DataSource = reserva_62FD.Detalles_62FD;

            lblMontoTotal.Text = reserva_62FD.MontoTotal_62FD.ToString("C2");

            ModoCobro_62FD();
        }

        private void ModoCobro_62FD()
        {
            lblEstado.Text = "Modo Cobro";
            ActualizarBarraEstado_62FD("Cobro");

            groupBox2.Enabled = true;
            btnAplicar.Enabled = true;
            btnImprimir.Enabled = false;
        }

        private void cmbMedioPago_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool esTarjeta_62FD = cmbMedioPago.Text != "Efectivo" && cmbMedioPago.SelectedIndex >= 0;

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

        private void btnAplicar_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbMedioPago.SelectedIndex < 0)
                {
                    MessageBox.Show("Debe seleccionar un medio de pago.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

                eventoBLL_23DB.RegistrarEvento_23DB(SessionManager_23DB.ObtenerInstancia_23DB().DNI_23DB, "Pasajes", "Generar Pasaje", 2);

                label4.Text = "Factura: " + factura_62FD.NroFactura_62FD; // label4 es el de Numero de Factura
                txtEstadoReserva.Text = "Confirmada";

                MessageBox.Show("Pasajes generados correctamente. Factura: " + factura_62FD.NroFactura_62FD, "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);

                ModoEmitido_62FD();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al generar el pasaje: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ModoEmitido_62FD()
        {
            lblEstado.Text = "Pasajes emitidos";
            ActualizarBarraEstado_62FD("Pasajes emitidos");

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
            MessageBox.Show("Función de impresión en desarrollo.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
    }
}
