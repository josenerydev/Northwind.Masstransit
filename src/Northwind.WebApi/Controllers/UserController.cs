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

        /// <summary>
        /// Endpoint para registrar um usuário utilizando o ISendEndpointProvider.
        ///
        /// Quando uma mensagem é enviada no MassTransit, ela é direcionada para um endpoint específico usando um "DestinationAddress".
        /// Nesse contexto, uma mensagem enviada é descrita como um "Command". Comandos no MassTransit representam uma instrução
        /// específica para um serviço realizar algo e geralmente devem ser consumidos por um único consumidor. Ao manter a relação
        /// um-para-um de um comando para um consumidor, os comandos podem ser publicados e serão automaticamente roteados para o consumidor.
        /// Nomes de comandos devem ser expressos em uma sequência verbo-substantivo, seguindo o estilo "tell". Por exemplo: "RegisterUser".
        ///
        /// Neste método, estamos enviando o comando "RegisterUser" para a fila "register-user-queue" usando o ISendEndpointProvider.
        ///
        /// Adicionalmente, usando MassTransit com RabbitMQ, ao obter um send endpoint para uma fila, a chamada cria uma exchange
        /// com o mesmo nome da fila, caso não exista.
        /// Aqui, estamos empregando o ISendEndpointProvider, que é uma interface do MassTransit,
        /// para direcionar e enviar mensagens a um endpoint específico (neste caso, uma fila).
        ///
        /// Quando utilizamos o RabbitMQ com o MassTransit e tentamos enviar uma mensagem para uma fila específica,
        /// se a fila e a exchange correspondente não existirem previamente, o MassTransit se encarregará de criá-las.
        ///
        /// Mais especificamente, o MassTransit configura uma exchange do tipo 'fanout' com o mesmo nome da fila.
        /// A exchange 'fanout' é projetada para transmitir todas as mensagens que recebe para todas as filas às quais
        /// está ligada. Portanto, ao enviar uma mensagem para "register-user-queue" através do ISendEndpointProvider,
        /// a mensagem primeiro chega à exchange "register-user-queue", que, por sua vez, encaminha a mensagem para
        /// a fila "register-user-queue".
        ///
        /// Esta configuração simplifica o processo de roteamento de mensagens no RabbitMQ, garantindo que a mensagem
        /// chegue ao destino pretendido sem a necessidade de complexas configurações de roteamento.
        /// Usando o ISendEndpointProvider para enviar uma mensagem diretamente para uma fila específica.
        /// O MassTransit criará uma exchange fanout com o nome da fila (neste caso, "register-user-queue").
        /// Essa exchange será usada para rotear a mensagem para a fila desejada.
        /// </summary>
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

        /// <summary>
        /// Endpoint para registrar um usuário utilizando o IBusControl.
        ///
        /// Assim como o método acima, estamos enviando um "Command" para registrar um usuário.
        /// No entanto, neste método, estamos utilizando o IBusControl para obter o endpoint e enviar a mensagem.
        ///
        /// Adicionalmente, usando MassTransit com RabbitMQ, ao obter um send endpoint para uma fila, a chamada cria uma exchange
        /// com o mesmo nome da fila, caso não exista.
        /// Usando o IBusControl para enviar uma mensagem. Assim como o ISendEndpointProvider,
        /// se você especificar uma fila diretamente, o MassTransit criará uma exchange com o nome da fila.
        /// Esta é uma maneira de garantir que a mensagem seja roteada para a fila especificada.
        /// </summary>
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