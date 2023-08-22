namespace Guestline.Games.Battleships.Client
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllers();

            var app = builder.Build();
            app.MapControllerRoute(name: "default",pattern: "{controller=Home}/{action=Index}");
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.Run();
        }
    }
}