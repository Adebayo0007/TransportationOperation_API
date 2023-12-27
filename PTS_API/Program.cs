using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;
using PTS_API.Authentication;
using PTS_API.Utils;
using PTS_BUSINESS.Services.Implementations;
using PTS_BUSINESS.Services.Interfaces;
using PTS_CORE.Domain.Entities;
using PTS_DATA.EfCore.Context;
using PTS_DATA.Repository.Implementations;
using PTS_DATA.Repository.Interfaces;

namespace PTS_API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Logging.ClearProviders();
            builder.Logging.AddConsole();

            builder.Services.AddCors(a => a.AddPolicy("CorsPolicy", b =>
            {
                //b.WithOrigins("http://localhost:5000/")
                b.AllowAnyMethod()
                .AllowAnyOrigin()
                .AllowAnyHeader();

            }));

            builder.Services.AddHttpContextAccessor();
            builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            
            builder.Services.AddDbContext<ApplicationDBContext>(options =>
               options.UseSqlServer(builder.Configuration.GetConnectionString("Database")));

            #region Identity


            builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                //password requirement
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireDigit = true;

                //locking out user after some attempt
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);

                //unique email for users
                options.User.RequireUniqueEmail = true;
            })
          .AddEntityFrameworkStores<ApplicationDBContext>()
          .AddDefaultTokenProviders();    //Adding Configuration of the identity to DI
            #endregion
            
            builder.Services.AddEndpointsApiExplorer();

            #region Swagger
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Pacesetter Transportation Project", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please Bearer and then token is the field",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey

                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement{
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }

                 });
            });
            #endregion

            builder.Services.AddHttpContextAccessor();
            builder.Services.AddScoped<DBInitializer>();  //Seeding datas in to the DB
            builder.Services.AddTransient<IAccountService, AccountService>();

            builder.Services.AddTransient<IEmployeeRepository, EmployeeRepository>();
            builder.Services.AddTransient<IEmployeeService, EmployeeService>();
            builder.Services.AddScoped<IJWTAuthentication, JWTAuthentication>();
            builder.Services.AddRouting(option => option.LowercaseUrls = true);
           /* builder.Services.AddControllers().AddJsonOptions(opt => opt.JsonSerializerOptions.PropertyNamingPolicy = null)
                .AddNewtonsoftJson(opt =>
                {
                    opt.SerializerSettings.ContractResolver = new DefaultContractResolver();
                    opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                });*/


            var app = builder.Build();

            // to seed some datas into the database
            ///////////////////////////////////////////////////////////////////////
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    // Migrate the database
                    var dbContext = services.GetRequiredService<ApplicationDBContext>();
                    dbContext.Database.Migrate();

                    // Seed data using IdentitySeedService
                    var seedService = services.GetRequiredService<DBInitializer>();
                    seedService.SeedInitialData().Wait();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
            }
            /////////////////////////////////////////////////////////////////////////////

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseRouting();
            app.UseHttpsRedirection();
            app.UseCors("CorsPolicy");
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}