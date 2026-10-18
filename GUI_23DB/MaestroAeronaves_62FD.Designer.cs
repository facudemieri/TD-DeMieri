namespace GUI_23DB
{
    partial class MaestroAeronaves_62FD
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.colMatricula = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNroSerie = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colModelo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colFabricante = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEstadoOperativo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colHorasVuelo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colIntervaloRevision = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colProximaRevision = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colActivo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnActualizar = new System.Windows.Forms.Button();
            this.btnAñadir = new System.Windows.Forms.Button();
            this.btnModificar = new System.Windows.Forms.Button();
            this.btnEliminar = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.txtMatricula = new System.Windows.Forms.TextBox();
            this.txtHorasVueloAcumuladas = new System.Windows.Forms.TextBox();
            this.txtIntervaloProxRevision = new System.Windows.Forms.TextBox();
            this.txtNroSerie = new System.Windows.Forms.TextBox();
            this.txtModelo = new System.Windows.Forms.TextBox();
            this.txtFabricante = new System.Windows.Forms.TextBox();
            this.cmbEstadoOperativo = new System.Windows.Forms.ComboBox();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.btnSalir = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.btnAplicar = new System.Windows.Forms.Button();
            this.btnBitacoraCambios = new System.Windows.Forms.Button();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
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
            this.panel1.Size = new System.Drawing.Size(970, 60);
            this.panel1.TabIndex = 11;
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
            this.label1.Size = new System.Drawing.Size(406, 25);
            this.label1.TabIndex = 8;
            this.label1.Text = "Maestro de Aeronaves - AeroManager";
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colMatricula,
            this.colNroSerie,
            this.colModelo,
            this.colFabricante,
            this.colEstadoOperativo,
            this.colHorasVuelo,
            this.colIntervaloRevision,
            this.colProximaRevision,
            this.colActivo});
            this.dataGridView1.Location = new System.Drawing.Point(13, 68);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(769, 302);
            this.dataGridView1.TabIndex = 12;
            // 
            // colMatricula
            // 
            this.colMatricula.DataPropertyName = "Matricula_62FD";
            this.colMatricula.HeaderText = "Matrícula";
            this.colMatricula.Name = "colMatricula";
            // 
            // colNroSerie
            // 
            this.colNroSerie.DataPropertyName = "NroSerie_62FD";
            this.colNroSerie.HeaderText = "Nro de Serie";
            this.colNroSerie.Name = "colNroSerie";
            // 
            // colModelo
            // 
            this.colModelo.DataPropertyName = "Modelo_62FD";
            this.colModelo.HeaderText = "Modelo";
            this.colModelo.Name = "colModelo";
            // 
            // colFabricante
            // 
            this.colFabricante.DataPropertyName = "Fabricante_62FD";
            this.colFabricante.HeaderText = "Fabricante";
            this.colFabricante.Name = "colFabricante";
            // 
            // colEstadoOperativo
            // 
            this.colEstadoOperativo.DataPropertyName = "EstadoOperativo_62FD";
            this.colEstadoOperativo.HeaderText = "Estado Operativo";
            this.colEstadoOperativo.Name = "colEstadoOperativo";
            // 
            // colHorasVuelo
            // 
            this.colHorasVuelo.DataPropertyName = "HorasVueloAcumuladas_62FD";
            this.colHorasVuelo.HeaderText = "Horas de Vuelo";
            this.colHorasVuelo.Name = "colHorasVuelo";
            // 
            // colIntervaloRevision
            // 
            this.colIntervaloRevision.DataPropertyName = "IntervaloRevision_62FD";
            this.colIntervaloRevision.HeaderText = "Intervalo (hs)";
            this.colIntervaloRevision.Name = "colIntervaloRevision";
            // 
            // colProximaRevision
            // 
            this.colProximaRevision.DataPropertyName = "FechaProximaRevision_62FD";
            this.colProximaRevision.HeaderText = "Próxima Revisión";
            this.colProximaRevision.Name = "colProximaRevision";
            // 
            // colActivo
            // 
            this.colActivo.DataPropertyName = "Activo_62FD";
            this.colActivo.HeaderText = "Activo";
            this.colActivo.Name = "colActivo";
            // 
            // btnActualizar
            // 
            this.btnActualizar.Location = new System.Drawing.Point(799, 109);
            this.btnActualizar.Name = "btnActualizar";
            this.btnActualizar.Size = new System.Drawing.Size(159, 29);
            this.btnActualizar.TabIndex = 13;
            this.btnActualizar.Text = "Actualizar";
            this.btnActualizar.UseVisualStyleBackColor = true;
            this.btnActualizar.Click += new System.EventHandler(this.btnActualizar_Click);
            // 
            // btnAñadir
            // 
            this.btnAñadir.Location = new System.Drawing.Point(799, 144);
            this.btnAñadir.Name = "btnAñadir";
            this.btnAñadir.Size = new System.Drawing.Size(159, 29);
            this.btnAñadir.TabIndex = 14;
            this.btnAñadir.Text = "Añadir";
            this.btnAñadir.UseVisualStyleBackColor = true;
            this.btnAñadir.Click += new System.EventHandler(this.btnAñadir_Click);
            // 
            // btnModificar
            // 
            this.btnModificar.Location = new System.Drawing.Point(799, 179);
            this.btnModificar.Name = "btnModificar";
            this.btnModificar.Size = new System.Drawing.Size(159, 29);
            this.btnModificar.TabIndex = 15;
            this.btnModificar.Text = "Modificar";
            this.btnModificar.UseVisualStyleBackColor = true;
            this.btnModificar.Click += new System.EventHandler(this.btnModificar_Click);
            // 
            // btnEliminar
            // 
            this.btnEliminar.Location = new System.Drawing.Point(799, 214);
            this.btnEliminar.Name = "btnEliminar";
            this.btnEliminar.Size = new System.Drawing.Size(159, 29);
            this.btnEliminar.TabIndex = 16;
            this.btnEliminar.Text = "Eliminar";
            this.btnEliminar.UseVisualStyleBackColor = true;
            this.btnEliminar.Click += new System.EventHandler(this.btnEliminar_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tableLayoutPanel1);
            this.groupBox1.Location = new System.Drawing.Point(13, 387);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(743, 211);
            this.groupBox1.TabIndex = 17;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Datos de la Aeronave";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 98F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 172F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 238F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 209F));
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.label5, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.label6, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.label7, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.label8, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.label9, 2, 3);
            this.tableLayoutPanel1.Controls.Add(this.txtMatricula, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.txtHorasVueloAcumuladas, 3, 1);
            this.tableLayoutPanel1.Controls.Add(this.txtIntervaloProxRevision, 3, 2);
            this.tableLayoutPanel1.Controls.Add(this.txtNroSerie, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.txtModelo, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.txtFabricante, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.cmbEstadoOperativo, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.dateTimePicker1, 3, 3);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(8, 22);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 38F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 41F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 21F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(717, 170);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 17);
            this.label2.TabIndex = 0;
            this.label2.Text = "Matrícula:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 38);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(89, 17);
            this.label3.TabIndex = 1;
            this.label3.Text = "Nro de Serie:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 79);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(61, 17);
            this.label4.TabIndex = 2;
            this.label4.Text = "Modelo:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 119);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(81, 17);
            this.label5.TabIndex = 3;
            this.label5.Text = "Fabricante:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(273, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(126, 17);
            this.label6.TabIndex = 4;
            this.label6.Text = "Estado Operativo:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(273, 38);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(195, 17);
            this.label7.TabIndex = 5;
            this.label7.Text = "Horas de Vuelo Acumuladas:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(273, 79);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(214, 17);
            this.label8.TabIndex = 6;
            this.label8.Text = "Intervalo de Revisión (en horas):";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(273, 119);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(163, 17);
            this.label9.TabIndex = 7;
            this.label9.Text = "Fecha Próxima Revisión:";
            // 
            // txtMatricula
            // 
            this.txtMatricula.Location = new System.Drawing.Point(101, 3);
            this.txtMatricula.Name = "txtMatricula";
            this.txtMatricula.Size = new System.Drawing.Size(149, 23);
            this.txtMatricula.TabIndex = 8;
            // 
            // txtHorasVueloAcumuladas
            // 
            this.txtHorasVueloAcumuladas.Location = new System.Drawing.Point(511, 41);
            this.txtHorasVueloAcumuladas.Name = "txtHorasVueloAcumuladas";
            this.txtHorasVueloAcumuladas.Size = new System.Drawing.Size(171, 23);
            this.txtHorasVueloAcumuladas.TabIndex = 9;
            // 
            // txtIntervaloProxRevision
            // 
            this.txtIntervaloProxRevision.Location = new System.Drawing.Point(511, 82);
            this.txtIntervaloProxRevision.Name = "txtIntervaloProxRevision";
            this.txtIntervaloProxRevision.Size = new System.Drawing.Size(171, 23);
            this.txtIntervaloProxRevision.TabIndex = 10;
            // 
            // txtNroSerie
            // 
            this.txtNroSerie.Location = new System.Drawing.Point(101, 41);
            this.txtNroSerie.Name = "txtNroSerie";
            this.txtNroSerie.Size = new System.Drawing.Size(149, 23);
            this.txtNroSerie.TabIndex = 11;
            // 
            // txtModelo
            // 
            this.txtModelo.Location = new System.Drawing.Point(101, 82);
            this.txtModelo.Name = "txtModelo";
            this.txtModelo.Size = new System.Drawing.Size(149, 23);
            this.txtModelo.TabIndex = 12;
            // 
            // txtFabricante
            // 
            this.txtFabricante.Location = new System.Drawing.Point(101, 122);
            this.txtFabricante.Name = "txtFabricante";
            this.txtFabricante.Size = new System.Drawing.Size(149, 23);
            this.txtFabricante.TabIndex = 13;
            // 
            // cmbEstadoOperativo
            // 
            this.cmbEstadoOperativo.FormattingEnabled = true;
            this.cmbEstadoOperativo.Location = new System.Drawing.Point(511, 3);
            this.cmbEstadoOperativo.Name = "cmbEstadoOperativo";
            this.cmbEstadoOperativo.Size = new System.Drawing.Size(134, 25);
            this.cmbEstadoOperativo.TabIndex = 14;
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Location = new System.Drawing.Point(511, 122);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(200, 23);
            this.dateTimePicker1.TabIndex = 15;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripStatusLabel2,
            this.toolStripStatusLabel3});
            this.statusStrip1.Location = new System.Drawing.Point(0, 689);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(970, 22);
            this.statusStrip1.TabIndex = 18;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(318, 17);
            this.toolStripStatusLabel1.Spring = true;
            this.toolStripStatusLabel1.Text = "Modo:";
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(318, 17);
            this.toolStripStatusLabel2.Spring = true;
            this.toolStripStatusLabel2.Text = "Usuario:";
            // 
            // toolStripStatusLabel3
            // 
            this.toolStripStatusLabel3.Name = "toolStripStatusLabel3";
            this.toolStripStatusLabel3.Size = new System.Drawing.Size(318, 17);
            this.toolStripStatusLabel3.Spring = true;
            this.toolStripStatusLabel3.Text = "Fecha:";
            // 
            // btnSalir
            // 
            this.btnSalir.Location = new System.Drawing.Point(799, 504);
            this.btnSalir.Name = "btnSalir";
            this.btnSalir.Size = new System.Drawing.Size(159, 29);
            this.btnSalir.TabIndex = 21;
            this.btnSalir.Text = "Salir";
            this.btnSalir.UseVisualStyleBackColor = true;
            this.btnSalir.Click += new System.EventHandler(this.btnSalir_Click);
            // 
            // btnCancelar
            // 
            this.btnCancelar.Location = new System.Drawing.Point(799, 469);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(159, 29);
            this.btnCancelar.TabIndex = 20;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = true;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            // 
            // btnAplicar
            // 
            this.btnAplicar.Location = new System.Drawing.Point(799, 434);
            this.btnAplicar.Name = "btnAplicar";
            this.btnAplicar.Size = new System.Drawing.Size(159, 29);
            this.btnAplicar.TabIndex = 19;
            this.btnAplicar.Text = "Aplicar";
            this.btnAplicar.UseVisualStyleBackColor = true;
            this.btnAplicar.Click += new System.EventHandler(this.btnAplicar_Click);
            // 
            // btnBitacoraCambios
            // 
            this.btnBitacoraCambios.Location = new System.Drawing.Point(12, 604);
            this.btnBitacoraCambios.Name = "btnBitacoraCambios";
            this.btnBitacoraCambios.Size = new System.Drawing.Size(201, 29);
            this.btnBitacoraCambios.TabIndex = 22;
            this.btnBitacoraCambios.Text = "Ver Bitácora de Cambios";
            this.btnBitacoraCambios.UseVisualStyleBackColor = true;
            this.btnBitacoraCambios.Click += new System.EventHandler(this.btnBitacoraCambios_Click);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(799, 266);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(157, 21);
            this.checkBox1.TabIndex = 23;
            this.checkBox1.Text = "Aeronaves inactivas";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // MaestroAeronaves_62FD
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(970, 711);
            this.ControlBox = false;
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.btnBitacoraCambios);
            this.Controls.Add(this.btnSalir);
            this.Controls.Add(this.btnCancelar);
            this.Controls.Add(this.btnAplicar);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnEliminar);
            this.Controls.Add(this.btnModificar);
            this.Controls.Add(this.btnAñadir);
            this.Controls.Add(this.btnActualizar);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "MaestroAeronaves_62FD";
            this.Text = "MaestroAeronaves_62FD";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MaestroAeronaves_62FD_FormClosing);
            this.Load += new System.EventHandler(this.MaestroAeronaves_62FD_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button btnActualizar;
        private System.Windows.Forms.Button btnAñadir;
        private System.Windows.Forms.Button btnModificar;
        private System.Windows.Forms.Button btnEliminar;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel3;
        private System.Windows.Forms.Button btnSalir;
        private System.Windows.Forms.Button btnCancelar;
        private System.Windows.Forms.Button btnAplicar;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtMatricula;
        private System.Windows.Forms.TextBox txtHorasVueloAcumuladas;
        private System.Windows.Forms.TextBox txtIntervaloProxRevision;
        private System.Windows.Forms.TextBox txtNroSerie;
        private System.Windows.Forms.TextBox txtModelo;
        private System.Windows.Forms.TextBox txtFabricante;
        private System.Windows.Forms.ComboBox cmbEstadoOperativo;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.Button btnBitacoraCambios;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMatricula;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNroSerie;
        private System.Windows.Forms.DataGridViewTextBoxColumn colModelo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFabricante;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEstadoOperativo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHorasVuelo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colIntervaloRevision;
        private System.Windows.Forms.DataGridViewTextBoxColumn colProximaRevision;
        private System.Windows.Forms.DataGridViewTextBoxColumn colActivo;
    }
}