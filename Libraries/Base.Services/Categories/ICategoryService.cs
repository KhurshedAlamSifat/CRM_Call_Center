using Base.Core;
using Base.Core.Domain.Categories;
using Base.Core.Domain.BusinessUnits;
using Base.Core.Domain.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using Base.Core.Domain.Brands;

namespace Base.Services.Categories
{
    public interface ICategoryService
    {
        Task DeleteCategoryAsync(Category category);
        Task UpdateCategoryAsync(Category category);
        Task InsertCategoryAsync(Category category);
        Task<IList<Category>> GetAllCategoryByBrandAsync(int brandId = 0);
        Task<Category> GetCategoryByIdAsync(int id);
        Task<IList<Category>> GetAllCategoryAsync();
        Task<IPagedList<Category>> GetAllCategoryPaggedAsync(string categoryName, int pageIndex = 0, int pageSize = int.MaxValue, bool getOnlyTotalCount = false);
    }
}
