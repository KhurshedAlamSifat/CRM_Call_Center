using Base.Core.Domain.ServiceCenters;
using Base.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Services.ServiceCenters
{
    public interface IServiceCenterService
    {
        Task<ServiceCenter> GetServiceCenterAsync(int id);
        Task<IPagedList<ServiceCenter>> GetAllServiceCenterAsync(string serviceCentername = null, int thana = 0, int pageIndex = 0, int pageSize = int.MaxValue, bool getOnlyTotalCount = false);
        Task<List<ServiceCenter>> GetServiceCenterAsync();
        Task InsertServiceCenterAsync(ServiceCenter serviceCenterName);
        Task UpdateServiceCenterAsync(ServiceCenter ServiceCenter);
        Task DeleteServiceCenterAsync(ServiceCenter ServiceCenter);
        Task<List<ServiceCenter>> GetServiceCenterByThanaAsync(int thana);
    }
}
    