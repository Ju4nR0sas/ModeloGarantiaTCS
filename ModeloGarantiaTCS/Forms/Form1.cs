using ModeloGarantiaTCS.Models;
using ModeloGarantiaTCS.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
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
        private static readonly DateTime fechaActual = DateTime.Now;

        /// <summary>
        /// Devuelve true si existe al menos un ticket cargado; de lo contrario
        /// muestra un mensaje y devuelve false.
        /// </summary>
        private bool HayTicketsCargados()
        {
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

        public Form1()
        {
            InitializeComponent();

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

        private void Form1_Load(object sender, EventArgs e)
        {
            btnFilter.Image = Image.FromFile("C:/Programacion/entregable/ModeloGarantiaTCS/ModeloGarantiaTCS/Resources/sort.png");
            btnFilter.ImageAlign = ContentAlignment.MiddleCenter;
            btnFilter.TextImageRelation = TextImageRelation.ImageBeforeText;
        }

        private void btnFilter_Click(object sender, EventArgs e)
        {
            btnFilter.AutoSize = true;

            contextMenuStrip1.Show(btnFilter, new Point(0, btnFilter.Height));

            contextMenuStrip1.Items[0].Click += (s, EventArgs) => SinCertificacion();
            contextMenuStrip1.Items[1].Click += (s, EventArgs) => EnGarantia();
            ((ToolStripMenuItem)contextMenuStrip1.Items[2]).DropDownItems[0].Click += (s, EventArgs) => ConCertificacion();





        }

        //Certificación Vencida
        private void EnGarantia()
        {
            if (!HayTicketsCargados()) return;

            var filtrados = _tickets
                .Where(t => t.FechaCertificacion < fechaActual && t.FechaRealPasoProduccion.HasValue == false)
                .ToList();

            dataGridViewTickets.DataSource = filtrados;
            AplicarEstiloGrid();
            ActivarFiltroVista("Filtrado: cerrados");
        }
        //Fechas > Fechas Tentativas
        public void ConCertificacion()
        {
            if (!HayTicketsCargados()) return;

            var filtrados = _tickets
                .Where(t => t.FechaTentativaPasoProduccion.HasValue)
                .ToList();

            dataGridViewTickets.DataSource = filtrados;
            AplicarEstiloGrid();
            ActivarFiltroVista("Filtrado: certificación");
        }

        //Sin Certificación
        public void SinCertificacion()
        {
            if (!HayTicketsCargados()) return;

            var filtrados = _tickets
                .Where(t => t.FechaCertificacion == null)
                .ToList();

            dataGridViewTickets.DataSource = filtrados;
            AplicarEstiloGrid();
            ActivarFiltroVista("Filtrado: Sin Certificación");
        }

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

        private void btnBorrarFiltro_Click(object sender, EventArgs e)
        {
            txtFiltroClave.Text = "";
            DesactivarFiltroVista();
            MostrarTickets();          
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
                MessageBox.Show("Por favor ingrese solo números en el filtro de clave.", "Filtro inválido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var filtrados = _tickets
                .Where(t => t.Clave != null && t.Clave.IndexOf(clave, StringComparison.OrdinalIgnoreCase) >= 0)
                .ToList();

            if (filtrados.Count == 0)
            {
                MessageBox.Show("No se encontraron registros con ese filtro.", "Sin resultados", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            dataGridViewTickets.DataSource = null;
            dataGridViewTickets.DataSource = filtrados;
            AplicarEstiloGrid();
        }

        private void txtFiltroClave_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Permitir solo números y teclas de control
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true; // Bloquea el carácter
            }
        }

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

        private void AplicarEstiloGrid()
        {
            dataGridViewTickets.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            dataGridViewTickets.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dataGridViewTickets.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dataGridViewTickets.RowHeadersVisible = false;
            dataGridViewTickets.AllowUserToAddRows = false;
            dataGridViewTickets.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            // Ocultar la columna "En flujo"
            if (dataGridViewTickets.Columns["Flujo"] != null)
                dataGridViewTickets.Columns["Flujo"].Visible = false;

            // Ajuste de decimales
            if (dataGridViewTickets.Columns["EsfuerzoTotal"] != null)
                dataGridViewTickets.Columns["EsfuerzoTotal"].DefaultCellStyle.Format = "N2";

            // Filas alternas
            dataGridViewTickets.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGray;

            // Color para filas con paso a producción vencido
            foreach (DataGridViewRow row in dataGridViewTickets.Rows)
            {
                if (row.Cells["FechaTentativaPasoProduccion"].Value != null &&
                    DateTime.TryParse(row.Cells["FechaTentativaPasoProduccion"].Value.ToString(), out DateTime fechaProd) &&
                    fechaProd < DateTime.Today)
                {
                    row.DefaultCellStyle.BackColor = Color.LightCoral;
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


        private void dataGridViewTickets_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void txtFiltroClave_TextChanged(object sender, EventArgs e)
        {
        }

        private void lblPagina_Click(object sender, EventArgs e)
        {
        }

        /// Deshabilita paginación y marca la vista como filtrada.
        private void ActivarFiltroVista(string descripcion)
        {
            _filtroActivo = true;
            btnAnterior.Enabled = false;
            btnSiguiente.Enabled = false;
            lblPagina.Text = descripcion;
        }

        /// Vuelve al modo de paginación normal.
        private void DesactivarFiltroVista()
        {
            _filtroActivo = false;
            btnAnterior.Enabled = true;
            btnSiguiente.Enabled = true;
        }
    }
}
