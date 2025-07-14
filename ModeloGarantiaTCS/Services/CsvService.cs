using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using ModeloGarantiaTCS.Models;

namespace ModeloGarantiaTCS.Services
{
    /// <summary>
    /// Servicio para cargar y exportar archivos CSV relacionados con los tickets.
    /// El campo “Complejidad” del CSV ya NO se usa; la complejidad se infiere solo
    /// a partir de las horas (Esfuerzo estimado total horas o Esfuerzo Total).
    /// </summary>
    public class CsvService
    {
        // ---------- Helpers privados ----------
        private static double LeerDouble(IDictionary<string, object> dict, params string[] claves)
        {
            foreach (var clave in claves)
            {
                object raw;
                if (dict.TryGetValue(clave, out raw) &&
                    double.TryParse(raw?.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out double val))
                {
                    return val;
                }
            }
            return 0.0;
        }

        private static DateTime? LeerFecha(IDictionary<string, object> dict, params string[] claves)
        {
            foreach (var clave in claves)
            {
                object raw;
                if (dict.TryGetValue(clave, out raw) &&
                    DateTime.TryParse(raw?.ToString(), out DateTime fecha))
                {
                    return fecha;
                }
            }
            return null;
        }

        // ---------- Cargar CSV ----------
        public List<Ticket> CargarCsv(string filePath)
        {
            var tickets = new List<Ticket>();

            using (var reader = new StreamReader(filePath))
            {
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    foreach (IDictionary<string, object> fila in csv.GetRecords<dynamic>())
                    {
                        var clave = fila.ContainsKey("Clave de incidencia") ? fila["Clave de incidencia"]?.ToString() : "";
                        var resumen = fila.ContainsKey("Resumen") ? fila["Resumen"]?.ToString() : "";
                        var tipo = fila.ContainsKey("Tipo de Incidencia") ? fila["Tipo de Incidencia"]?.ToString() : "";

                        // Horas: primero “Esfuerzo estimado total horas”; fallback “Esfuerzo Total”
                        double horas = LeerDouble(fila,
                            "Esfuerzo estimado total horas",
                            "Campo personalizado (Esfuerzo estimado total horas)",
                            "Campo personalizado (Esfuerzo Total)");

                        DateTime? fechaCert = LeerFecha(fila,
                            "Campo personalizado (Fecha Vencimiento Certificacion)",
                            "Fecha Vencimiento Certificacion");

                        var ticket = new Ticket
                        {
                            Clave = clave,
                            Resumen = resumen,
                            Tipo = tipo,
                            EsfuerzoTotal = horas,
                            FechaCertificacion = fechaCert
                            // COMPLEJIDAD se asigna abajo
                        };

                        // Inferir complejidad según horas
                        if (horas <= 100) ticket.Complejidad = "Baja";
                        else if (horas <= 200) ticket.Complejidad = "Media";
                        else ticket.Complejidad = "Alta";

                        ticket.CalcularFechas();
                        tickets.Add(ticket);
                    }
                }
            }

            // Devolver solo “Solicitud de software”
            return tickets
                .Where(t => t.Tipo.Equals("Solicitud de software", StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        // ---------- Exportar CSV ----------
        public void ExportarCsv(List<Ticket> tickets, string filePath)
        {
            using (var writer = new StreamWriter(filePath))
            {
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    csv.WriteRecords(tickets);
                }
            }
        }
    }
}
