using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using ModeloGarantiaTCS.Models;

namespace ModeloGarantiaTCS.Services
{
    public class CsvService
    {
        /* ----------------------------------------------------------------
         * 1.  Utilidades de lectura
         * ----------------------------------------------------------------*/
        private static double LeerDouble(IDictionary<string, object> dict, params string[] claves)
        {
            foreach (var clave in claves)
            {
                object raw;
                if (dict.TryGetValue(clave, out raw))
                {
                    var s = raw?.ToString()?.Trim();
                    var val = ParseDoubleFlexible(s);
                    if (val != 0) return val;          // devuelve el primer valor > 0
                }
            }
            return 0.0;
        }

        /// Intenta convertir string → double aceptando tanto “,” como “.” como separador decimal.
        private static double ParseDoubleFlexible(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return 0;

            // Elimina espacios, normaliza comas y puntos
            s = s.Trim();

            // Si contiene ambos "," y ".", hay que deducir cuál es decimal
            if (s.Contains(',') && s.Contains('.'))
            {
                // Se asume que el separador decimal es la última aparición (más común)
                if (s.LastIndexOf(',') > s.LastIndexOf('.'))
                {
                    s = s.Replace(".", "");          // elimina miles con punto
                    s = s.Replace(',', '.');         // decimal con coma → punto
                }
                else
                {
                    s = s.Replace(",", "");          // elimina miles con coma
                                                     // decimal ya es punto
                }
            }
            else if (s.Contains('.'))
            {
                // Solo punto → asume punto decimal
                s = s.Replace(",", ""); // por si acaso hay alguna coma de miles errónea
            }
            else if (s.Contains(','))
            {
                // Solo coma → asume coma decimal
                s = s.Replace(".", ""); // elimina puntos de miles si los hay
                s = s.Replace(',', '.'); // decimal → punto
            }

            double v;
            return double.TryParse(s, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out v) ? v : 0;
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

        /* ----------------------------------------------------------------
         * 2.  Cargar CSV
         * ----------------------------------------------------------------*/
        public List<Ticket> CargarCsv(string filePath)
        {
            var tickets = new List<Ticket>();

            using (var reader = new StreamReader(filePath))
            {
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    foreach (IDictionary<string, object> fila in csv.GetRecords<dynamic>())
                    {
                        // Campos principales
                        var clave = fila.ContainsKey("Clave de incidencia") ? fila["Clave de incidencia"]?.ToString() : "";
                        var resumen = fila.ContainsKey("Resumen") ? fila["Resumen"]?.ToString() : "";
                        var tipo = fila.ContainsKey("Tipo de Incidencia") ? fila["Tipo de Incidencia"]?.ToString() : "";

                        // Horas (acepta diferentes cabeceras)
                        double horas = LeerDouble(fila,
                            "Esfuerzo estimado total horas",
                            "Campo personalizado (Esfuerzo estimado total horas)",
                            "Campo personalizado (Esfuerzo Total)",
                            "Esfuerzo Total");

                        // ► Descarta filas sin horas (>0)
                        if (horas <= 0) continue;

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
                        };

                        // Complejidad inferida por horas
                        if (horas <= 100) ticket.Complejidad = "Baja";
                        else if (horas <= 200) ticket.Complejidad = "Media";
                        else ticket.Complejidad = "Alta";

                        ticket.CalcularFechas();
                        tickets.Add(ticket);
                    }
                }
            }

            // Solo “Solicitud de software”
            return tickets
                .Where(t => t.Tipo.Equals("Solicitud de software", StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        /* ----------------------------------------------------------------
         * 3.  Exportar CSV
         * ----------------------------------------------------------------*/
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
