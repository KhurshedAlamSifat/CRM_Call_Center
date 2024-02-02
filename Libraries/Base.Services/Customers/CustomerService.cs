using Base.Core;
using Base.Core.Caching;
using Base.Core.Domain.Customers;
using Base.Core.Domain.Users;
using Base.Data;
using Base.Services.Security;
using DocumentFormat.OpenXml.Vml.Spreadsheet;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Base.Services.Customers
{
    public class CustomerService : ICustomerService
    {
        private readonly IRepository<Customer> _customerRepository;
        private readonly IStaticCacheManager _staticCacheManager;
        public CustomerService(IRepository<Customer> customerRepository, IStaticCacheManager staticCacheManager)
        {
            _customerRepository = customerRepository;
            _staticCacheManager = staticCacheManager;
        }


        public virtual async Task<IPagedList<Customer>> GetAllCustomersAsync(string phoneNumber = null, int pageIndex = 0, int pageSize = int.MaxValue, bool getOnlyTotalCount = false)
        {
            var districts = await _customerRepository.GetAllPagedAsync(query =>
            {
                if (!string.IsNullOrEmpty(phoneNumber))
                    query = query.Where(d => d.PhoneNumber == phoneNumber);
                query = query.OrderByDescending(c => c.Id);

                return query;
            }, pageIndex, pageSize, getOnlyTotalCount);

            return districts;

        }


        public async Task<Customer> GetCustomerAsync(int id)
        {
            var customer = await _customerRepository.GetByIdAsync(id);

            return customer;
        }

        public async Task InsertCustomerAsync(Customer customer)
        {
            await _customerRepository.InsertAsync(customer);
        }

        public async Task UpdateCustomerAsync(Customer customer)
        {
            await _customerRepository.UpdateAsync(customer);

            var keyByPhoneNo = _staticCacheManager.PrepareKeyForDefaultCache(new("Crm.customer.byphone-{0}"), customer.PhoneNumber);
            await _staticCacheManager.RemoveAsync(keyByPhoneNo);
        }

        public async Task DeleteCustomerAsync(Customer customer)
        {
            await _customerRepository.DeleteAsync(customer);

            var keyByPhoneNo = _staticCacheManager.PrepareKeyForDefaultCache(new("Crm.customer.byphone-{0}"), customer.PhoneNumber);
            await _staticCacheManager.RemoveAsync(keyByPhoneNo);
        }

        public async Task<Customer> GetCustomerByPhoneNo(string phoneNumber)
        {
            var key = _staticCacheManager.PrepareKeyForDefaultCache(new("Crm.customer.byphone-{0}"), phoneNumber);

            var customer = await _staticCacheManager.GetAsync(key, async () => await _customerRepository.Table.Where(x=>x.PhoneNumber== phoneNumber).FirstOrDefaultAsync());
            return customer;
        }
    }
}
