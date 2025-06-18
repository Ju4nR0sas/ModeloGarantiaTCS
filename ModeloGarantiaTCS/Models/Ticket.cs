using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModeloGarantiaTCS.Models
{
    /// <summary>
    /// Representa un ticket de Jira con los campos necesarios y cálculos de garantía.
    /// </summary>
    public class Ticket
    {
        // Datos básicos
        public string Clave { get; set; }
        public string Resumen { get; set; }
        public string Tipo { get; set; }
        public string Complejidad { get; set; }
        public int HorasImplementacion { get; set; }
        public DateTime? FechaCertificacion { get; set; }

        // Fechas calculadas
        public DateTime? FechaTentativaPasoProduccion { get; set; }
        public DateTime? FechaTentativaEstabilizacion { get; set; }
        public DateTime? FechaTentativaGarantia { get; set; }

        // Estado calculado según lógica de garantía
        public string EstadoCalculado { get; set; }
        /// <summary>
        /// Realiza los cálculos de fechas tentativas y estado de garantía basado en las reglas del modelo.
        /// </summary>
        /// 
        private DateTime SumarDiasHabiles(DateTime inicio, int dias)
        {
            int sumados = 0;
            var fecha = inicio;
            while (sumados < dias)
            {
                fecha = fecha.AddDays(1);
                if (fecha.DayOfWeek != DayOfWeek.Saturday && fecha.DayOfWeek != DayOfWeek.Sunday)
                {
                    sumados++;
                }
            }
            return fecha;
        }

        public void CalcularFechas()
        {
            if (FechaCertificacion.HasValue)
            {
                // Paso a producción tentativo: 2 meses tras certificación
                FechaTentativaPasoProduccion = FechaCertificacion.Value.AddMonths(2);

                // Estabilización: calcula días hábiles según complejidad
                int diasEstabilizacion = 0;
                switch (Complejidad?.ToLower())
                {
                    case "baja":
                        diasEstabilizacion = 5;
                        break;
                    case "media":
                        diasEstabilizacion = 10;
                        break;
                    case "alta":
                        diasEstabilizacion = 15;
                        break;
                }
                FechaTentativaEstabilizacion = SumarDiasHabiles(FechaTentativaPasoProduccion.Value, diasEstabilizacion);

                // Garantía: calcula en semanas calendario
                int semanasGarantia = 0;
                if (HorasImplementacion <= 100)
                    semanasGarantia = 5;
                else if (HorasImplementacion <= 200)
                    semanasGarantia = 8;
                else
                    semanasGarantia = 16;

                FechaTentativaGarantia = FechaTentativaEstabilizacion?.AddDays(semanasGarantia * 7);

                // Estado calculado
                if (DateTime.Today > FechaTentativaPasoProduccion)
                {
                    EstadoCalculado = "En garantía directa (no pasó a producción a tiempo)";
                }
                else
                {
                    EstadoCalculado = "En certificación / esperando producción";
                }
            }
            else
            {
                EstadoCalculado = "Sin certificación";
            }
        }
    }
}

