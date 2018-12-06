using GraphQL.SchemaCreator.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphQL.SchemaCreator.Test.SchemaModels2
{

    [ObjectGraphType(IsInputType = true)]
    public class DroidInput
    {
        [FieldGraphType(Description = "Id del Bl")]
        public string Name { get; set; }

    }

    [ObjectGraphType]
    public class Droid
    {
        [FieldGraphType(Description = "Id del Bl", IsId = true)]
        public string Id { get; set; }
        [FieldGraphType(Description = "Name del Bl")]
        public string Name { get; set; }
        [FieldGraphType(Description = "Hijo")]
        public List<ChildDroid> Childs { get; set; }
    }

    [ObjectGraphType]
    public class ChildDroid
    {
        [FieldGraphType(Description = "Id del Bl", IsId = true)]
        public string Id { get; set; }
        [FieldGraphType(Description = "Name del Bl")]
        public string Name { get; set; }
    }

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

    [ObjectGraphType]
    public class Mutation
    {
        private List<Droid> _droids = new List<Droid>
        {
            new Droid { Id = "123", Name = "R2-D2" }
        };

 
        [EndPointGraphType(Name = "droidsAdd", Description = "Obtiene los nbls")]
        public List<Droid> DroidsAdd(List<DroidInput> childs)
        {
            _droids.Add(new Droid { Id = "1", Name = "R3-D3", Childs = new List<ChildDroid> { new ChildDroid { Id = "456", Name = "soy el hijo" } } });
            return _droids;
        }
    }
}
