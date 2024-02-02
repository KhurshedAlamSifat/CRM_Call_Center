
using Base.Core;
using Base.Core.Caching;
using Base.Core.Domain.Districts;
using Base.Core.Domain.Thanas;
using Base.Core.Domain.Users;
using Base.Data;
using Base.Services.Security;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Services.Thanas
{
    public class ThanaService : IThanaService
    {
        private readonly IRepository<Thana> _thanaRepository;
        private readonly IStaticCacheManager _staticCacheManager;

        public ThanaService(IRepository<Thana> thanaRepository, IStaticCacheManager staticCacheManager)
        {
            _thanaRepository = thanaRepository;
            _staticCacheManager = staticCacheManager;
        }

        public ThanaService() { }

        public async Task DeleteThanaAsync(Thana thana)
        {
            await _thanaRepository.DeleteAsync(thana);

            var keyByDistrict = _staticCacheManager.PrepareKeyForDefaultCache(new("Crm.thana.all-{0}"),thana.District_Id);
            await _staticCacheManager.RemoveAsync(keyByDistrict);
        }

        public async Task<IPagedList<Thana>> GetAllThanaAsync(string thananame = null, int district = 0, int pageIndex = 0, int pageSize = int.MaxValue, bool getOnlyTotalCount = false)
        {
            var thana = await _thanaRepository.GetAllPagedAsync(query =>
            {
                if (district > 0)
                    query = query.Where(d => d.District_Id == district);
                if (!string.IsNullOrEmpty(thananame))
                    query = query.Where(d => d.ThanaName == thananame);
                query = query.OrderByDescending(c => c.Id);

                return query;
            }, pageIndex, pageSize, getOnlyTotalCount);

            return thana;
        }

        public async Task InsertThanaAsync(Thana thanaName)
        {
            await _thanaRepository.InsertAsync(thanaName);
            var keyByDistrict = _staticCacheManager.PrepareKeyForDefaultCache(new("Crm.thana.all-{0}"), thanaName.District_Id);
            await _staticCacheManager.RemoveAsync(keyByDistrict);

        }

        public async Task UpdateThanaAsync(Thana thana)
        {
            await _thanaRepository.UpdateAsync(thana);
            var keyByDistrict = _staticCacheManager.PrepareKeyForDefaultCache(new("Crm.thana.all-{0}"), thana.District_Id);
            await _staticCacheManager.RemoveAsync(keyByDistrict);

        }

        public async Task<Thana> GetThanaAsync(int id)
        {
            if (id == 0)
                return null;

            var thana = await _thanaRepository.GetByIdAsync(id);

            return thana;
        }

        public async Task<List<Thana>> GetThanaAsync()
        {
            var thanas = await _thanaRepository.GetAllAsync(query =>
            {
                query = query.OrderByDescending(c => c.Id);

                return query;
            });

            return await thanas.ToListAsync();
        }

        public async Task<List<Thana>> GetThanaByDistrictAsync(int districtId)
        {
            var key = _staticCacheManager.PrepareKeyForDefaultCache(new("Crm.thana.all-{0}"), districtId);
            var thanas = await _staticCacheManager.GetAsync(key, async () =>
            {
                return await _thanaRepository.Table.Where(t => t.District_Id == districtId).ToListAsync();

            });
            return thanas;
        }

       
    }
}
