using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Base.Web.Framework.MVC.ModelBinding
{
    public class BaseResourceDisplayNameAttribute:DisplayNameAttribute, IModelAttribute
    {
       

        private string _resourceValue = string.Empty;

        public BaseResourceDisplayNameAttribute(string resourceKey) : base(resourceKey)
        {
            ResourceKey = resourceKey;
        }

       
        public string ResourceKey { get; set; }

        
        public override string DisplayName
        {
            get
            {
                
                return "AAAAA";
            }
        }

        public string Name => nameof(BaseResourceDisplayNameAttribute);
    }
}
