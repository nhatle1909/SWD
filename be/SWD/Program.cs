using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Models.Repository;
using MongoDB.Driver;
using Repositories.Repository;
using Services.Tool;
using Services.Interface;
using Services.Service;
using System.Text;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using System.Text.Json.Serialization;

namespace SWD
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            builder.Services.AddScoped<IAccountService, AccountService>();
            builder.Services.AddScoped<IMaterialService, MaterialService>();
            builder.Services.AddScoped<IInteriorService, InteriorService>();
            builder.Services.AddScoped<IBlogService, BlogService>();
            builder.Services.AddScoped<IRequestService, RequestService>();
            builder.Services.AddScoped<IContactService, ContactService>();
            builder.Services.AddScoped<ICartService, CartService>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            //Add HttpContext
            builder.Services.AddHttpContextAccessor();

            //Add email sender
            builder.Services.AddOptions();
            var mailsetting = builder.Configuration.GetSection("MailSettings");
            builder.Services.Configure<MailSettings>(mailsetting);
            builder.Services.AddSingleton<IEmailSender, SendEmailTool>();

            builder.Services.AddControllers();

            builder.Services.AddAutoMapper(typeof(MapperProfileTool));

            builder.Services.AddControllersWithViews();
            //enum convert to string
            builder.Services.AddControllers()
                            .AddJsonOptions(options =>
                            {
                                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                            });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1",
                    new OpenApiInfo
                    {
                        Title = "SWD",
                        Description = "SWD Source",
                        Version = "v1",
                    });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });



            builder.Services.AddSingleton<IMongoClient, MongoClient>(s =>
            {
                var uri = s.GetRequiredService<IConfiguration>()["ConnectionString"];
                return new MongoClient(uri);
            });
            builder.Services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.AddConsole(); // Console Logging
            });
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddCookie()
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"] ?? throw new ArgumentNullException("builder.Configuration[\"Jwt:Key\"]", "Jwt:Key is null"))),
                    ValidIssuer = builder.Configuration["JWT:Issure"],
                    ValidAudience = builder.Configuration["JWT:Audience"],
                    ClockSkew = TimeSpan.Zero
                };
            });
            //Add Cors Policy
            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
            });
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            // run local
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            //publish api

            //app.UseSwagger(options =>
            //{
            //    options.RouteTemplate = "swagger/{documentName}/swagger.json";
            //});
            //app.UseSwaggerUI(c =>
            //{
            //    c.SwaggerEndpoint("/swagger/v1/swagger.json", "SWD API");

            //    c.RoutePrefix = "";
            //    c.EnableTryItOutByDefault();
            //});
            app.UseHttpsRedirection();
            app.UseCors();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization();

            app.MapControllers();
            app.Run();
        }
    }
}