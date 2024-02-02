using Base.Core.Caching;
using Base.Core.Domain.Security;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Base.Services.Security
{
    public static partial class SecurityDefaults
    {        
        public static string EncryptionKey => "9957130100808112"; //llave al random puede cambiar

        public static CacheKey PermissionRecordsAllCacheKey => new("Crm.permissionrecord.all.{0}", CrmEntityCacheDefaults<PermissionRecord>.AllPrefix);

    }
}
