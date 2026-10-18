namespace GUI_23DB
{
    partial class CertificarIntervencion_62FD
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.txtHorasInsumidas = new System.Windows.Forms.TextBox();
            this.txtMatricula = new System.Windows.Forms.TextBox();
            this.txtNroOrden = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txtNroIntervencion = new System.Windows.Forms.TextBox();
            this.txtFechaHoraFin = new System.Windows.Forms.TextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.txtObservaciones = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.txtCodigoHabilitante = new System.Windows.Forms.TextBox();
            this.dtpFechaHoraCertificacion = new System.Windows.Forms.DateTimePicker();
            this.cmbResultadoInspeccion = new System.Windows.Forms.ComboBox();
            this.btnAplicar = new System.Windows.Forms.Button();
            this.btnImprimir = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.btnSalir = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.colIdIntervencion = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colIdOrden = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMatricula = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colFechaHoraFin = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTotalHoras = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            this.colDescripcionTarea = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colResultadoTarea = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colHorasTarea = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRepuestosTarea = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colObservacionesTarea = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.LightSkyBlue;
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1166, 46);
            this.panel1.TabIndex = 12;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.LightSkyBlue;
            this.label1.Font = new System.Drawing.Font("Century Gothic", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.Control;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(401, 25);
            this.label1.TabIndex = 8;
            this.label1.Text = "Certificar Intervención - AeroManager";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dataGridView1);
            this.groupBox1.Location = new System.Drawing.Point(12, 52);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(565, 213);
            this.groupBox1.TabIndex = 13;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Intervenciones Pendientes de Certificación";
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colIdIntervencion,
            this.colIdOrden,
            this.colMatricula,
            this.colFechaHoraFin,
            this.colTotalHoras});
            this.dataGridView1.Location = new System.Drawing.Point(6, 23);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(543, 174);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellClick);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.tableLayoutPanel1);
            this.groupBox2.Location = new System.Drawing.Point(127, 272);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(914, 112);
            this.groupBox2.TabIndex = 14;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Intervención Seleccionada";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 6;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 188F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 157F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 168F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 89F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 137F));
            this.tableLayoutPanel1.Controls.Add(this.txtHorasInsumidas, 3, 1);
            this.tableLayoutPanel1.Controls.Add(this.txtMatricula, 5, 0);
            this.tableLayoutPanel1.Controls.Add(this.txtNroOrden, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label4, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.label5, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.label6, 4, 0);
            this.tableLayoutPanel1.Controls.Add(this.txtNroIntervencion, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.txtFechaHoraFin, 1, 1);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(7, 23);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(898, 71);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // txtHorasInsumidas
            // 
            this.txtHorasInsumidas.Location = new System.Drawing.Point(507, 38);
            this.txtHorasInsumidas.Name = "txtHorasInsumidas";
            this.txtHorasInsumidas.Size = new System.Drawing.Size(112, 23);
            this.txtHorasInsumidas.TabIndex = 9;
            // 
            // txtMatricula
            // 
            this.txtMatricula.Location = new System.Drawing.Point(764, 3);
            this.txtMatricula.Name = "txtMatricula";
            this.txtMatricula.Size = new System.Drawing.Size(131, 23);
            this.txtMatricula.TabIndex = 8;
            // 
            // txtNroOrden
            // 
            this.txtNroOrden.Location = new System.Drawing.Point(507, 3);
            this.txtNroOrden.Name = "txtNroOrden";
            this.txtNroOrden.Size = new System.Drawing.Size(156, 23);
            this.txtNroOrden.TabIndex = 7;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(119, 17);
            this.label2.TabIndex = 0;
            this.label2.Text = "Nro Intervención:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 35);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(138, 17);
            this.label3.TabIndex = 1;
            this.label3.Text = "Fecha y Hora de Fin:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(350, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(79, 17);
            this.label4.TabIndex = 2;
            this.label4.Text = "Nro Orden:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(350, 35);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(150, 17);
            this.label5.TabIndex = 3;
            this.label5.Text = "Total Horas Insumidas:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(675, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(72, 17);
            this.label6.TabIndex = 4;
            this.label6.Text = "Matrícula:";
            // 
            // txtNroIntervencion
            // 
            this.txtNroIntervencion.Location = new System.Drawing.Point(162, 3);
            this.txtNroIntervencion.Name = "txtNroIntervencion";
            this.txtNroIntervencion.Size = new System.Drawing.Size(170, 23);
            this.txtNroIntervencion.TabIndex = 5;
            // 
            // txtFechaHoraFin
            // 
            this.txtFechaHoraFin.Location = new System.Drawing.Point(162, 38);
            this.txtFechaHoraFin.Name = "txtFechaHoraFin";
            this.txtFechaHoraFin.Size = new System.Drawing.Size(170, 23);
            this.txtFechaHoraFin.TabIndex = 6;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.txtObservaciones);
            this.groupBox3.Controls.Add(this.label10);
            this.groupBox3.Controls.Add(this.tableLayoutPanel2);
            this.groupBox3.Location = new System.Drawing.Point(126, 390);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(915, 178);
            this.groupBox3.TabIndex = 15;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Datos de la Certificación";
            // 
            // txtObservaciones
            // 
            this.txtObservaciones.Location = new System.Drawing.Point(136, 99);
            this.txtObservaciones.Multiline = true;
            this.txtObservaciones.Name = "txtObservaciones";
            this.txtObservaciones.Size = new System.Drawing.Size(770, 62);
            this.txtObservaciones.TabIndex = 9;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(11, 96);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(107, 17);
            this.label10.TabIndex = 2;
            this.label10.Text = "Observaciones:";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 4;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 223F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 218F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 227F));
            this.tableLayoutPanel2.Controls.Add(this.label7, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.label8, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.label9, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.txtCodigoHabilitante, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.dtpFechaHoraCertificacion, 3, 0);
            this.tableLayoutPanel2.Controls.Add(this.cmbResultadoInspeccion, 1, 1);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(8, 22);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 36F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(898, 71);
            this.tableLayoutPanel2.TabIndex = 1;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(3, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(224, 17);
            this.label7.TabIndex = 0;
            this.label7.Text = "Código Habilitante del Inspector:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(3, 36);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(186, 17);
            this.label8.TabIndex = 1;
            this.label8.Text = "Resultado de la Inspección:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(456, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(204, 17);
            this.label9.TabIndex = 2;
            this.label9.Text = "Fecha y Hora de Certificación:";
            // 
            // txtCodigoHabilitante
            // 
            this.txtCodigoHabilitante.Location = new System.Drawing.Point(233, 3);
            this.txtCodigoHabilitante.Name = "txtCodigoHabilitante";
            this.txtCodigoHabilitante.Size = new System.Drawing.Size(170, 23);
            this.txtCodigoHabilitante.TabIndex = 5;
            // 
            // dtpFechaHoraCertificacion
            // 
            this.dtpFechaHoraCertificacion.Location = new System.Drawing.Point(674, 3);
            this.dtpFechaHoraCertificacion.Name = "dtpFechaHoraCertificacion";
            this.dtpFechaHoraCertificacion.Size = new System.Drawing.Size(200, 23);
            this.dtpFechaHoraCertificacion.TabIndex = 7;
            // 
            // cmbResultadoInspeccion
            // 
            this.cmbResultadoInspeccion.FormattingEnabled = true;
            this.cmbResultadoInspeccion.Location = new System.Drawing.Point(233, 39);
            this.cmbResultadoInspeccion.Name = "cmbResultadoInspeccion";
            this.cmbResultadoInspeccion.Size = new System.Drawing.Size(214, 25);
            this.cmbResultadoInspeccion.TabIndex = 8;
            // 
            // btnAplicar
            // 
            this.btnAplicar.Location = new System.Drawing.Point(486, 574);
            this.btnAplicar.Name = "btnAplicar";
            this.btnAplicar.Size = new System.Drawing.Size(91, 28);
            this.btnAplicar.TabIndex = 16;
            this.btnAplicar.Text = "Aplicar";
            this.btnAplicar.UseVisualStyleBackColor = true;
            this.btnAplicar.Click += new System.EventHandler(this.btnAplicar_Click);
            // 
            // btnImprimir
            // 
            this.btnImprimir.Location = new System.Drawing.Point(583, 574);
            this.btnImprimir.Name = "btnImprimir";
            this.btnImprimir.Size = new System.Drawing.Size(150, 28);
            this.btnImprimir.TabIndex = 17;
            this.btnImprimir.Text = "Imprimir Certificado";
            this.btnImprimir.UseVisualStyleBackColor = true;
            this.btnImprimir.Click += new System.EventHandler(this.btnImprimir_Click);
            // 
            // btnCancelar
            // 
            this.btnCancelar.Location = new System.Drawing.Point(739, 574);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(91, 28);
            this.btnCancelar.TabIndex = 18;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = true;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            // 
            // btnSalir
            // 
            this.btnSalir.Location = new System.Drawing.Point(836, 574);
            this.btnSalir.Name = "btnSalir";
            this.btnSalir.Size = new System.Drawing.Size(91, 28);
            this.btnSalir.TabIndex = 19;
            this.btnSalir.Text = "Salir";
            this.btnSalir.UseVisualStyleBackColor = true;
            this.btnSalir.Click += new System.EventHandler(this.btnSalir_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripStatusLabel2,
            this.toolStripStatusLabel3});
            this.statusStrip1.Location = new System.Drawing.Point(0, 636);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1166, 22);
            this.statusStrip1.TabIndex = 20;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(307, 17);
            this.toolStripStatusLabel1.Spring = true;
            this.toolStripStatusLabel1.Text = "Modo:";
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(307, 17);
            this.toolStripStatusLabel2.Spring = true;
            this.toolStripStatusLabel2.Text = "Usuario:";
            // 
            // toolStripStatusLabel3
            // 
            this.toolStripStatusLabel3.Name = "toolStripStatusLabel3";
            this.toolStripStatusLabel3.Size = new System.Drawing.Size(307, 17);
            this.toolStripStatusLabel3.Spring = true;
            this.toolStripStatusLabel3.Text = "Fecha:";
            // 
            // colIdIntervencion
            // 
            this.colIdIntervencion.DataPropertyName = "IdIntervencion_62FD";
            this.colIdIntervencion.HeaderText = "Nro Intervención";
            this.colIdIntervencion.Name = "colIdIntervencion";
            // 
            // colIdOrden
            // 
            this.colIdOrden.DataPropertyName = "IdOrden_62FD";
            this.colIdOrden.HeaderText = "Nro Orden";
            this.colIdOrden.Name = "colIdOrden";
            // 
            // colMatricula
            // 
            this.colMatricula.DataPropertyName = "Matricula_62FD";
            this.colMatricula.HeaderText = "Matrícula";
            this.colMatricula.Name = "colMatricula";
            // 
            // colFechaHoraFin
            // 
            this.colFechaHoraFin.DataPropertyName = "fechaHoraFin_62FD";
            this.colFechaHoraFin.HeaderText = "Fecha y Hora de Fin";
            this.colFechaHoraFin.Name = "colFechaHoraFin";
            // 
            // colTotalHoras
            // 
            this.colTotalHoras.DataPropertyName = "totalHorasInsumidas_62FD";
            this.colTotalHoras.HeaderText = "Total Horas Insumidas";
            this.colTotalHoras.Name = "colTotalHoras";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.dataGridView2);
            this.groupBox4.Location = new System.Drawing.Point(583, 53);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(565, 213);
            this.groupBox4.TabIndex = 21;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Tareas de la Intervención seleccionada";
            // 
            // dataGridView2
            // 
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView2.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colDescripcionTarea,
            this.colResultadoTarea,
            this.colHorasTarea,
            this.colRepuestosTarea,
            this.colObservacionesTarea});
            this.dataGridView2.Location = new System.Drawing.Point(7, 22);
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.Size = new System.Drawing.Size(542, 174);
            this.dataGridView2.TabIndex = 0;
            // 
            // colDescripcionTarea
            // 
            this.colDescripcionTarea.DataPropertyName = "Descripcion_62FD";
            this.colDescripcionTarea.HeaderText = "Descripción";
            this.colDescripcionTarea.Name = "colDescripcionTarea";
            // 
            // colResultadoTarea
            // 
            this.colResultadoTarea.DataPropertyName = "ResultadoObtenido_62FD";
            this.colResultadoTarea.HeaderText = "Resultado Obtenido";
            this.colResultadoTarea.Name = "colResultadoTarea";
            // 
            // colHorasTarea
            // 
            this.colHorasTarea.DataPropertyName = "HorasInsumidas_62FD";
            this.colHorasTarea.HeaderText = "Horas Insumidas";
            this.colHorasTarea.Name = "colHorasTarea";
            // 
            // colRepuestosTarea
            // 
            this.colRepuestosTarea.DataPropertyName = "EstadoRepuestos_62FD";
            this.colRepuestosTarea.HeaderText = "Repuestos Utilizados";
            this.colRepuestosTarea.Name = "colRepuestosTarea";
            // 
            // colObservacionesTarea
            // 
            this.colObservacionesTarea.DataPropertyName = "observacionesTecnicas_62FD";
            this.colObservacionesTarea.HeaderText = "Observaciones Técnicas";
            this.colObservacionesTarea.Name = "colObservacionesTarea";
            // 
            // CertificarIntervencion_62FD
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1166, 658);
            this.ControlBox = false;
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.btnSalir);
            this.Controls.Add(this.btnCancelar);
            this.Controls.Add(this.btnImprimir);
            this.Controls.Add(this.btnAplicar);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "CertificarIntervencion_62FD";
            this.Text = "CertificarIntervencion_62FD";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CertificarIntervencion_62FD_FormClosing);
            this.Load += new System.EventHandler(this.CertificarIntervencion_62FD_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btnAplicar;
        private System.Windows.Forms.Button btnImprimir;
        private System.Windows.Forms.Button btnCancelar;
        private System.Windows.Forms.Button btnSalir;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TextBox txtHorasInsumidas;
        private System.Windows.Forms.TextBox txtMatricula;
        private System.Windows.Forms.TextBox txtNroOrden;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtNroIntervencion;
        private System.Windows.Forms.TextBox txtFechaHoraFin;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtCodigoHabilitante;
        private System.Windows.Forms.TextBox txtObservaciones;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.DateTimePicker dtpFechaHoraCertificacion;
        private System.Windows.Forms.ComboBox cmbResultadoInspeccion;
        private System.Windows.Forms.DataGridViewTextBoxColumn colIdIntervencion;
        private System.Windows.Forms.DataGridViewTextBoxColumn colIdOrden;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMatricula;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFechaHoraFin;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTotalHoras;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.DataGridView dataGridView2;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDescripcionTarea;
        private System.Windows.Forms.DataGridViewTextBoxColumn colResultadoTarea;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHorasTarea;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRepuestosTarea;
        private System.Windows.Forms.DataGridViewTextBoxColumn colObservacionesTarea;
    }
}