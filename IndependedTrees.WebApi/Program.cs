using AutoMapper.Extensions.ExpressionMapping;
using IndependedTrees.BLL.Configuration;
using IndependedTrees.BLL.Services.Journal;
using IndependedTrees.BLL.Services.Trees;
using IndependedTrees.DAL;
using IndependedTrees.DAL.Repository;
using IndependedTrees.WebApi.Configuration;
using IndependedTrees.WebApi.ExceptionsHandler;
using Microsoft.EntityFrameworkCore;

namespace IndependedTrees.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            ConfigureAppDb(builder.Services, builder.Configuration);
            ConfigureBusinessServices(builder.Services, builder.Configuration);
            ConfigureAppServices(builder.Services, builder.Configuration);

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }
            app.UseExceptionHandler(o => { }); //https://github.com/dotnet/aspnetcore/issues/51888
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }

        private static void ConfigureAppDb(IServiceCollection services, IConfiguration configuration)
        {
            string? connection = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<IndependedTreesDbContext>(options => options.UseSqlServer(connection));
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        }

        private static void ConfigureBusinessServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IExceptionsJournalService, ExceptionsJournalService>();
            services.AddScoped<IIndependedTreesService, IndependedTreesService>();
        }

        private static void ConfigureAppServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers();
            services.AddOpenApi();
            services.AddAutoMapper(cfg => cfg.AddExpressionMapping(), typeof(BLLMappingProfile), typeof(MappingProfile));
            services.AddExceptionHandler<GlobalExceptionsHandler>();
        }
    }
}
