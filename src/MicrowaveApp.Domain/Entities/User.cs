namespace MicrowaveApp.Domain.Entities;

public class User
{
    public int Id { get; private set; }
    public string Username { get; private set; }
    public string PasswordHash { get; private set; } // SHA-256
    public DateTime CreatedAt { get; private set; }
    

    public User(string username, string passwordHash)
    {
        if (string.IsNullOrWhiteSpace(username))
            throw new ArgumentException("Nome de usuário é obrigatório.");

        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new ArgumentException("Hash da senha é obrigatório.");

        Username = username.Trim().ToLower();
        PasswordHash = passwordHash;
        CreatedAt = DateTime.UtcNow;
    }
}