using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModeloGarantiaTCS.Models;
using System.IO;
using CsvHelper;
using System.Globalization;

namespace ModeloGarantiaTCS.Services
{
    /// <summary>
    /// Servicio para cargar y exportar archivos CSV relacionados con los tickets.
    /// </summary>
    public class CsvService
    {
        /// <summary>
        /// Carga un archivo CSV y convierte los registros en una lista de Ticket.
        /// </summary>
        /// <param name="filePath">Ruta del archivo CSV</param>
        /// <returns>Lista de tickets</returns>
        public List<Ticket> CargarCsv(string filePath)
        {
            var tickets = new List<Ticket>();

            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var records = csv.GetRecords<dynamic>();

                foreach (var record in records)
                {
                    var dict = (IDictionary<string, object>)record;

                    var ticket = new Ticket
                    {
                        // Clave de incidencia - GPTCS-NNNNN
                        Clave = dict.ContainsKey("Clave de incidencia") ? dict["Clave de incidencia"]?.ToString() : "",

                        // Resumen de la incidencia
                        Resumen = dict.ContainsKey("Resumen") ? dict["Resumen"]?.ToString() : "",

                        // Tipo de incidencia                        
                        Tipo = dict.ContainsKey("Tipo de Incidencia") ? dict["Tipo de Incidencia"]?.ToString() : "",

                        // Complejidad de la incidencia
                        Complejidad = dict.ContainsKey("Campo personalizado (Complejidad)") ? dict["Campo personalizado (Complejidad)"]?.ToString() : "",

                        // Horas de implementación (esfuerzo total), se calculan cuando en complejidad no se especifica
                        EsfuerzoTotal = double.TryParse(
                            dict.ContainsKey("Campo personalizado (Esfuerzo Total)") ? dict["Campo personalizado (Esfuerzo Total)"]?.ToString() : "",
                            out double h) ? h : 0.0,

                        // Campo de fecha crtificación
                        FechaCertificacion = DateTime.TryParse(
                            dict.ContainsKey("Campo personalizado (Fecha Vencimiento Certificacion)") ? dict["Campo personalizado (Fecha Vencimiento Certificacion)"]?.ToString() : "",
                            out DateTime fc) ? fc : (DateTime?)null
                    };

                    // Si no hay complejidad, inferir por horas
                    if (string.IsNullOrWhiteSpace(ticket.Complejidad))
                    {
                        if (ticket.EsfuerzoTotal <= 100.0)
                            ticket.Complejidad = "Baja";
                        else if (ticket.EsfuerzoTotal <= 200.0)
                            ticket.Complejidad = "Media";
                        else
                            ticket.Complejidad = "Alta";
                    }

                    ticket.CalcularFechas();
                    tickets.Add(ticket);
                }
            }

            // Retorna solo tickets que conincidan con el Tipo Solicitud de software
            tickets = tickets.Where(t => t.Tipo.Equals("Solicitud de software", StringComparison.OrdinalIgnoreCase)).ToList();
            return tickets;
        }

        /// <summary>
        /// Exporta una lista de tickets a un archivo CSV.
        /// </summary>
        /// <param name="tickets">Lista de tickets</param>
        /// <param name="filePath">Ruta destino del archivo CSV</param>
        public void ExportarCsv(List<Ticket> tickets, string filePath)
        {
            using (var writer = new StreamWriter(filePath))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(tickets);
            }
        }
    }
}

