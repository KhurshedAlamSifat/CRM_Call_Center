using Base.Core.Domain.Districts;
using Base.Core;
using Base.Core.Domain.Thanas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Services.Thanas
{
    public interface IThanaService
    {
        Task<Thana> GetThanaAsync(int id);
        Task<IPagedList<Thana>> GetAllThanaAsync(string thananame = null, int district = 0, int pageIndex = 0, int pageSize = int.MaxValue, bool getOnlyTotalCount = false);
        Task<List<Thana>> GetThanaAsync();
        Task InsertThanaAsync(Thana thanaName);
        Task UpdateThanaAsync(Thana thana);
        Task DeleteThanaAsync(Thana thana);
        Task<List<Thana>> GetThanaByDistrictAsync(int districtId);
    }
}
