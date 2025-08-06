using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModeloGarantiaTCS.Models;

namespace ModeloGarantiaTCS.Utils
{
    public static class CalculadoraFechas
    {
        private static readonly DateTime fechaActual = DateTime.Now;

        public static void Procesar(Ticket t)
        {
            /*-------------------------------------------------
             * Inicio· Validar si exista fecha de certificación
             *------------------------------------------------*/
            if (!t.FechaCertificacion.HasValue)
            {
                t.EstadoCalculado = "Aún sin fecha de certificación";
                return;
            }          

            /*-------------------------------------------------
             * Paso 1 · Fechas base + Validación si tiene fecha de paso a producción
             *------------------------------------------------*/
            if (t.FechaRealPasoProduccion.HasValue)
            {
                t.FechaRealPasoProduccion = t.FechaRealPasoProduccion.Value.Date;
            } else 
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

            if(t.FechaTentativaPasoProduccion == null)
            {
                t.FechaTentativaEstabilizacion =
                SumarDiasHabiles(t.FechaRealPasoProduccion.Value, diasEstabilizacion);
            } else
                t.FechaTentativaEstabilizacion =
                SumarDiasHabiles(t.FechaTentativaPasoProduccion.Value, diasEstabilizacion);


            /*-------------------------------------------------
             * Paso 3 · Semanas de garantía
             * –  ≤100 h  → 5 sem
             * – ≤200 h  → 8 sem
             * –  >200 h  → 16 sem
             * Si la fecha de certificación es superior a la fecha actual y no se encuentra en estado Certificado o no existe
             * fecha de paso a producción esta garantía se deberá aplicar
             *------------------------------------------------*/
            int semanasGarantia = t.EsfuerzoTotal <= 100 ? 5
                                    : t.EsfuerzoTotal <= 200 ? 8
                                    : 16;
            if (t.FechaCertificacion < fechaActual && t.FechaRealPasoProduccion.HasValue == false)
            {
                t.FechaTentativaGarantia = t.FechaCertificacion?.AddDays(semanasGarantia * 7);
            }
            else
            {
                t.FechaTentativaGarantia = t.FechaTentativaEstabilizacion?.AddDays(semanasGarantia * 7);
            }

            /*-------------------------------------------------
             * Paso 4 · Estado calculado - Se agregan comentarios para colocarlos en JIRA
             *------------------------------------------------*/
            t.EstadoCalculado = DateTime.Today > t.FechaTentativaPasoProduccion
                ? "Tiempo de certificación superado se pasa a garantía"
                : "En proceso de certificación / esperando paso a producción";

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
                ? "Tiempo de certificación superado se pasa a garantía"
                : "En proceso de certificación / esperando paso a producción";
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

