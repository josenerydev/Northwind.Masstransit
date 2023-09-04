namespace Northwind.Contracts
{
    public interface SendWelcomeEmail
    {
        Guid UserId { get; }
        string Email { get; }
    }
}