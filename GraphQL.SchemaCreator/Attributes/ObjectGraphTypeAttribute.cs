using System;


namespace GraphQL.SchemaCreator.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Enum | AttributeTargets.Property | AttributeTargets.Field)]
    public class ObjectGraphTypeAttribute : Attribute
    {

        public string Name { get; set; }
        public string Description { get; set; }

        public bool IsInputType { get; set; }

        public ObjectGraphTypeAttribute(string name = "", string description =  "")
        {
            Name = name;
            Description = description;
        }
    }
}
