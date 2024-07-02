using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Net;

namespace ServerWeb
{
    public class WebServer
    {

        public static void Main(string[]? args) { }

        public async static void Start(string[]? args)
        {

            var host = Host.CreateDefaultBuilder(args)
        .ConfigureWebHostDefaults(webBuilder =>
        {
            webBuilder.ConfigureServices(services =>
            {
                //services.AddGrpc();

                services.AddControllers();
                // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
                services.AddEndpointsApiExplorer();
                services.AddSwaggerGen();

            });
            webBuilder.ConfigureKestrel(serverOptions =>
            {

                serverOptions.ListenAnyIP(5050); // Ascolta sulla porta 5000
                serverOptions.ListenAnyIP(5051, listenOptions =>
                {
                    listenOptions.UseHttps(); // Ascolta sulla porta 5001 con HTTPS
                });

                //options.Listen(IPAddress.Loopback, 5167);
                //options.Listen(new IPAddress([192,168,1,2]), 5000);
                //options.Listen(IPAddress.Loopback, 5055, configure => configure.UseHttps());
            });
            webBuilder.Configure(app =>
            {

                app.UseRouting();


                //if (app.Environment.IsDevelopment())
                //{

                //http://localhost:5050/swagger/index.html
                app.UseSwagger();
                    app.UseSwaggerUI();
                //}

                //app.UseHttpsRedirection();

                app.UseStaticFiles();

                app.UseAuthorization();

                //app.MapControllers();
                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                });

                //app.UseEndpoints(endpoints =>
                //{
                //    endpoints.MapGrpcService<GreeterService>();

                //    endpoints.MapGet("/", async context =>
                //    {
                //        await context.Response.WriteAsync("Communication with gRPC endpoints must be made through a gRPC client.");
                //    });
                //});
            });
        })
        .Build();

            await host.RunAsync();


        }

    }
}
