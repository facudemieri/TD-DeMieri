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
    public partial class MenuPrincipal_23DB : Form, IIdiomaObserver_23DB
    {
      
        public MenuPrincipal_23DB(List<string> patentes_23DB)
        {
            InitializeComponent();
            CargarDatosSesion_23DB();
            Observer_23DB.ObtenerInstancia_23DB().Suscribir_23DB(this);
            AplicarIdiomaActual_23DB();
            AplicarPatentes_23DB(patentes_23DB);
        }
        private void AplicarPatentes_23DB(List<string> patentes_23DB)
        {
            bool gestionUsuarios_23DB = patentes_23DB.Contains("Gestion de Usuarios");
            bool gestionBitacora_23DB = patentes_23DB.Contains("Gestion de Bitacora");
            bool gestionPerfiles_23DB = patentes_23DB.Contains("Gestion de Perfiles");
            bool gestionRespaldo_23DB = patentes_23DB.Contains("Gestion de Respaldo");

            gestionDeUsuariosToolStripMenuItem.Visible = gestionUsuarios_23DB;
            bitacoraDeEventosToolStripMenuItem.Visible = gestionBitacora_23DB;
            gestionDePerfilesToolStripMenuItem.Visible = gestionPerfiles_23DB;
            gestionDeRespaldosToolStripMenuItem.Visible = gestionRespaldo_23DB;

            btnAdmin.Visible = gestionUsuarios_23DB || gestionBitacora_23DB || gestionPerfiles_23DB || gestionRespaldo_23DB;

            cambiarContraseñaToolStripMenuItem.Visible = patentes_23DB.Contains("Cambio de Clave");

            btnPasaje.Visible = patentes_23DB.Contains("Venta de Pasajes");

            bool maestroAeronaves_23DB = patentes_23DB.Contains("Maestro de Aeronaves");
            bool maestroVuelos_23DB = patentes_23DB.Contains("Maestro de Vuelos");

            maestroAeronavesToolStripMenuItem.Visible = maestroAeronaves_23DB;
            maestroVuelosToolStripMenuItem.Visible = maestroVuelos_23DB;

            btnMaestros.Visible = maestroAeronaves_23DB || maestroVuelos_23DB;

            bool generarOrden_62FD = patentes_23DB.Contains("Generar Orden de Trabajo");
            bool registrarIntervencion_62FD = patentes_23DB.Contains("Registrar Intervencion");
            bool certificarIntervencion_62FD = patentes_23DB.Contains("Certificar Intervencion");

            generarOrdenDeTrabajoToolStripMenuItem.Visible = generarOrden_62FD;
            registrarIntervenciónToolStripMenuItem.Visible = registrarIntervencion_62FD;
            certificarIntervenciónToolStripMenuItem.Visible = certificarIntervencion_62FD;

            btnAeronaves.Visible = generarOrden_62FD || registrarIntervencion_62FD || certificarIntervencion_62FD;
        }

        private EventoBLL_23DB eventoBLL_23DB = new EventoBLL_23DB();

        private void CargarDatosSesion_23DB()
        {
            SessionManager_23DB sesion_23DB = SessionManager_23DB.ObtenerInstancia_23DB();
            lblLoginMp.Text = string.Format(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_Bienvenido"), sesion_23DB.Login_23DB);
            lblRol.Text = string.Format(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_RolSesion"), sesion_23DB.Rol_23DB);
        }

        private void MenuPrincipal_23DB_Load(object sender, EventArgs e)
        {
            
        }

        private void btnUsuario_Click(object sender, EventArgs e)
        {
            cmsUsuario.Show(btnUsuario, new System.Drawing.Point(btnUsuario.Width, 0));
        }

        private void btnAdmin_Click(object sender, EventArgs e)
        {

            cmsAeronaves.Show(btnAeronaves, new System.Drawing.Point(btnAeronaves.Width, 0));

        }

        private void cambiarContraseñaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CambiarContraseña_23DB cambiarContraseñaForm_23DB = new CambiarContraseña_23DB();
            cambiarContraseñaForm_23DB.ShowDialog();
        }

        private void cambiarIdiomaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CambiarIdioma_23DB cambiarIdioma_23DB = new CambiarIdioma_23DB();
            cambiarIdioma_23DB.ShowDialog();
        }

        private void reloginToolStripMenuItem_Click(object sender, EventArgs e)
        {

            InicioSesion_23DB login_23DB = new InicioSesion_23DB();
            login_23DB.EsRelogin_23DB = true;
            DialogResult resultado_23DB = login_23DB.ShowDialog();
            
            if(resultado_23DB == DialogResult.OK)
            {
                CargarDatosSesion_23DB();
                AplicarIdiomaActual_23DB();
                RolBLL_23DB rolBLL_23DB = new RolBLL_23DB();
                Rol_23DB rol_23DB = rolBLL_23DB.ObtenerRolPorNombre_23DB(SessionManager_23DB.ObtenerInstancia_23DB().Rol_23DB);
                if(rol_23DB != null)
                {
                    AplicarPatentes_23DB(rolBLL_23DB.ObtenerPatentesDeRol_23DB(rol_23DB.IdRol_23DB));
                }
            }

        }

        private void gestionDeUsuariosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GestionUsuario_23DB gestionUsuarios_23DB = new GestionUsuario_23DB();
            gestionUsuarios_23DB.ShowDialog();
        }

        private void bitacoraDeEventosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AuditoriaEventos_23DB auditEventos_23DB = new AuditoriaEventos_23DB();
            auditEventos_23DB.ShowDialog();
        }

        private void gestionDePerfilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            DialogResult resultado_23DB = MessageBox.Show(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_ConfirmarCerrarSesion"), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Confirmar"), MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if(resultado_23DB == DialogResult.Yes)
            {
                EventoBLL_23DB eventoBLL_23DB = new EventoBLL_23DB();
                string dni_23DB = SessionManager_23DB.ObtenerInstancia_23DB().DNI_23DB;

                try
                {
                    eventoBLL_23DB.RegistrarEvento_23DB(dni_23DB, "Usuarios", "Logout", 1);
                }
                catch(Exception ex_23DB)
                {
                    System.Diagnostics.Debug.WriteLine(ex_23DB);
                }
                SessionManager_23DB.ObtenerInstancia_23DB().CerrarSesion_23DB();
                Application.Exit();
            }
        }

        private void btnAyuda_Click(object sender, EventArgs e) //admin nuevo
        {
            cmsAdmin.Show(btnAeronaves, new System.Drawing.Point(btnAeronaves.Width, 0));
            
        }

        private void btnReportes_Click(object sender, EventArgs e)
        {
            cmsPasajes.Show(btnPasaje, new System.Drawing.Point(btnPasaje.Width, 0));

        }

        private void cmsAdmin_Opening(object sender, CancelEventArgs e)
        {

        }

        private void administradorPerfilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CrearFamilias_23DB crearFamilias_23DB = new CrearFamilias_23DB();
            crearFamilias_23DB.Show();
        }

        private void administradorRToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AdministradorRoles_23DB administrador = new AdministradorRoles_23DB();
            administrador.Show();
        }

        public void ActualizarIdioma_23DB(Dictionary<string, string> configuracion_23DB)
        {
            Traductor_23DB.AplicarIdioma_23DB(this, configuracion_23DB, cmsUsuario, cmsAdmin, cmsPasajes, cmsMaestros, cmsAeronaves);
            CargarDatosSesion_23DB(); // lblLoginMp y lblRol llevan datos de la sesión, los arma el formulario
        }

        private void MenuPrincipal_23DB_FormClosing(object sender, FormClosingEventArgs e)
        {
            Observer_23DB.ObtenerInstancia_23DB().Desuscribir_23DB(this);
        }

        public void AplicarIdiomaActual_23DB()
        {
            string idiomaActual_23DB = SessionManager_23DB.ObtenerInstancia_23DB().UltimoIdioma_23DB;
            if(string.IsNullOrEmpty(idiomaActual_23DB))
            {
                idiomaActual_23DB = "español";
            }
            
            IdiomaBLL_23DB idiomaBLL_23DB = new IdiomaBLL_23DB();
            ActualizarIdioma_23DB(idiomaBLL_23DB.CargarConfiguracion_23DB(idiomaActual_23DB));
        }

        private void administradorDeRolesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AdministradorRoles_23DB administradorRoles_23DB = new AdministradorRoles_23DB();
            administradorRoles_23DB.Show();
        }

        private void administradorDePerfilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CrearFamilias_23DB crearFamilias_23DB = new CrearFamilias_23DB();
            crearFamilias_23DB.Show();
        }

        private void gestionDeRespaldosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GestionRespaldo_23DB gestionRespaldo_23DB = new GestionRespaldo_23DB();
            gestionRespaldo_23DB.Show();
        }

        private void btnMantenimiento_Click(object sender, EventArgs e) //AYUDA
        {
            MessageBox.Show(IdiomaBLL_23DB.ObtenerTexto_23DB("Msg_FuncionEnDesarrollo"), IdiomaBLL_23DB.ObtenerTexto_23DB("Titulo_Informacion"), MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnReporte_Click(object sender, EventArgs e)
        {
            cmsMaestros.Show(btnMaestros, new System.Drawing.Point(btnMaestros.Width, 0));
        }

        private void reservasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GenerarPasaje_62FD generarPasaje_62FD = new GenerarPasaje_62FD();
            generarPasaje_62FD.Show();
        }

        private void vuelosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GenerarReserva_62FD generarReserva_62FD = new GenerarReserva_62FD();
            generarReserva_62FD.Show();
        }

        private void generarOrdenDeTrabajoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SeleccionarAeronave_62FD seleccionarAeronave_62FD = new SeleccionarAeronave_62FD();
            seleccionarAeronave_62FD.ShowDialog(); 
        }

        private void registrarIntervenciónToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RegistrarIntervencion_62FD registrarIntervencion_62FD = new RegistrarIntervencion_62FD();
            registrarIntervencion_62FD.ShowDialog();
        }

        private void certificarIntervenciónToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CertificarIntervencion_62FD certificarIntervencion_62FD = new CertificarIntervencion_62FD(); ;
            certificarIntervencion_62FD.ShowDialog();
        }

        private void cmsAeronaves_Opening(object sender, CancelEventArgs e)
        {

        }

        private void maestroAeronavesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MaestroAeronaves_62FD maestroAeronaves_62FD = new MaestroAeronaves_62FD();
            maestroAeronaves_62FD.Show();
        }

        private void maestroVuelosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MaestroVuelos_62FD maestroVuelos_62FD = new MaestroVuelos_62FD();
            maestroVuelos_62FD.Show();
        }
    }
}
