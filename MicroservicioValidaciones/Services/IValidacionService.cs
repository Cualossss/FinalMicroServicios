using MicroservicioValidaciones.DTOs;

namespace MicroservicioValidaciones.Services
{
    public interface IValidacionService
    {
        Task<string> ValidarSolicitudAsync(ValidacionRequest request);
    }
}
