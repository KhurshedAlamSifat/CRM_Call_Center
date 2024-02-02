using Base.Core;
using Base.Core.Caching;
using Base.Core.Domain.Districts;
using Base.Core.Domain.Users;
using Base.Data;
using Base.Services.Security;
using DocumentFormat.OpenXml.Vml.Spreadsheet;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Base.Services.Districts
{
    public class DistrictService : IDistrictService
    {
        private readonly IRepository<District> _districtRepository;
        private readonly IStaticCacheManager _staticCacheManager;
        public DistrictService(IRepository<District> districtRepository, IStaticCacheManager staticCacheManager)
        {
            _districtRepository = districtRepository;
            _staticCacheManager = staticCacheManager;
        }


        public virtual async Task<IPagedList<District>> GetAllDistrictsAsync(string district = null, int pageIndex = 0, int pageSize = int.MaxValue, bool getOnlyTotalCount = false)
        {
            var districts = await _districtRepository.GetAllPagedAsync(query =>
            {
                if (!string.IsNullOrEmpty(district))
                    query = query.Where(d => d.DistrictName == district);
                query = query.OrderByDescending(c => c.Id);

                return query;
            }, pageIndex, pageSize, getOnlyTotalCount);

            return districts;

        }

        public async Task<District> GetDistrictAsync(int id)
        {
            var district = await _districtRepository.GetByIdAsync(id);

            return district;
        }

        public async Task InsertDistrictAsync(District districtName)
        {
            await _districtRepository.InsertAsync(districtName);
        }

        public async Task UpdateDistrictAsync(District district)
        {
            await _districtRepository.UpdateAsync(district);
        }

        public async Task DeleteDistrictAsync(District district)
        {
            await _districtRepository.DeleteAsync(district);
        }

        public async Task<List<District>> GetAllDistrictWithoutPaggingAsync()
        {
            var districts = await _districtRepository.GetAllAsync(query => 
            {
                return query.OrderBy(x=>x.Id);
            });
            return await districts.ToListAsync();
        }
    }
}
