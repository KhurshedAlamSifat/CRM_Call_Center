using Base.Core;
using Base.Core.Domain.Customers;
using Base.Core.Domain.Tickets;
using Base.Core.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Services.Tickets
{
    public interface ITicketService
    {
        Task<IPagedList<Ticket>> GetAllTicketAsync(string customerPhoneNumber = null, int pageIndex = 0, int pageSize = int.MaxValue, bool getOnlyTotalCount = false);
        Task<Ticket> GetTicketAsync(int id);
        Task InsertTicketAsync(Ticket ticket);
        Task UpdateTicketAsync(Ticket ticket);
        Task DeleteTicketAsync(Ticket ticket);
    }
}
