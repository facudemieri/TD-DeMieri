namespace GUI_23DB
{
    partial class MaestroVuelos_62FD
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MaestroVuelos_62FD));
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dgvVuelos = new System.Windows.Forms.DataGridView();
            this.btnActualizar = new System.Windows.Forms.Button();
            this.btnAñadir = new System.Windows.Forms.Button();
            this.btnModificar = new System.Windows.Forms.Button();
            this.btnEliminar = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.txtNroVuelo = new System.Windows.Forms.TextBox();
            this.txtPrecioBase = new System.Windows.Forms.TextBox();
            this.txtMatricula = new System.Windows.Forms.TextBox();
            this.txtOrigen = new System.Windows.Forms.TextBox();
            this.txtDestino = new System.Windows.Forms.TextBox();
            this.dtFechaHoraSalida = new System.Windows.Forms.DateTimePicker();
            this.cmbEstado = new System.Windows.Forms.ComboBox();
            this.dgvAsientos = new System.Windows.Forms.DataGridView();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.cmbClase = new System.Windows.Forms.ComboBox();
            this.cmbEstadoAsiento = new System.Windows.Forms.ComboBox();
            this.txtNroAsiento = new System.Windows.Forms.TextBox();
            this.btnAñadirAsiento = new System.Windows.Forms.Button();
            this.btnModificarAsiento = new System.Windows.Forms.Button();
            this.btnEliminarAsiento = new System.Windows.Forms.Button();
            this.btnSerializar = new System.Windows.Forms.Button();
            this.btnDeserializar = new System.Windows.Forms.Button();
            this.txtRutaSerializar = new System.Windows.Forms.TextBox();
            this.txtRutaDeserializar = new System.Windows.Forms.TextBox();
            this.btnRutaSerializar = new System.Windows.Forms.Button();
            this.btnRutaDeserializar = new System.Windows.Forms.Button();
            this.btnSalir = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.btnAplicar = new System.Windows.Forms.Button();
            this.colNroVuelo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colOrigen = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDestino = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colFechaHoraSalida = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMatriculaVuelo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPrecio = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEstadoVuelo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colIdAsiento = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNroAsiento = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colClase = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEstadoAsiento = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnLimpiar = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvVuelos)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAsientos)).BeginInit();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.RoyalBlue;
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1050, 60);
            this.panel1.TabIndex = 12;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.RoyalBlue;
            this.label1.Font = new System.Drawing.Font("Century Gothic", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.Control;
            this.label1.Location = new System.Drawing.Point(16, 12);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(364, 25);
            this.label1.TabIndex = 8;
            this.label1.Text = "Maestro de Vuelos - AeroManager";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnLimpiar);
            this.groupBox1.Controls.Add(this.btnEliminar);
            this.groupBox1.Controls.Add(this.btnModificar);
            this.groupBox1.Controls.Add(this.btnAñadir);
            this.groupBox1.Controls.Add(this.btnActualizar);
            this.groupBox1.Controls.Add(this.dgvVuelos);
            this.groupBox1.Location = new System.Drawing.Point(12, 67);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(875, 227);
            this.groupBox1.TabIndex = 13;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Vuelos";
            // 
            // dgvVuelos
            // 
            this.dgvVuelos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvVuelos.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colNroVuelo,
            this.colOrigen,
            this.colDestino,
            this.colFechaHoraSalida,
            this.colMatriculaVuelo,
            this.colPrecio,
            this.colEstadoVuelo});
            this.dgvVuelos.Location = new System.Drawing.Point(9, 23);
            this.dgvVuelos.Name = "dgvVuelos";
            this.dgvVuelos.Size = new System.Drawing.Size(712, 189);
            this.dgvVuelos.TabIndex = 0;
            this.dgvVuelos.SelectionChanged += new System.EventHandler(this.dgvVuelos_SelectionChanged);
            // 
            // btnActualizar
            // 
            this.btnActualizar.Location = new System.Drawing.Point(749, 23);
            this.btnActualizar.Name = "btnActualizar";
            this.btnActualizar.Size = new System.Drawing.Size(114, 24);
            this.btnActualizar.TabIndex = 1;
            this.btnActualizar.Text = "Actualizar";
            this.btnActualizar.UseVisualStyleBackColor = true;
            this.btnActualizar.Click += new System.EventHandler(this.btnActualizar_Click);
            // 
            // btnAñadir
            // 
            this.btnAñadir.Location = new System.Drawing.Point(749, 53);
            this.btnAñadir.Name = "btnAñadir";
            this.btnAñadir.Size = new System.Drawing.Size(114, 24);
            this.btnAñadir.TabIndex = 2;
            this.btnAñadir.Text = "Añadir";
            this.btnAñadir.UseVisualStyleBackColor = true;
            this.btnAñadir.Click += new System.EventHandler(this.btnAñadir_Click);
            // 
            // btnModificar
            // 
            this.btnModificar.Location = new System.Drawing.Point(749, 83);
            this.btnModificar.Name = "btnModificar";
            this.btnModificar.Size = new System.Drawing.Size(114, 24);
            this.btnModificar.TabIndex = 3;
            this.btnModificar.Text = "Modificar";
            this.btnModificar.UseVisualStyleBackColor = true;
            this.btnModificar.Click += new System.EventHandler(this.btnModificar_Click);
            // 
            // btnEliminar
            // 
            this.btnEliminar.Location = new System.Drawing.Point(749, 113);
            this.btnEliminar.Name = "btnEliminar";
            this.btnEliminar.Size = new System.Drawing.Size(114, 24);
            this.btnEliminar.TabIndex = 4;
            this.btnEliminar.Text = "Eliminar";
            this.btnEliminar.UseVisualStyleBackColor = true;
            this.btnEliminar.Click += new System.EventHandler(this.btnEliminar_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripStatusLabel2,
            this.toolStripStatusLabel3});
            this.statusStrip1.Location = new System.Drawing.Point(0, 774);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1050, 22);
            this.statusStrip1.TabIndex = 14;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(345, 17);
            this.toolStripStatusLabel1.Spring = true;
            this.toolStripStatusLabel1.Text = "Modo:";
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(345, 17);
            this.toolStripStatusLabel2.Spring = true;
            this.toolStripStatusLabel2.Text = "Usuario:";
            // 
            // toolStripStatusLabel3
            // 
            this.toolStripStatusLabel3.Name = "toolStripStatusLabel3";
            this.toolStripStatusLabel3.Size = new System.Drawing.Size(345, 17);
            this.toolStripStatusLabel3.Spring = true;
            this.toolStripStatusLabel3.Text = "Fecha:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.tableLayoutPanel1);
            this.groupBox2.Location = new System.Drawing.Point(12, 300);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(875, 200);
            this.groupBox2.TabIndex = 15;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Datos del Vuelo";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btnEliminarAsiento);
            this.groupBox3.Controls.Add(this.btnModificarAsiento);
            this.groupBox3.Controls.Add(this.btnAñadirAsiento);
            this.groupBox3.Controls.Add(this.tableLayoutPanel2);
            this.groupBox3.Controls.Add(this.dgvAsientos);
            this.groupBox3.Location = new System.Drawing.Point(12, 506);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(875, 186);
            this.groupBox3.TabIndex = 16;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Asientos del Vuelo Seleccionado";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 185F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 171F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 190F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 8F));
            this.tableLayoutPanel1.Controls.Add(this.dtFechaHoraSalida, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.label5, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.txtNroVuelo, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.txtOrigen, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.txtDestino, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.label7, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.txtPrecioBase, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.label8, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.txtMatricula, 3, 1);
            this.tableLayoutPanel1.Controls.Add(this.label9, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.cmbEstado, 3, 2);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(8, 22);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 38F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 41F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 21F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(813, 160);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(97, 17);
            this.label2.TabIndex = 0;
            this.label2.Text = "Nro de Vuelo:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 38);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(55, 17);
            this.label3.TabIndex = 1;
            this.label3.Text = "Origen:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 79);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(60, 17);
            this.label4.TabIndex = 2;
            this.label4.Text = "Destino:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 119);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(160, 17);
            this.label5.TabIndex = 3;
            this.label5.Text = "Fecha y Hora de Salida:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(359, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(85, 17);
            this.label7.TabIndex = 5;
            this.label7.Text = "Precio Base:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(359, 38);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(72, 17);
            this.label8.TabIndex = 6;
            this.label8.Text = "Matrícula:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(359, 79);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(56, 17);
            this.label9.TabIndex = 7;
            this.label9.Text = "Estado:";
            // 
            // txtNroVuelo
            // 
            this.txtNroVuelo.Location = new System.Drawing.Point(188, 3);
            this.txtNroVuelo.Name = "txtNroVuelo";
            this.txtNroVuelo.Size = new System.Drawing.Size(149, 23);
            this.txtNroVuelo.TabIndex = 8;
            // 
            // txtPrecioBase
            // 
            this.txtPrecioBase.Location = new System.Drawing.Point(549, 3);
            this.txtPrecioBase.Name = "txtPrecioBase";
            this.txtPrecioBase.Size = new System.Drawing.Size(171, 23);
            this.txtPrecioBase.TabIndex = 9;
            // 
            // txtMatricula
            // 
            this.txtMatricula.Location = new System.Drawing.Point(549, 41);
            this.txtMatricula.Name = "txtMatricula";
            this.txtMatricula.Size = new System.Drawing.Size(171, 23);
            this.txtMatricula.TabIndex = 10;
            // 
            // txtOrigen
            // 
            this.txtOrigen.Location = new System.Drawing.Point(188, 41);
            this.txtOrigen.Name = "txtOrigen";
            this.txtOrigen.Size = new System.Drawing.Size(149, 23);
            this.txtOrigen.TabIndex = 11;
            // 
            // txtDestino
            // 
            this.txtDestino.Location = new System.Drawing.Point(188, 82);
            this.txtDestino.Name = "txtDestino";
            this.txtDestino.Size = new System.Drawing.Size(149, 23);
            this.txtDestino.TabIndex = 12;
            // 
            // dtFechaHoraSalida
            // 
            this.dtFechaHoraSalida.Location = new System.Drawing.Point(188, 122);
            this.dtFechaHoraSalida.Name = "dtFechaHoraSalida";
            this.dtFechaHoraSalida.Size = new System.Drawing.Size(165, 23);
            this.dtFechaHoraSalida.TabIndex = 16;
            // 
            // cmbEstado
            // 
            this.cmbEstado.FormattingEnabled = true;
            this.cmbEstado.Location = new System.Drawing.Point(549, 82);
            this.cmbEstado.Name = "cmbEstado";
            this.cmbEstado.Size = new System.Drawing.Size(171, 25);
            this.cmbEstado.TabIndex = 17;
            // 
            // dgvAsientos
            // 
            this.dgvAsientos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAsientos.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colIdAsiento,
            this.colNroAsiento,
            this.colClase,
            this.colEstadoAsiento});
            this.dgvAsientos.Location = new System.Drawing.Point(9, 22);
            this.dgvAsientos.Name = "dgvAsientos";
            this.dgvAsientos.Size = new System.Drawing.Size(293, 150);
            this.dgvAsientos.TabIndex = 0;
            this.dgvAsientos.SelectionChanged += new System.EventHandler(this.dgvAsientos_SelectionChanged);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 6;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 73F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 55F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 65F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 194F));
            this.tableLayoutPanel2.Controls.Add(this.txtNroAsiento, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.cmbEstadoAsiento, 5, 0);
            this.tableLayoutPanel2.Controls.Add(this.cmbClase, 3, 0);
            this.tableLayoutPanel2.Controls.Add(this.label10, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.label11, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.label12, 4, 0);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(320, 22);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(549, 46);
            this.tableLayoutPanel2.TabIndex = 1;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(3, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(59, 34);
            this.label10.TabIndex = 3;
            this.label10.Text = "Nro de Asiento:";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(176, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(48, 17);
            this.label11.TabIndex = 4;
            this.label11.Text = "Clase:";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(351, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(56, 17);
            this.label12.TabIndex = 5;
            this.label12.Text = "Estado:";
            // 
            // cmbClase
            // 
            this.cmbClase.FormattingEnabled = true;
            this.cmbClase.Location = new System.Drawing.Point(231, 3);
            this.cmbClase.Name = "cmbClase";
            this.cmbClase.Size = new System.Drawing.Size(104, 25);
            this.cmbClase.TabIndex = 19;
            // 
            // cmbEstadoAsiento
            // 
            this.cmbEstadoAsiento.FormattingEnabled = true;
            this.cmbEstadoAsiento.Location = new System.Drawing.Point(416, 3);
            this.cmbEstadoAsiento.Name = "cmbEstadoAsiento";
            this.cmbEstadoAsiento.Size = new System.Drawing.Size(127, 25);
            this.cmbEstadoAsiento.TabIndex = 20;
            // 
            // txtNroAsiento
            // 
            this.txtNroAsiento.Location = new System.Drawing.Point(76, 3);
            this.txtNroAsiento.Name = "txtNroAsiento";
            this.txtNroAsiento.Size = new System.Drawing.Size(87, 23);
            this.txtNroAsiento.TabIndex = 21;
            // 
            // btnAñadirAsiento
            // 
            this.btnAñadirAsiento.Location = new System.Drawing.Point(320, 74);
            this.btnAñadirAsiento.Name = "btnAñadirAsiento";
            this.btnAñadirAsiento.Size = new System.Drawing.Size(135, 24);
            this.btnAñadirAsiento.TabIndex = 2;
            this.btnAñadirAsiento.Text = "Añadir Asiento";
            this.btnAñadirAsiento.UseVisualStyleBackColor = true;
            this.btnAñadirAsiento.Click += new System.EventHandler(this.btnAñadirAsiento_Click);
            // 
            // btnModificarAsiento
            // 
            this.btnModificarAsiento.Location = new System.Drawing.Point(461, 74);
            this.btnModificarAsiento.Name = "btnModificarAsiento";
            this.btnModificarAsiento.Size = new System.Drawing.Size(135, 24);
            this.btnModificarAsiento.TabIndex = 3;
            this.btnModificarAsiento.Text = "Modificar Asiento";
            this.btnModificarAsiento.UseVisualStyleBackColor = true;
            this.btnModificarAsiento.Click += new System.EventHandler(this.btnModificarAsiento_Click);
            // 
            // btnEliminarAsiento
            // 
            this.btnEliminarAsiento.Location = new System.Drawing.Point(602, 74);
            this.btnEliminarAsiento.Name = "btnEliminarAsiento";
            this.btnEliminarAsiento.Size = new System.Drawing.Size(135, 24);
            this.btnEliminarAsiento.TabIndex = 4;
            this.btnEliminarAsiento.Text = "Eliminar Asiento";
            this.btnEliminarAsiento.UseVisualStyleBackColor = true;
            this.btnEliminarAsiento.Click += new System.EventHandler(this.btnEliminarAsiento_Click);
            // 
            // btnSerializar
            // 
            this.btnSerializar.Location = new System.Drawing.Point(12, 698);
            this.btnSerializar.Name = "btnSerializar";
            this.btnSerializar.Size = new System.Drawing.Size(114, 24);
            this.btnSerializar.TabIndex = 17;
            this.btnSerializar.Text = "SERIALIZAR";
            this.btnSerializar.UseVisualStyleBackColor = true;
            this.btnSerializar.Click += new System.EventHandler(this.btnSerializar_Click);
            // 
            // btnDeserializar
            // 
            this.btnDeserializar.Location = new System.Drawing.Point(425, 698);
            this.btnDeserializar.Name = "btnDeserializar";
            this.btnDeserializar.Size = new System.Drawing.Size(114, 24);
            this.btnDeserializar.TabIndex = 18;
            this.btnDeserializar.Text = "DES-SERIALIZAR";
            this.btnDeserializar.UseVisualStyleBackColor = true;
            this.btnDeserializar.Click += new System.EventHandler(this.btnDeserializar_Click);
            // 
            // txtRutaSerializar
            // 
            this.txtRutaSerializar.Location = new System.Drawing.Point(12, 728);
            this.txtRutaSerializar.Name = "txtRutaSerializar";
            this.txtRutaSerializar.Size = new System.Drawing.Size(320, 23);
            this.txtRutaSerializar.TabIndex = 18;
            // 
            // txtRutaDeserializar
            // 
            this.txtRutaDeserializar.Location = new System.Drawing.Point(425, 727);
            this.txtRutaDeserializar.Name = "txtRutaDeserializar";
            this.txtRutaDeserializar.Size = new System.Drawing.Size(320, 23);
            this.txtRutaDeserializar.TabIndex = 19;
            // 
            // btnRutaSerializar
            // 
            this.btnRutaSerializar.BackColor = System.Drawing.Color.SteelBlue;
            this.btnRutaSerializar.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnRutaSerializar.BackgroundImage")));
            this.btnRutaSerializar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnRutaSerializar.FlatAppearance.BorderSize = 0;
            this.btnRutaSerializar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRutaSerializar.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRutaSerializar.ForeColor = System.Drawing.Color.Snow;
            this.btnRutaSerializar.Location = new System.Drawing.Point(339, 729);
            this.btnRutaSerializar.Margin = new System.Windows.Forms.Padding(4);
            this.btnRutaSerializar.Name = "btnRutaSerializar";
            this.btnRutaSerializar.Size = new System.Drawing.Size(41, 22);
            this.btnRutaSerializar.TabIndex = 32;
            this.btnRutaSerializar.UseVisualStyleBackColor = false;
            this.btnRutaSerializar.Click += new System.EventHandler(this.btnRutaSerializar_Click);
            // 
            // btnRutaDeserializar
            // 
            this.btnRutaDeserializar.BackColor = System.Drawing.Color.SteelBlue;
            this.btnRutaDeserializar.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnRutaDeserializar.BackgroundImage")));
            this.btnRutaDeserializar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnRutaDeserializar.FlatAppearance.BorderSize = 0;
            this.btnRutaDeserializar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRutaDeserializar.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRutaDeserializar.ForeColor = System.Drawing.Color.Snow;
            this.btnRutaDeserializar.Location = new System.Drawing.Point(752, 727);
            this.btnRutaDeserializar.Margin = new System.Windows.Forms.Padding(4);
            this.btnRutaDeserializar.Name = "btnRutaDeserializar";
            this.btnRutaDeserializar.Size = new System.Drawing.Size(41, 22);
            this.btnRutaDeserializar.TabIndex = 33;
            this.btnRutaDeserializar.UseVisualStyleBackColor = false;
            this.btnRutaDeserializar.Click += new System.EventHandler(this.btnRutaDeserializar_Click);
            // 
            // btnSalir
            // 
            this.btnSalir.Location = new System.Drawing.Point(913, 661);
            this.btnSalir.Name = "btnSalir";
            this.btnSalir.Size = new System.Drawing.Size(114, 24);
            this.btnSalir.TabIndex = 34;
            this.btnSalir.Text = "Salir";
            this.btnSalir.UseVisualStyleBackColor = true;
            this.btnSalir.Click += new System.EventHandler(this.btnSalir_Click);
            // 
            // btnCancelar
            // 
            this.btnCancelar.Location = new System.Drawing.Point(913, 631);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(114, 24);
            this.btnCancelar.TabIndex = 35;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = true;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            // 
            // btnAplicar
            // 
            this.btnAplicar.Location = new System.Drawing.Point(913, 601);
            this.btnAplicar.Name = "btnAplicar";
            this.btnAplicar.Size = new System.Drawing.Size(114, 24);
            this.btnAplicar.TabIndex = 36;
            this.btnAplicar.Text = "Aplicar";
            this.btnAplicar.UseVisualStyleBackColor = true;
            this.btnAplicar.Click += new System.EventHandler(this.btnAplicar_Click);
            // 
            // colNroVuelo
            // 
            this.colNroVuelo.DataPropertyName = "NroVuelo_62FD";
            this.colNroVuelo.HeaderText = "Nro de Vuelo";
            this.colNroVuelo.Name = "colNroVuelo";
            // 
            // colOrigen
            // 
            this.colOrigen.DataPropertyName = "Origen_62FD";
            this.colOrigen.HeaderText = "Origen";
            this.colOrigen.Name = "colOrigen";
            // 
            // colDestino
            // 
            this.colDestino.DataPropertyName = "Destino_62FD";
            this.colDestino.HeaderText = "Destino";
            this.colDestino.Name = "colDestino";
            // 
            // colFechaHoraSalida
            // 
            this.colFechaHoraSalida.DataPropertyName = "FechaHoraSalida_62FD";
            this.colFechaHoraSalida.HeaderText = "Fecha y Hora de Salida";
            this.colFechaHoraSalida.Name = "colFechaHoraSalida";
            // 
            // colMatriculaVuelo
            // 
            this.colMatriculaVuelo.DataPropertyName = "Matricula_62FD";
            this.colMatriculaVuelo.HeaderText = "Matrícula";
            this.colMatriculaVuelo.Name = "colMatriculaVuelo";
            // 
            // colPrecio
            // 
            this.colPrecio.DataPropertyName = "Precio_62FD";
            this.colPrecio.HeaderText = "Precio Base";
            this.colPrecio.Name = "colPrecio";
            // 
            // colEstadoVuelo
            // 
            this.colEstadoVuelo.DataPropertyName = "EstadoVuelo_62FD";
            this.colEstadoVuelo.HeaderText = "Estado";
            this.colEstadoVuelo.Name = "colEstadoVuelo";
            // 
            // colIdAsiento
            // 
            this.colIdAsiento.DataPropertyName = "IdAsiento_62FD";
            this.colIdAsiento.HeaderText = "Id";
            this.colIdAsiento.Name = "colIdAsiento";
            // 
            // colNroAsiento
            // 
            this.colNroAsiento.DataPropertyName = "NroAsiento_62FD";
            this.colNroAsiento.HeaderText = "Nro de Asiento";
            this.colNroAsiento.Name = "colNroAsiento";
            // 
            // colClase
            // 
            this.colClase.DataPropertyName = "Clase_62FD";
            this.colClase.HeaderText = "Clase";
            this.colClase.Name = "colClase";
            // 
            // colEstadoAsiento
            // 
            this.colEstadoAsiento.DataPropertyName = "EstadoAsiento_62FD";
            this.colEstadoAsiento.HeaderText = "Estado";
            this.colEstadoAsiento.Name = "colEstadoAsiento";
            // 
            // btnLimpiar
            // 
            this.btnLimpiar.Location = new System.Drawing.Point(749, 143);
            this.btnLimpiar.Name = "btnLimpiar";
            this.btnLimpiar.Size = new System.Drawing.Size(114, 24);
            this.btnLimpiar.TabIndex = 5;
            this.btnLimpiar.Text = "Limpiar";
            this.btnLimpiar.UseVisualStyleBackColor = true;
            this.btnLimpiar.Click += new System.EventHandler(this.btnLimpiar_Click);
            // 
            // MaestroVuelos_62FD
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1050, 796);
            this.ControlBox = false;
            this.Controls.Add(this.btnAplicar);
            this.Controls.Add(this.btnCancelar);
            this.Controls.Add(this.btnSalir);
            this.Controls.Add(this.btnRutaDeserializar);
            this.Controls.Add(this.btnRutaSerializar);
            this.Controls.Add(this.txtRutaDeserializar);
            this.Controls.Add(this.txtRutaSerializar);
            this.Controls.Add(this.btnDeserializar);
            this.Controls.Add(this.btnSerializar);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "MaestroVuelos_62FD";
            this.Text = "MaestroVuelos_62FD";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MaestroVuelos_62FD_FormClosing);
            this.Load += new System.EventHandler(this.MaestroVuelos_62FD_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvVuelos)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAsientos)).EndInit();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridView dgvVuelos;
        private System.Windows.Forms.Button btnEliminar;
        private System.Windows.Forms.Button btnModificar;
        private System.Windows.Forms.Button btnAñadir;
        private System.Windows.Forms.Button btnActualizar;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel3;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.ComboBox cmbEstado;
        private System.Windows.Forms.DateTimePicker dtFechaHoraSalida;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtNroVuelo;
        private System.Windows.Forms.TextBox txtPrecioBase;
        private System.Windows.Forms.TextBox txtMatricula;
        private System.Windows.Forms.TextBox txtOrigen;
        private System.Windows.Forms.TextBox txtDestino;
        private System.Windows.Forms.Button btnEliminarAsiento;
        private System.Windows.Forms.Button btnModificarAsiento;
        private System.Windows.Forms.Button btnAñadirAsiento;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TextBox txtNroAsiento;
        private System.Windows.Forms.ComboBox cmbEstadoAsiento;
        private System.Windows.Forms.ComboBox cmbClase;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.DataGridView dgvAsientos;
        private System.Windows.Forms.Button btnSerializar;
        private System.Windows.Forms.Button btnDeserializar;
        private System.Windows.Forms.TextBox txtRutaSerializar;
        private System.Windows.Forms.TextBox txtRutaDeserializar;
        private System.Windows.Forms.Button btnRutaSerializar;
        private System.Windows.Forms.Button btnRutaDeserializar;
        private System.Windows.Forms.Button btnSalir;
        private System.Windows.Forms.Button btnCancelar;
        private System.Windows.Forms.Button btnAplicar;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNroVuelo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOrigen;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDestino;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFechaHoraSalida;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMatriculaVuelo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPrecio;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEstadoVuelo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colIdAsiento;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNroAsiento;
        private System.Windows.Forms.DataGridViewTextBoxColumn colClase;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEstadoAsiento;
        private System.Windows.Forms.Button btnLimpiar;
    }
}