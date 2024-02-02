using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Core.Domain.Categories
{ 
    public class Category : BaseEntity
    {
        public string CategoryName { get; set; }
        public int Brand_Id { get; set; }
    }
}
