using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Core.Domain.Brands
{
    public class Brand : BaseEntity
    {
        public string BrandName { get; set; }
        public int BusinessUnit_Id { get; set; }
    }
}
