using Base.Core.Caching;
using Base.Core.Domain.Users;

namespace Base.Services.Users
{
    /// <summary>
    /// Represents default values related to user services
    /// </summary>
    public static partial class CrmUserServicesDefaults
    {

        #region Caching defaults

        #region User

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : system name
        /// </remarks>
        public static CacheKey UserBySystemNameCacheKey => new("Crm.user.bysystemname.{0}");

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : user GUID
        /// </remarks>
        public static CacheKey UserByGuidCacheKey => new("Crm.user.byguid.{0}");

        #endregion

        #region User roles

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : show hidden records?
        /// </remarks>
        public static CacheKey UserRolesAllCacheKey => new("Crm.userrole.all.{0}", CrmEntityCacheDefaults<UserRole>.AllPrefix);

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : system name
        /// </remarks>
        public static CacheKey UserRolesBySystemNameCacheKey => new("Crm.userrole.bysystemname.{0}", UserRolesBySystemNamePrefix);

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string UserRolesBySystemNamePrefix => "Crm.customerrole.bysystemname.";

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : customer identifier
        /// {1} : show hidden
        /// </remarks>
        public static CacheKey UserRoleIdsCacheKey => new("Crm.user.userrole.ids.{0}-{1}", UserUserRolesPrefix);

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : customer identifier
        /// {1} : show hidden
        /// </remarks>
        public static CacheKey UserRolesCacheKey => new("Crm.user.userrole.{0}-{1}", UserUserRolesByUserPrefix, UserUserRolesPrefix);

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string UserUserRolesPrefix => "Crm.user.userrole.";

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        /// <remarks>
        /// {0} : customer identifier
        /// </remarks>
        public static string UserUserRolesByUserPrefix => "Crm.user.userrole.{0}";
      
        #endregion

        #region Addresses

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : customer identifier
        /// </remarks>
        public static CacheKey UserAddressesCacheKey => new("Crm.user.addresses.{0}", UserAddressesPrefix);

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : customer identifier
        /// {1} : address identifier
        /// </remarks>
        public static CacheKey UserAddressCacheKey => new("Crm.user.addresses.{0}-{1}", UserAddressesByUserPrefix, UserAddressesPrefix);

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string UserAddressesPrefix => "Crm.user.addresses.";

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        /// <remarks>
        /// {0} : customer identifier
        /// </remarks>
        public static string UserAddressesByUserPrefix => "Crm.user.addresses.{0}";
        
        #endregion

        #region Customer password

        /// <summary>
        /// Gets a key for caching current customer password lifetime
        /// </summary>
        /// <remarks>
        /// {0} : customer identifier
        /// </remarks>
        public static CacheKey UserPasswordLifetimeCacheKey => new("Crm.userpassword.lifetime.{0}");

        #endregion

        #endregion

    }
}