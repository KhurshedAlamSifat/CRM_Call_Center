using Base.Core.Domain.Technicians;
using Base.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.Core.Domain.ServiceCenters;

namespace Base.Services.Technicians
{
    public interface ITechnicianService
    {
        Task<Technician> GetTechnicianAsync(int id);
        Task<IPagedList<Technician>> GetAllTechnicianAsync(string technicianname = null, int thana = 0, int serviceCenter = 0,
            int categoryId = 0, int pageIndex = 0, int pageSize = int.MaxValue, bool getOnlyTotalCount = false);

        Task<List<Technician>> GetTechnicianAsync();
        Task InsertTechnicianAsync(Technician technicianName);
        Task UpdateTechnicianAsync(Technician technician);
        Task DeleteTechnicianAsync(Technician technician);
        Task<List<Technician>> GetTechnicianByThanaAsynce(int thana);
        Task<List<Technician>> GetTechnicianByServiceCenterAsynce(int serviceCenter);
    }
}
