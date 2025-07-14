using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModeloGarantiaTCS.Models
{
    public enum EstadoFlujo
    {
        SinCertificacion,      // no hay fecha de certificación
        EnCertificacion,       // todavía dentro de los 2 meses + estabilización
        Cerrado                // ya venció la fecha de paso a producción
    }
}

