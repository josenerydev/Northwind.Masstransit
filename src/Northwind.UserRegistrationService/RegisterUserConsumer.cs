using MassTransit;

using Northwind.Contracts;

namespace Northwind.UserRegistrationService
{
    public class RegisterUserConsumer : IConsumer<RegisterUser>
    {
        public async Task Consume(ConsumeContext<RegisterUser> context)
        {
            Console.WriteLine($"Recebido registro para o usuário: {context.Message.Username}");

            await context.Publish<UserRegistered>(new
            {
                context.Message.UserId,
                context.Message.Username
            });
        }
    }
}