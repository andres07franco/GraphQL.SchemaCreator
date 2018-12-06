using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphQL.SchemaCreator.Attributes
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public class NullableGraphTypeAttribute : Attribute
    {
    }
}
