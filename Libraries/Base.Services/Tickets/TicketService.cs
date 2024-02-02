using Base.Core;
using Base.Core.Caching;
using Base.Core.Domain.Customers;
using Base.Core.Domain.Tickets;
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

namespace Base.Services.Tickets
{
    public class TicketService : ITicketService
    {
        private readonly IRepository<Ticket> _ticketRepository;
        private readonly IRepository<Customer> _customerRepository;
        private readonly IStaticCacheManager _staticCacheManager;
        public TicketService(IRepository<Ticket> ticketRepository, IRepository<Customer> customerRepository, IStaticCacheManager staticCacheManager)
        {
            _ticketRepository = ticketRepository;
            _staticCacheManager = staticCacheManager;
            _customerRepository= customerRepository;
        }


        public virtual async Task<IPagedList<Ticket>> GetAllTicketAsync(string customerPhoneNumber = null, int pageIndex = 0, int pageSize = int.MaxValue, bool getOnlyTotalCount = false)
        {
            var districts = await _ticketRepository.GetAllPagedAsync(query =>
            {
                if (!string.IsNullOrEmpty(customerPhoneNumber))
                {
                    //query = query.Where(d => d.PhoneNumber == phoneNumber);
                    query = from q in query
                            join cst in _customerRepository.Table on q.Customer_Id equals cst.Id
                            where q.CustomerPhoneNo == customerPhoneNumber
                            select q;
                }
                query = query.OrderByDescending(c => c.Id);

                return query;
            }, pageIndex, pageSize, getOnlyTotalCount);

            return districts;

        }


        public async Task<Ticket> GetTicketAsync(int id)
        {
            var ticket = await _ticketRepository.GetByIdAsync(id);

            return ticket;
        }

        public async Task InsertTicketAsync(Ticket ticket)
        {
            await _ticketRepository.InsertAsync(ticket);
        }

        public async Task UpdateTicketAsync(Ticket ticket)
        {
            await _ticketRepository.UpdateAsync(ticket);
        }

        public async Task DeleteTicketAsync(Ticket ticket)
        {
            await _ticketRepository.DeleteAsync(ticket);
        }
    }
}
