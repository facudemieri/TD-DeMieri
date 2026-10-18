using BE_23DB;
using BLL_23DB;
using Services_23DB;
using Services_23DB.Serializacion;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI_23DB
{
    public partial class MaestroVuelos_62FD : Form, IIdiomaObserver_23DB
    {
        VueloBLL_62FD vueloBLL_62FD = new VueloBLL_62FD();
        AsientoBLL_62FD asientoBLL_62FD = new AsientoBLL_62FD();

        SerializarBLL_62FD serializarBLL_62DB = new SerializarBLL_62FD();
        DeserializarBLL_62FD deserializarBLL_62DB = new DeserializarBLL_62FD();
        EventoBLL_23DB eventoBLL_23DB = new EventoBLL_23DB();

        // Precio_62FD es decimal(10,2): no puede superar este valor
        private const decimal precioMaximo_62FD = 99999999.99m;

        // modoActual_62FD se compara en el código: sus valores quedan literales y solo se traduce lo que muestra la barra de estado
        private string modoActual_62FD = "Consulta";
        private List<Vuelo_62FD> listaVuelos_62FD = new List<Vuelo_62FD>();
        private List<Asiento_62FD> listaAsientos_62FD = new List<Asiento_62FD>();
        private Vuelo_62FD vueloSeleccionado_62FD = null;
        private bool grillaDesdeArchivo_62FD = false;
        private string columnaOrdenVuelos_62FD = null;
        private bool ordenAscendenteVuelos_62FD = true;
        private string columnaOrdenAsientos_62FD = null;
        private bool ordenAscendenteAsientos_62FD = true;
        private DateTime fechaModo_62FD = DateTime.Now;
        private ToolTip ayuda_62FD = new ToolTip();
        public MaestroVuelos_62FD()
        {
            InitializeComponent();
            Observer_23DB.ObtenerInstancia_23DB().Suscribir_23DB(this);
            AplicarIdiomaActual_23DB();

            dgvVuelos.AutoGenerateColumns = false;
            dgvAsientos.AutoGenerateColumns = false;

            ConfigurarControles_62FD();
        }

        private void ConfigurarControles_62FD()
        {
            // Los valores de los tres combos se guardan en la base y se comparan en el código: no se traducen
            cmbEstado.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbEstado.Items.Clear();
            cmbEstado.Items.Add("Programado");
            cmbEstado.Items.Add("Cancelado");

            cmbClase.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbClase.Items.Clear();
            cmbClase.Items.Add("Ejecutiva");
            cmbClase.Items.Add("Economica");

            cmbEstadoAsiento.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbEstadoAsiento.Items.Clear();
            cmbEstadoAsiento.Items.Add("Disponible");
            cmbEstadoAsiento.Items.Add("Reservado");
            cmbEstadoAsiento.Items.Add("Ocupado");

            dtFechaHoraSalida.Format = DateTimePickerFormat.Custom;
            dtFechaHoraSalida.CustomFormat = "dd/MM/yyyy HH:mm";

            // Largos iguales a las columnas de Vuelo_62FD y Asiento_62FD
            txtNroVuelo.MaxLength = 10;
            // Origen y destino son códigos IATA: regla de negocio de la pantalla, las columnas siguen siendo de 50
            txtOrigen.MaxLength = 3;
            txtDestino.MaxLength = 3;
            txtOrigen.KeyPress += SoloLetras_62FD;
            txtDestino.KeyPress += SoloLetras_62FD;
            txtMatricula.MaxLength = 10;
            txtNroAsiento.MaxLength = 5;
            // decimal(10,2) con separadores: "99.999.999,99" son 13 caracteres
            txtPrecioBase.MaxLength = 13;
            txtPrecioBase.KeyPress += SoloImporte_62FD;

            txtRutaSerializar.ReadOnly = true;
            txtRutaDeserializar.ReadOnly = true;

            ConfigurarGrilla_62FD(dgvVuelos);
            ConfigurarGrilla_62FD(dgvAsientos);

            colNroVuelo.FillWeight = 80;
            colFechaHoraSalida.DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
            colFechaHoraSalida.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colPrecio.DefaultCellStyle.Format = "N2";
            colPrecio.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            colEstadoVuelo.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            colIdAsiento.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            colIdAsiento.FillWeight = 50;
            colNroAsiento.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colClase.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colEstadoAsiento.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgvVuelos.Paint += Grilla_Paint_62FD;
            dgvAsientos.Paint += Grilla_Paint_62FD;
            dgvVuelos.ColumnHeaderMouseClick += dgvVuelos_ColumnHeaderMouseClick;
            dgvAsientos.ColumnHeaderMouseClick += dgvAsientos_ColumnHeaderMouseClick;
            this.Shown += (s_62FD, e_62FD) =>
            {
                LimpiarSeleccionGrilla_62FD(dgvVuelos);
                LimpiarSeleccionGrilla_62FD(dgvAsientos);
            };

            this.CancelButton = btnSalir;
        }

        public void ActualizarIdioma_23DB(Dictionary<string, string> configuracion_23DB)
        {
            Traductor_23DB.AplicarIdioma_23DB(this, configuracion_23DB, statusStrip1);
            AplicarToolTips_62FD();
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

        // Se llama desde ActualizarIdioma_23DB para que los ToolTip también cambien de idioma
        private void AplicarToolTips_62FD()
        {
            ayuda_62FD.SetToolTip(btnLimpiar, IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_AyudaLimpiarVuelos"));
            ayuda_62FD.SetToolTip(btnActualizar, IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_AyudaActualizarVuelos"));
            ayuda_62FD.SetToolTip(btnRutaSerializar, IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_AyudaRutaSerializar"));
            ayuda_62FD.SetToolTip(btnRutaDeserializar, IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_AyudaRutaDeserializar"));
        }

        private void MaestroVuelos_62FD_Load(object sender, EventArgs e)
        {
            CargarGrillaVuelos_62FD();
            ModoConsulta_62FD();
        }

        private void ModoConsulta_62FD()
        {
            modoActual_62FD = "Consulta";

            groupBox2.Enabled = false;
            btnAplicar.Enabled = false;
            btnCancelar.Enabled = false;

            dgvVuelos.Enabled = true;
            btnActualizar.Enabled = true;
            btnLimpiar.Enabled = true;
            btnSerializar.Enabled = true;
            btnDeserializar.Enabled = true;
            btnRutaSerializar.Enabled = true;
            btnRutaDeserializar.Enabled = true;

            btnAñadir.Enabled = !grillaDesdeArchivo_62FD;
            btnModificar.Enabled = !grillaDesdeArchivo_62FD;
            btnEliminar.Enabled = !grillaDesdeArchivo_62FD;

            groupBox3.Enabled = false;

            ActualizarBarraEstado_62FD();
        }

        private void CargarGrillaVuelos_62FD()
        {
            try
            {
                listaVuelos_62FD = vueloBLL_62FD.ObtenerVuelosMaestro_62FD();
                grillaDesdeArchivo_62FD = false;

                EnlazarGrillaVuelos_62FD();
                LimpiarAsientos_62FD();
            }
            catch (Exception ex_62FD)
            {
                MessageBox.Show(string.Format(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_ErrorRecuperarVuelos"), ex_62FD.Message), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void EnlazarGrillaVuelos_62FD()
        {
            columnaOrdenVuelos_62FD = null;

            dgvVuelos.DataSource = null;
            dgvVuelos.DataSource = listaVuelos_62FD;
            Traductor_23DB.TraducirGrilla_23DB(dgvVuelos, "MaestroVuelos");
            LimpiarSeleccionGrilla_62FD(dgvVuelos);
        }

        private void LimpiarAsientos_62FD()
        {
            vueloSeleccionado_62FD = null;
            listaAsientos_62FD = new List<Asiento_62FD>();
            dgvAsientos.DataSource = null;
            LimpiarCamposAsiento_62FD();
            groupBox3.Enabled = false;
        }

        private void dgvVuelos_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvVuelos.SelectedRows.Count == 0 || dgvVuelos.SelectedRows[0].DataBoundItem == null)
            {
                return;
            }

            vueloSeleccionado_62FD = (Vuelo_62FD)dgvVuelos.SelectedRows[0].DataBoundItem;

            if (!grillaDesdeArchivo_62FD && modoActual_62FD == "Consulta")
            {
                CargarGrillaAsientos_62FD();
                groupBox3.Enabled = true;
            }
        }

        private void CargarGrillaAsientos_62FD()
        {
            try
            {
                listaAsientos_62FD = asientoBLL_62FD.ObtenerTodosPorVuelo_62FD(vueloSeleccionado_62FD.NroVuelo_62FD);
                columnaOrdenAsientos_62FD = null;

                dgvAsientos.DataSource = null;
                dgvAsientos.DataSource = listaAsientos_62FD;
                Traductor_23DB.TraducirGrilla_23DB(dgvAsientos, "MaestroVuelos");
                LimpiarSeleccionGrilla_62FD(dgvAsientos);

                LimpiarCamposAsiento_62FD();
            }
            catch (Exception ex_62FD)
            {
                MessageBox.Show(string.Format(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_ErrorRecuperarAsientos"), ex_62FD.Message), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LimpiarCamposAsiento_62FD()
        {
            txtNroAsiento.Clear();
            cmbClase.SelectedIndex = -1;
            cmbEstadoAsiento.SelectedIndex = -1;
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            CargarGrillaVuelos_62FD();
            ModoConsulta_62FD();
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            listaVuelos_62FD = new List<Vuelo_62FD>();
            dgvVuelos.DataSource = null;
            grillaDesdeArchivo_62FD = false;

            LimpiarAsientos_62FD();
            ModoConsulta_62FD();
        }

        private void btnAñadir_Click(object sender, EventArgs e)
        {
            LimpiarCamposVuelo_62FD();
            ModoAlta_62FD();
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            if (dgvVuelos.SelectedRows.Count == 0 || dgvVuelos.SelectedRows[0].DataBoundItem == null)
            {
                MessageBox.Show(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_SeleccioneVuelo"), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Informacion"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            vueloSeleccionado_62FD = (Vuelo_62FD)dgvVuelos.SelectedRows[0].DataBoundItem;

            txtNroVuelo.Text = vueloSeleccionado_62FD.NroVuelo_62FD;
            txtOrigen.Text = vueloSeleccionado_62FD.Origen_62FD;
            txtDestino.Text = vueloSeleccionado_62FD.Destino_62FD;
            dtFechaHoraSalida.Value = vueloSeleccionado_62FD.FechaHoraSalida_62FD;
            txtMatricula.Text = vueloSeleccionado_62FD.Matricula_62FD;
            txtPrecioBase.Text = vueloSeleccionado_62FD.Precio_62FD.ToString("N2");
            cmbEstado.Text = vueloSeleccionado_62FD.EstadoVuelo_62FD;

            ModoModificacion_62FD();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvVuelos.SelectedRows.Count == 0 || dgvVuelos.SelectedRows[0].DataBoundItem == null)
                {
                    MessageBox.Show(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_SeleccioneVuelo"), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Informacion"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                Vuelo_62FD vuelo_62FD = (Vuelo_62FD)dgvVuelos.SelectedRows[0].DataBoundItem;

                if (vuelo_62FD.EstadoVuelo_62FD == "Cancelado")
                {
                    MessageBox.Show(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_VueloYaCancelado"), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Advertencia"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (MessageBox.Show(string.Format(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_ConfirmarCancelarVuelo"), vuelo_62FD.NroVuelo_62FD), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Confirmar"), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    return;
                }

                if (!vueloBLL_62FD.CancelarVuelo_62FD(vuelo_62FD.NroVuelo_62FD))
                {
                    MessageBox.Show(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_NoSePudoCancelarVuelo"), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                RegistrarEventoBitacora_62FD("Cancelar Vuelo", 3);

                CargarGrillaVuelos_62FD();
                ModoConsulta_62FD();
            }
            catch (Exception ex_62FD)
            {
                MessageBox.Show(string.Format(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_ErrorCancelarVuelo"), ex_62FD.Message), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAplicar_Click(object sender, EventArgs e)
        {
            try
            {
                btnAplicar.Enabled = false;

                decimal precio_62FD;

                if (string.IsNullOrWhiteSpace(txtNroVuelo.Text) || string.IsNullOrWhiteSpace(txtOrigen.Text) || string.IsNullOrWhiteSpace(txtDestino.Text) || string.IsNullOrWhiteSpace(txtMatricula.Text) || cmbEstado.SelectedIndex < 0)
                {
                    MessageBox.Show(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_CompletarDatosVuelo"), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Informacion"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (!EsCodigoIata_62FD(txtOrigen.Text) || !EsCodigoIata_62FD(txtDestino.Text))
                {
                    MessageBox.Show(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_OrigenDestinoInvalido"), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Informacion"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (txtOrigen.Text.Trim().ToUpper() == txtDestino.Text.Trim().ToUpper())
                {
                    MessageBox.Show(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_OrigenDestinoIguales"), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Informacion"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // Mayor que cero, que entre en decimal(10,2) y con hasta dos decimales (si no, SQL desborda o redondea)
                if (!decimal.TryParse(txtPrecioBase.Text.Trim(), out precio_62FD) || precio_62FD <= 0 || precio_62FD > precioMaximo_62FD || decimal.Round(precio_62FD, 2) != precio_62FD)
                {
                    MessageBox.Show(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_PrecioBaseInvalido"), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Informacion"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (modoActual_62FD == "Alta" && dtFechaHoraSalida.Value < DateTime.Now)
                {
                    MessageBox.Show(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_FechaSalidaPasada"), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Informacion"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                Vuelo_62FD vuelo_62FD = new Vuelo_62FD();
                vuelo_62FD.NroVuelo_62FD = txtNroVuelo.Text.Trim().ToUpper();
                vuelo_62FD.Origen_62FD = txtOrigen.Text.Trim().ToUpper();
                vuelo_62FD.Destino_62FD = txtDestino.Text.Trim().ToUpper();
                vuelo_62FD.FechaHoraSalida_62FD = dtFechaHoraSalida.Value;
                vuelo_62FD.Matricula_62FD = txtMatricula.Text.Trim().ToUpper();
                vuelo_62FD.Precio_62FD = precio_62FD;
                vuelo_62FD.EstadoVuelo_62FD = cmbEstado.Text;

                int resultado_62FD;

                if (modoActual_62FD == "Alta")
                {
                    resultado_62FD = vueloBLL_62FD.CrearVuelo_62FD(vuelo_62FD);
                }
                else
                {
                    resultado_62FD = vueloBLL_62FD.ModificarVuelo_62FD(vuelo_62FD);
                }

                if (resultado_62FD == -2)
                {
                    MessageBox.Show(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_MatriculaNoEnFlota"), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Advertencia"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (resultado_62FD == 0)
                {
                    MessageBox.Show(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_VueloNroExistente"), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Advertencia"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (resultado_62FD < 0)
                {
                    MessageBox.Show(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_NoSePudoGuardarVuelo"), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (modoActual_62FD == "Alta")
                {
                    RegistrarEventoBitacora_62FD("Crear Vuelo", 2);
                    MessageBox.Show(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_VueloRegistrado"), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Informacion"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    RegistrarEventoBitacora_62FD("Modificar Vuelo", 2);
                    MessageBox.Show(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_VueloModificado"), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Informacion"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                CargarGrillaVuelos_62FD();
                ModoConsulta_62FD();
            }
            catch (Exception ex_62FD)
            {
                MessageBox.Show(string.Format(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_ErrorGuardarVuelo"), ex_62FD.Message), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnAplicar.Enabled = modoActual_62FD != "Consulta";
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            LimpiarCamposVuelo_62FD();
            ModoConsulta_62FD();
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();

        }

        private void dgvAsientos_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvAsientos.SelectedRows.Count == 0 || dgvAsientos.SelectedRows[0].DataBoundItem == null)
            {
                return;
            }

            Asiento_62FD asiento_62FD = (Asiento_62FD)dgvAsientos.SelectedRows[0].DataBoundItem;

            txtNroAsiento.Text = asiento_62FD.NroAsiento_62FD;
            cmbClase.Text = asiento_62FD.Clase_62FD;
            cmbEstadoAsiento.Text = asiento_62FD.EstadoAsiento_62FD;
        }

        private Asiento_62FD ObtenerAsientoSeleccionado_62FD()
        {
            if (dgvAsientos.SelectedRows.Count == 0 || dgvAsientos.SelectedRows[0].DataBoundItem == null)
            {
                MessageBox.Show(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_SeleccioneAsiento"), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Informacion"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return null;
            }

            return (Asiento_62FD)dgvAsientos.SelectedRows[0].DataBoundItem;
        }

        private void btnAñadirAsiento_Click(object sender, EventArgs e)
        {
            try
            {
                btnAñadirAsiento.Enabled = false;

                if (vueloSeleccionado_62FD == null)
                {
                    MessageBox.Show(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_SeleccioneVuelo"), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Informacion"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtNroAsiento.Text) || cmbClase.SelectedIndex < 0)
                {
                    MessageBox.Show(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_CompletarDatosAsientoAlta"), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Informacion"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                Asiento_62FD asiento_62FD = new Asiento_62FD();
                asiento_62FD.NroVuelo_62FD = vueloSeleccionado_62FD.NroVuelo_62FD;
                asiento_62FD.NroAsiento_62FD = txtNroAsiento.Text.Trim().ToUpper();
                asiento_62FD.Clase_62FD = cmbClase.Text;

                int resultado_62FD = asientoBLL_62FD.AgregarAsiento_62FD(asiento_62FD);

                if (resultado_62FD == 0)
                {
                    MessageBox.Show(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_AsientoExistente"), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Advertencia"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (resultado_62FD < 0)
                {
                    MessageBox.Show(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_NoSePudoAgregarAsiento"), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                RegistrarEventoBitacora_62FD("Agregar Asiento", 2);

                CargarGrillaAsientos_62FD();
            }
            catch (Exception ex_62FD)
            {
                MessageBox.Show(string.Format(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_ErrorAgregarAsiento"), ex_62FD.Message), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnAñadirAsiento.Enabled = true;
            }
        }

        private void btnModificarAsiento_Click(object sender, EventArgs e)
        {
            try
            {
                btnModificarAsiento.Enabled = false;

                Asiento_62FD seleccionado_62FD = ObtenerAsientoSeleccionado_62FD();

                if (seleccionado_62FD == null)
                {
                    return;
                }

                if (cmbClase.SelectedIndex < 0 || cmbEstadoAsiento.SelectedIndex < 0)
                {
                    MessageBox.Show(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_CompletarDatosAsientoModificacion"), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Informacion"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                Asiento_62FD asiento_62FD = new Asiento_62FD();
                asiento_62FD.IdAsiento_62FD = seleccionado_62FD.IdAsiento_62FD;
                asiento_62FD.Clase_62FD = cmbClase.Text;
                asiento_62FD.EstadoAsiento_62FD = cmbEstadoAsiento.Text;

                if (!asientoBLL_62FD.ModificarAsiento_62FD(asiento_62FD))
                {
                    MessageBox.Show(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_NoSePudoModificarAsiento"), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                RegistrarEventoBitacora_62FD("Modificar Asiento", 2);

                CargarGrillaAsientos_62FD();
            }
            catch (Exception ex_62FD)
            {
                MessageBox.Show(string.Format(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_ErrorModificarAsiento"), ex_62FD.Message), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnModificarAsiento.Enabled = true;
            }
        }

        private void btnEliminarAsiento_Click(object sender, EventArgs e)
        {
            try
            {
                btnEliminarAsiento.Enabled = false;

                Asiento_62FD asiento_62FD = ObtenerAsientoSeleccionado_62FD();

                if (asiento_62FD == null)
                {
                    return;
                }

                if (MessageBox.Show(string.Format(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_ConfirmarEliminarAsiento"), asiento_62FD.NroAsiento_62FD, vueloSeleccionado_62FD.NroVuelo_62FD), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Confirmar"), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    return;
                }

                int resultado_62FD = asientoBLL_62FD.EliminarAsiento_62FD(asiento_62FD.IdAsiento_62FD);

                if (resultado_62FD == 0)
                {
                    MessageBox.Show(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_AsientoNoEliminable"), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Advertencia"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (resultado_62FD < 0)
                {
                    MessageBox.Show(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_NoSePudoEliminarAsiento"), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                RegistrarEventoBitacora_62FD("Eliminar Asiento", 3);

                CargarGrillaAsientos_62FD();
            }
            catch (Exception ex_62FD)
            {
                MessageBox.Show(string.Format(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_ErrorEliminarAsiento"), ex_62FD.Message), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnEliminarAsiento.Enabled = true;
            }
        }

        private void btnSerializar_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtRutaSerializar.Text))
                {
                    MessageBox.Show(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_SeleccioneRutaSerializar"), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Informacion"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (listaVuelos_62FD.Count == 0)
                {
                    MessageBox.Show(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_SinDatosSerializar"), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Informacion"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                serializarBLL_62DB.Serializar_62DB(listaVuelos_62FD, txtRutaSerializar.Text);

                RegistrarEventoBitacora_62FD("Serializar Vuelos", 1);

                MessageBox.Show(string.Format(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_VuelosSerializados"), listaVuelos_62FD.Count), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Informacion"), MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex_62FD)
            {
                MessageBox.Show(string.Format(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_ErrorSerializar"), ex_62FD.Message), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRutaSerializar_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialogo_62FD = new SaveFileDialog();
            dialogo_62FD.Filter = IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_FiltroArchivosXml");
            dialogo_62FD.DefaultExt = "xml";
            dialogo_62FD.AddExtension = true;
            dialogo_62FD.Title = IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_DialogoGuardarXml");

            if (dialogo_62FD.ShowDialog() == DialogResult.OK)
            {
                txtRutaSerializar.Text = dialogo_62FD.FileName;
            }
        }

        private void btnRutaDeserializar_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialogo_62FD = new OpenFileDialog();
            dialogo_62FD.Filter = IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_FiltroArchivosXml");
            dialogo_62FD.Title = IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_DialogoAbrirXml");

            if (dialogo_62FD.ShowDialog() == DialogResult.OK)
            {
                txtRutaDeserializar.Text = dialogo_62FD.FileName;
            }
        }

        private void btnDeserializar_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtRutaDeserializar.Text))
                {
                    MessageBox.Show(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_SeleccioneArchivoDeserializar"), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Informacion"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                listaVuelos_62FD = deserializarBLL_62DB.Deserializar_62DB<Vuelo_62FD>(txtRutaDeserializar.Text);
                grillaDesdeArchivo_62FD = true;

                EnlazarGrillaVuelos_62FD();

                LimpiarAsientos_62FD();
                ModoConsulta_62FD();

                RegistrarEventoBitacora_62FD("Deserializar Vuelos", 1);

                MessageBox.Show(string.Format(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_VuelosDeserializados"), listaVuelos_62FD.Count), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Informacion"), MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex_62FD)
            {
                MessageBox.Show(string.Format(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_ErrorDeserializar"), ex_62FD.Message), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LimpiarCamposVuelo_62FD()
        {
            txtNroVuelo.Clear();
            txtOrigen.Clear();
            txtDestino.Clear();
            txtMatricula.Clear();
            txtPrecioBase.Clear();
            cmbEstado.SelectedIndex = -1;
            dtFechaHoraSalida.Value = DateTime.Now;
        }


        private void ModoAlta_62FD()
        {
            modoActual_62FD = "Alta";
            PrepararModoEdicion_62FD();
            txtNroVuelo.ReadOnly = false;
            txtNroVuelo.Focus();
        }

        private void ModoModificacion_62FD()
        {
            modoActual_62FD = "Modificación";
            PrepararModoEdicion_62FD();
            txtNroVuelo.ReadOnly = true;
            txtOrigen.Focus();
        }

        private void PrepararModoEdicion_62FD()
        {
            groupBox2.Enabled = true;
            btnAplicar.Enabled = true;
            btnCancelar.Enabled = true;

            dgvVuelos.Enabled = false;
            btnActualizar.Enabled = false;
            btnLimpiar.Enabled = false;
            btnAñadir.Enabled = false;
            btnModificar.Enabled = false;
            btnEliminar.Enabled = false;
            btnSerializar.Enabled = false;
            btnDeserializar.Enabled = false;
            btnRutaSerializar.Enabled = false;
            btnRutaDeserializar.Enabled = false;

            groupBox3.Enabled = false;

            ActualizarBarraEstado_62FD();
        }

        // Código IATA: exactamente tres letras A-Z (sin tildes ni ñ), sin importar mayúsculas
        private bool EsCodigoIata_62FD(string texto_62FD)
        {
            string codigo_62FD = texto_62FD.Trim().ToUpperInvariant();

            if (codigo_62FD.Length != 3)
            {
                return false;
            }

            foreach (char c_62FD in codigo_62FD)
            {
                if (c_62FD < 'A' || c_62FD > 'Z')
                {
                    return false;
                }
            }

            return true;
        }

        // Solo letras A-Z al tipear; lo pegado lo sigue frenando la validación de btnAplicar_Click
        private void SoloLetras_62FD(object sender, KeyPressEventArgs e)
        {
            char tecla_62FD = char.ToUpperInvariant(e.KeyChar);

            if (!char.IsControl(e.KeyChar) && (tecla_62FD < 'A' || tecla_62FD > 'Z'))
            {
                e.Handled = true;
            }
        }

        // Solo dígitos y los separadores de la cultura actual; lo pegado lo sigue frenando la validación de btnAplicar_Click
        private void SoloImporte_62FD(object sender, KeyPressEventArgs e)
        {
            NumberFormatInfo formato_62FD = CultureInfo.CurrentCulture.NumberFormat;
            string tecla_62FD = e.KeyChar.ToString();

            if (char.IsControl(e.KeyChar) || char.IsDigit(e.KeyChar) || tecla_62FD == formato_62FD.NumberDecimalSeparator || tecla_62FD == formato_62FD.NumberGroupSeparator)
            {
                return;
            }

            e.Handled = true;
        }

        private void RegistrarEventoBitacora_62FD(string evento_62FD, int criticidad_62FD)
        {
            try
            {
                eventoBLL_23DB.RegistrarEvento_23DB(SessionManager_23DB.ObtenerInstancia_23DB().DNI_23DB, "Maestros", evento_62FD, criticidad_62FD);
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

        private void dgvVuelos_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridViewColumn columna_62FD = dgvVuelos.Columns[e.ColumnIndex];
            bool ascendente_62FD = columnaOrdenVuelos_62FD == columna_62FD.DataPropertyName ? !ordenAscendenteVuelos_62FD : true;

            listaVuelos_62FD = Ordenar_62FD(listaVuelos_62FD, columna_62FD.DataPropertyName, ascendente_62FD);
            EnlazarGrillaVuelos_62FD();
            // Al reenlazar se dispara SelectionChanged: los asientos se vacían igual que al recargar los vuelos
            LimpiarAsientos_62FD();

            columnaOrdenVuelos_62FD = columna_62FD.DataPropertyName;
            ordenAscendenteVuelos_62FD = ascendente_62FD;

            // El glifo se pierde al reenlazar, por eso se pone después
            dgvVuelos.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection = ascendente_62FD ? SortOrder.Ascending : SortOrder.Descending;
        }

        private void dgvAsientos_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridViewColumn columna_62FD = dgvAsientos.Columns[e.ColumnIndex];
            ordenAscendenteAsientos_62FD = columnaOrdenAsientos_62FD == columna_62FD.DataPropertyName ? !ordenAscendenteAsientos_62FD : true;
            columnaOrdenAsientos_62FD = columna_62FD.DataPropertyName;

            listaAsientos_62FD = Ordenar_62FD(listaAsientos_62FD, columnaOrdenAsientos_62FD, ordenAscendenteAsientos_62FD);
            dgvAsientos.DataSource = null;
            dgvAsientos.DataSource = listaAsientos_62FD;
            Traductor_23DB.TraducirGrilla_23DB(dgvAsientos, "MaestroVuelos");
            LimpiarSeleccionGrilla_62FD(dgvAsientos);
            LimpiarCamposAsiento_62FD();

            // El glifo se pierde al reenlazar, por eso se pone después
            dgvAsientos.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection = ordenAscendenteAsientos_62FD ? SortOrder.Ascending : SortOrder.Descending;
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

        private void MaestroVuelos_62FD_FormClosing(object sender, FormClosingEventArgs e)
        {
            Observer_23DB.ObtenerInstancia_23DB().Desuscribir_23DB(this);

        }
    }
}
