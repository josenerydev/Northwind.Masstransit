using MassTransit;

using Microsoft.AspNetCore.Mvc;

using Northwind.Contracts;

namespace Northwind.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ISendEndpointProvider _sendEndpointProvider;
        private readonly IBusControl _busControl;

        public UserController(ISendEndpointProvider sendEndpointProvider, IBusControl busControl)
        {
            _sendEndpointProvider = sendEndpointProvider;
            _busControl = busControl;
        }

        // Usando o ISendEndpointProvider para enviar uma mensagem diretamente para uma fila específica.
        // O MassTransit criará uma exchange fanout com o nome da fila (neste caso, "register-user-queue").
        // Essa exchange será usada para rotear a mensagem para a fila desejada.
        [HttpPost("register-endpoint")]
        public async Task<IActionResult> RegisterWithSendEndPointProvider([FromBody] RegisterUserRequest request)
        {
            var endpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri("queue:register-user-queue"));

            await endpoint.Send<RegisterUser>(new
            {
                UserId = Guid.NewGuid(),
                Username = request.Username,
                Password = request.Password
            });

            return Accepted();
        }

        // Usando o IBusControl para enviar uma mensagem. Assim como o ISendEndpointProvider, 
        // se você especificar uma fila diretamente, o MassTransit criará uma exchange com o nome da fila.
        // Esta é uma maneira de garantir que a mensagem seja roteada para a fila especificada.
        [HttpPost("register-bus")]
        public async Task<IActionResult> RegisterWithBus([FromBody] RegisterUserRequest request)
        {
            var endpoint = await _busControl.GetSendEndpoint(new Uri("queue:register-user-queue"));

            await endpoint.Send<RegisterUser>(new
            {
                UserId = Guid.NewGuid(),
                Username = request.Username,
                Password = request.Password
            });

            return Accepted();
        }
    }

    public class RegisterUserRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
