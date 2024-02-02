using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using Autofac.Extensions.DependencyInjection;
using Base.Core.Configuration;
using Base.Core.Infrastructure;
using Base.Web.Framework.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile(CrmConfigurationDefaults.AppSettingsFilePath, true, true);
if (!string.IsNullOrEmpty(builder.Environment?.EnvironmentName))
{
    var path = string.Format(CrmConfigurationDefaults.AppSettingsEnvironmentFilePath, builder.Environment.EnvironmentName);
    builder.Configuration.AddJsonFile(path, true, true);
}
builder.Configuration.AddEnvironmentVariables();

//load application settings
builder.Services.ConfigureApplicationSettings(builder);

var appSettings = Singleton<AppSettings>.Instance;
var useAutofac = appSettings.Get<CommonConfig>().UseAutofac;

if (useAutofac)
    builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
else
    builder.Host.UseDefaultServiceProvider(options =>
    {
        //we don't validate the scopes, since at the app start and the initial configuration we need 
        //to resolve some services (registered as "scoped") through the root container
        options.ValidateScopes = false;
        options.ValidateOnBuild = true;
    });

builder.Services.ConfigureApplicationServices(builder);

var app = builder.Build();
app.ConfigureRequestPipeline();

app.Run();


