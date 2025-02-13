using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ManagerLayer.Interfaces;
using ManagerLayer.Services;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RepositoryLayer.Context;
using RepositoryLayer.Interfaces;
using RepositoryLayer.Services;

namespace FundooNotes
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        //startup class-> configureSevices method-> services.AddDbContext<FundooDBContext>(a => a.UseSqlServer(Configuration["ConnectionStrings:DBConnection"]));


        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)//, IConfiguration configuration)// added IConfiguration for RedisCache
        {
            services.AddControllers();
            services.AddDbContext<FundooDBContext>(a => a.UseSqlServer(Configuration["ConnectionStrings:DBConnection"]));
            services.AddTransient<IUserRepository, UserRepository>();// for User
            services.AddTransient<IUserManager, UserManager>();// for User

            services.AddTransient<INotesRepository, NotesRepository>();// for Notes
            services.AddTransient<INotesManager, NotesManager>();// for Notes
            services.AddTransient<ILabelManager, LabelManager>();// for label
            services.AddTransient<ILabelRepository, LabelRepository>();
            services.AddTransient<ICollaboratorManager, CollaboratorManager>();// for collaborator
            services.AddTransient<ICollaboratorRepository, CollaboratorRepository>();

            services.AddStackExchangeRedisCache(options => { options.Configuration = Configuration["RedisCacheUrl"]; });// added for RedisCache

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "JWTToken_Auth_API",
                    Version = "v1"
                });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                    {
                        new OpenApiSecurityScheme {
                            Reference = new OpenApiReference {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
            });


            
            services.AddMassTransit(x =>
            {
                x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(config =>
                {
                    config.UseHealthCheck(provider);
                    config.Host(new Uri("rabbitmq://localhost"), h =>
                    {
                        h.Username("guest");
                        h.Password("guest");
                    });
                }));
            });
            services.AddMassTransitHostedService();

            services.AddAuthentication(options => {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
        // Adding Jwt Bearer
        .AddJwtBearer(options => {
            options.SaveToken = true;
            options.RequireHttpsMetadata = false;
            options.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidAudience = Configuration["Jwt:Audience"],
                ValidIssuer = Configuration["Jwt:Issuer"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
            };
        });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseAuthentication();
            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Employee API V1");
            });


            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

       
    }
}
