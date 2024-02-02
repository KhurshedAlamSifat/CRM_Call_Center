using Base.Core;
using Base.Core.Domain.ComplainTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Services.ComplainTypes
{
    public interface IComplainTypeService
    {
        Task<ComplainType> GetComplainTypeAsync(int id);
        Task<IPagedList<ComplainType>> GetAllComplainAsync(string complainTypeName = "", int pageIndex = 0, int pageSize = int.MaxValue, bool getOnlyTotalCount = false);
        Task<List<ComplainType>> GetAllComplainTypeAsync();
        Task InsertComplainTypeAsync(ComplainType complainType);
        Task UpdateComplainTypeAsync(ComplainType complainType);
        Task DeleteComplainTypeAsync(ComplainType complainType);
    }
}
