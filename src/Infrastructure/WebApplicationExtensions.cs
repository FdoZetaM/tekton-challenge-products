namespace TektonChallengeProducts.Infrastructure;

using Persistence.Sql;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

public static class WebApplicationExtensions
{
    public static void ApplyPendingMigrations(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<EntityFrameworkUnitOfWork>();
        db.Database.Migrate();
    }
}