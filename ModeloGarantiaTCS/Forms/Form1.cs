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
        }

        private void MostrarTickets()
        {
            _paginaActual = 1;
            _registrosPorPagina = Math.Max(1, dataGridViewTickets.DisplayRectangle.Height / dataGridViewTickets.RowTemplate.Height);
            MostrarPagina();
        }

        private void MostrarPagina()
        {
            var paged = _tickets
                .Skip((_paginaActual - 1) * _registrosPorPagina)
                .Take(_registrosPorPagina)
                .ToList();

            dataGridViewTickets.DataSource = null;
            dataGridViewTickets.DataSource = paged;
            lblPagina.Text = $"Página: {_paginaActual}";

            AplicarEstiloGrid();
        }

        private int PaginaMaxima()
        {
            return (int)Math.Ceiling((double)_tickets.Count / _registrosPorPagina);
        }

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
            if (_paginaActual > 1)
            {
                _paginaActual--;
                MostrarPagina();
            }
        }

        private void btnSiguiente_Click(object sender, EventArgs e)
        {
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

            // Formato de fechas
            if (dataGridViewTickets.Columns["FechaCertificacion"] != null)
                dataGridViewTickets.Columns["FechaCertificacion"].DefaultCellStyle.Format = "dd/MM/yyyy";
            if (dataGridViewTickets.Columns["FechaTentativaPasoProduccion"] != null)
                dataGridViewTickets.Columns["FechaTentativaPasoProduccion"].DefaultCellStyle.Format = "dd/MM/yyyy";
            if (dataGridViewTickets.Columns["FechaTentativaEstabilizacion"] != null)
                dataGridViewTickets.Columns["FechaTentativaEstabilizacion"].DefaultCellStyle.Format = "dd/MM/yyyy";
            if (dataGridViewTickets.Columns["FechaTentativaGarantia"] != null)
                dataGridViewTickets.Columns["FechaTentativaGarantia"].DefaultCellStyle.Format = "dd/MM/yyyy";

            // Filas alternas
            dataGridViewTickets.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGray;

            // Color para filas con paso a producción vencido
            foreach (DataGridViewRow row in dataGridViewTickets.Rows)
            {
                if (row.Cells["FechaTentativaPasoProduccion"].Value != null &&
                    DateTime.TryParse(row.Cells["FechaTentativaPasoProduccion"].Value.ToString(), out DateTime fechaProd))
                {
                    if (fechaProd < DateTime.Today)
                    {
                        row.DefaultCellStyle.BackColor = Color.LightCoral;
                    }
                }
            }

            // Mover EstadoCalculado después de Resumen
            if (dataGridViewTickets.Columns["EstadoCalculado"] != null &&
                dataGridViewTickets.Columns["Resumen"] != null)
            {
                int indexResumen = dataGridViewTickets.Columns["Resumen"].DisplayIndex;
                dataGridViewTickets.Columns["EstadoCalculado"].DisplayIndex = indexResumen + 1;
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

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
