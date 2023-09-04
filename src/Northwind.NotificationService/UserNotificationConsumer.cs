using MassTransit;

using Northwind.Contracts;

namespace Northwind.NotificationService
{
    public class UserNotificationConsumer : IConsumer<UserRegistered>
    {
        public async Task Consume(ConsumeContext<UserRegistered> context)
        {
            // Por exemplo, vamos supor que você quer notificar um administrador
            // sempre que um novo usuário se registra no sistema.
            var userId = context.Message.UserId;
            var username = context.Message.Username;
            var email = context.Message.Email;

            Console.WriteLine($"Notificando administradores sobre o novo registro do usuário: {username} (ID: {userId}) com o email: {email}");

            // Aqui, você pode adicionar lógica adicional como:
            // - Enviar um email para um administrador
            // - Logar a informação em algum sistema de monitoramento
            // - Emitir outras mensagens ou eventos relacionados

            // Notificação fictícia para exemplo:
            SendAdminNotification(username, email);

            Console.WriteLine($"Administração notificada sobre o registro do usuário: {username}");
        }

        private void SendAdminNotification(string username, string email)
        {
            // Aqui, você coloca a lógica real para enviar a notificação.
            // Por exemplo, você poderia integrar com um serviço de email ou API de notificações.
            Console.WriteLine($"[Notificação fictícia] Admin informado sobre o registro de {username} com o email {email}");
        }
    }
}