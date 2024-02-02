using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Core.Domain.Customers
{
    public class Customer : BaseEntity
    {
        public string PhoneNumber { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public int District_Id { get; set; }
        public int Thana_Id { get; set; }


    }
}
