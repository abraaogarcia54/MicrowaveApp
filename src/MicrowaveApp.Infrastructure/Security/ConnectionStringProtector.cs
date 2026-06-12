namespace MicrowaveApp.Infrastructure.Security;

public sealed class ConnectionStringProtector
{
    public string Protect(string connectionString)
    {
        return connectionString;
    }

    public string Unprotect(string protectedConnectionString)
    {
        return protectedConnectionString;
    }
}
