using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Core.Domain.Problems
{
    public partial class Problem : BaseEntity
    {
        public string ProblemDescription { get; set; }
        public int CategoryId { get; set; }
    }
}
