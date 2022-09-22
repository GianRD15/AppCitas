using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using AppCitas.Service.Data;
using AppCitas.Service.Interfaces;
using AppCitas.Service.Services;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using AppCitas.Service.Extensions;
using AppCitas.Service.Middleware;

namespace AppCitas;

public class Startup
{
    public IConfiguration _config;
    public Startup(IConfiguration configuration)
    {
        _config = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddAplicationServices(_config);
        services.AddControllers();
        services.AddCors();
        services.AddIdentityServices(_config);
        //services.AddSwaggerGen(c =>
        //{
        //    c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebAPIv5", Version = "v1" });
        //});
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseMiddleware<ExceptionMiddleware>();

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseCors(p => p.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:4200"));

        app.UseAuthentication();

        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
