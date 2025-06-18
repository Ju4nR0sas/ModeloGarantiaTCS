using ModeloGarantiaTCS.Models;
using ModeloGarantiaTCS.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ModeloGarantiaTCS
{

    public partial class Form1: Form
    {
        private List<Ticket> _tickets = new List<Ticket>();
        private CsvService _csvService = new CsvService();

        //Label paginado
        private int _paginaActual = 1;
        private int _registrosPorPagina;
        public Form1()
        {
            InitializeComponent();


        }

        // MostrarTickets: Resetea a la primera página y muestra
        private void MostrarTickets()
        {
            _paginaActual = 1;
            _registrosPorPagina = Math.Max(1, dataGridViewTickets.DisplayRectangle.Height / dataGridViewTickets.RowTemplate.Height);
            MostrarPagina();
        }

        // button1_Click: Cargar CSV desde un archivo
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

        private void dataGridViewTickets_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        // btnExportarCsv_Click: Exportar los tickets cargados a un nuevo CSV
        private void btnExportarCsv_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "CSV files (*.csv)|*.csv";
                sfd.FileName = "TicketsExportados.csv";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    var csvService = new CsvService();
                    csvService.ExportarCsv(_tickets, sfd.FileName);
                    MessageBox.Show("Archivo exportado correctamente", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void txtFiltroClave_TextChanged(object sender, EventArgs e)
        {

        }

        // btnFiltrar_Click: Filtrar por clave y mostrar en el grid
        private void btnFiltrar_Click(object sender, EventArgs e)
        {
            string clave = txtFiltroClave.Text.Trim();

            if (string.IsNullOrEmpty(clave))
            {
                MostrarTickets(); // Muestra todo  
                return;
            }

            var filtrados = _tickets.FindAll(t => t.Clave != null && t.Clave.IndexOf(clave, StringComparison.OrdinalIgnoreCase) >= 0);
            dataGridViewTickets.DataSource = null;
            dataGridViewTickets.DataSource = filtrados;
        }

        // MostrarPagina: Muestra la página actual de tickets con paginación
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

        // PaginaMaxima: Calcula el número máximo de páginas
        private int PaginaMaxima()
        {
            return (int)Math.Ceiling((double)_tickets.Count / _registrosPorPagina);
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

        private void lblPagina_Click(object sender, EventArgs e)
        {

        }


        // AplicarEstiloGrid: Aplica estilos visuales al grid (formatos, colores)
        private void AplicarEstiloGrid()
        {
            // Ajuste de columnas
            dataGridViewTickets.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            dataGridViewTickets.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dataGridViewTickets.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dataGridViewTickets.RowHeadersVisible = false;

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

            // Ajustes generales
            dataGridViewTickets.AllowUserToAddRows = false;
            dataGridViewTickets.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }
    }
}
