using Base.Core;
using Base.Core.Domain.Brands;
using Base.Core.Domain.Brands;
using Base.Core.Domain.BusinessUnits;
using Base.Core.Domain.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace Base.Services.Brands
{
    public interface IBrandService
    {
        Task DeleteBrandAsync(Brand brand);
        Task UpdateBrandAsync(Brand brand);
        Task InsertBrandAsync(Brand brand);
        Task<IList<Brand>> GetAllBrandByBusinessUnitAsync(int businessId = 0);
        Task<Brand> GetBrandByIdAsync(int id);
        Task<IPagedList<Brand>> GetAllBrandPaggedAsync(string brandName, int pageIndex = 0, int pageSize = int.MaxValue, bool getOnlyTotalCount = false);
      //------test
        Task<IList<Brand>> GetAllBrandAsync();
    }
}

