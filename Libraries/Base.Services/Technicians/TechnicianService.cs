using Base.Core;
using Base.Core.Caching;
using Base.Core.Domain.ServiceCenters;
using Base.Core.Domain.Technicians;
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

namespace Base.Services.Technicians
{


    public class TechnicianService : ITechnicianService
    {
        private readonly IEncryptionService _encryptionService;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _environment;
        private readonly IRepository<Technician> _technicianRepository;
        private readonly IStaticCacheManager _staticCacheManager;
        public TechnicianService(
            IEncryptionService encryptionService,
            IConfiguration configuration,
            IWebHostEnvironment environment,
            IRepository<Technician> technicianRepository,
            IStaticCacheManager staticCacheManager)
        {
            this._encryptionService = encryptionService;
            _configuration = configuration;
            _environment = environment;
            _technicianRepository = technicianRepository;
            _staticCacheManager = staticCacheManager;
        }


        public TechnicianService() { }

        public async Task DeleteTechnicianAsync(Technician technician)
        {
            await _technicianRepository.DeleteAsync(technician);
            var key = _staticCacheManager.PrepareKeyForDefaultCache(new("Crm.technician.all"));
            await _staticCacheManager.RemoveAsync(key);
        }

        public async Task<IPagedList<Technician>> GetAllTechnicianAsync( string technicianname = null,int thana = 0,int serviceCenter = 0, 
            int categoryId=0,int pageIndex = 0,int pageSize = int.MaxValue,bool getOnlyTotalCount = false)
        {
            var technicians = await _technicianRepository.GetAllPagedAsync(query =>
            {
                if (thana > 0)
                    query = query.Where(t => t.Thana_Id == thana);

                if (serviceCenter > 0)
                    query = query.Where(t => t.ServiceCenter_Id == serviceCenter);

                if (categoryId > 0)
                    query = query.Where(t => t.Category_Id == categoryId);

                if (!string.IsNullOrEmpty(technicianname))
                    query = query.Where(t => t.TechnicianName == technicianname);

                query = query.OrderByDescending(t => t.Id);

                return query;
            }, pageIndex, pageSize, getOnlyTotalCount);


            return technicians;
        }



        public async Task<Technician> GetTechnicianAsync(int id)
        {
            if (id == 0)
            {
                return null;
            }

            return await _technicianRepository.GetByIdAsync(id);
        }

        public async Task<List<Technician>> GetTechnicianAsync()
        {
            var technician = await _technicianRepository.GetAllAsync(query =>
            {
                query = query.OrderBy(t => t.Id);
                return query;
            });
            return await technician.ToListAsync();
        }

        public async  Task<List<Technician>> GetTechnicianByThanaAsynce(int thana)
        {
            var key = _staticCacheManager.PrepareKeyForDefaultCache(new("Crm.technician.allbythana-{0}"), thana);
            var query = _technicianRepository.Table;
            var technician = await _staticCacheManager.GetAsync(key, async () =>
            {
                query = query.Where(t => t.Thana_Id == thana);
                return await query.ToListAsync();

            });
            return technician;
        }

        public async Task<List<Technician>> GetTechnicianByServiceCenterAsynce(int serviceCenter)
        {
            var key = _staticCacheManager.PrepareKeyForDefaultCache(new("Crm.technician.allservicecenter-{0}"), serviceCenter);
            var query = _technicianRepository.Table;
            var technician = await _staticCacheManager.GetAsync(key, async () =>
            {
                if (serviceCenter > 0)
                {
                    query = query.Where(t => t.ServiceCenter_Id == serviceCenter);
                }
                return await query.ToListAsync();
            });
            return technician;
        }

        public async Task InsertTechnicianAsync(Technician technicianName)
        {
            await _technicianRepository.InsertAsync(technicianName);
       }

        public async Task UpdateTechnicianAsync(Technician technician)
        {
            await _technicianRepository.UpdateAsync(technician);      
        }
    }
}
