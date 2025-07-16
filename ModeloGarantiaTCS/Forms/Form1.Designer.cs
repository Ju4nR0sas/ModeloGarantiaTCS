namespace ModeloGarantiaTCS
{
    partial class Form1
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnCargarCsv = new System.Windows.Forms.Button();
            this.dataGridViewTickets = new System.Windows.Forms.DataGridView();
            this.btnExportarCsv = new System.Windows.Forms.Button();
            this.txtFiltroClave = new System.Windows.Forms.TextBox();
            this.btnFiltrar = new System.Windows.Forms.Button();
            this.btnAnterior = new System.Windows.Forms.Button();
            this.btnSiguiente = new System.Windows.Forms.Button();
            this.lblPagina = new System.Windows.Forms.Label();
            this.btnBorrarFiltro = new System.Windows.Forms.Button();
            this.btnSoloCertificacion = new System.Windows.Forms.Button();
            this.btnSoloCerrados = new System.Windows.Forms.Button();
            this.btnFilter = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTickets)).BeginInit();
            this.SuspendLayout();
            // 
            // btnCargarCsv
            // 
            this.btnCargarCsv.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCargarCsv.Location = new System.Drawing.Point(16, 511);
            this.btnCargarCsv.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnCargarCsv.Name = "btnCargarCsv";
            this.btnCargarCsv.Size = new System.Drawing.Size(172, 28);
            this.btnCargarCsv.TabIndex = 0;
            this.btnCargarCsv.Tag = "";
            this.btnCargarCsv.Text = "Cargar CSV";
            this.btnCargarCsv.UseVisualStyleBackColor = true;
            this.btnCargarCsv.Click += new System.EventHandler(this.button1_Click);
            // 
            // dataGridViewTickets
            // 
            this.dataGridViewTickets.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewTickets.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewTickets.Location = new System.Drawing.Point(16, 50);
            this.dataGridViewTickets.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dataGridViewTickets.Name = "dataGridViewTickets";
            this.dataGridViewTickets.RowHeadersWidth = 51;
            this.dataGridViewTickets.Size = new System.Drawing.Size(1035, 453);
            this.dataGridViewTickets.TabIndex = 1;
            this.dataGridViewTickets.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewTickets_CellContentClick);
            // 
            // btnExportarCsv
            // 
            this.btnExportarCsv.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnExportarCsv.Location = new System.Drawing.Point(196, 511);
            this.btnExportarCsv.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnExportarCsv.Name = "btnExportarCsv";
            this.btnExportarCsv.Size = new System.Drawing.Size(188, 28);
            this.btnExportarCsv.TabIndex = 2;
            this.btnExportarCsv.Text = "Exportar CSV";
            this.btnExportarCsv.UseVisualStyleBackColor = true;
            this.btnExportarCsv.Click += new System.EventHandler(this.btnExportarCsv_Click);
            // 
            // txtFiltroClave
            // 
            this.txtFiltroClave.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFiltroClave.Location = new System.Drawing.Point(411, 15);
            this.txtFiltroClave.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtFiltroClave.Name = "txtFiltroClave";
            this.txtFiltroClave.Size = new System.Drawing.Size(360, 22);
            this.txtFiltroClave.TabIndex = 3;
            this.txtFiltroClave.TextChanged += new System.EventHandler(this.txtFiltroClave_TextChanged);
            // 
            // btnFiltrar
            // 
            this.btnFiltrar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFiltrar.Location = new System.Drawing.Point(779, 12);
            this.btnFiltrar.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnFiltrar.Name = "btnFiltrar";
            this.btnFiltrar.Size = new System.Drawing.Size(107, 28);
            this.btnFiltrar.TabIndex = 4;
            this.btnFiltrar.Text = "Filtrar";
            this.btnFiltrar.UseVisualStyleBackColor = true;
            this.btnFiltrar.Click += new System.EventHandler(this.btnFiltrar_Click);
            // 
            // btnAnterior
            // 
            this.btnAnterior.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAnterior.Location = new System.Drawing.Point(796, 511);
            this.btnAnterior.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnAnterior.Name = "btnAnterior";
            this.btnAnterior.Size = new System.Drawing.Size(32, 28);
            this.btnAnterior.TabIndex = 5;
            this.btnAnterior.Text = "<";
            this.btnAnterior.UseVisualStyleBackColor = true;
            this.btnAnterior.Click += new System.EventHandler(this.btnAnterior_Click);
            // 
            // btnSiguiente
            // 
            this.btnSiguiente.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSiguiente.Location = new System.Drawing.Point(1001, 511);
            this.btnSiguiente.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnSiguiente.Name = "btnSiguiente";
            this.btnSiguiente.Size = new System.Drawing.Size(32, 28);
            this.btnSiguiente.TabIndex = 6;
            this.btnSiguiente.Text = ">";
            this.btnSiguiente.UseVisualStyleBackColor = true;
            this.btnSiguiente.Click += new System.EventHandler(this.btnSiguiente_Click);
            // 
            // lblPagina
            // 
            this.lblPagina.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblPagina.AutoSize = true;
            this.lblPagina.Location = new System.Drawing.Point(861, 517);
            this.lblPagina.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPagina.Name = "lblPagina";
            this.lblPagina.Size = new System.Drawing.Size(63, 16);
            this.lblPagina.TabIndex = 7;
            this.lblPagina.Text = "Página: 1";
            this.lblPagina.Click += new System.EventHandler(this.lblPagina_Click);
            // 
            // btnBorrarFiltro
            // 
            this.btnBorrarFiltro.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBorrarFiltro.Location = new System.Drawing.Point(894, 11);
            this.btnBorrarFiltro.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnBorrarFiltro.Name = "btnBorrarFiltro";
            this.btnBorrarFiltro.Size = new System.Drawing.Size(100, 28);
            this.btnBorrarFiltro.TabIndex = 8;
            this.btnBorrarFiltro.Text = "Limpiar";
            this.btnBorrarFiltro.UseVisualStyleBackColor = true;
            this.btnBorrarFiltro.Click += new System.EventHandler(this.btnBorrarFiltro_Click);
            // 
            // btnSoloCertificacion
            // 
            this.btnSoloCertificacion.Location = new System.Drawing.Point(21, 11);
            this.btnSoloCertificacion.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnSoloCertificacion.Name = "btnSoloCertificacion";
            this.btnSoloCertificacion.Size = new System.Drawing.Size(167, 28);
            this.btnSoloCertificacion.TabIndex = 9;
            this.btnSoloCertificacion.Text = "Con Certificacion";
            this.btnSoloCertificacion.UseVisualStyleBackColor = true;
            this.btnSoloCertificacion.Click += new System.EventHandler(this.btnSoloCertificacion_Click);
            // 
            // btnSoloCerrados
            // 
            this.btnSoloCerrados.Location = new System.Drawing.Point(196, 12);
            this.btnSoloCerrados.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnSoloCerrados.Name = "btnSoloCerrados";
            this.btnSoloCerrados.Size = new System.Drawing.Size(188, 28);
            this.btnSoloCerrados.TabIndex = 10;
            this.btnSoloCerrados.Text = "En Garantia";
            this.btnSoloCerrados.UseVisualStyleBackColor = true;
            this.btnSoloCerrados.Click += new System.EventHandler(this.btnSoloCerrados_Click);
            // 
            // btnFilter
            // 
            this.btnFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFilter.Location = new System.Drawing.Point(1001, 11);
            this.btnFilter.Name = "btnFilter";
            this.btnFilter.Size = new System.Drawing.Size(50, 28);
            this.btnFilter.TabIndex = 11;
            this.btnFilter.UseVisualStyleBackColor = true;
            this.btnFilter.Click += new System.EventHandler(this.btnFilter_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1067, 554);
            this.Controls.Add(this.btnFilter);
            this.Controls.Add(this.btnSoloCerrados);
            this.Controls.Add(this.btnSoloCertificacion);
            this.Controls.Add(this.btnBorrarFiltro);
            this.Controls.Add(this.lblPagina);
            this.Controls.Add(this.btnSiguiente);
            this.Controls.Add(this.btnAnterior);
            this.Controls.Add(this.btnFiltrar);
            this.Controls.Add(this.txtFiltroClave);
            this.Controls.Add(this.btnExportarCsv);
            this.Controls.Add(this.dataGridViewTickets);
            this.Controls.Add(this.btnCargarCsv);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "Form1";
            this.Text = "Calculo para Garantia";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTickets)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCargarCsv;
        private System.Windows.Forms.DataGridView dataGridViewTickets;
        private System.Windows.Forms.Button btnExportarCsv;
        private System.Windows.Forms.TextBox txtFiltroClave;
        private System.Windows.Forms.Button btnFiltrar;
        private System.Windows.Forms.Button btnAnterior;
        private System.Windows.Forms.Button btnSiguiente;
        private System.Windows.Forms.Label lblPagina;
        private System.Windows.Forms.Button btnBorrarFiltro;
        private System.Windows.Forms.Button btnSoloCertificacion;
        private System.Windows.Forms.Button btnSoloCerrados;
        private System.Windows.Forms.Button btnFilter;
    }
}

