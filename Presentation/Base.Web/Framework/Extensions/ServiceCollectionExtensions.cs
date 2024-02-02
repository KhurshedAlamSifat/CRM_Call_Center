using System;
using System.IO.Compression;
using System.Linq;
using System.Net;
using Azure.Identity;
using Base.Core;
using Base.Core.Caching;
using Base.Core.Configuration;
using Base.Core.Events;
using Base.Core.Http;
using Base.Core.Infrastructure;
using Base.Core.Security;
using Base.Data;
using Base.Data.Mapping;
using Base.Services.Authentication;
using Base.Services.Events;
using Base.Services.Security;
using Base.Services.Users;
using FluentMigrator.Runner.Conventions;
using FluentMigrator.Runner.Initialization;
using FluentMigrator.Runner.Processors;
using FluentMigrator.Runner;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;
using NUglify;
using StackExchange.Profiling.Storage;
using WebMarkupMin.AspNetCore7;
using WebMarkupMin.Core;
using Base.Services.Districts;
using Base.Services.Thanas;
using Base.Services.ComplainTypes;
using Microsoft.AspNetCore.Mvc.Routing;
using Base.Web.Framework.UI;
using Base.Web.Framework.Menu;
using Base.Services.BusinessUnits;
using Base.Services.Notification;
using Base.Services.Brands;
using Base.Services.Categories;
using Base.Services.ServiceCenters;
using Base.Services.Technicians;
using Base.Services.Problems;
using Base.Services.Customers;
using Base.Services.Tickets;
using AutoMapper;
using Base.Web.Areas.Secure.Mapping;
using Base.Web.Areas.Secure.Validators;
using Base.Web.Areas.Validatiors;

namespace Base.Web.Framework.Extensions
{
    /// <summary>
    /// Represents extensions of IServiceCollection
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Configure base application settings
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        /// <param name="builder">A builder for web applications and services</param>
        public static void ConfigureApplicationSettings(this IServiceCollection services,
            WebApplicationBuilder builder)
        {
            //let the operating system decide what TLS protocol version to use
            //see https://docs.microsoft.com/dotnet/framework/network-programming/tls
            ServicePointManager.SecurityProtocol = SecurityProtocolType.SystemDefault;
            //create default file provider
            CommonHelper.DefaultFileProvider = new CrmFileProvider(builder.Environment);

            //register type finder
            var typeFinder = new WebAppTypeFinder();
            Singleton<ITypeFinder>.Instance = typeFinder;
            services.AddSingleton<ITypeFinder>(typeFinder);

            //add configuration parameters
            var configurations = typeFinder
                .FindClassesOfType<IConfig>()
                .Select(configType => (IConfig)Activator.CreateInstance(configType))
                .ToList();

            foreach (var config in configurations)
                builder.Configuration.GetSection(config.Name).Bind(config, options => options.BindNonPublicProperties = true);


            var appSettings = AppSettingsHelper.SaveAppSettings(configurations, CommonHelper.DefaultFileProvider, false);
            services.AddSingleton(appSettings);
        }

        /// <summary>
        /// Add services to the application and configure service provider
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        /// <param name="builder">A builder for web applications and services</param>
        public static void ConfigureApplicationServices(this IServiceCollection services, WebApplicationBuilder builder)
        {
            var engine = EngineContext.Create();

            services.AddHttpContextAccessor();
            services.AddResponseCompression(options =>
            {
                options.EnableForHttps = true;
                options.Providers.Add<BrotliCompressionProvider>();
                options.Providers.Add<GzipCompressionProvider>();
            });
            services.Configure<BrotliCompressionProviderOptions>(options =>
            {
                options.Level = CompressionLevel.Fastest;
            });

            services.Configure<GzipCompressionProviderOptions>(options =>
            {
                options.Level = CompressionLevel.SmallestSize;
            });
            services.AddWebOptimizer();

            services
            .AddScoped<IConnectionStringAccessor>(x => DataSettingsManager.LoadSettings())
            .AddTransient<IMappingEntityAccessor>(x => x.GetRequiredService<IDataProviderManager>().DataProvider)
            .ConfigureRunner(rb =>rb.AddSqlServer());

            

            services.AddTransient<IDataProviderManager, DataProviderManager>();
            services.AddTransient(serviceProvider =>
            serviceProvider.GetRequiredService<IDataProviderManager>().DataProvider);
            services.AddScoped(typeof(IRepository<>), typeof(EntityRepository<>));
            services.AddTransient<IEventPublisher, EventPublisher>();

            services.AddOptions();
            services.AddNopWebMarkupMin();
            services.AddHttpSession();
            services.AddAntiForgery();
            services.AddCrmDataProtection();
            services.AddCrmAuthentication();
            services.AddCrmMvc();
            services.AddWebEncoders();
            services.ResolveDependency();

            engine.ConfigureServices(services, builder.Configuration);
        }

        //Do not delete in future we may use it.
        //public static void AddDistributedCache(this IServiceCollection services)
        //{
        //    var appSettings = Singleton<AppSettings>.Instance;
        //    var distributedCacheConfig = appSettings.Get<DistributedCacheConfig>();

        //    if (!distributedCacheConfig.Enabled)
        //        return;

        //    switch (distributedCacheConfig.DistributedCacheType)
        //    {
        //        case DistributedCacheType.Memory:
        //            services.AddDistributedMemoryCache();
        //            break;

        //        case DistributedCacheType.Redis:
        //            services.AddStackExchangeRedisCache(options =>
        //            {
        //                options.Configuration = distributedCacheConfig.ConnectionString;
        //            });
        //            break;
        //    }
        //}

        public static void AddNopWebMarkupMin(this IServiceCollection services)
        {
            services.AddWebMarkupMin(
            options =>
             {
              options.AllowMinificationInDevelopmentEnvironment = true;
              options.AllowCompressionInDevelopmentEnvironment = true;
          })
          .AddHtmlMinification(
              options =>
              {
                  options.MinificationSettings.RemoveRedundantAttributes = true;
                  options.MinificationSettings.RemoveHttpProtocolFromAttributes = true;
                  options.MinificationSettings.RemoveHttpsProtocolFromAttributes = true;
              })
          .AddHttpCompression();
        }
        public static void AddHttpSession(this IServiceCollection services)
        {
            services.AddSession(options =>
            {
                options.Cookie.Name = $"{CrmCookieDefaults.Prefix}{CrmCookieDefaults.SessionCookie}";
                options.Cookie.HttpOnly = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
            });
        }
        public static void AddAntiForgery(this IServiceCollection services)
        {
            //override cookie name
            services.AddAntiforgery(options =>
            {
                options.Cookie.Name = $"{CrmCookieDefaults.Prefix}{CrmCookieDefaults.AntiforgeryCookie}";
                options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
            });
        }
        public static void AddCrmDataProtection(this IServiceCollection services)
        {

                var dataProtectionKeysPath = CommonHelper.DefaultFileProvider.MapPath(CrmDataProtectionDefaults.DataProtectionKeysPath);
                var dataProtectionKeysFolder = new System.IO.DirectoryInfo(dataProtectionKeysPath);

                services.AddDataProtection().PersistKeysToFileSystem(dataProtectionKeysFolder);
        }
        public static void AddCrmAuthentication(this IServiceCollection services)
        {
            //set default authentication schemes
            var authenticationBuilder = services.AddAuthentication(options =>
            {
                options.DefaultChallengeScheme = AuthenticationDefaults.AuthenticationScheme;
                options.DefaultScheme = AuthenticationDefaults.AuthenticationScheme;
            });

            //add main cookie authentication
            authenticationBuilder.AddCookie(AuthenticationDefaults.AuthenticationScheme, options =>
            {
                options.Cookie.Name = $"{CrmCookieDefaults.Prefix}{CrmCookieDefaults.AuthenticationCookie}";
                options.Cookie.HttpOnly = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
                options.LoginPath = AuthenticationDefaults.LoginPath;
                options.AccessDeniedPath = AuthenticationDefaults.AccessDeniedPath;
            });
        }

        [Obsolete]
        public static IMvcBuilder AddCrmMvc(this IServiceCollection services)
        {
            var mvcBuilder = services.AddControllersWithViews();
            mvcBuilder.AddRazorRuntimeCompilation();

            mvcBuilder.AddCookieTempDataProvider(options =>
            {
                options.Cookie.Name = $"{CrmCookieDefaults.Prefix}{CrmCookieDefaults.TempDataCookie}";
                options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
            });
            services.AddRazorPages();
            mvcBuilder.AddNewtonsoftJson(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver());
            services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();
            services.AddFluentValidation(fv =>
            {
                fv.RegisterValidatorsFromAssemblyContaining<ComplainTypeValidator>();
                fv.RegisterValidatorsFromAssemblyContaining<UserValidator>();
                fv.RegisterValidatorsFromAssemblyContaining<BrandValidator>();
                fv.RegisterValidatorsFromAssemblyContaining<BusinessUnitValidator>();
                fv.RegisterValidatorsFromAssemblyContaining<CategoryValidator>();
                fv.RegisterValidatorsFromAssemblyContaining<DistrictValidator>();
                fv.RegisterValidatorsFromAssemblyContaining<ProblemValidator>();
                fv.RegisterValidatorsFromAssemblyContaining<ServiceCenterValidator>();
                fv.RegisterValidatorsFromAssemblyContaining<TechnicianValidator>();
                fv.RegisterValidatorsFromAssemblyContaining<ThanaValidator>();
            });
            mvcBuilder.AddControllersAsServices();


            return mvcBuilder;
        }

        public static void ResolveDependency(this IServiceCollection services)
        {
            services.AddSingleton<ILocker, MemoryCacheManager>();
            services.AddSingleton<IStaticCacheManager, MemoryCacheManager>();
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddSingleton<INotificationService, NotificationService>();
            services.AddScoped<IEncryptionService, EncryptionService>();
            services.AddScoped<IAuthenticationService, CookieAuthenticationService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUrlHelperFactory, UrlHelperFactory>();
            services.AddScoped<IUserRegistrationService, UserRegistrationService>();
            services.AddScoped<IWebHelper, WebHelper>();
            services.AddScoped<IWorkContext, WebWorkContext>();


            services.AddScoped<IComplainTypeService, ComplainTypeService>();

            services.AddScoped<IDistrictService, DistrictService>();
            services.AddScoped<IThanaService, ThanaService> ();
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<IComplainTypeService, ComplainTypeService>();
            services.AddScoped<IProblemService, ProblemService>();
            services.AddScoped<IBusinessUnitService, BusinessUnitService>();
            services.AddScoped<IBrandService, BrandService>();
            services.AddScoped<ITicketService, TicketService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IPermissionService, PermissionService>();
            services.AddScoped<IPermissionProvider, StandardPermissionProvider>();
            services.AddScoped<ICrmHtmlHelper, CrmHtmlHelper>();
        
            services.AddScoped<IXmlSiteMap, XmlSiteMap>();
            services.AddScoped<IPermissionService, PermissionService>();
            services.AddScoped<IPermissionProvider, StandardPermissionProvider>();
            services.AddScoped<ICrmHtmlHelper, CrmHtmlHelper>();

            services.AddScoped<IXmlSiteMap, XmlSiteMap>();
            services.AddScoped<IDistrictService, DistrictService>();
            services.AddScoped<IThanaService, ThanaService> ();
            services.AddScoped<IServiceCenterService, ServiceCenterService>();
            services.AddScoped<ITechnicianService, TechnicianService>();

            services.AddScoped<IMapper>(sp =>
            {
                var configuration = new MapperConfiguration(cfg =>
                {
                    // Add your AutoMapper profiles here
                    cfg.AddProfile<MappingProfile>(); // Replace with your actual profile class
                });

                return new Mapper(configuration);
            });

        }
        /// <summary>
        /// Register HttpContextAccessor
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        public static void AddHttpContextAccessor(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }

    }
}