namespace HrDirector.Service;

public static class StartupNotifierExtensions
{
    public static async Task UseStartupNotifier(this IApplicationBuilder app, IServiceProvider serviceProvider)
    {
        _ = Task.Run(async () =>
        {
            using var scope = serviceProvider.CreateScope();
            var notifier = scope.ServiceProvider.GetRequiredService<StartupNotifierService>();

            try
            {
                await notifier.StartAsync(CancellationToken.None);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in StartupNotifierService: {ex.Message}");
            }
        });

        await Task.CompletedTask; 
    }
}