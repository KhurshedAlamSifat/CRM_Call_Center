using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Core.Domain.Tickets
{
    public class Ticket : BaseEntity
    {
        public int ComplainType_Id { get; set; }
        public int Customer_Id { get; set; }
        public int? Technician_Id { get; set; }
        public string CustomerPhoneNo { get; set; }
        public int District_Id { get; set; }
        public int Thana_Id { get; set; }
        public int Business_Id { get; set; }
        public int Brand_Id { get; set; }
        public int Category_Id { get; set; }
        public string ProblemIds { get; set; }
        public string Note { get; set; }
        public int? ServiceCenter_Id { get; set; }
    }
}
