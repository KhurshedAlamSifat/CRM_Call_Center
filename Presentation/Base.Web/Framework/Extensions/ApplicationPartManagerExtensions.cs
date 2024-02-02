using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using Base.Core;
using Base.Core.Infrastructure;
using Microsoft.AspNetCore.Mvc.ApplicationParts;


namespace Base.Web.Framework.Extensions
{
    /// <summary>
    /// Represents application part manager extensions
    /// </summary>
    public static partial class ApplicationPartManagerExtensions
    {
        #region Fields

        private static readonly ICrmFileProvider _fileProvider;
        private static readonly List<KeyValuePair<string, Version>> _baseAppLibraries;
        private static readonly ReaderWriterLockSlim _locker = new();

        #endregion

        #region Ctor

        static ApplicationPartManagerExtensions()
        {
            //we use the default file provider, since the DI isn't initialized yet
            _fileProvider = CommonHelper.DefaultFileProvider;

            _baseAppLibraries = new List<KeyValuePair<string, Version>>();
            
            //get all libraries from /bin/{version}/ directory
            foreach (var file in _fileProvider.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll")) 
                _baseAppLibraries.Add(new KeyValuePair<string, Version>(_fileProvider.GetFileName(file), GetAssemblyVersion(file)));

            //get all libraries from base site directory
            if (!AppDomain.CurrentDomain.BaseDirectory.Equals(Environment.CurrentDirectory, StringComparison.InvariantCultureIgnoreCase))
                foreach (var file in _fileProvider.GetFiles(Environment.CurrentDirectory, "*.dll"))
                    _baseAppLibraries.Add(new KeyValuePair<string, Version>(_fileProvider.GetFileName(file), GetAssemblyVersion(file)));
        }

        #endregion

        #region Utilities

        private static Version GetAssemblyVersion(string filePath)
        {
            try
            {
                return AssemblyName.GetAssemblyName(filePath).Version;
            }
            catch (BadImageFormatException)
            {
                //ignore
            }

            return null;
        }

        /// <summary>
        /// Load and register the assembly
        /// </summary>
        /// <param name="applicationPartManager">Application part manager</param>
        /// <param name="assemblyFile">Path to the assembly file</param>
        /// <param name="useUnsafeLoadAssembly">Indicating whether to load an assembly into the load-from context, bypassing some security checks</param>
        /// <returns>Assembly</returns>
        private static Assembly AddApplicationParts(ApplicationPartManager applicationPartManager, string assemblyFile, bool useUnsafeLoadAssembly)
        {
            //try to load a assembly
            Assembly assembly;

            try
            {
                assembly = Assembly.LoadFrom(assemblyFile);
            }
            catch (FileLoadException)
            {
                if (useUnsafeLoadAssembly)
                {
                    //if an application has been copied from the web, it is flagged by Windows as being a web application,
                    //even if it resides on the local computer.You can change that designation by changing the file properties,
                    //or you can use the<loadFromRemoteSources> element to grant the assembly full trust.As an alternative,
                    //you can use the UnsafeLoadFrom method to load a local assembly that the operating system has flagged as
                    //having been loaded from the web.
                    //see http://go.microsoft.com/fwlink/?LinkId=155569 for more information.
                    assembly = Assembly.UnsafeLoadFrom(assemblyFile);
                }
                else
                    throw;
            }

            //register the plugin definition
            applicationPartManager.ApplicationParts.Add(new AssemblyPart(assembly));

            return assembly;
        }

        #endregion

    }
}
