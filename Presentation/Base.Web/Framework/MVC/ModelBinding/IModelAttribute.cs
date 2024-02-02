using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Base.Web.Framework.MVC.ModelBinding
{
    public interface IModelAttribute
    {
        /// <summary>
        /// Gets name of the attribute
        /// </summary>
        string Name { get; }
    }
}
