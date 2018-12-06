using GraphQL.SchemaCreator.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphQL.SchemaCreator.Test.SchemaModels
{
    [ObjectGraphType]
    public class Query
    {
        private List<Droid> _droids = new List<Droid>
        {
            new Droid { Id = "123", Name = "R2-D2" }
        };

        [EndPointGraphType(Name = "droid", Description = "Obtiene los nbls")]
        public Droid GetDroid(string id)
        {
            return _droids.FirstOrDefault();
        }
    }
}
