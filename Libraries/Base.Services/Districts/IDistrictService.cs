using Base.Core;
using Base.Core.Domain.Districts;
using Base.Core.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Services.Districts
{
    public interface IDistrictService
    {
        Task<District> GetDistrictAsync(int id);
        Task<IPagedList<District>> GetAllDistrictsAsync(string district = null,int pageIndex = 0, int pageSize = int.MaxValue, bool getOnlyTotalCount = false);
        Task<List<District>> GetAllDistrictWithoutPaggingAsync();
        Task InsertDistrictAsync(District districtName);
        Task UpdateDistrictAsync(District district);
        Task DeleteDistrictAsync(District district);

    }
}
