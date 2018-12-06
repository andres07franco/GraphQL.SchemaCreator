using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphQL.SchemaCreator.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class FieldGraphTypeAttribute : Attribute
    {
        public bool Nullable { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }

        public bool IsId { get; set; }

        public FieldGraphTypeAttribute(bool nullable = true, string description = "", string name = "", bool isId = false)
        {
            Nullable = nullable;
            Description = description;
            Name = name;
            IsId = isId;

        }
    }
}
