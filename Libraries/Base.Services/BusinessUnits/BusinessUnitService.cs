using System;
using System.Collections.Generic;
using System.Text;
using Base.Data;
using Base.Services.Security;
using System.Collections.Immutable;
using Base.Core.Domain.Users;
using System.Threading.Tasks;
using Base.Core.Caching;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using System.Data;
using Base.Core;
using DocumentFormat.OpenXml.Bibliography;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using Base.Services.Authentication;
using Base.Core.Domain.BusinessUnits;
using DocumentFormat.OpenXml.Vml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using Irony.Parsing;

namespace Base.Services.BusinessUnits
{
    public class BusinessUnitService : IBusinessUnitService
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _environment;
        private readonly IRepository<BusinessUnit> _businessUnitRepo;
        private readonly IStaticCacheManager _staticCacheManager;
        private readonly IRepository<UserUserRoleMapping> _userUserRoleRepo;

        public BusinessUnitService(IConfiguration configuration, IWebHostEnvironment environment,
            IRepository<BusinessUnit> businessUnitRepo,IStaticCacheManager staticCacheManager)
        {
            _configuration = configuration;
            _environment = environment;
            _businessUnitRepo = businessUnitRepo;
            _staticCacheManager = staticCacheManager;
        }



        #region Usuarios
        public async Task DeleteBusinessUnitAsync(BusinessUnit businessUnit)
        {
            await _businessUnitRepo.DeleteAsync(businessUnit);
        }

        public virtual async Task UpdateBusinessUnitAsync(BusinessUnit businessUnit)
        {
            await _businessUnitRepo.UpdateAsync(businessUnit);
        }


        public virtual async Task InsertBusinessUnitAsync(BusinessUnit businessUnit)
        {
            await _businessUnitRepo.InsertAsync(businessUnit);
        }

        public virtual async Task<IList<BusinessUnit>> GetAllBusinessUnitAsync()
        {
            var businessUnits = await _businessUnitRepo.GetAllAsync(query =>
            {
                query = query.OrderByDescending(c => c.Id);

                return query;
            });

            return await businessUnits.ToListAsync();
        }

        public virtual async Task<IPagedList<BusinessUnit>> GetAllBusinessUnitPaggedAsync(string businessUnitName, int pageIndex = 0, int pageSize = int.MaxValue, bool getOnlyTotalCount = false)
        {
            var businessUnits = await _businessUnitRepo.GetAllPagedAsync(query =>
            {
                if(!string.IsNullOrEmpty(businessUnitName))
                    query = query.Where(c => c.BusinessUnitName== businessUnitName);

                query = query.OrderByDescending(c => c.Id);

                return query;
            }, pageIndex, pageSize, getOnlyTotalCount);

            return businessUnits;
        }

        public virtual async Task<BusinessUnit> GetBusinessUnitByIdAsync(int id)
        {
            if (id == 0)
                return null;

            return await _businessUnitRepo.GetByIdAsync(id);
        }

        #endregion

    }
}
