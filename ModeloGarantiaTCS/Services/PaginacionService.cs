using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModeloGarantiaTCS.Services
{
    public class PaginacionService<T>
    {
        private readonly List<T> _items;
        private readonly int _porPagina;

        public PaginacionService(List<T> items, int porPagina)
        {
            _items = items ?? new List<T>();
            _porPagina = Math.Max(1, porPagina);
        }

        public List<T> ObtenerPagina(int numeroPagina)
        {
            return _items
                .Skip((numeroPagina - 1) * _porPagina)
                .Take(_porPagina)
                .ToList();
        }

        public int TotalPaginas => (int)Math.Ceiling((double)_items.Count / _porPagina);
    }
}