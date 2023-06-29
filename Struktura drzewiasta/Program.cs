using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Struktura_drzewiasta.Context;

namespace Struktura_drzewiasta
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            // Ustawiamy nazwê bazy danych
            optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=TreeNodes;Trusted_Connection=True;");
            using (var context = new ApplicationDbContext(optionsBuilder.Options))
            {
                ApplicationDbContextInitializer.Initialize(context);
            }

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
