using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphQL.SchemaCreator.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class EndPointGraphTypeAttribute : Attribute
    {
        public string Description { get; set; }
        public string Name { get; set; }



        public EndPointGraphTypeAttribute( string description = "", string name = "")
        {
      
            Description = description;
            Name = name;
          

        }
    }
}
