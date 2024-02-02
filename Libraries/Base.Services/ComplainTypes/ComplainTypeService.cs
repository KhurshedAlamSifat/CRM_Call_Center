using Base.Core;
using Base.Core.Caching;
using Base.Core.Domain.ComplainTypes;
using Base.Core.Domain.Users;
using Base.Data;
using Base.Services.Security;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using StackExchange.Profiling.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Services.ComplainTypes
{
    public class ComplainTypeService : IComplainTypeService
    {
        private readonly IEncryptionService _encryptionService;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _environment;
        private readonly IRepository<ComplainType> _complaintypeRepository;
        private readonly IStaticCacheManager _staticCacheManager;
        public ComplainTypeService(IEncryptionService encryptionService, IConfiguration configuration,
            IWebHostEnvironment environment, IRepository<ComplainType> complaintypeRepository, IStaticCacheManager staticCacheManager)
        {
            this._encryptionService = encryptionService;
            _configuration = configuration;
            _environment = environment;
            _complaintypeRepository = complaintypeRepository;
            _staticCacheManager = staticCacheManager;
        }

        #region CTcrud

        public async Task<ComplainType> GetComplainTypeAsync(int id)
        {

            var complain = await _complaintypeRepository.Table.FirstOrDefaultAsync(p => p.Id == id);

            return complain;
        }

        public async Task DeleteComplainTypeAsync(ComplainType complainType)
        {
            await _complaintypeRepository.DeleteAsync(complainType);
        }

        public async Task<List<ComplainType>> GetAllComplainTypeAsync()
        {
            var complaintypes = await _complaintypeRepository.GetAllAsync(query =>
            {
                query = query.OrderByDescending(c => c.Id);

                return query;
            });

            return await complaintypes.ToListAsync();
        }

        public async Task InsertComplainTypeAsync(ComplainType complainType)
        {
            await _complaintypeRepository.InsertAsync(complainType);
        }

        public async Task UpdateComplainTypeAsync(ComplainType complainType)
        {
            await _complaintypeRepository.UpdateAsync(complainType);
        }

        public virtual async Task<IPagedList<ComplainType>> GetAllComplainAsync(string complainTypeName = "", int pageIndex = 0, int pageSize = int.MaxValue, bool getOnlyTotalCount = false)
        {
            var complains = await _complaintypeRepository.GetAllPagedAsync(query =>
            {
                if (!string.IsNullOrEmpty(complainTypeName))
                    query = query.Where(x => x.ComplainTypeName == complainTypeName);

                query = query.OrderBy(c => c.Id);

                return query;
            }, pageIndex, pageSize, getOnlyTotalCount);

            return complains;
        }
        #endregion
    }
}
