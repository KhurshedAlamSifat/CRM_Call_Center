using System.Collections.Generic;
using System.Xml.Serialization;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Base.Web.Framework.Models
{
    public partial class BaseCrmModel
    {
        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        public BaseCrmModel()
        {
            CustomProperties = new Dictionary<string, object>();
            PostInitialize();
        }

        #endregion

        #region Methods
        public virtual void BindModel(ModelBindingContext bindingContext)
        {
        }

        /// <summary>
        /// Perform additional actions for the model initialization
        /// </summary>
        /// <remarks>Developers can override this method in custom partial classes in order to add some custom initialization code to constructors</remarks>
        protected virtual void PostInitialize()
        {
        }

        #endregion

        #region Properties

        ////MVC is suppressing further validation if the IFormCollection is passed to a controller method. That's why we add it to the model
        //[XmlIgnore]
        //public IFormCollection Form { get; set; }

        /// <summary>
        /// Gets or sets property to store any custom values for models 
        /// </summary>
        [XmlIgnore]
        public Dictionary<string, object> CustomProperties { get; set; }

        #endregion

    }
}