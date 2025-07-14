using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModeloGarantiaTCS.Models;

namespace ModeloGarantiaTCS.Utils
{
    /// <summary>
    /// Calcula fechas tentativas y estado de un ticket
    /// a partir de su fecha de certificación y horas de trabajo.
    /// No depende de la propiedad textual "Complejidad".
    /// </summary>
    public static class CalculadoraFechas
    {
        /// <summary>Aplica reglas de negocio sobre el ticket recibido.</summary>
        public static void Procesar(Ticket t)
        {
            if (!t.FechaCertificacion.HasValue)
            {
                t.EstadoCalculado = "Sin certificación";
                return;
            }

            /*-------------------------------------------------
             * Paso 1 · Fechas base
             *------------------------------------------------*/
            t.FechaTentativaPasoProduccion = t.FechaCertificacion.Value.AddMonths(2);

            /*-------------------------------------------------
             * Paso 2 · Días hábiles de estabilización
             * –  ≤100 h  →  5 d/hábiles
             * – ≤200 h  → 10 d/hábiles
             * –  >200 h  → 15 d/hábiles
             *------------------------------------------------*/
            int diasEstabilizacion = t.EsfuerzoTotal <= 100 ? 5
                                   : t.EsfuerzoTotal <= 200 ? 10
                                   : 15;

            t.FechaTentativaEstabilizacion =
                SumarDiasHabiles(t.FechaTentativaPasoProduccion.Value, diasEstabilizacion);

            /*-------------------------------------------------
             * Paso 3 · Semanas de garantía
             * –  ≤100 h  → 5 sem
             * – ≤200 h  → 8 sem
             * –  >200 h  → 16 sem
             *------------------------------------------------*/
            int semanasGarantia = t.EsfuerzoTotal <= 100 ? 5
                                : t.EsfuerzoTotal <= 200 ? 8
                                : 16;

            t.FechaTentativaGarantia = t.FechaTentativaEstabilizacion?.AddDays(semanasGarantia * 7);

            /*-------------------------------------------------
             * Paso 4 · Estado calculado
             *------------------------------------------------*/
            t.EstadoCalculado = DateTime.Today > t.FechaTentativaPasoProduccion
                ? "En garantía directa (no pasó a producción a tiempo)"
                : "En certificación / esperando producción";

            if (!t.FechaCertificacion.HasValue)
            {
                t.Flujo = EstadoFlujo.SinCertificacion;
                t.EstadoCalculado = "Sin certificación";
                return;
            }

            t.Flujo = DateTime.Today > t.FechaTentativaPasoProduccion
                      ? EstadoFlujo.Cerrado
                      : EstadoFlujo.EnCertificacion;

            t.EstadoCalculado = t.Flujo == EstadoFlujo.Cerrado
                ? "En garantía directa (no pasó a producción a tiempo)"
                : "En certificación / esperando producción";

        }



        /*-----------------------------------------------------
         *  Utilidades privadas
         *----------------------------------------------------*/
        private static DateTime SumarDiasHabiles(DateTime inicio, int dias)
        {
            var fecha = inicio;
            for (int sumados = 0; sumados < dias;)
            {
                fecha = fecha.AddDays(1);
                if (fecha.DayOfWeek != DayOfWeek.Saturday &&
                    fecha.DayOfWeek != DayOfWeek.Sunday)
                {
                    sumados++;
                }
            }
            return fecha;
        }
    }
}

