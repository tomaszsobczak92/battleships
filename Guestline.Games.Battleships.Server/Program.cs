using Guestline.Games.Battleships.Server.Abstractions;
using Guestline.Games.Battleships.Server.Hubs;
using Guestline.Games.Battleships.Server.Infrastructures;
using Guestline.Games.Battleships.Server.Services;

namespace Guestline.Games.Battleships.Server
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddMemoryCache();
            builder.Services.AddLogging();
            builder.Services.AddSingleton<IMemoryCacheWrapper, MemoryCacheWrapper>();
            builder.Services.AddSingleton<IRandomWrapper, RandomWrapper>();
            builder.Services.AddTransient<IShipService, ShipService>();
            builder.Services.AddTransient<IBattleshipService, BattleshipsService>();

            builder.Services.AddSignalR();

            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    builder =>
                    {
                        builder.WithOrigins("https://localhost:7185")
                            .AllowAnyHeader()
                            .WithMethods("GET", "POST")
                            .AllowCredentials();
                    });
            });

            var app = builder.Build();

            app.UseCors();
            app.MapHub<BattleshipsGameHub>("/battleshipsHub");

            await app.RunAsync();
        }
    }
}