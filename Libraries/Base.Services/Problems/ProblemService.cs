using Base.Core.Domain.Problems;
using Base.Core;
using Base.Data;
using Base.Services.Security;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.Core.Caching;
//using Base.Core.Domain.Problems;

namespace Base.Services.Problems
{
    public class ProblemService : IProblemService
    {
        private readonly IEncryptionService _encryptionService;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _environment;
        private readonly IRepository<Problem> _problemRepository;
        private readonly IStaticCacheManager _staticCacheManager;


        public ProblemService(IEncryptionService encryptionService, IConfiguration configuration, 
            IWebHostEnvironment environment, IRepository<Problem> problemRepository, IStaticCacheManager staticCacheManager)
        {
            this._encryptionService = encryptionService;
            _configuration = configuration;
            _environment = environment;
            _problemRepository = problemRepository;
            _staticCacheManager = staticCacheManager;
        }

        #region CTcrud

        public async Task<Problem> GetProblemAsync(int id)
        {

            var problem = await _problemRepository.Table.FirstOrDefaultAsync(p => p.Id == id);

            return problem;
        }

        public async Task DeleteProblemAsync(Problem problem)
        {
            await _problemRepository.DeleteAsync(problem);
        }

        public async Task<List<Problem>> GetProblemAsync()
        {
            var problems = await _problemRepository.GetAllAsync(query =>
            {
                query=query.OrderBy(p => p.Id); 
                return query;
            });

            return await problems.ToListAsync();
        }

        public async Task InsertProblemAsync(Problem problem)
        {
            await _problemRepository.InsertAsync(problem);
        }

        public async Task UpdateProblemAsync(Problem problem)
        {
            await _problemRepository.UpdateAsync(problem);
        }

        public virtual async Task<IPagedList<Problem>> GetAllProblemAsync(string problemName = "", int pageIndex = 0, int pageSize = int.MaxValue, bool getOnlyTotalCount = false)
        {
            var problems = await _problemRepository.GetAllPagedAsync(query =>
            {
                if (!string.IsNullOrEmpty(problemName))
                    query = query.Where(x => x.ProblemDescription == problemName);

                query = query.OrderBy(c => c.Id);

                return query;
            }, pageIndex, pageSize, getOnlyTotalCount);

            return problems;
        }

        public virtual async Task<IList<Problem>> GetAllProblemByCategoryAsync(int categoryId = 0)
        {
            var categories = await _problemRepository.GetAllAsync(query =>
            {
                if (categoryId > 0)
                    query = query.Where(c => c.CategoryId == categoryId);

                query = query.OrderByDescending(c => c.Id);

                return query;
            });

            return await categories.ToListAsync();
        }

        public virtual async Task<Problem> GetProblemByIdAsync(int id)
        {
            if (id == 0)
                return null;

            return await _problemRepository.GetByIdAsync(id);
        }
        #endregion
    }
}
