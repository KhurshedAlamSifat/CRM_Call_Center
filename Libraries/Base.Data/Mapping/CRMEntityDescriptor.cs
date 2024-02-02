using System.Collections.Generic;

namespace Base.Data.Mapping
{
    public partial class CRMEntityDescriptor
    {
        public CRMEntityDescriptor()
        {
            Fields = new List<CRMEntityFieldDescriptor>();
        }

        public string EntityName { get; set; }
        public string SchemaName { get; set; }
        public ICollection<CRMEntityFieldDescriptor> Fields { get; set; }
    }
}