using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;

namespace Base.Services.Notification
{
    /// <summary>
    /// Notification service
    /// </summary>
    public partial class NotificationService : INotificationService
    {
        #region Fields

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ITempDataDictionaryFactory _tempDataDictionaryFactory;

        #endregion

        #region Ctor

        public NotificationService(IHttpContextAccessor httpContextAccessor,
            ITempDataDictionaryFactory tempDataDictionaryFactory)
        {
            _httpContextAccessor = httpContextAccessor;
            _tempDataDictionaryFactory = tempDataDictionaryFactory;
        }

        #endregion

        #region Utilities

        /// <summary>
        /// Save message into TempData
        /// </summary>
        /// <param name="type">Notification type</param>
        /// <param name="message">Message</param>
        /// <param name="encode">A value indicating whether the message should not be encoded</param>
        protected virtual void PrepareTempData(NotifyType type, string message, bool encode = true)
        {
            var context = _httpContextAccessor.HttpContext;
            var tempData = _tempDataDictionaryFactory.GetTempData(context);

            //Messages have stored in a serialized list
            var messages = tempData.ContainsKey(MapMessageDefaults.NotificationListKey)
                ? JsonConvert.DeserializeObject<IList<NotifyData>>(tempData[MapMessageDefaults.NotificationListKey].ToString())
                : new List<NotifyData>();

            messages.Add(new NotifyData
            {
                Message = message,
                Type = type,
                Encode = encode
            });

            tempData[MapMessageDefaults.NotificationListKey] = JsonConvert.SerializeObject(messages);
        }

   
        #endregion

        #region Methods

        /// <summary>
        /// Display notification
        /// </summary>
        /// <param name="type">Notification type</param>
        /// <param name="message">Message</param>
        /// <param name="encode">A value indicating whether the message should not be encoded</param>
        public virtual void Notification(NotifyType type, string message, bool encode = true)
        {
            PrepareTempData(type, message, encode);
        }

        /// <summary>
        /// Display success notification
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="encode">A value indicating whether the message should not be encoded</param>
        public virtual void SuccessNotification(string message, bool encode = true)
        {
            PrepareTempData(NotifyType.Success, message, encode);
        }

        /// <summary>
        /// Display warning notification
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="encode">A value indicating whether the message should not be encoded</param>
        public virtual void WarningNotification(string message, bool encode = true)
        {
            PrepareTempData(NotifyType.Warning, message, encode);
        }

        /// <summary>
        /// Display error notification
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="encode">A value indicating whether the message should not be encoded</param>
        public virtual void ErrorNotification(string message, bool encode = true)
        {
            PrepareTempData(NotifyType.Error, message, encode);
        }

        /// <summary>
        /// Display error notification
        /// </summary>
        /// <param name="exception">Exception</param>
        /// <param name="logException">A value indicating whether exception should be logged</param>
        public virtual void ErrorNotification(Exception exception, bool logException = true)
        {
            if (exception == null)
                return;

            
            ErrorNotification(exception.Message);
        }

        #endregion
    }
}
