namespace Tiendaenlinea.Dominio.DTOs
{
    public class AgregarDireccionDto
    {
        public int UsuarioID { get; set; }
        public string Pais { get; set; } = "";
        public string Ciudad { get; set; } = "";
        public string CodigoPostal { get; set; } = "";
        public string DireccionDetalle { get; set; } = "";
    }
}