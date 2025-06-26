using System.Collections.Concurrent;
using Tiendaenlinea.Dominio.DTOs;

namespace Tiendaenlinea.Dominio.Entidades
{
    public static class CarritoMemoria
    {
        public static ConcurrentDictionary<int, List<AgregarAlCarritoDto>> Carritos = new();
    }
}