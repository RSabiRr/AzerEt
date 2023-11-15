namespace AzerEt
{
   using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

	using Microsoft.Extensions.Logging;
    using static AzerEt.SeedData;

    internal class Startup
{
    public static void Main(string[] args)
    {
            var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

                // Rol oluşturma işlemini başlatın
                RoleInitializer.InitializeAsync(roleManager).Wait();
            }

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });


}

}