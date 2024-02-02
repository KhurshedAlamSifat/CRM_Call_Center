using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Core.Domain.Technicians
{
    public partial class Technician : BaseEntity
    {
        public string TechnicianName { get; set; }
        public string TechnicianPhoneNo { get; set; }
        public int? Thana_Id { get; set; }
        public int? ServiceCenter_Id { get; set; }
        public int? Category_Id { get; set; }
    }

}
