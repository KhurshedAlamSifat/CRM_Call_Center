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
using Base.Core.Domain.Brands;
using Base.Core.Domain.BusinessUnits;

namespace Base.Services.Brands
{
    public class BrandService : IBrandService
    {
        private readonly IRepository<Brand> _brandRepo;

        private readonly IStaticCacheManager _staticCacheManager;

        public BrandService(IRepository<Brand> brandRepo, IStaticCacheManager staticCacheManager)
        {
            _brandRepo = brandRepo;
            _staticCacheManager = staticCacheManager;
        }



        #region Usuarios
        public async Task DeleteBrandAsync(Brand brand)
        {
            await _brandRepo.DeleteAsync(brand);
        }

        public virtual async Task UpdateBrandAsync(Brand brand)
        {
            await _brandRepo.UpdateAsync(brand);
        }


        public virtual async Task InsertBrandAsync(Brand brand)
        {
            await _brandRepo.InsertAsync(brand);
        }

        public virtual async Task<IList<Brand>> GetAllBrandByBusinessUnitAsync(int businessId = 0)
        {
            var businessUnits = await _brandRepo.GetAllAsync(query =>
            {
                if (businessId > 0)
                    query = query.Where(c => c.BusinessUnit_Id == businessId);

                query = query.OrderByDescending(c => c.Id);

                return query;
            });

            return await businessUnits.ToListAsync();
        }

        public virtual async Task<IPagedList<Brand>> GetAllBrandPaggedAsync(string brandName, int pageIndex = 0, int pageSize = int.MaxValue, bool getOnlyTotalCount = false)
        {
            var businessUnits = await _brandRepo.GetAllPagedAsync(query =>
            {
                if (!string.IsNullOrEmpty(brandName))
                    query = query.Where(c => c.BrandName == brandName);

                query = query.OrderByDescending(c => c.Id);

                return query;
            }, pageIndex, pageSize, getOnlyTotalCount);

            return businessUnits;
        }

        public virtual async Task<Brand> GetBrandByIdAsync(int id)
        {
            if (id == 0)
                return null;

            return await _brandRepo.GetByIdAsync(id);
        }

        public virtual async Task<IList<Brand>> GetAllBrandAsync()
        {
            var brands = await _brandRepo.GetAllAsync(query =>
            {
                query = query.OrderByDescending(c => c.Id);

                return query;
            });

            return await brands.ToListAsync();
        }

        #endregion

    }
}
