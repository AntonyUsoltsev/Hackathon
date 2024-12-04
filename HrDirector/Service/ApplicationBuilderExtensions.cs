namespace HrDirector.Service;

public static class ApplicationBuilderExtensions
{
    public static async Task UseStartupNotifier(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var notifierService = scope.ServiceProvider.GetRequiredService<StartupNotifierService>();
        await notifierService.StartAsync(CancellationToken.None);
    }
}