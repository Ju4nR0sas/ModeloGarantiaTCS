using System;

namespace ModeloGarantiaTCS.Models
{
    public class Ticket
    {
        // Datos básicos
        public string Clave { get; set; }
        public string Resumen { get; set; }
        public string Tipo { get; set; }
        public double EsfuerzoTotal { get; set; }
        public DateTime? FechaCertificacion { get; set; }
        public EstadoFlujo Flujo { get; set; }


        // Fechas calculadas
        public DateTime? FechaTentativaPasoProduccion { get; set; }
        public DateTime? FechaRealPasoProduccion { get; set; }
        public DateTime? FechaTentativaEstabilizacion { get; set; }
        public DateTime? FechaTentativaGarantia { get; set; }

        // Estado
        public string EstadoCalculado { get; set; }
    }
}
