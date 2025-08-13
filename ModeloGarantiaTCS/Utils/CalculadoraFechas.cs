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
            }
            else
            {
                int diaCertificacionInt = (int)t.FechaCertificacion.Value.Day;
                t.FechaTentativaPasoProduccion = t.FechaCertificacion.Value.AddMonths(2);
                t.FechaTentativaPasoProduccion = AjustarAFechaHabil(t.FechaTentativaPasoProduccion.Value);
            }
            /*-------------------------------------------------
             * Paso 2 · Días hábiles de estabilización
             * –  ≤100 h  →  5 d/hábiles
             * – ≤200 h  → 10 d/hábiles
             * –  >200 h  → 15 d/hábiles
             *------------------------------------------------*/
            int diasEstabilizacion = t.EsfuerzoTotal <= 100 ? 5
                                   : t.EsfuerzoTotal <= 200 ? 10
                                   : 15;

            if (t.FechaTentativaPasoProduccion == null)
            {
                t.FechaTentativaEstabilizacion =
                SumarDiasHabiles(t.FechaRealPasoProduccion.Value, diasEstabilizacion);
            }
            else
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
            if (DateTime.Today > t.FechaTentativaGarantia)
            {
                t.Flujo = EstadoFlujo.Cerrado;
                t.EstadoCalculado = "Tiempo de garantía superado";
            }
            else if (DateTime.Today > t.FechaCertificacion && !t.FechaRealPasoProduccion.HasValue)
            {
                t.Flujo = EstadoFlujo.Cerrado;
                t.EstadoCalculado = "Tiempo de certificación superado, está en garantía";
            }
            else
            {
                t.Flujo = EstadoFlujo.EnCertificacion;
                t.EstadoCalculado = "En proceso de certificación / esperando paso a producción";
            }
        }


        /*-----------------------------------------------------
         *  Utilidades - Validación de días hábiles / Fecha festivas / Semana Santa 
         *----------------------------------------------------*/
        private static DateTime AjustarAFechaHabil(DateTime fecha)
        {
            while (fecha.DayOfWeek == DayOfWeek.Saturday ||
                   fecha.DayOfWeek == DayOfWeek.Sunday ||
                   EsFestivo(fecha))
            {
                fecha = fecha.AddDays(1);
            }

            return fecha;
        }

        private static DateTime SumarDiasHabiles(DateTime inicio, int dias)
        {
            var fecha = inicio;
            int sumados = 0;

            while (sumados < dias)
            {

                if (fecha.DayOfWeek != DayOfWeek.Saturday &&
                    fecha.DayOfWeek != DayOfWeek.Sunday &&
                    !EsFestivo(fecha))
                {
                    sumados++;
                }
                fecha = fecha.AddDays(1);
                    
            }
            return fecha;
        }

        private static bool EsFestivo(DateTime fecha)
        {
            int year = fecha.Year;

            var festivosFijos = new List<DateTime>
            {
                new DateTime(year, 1, 1),
                new DateTime(year, 5, 1),
                new DateTime(year, 7, 20),
                new DateTime(year, 8, 7),
                new DateTime(year, 12, 8),
                new DateTime(year, 12, 25),
            };

            var festivosTrasladables = new List<DateTime>
            {
                new DateTime(year, 1, 6),
                new DateTime(year, 3, 19),
                new DateTime(year, 6, 29),
                new DateTime(year, 8, 15),
                new DateTime(year, 10, 12),
                new DateTime(year, 11, 1),
                new DateTime(year, 11, 11),
            };

            if (festivosFijos.Any(f => f.Date == fecha.Date))
                return true;

            foreach (var festivo in festivosTrasladables)
            {
                if (festivo.DayOfWeek != DayOfWeek.Monday)
                {
                    int diasParaLunes = ((int)DayOfWeek.Monday - (int)festivo.DayOfWeek + 7) % 7;
                    var traslado = festivo.AddDays(diasParaLunes);
                    if (fecha.Date == traslado.Date)
                        return true;
                }
                else
                {
                    if (fecha.Date == festivo.Date)
                        return true;
                }
            }

            var domingoPascua = CalcularPascua(year);            
            var juevesSanto = domingoPascua.AddDays(-3);
            var viernesSanto = domingoPascua.AddDays(-2);


            if (fecha.Date == domingoPascua.Date ||
                fecha.Date == juevesSanto.Date ||
                fecha.Date == viernesSanto.Date)
                return true;

            return false;
        }

        //Algoritmo para el calendario lunar (Semana santa)
        private static DateTime CalcularPascua(int year)
        {
            int day = 0;
            int month = 0;

            int g = year % 19;
            int c = year / 100;
            int h = (c - (int)(c / 4) - (int)((8 * c + 13) / 25) + 19 * g + 15) % 30;
            int i = h - (int)(h / 28) * (1 - (int)(h / 28) * (int)(29 / (h + 1)) * (int)((21 - g) / 11));

            day = i - ((year + (int)(year / 4) + i + 2 - c + (int)(c / 4)) % 7) + 28;
            month = 3;

            if (day > 31)
            {
                month++;
                day -= 31;
            }

            return new DateTime(year, month, day);
        }
    }
}

