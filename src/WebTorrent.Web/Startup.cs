using System.Net;
using AspNet.Security.OAuth.Validation;
using AspNet.Security.OpenIdConnect.Primitives;
using AutoMapper;
using Exceptionless;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Swagger;
using WebTorrent.Data;
using WebTorrent.Data.Core;
using WebTorrent.Data.Core.Interfaces;
using WebTorrent.Data.Models;
using WebTorrent.Services.FileSystemService;
using WebTorrent.Services.TorrentService;
using WebTorrent.Tools.FFmpeg;
using WebTorrent.Web.Authorization;
using WebTorrent.Web.Helpers;
using WebTorrent.Web.ViewModels;
using AppPermissions = WebTorrent.Data.Core.ApplicationPermissions;

namespace WebTorrent.Web
{
    public class Startup
    {
        //private readonly IHostingEnvironment _hostingEnvironment;


        public Startup(IConfiguration configuration /*, IHostingEnvironment env*/)
        {
            Configuration = configuration;
            //_hostingEnvironment = env;
        }

        public IConfiguration Configuration { get; }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlite(Configuration["ConnectionStrings:Sqlite"],
                    b => b.MigrationsAssembly("WebTorrent.Web"));

                //localdb
                //options.UseSqlServer(Configuration["ConnectionStrings:DefaultConnection"], b => b.MigrationsAssembly("WebTorrent.Web"));
                options.UseOpenIddict();
            });

            // add identity
            services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // Configure Identity options and password complexity here
            services.Configure<IdentityOptions>(options =>
            {
                // User settings
                options.User.RequireUniqueEmail = true;

                //    //// Password settings
                //    //options.Password.RequireDigit = true;
                //    //options.Password.RequiredLength = 8;
                //    //options.Password.RequireNonAlphanumeric = false;
                //    //options.Password.RequireUppercase = true;
                //    //options.Password.RequireLowercase = false;

                //    //// Lockout settings
                //    //options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                //    //options.Lockout.MaxFailedAccessAttempts = 10;

                options.ClaimsIdentity.UserNameClaimType = OpenIdConnectConstants.Claims.Name;
                options.ClaimsIdentity.UserIdClaimType = OpenIdConnectConstants.Claims.Subject;
                options.ClaimsIdentity.RoleClaimType = OpenIdConnectConstants.Claims.Role;
            });


            // Register the OpenIddict services.
            services.AddOpenIddict(options =>
            {
                options.AddEntityFrameworkCoreStores<ApplicationDbContext>();
                options.AddMvcBinders();
                options.EnableTokenEndpoint("/connect/token");
                options.AllowPasswordFlow();
                options.AllowRefreshTokenFlow();

                //if (_hostingEnvironment.IsDevelopment()) //Uncomment to only disable Https during development
                options.DisableHttpsRequirement();

                //options.UseRollingTokens(); //Uncomment to renew refresh tokens on every refreshToken request
                //options.AddSigningKey(new SymmetricSecurityKey(System.Text.Encoding.ASCII.GetBytes(Configuration["STSKey"])));
            });


            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = OAuthValidationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = OAuthValidationDefaults.AuthenticationScheme;
            }).AddOAuthValidation();


            // Add cors
            services.AddCors();

            // Add framework services.
            services.AddMvc();


            // Enforce https during production. To quickly enable ssl during development. Go to: Project Properties->Debug->Enable SSL
            //if (!_hostingEnvironment.IsDevelopment())
            //    services.Configure<MvcOptions>(options => options.Filters.Add(new RequireHttpsAttribute()));


            //Todo: ***Using DataAnnotations for validation until Swashbuckle supports FluentValidation***
            //services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<Startup>());


            //.AddJsonOptions(opts =>
            //{
            //    opts.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            //});


            services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("BearerAuth", new ApiKeyScheme
                {
                    Name = "Authorization",
                    Description = "Login with your bearer authentication token. e.g. Bearer <auth-token>",
                    In = "header",
                    Type = "apiKey"
                });

                c.SwaggerDoc("v1", new Info {Title = "QuickApp API", Version = "v1"});
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy(Policies.ViewAllUsersPolicy,
                    policy => policy.RequireClaim(CustomClaimTypes.Permission, AppPermissions.ViewUsers));
                options.AddPolicy(Policies.ManageAllUsersPolicy,
                    policy => policy.RequireClaim(CustomClaimTypes.Permission, AppPermissions.ManageUsers));

                options.AddPolicy(Policies.ViewAllRolesPolicy,
                    policy => policy.RequireClaim(CustomClaimTypes.Permission, AppPermissions.ViewRoles));
                options.AddPolicy(Policies.ViewRoleByRoleNamePolicy,
                    policy => policy.Requirements.Add(new ViewRoleAuthorizationRequirement()));
                options.AddPolicy(Policies.ManageAllRolesPolicy,
                    policy => policy.RequireClaim(CustomClaimTypes.Permission, AppPermissions.ManageRoles));

                options.AddPolicy(Policies.AssignAllowedRolesPolicy,
                    policy => policy.Requirements.Add(new AssignRolesAuthorizationRequirement()));
            });

            Mapper.Initialize(cfg => { cfg.AddProfile<AutoMapperProfile>(); });


            // Configurations
            services.Configure<SmtpConfig>(Configuration.GetSection("SmtpConfig"));


            // Business Services
            services.AddScoped<IEmailer, Emailer>();


            // Repositories
            services.AddScoped<IUnitOfWork, HttpUnitOfWork>();
            services.AddScoped<IAccountManager, AccountManager>();

            // Auth Handlers
            services.AddSingleton<IAuthorizationHandler, ViewUserAuthorizationHandler>();
            services.AddSingleton<IAuthorizationHandler, ManageUserAuthorizationHandler>();
            services.AddSingleton<IAuthorizationHandler, ViewRoleAuthorizationHandler>();
            services.AddSingleton<IAuthorizationHandler, AssignRolesAuthorizationHandler>();

            // DB Creation and Seeding
            services.AddTransient<IDatabaseInitializer, DatabaseInitializer>();

            // Services
            services.AddSingleton<IFileSystemService, FsInfoService>();
            services.AddSingleton<ITorrentService, TorrentService>();

            services.AddSingleton<FFmpeg>();

            // Configure FFmpeg using appsettings.json file.
            services.Configure<FFmpegSettings>(Configuration.GetSection("FFmpeg"));

            services.AddMemoryCache();
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug(LogLevel.Warning);
            loggerFactory.AddFile(Configuration.GetSection("Logging"));

            Utilities.ConfigureLogger(loggerFactory);
            EmailTemplates.Initialize(env);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions
                {
                    HotModuleReplacement = true
                });
            }
            else
            {
                // Enforce https during production
                //var rewriteOptions = new RewriteOptions()
                //    .AddRedirectToHttps();
                //app.UseRewriter(rewriteOptions);

                app.UseExceptionHandler("/Home/Error");
            }


            //Configure Cors
            app.UseCors(builder => builder
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod());


            app.UseStaticFiles();
            app.UseAuthentication();


            app.UseExceptionHandler(builder =>
            {
                builder.Run(async context =>
                {
                    context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
                    context.Response.ContentType = MediaTypeNames.ApplicationJson;

                    var error = context.Features.Get<IExceptionHandlerFeature>();

                    if (error != null)
                    {
                        string errorMsg = JsonConvert.SerializeObject(new {error = error.Error.Message});
                        await context.Response.WriteAsync(errorMsg).ConfigureAwait(false);
                    }
                });
            });


            app.UseSwagger();
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "QuickApp API V1"); });


            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                routes.MapSpaFallbackRoute(
                    name: "spa-fallback",
                    defaults: new { controller = "Home", action = "Index" });
            });


            // Set up custom content types -associating file extension to MIME type
            var provider = new FileExtensionContentTypeProvider
            {
                Mappings =
                {
                    [".m3u8"] = "application/x-mpegURL",
                    [".woff"] = "application/x-font-woff",
                    [".woff2"] = "font/woff2",
                    [".ttf"] = "font/ttf"
                }
            };
            // Add new mappings

            app.UseStaticFiles(new StaticFileOptions
            {
                ContentTypeProvider = provider
            });

            //app.UseDirectoryBrowser(new DirectoryBrowserOptions()
            //{
            //    FileProvider = new PhysicalFileProvider(Path.Combine(env.WebRootPath, "logs")),
            //    RequestPath = new PathString("/logs")
            //});

            app.UseWebSockets();

            //store unhandled exceptions https://be.exceptionless.io/dashboard
            app.UseExceptionless("XA6U77cTOrVkt1J23Wh9Hs0IO34PDYUGCYLzXyCC");
        }
    }
}