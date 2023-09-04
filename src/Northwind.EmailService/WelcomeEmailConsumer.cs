using MassTransit;

using Northwind.Contracts;

namespace Northwind.EmailService
{
    public class WelcomeEmailConsumer : IConsumer<SendWelcomeEmail>
    {
        public async Task Consume(ConsumeContext<SendWelcomeEmail> context)
        {
            Console.WriteLine($"Enviando email de boas-vindas para o usuário: {context.Message.UserId} com email: {context.Message.Email}");

            // ... Logic to send the email ...
        }
    }
}