using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace MicrowaveApp.Infrastructure.Data;

public sealed class MicrowaveDbContextFactory : IDesignTimeDbContextFactory<MicrowaveDbContext>
{
    public MicrowaveDbContext CreateDbContext(string[] args)
    {
        var options = new DbContextOptionsBuilder<MicrowaveDbContext>()
            .UseSqlServer("Server=localhost,1433;Database=MicrowaveApp;User Id=sa;Password=Your_password123;TrustServerCertificate=True")
            .Options;

        return new MicrowaveDbContext(options);
    }
}
