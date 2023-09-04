namespace Northwind.Contracts
{
    public interface UserRegistered
    {
        Guid UserId { get; }
        string Username { get; }
    }
}