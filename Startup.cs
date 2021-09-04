using API_CORE.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using API_CORE.Repository;
using API_CORE.Repository.IRepository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace API_CORE
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            //Auto Mapper Dependency Injection
            services.AddAutoMapper(typeof(MyMapper));
            services.AddScoped<INationalParkRepository, NationalParkRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("NationalPark", new OpenApiInfo
                {
                    Title = "lower API_CORE",
                    Version = "v1",
                    Contact = new OpenApiContact
                    {
                        Name = "Ridwan",
                        Email = "ridwan.pust@gmail.com"
                    }
                });
                //this is not good practics
                //c.IncludeXmlComments("API CORE.xml");

                //var xmlCommentFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                //var cmlCommentsFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentFile);
                //c.IncludeXmlComments(cmlCommentsFullPath);


            });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("User", new OpenApiInfo
                {
                    Title = "lower API_CORE",
                    Version = "v1",
                    Contact = new OpenApiContact
                    {
                        Name = "Ridwan",
                        Email = "ridwan.pust@gmail.com"
                    }
                });
                //this is not good practics
                //c.IncludeXmlComments("API CORE.xml");

                //var xmlCommentFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                //var cmlCommentsFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentFile);
                //c.IncludeXmlComments(cmlCommentsFullPath);


            });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("Trail", new OpenApiInfo
                {
                    Title = "lower API_CORE",
                    Version = "v1",
                    Contact = new OpenApiContact
                    {
                        Name = "Ridwan",
                        Email = "ridwan.pust@gmail.com"
                    }
                });


                //this is not good practics
                //c.IncludeXmlComments("API CORE.xml");

                var xmlCommentFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var cmlCommentsFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentFile);
                c.IncludeXmlComments(cmlCommentsFullPath);


            });

            services.AddSwaggerGen(options =>
            {

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description =
               "JWT Authorization header using the Bearer scheme. \r\n\r\n " +
               "Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\n" +
               "Example: \"Bearer 12345abcdef\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,

                        },
                        new List<string>()
                    }
                });

            });
            //Dependency Injection
            //extract Configuration becauser we need the actual "AppSettings" Section value  for jwt token
            var AppSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(AppSettingsSection);

            var appSettings = AppSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret); 

            services.AddAuthentication(x=> {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x=> {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey=new SymmetricSecurityKey(key),
                    ValidateIssuer=false,
                    ValidateAudience=false
                
                };
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/NationalPark/swagger.json", "Upper API_CORE v1");
                    c.SwaggerEndpoint("/swagger/Trail/swagger.json", "Upper trail API_CORE v1");
                    c.SwaggerEndpoint("/swagger/User/swagger.json", "Upper User API_CORE v1");

                    //change base url of Swagger 
                    // c.RoutePrefix = "Ball";
                });
                
            }
            

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors(x=>x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

        }
    }
}
