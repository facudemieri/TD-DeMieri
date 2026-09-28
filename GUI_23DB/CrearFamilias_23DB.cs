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
    public partial class CrearFamilias_23DB : Form, IIdiomaObserver_23DB
    {
        private FamiliaBLL_23DB familiaBLL_23DB = new FamiliaBLL_23DB();
        private PatenteBLL_23DB patenteBLL_23DB = new PatenteBLL_23DB();
        private EventoBLL_23DB eventoBLL_23DB = new EventoBLL_23DB();
        private List<Rol_23DB> componentes_23DB = new List<Rol_23DB>();
        private Familia_23DB familiaSeleccionada_23DB = null;
        private bool modoModificar_23DB = false;

        private void ModoConsulta_23DB()
        {
            btnCrear.Enabled = true;
            btnModificar.Enabled = false;
            btnEliminar.Enabled = false;
            btnAplicar.Enabled = false;
            btnQuitar.Enabled = false;
            btnCancelar.Enabled = true;
        }

        private void ModoModificar_23DB()
        {
            btnCrear.Enabled = false;
            btnModificar.Enabled = false;
            btnEliminar.Enabled = false;
            btnAplicar.Enabled = true;
            btnQuitar.Enabled = true;
            btnCancelar.Enabled = true;
        }


        public CrearFamilias_23DB()
        {
            InitializeComponent();
            Observer_23DB.ObtenerInstancia_23DB().Suscribir_23DB(this);
            AplicarIdiomaActual_23DB();
        }

        private void CargarListBox_23DB()
        {
            lstPatFam.Items.Clear();
            List<Patente_23DB> patentes_23DB = patenteBLL_23DB.ObtenerPatentes_23DB();
            List<Familia_23DB> familias_23DB = familiaBLL_23DB.ObtenerFamilias_23DB();
            foreach(Patente_23DB patente_23DB in patentes_23DB)
            {
                lstPatFam.Items.Add(patente_23DB);
            }
                
            foreach(Familia_23DB familia_23DB in familias_23DB)
            {
                lstPatFam.Items.Add(familia_23DB);
            }
                
        }

        private void AgregarAlTreeView_23DB(Rol_23DB componente_23DB)
        {
            TreeNode nodo_23DB = new TreeNode(componente_23DB.ObtenerNombre_23DB());
            nodo_23DB.Tag = componente_23DB;
            AgregarHijosAlNodo_23DB(nodo_23DB, componente_23DB);
            tvListaPatFam.Nodes.Add(nodo_23DB);
            tvListaPatFam.ExpandAll();
        }

        private void AgregarHijosAlNodo_23DB(TreeNode nodo_23DB, Rol_23DB componente_23DB)
        {
            foreach(Rol_23DB hijo_23DB in componente_23DB.ObtenerHijos_23DB())
            {
                TreeNode nodoHijo_23DB = new TreeNode(hijo_23DB.ObtenerNombre_23DB());
                nodoHijo_23DB.Tag = hijo_23DB;
                AgregarHijosAlNodo_23DB(nodoHijo_23DB, hijo_23DB);
                nodo_23DB.Nodes.Add(nodoHijo_23DB);
            }
        }

        private void LimpiarCampos_23DB()
        {
            txtNombre.Text = string.Empty;
            tvListaPatFam.Nodes.Clear();
            componentes_23DB.Clear();
            ModoConsulta_23DB();
        }

        public void ActualizarIdioma_23DB(Dictionary<string, string> configuracion_23DB)
        {
            string formName_23DB = "CrearFamilias";
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

        private void btnSalir_Click(object sender, EventArgs e)
        {        
            this.Close();
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

        private void CrearFamilias_23DB_Load(object sender, EventArgs e)
        {
            CargarListBox_23DB();
            ModoConsulta_23DB();
            
        }

        private void lstPatFam_DoubleClick_1(object sender, EventArgs e)
        {
            if(lstPatFam.SelectedItem == null) return;
            Rol_23DB seleccionado_23DB = (Rol_23DB)lstPatFam.SelectedItem;

            if(seleccionado_23DB is Familia_23DB)
            {
                seleccionado_23DB = familiaBLL_23DB.ObtenerFamiliaCompleta_23DB(((Familia_23DB)seleccionado_23DB).IdFamilia_23DB);
                List<string> patentesTreeView_23DB = ObtenerPatentesDelTreeView_23DB();
                if(familiaBLL_23DB.TienePatentesRepetidas_23DB(seleccionado_23DB, patentesTreeView_23DB))
                {
                    MessageBox.Show("La Familia contiene patentes que ya están incluidas.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            // Validación para Patentes directas
            if(seleccionado_23DB is Patente_23DB)
            {
                List<string> patentesTreeView_23DB = ObtenerPatentesDelTreeView_23DB();
                if(patentesTreeView_23DB.Contains(seleccionado_23DB.ObtenerNombre_23DB()))
                {
                    MessageBox.Show($"La patente '{seleccionado_23DB.ObtenerNombre_23DB()}' ya está incluida.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            // Verificar si ya existe en componentes
            foreach(Rol_23DB comp_23DB in componentes_23DB)
            {
                if(comp_23DB.ObtenerNombre_23DB() == seleccionado_23DB.ObtenerNombre_23DB())
                {
                    MessageBox.Show("El elemento ya fue agregado.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            componentes_23DB.Add(seleccionado_23DB);
            AgregarAlTreeView_23DB(seleccionado_23DB);
            lstPatFam.Items.Remove(lstPatFam.SelectedItem);
        }

        private List<string> ObtenerPatentesDeRol_23DB(Rol_23DB rol_23DB)
        {
            List<string> patentes_23DB = new List<string>();
            foreach(Rol_23DB hijo_23DB in rol_23DB.ObtenerHijos_23DB())
            {
                if(hijo_23DB is Patente_23DB)
                { 
                    patentes_23DB.Add(hijo_23DB.ObtenerNombre_23DB()); 
                }
                else
                { 
                    patentes_23DB.AddRange(ObtenerPatentesDeRol_23DB(hijo_23DB)); 
                }
            }
            return patentes_23DB;
        }
        private List<string> ObtenerPatentesDelTreeView_23DB()
        {
            List<string> patentes_23DB = new List<string>();
            foreach(TreeNode nodo_23DB in tvListaPatFam.Nodes)
            { 
                ObtenerPatentesDeNodo_23DB(nodo_23DB, patentes_23DB); 
            }
            return patentes_23DB;


        }

        private TreeNode CrearNodoRecursivo_23DB(Rol_23DB componente_23DB)
        {
            TreeNode nodo_23DB = new TreeNode(componente_23DB.ObtenerNombre_23DB());
            nodo_23DB.Tag = componente_23DB;
            foreach(Rol_23DB hijo_23DB in componente_23DB.ObtenerHijos_23DB())
            { 
                nodo_23DB.Nodes.Add(CrearNodoRecursivo_23DB(hijo_23DB)); 
            }
            return nodo_23DB;
        }
        private void ObtenerPatentesDeNodo_23DB(TreeNode nodo_23DB, List<string> patentes_23DB)
        {
            Rol_23DB componente_23DB = (Rol_23DB)nodo_23DB.Tag;
            if(componente_23DB is Patente_23DB)
            {
                if(!patentes_23DB.Contains(componente_23DB.ObtenerNombre_23DB()))
                { 
                    patentes_23DB.Add(componente_23DB.ObtenerNombre_23DB()); 
                }
            }
            foreach(TreeNode hijo_23DB in nodo_23DB.Nodes)
            { 
                ObtenerPatentesDeNodo_23DB(hijo_23DB, patentes_23DB); 
            }
        }

        private void btnCrear_Click_1(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(txtNombre.Text))
            {
                MessageBox.Show("Debe ingresar un nombre para la Familia.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if(!familiaBLL_23DB.ValidarElementos_23DB(componentes_23DB))
            {
                MessageBox.Show("Debe seleccionar al menos un elemento.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if(!familiaBLL_23DB.ValidarNombre_23DB(txtNombre.Text, familiaBLL_23DB.ObtenerFamilias_23DB()))
            {
                MessageBox.Show("Ya existe una Familia con ese nombre.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            familiaBLL_23DB.CrearFamilia_23DB(txtNombre.Text, componentes_23DB);
            eventoBLL_23DB.RegistrarEvento_23DB(SessionManager_23DB.ObtenerInstancia_23DB().DNI_23DB, "Administrador", "Crear Familia", 2);
            MessageBox.Show("Familia creada correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            LimpiarCampos_23DB();
            CargarListBox_23DB();
        }

        private void btnCancelar_Click_1(object sender, EventArgs e)
        {
            LimpiarCampos_23DB();
            CargarListBox_23DB();
            ModoConsulta_23DB();
        }

        private void CrearFamilias_23DB_FormClosing_1(object sender, FormClosingEventArgs e)
        {
            Observer_23DB.ObtenerInstancia_23DB().Desuscribir_23DB(this);
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            if(lstPatFam.SelectedItem == null)
            {
                MessageBox.Show("Debe seleccionar una Familia.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if(!(lstPatFam.SelectedItem is Familia_23DB))
            {
                MessageBox.Show("Solo se pueden modificar Familias.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Familia_23DB seleccionada_23DB = (Familia_23DB)lstPatFam.SelectedItem;
            familiaSeleccionada_23DB = familiaBLL_23DB.ObtenerFamiliaCompleta_23DB(seleccionada_23DB.IdFamilia_23DB);

            modoModificar_23DB = true;
            txtNombre.Text = familiaSeleccionada_23DB.NombreFamilia_23DB;
            tvListaPatFam.Nodes.Clear();
            componentes_23DB.Clear();

            
            lstPatFam.Items.Remove(lstPatFam.SelectedItem);

           // vuelvo a armar el treeview con la familia como nodo raiz
            TreeNode nodoFamilia_23DB = new TreeNode(familiaSeleccionada_23DB.ObtenerNombre_23DB());
            nodoFamilia_23DB.Tag = familiaSeleccionada_23DB;


            foreach(Rol_23DB componente_23DB in familiaSeleccionada_23DB.ObtenerHijos_23DB())
            {
                componentes_23DB.Add(componente_23DB);
                nodoFamilia_23DB.Nodes.Add(CrearNodoRecursivo_23DB(componente_23DB));

                foreach(object item_23DB in lstPatFam.Items.Cast<object>().ToList())
                {
                    if(item_23DB is Rol_23DB rol_23DB && rol_23DB.ObtenerNombre_23DB() == componente_23DB.ObtenerNombre_23DB())
                    {
                        lstPatFam.Items.Remove(item_23DB);
                        break;
                    }
                }
            }

            tvListaPatFam.Nodes.Add(nodoFamilia_23DB);
            tvListaPatFam.ExpandAll();
            ModoModificar_23DB();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            
            if(lstPatFam.SelectedItem == null)
            {
                MessageBox.Show("Debe seleccionar una Familia.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if(!(lstPatFam.SelectedItem is Familia_23DB))
            {
                MessageBox.Show("Solo se pueden eliminar Familias.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }


            Familia_23DB familia_23DB = (Familia_23DB)lstPatFam.SelectedItem;

            if(familiaBLL_23DB.FamiliaEstaEnRol_23DB(familia_23DB.IdFamilia_23DB))
            {
                MessageBox.Show("No se puede eliminar una Familia que está siendo usada por un Rol.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if(familiaBLL_23DB.FamiliaEstaEnFamilia_23DB(familia_23DB.IdFamilia_23DB))
            {
                MessageBox.Show("No se puede eliminar una Familia que está siendo usada por otra Familia.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            } 

            Familia_23DB familiaCompleta_23DB = familiaBLL_23DB.ObtenerFamiliaCompleta_23DB(familia_23DB.IdFamilia_23DB);
            if(familiaBLL_23DB.TieneComponentes_23DB(familia_23DB.IdFamilia_23DB))
            {
                MessageBox.Show("No se puede eliminar una Familia que tiene componentes.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult resultado_23DB = MessageBox.Show($"¿Está seguro que desea eliminar la Familia {familia_23DB.NombreFamilia_23DB}?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if(resultado_23DB == DialogResult.Yes)
            {
                familiaBLL_23DB.EliminarFamilia_23DB(familia_23DB.IdFamilia_23DB);
                eventoBLL_23DB.RegistrarEvento_23DB(SessionManager_23DB.ObtenerInstancia_23DB().DNI_23DB, "Administrador", "Eliminar Familia", 2);
                MessageBox.Show("Familia eliminada correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LimpiarCampos_23DB();
                CargarListBox_23DB();
            }
            ModoConsulta_23DB();
        }

        private void AgregarAListBoxSiNoExiste_23DB(Rol_23DB componente_23DB)
        {
            foreach(object item_23DB in lstPatFam.Items)
            {
                if(item_23DB is Rol_23DB rol_23DB && rol_23DB.ObtenerNombre_23DB() == componente_23DB.ObtenerNombre_23DB()) return;
            }
            lstPatFam.Items.Add(componente_23DB);
        }

        private void btnQuitar_Click(object sender, EventArgs e)
        {
            if(tvListaPatFam.SelectedNode == null)
            {
                MessageBox.Show("Debe seleccionar un elemento.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            TreeNode nodoSeleccionado_23DB = tvListaPatFam.SelectedNode;
            Rol_23DB componente_23DB = (Rol_23DB)nodoSeleccionado_23DB.Tag;

            if(nodoSeleccionado_23DB.Parent == null)
            {
                // nodo raiz
                componentes_23DB.Remove(componente_23DB);
                AgregarAListBoxSiNoExiste_23DB(componente_23DB);
                tvListaPatFam.Nodes.Remove(nodoSeleccionado_23DB);
            }
            else
            {
                Rol_23DB padre_23DB = (Rol_23DB)nodoSeleccionado_23DB.Parent.Tag;
                padre_23DB.Quitar_23DB(componente_23DB);
                componentes_23DB.Remove(componente_23DB);
                AgregarAListBoxSiNoExiste_23DB(componente_23DB);
                nodoSeleccionado_23DB.Parent.Nodes.Remove(nodoSeleccionado_23DB);
            }


        }

        private void btnAplicar_Click(object sender, EventArgs e)
        {
            if(!modoModificar_23DB) return;

            if(string.IsNullOrEmpty(txtNombre.Text))
            {
                MessageBox.Show("Debe ingresar un nombre para la Familia.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if(!familiaBLL_23DB.ValidarNombre_23DB(txtNombre.Text, familiaBLL_23DB.ObtenerFamilias_23DB().FindAll(f => f.IdFamilia_23DB != familiaSeleccionada_23DB.IdFamilia_23DB)))
            {
                MessageBox.Show("Ya existe una Familia con ese nombre.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            familiaBLL_23DB.ModificarFamilia_23DB(familiaSeleccionada_23DB.IdFamilia_23DB, txtNombre.Text, componentes_23DB);
            eventoBLL_23DB.RegistrarEvento_23DB(SessionManager_23DB.ObtenerInstancia_23DB().DNI_23DB, "Administrador", "Modificar Familia", 2);
            MessageBox.Show("Familia modificada correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            modoModificar_23DB = false;
            familiaSeleccionada_23DB = null;
            LimpiarCampos_23DB();
            CargarListBox_23DB();
            ModoConsulta_23DB();
        }

        private void lstPatFam_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(lstPatFam.SelectedItem is Familia_23DB)
            {
                btnModificar.Enabled = true;
                btnEliminar.Enabled = true;
            }
            else
            {
                btnModificar.Enabled = false;
                btnEliminar.Enabled = false;
            }
        }
    }
}
