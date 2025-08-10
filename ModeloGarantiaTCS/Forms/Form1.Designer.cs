using System.Windows.Forms;

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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.btnCargarCsv = new System.Windows.Forms.Button();
            this.dataGridViewTickets = new System.Windows.Forms.DataGridView();
            this.btnExportarCsv = new System.Windows.Forms.Button();
            this.txtFiltroClave = new System.Windows.Forms.TextBox();
            this.btnFiltrar = new System.Windows.Forms.Button();
            this.btnAnterior = new System.Windows.Forms.Button();
            this.btnSiguiente = new System.Windows.Forms.Button();
            this.lblPagina = new System.Windows.Forms.Label();
            this.btnBorrarFiltro = new System.Windows.Forms.Button();
            this.btnFilter = new System.Windows.Forms.Button();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.sinCertificaciónToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cerradosToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.certificaciónVencidaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fechasTentativasToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTickets)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCargarCsv
            // 
            this.btnCargarCsv.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCargarCsv.Location = new System.Drawing.Point(14, 543);
            this.btnCargarCsv.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.btnCargarCsv.Name = "btnCargarCsv";
            this.btnCargarCsv.Size = new System.Drawing.Size(151, 29);
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
            this.dataGridViewTickets.BackgroundColor = System.Drawing.Color.DarkGray;
            this.dataGridViewTickets.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewTickets.Location = new System.Drawing.Point(14, 53);
            this.dataGridViewTickets.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.dataGridViewTickets.Name = "dataGridViewTickets";
            this.dataGridViewTickets.RowHeadersWidth = 51;
            this.dataGridViewTickets.Size = new System.Drawing.Size(964, 481);
            this.dataGridViewTickets.TabIndex = 1;
            this.dataGridViewTickets.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewTickets_CellContentClick);
            // 
            // btnExportarCsv
            // 
            this.btnExportarCsv.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnExportarCsv.Location = new System.Drawing.Point(171, 543);
            this.btnExportarCsv.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.btnExportarCsv.Name = "btnExportarCsv";
            this.btnExportarCsv.Size = new System.Drawing.Size(165, 29);
            this.btnExportarCsv.TabIndex = 2;
            this.btnExportarCsv.Text = "Exportar CSV";
            this.btnExportarCsv.UseVisualStyleBackColor = true;
            this.btnExportarCsv.Click += new System.EventHandler(this.btnExportarCsv_Click);
            // 
            // txtFiltroClave
            // 
            this.txtFiltroClave.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFiltroClave.Location = new System.Drawing.Point(359, 16);
            this.txtFiltroClave.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.txtFiltroClave.Name = "txtFiltroClave";
            this.txtFiltroClave.Size = new System.Drawing.Size(373, 25);
            this.txtFiltroClave.TabIndex = 3;
            this.txtFiltroClave.TextChanged += new System.EventHandler(this.txtFiltroClave_TextChanged);
            // 
            // btnFiltrar
            // 
            this.btnFiltrar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFiltrar.Location = new System.Drawing.Point(740, 12);
            this.btnFiltrar.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.btnFiltrar.Name = "btnFiltrar";
            this.btnFiltrar.Size = new System.Drawing.Size(93, 29);
            this.btnFiltrar.TabIndex = 4;
            this.btnFiltrar.Text = "Buscar";
            this.btnFiltrar.UseVisualStyleBackColor = true;
            this.btnFiltrar.Click += new System.EventHandler(this.btnFiltrar_Click);
            // 
            // btnAnterior
            // 
            this.btnAnterior.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAnterior.Location = new System.Drawing.Point(754, 543);
            this.btnAnterior.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.btnAnterior.Name = "btnAnterior";
            this.btnAnterior.Size = new System.Drawing.Size(28, 29);
            this.btnAnterior.TabIndex = 5;
            this.btnAnterior.Text = "<";
            this.btnAnterior.UseVisualStyleBackColor = true;
            this.btnAnterior.Click += new System.EventHandler(this.btnAnterior_Click);
            // 
            // btnSiguiente
            // 
            this.btnSiguiente.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSiguiente.Location = new System.Drawing.Point(933, 543);
            this.btnSiguiente.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.btnSiguiente.Name = "btnSiguiente";
            this.btnSiguiente.Size = new System.Drawing.Size(28, 29);
            this.btnSiguiente.TabIndex = 6;
            this.btnSiguiente.Text = ">";
            this.btnSiguiente.UseVisualStyleBackColor = true;
            this.btnSiguiente.Click += new System.EventHandler(this.btnSiguiente_Click);
            // 
            // lblPagina
            // 
            this.lblPagina.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblPagina.AutoSize = true;
            this.lblPagina.Location = new System.Drawing.Point(811, 549);
            this.lblPagina.Name = "lblPagina";
            this.lblPagina.Size = new System.Drawing.Size(59, 17);
            this.lblPagina.TabIndex = 7;
            this.lblPagina.Text = "Página: 1";
            this.lblPagina.Click += new System.EventHandler(this.lblPagina_Click);
            // 
            // btnBorrarFiltro
            // 
            this.btnBorrarFiltro.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBorrarFiltro.Location = new System.Drawing.Point(840, 12);
            this.btnBorrarFiltro.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.btnBorrarFiltro.Name = "btnBorrarFiltro";
            this.btnBorrarFiltro.Size = new System.Drawing.Size(87, 29);
            this.btnBorrarFiltro.TabIndex = 8;
            this.btnBorrarFiltro.Text = "Limpiar";
            this.btnBorrarFiltro.UseVisualStyleBackColor = true;
            this.btnBorrarFiltro.Click += new System.EventHandler(this.btnBorrarFiltro_Click);
            // 
            // btnFilter
            // 
            this.btnFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFilter.Location = new System.Drawing.Point(933, 12);
            this.btnFilter.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btnFilter.Name = "btnFilter";
            this.btnFilter.Size = new System.Drawing.Size(44, 29);
            this.btnFilter.TabIndex = 11;
            this.btnFilter.UseVisualStyleBackColor = true;
            this.btnFilter.Click += new System.EventHandler(this.btnFilter_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sinCertificaciónToolStripMenuItem,
            this.cerradosToolStripMenuItem,
            this.certificaciónVencidaToolStripMenuItem,
            this.fechasTentativasToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(288, 100);
            // 
            // sinCertificaciónToolStripMenuItem
            // 
            this.sinCertificaciónToolStripMenuItem.Name = "sinCertificaciónToolStripMenuItem";
            this.sinCertificaciónToolStripMenuItem.Size = new System.Drawing.Size(287, 24);
            this.sinCertificaciónToolStripMenuItem.Text = "Aún sin Certificación";
            // 
            // cerradosToolStripMenuItem
            // 
            this.cerradosToolStripMenuItem.Name = "cerradosToolStripMenuItem";
            this.cerradosToolStripMenuItem.Size = new System.Drawing.Size(287, 24);
            this.cerradosToolStripMenuItem.Text = "Cerrados";
            // 
            // certificaciónVencidaToolStripMenuItem
            // 
            this.certificaciónVencidaToolStripMenuItem.Name = "certificaciónVencidaToolStripMenuItem";
            this.certificaciónVencidaToolStripMenuItem.Size = new System.Drawing.Size(287, 24);
            this.certificaciónVencidaToolStripMenuItem.Text = "Certificación Vencida (Garantía)";
            // 
            // fechasTentativasToolStripMenuItem
            // 
            this.fechasTentativasToolStripMenuItem.Name = "fechasTentativasToolStripMenuItem";
            this.fechasTentativasToolStripMenuItem.Size = new System.Drawing.Size(287, 24);
            this.fechasTentativasToolStripMenuItem.Text = "Fechas Tentativas";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(992, 589);
            this.Controls.Add(this.btnFilter);
            this.Controls.Add(this.btnBorrarFiltro);
            this.Controls.Add(this.lblPagina);
            this.Controls.Add(this.btnSiguiente);
            this.Controls.Add(this.btnAnterior);
            this.Controls.Add(this.btnFiltrar);
            this.Controls.Add(this.txtFiltroClave);
            this.Controls.Add(this.btnExportarCsv);
            this.Controls.Add(this.dataGridViewTickets);
            this.Controls.Add(this.btnCargarCsv);
            this.Font = new System.Drawing.Font("Segoe UI Variable Text", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.MinimumSize = new System.Drawing.Size(830, 636);
            this.Name = "Form1";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.Text = "Calculo para Garantia";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTickets)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
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
        private System.Windows.Forms.Button btnFilter;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem sinCertificaciónToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem certificaciónVencidaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fechasTentativasToolStripMenuItem;
        private ToolStripMenuItem cerradosToolStripMenuItem;
    }
}

