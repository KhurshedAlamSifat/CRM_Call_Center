using Base.Core;
using Base.Core.Domain.Customers;
using Base.Core.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Services.Customers
{
    public interface ICustomerService
    {
        Task<Customer> GetCustomerAsync(int id);
        Task<IPagedList<Customer>> GetAllCustomersAsync(string phoneNumber = null,int pageIndex = 0, int pageSize = int.MaxValue, bool getOnlyTotalCount = false);
        Task InsertCustomerAsync(Customer customer);
        Task UpdateCustomerAsync(Customer customer);
        Task DeleteCustomerAsync(Customer customer);
        Task<Customer> GetCustomerByPhoneNo(string phoneNumber);

    }
}
