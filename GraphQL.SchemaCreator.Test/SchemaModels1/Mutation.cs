using GraphQL.SchemaCreator.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphQL.SchemaCreator.Test.SchemaModels
{
    [ObjectGraphType]
    public class Mutation
    {
        private List<Droid> _droids = new List<Droid>
        {
            new Droid { Id = "123", Name = "R2-D2" }
        };

        [EndPointGraphType(Name ="droidAdd")]
        public Droid DroidAdd(string id)
        {
            _droids.Add(new Droid { Id = "1", Name = "R3-D3", Child = new ChildDroid { Id = "456", Name = "soy el hijo" } });
            return _droids.FirstOrDefault(x => x.Id == id);
        }
    }
}
