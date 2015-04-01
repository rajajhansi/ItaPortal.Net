using System;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;
using Microsoft.Framework.DependencyInjection;
using Microsoft.Framework.ConfigurationModel;
using Microsoft.AspNet.Hosting;
using ItaPortal.Net.Models.Identity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Security.DataProtection;
using Microsoft.Data.Entity;
using Microsoft.Framework.Logging;
using Microsoft.Framework.Logging.Console;
using Microsoft.AspNet.Diagnostics;
using Microsoft.AspNet.Diagnostics.Entity;
using System.Threading.Tasks;
using Microsoft.Data.Entity.SqlServer;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.Identity.EntityFramework;
using ItaPortal.Net.Services;
using System.Net.Mail;
using System.Net;
using ItaPortal.Net.Data;
using Microsoft.AspNet.Mvc;
using Newtonsoft.Json.Serialization;

namespace ItaPortal.Net
{
    public class Startup
    {

        public IConfiguration Configuration { get; set; }
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public Startup(IHostingEnvironment env)
        {
            // Setup configuration sources
            Configuration = new Configuration()
                .AddJsonFile("config.json")
                .AddEnvironmentVariables();
        }
        public void ConfigureServices(IServiceCollection services)
        {
            // add Entity Framework
            services.AddEntityFramework(Configuration)
                    .AddSqlServer()
                    .AddDbContext<ApplicationDbContext>(options =>
                    {
                        options.UseSqlServer(Configuration.Get("Data:DefaultConnection:ConnectionString"));
                    });
            services.Configure<EmailMessageServiceOptions>(options =>
            {
                options.DeliveryMethod = (SmtpDeliveryMethod)Enum.Parse(typeof(SmtpDeliveryMethod), Configuration.Get("Smtp:DeliveryMethod"));
                options.UseDefaultCredentials = bool.Parse(Configuration.Get("Smtp:UseDefaultCredentials"));
                options.EnableSsl = bool.Parse(Configuration.Get("Smtp:EnableSsl"));
                options.Host = Configuration.Get("Smtp:Host");
                options.Port = int.Parse(Configuration.Get("Smtp:Port"));
                options.Credential = new NetworkCredential(Configuration.Get("Smtp:Credential:Username"),
                    Configuration.Get("Smtp:Credential:Password"));
            });
            //services.AddInstance<IUserTokenProvider<ApplicationUser>, new IDataProtectionProvider<ApplicationUser>(DataProtectionProvider.Create("My Asp.Net Identity"))();            
            // Add Identity Services to the Services Container
            services.AddIdentity<ApplicationUser, IdentityRole>(Configuration)
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // Add MVC Services to the Services Container
            services.AddMvc().Configure<MvcOptions>(options =>
            {
                // See Strathweb's great discussion of formatters: http://www.strathweb.com/2014/11/formatters-asp-net-mvc-6/

                // Support Camelcasing in MVC API Controllers
                var jsonOutputFormatter = new JsonOutputFormatter();
                jsonOutputFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                jsonOutputFormatter.SerializerSettings.DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Ignore;

                options.OutputFormatters.RemoveAll(formatter => formatter.Instance.GetType() == typeof(JsonOutputFormatter));
                options.OutputFormatters.Insert(0, jsonOutputFormatter);
            }); 
            // Add other services
            services.AddScoped<IAuthRepository, AuthRepository>();
            services.AddScoped<ILookupRepository, LookupRepository>();
            services.AddScoped<IMessageService, EmailMessageService>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerfactory)
        {
            // Configure the HTTP request pipeline.
            // Add the console logger.
            loggerfactory.AddConsole();

            // Add the following to the request pipeline only in development environment.
            if (string.Equals(env.EnvironmentName, "Development", StringComparison.OrdinalIgnoreCase))
            {
                app.UseBrowserLink();
                app.UseErrorPage(ErrorPageOptions.ShowAll);
                app.UseDatabaseErrorPage(DatabaseErrorPageOptions.ShowAll);
            }
            else
            {
                // Add Error handling middleware which catches all application specific errors and
                // send the request to the following path or controller action.
                app.UseErrorHandler("/Home/Error");
            }

            // Add static files to the request pipeline
            app.UseStaticFiles();

            // Add cookie-based authentication to the request pipeline.
            app.UseIdentity();

            // Add Mvc
            app.UseMvc();
            
            app.UseWelcomePage();
            // Add Seed Data
            var seedData = ActivatorUtilities.CreateInstance<SeedData>(app.ApplicationServices);
#if SEED_DATA
            seedData.Seed();
#endif            
        }
        
        //private static async Task InitializeUserManager(IServiceProvider serviceProvider)
        //{           
        //    var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        //    //userManager.RegisterTokenProvider()
        //}
        public static async Task Seed(IServiceProvider applicationServices)
        {
            using (var dbContext = applicationServices.GetService<ApplicationDbContext>())
            {
                var sqlServerDatabase = dbContext.Database as SqlServerDatabase;
                if(sqlServerDatabase != null)
                {
                    // Create database in user root
                    //if(await sqlServerDatabase.EnsureCreatedAsync())
                    {
                        // Add some default users
                        var users = new List<ApplicationUser>
                        {
                            new ApplicationUser { FirstName = "Raja", LastName = "Mani", Email = "myraja@hotmail.com", EmailConfirmed = true, Level = 1, JoinDate = new DateTime(2012, 12, 1)},
                            new ApplicationUser { FirstName = "Jhansirani", LastName = "Sarvamohan", Email = "jrani@hotmail.com", EmailConfirmed = true, Level = 1, JoinDate = new DateTime(2012, 12, 10)}
                        };
                        //users.ForEach(user => dbContext.Users.AddAsync(user));

                        // add some users
                        var userManager = applicationServices.GetService<UserManager<ApplicationUser>>();
                        //var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
                        var raja = new ApplicationUser { FirstName = "Raja", LastName = "Mani", Email = "rajajhansi@yahoo.com", EmailConfirmed = true, Level = 1, JoinDate = new DateTime(2012, 12, 1) };
                        await userManager.CreateAsync(raja, "P@ssw0rd1");

                        var jhansi = new ApplicationUser { FirstName = "Jhansirani", LastName = "Sarvamohan", Email = "jhansi_sm@yahoo.com", EmailConfirmed = true, Level = 1, JoinDate = new DateTime(2012, 12, 10) };
                        await userManager.CreateAsync(jhansi, "P@ssw0rd1");

                        //var createUserTasks = users.Select(user => userManager.CreateAsync(user, "P@ssw0rd"));

                        //var addClaimsToUserTasks = users.Select(user => userManager.AddClaimAsync(user, new System.Security.Claims.Claim("CanEdit", "true")));

                        //var results = await Task.WhenAll(createUserTasks);
                        //results = await Task.WhenAll(addClaimsToUserTasks);
                    }
                }
            }
        }
    }
}
