using Hangfire;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MimeKit;
using PTS_API.Authentication;
using PTS_API.GateWay.Email;
using PTS_API.Utils;
using PTS_BUSINESS.Email;
using PTS_BUSINESS.Services.Implementations;
using PTS_BUSINESS.Services.Interfaces;
using PTS_CORE.Domain.Entities;
using PTS_DATA.EfCore.Context;
using PTS_DATA.Repository.Implementations;
using PTS_DATA.Repository.Interfaces;
using System.Net.Mail;
using System.Text;

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
                b.WithOrigins("https://localhost:7065/")
                .AllowAnyMethod()
               // .AllowAnyOrigin()
                .AllowAnyHeader();

            }));

            builder.Services.AddHttpContextAccessor();
            builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

            builder.Services.AddHangfire(configuration => configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(builder.Configuration.GetConnectionString("Database"), new SqlServerStorageOptions
                {
                    CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                    SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                    QueuePollInterval = TimeSpan.Zero,
                    UseRecommendedIsolationLevel = true,
                    DisableGlobalLocks = true
                }));

           
            builder.Services.AddHangfireServer();

            builder.Services.AddDbContext<ApplicationDBContext>(options =>
               options.UseSqlServer(builder.Configuration.GetConnectionString("Database")));

            builder.Services.AddFluentEmail("tijaniadebayoabdllahi@gmail.com")
                .AddRazorRenderer()
                .AddSmtpSender("smtp-relay.brevo.com", 587);

            #region Identity


            builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                //password requirement
                options.Password.RequiredLength = 8;
               // options.Password.RequireNonAlphanumeric = true;
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

            #region Dependencies
            builder.Services.AddCors();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddScoped<DBInitializer>();  //Seeding datas in to the DB
            builder.Services.AddScoped<IAccountService, AccountService>();

            builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            builder.Services.AddScoped<IEmployeeService, EmployeeService>();

            builder.Services.AddScoped<IVehicleRepository, VehicleRepository>();
            builder.Services.AddScoped<IVehicleService, VehicleService>();

            builder.Services.AddScoped<ITerminalRepository, TerminalRepository>();
            builder.Services.AddScoped<ITerminalService, TerminalService>();

            builder.Services.AddScoped<IStoreItemRepository, StoreItemRepository>();
            builder.Services.AddScoped<IStoreItemService, StoreItemService>();

            builder.Services.AddScoped<ILeaveRepository, LeaveRepository>();
            builder.Services.AddScoped<ILeaveService, LeaveService>();

            builder.Services.AddScoped<IComplainRepository, ComplainRepository>();
            builder.Services.AddScoped<IComplainService, ComplainService>();

            builder.Services.AddScoped<IOtherRequestRepository, OtherRequestRepository>();
            builder.Services.AddScoped<IOtherRequestService, OtherRequestService>();

            builder.Services.AddScoped<IStoreItemRequestRepository, StoreItemRequestRepository>();
            builder.Services.AddScoped<IStoreItemRequestService, StoreItemRequestService>();

            builder.Services.AddScoped<IStoreAssetRepository, StoreAssetRepository>();
            builder.Services.AddScoped<IStoreAssetService, StoreAssetService>();

            builder.Services.AddScoped<IStaffAssetRepository, StaffAssetRepository>();
            builder.Services.AddScoped<IStaffAssetService, StaffAssetService>(); 
            
            builder.Services.AddScoped<IBudgetTrackingRepository, BudgetTrackingRepository>();
            builder.Services.AddScoped<IBudgetTrackingService, BudgetTrackingService>();

            builder.Services.AddScoped<IExpenditureRepository, ExpenditureRepository>();
            builder.Services.AddScoped<IExpenditureService, ExpenditureService>(); 
            
            builder.Services.AddScoped<IBusBrandingRepository, BusBrandingRepository>();
            builder.Services.AddScoped<IBusBrandingService, BusBrandingService>();

            builder.Services.AddScoped<IHireVehicleRepository, HireVehicleRepository>();
            builder.Services.AddScoped<IHireVehicleService, HireVehicleService>();

            builder.Services.AddScoped<ISaleRepository, SaleRepository>();
            builder.Services.AddScoped<ISaleService, SaleService>();

            builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            builder.Services.AddScoped<IDepartmentService, DepartmentService>();

            builder.Services.AddScoped<IDepartmentalSaleRepository, DepartmentalSaleRepository>();
            builder.Services.AddScoped<IDepartmentalSaleService, DepartmentalSaleService>();

            builder.Services.AddScoped<IDepartmentalExpenditureBudgetRepository, DepartmentalExpenditureBudgetRepository>();
            builder.Services.AddScoped<IDepartmentalExpenditureBudgetService, DepartmentalExpenditureBudgetService>();

            builder.Services.AddScoped<IEmailSender, EmailSender>();
            builder.Services.AddScoped<IEmailSender1, EmailSender1>();
         


            //builder.Services.AddScoped<ITokenInvalidationService, TokenInvalidationService>();
            
          

            builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));

            // Configure MailKit manually
            builder.Services.AddScoped(provider =>
            {
                var emailSettings = provider.GetRequiredService<IOptions<EmailSettings>>().Value;

                return new SmtpClient
                {
                   /* ServerCertificateValidationCallback = (s, c, h, e) => true,
                    ConnectTimeout = 15000, // in milliseconds*/
                    Port = emailSettings.SmtpPort,
                    DeliveryFormat = SmtpDeliveryFormat.International,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                   /* SecureSocketOptions = SecureSocketOptions.Auto,
                    CheckCertificateRevocation = false*/
                };
            });

            builder.Services.AddScoped(provider =>
            {
                var emailSettings = provider.GetRequiredService<IOptions<EmailSettings>>().Value;
                var smtpClient = provider.GetRequiredService<SmtpClient>();

                return new MimeMessage
                {
                    Sender = new MailboxAddress(emailSettings.SenderName, emailSettings.SenderEmail),
                    Body = new TextPart("plain") { Text = "Test message" }
                };
            });

            builder.Services.AddScoped<IJWTAuthentication, JWTAuthentication>();
            builder.Services.AddRouting(option => option.LowercaseUrls = true);
            /* builder.Services.AddControllers().AddJsonOptions(opt => opt.JsonSerializerOptions.PropertyNamingPolicy = null)
                 .AddNewtonsoftJson(opt =>
                 {
                     opt.SerializerSettings.ContractResolver = new DefaultContractResolver();
                     opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                 });*/
            



            builder.Services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetValue<string>("Key"))),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };

                });

            #endregion


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
               //if (app.Environment.IsDevelopment())
               //{
                   app.UseSwagger();
                   app.UseSwaggerUI();
              // }
            #region Security

                app.Use(async (context, next) =>
                {
                    context.Response.Headers.Add("Content-Security-Policy", "default-src 'self'");
                    await next();
                });
                app.Use(async (context, next) =>
                {
                    context.Response.Headers.Add("X-Frame-Options", "DENY");
                    await next();
                });
                app.Use(async (context, next) =>
                {
                    context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
                    await next();
                });
                app.UseForwardedHeaders(new ForwardedHeadersOptions
                {
                    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
                });
                app.UseCors(b =>
                {
                    b.WithOrigins("https://localhost:7065/")
                    .AllowAnyMethod()
                    // .AllowAnyOrigin()
                    .AllowAnyHeader();

                });
            #endregion

            app.UseHangfireDashboard();
               app.UseHangfireServer();

               RecurringJob.AddOrUpdate<IBusBrandingService>
                ("MarkExpiredBrandAsDeletedJob", x => x.MarkExpiredBrandAsDeleted(), "*/5 * * * *"); // Run every 5 minutes
               RecurringJob.AddOrUpdate<IEmployeeService>
                ("SendBirthdayMailForEmployee", x => x.EmployeeBirthdayForToday(), "*/2 * * * *"); //Runs every 2 minutes

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