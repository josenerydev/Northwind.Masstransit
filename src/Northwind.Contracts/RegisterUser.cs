namespace Northwind.Contracts
{
    public interface RegisterUser
    {
        Guid UserId { get; }
        string Username { get; }
        string Password { get; }
    }
}