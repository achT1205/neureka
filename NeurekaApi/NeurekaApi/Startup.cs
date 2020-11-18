using AutoMapper;
using NeurekaApi.Hubs;
using NeurekaDAL.Models;
using NeurekaDAL.Repositories;
using NeurekaService.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace NeurekaApi
{
    public class Startup
    {
        readonly string CorsPolicy = "_CorsPolicy";
        private readonly IWebHostEnvironment _env;
        public Startup(IWebHostEnvironment env)
        {
            _env = env;
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddTransient<INeurekaDBContext, NeurekaDBContext>();
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = $"APIs({_env.EnvironmentName}) Authorization", Version = "V1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Autorization header using the Bearer scheme Example : \"Authorization: Bearer {token}\" ",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                {
                 new OpenApiSecurityScheme
                 {
                   Reference = new OpenApiReference
                   {
                     Type = ReferenceType.SecurityScheme,
                     Id = "Bearer"
                   }
                  },
                  new string[] { }
                }
              });
            });
            services.Configure<NeurekaAppSettings>(Configuration.GetSection(nameof(NeurekaAppSettings)));
            services.AddSingleton<INeurekaAppSettings>(sp => sp.GetRequiredService<IOptions<NeurekaAppSettings>>().Value);
            services.Configure<MailKitConfig>(Configuration.GetSection(nameof(MailKitConfig)));
            services.AddSingleton<IMailKitConfig>(sp => sp.GetRequiredService<IOptions<MailKitConfig>>().Value);
            services.AddTransient<IPatientService, PatientService>();
            services.AddTransient<IPatientRepository, PatientRepository>();
            services.AddTransient<IVisitService, VisitService>();
            services.AddTransient<IVisitRepository, VisitRepository>();
            services.AddTransient<ITemplateService, TemplateService>();
            services.AddTransient<ITemplateRepository, TemplateRepository>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddAutoMapper(typeof(Startup));

            var settings = Configuration.GetSection("NeurekaAppSettings").Get<NeurekaAppSettings>();
            services.AddAuthentication("OAuth")
              .AddJwtBearer("OAuth", config =>
              {
                  var secreBytes = Encoding.UTF8.GetBytes(settings.Secret);
                  var key = new SymmetricSecurityKey(secreBytes);

                  config.TokenValidationParameters = new TokenValidationParameters()
                  {
                      IssuerSigningKey = key,
                      ValidIssuer = settings.Issuer,
                      ValidAudience = settings.Audience,
                  };
              });

            services.AddCors(options =>
            {
                options.AddPolicy(name: CorsPolicy,
            builder =>
            {
                builder
                .AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowed(origin => true)
                .AllowCredentials();
            });
            });
            services.AddSignalR();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
            });
            app.UseCors(CorsPolicy);
            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<NotificationHub>("/notification");
            });
        }
    }
}
