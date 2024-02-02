using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Base.Core;
using Base.Core.Http;
using Newtonsoft.Json.Serialization;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.FileProviders;
using Base.Core.Configuration;
using StackExchange.Profiling.Storage;

namespace Base.Core.Infrastructure
{
    /// <summary>
    /// Represents Nop engine
    /// </summary>
    public partial class CrmEngine : IEngine
    {
        #region Utilities

        /// <summary>
        /// Get IServiceProvider
        /// </summary>
        /// <returns>IServiceProvider</returns>
        protected virtual IServiceProvider GetServiceProvider(IServiceScope scope = null)
        {
            if (scope == null)
            {
                var accessor = ServiceProvider?.GetService<IHttpContextAccessor>();
                var context = accessor?.HttpContext;
                return context?.RequestServices ?? ServiceProvider;
            }
            return scope.ServiceProvider;
        }

        protected virtual Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            //check for assembly already loaded
            var assembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(a => a.FullName == args.Name);
            if (assembly != null)
                return assembly;

            //get assembly from TypeFinder
            var typeFinder = Singleton<ITypeFinder>.Instance;
            assembly = typeFinder?.GetAssemblies().FirstOrDefault(a => a.FullName == args.Name);
            return assembly;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Add and configure services
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        /// <param name="configuration">Configuration of the application</param>
        public virtual void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            //register engine
            services.AddSingleton<IEngine>(this);
            services.AddScoped<ICrmFileProvider, CrmFileProvider>();

            services.AddSingleton(services);
        }

        public virtual void ConfigureRequestPipeline(IApplicationBuilder application)
        {
            ServiceProvider = application.ApplicationServices;
        }

        /// <summary>
        /// Resolve dependency
        /// </summary>
        /// <param name="scope">Scope</param>
        /// <typeparam name="T">Type of resolved service</typeparam>
        /// <returns>Resolved service</returns>
        public virtual T Resolve<T>(IServiceScope scope = null) where T : class
        {
            return (T)Resolve(typeof(T), scope);
        }

        /// <summary>
        /// Resolve dependency
        /// </summary>
        /// <param name="type">Type of resolved service</param>
        /// <param name="scope">Scope</param>
        /// <returns>Resolved service</returns>
        public virtual object Resolve(Type type, IServiceScope scope = null)
        {
            return GetServiceProvider(scope)?.GetService(type);
        }

        /// <summary>
        /// Resolve dependencies
        /// </summary>
        /// <typeparam name="T">Type of resolved services</typeparam>
        /// <returns>Collection of resolved services</returns>
        public virtual IEnumerable<T> ResolveAll<T>()
        {
            return (IEnumerable<T>)GetServiceProvider().GetServices(typeof(T));
        }

        /// <summary>
        /// Resolve unregistered service
        /// </summary>
        /// <param name="type">Type of service</param>
        /// <returns>Resolved service</returns>
        public virtual object ResolveUnregistered(Type type)
        {
            Exception innerException = null;
            foreach (var constructor in type.GetConstructors())
            {
                try
                {
                    //try to resolve constructor parameters
                    var parameters = constructor.GetParameters().Select(parameter =>
                    {
                        var service = Resolve(parameter.ParameterType);
                        if (service == null)
                            throw new Exception("Unknown dependency");
                        return service;
                    });

                    //all is ok, so create instance
                    return Activator.CreateInstance(type, parameters.ToArray());
                }
                catch (Exception ex)
                {
                    innerException = ex;
                }
            }

            throw new Exception("No constructor was found that had all the dependencies satisfied.", innerException);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Service provider
        /// </summary>
        public virtual IServiceProvider ServiceProvider { get; protected set; }

        #endregion
    }
}