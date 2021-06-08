using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;

namespace GamesAPI
{
    public class Startup
    {
        private static readonly List<Game> Games = new()
        {
            new(1, "Settlers", 70, 1993),
            new(2, "Dune II", 80, 1993),
            new(3, "Silent Hill 2", 87, 2001),
            new(4, "Resident Evil", 73, 1996),

        };


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.Map("/games", async context =>
                {
                    await context.Response.WriteAsJsonAsync(Games);
                });

                endpoints.Map("/game/{id}", async context =>
                {
                    var id = int.Parse(context.GetRouteValue("id").ToString());
                    var game = Games.FirstOrDefault(g => g.Id == id);
                    await context.Response.WriteAsJsonAsync(game);
                });
            });
        }


        public void ConfigureServices(IServiceCollection services)
        {
        }
    }
}
