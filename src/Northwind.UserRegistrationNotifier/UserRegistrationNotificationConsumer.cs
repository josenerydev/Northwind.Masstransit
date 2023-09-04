using MassTransit;

using Northwind.Contracts;

namespace Northwind.UserRegistrationNotifier
{
    public class UserRegistrationNotificationConsumer : IConsumer<UserRegistered>
    {
        public async Task Consume(ConsumeContext<UserRegistered> context)
        {
            var userId = context.Message.UserId;
            var username = context.Message.Username;
            var email = context.Message.Email;

            Console.WriteLine($"Notificando administradores sobre o novo registro do usuário: {username} (ID: {userId}) com o email: {email}");

            SendAdminNotification(username, email);

            Console.WriteLine($"Administração notificada sobre o registro do usuário: {username}");
        }

        private void SendAdminNotification(string username, string email)
        {
            Console.WriteLine($"[Notificação fictícia] Admin informado sobre o registro de {username} com o email {email}");
        }
    }
}