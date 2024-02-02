using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Core.Domain.ServiceCenters
{
    public partial class ServiceCenter  : BaseEntity
    {
        public string ServiceCenterName { get; set; }
        public int Thana_Id { get; set; }
       

    }
}
