using Base.Core;
using Base.Core.Caching;
using Base.Core.Domain.ServiceCenters;
using Base.Core.Domain.Thanas;
using Base.Data;
using Base.Services.Security;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Services.ServiceCenters
{
    public class ServiceCenterService : IServiceCenterService
    {

        private readonly IEncryptionService _encryptionService;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _environment;
        private readonly IRepository<ServiceCenter> _serviceCenterRepository;
        private readonly IStaticCacheManager _staticCacheManager;
        public ServiceCenterService(IEncryptionService encryptionService, IConfiguration configuration,
            IWebHostEnvironment environment, IRepository<ServiceCenter> serviceCenterRepository, IStaticCacheManager staticCacheManager)
        {
            _encryptionService = encryptionService;
            _configuration = configuration;
            _environment = environment;
            _serviceCenterRepository = serviceCenterRepository;
            _staticCacheManager = staticCacheManager;
        }

        public async Task DeleteServiceCenterAsync(ServiceCenter serviceCenter)
        {
            await _serviceCenterRepository.DeleteAsync(serviceCenter);
        }

        public async Task<IPagedList<ServiceCenter>> GetAllServiceCenterAsync(string serviceCentername = null, int thana = 0, int pageIndex = 0, int pageSize = int.MaxValue, bool getOnlyTotalCount = false)
        {
            var serviceCenter = await _serviceCenterRepository.GetAllPagedAsync(query =>
            {
                if (thana > 0)
                    query = query.Where(d => d.Thana_Id == thana);
                if (!string.IsNullOrEmpty(serviceCentername))
                    query = query.Where(d => d.ServiceCenterName == serviceCentername);
                query = query.OrderByDescending(c => c.Id);

                return query;
            }, pageIndex, pageSize, getOnlyTotalCount);

            return serviceCenter;
        }

        public async Task InsertServiceCenterAsync(ServiceCenter serviceCenterName)
        {
            await _serviceCenterRepository.InsertAsync(serviceCenterName);
        }

        public async Task UpdateServiceCenterAsync(ServiceCenter serviceCenter)
        {
            await _serviceCenterRepository.UpdateAsync(serviceCenter);
        }

        public async Task<ServiceCenter> GetServiceCenterAsync(int id)
        {
            if (id == 0)
                return null;

            var serviceCenter = await _serviceCenterRepository.GetByIdAsync(id);

            return serviceCenter;
        }

        public async Task<List<ServiceCenter>> GetServiceCenterAsync()
        {
            var serviceCenter = await _serviceCenterRepository.GetAllAsync(query =>
            {
                query = query.OrderByDescending(c => c.Id);

                return query;
            });
            return await serviceCenter.ToListAsync();
        }

        public async Task<List<ServiceCenter>> GetServiceCenterByThanaAsync(int thana)
        {
            var key = _staticCacheManager.PrepareKeyForDefaultCache(new("Crm.servicecenter.allbythana-{0}"), thana);
            var query = _serviceCenterRepository.Table;

            var serviceCenter = await _staticCacheManager.GetAsync(key, async () =>
            {
                query = query.Where(t => t.Thana_Id == thana);
                return await query.ToListAsync();

            });
            return serviceCenter;
        }
    }
}
