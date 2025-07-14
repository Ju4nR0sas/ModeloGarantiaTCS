using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using ModeloGarantiaTCS.Models;
using ModeloGarantiaTCS.Utils;

namespace ModeloGarantiaTCS.Services
{
    public class CsvService
    {
        public List<Ticket> CargarCsv(string filePath)
        {
            var tickets = new List<Ticket>();

            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var records = csv.GetRecords<dynamic>();

                foreach (var record in records)
                {
                    var fila = (IDictionary<string, object>)record;

                    string tipo = fila.ContainsKey("Tipo de Incidencia") ? fila["Tipo de Incidencia"]?.ToString().Trim() ?? "" : "";

                    // Calcular esfuerzo según el tipo de incidencia
                    double horas;

                    if (tipo.Equals("Control de cambios", StringComparison.OrdinalIgnoreCase))
                    {
                        horas = LeerDouble(fila,
                            "Esfuerzo estimado total horas",
                            "Campo personalizado (Esfuerzo estimado total horas)");
                    }
                    else // Solicitud de software y otros
                    {
                        horas = LeerDouble(fila,
                            "Esfuerzo estimado total horas",
                            "Campo personalizado (Esfuerzo estimado total horas)",
                            "Campo personalizado (Esfuerzo Total)",
                            "Esfuerzo Total");
                    }

                    // Ignorar esfuerzos no válidos
                    if (horas <= 0)
                        continue;

                    var ticket = new Ticket
                    {
                        Clave = LeerTexto(fila, "Clave de incidencia"),
                        Resumen = LeerTexto(fila, "Resumen"),
                        Tipo = tipo,
                        EsfuerzoTotal = horas,
                        FechaCertificacion = LeerFecha(fila,
                            "Campo personalizado (Fecha Vencimiento Certificacion)",
                            "Fecha de certificación",
                            "Fecha Certificación")
                    };

                    CalculadoraFechas.Procesar(ticket);
                    tickets.Add(ticket);
                }
            }

            // Devolver solo tipos válidos
            return tickets
                .Where(t =>
                    t.Tipo.Equals("Solicitud de software", StringComparison.OrdinalIgnoreCase) ||
                    t.Tipo.Equals("Control de cambios", StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        public void ExportarCsv(List<Ticket> tickets, string filePath)
        {
            using (var writer = new StreamWriter(filePath))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(tickets);
            }
        }

        // Métodos auxiliares de parsing
        private static string LeerTexto(IDictionary<string, object> fila, params string[] posiblesClaves)
        {
            foreach (var clave in posiblesClaves)
            {
                if (fila.ContainsKey(clave))
                    return fila[clave]?.ToString()?.Trim() ?? "";
            }
            return "";
        }

        private static DateTime? LeerFecha(IDictionary<string, object> fila, params string[] posiblesClaves)
        {
            foreach (var clave in posiblesClaves)
            {
                if (fila.ContainsKey(clave))
                {
                    string valor = fila[clave]?.ToString()?.Trim();
                    if (DateTime.TryParse(valor, out DateTime resultado))
                        return resultado;
                }
            }
            return null;
        }

        private static double LeerDouble(IDictionary<string, object> fila, params string[] posiblesClaves)
        {
            foreach (var clave in posiblesClaves)
            {
                if (fila.ContainsKey(clave))
                {
                    string valor = fila[clave]?.ToString()?.Trim();
                    if (string.IsNullOrWhiteSpace(valor)) continue;

                    // Reemplaza separadores y prueba parseo
                    valor = valor.Replace(",", "."); // uniformiza el separador decimal

                    if (double.TryParse(valor, NumberStyles.Any, CultureInfo.InvariantCulture, out double resultado))
                        return resultado;
                }
            }
            return 0.0;
        }
    }
}
