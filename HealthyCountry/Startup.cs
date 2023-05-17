using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using AutoMapper;
using HealthyCountry.Hubs;
using HealthyCountry.Models;
using HealthyCountry.Repositories;
using HealthyCountry.RTC;
using HealthyCountry.RTC.Interfaces.Repositories;
using HealthyCountry.RTC.Interfaces.Services;
using HealthyCountry.RTC.Repositories;
using HealthyCountry.RTC.Services;
using HealthyCountry.Services;
using HealthyCountry.Utilities;
using HealthyCountry.Utilities.DbContext;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace HealthyCountry
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient();
            services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins,
                    builder =>
                    {
                        builder.WithOrigins("http://localhost:3000",
                            "http://localhost:5000")
                            .AllowCredentials()
                            .AllowAnyHeader()
                            .AllowAnyMethod()
                            ;
                    });
            });
            services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new CamelCaseNamingStrategy { ProcessDictionaryKeys = true }
                };
                options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });;
            services.AddDbContext<ApplicationDbContext>(
                options => options.UseNpgsql(Configuration.GetConnectionString("database")));
            
            var websocketDbSettings = Configuration.GetSection("websocketDb").Get<MongoDbContextOptions<WebsocketContext>>();
            services.AddDbContext<WebsocketContext>(options =>
            {
                options.MongoDbConnectionString = websocketDbSettings.MongoDbConnectionString;
                options.Mapping = websocketDbSettings.Mapping;
            });
            
            services.AddScoped<IConferenceService, ConferenceService>();
            services.AddScoped<IChatsService, ChatsService>();
            services.AddScoped<IChatsGroupsService, ChatsGroupsService>();
            services.AddScoped<INotificationGroupsService, NotificationGroupsService>();
            services.AddSingleton<IEventsRepository, EventsRepository>();
            services.AddSingleton<ICallGroupsRepository, CallGroupsRepository>();
            services.AddSingleton<INotificationGroupsRepository, NotificationGroupsRepository>();
            services.AddSingleton<IIceServerRepository, IceServerRepository>();
            services.AddSingleton<IChatsGroupsRepository, ChatsGroupsRepository>();
            services.AddSingleton<IChatsRepository, ChatsRepository>();

            var signingKey = Encoding.ASCII.GetBytes(Configuration.GetSection("auth:key").Value);
            services.AddAuthentication(x =>
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
                        IssuerSigningKey = new SymmetricSecurityKey(signingKey),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                    x.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            var accessToken = context.Request.Query["access_token"];

                            // If the request is for our hub...
                            var path = context.HttpContext.Request.Path;
                            if (!string.IsNullOrEmpty(accessToken) &&
                                (path.StartsWithSegments("/signalrtc")))
                            {
                                // Read the token out of the query string
                                context.Token = accessToken;
                            }
                            return Task.CompletedTask;
                        }
                    };
                });

            // configure DI for application services
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IOrganizationService, OrganizationService>();
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new EntityToModelProfile());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "HealthyCountry",
                    Description = "HealthyCountry API.",
                    
                });
                
                c.CustomSchemaIds(x => x.FullName);
                c.ResolveConflictingActions(x => x.First());

            });
            services.AddSignalR()
                // .AddStackExchangeRedis(appConfiguration.Redis.ConnectionString, options =>
                // {
                //     options.Configuration.ChannelPrefix = "sockets";
                //     options.Configuration.DefaultDatabase = appConfiguration.Redis.DatabaseId;
                // })
                .AddNewtonsoftJsonProtocol(options =>
                {
                    options.PayloadSerializerSettings.ContractResolver = new DefaultContractResolver
                    {
                        NamingStrategy = new CamelCaseNamingStrategy {ProcessDictionaryKeys = true}
                    };
                    options.PayloadSerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                });
                // .AddJsonProtocol(options => {
                //     options.PayloadSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                //     options.PayloadSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
                //     options.PayloadSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                //     options.PayloadSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                // });
            services.AddSwaggerGenNewtonsoftSupport();
            services.AddLocalization();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ApplicationDbContext dbContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseHttpsRedirection();
            app.UseCors(MyAllowSpecificOrigins);
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<RtcHub>("/signalrtc");
            });
            dbContext.Database.Migrate();
            app.ApplicationServices.GetRequiredService<WebsocketContext>().Migrate();
        }
    }
}