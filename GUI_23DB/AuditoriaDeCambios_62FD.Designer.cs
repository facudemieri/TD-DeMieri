namespace GUI_23DB
{
    partial class AuditoriaDeCambios_62FD
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.dtpFechaHasta = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.cmbMatricula = new System.Windows.Forms.ComboBox();
            this.cmbModelo = new System.Windows.Forms.ComboBox();
            this.dtpFechaDesde = new System.Windows.Forms.DateTimePicker();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.btnAplicar = new System.Windows.Forms.Button();
            this.btnLimpiar = new System.Windows.Forms.Button();
            this.btnSalir = new System.Windows.Forms.Button();
            this.btnActivar = new System.Windows.Forms.Button();
            this.colMatriculaCambio = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colFechaCambio = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colHoraCambio = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNroSerieCambio = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colModeloCambio = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colFabricanteCambio = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEstadoOperativoCambio = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colHorasVueloCambio = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colIntervaloCambio = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colProxRevisionCambio = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colActivaCambio = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colVigenteCambio = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
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
            this.panel1.Size = new System.Drawing.Size(1314, 60);
            this.panel1.TabIndex = 13;
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
            this.label1.Size = new System.Drawing.Size(399, 25);
            this.label1.TabIndex = 8;
            this.label1.Text = "Auditoria de Cambios - AeroManager";
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colMatriculaCambio,
            this.colFechaCambio,
            this.colHoraCambio,
            this.colNroSerieCambio,
            this.colModeloCambio,
            this.colFabricanteCambio,
            this.colEstadoOperativoCambio,
            this.colHorasVueloCambio,
            this.colIntervaloCambio,
            this.colProxRevisionCambio,
            this.colActivaCambio,
            this.colVigenteCambio});
            this.dataGridView1.Location = new System.Drawing.Point(21, 78);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(1243, 300);
            this.dataGridView1.TabIndex = 14;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 109F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 194F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 126F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 213F));
            this.tableLayoutPanel1.Controls.Add(this.dtpFechaHasta, 3, 1);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label4, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.label5, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.cmbMatricula, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.cmbModelo, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.dtpFechaDesde, 3, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(221, 417);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 53F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(642, 100);
            this.tableLayoutPanel1.TabIndex = 15;
            // 
            // dtpFechaHasta
            // 
            this.dtpFechaHasta.Location = new System.Drawing.Point(432, 56);
            this.dtpFechaHasta.Name = "dtpFechaHasta";
            this.dtpFechaHasta.Size = new System.Drawing.Size(200, 23);
            this.dtpFechaHasta.TabIndex = 7;
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
            this.label3.Location = new System.Drawing.Point(3, 53);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(61, 17);
            this.label3.TabIndex = 1;
            this.label3.Text = "Modelo:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(306, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(95, 17);
            this.label4.TabIndex = 2;
            this.label4.Text = "Fecha Desde:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(306, 53);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(92, 17);
            this.label5.TabIndex = 3;
            this.label5.Text = "Fecha Hasta:";
            // 
            // cmbMatricula
            // 
            this.cmbMatricula.FormattingEnabled = true;
            this.cmbMatricula.Location = new System.Drawing.Point(112, 3);
            this.cmbMatricula.Name = "cmbMatricula";
            this.cmbMatricula.Size = new System.Drawing.Size(158, 25);
            this.cmbMatricula.TabIndex = 4;
            // 
            // cmbModelo
            // 
            this.cmbModelo.FormattingEnabled = true;
            this.cmbModelo.Location = new System.Drawing.Point(112, 56);
            this.cmbModelo.Name = "cmbModelo";
            this.cmbModelo.Size = new System.Drawing.Size(158, 25);
            this.cmbModelo.TabIndex = 5;
            // 
            // dtpFechaDesde
            // 
            this.dtpFechaDesde.Location = new System.Drawing.Point(432, 3);
            this.dtpFechaDesde.Name = "dtpFechaDesde";
            this.dtpFechaDesde.Size = new System.Drawing.Size(200, 23);
            this.dtpFechaDesde.TabIndex = 6;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripStatusLabel2,
            this.toolStripStatusLabel3});
            this.statusStrip1.Location = new System.Drawing.Point(0, 566);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1314, 22);
            this.statusStrip1.TabIndex = 16;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(433, 17);
            this.toolStripStatusLabel1.Spring = true;
            this.toolStripStatusLabel1.Text = "Modo:";
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(433, 17);
            this.toolStripStatusLabel2.Spring = true;
            this.toolStripStatusLabel2.Text = "Usuario:";
            // 
            // toolStripStatusLabel3
            // 
            this.toolStripStatusLabel3.Name = "toolStripStatusLabel3";
            this.toolStripStatusLabel3.Size = new System.Drawing.Size(433, 17);
            this.toolStripStatusLabel3.Spring = true;
            this.toolStripStatusLabel3.Text = "Fecha:";
            // 
            // btnAplicar
            // 
            this.btnAplicar.Location = new System.Drawing.Point(909, 404);
            this.btnAplicar.Name = "btnAplicar";
            this.btnAplicar.Size = new System.Drawing.Size(111, 31);
            this.btnAplicar.TabIndex = 17;
            this.btnAplicar.Text = "APLICAR";
            this.btnAplicar.UseVisualStyleBackColor = true;
            this.btnAplicar.Click += new System.EventHandler(this.btnAplicar_Click);
            // 
            // btnLimpiar
            // 
            this.btnLimpiar.Location = new System.Drawing.Point(909, 441);
            this.btnLimpiar.Name = "btnLimpiar";
            this.btnLimpiar.Size = new System.Drawing.Size(111, 31);
            this.btnLimpiar.TabIndex = 18;
            this.btnLimpiar.Text = "LIMPIAR";
            this.btnLimpiar.UseVisualStyleBackColor = true;
            this.btnLimpiar.Click += new System.EventHandler(this.btnLimpiar_Click);
            // 
            // btnSalir
            // 
            this.btnSalir.Location = new System.Drawing.Point(909, 478);
            this.btnSalir.Name = "btnSalir";
            this.btnSalir.Size = new System.Drawing.Size(111, 31);
            this.btnSalir.TabIndex = 19;
            this.btnSalir.Text = "SALIR";
            this.btnSalir.UseVisualStyleBackColor = true;
            this.btnSalir.Click += new System.EventHandler(this.btnSalir_Click);
            // 
            // btnActivar
            // 
            this.btnActivar.Location = new System.Drawing.Point(909, 515);
            this.btnActivar.Name = "btnActivar";
            this.btnActivar.Size = new System.Drawing.Size(111, 31);
            this.btnActivar.TabIndex = 20;
            this.btnActivar.Text = "ACTIVAR";
            this.btnActivar.UseVisualStyleBackColor = true;
            this.btnActivar.Click += new System.EventHandler(this.btnActivar_Click);
            // 
            // colMatriculaCambio
            // 
            this.colMatriculaCambio.DataPropertyName = "Matricula_62FD";
            this.colMatriculaCambio.HeaderText = "Matrícula";
            this.colMatriculaCambio.Name = "colMatriculaCambio";
            // 
            // colFechaCambio
            // 
            this.colFechaCambio.DataPropertyName = "Fecha_62FD";
            this.colFechaCambio.HeaderText = "Fecha";
            this.colFechaCambio.Name = "colFechaCambio";
            // 
            // colHoraCambio
            // 
            this.colHoraCambio.DataPropertyName = "Hora_62FD";
            this.colHoraCambio.HeaderText = "Hora";
            this.colHoraCambio.Name = "colHoraCambio";
            // 
            // colNroSerieCambio
            // 
            this.colNroSerieCambio.DataPropertyName = "NroSerie_62FD";
            this.colNroSerieCambio.HeaderText = "Nro de Serie";
            this.colNroSerieCambio.Name = "colNroSerieCambio";
            // 
            // colModeloCambio
            // 
            this.colModeloCambio.DataPropertyName = "Modelo_62FD";
            this.colModeloCambio.HeaderText = "Modelo";
            this.colModeloCambio.Name = "colModeloCambio";
            // 
            // colFabricanteCambio
            // 
            this.colFabricanteCambio.DataPropertyName = "Fabricante_62FD";
            this.colFabricanteCambio.HeaderText = "Fabricante";
            this.colFabricanteCambio.Name = "colFabricanteCambio";
            // 
            // colEstadoOperativoCambio
            // 
            this.colEstadoOperativoCambio.DataPropertyName = "EstadoOperativo_62FD";
            this.colEstadoOperativoCambio.HeaderText = "Estado Operativo";
            this.colEstadoOperativoCambio.Name = "colEstadoOperativoCambio";
            // 
            // colHorasVueloCambio
            // 
            this.colHorasVueloCambio.DataPropertyName = "HorasVueloAcumuladas_62FD";
            this.colHorasVueloCambio.HeaderText = "Horas de Vuelo";
            this.colHorasVueloCambio.Name = "colHorasVueloCambio";
            // 
            // colIntervaloCambio
            // 
            this.colIntervaloCambio.DataPropertyName = "IntervaloRevision_62FD";
            this.colIntervaloCambio.HeaderText = "Intervalo (hs)";
            this.colIntervaloCambio.Name = "colIntervaloCambio";
            // 
            // colProxRevisionCambio
            // 
            this.colProxRevisionCambio.DataPropertyName = "FechaProximaRevision_62FD";
            this.colProxRevisionCambio.HeaderText = "Próxima Revisión";
            this.colProxRevisionCambio.Name = "colProxRevisionCambio";
            // 
            // colActivaCambio
            // 
            this.colActivaCambio.DataPropertyName = "Activo_62FD";
            this.colActivaCambio.HeaderText = "Activa";
            this.colActivaCambio.Name = "colActivaCambio";
            // 
            // colVigenteCambio
            // 
            this.colVigenteCambio.DataPropertyName = "Act_62FD";
            this.colVigenteCambio.HeaderText = "Vigente";
            this.colVigenteCambio.Name = "colVigenteCambio";
            // 
            // AuditoriaDeCambios_62FD
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1314, 588);
            this.ControlBox = false;
            this.Controls.Add(this.btnActivar);
            this.Controls.Add(this.btnSalir);
            this.Controls.Add(this.btnLimpiar);
            this.Controls.Add(this.btnAplicar);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "AuditoriaDeCambios_62FD";
            this.Text = "AuditoriaDeCambios_62FD";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AuditoriaDeCambios_62FD_FormClosing);
            this.Load += new System.EventHandler(this.AuditoriaDeCambios_62FD_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
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
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.DateTimePicker dtpFechaHasta;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cmbMatricula;
        private System.Windows.Forms.ComboBox cmbModelo;
        private System.Windows.Forms.DateTimePicker dtpFechaDesde;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel3;
        private System.Windows.Forms.Button btnAplicar;
        private System.Windows.Forms.Button btnLimpiar;
        private System.Windows.Forms.Button btnSalir;
        private System.Windows.Forms.Button btnActivar;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMatriculaCambio;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFechaCambio;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHoraCambio;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNroSerieCambio;
        private System.Windows.Forms.DataGridViewTextBoxColumn colModeloCambio;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFabricanteCambio;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEstadoOperativoCambio;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHorasVueloCambio;
        private System.Windows.Forms.DataGridViewTextBoxColumn colIntervaloCambio;
        private System.Windows.Forms.DataGridViewTextBoxColumn colProxRevisionCambio;
        private System.Windows.Forms.DataGridViewTextBoxColumn colActivaCambio;
        private System.Windows.Forms.DataGridViewTextBoxColumn colVigenteCambio;
    }
}