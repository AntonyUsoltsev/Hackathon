using Microsoft.EntityFrameworkCore;

namespace HrDirector.DataBase;

public static class MigrationExtensions
{
    public static void ApplyMigrations(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();

        using var dbContext = scope.ServiceProvider.GetRequiredService<HackathonContext>();

        dbContext.Database.Migrate();
    }
}