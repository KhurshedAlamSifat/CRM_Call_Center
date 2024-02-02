using Base.Core.Domain.Districts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Core.Domain.Thanas
{
    public partial class Thana : BaseEntity
    {
        public string ThanaName { get; set; }  
        public int District_Id { get; set; }  

    }
}
