using MassTransit;

using Northwind.Contracts;

namespace Northwind.EmailService
{
    public class UserRegisteredConsumer : IConsumer<UserRegistered>
    {
        public async Task Consume(ConsumeContext<UserRegistered> context)
        {
            Console.WriteLine($"Recebendo mensagem UserRegistered para o usuário: {context.Message.UserId} com email: {context.Message.Email}");

            await context.Send<SendWelcomeEmail>(new Uri("queue:welcome-email-queue"), new
            {
                UserId = context.Message.UserId,
                Email = context.Message.Email
            });

            Console.WriteLine($"Mensagem SendWelcomeEmail enviada para o usuário: {context.Message.UserId}");
        }
    }
}