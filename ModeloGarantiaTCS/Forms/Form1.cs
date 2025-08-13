using ModeloGarantiaTCS.Models;
using ModeloGarantiaTCS.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace ModeloGarantiaTCS
{

    public partial class Form1 : Form
    {
        private List<Ticket> _tickets = new List<Ticket>();
        private CsvService _csvService = new CsvService();
        private int _paginaActual = 1;
        private int _registrosPorPagina;
        private PaginacionService<Ticket> _paginador = new PaginacionService<Ticket>(new List<Ticket>(), 10);
        private bool _filtroActivo = false;
        private static readonly DateTime fechaActual = DateTime.Today;
        private bool ocultarPasoProduccionEnProximoBinding = false;
        private string _nombreFiltroActual = "";

        //PRUEBA
        private const int WM_NCLBUTTONDOWN = 0x00A1;
        private const int HTSYSMENU = 3;
        public Form1()
        {
            InitializeComponent();

        // Se agrega un evento al botón de filtro
        contextMenuStrip1.ItemClicked += new ToolStripItemClickedEventHandler(ContexMenu_ItemClicked);
            //Submenu
            enGarantíaToolStripMenuItem.Click += (s, e) => EnGarantia();
            garantíaVencidaToolStripMenuItem.Click += (s, e) => GarantiaVencida();

            // Configura DataGridView como solo lectura
            dataGridViewTickets.ReadOnly = true;

            // Configura placeholder en el filtro
            txtFiltroClave.ForeColor = Color.Gray;
            txtFiltroClave.Text = "Ingrese número de caso";

            txtFiltroClave.GotFocus += (s, e) =>
            {
                if (txtFiltroClave.Text == "Ingrese número de caso")
                {
                    txtFiltroClave.Text = "";
                    txtFiltroClave.ForeColor = Color.Black;
                }
            };

            txtFiltroClave.LostFocus += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtFiltroClave.Text))
                {
                    txtFiltroClave.Text = "Ingrese clave para filtrar";
                    txtFiltroClave.ForeColor = Color.Gray;
                }
            };

            // Conectar KeyPress para validar números
            txtFiltroClave.KeyPress += txtFiltroClave_KeyPress;
            this.Load += Form1_Load;
        }
        #region Prueba
        //PRUEBA
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_NCLBUTTONDOWN && m.WParam.ToInt32() == HTSYSMENU)
            {
                // Mostrar ventana con el ícono grande
                ShowIconWindow();
            }
            base.WndProc(ref m);
        }

        //PRUEBA
        private void ShowIconWindow()
        {
            Form iconForm = new Form();
            iconForm.Text = "Zzzzzzzzz";
            iconForm.StartPosition = FormStartPosition.CenterParent;
            iconForm.Size = new Size(500, 500);
            iconForm.FormBorderStyle = FormBorderStyle.FixedDialog;
            iconForm.MaximizeBox = false;
            iconForm.MinimizeBox = false;

            PictureBox pictureBox = new PictureBox();
            pictureBox.Dock = DockStyle.Fill;

            // Convertir el icono del formulario a imagen (Bitmap) con tamaño grande
            Bitmap bmp = this.Icon.ToBitmap();

            // Crear una imagen más grande a partir del icono
            Bitmap bmpLarge = new Bitmap(bmp, 500, 500);

            pictureBox.Image = bmpLarge;
            pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;

            iconForm.Controls.Add(pictureBox);
            iconForm.ShowDialog();

            // Liberar recursos
            bmp.Dispose();
            bmpLarge.Dispose();
        }
        #endregion

        #region Ticket y Load

        /* Devuelve true si existe al menos un ticket cargado
         * de lo contrario muestra un mensaje y devuelve false.*/
        private bool HayTicketsCargados()
        {
            dataGridViewTickets.DataBindingComplete += dataGridViewTickets_DataBindingComplete;   

            if (_tickets == null || _tickets.Count == 0)
            {
                MessageBox.Show(
                    "Primero debes cargar un archivo CSV.",
                    "Sin datos",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return false;
            }
            return true;
        }

        private void EnGarantíaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        // Se define una ruta absoluta
        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                string basePath = AppDomain.CurrentDomain.BaseDirectory;
                string imagePath = Path.Combine(basePath, "Resources", "sort.png");

                if (File.Exists(imagePath))
                {
                    btnFilter.Image = Image.FromFile(imagePath);
                    btnFilter.ImageAlign = ContentAlignment.MiddleCenter;
                    btnFilter.TextImageRelation = TextImageRelation.ImageBeforeText;
                }
                else
                {
                    MessageBox.Show($"No se encontró la imagen: {imagePath}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error cargando la imagen: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            #region Leyenda
            //Leyenda
            //Panel Rojo
            Panel panelRojo = new Panel();
            panelRojo.BackColor = Color.Red;
            panelRojo.Size = new Size(20, 20);
            panelRojo.Location = new Point(20, 15);
            this.Controls.Add(panelRojo);

            Label labelRojo = new Label();
            labelRojo.Text = "Vencido";
            labelRojo.Location = new Point(45, 18);
            labelRojo.AutoSize = true;
            this.Controls.Add(labelRojo);

            // Panel Amarillo 
            Panel panelAmarillo = new Panel();
            panelAmarillo.BackColor = Color.Yellow;
            panelAmarillo.Size = new Size(20, 20);
            panelAmarillo.Location = new Point(110, 15);
            this.Controls.Add(panelAmarillo);

            Label labelAmarillo = new Label();
            labelAmarillo.Text = "Por Vencer Garantía";
            labelAmarillo.Location = new Point(135, 18);
            labelAmarillo.AutoSize = true;
            this.Controls.Add(labelAmarillo);

            // Panel Verde
            Panel panelVerde = new Panel();
            panelVerde.BackColor = Color.Green;
            panelVerde.Size = new Size(20, 20);
            panelVerde.Location = new Point(270, 15);
            this.Controls.Add(panelVerde);

            Label labelVerde = new Label();
            labelVerde.Text = "Cerrado";
            labelVerde.Location = new Point(295, 18);
            labelVerde.AutoSize = true;
            this.Controls.Add(labelVerde);
            #endregion
        }
        #endregion

        #region Eventos de Filtro
        private void btnFilter_Click(object sender, EventArgs e)
        {
            btnFilter.AutoSize = true;

            contextMenuStrip1.Show(btnFilter, new Point(0, btnFilter.Height));
        }


        //Switch de menú principal
        private void ContexMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            switch (e.ClickedItem.Name)
            {
                case "sinCertificaciónToolStripMenuItem":
                    SinCertificacion();
                    break;
                case "certificaciónVencidaToolStripMenuItem":
                    break;
                case "cerradosToolStripMenuItem":
                    EstadoCerrado();
                    break;
                case "fechasTentativasToolStripMenuItem":
                    FechasTentativas();
                    break;
                default:
                    break;
            }
        }
        #endregion

        #region Filtros de tickets

        //Sin Certificación - "Vacíos"
        public void SinCertificacion()
        {
            if (_nombreFiltroActual == "Filtrado: Sin Certificación") return;
            if (!HayTicketsCargados()) return;

            var filtrados = _tickets
                .Where(t => t.FechaCertificacion == null &&
                t.Estado != "Cerrado")
                .ToList();

            dataGridViewTickets.DataSource = filtrados;
            AplicarEstiloGrid();
            ActivarFiltroVista("Filtrado: Sin Certificación");
        }

        //Certificación Vencida
        private void EnGarantia()
        {
            if (_nombreFiltroActual == "Filtrado: En Garantía") return;
            if (!HayTicketsCargados()) return;

            var filtrados = _tickets
                .Where(t => t.FechaCertificacion < fechaActual &&
                !t.FechaRealPasoProduccion.HasValue &&
                t.FechaTentativaGarantia >= fechaActual &&
                t.Estado != "Cerrado") 
                .ToList();

            dataGridViewTickets.DataSource = filtrados;

            AplicarEstiloGrid();
            ActivarFiltroVista("Filtrado: En Garantía");
            OcultarPasoProduccion();
        }

        //Garantía Vencida
        private void GarantiaVencida()
        {
            if (_nombreFiltroActual == "Filtrado: Vencida") return;
            if (!HayTicketsCargados()) return;

            var filtrados = _tickets
                .Where(t => t.FechaTentativaGarantia < fechaActual &&
                t.Estado != "Cerrado")
                .ToList();

            dataGridViewTickets.DataSource = filtrados;

            AplicarEstiloGrid();
            ActivarFiltroVista("Filtrado: Vencida");
            OcultarPasoProduccion();
        }

        //Estado Cerrado
        public void EstadoCerrado()
        {
            if (_nombreFiltroActual == "Filtrado: Cerrado") return;
            if (!HayTicketsCargados()) return;

            var filtrados = _tickets
                .Where(t => t.Estado == "Cerrado")
                .ToList();

            dataGridViewTickets.DataSource = filtrados;
            AplicarEstiloGrid();
            ActivarFiltroVista("Filtrado: Cerrados");
        }

        //Los que ya tienen certificación y una fecha tentativa de paso a producción
        public void FechasTentativas()
        {
            if (_nombreFiltroActual == "Filtrado: Tentativas") return;
            if (!HayTicketsCargados()) return;

            var filtrados = _tickets
                .Where(t => t.FechaCertificacion > fechaActual && t.FechaTentativaPasoProduccion.HasValue)
                .ToList();

            dataGridViewTickets.DataSource = filtrados;
            AplicarEstiloGrid();
            ActivarFiltroVista("Filtrado: Tentativas");
            OcultarPasoProduccion();
        }

        //Fechas actuales
        public void FechasActuales()
        {
            if (_nombreFiltroActual == "Filtrado: Actuales") return;
            if (!HayTicketsCargados()) return;

            var filtrados = _tickets
                .ToList();

            dataGridViewTickets.DataSource = filtrados;
            AplicarEstiloGrid();
            ActivarFiltroVista("Filtrado: Actuales");
        }

        // Botón Busqueda
        private void btnFiltrar_Click(object sender, EventArgs e)
        {
            string clave = txtFiltroClave.Text.Trim();

            if (string.IsNullOrEmpty(clave) || clave == "Ingrese clave para filtrar")
            {
                MostrarTickets();
                return;
            }

            // Validar que solo haya números
            if (!clave.All(char.IsDigit))
            {
                MessageBox.Show("Por favor ingrese solo números en el filtro de clave.",
                    "Filtro inválido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var filtrados = _tickets
                .Where(t => t.Clave != null && t.Clave.IndexOf(clave, StringComparison.OrdinalIgnoreCase) >= 0)
                .ToList();

            if (filtrados.Count == 0)
            {
                MessageBox.Show("No se encontraron registros con ese filtro.",
                    "Sin resultados", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            dataGridViewTickets.DataSource = null;
            dataGridViewTickets.DataSource = filtrados;
            AplicarEstiloGrid();
        }

        // Validación de entrada de texto para la busqueda de clave
        private void txtFiltroClave_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Permitir solo números y teclas de control
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true; // Bloquea el carácter
            }
        }

        //Borrar filtro
        private void btnBorrarFiltro_Click(object sender, EventArgs e)
        {
            txtFiltroClave.Text = "";
            DesactivarFiltroVista();
            MostrarTickets();
        }
        #endregion

        #region Vista y Paginación
        private void MostrarTickets()
        {
            _paginaActual = 1;
            _registrosPorPagina = Math.Max(1, dataGridViewTickets.DisplayRectangle.Height / dataGridViewTickets.RowTemplate.Height);
            _paginador = new PaginacionService<Ticket>(_tickets, _registrosPorPagina);
            MostrarPagina();
            DesactivarFiltroVista();
        }

        private void MostrarPagina()
        {
            var paged = _paginador.ObtenerPagina(_paginaActual);

            dataGridViewTickets.DataSource = null;
            dataGridViewTickets.DataSource = paged;
            lblPagina.Text = $"Página: {_paginaActual} de {_paginador.TotalPaginas}";

            AplicarEstiloGrid();
        }

        private int PaginaMaxima() => _paginador.TotalPaginas;

        private void btnAnterior_Click(object sender, EventArgs e)
        {
            if (_filtroActivo) return;

            if (_paginaActual > 1)
            {
                _paginaActual--;
                MostrarPagina();
            }
        }

        private void btnSiguiente_Click(object sender, EventArgs e)
        {
            if (_filtroActivo) return;

            if (_paginaActual < PaginaMaxima())
            {
                _paginaActual++;
                MostrarPagina();
            }
        }

        /// Deshabilita paginación y marca la vista como filtrada.
        private void ActivarFiltroVista(string descripcion)
        {
            _filtroActivo = true;
            _nombreFiltroActual = descripcion;
            btnAnterior.Enabled = false;
            btnSiguiente.Enabled = false;
            lblPagina.Text = descripcion;
        }

        /// Vuelve al modo de paginación normal.
        private void DesactivarFiltroVista()
        {
            _filtroActivo = false;
            _nombreFiltroActual = "";
            btnAnterior.Enabled = true;
            btnSiguiente.Enabled = true;
        }

        #endregion

        #region Carga y Exportación CSV
        private void button1_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "CSV files (*.csv)|*.csv";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    _tickets = _csvService.CargarCsv(ofd.FileName);
                    MostrarTickets();
                }
            }
        }

        private void btnExportarCsv_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "CSV files (*.csv)|*.csv";
                sfd.FileName = "TicketsExportados.csv";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    _csvService.ExportarCsv(_tickets, sfd.FileName);
                    MessageBox.Show("Archivo exportado correctamente", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
        #endregion

        #region Eventos de DataGridView

        private void OcultarPasoProduccion()
        {
            if (dataGridViewTickets.Columns["FechaRealPasoProduccion"] != null)
                dataGridViewTickets.Columns["FechaRealPasoProduccion"].Visible = false;
        }

        //DataBindingComplete es un evento del control DataGridView que se dispara cuando se completa el enlace de datos.
        private void dataGridViewTickets_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if (ocultarPasoProduccionEnProximoBinding)
            {
                OcultarPasoProduccion();
                ocultarPasoProduccionEnProximoBinding = false; // Resetea para que no oculte siempre
            }
        }

        // Estilos
        private void AplicarEstiloGrid()
        {
            //Modificación de Header
            dataGridViewTickets.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(13, 27, 42);
            dataGridViewTickets.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dataGridViewTickets.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            dataGridViewTickets.EnableHeadersVisualStyles = false;  // Necesario para que tome el color personalizado
            dataGridViewTickets.ColumnHeadersHeight = 40;
            dataGridViewTickets.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;


            //Tamaños
            dataGridViewTickets.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            dataGridViewTickets.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dataGridViewTickets.DefaultCellStyle.WrapMode = DataGridViewTriState.False;

            //Visibilidad
            dataGridViewTickets.RowHeadersVisible = false;
            dataGridViewTickets.AllowUserToAddRows = false;

            //Selección
            dataGridViewTickets.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            //Colores y fuentes
            dataGridViewTickets.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(144, 221, 240);
            dataGridViewTickets.DefaultCellStyle.Font = new Font("Segoe UI Variable Text", 10);
            dataGridViewTickets.DefaultCellStyle.ForeColor = Color.Black;

            //Quitar selección al perder foco
            dataGridViewTickets.ClearSelection();
            dataGridViewTickets.CellLeave += (s, e) => dataGridViewTickets.ClearSelection();


            // Ocultar la columna "En flujo"
            if (dataGridViewTickets.Columns["Flujo"] != null)
                dataGridViewTickets.Columns["Flujo"].Visible = false;

            // Ocultar la columna "Tipo"
            if (dataGridViewTickets.Columns["Tipo"] != null)
                dataGridViewTickets.Columns["Tipo"].Visible = false;

            // Ocultar la columna "Estado"
            if (dataGridViewTickets.Columns["Estado"] != null)
                dataGridViewTickets.Columns["Estado"].Visible = false;

            // Ajuste de decimales
            if (dataGridViewTickets.Columns["EsfuerzoTotal"] != null)
                dataGridViewTickets.Columns["EsfuerzoTotal"].DefaultCellStyle.Format = "N2";

            //// Color para filas con paso a producción vencido
            foreach (DataGridViewRow row in dataGridViewTickets.Rows)
            {
                if (row.Cells["FechaCertificacion"].Value != null &&
                    DateTime.TryParse(row.Cells["FechaCertificacion"].Value.ToString(), out DateTime fechaProd) &&
                    fechaProd < DateTime.Today)
                {
                    row.DefaultCellStyle.BackColor = Color.LightGoldenrodYellow;
                }
            }

            foreach (DataGridViewRow row in dataGridViewTickets.Rows)
            {
                if (row.Cells["FechaTentativaGarantia"].Value != null &&
                    DateTime.TryParse(row.Cells["FechaTentativaGarantia"].Value.ToString(), out DateTime fechaProd) &&
                    fechaProd < DateTime.Today)
                {
                    row.DefaultCellStyle.BackColor = Color.LightCoral;
                } 
            }

            // Foreach para pintar las celdas en estado CERRADO
            foreach (DataGridViewRow row in dataGridViewTickets.Rows)
            {
                if (row.Cells["Estado"].Value != null &&
                    row.Cells["Estado"].Value.ToString() == "Cerrado")
                {
                    row.DefaultCellStyle.BackColor = Color.LightGreen;
                }
            }

            // Mover EstadoCalculado después de Resumen
            if (dataGridViewTickets.Columns["EstadoCalculado"] != null &&
                dataGridViewTickets.Columns["Resumen"] != null)
            {
                int idx = dataGridViewTickets.Columns["Resumen"].DisplayIndex;
                dataGridViewTickets.Columns["EstadoCalculado"].DisplayIndex = idx + 1;
            }
        }
        #endregion

        #region Sin uso
        private void dataGridViewTickets_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void txtFiltroClave_TextChanged(object sender, EventArgs e)
        {
        }

        private void lblPagina_Click(object sender, EventArgs e)
        {
        }
        #endregion
    }
}
