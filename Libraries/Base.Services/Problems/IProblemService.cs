using Base.Core.Domain.Problems;
using Base.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using Base.Core.Domain.Problems;

namespace Base.Services.Problems
{
    public interface IProblemService
    {
        Task<Problem> GetProblemAsync(int id);
        Task<IPagedList<Problem>> GetAllProblemAsync(string problemName = "", int pageIndex = 0, int pageSize = int.MaxValue, bool getOnlyTotalCount = false);
        Task<List<Problem>> GetProblemAsync();
        Task InsertProblemAsync(Problem problem);
        Task UpdateProblemAsync(Problem problem);
        Task DeleteProblemAsync(Problem problem);
        Task<IList<Problem>> GetAllProblemByCategoryAsync(int categoryId = 0);
        Task<Problem> GetProblemByIdAsync(int id);
    }
}
