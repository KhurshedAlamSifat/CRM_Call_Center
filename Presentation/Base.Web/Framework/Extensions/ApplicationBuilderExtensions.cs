using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.ExceptionServices;
using Base.Core;
using Base.Core.Configuration;
using Base.Core.Infrastructure;
using Base.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Net.Http.Headers;
using QuestPDF.Drawing;
using WebMarkupMin.AspNetCore7;

namespace Base.Web.Framework.Extensions
{
    /// <summary>
    /// Represents extensions of IApplicationBuilder
    /// </summary>
    public static class ApplicationBuilderExtensions
    {
        public static void ConfigureRequestPipeline(this IApplicationBuilder application)
        {
            EngineContext.Current.ConfigureRequestPipeline(application);

            application.UseResponseCompression();
            application.UseWebOptimizer();
            application.UseStaticFiles();
            application.UseSession();
            application.UseNopPdf();
            application.UseHttpsRedirection();
            application.UseWebMarkupMin();
            application.UseRouting();
            application.UseMiddleware<AuthenticationMiddleware>();
            application.UseAuthorization();
            application.RegisterRoutes();

        }
        public static void UseNopPdf(this IApplicationBuilder application)
        {

            var fileProvider = EngineContext.Current.Resolve<ICrmFileProvider>();
            var fontPaths = fileProvider.EnumerateFiles(fileProvider.MapPath("~/App_Data/Pdf/"), "*.ttf") ?? Enumerable.Empty<string>();

            //write placeholder characters instead of unavailable glyphs for both debug/release configurations
            QuestPDF.Settings.CheckIfAllTextGlyphsAreAvailable = false;

            foreach (var fp in fontPaths)
            {
                FontManager.RegisterFont(File.OpenRead(fp));
            }
        }

        public static void RegisterRoutes(this IApplicationBuilder application)
        {


            application.UseEndpoints(endpoints =>
            {
                var pattern = string.Empty;

                //areas
                endpoints.MapControllerRoute(name: "areaRoute",
                pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");


                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=User}/{action=Login}/{id?}");

                endpoints.MapControllerRoute("/", pattern,
                new { controller = "User", action = "Login" });

                endpoints.MapControllerRoute(name: "ValidateAccessToken",
                pattern: $"validaciondeidentidad",
                defaults: new { controller = "User", action = "ValidateAccessToken" });


                endpoints.MapControllerRoute(name: "Login",
                pattern: $"Iniciasesion/",
                defaults: new { controller = "User", action = "Login" });

                endpoints.MapControllerRoute(name: "Registrate",
                pattern: $"Registrate/",
                defaults: new { controller = "User", action = "Registrate" });

                endpoints.MapControllerRoute(name: "CallCenterAgentPage",
               pattern: $"AgentAccount/",
               defaults: new { controller = "User", action = "AgentAccount" });

                endpoints.MapControllerRoute(name: "RegisterResult",
                pattern: $"registerresult/{{resultId:min(0)}}",
                defaults: new { controller = "User", action = "RegisterResult" });

                endpoints.MapControllerRoute(name: "TicketEntry",
                pattern: $"ticket/{{phonenumber}}",
                defaults: new { controller = "Ticket", action = "TicketEntryForm" });


                endpoints.MapControllerRoute(
                    name: "Error",
                    pattern: $"error",
                    defaults: new { controller = "Common", action = "Error" });

                endpoints.MapControllerRoute(
                    name: "PageNotFound",
                    pattern: $"page-not-found",
                    defaults: new { controller = "Common", action = "PageNotFound" });


                endpoints.MapControllerRoute("Logout", $"logout/",
                new { controller = "User", action = "Logout" });

            });
        }
    }
}
