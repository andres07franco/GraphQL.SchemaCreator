﻿using GraphQL.SchemaCreator.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphQL.SchemaCreator.Test.SchemaModels
{
    [ObjectGraphType]
    public class ChildDroid
    {
        [FieldGraphType(Description =  "Id del Bl", IsId = true)]
        public string Id { get; set; }
        [FieldGraphType(Description =  "Name del Bl")]
        public string Name { get; set; }
    }

}
