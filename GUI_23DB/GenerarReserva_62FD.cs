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
        EventoBLL_23DB eventoBLL_23DB = new EventoBLL_23DB();

        private Vuelo_62FD vuelo_62FD = null;
        private Pasajero_62FD pasajeroActual_62FD = null;
        private List<Asiento_62FD> listaAsientos_62FD = new List<Asiento_62FD>();
        private List<ReservaDetalle_62FD> detalles_62FD = new List<ReservaDetalle_62FD>();
        private Asiento_62FD asientoSeleccionado_62FD = null;

        public GenerarReserva_62FD()
        {
            InitializeComponent();
            dataGridView2.AutoGenerateColumns = false;
            dataGridView1.AutoGenerateColumns = false;
            Observer_23DB.ObtenerInstancia_23DB().Suscribir_23DB(this);
            AplicarIdiomaActual_23DB();
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
            lblEstado.Text = "Modo Añadir";
            ActualizarBarraEstado_62FD("Añadir");
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
                    MessageBox.Show("Debe ingresar un número de documento.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                pasajeroActual_62FD = pasajeroBLL_62FD.ObtenerPasajero_62FD(txtDNI.Text);

                if (pasajeroActual_62FD == null)
                {
                    DialogResult respuesta_62FD = MessageBox.Show("El pasajero no se encuentra registrado. ¿Desea registrarlo?", "Pasajero no encontrado", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

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
                MessageBox.Show("Error al buscar el pasajero: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarAsientos_62FD()
        {
            listaAsientos_62FD = asientoBLL_62FD.ObtenerAsientosDisponibles_62FD(vuelo_62FD.NroVuelo_62FD);

            QuitarAsientosYaAsignados_62FD();

            dataGridView2.DataSource = null;
            dataGridView2.DataSource = listaAsientos_62FD;

            if (listaAsientos_62FD.Count == 0)
            {
                MessageBox.Show("No hay asientos disponibles para ese vuelo.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                MessageBox.Show("Debe buscar un pasajero antes de agregarlo.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (asientoSeleccionado_62FD == null)
            {
                MessageBox.Show("Debe seleccionar un asiento.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            foreach (ReservaDetalle_62FD d_62FD in detalles_62FD)
            {
                if (d_62FD.NumeroDocumento_62FD == pasajeroActual_62FD.NumeroDocumento_62FD)
                {
                    MessageBox.Show("El pasajero ya fue agregado a la reserva.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                MessageBox.Show("Debe seleccionar un pasajero de la reserva.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

                string codReserva_62FD = reservaBLL_62FD.GenerarReserva_62FD(reserva_62FD);

                eventoBLL_23DB.RegistrarEvento_23DB(SessionManager_23DB.ObtenerInstancia_23DB().DNI_23DB, "Pasajes", "Generar Reserva", 2);

                MessageBox.Show("Reserva creada con éxito. Código: " + codReserva_62FD, "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);

                ModoConsulta_62FD();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al generar la reserva: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ModoConsulta_62FD()
        {
            lblEstado.Text = "Modo Consulta";
            ActualizarBarraEstado_62FD("Consulta");

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

        private void ActualizarBarraEstado_62FD(string modo_62FD)
        {
            toolStripStatusLabel1.Text = "Modo: " + modo_62FD;
            toolStripStatusLabel2.Text = "Usuario: " + SessionManager_23DB.ObtenerInstancia_23DB().Login_23DB;
            toolStripStatusLabel3.Text = "Fecha: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm");
        }

        private void toolStripStatusLabel2_Click(object sender, EventArgs e)
        {

        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            if (detalles_62FD.Count > 0)
            {
                DialogResult respuesta_62FD = MessageBox.Show("¿Está seguro que desea descartar la reserva en curso?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

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
            string formName_23DB = "GenerarReserva";
            foreach (Control control_23DB in ObtenerTodosControles_23DB(this))
            {
                string clave_23DB = formName_23DB + "_" + control_23DB.Name;
                if (configuracion_23DB.ContainsKey(clave_23DB))
                {
                    control_23DB.Text = configuracion_23DB[clave_23DB];
                }

            }
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
        private void GenerarReserva_62FD_FormClosing(object sender, FormClosingEventArgs e)
        {
            Observer_23DB.ObtenerInstancia_23DB().Desuscribir_23DB(this);
        }
    }
}
