using MassTransit;

using Northwind.Contracts;

namespace Northwind.UserRegistrationService
{
    // A classe RegisterUserConsumer é um consumidor de mensagens do tipo RegisterUser.
    public class RegisterUserConsumer : IConsumer<RegisterUser>
    {
        // Este método é chamado automaticamente pelo MassTransit quando uma mensagem do tipo RegisterUser é recebida.
        public async Task Consume(ConsumeContext<RegisterUser> context)
        {
            // Imprime no console o nome do usuário que está sendo registrado.
            Console.WriteLine($"Recebido registro para o usuário: {context.Message.Username}");

            // Publica um evento informando que o usuário foi registrado.
            await context.Publish<UserRegistered>(new
            {
                context.Message.UserId,
                context.Message.Username,
                context.Message.Email
            });
        }
    }
}