using Base.Core;
using Base.Core.Domain.BusinessUnits;
using Base.Core.Domain.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace Base.Services.BusinessUnits
{
    public interface IBusinessUnitService
    {
        Task DeleteBusinessUnitAsync(BusinessUnit businessUnit);
        Task UpdateBusinessUnitAsync(BusinessUnit businessUnit);
        Task InsertBusinessUnitAsync(BusinessUnit businessUnit);
        Task<IList<BusinessUnit>> GetAllBusinessUnitAsync();
        Task <BusinessUnit> GetBusinessUnitByIdAsync(int id); 
        Task<IPagedList<BusinessUnit>> GetAllBusinessUnitPaggedAsync(string businessUnitName, int pageIndex = 0, int pageSize = int.MaxValue, bool getOnlyTotalCount = false);

    }
}
