using Application.Parameters;

namespace Application.Features.Clientes.Queries
{
    public class GetAllClientesParameters : RequestParameter
    {
        public string? Nombre { get; set; }
        public string? Apellido { get; set; }
    }
}
