using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.Core.Caching;
using Base.Core.Domain.Categories;
using Base.Core;
using Base.Data;
using Base.Core.Domain.Brands;

namespace Base.Services.Categories
{
    public class CategoryService : ICategoryService
    {
        private readonly IRepository<Category> _categoryRepo;

        private readonly IStaticCacheManager _staticCacheManager;

        public CategoryService(IRepository<Category> categoryRepo, IStaticCacheManager staticCacheManager)
        {
            _categoryRepo = categoryRepo;
            _staticCacheManager = staticCacheManager;
        }



        #region Usuarios
        public async Task DeleteCategoryAsync(Category category)
        {
            await _categoryRepo.DeleteAsync(category);
        }

        public virtual async Task UpdateCategoryAsync(Category category)
        {
            await _categoryRepo.UpdateAsync(category);
        }


        public virtual async Task InsertCategoryAsync(Category category)
        {
            await _categoryRepo.InsertAsync(category);
        }

        public virtual async Task<IList<Category>> GetAllCategoryByBrandAsync(int brandId = 0)
        {
            var brands = await _categoryRepo.GetAllAsync(query =>
            {
                if (brandId > 0)
                    query = query.Where(c => c.Brand_Id == brandId);

                query = query.OrderByDescending(c => c.Id);

                return query;
            });

            return await brands.ToListAsync();
        }

        public virtual async Task<IPagedList<Category>> GetAllCategoryPaggedAsync(string categoryName, int pageIndex = 0, int pageSize = int.MaxValue, bool getOnlyTotalCount = false)
        {
            var brands = await _categoryRepo.GetAllPagedAsync(query =>
            {
                if (!string.IsNullOrEmpty(categoryName))
                    query = query.Where(c => c.CategoryName == categoryName);

                query = query.OrderByDescending(c => c.Id);

                return query;
            }, pageIndex, pageSize, getOnlyTotalCount);

            return brands;
        }

        public virtual async Task<Category> GetCategoryByIdAsync(int id)
        {
            if (id == 0)
                return null;

            return await _categoryRepo.GetByIdAsync(id);
        }

        public virtual async Task<IList<Category>> GetAllCategoryAsync()
        {
            var categories = await _categoryRepo.GetAllAsync(query =>
            {
                query = query.OrderByDescending(c => c.Id);

                return query;
            });

            return await categories.ToListAsync();
        }

        #endregion

    }
}
