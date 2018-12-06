using System;
using GraphQL.SchemaCreator.Test.SchemaModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GraphQL.SchemaCreator.Test
{
    [TestClass]
    public class BuildSchemaTest
    {

        [TestMethod]
        [Description("Prueba a genracion de un simple objeto a un tipo Graph")]
        public void TestSimpleType()
        {

            string actualSchema = "";
            string expectedSchema = "scalar Date \ntype ChildDroid{\n id: ID!\n name: String\n}\n";

            actualSchema = SchemaBuilder.Make()
                                .Build(typeof(ChildDroid));

            // assert  
            Assert.AreEqual(expectedSchema, actualSchema);
        }

        [TestMethod]
        [Description("Prueba a genracion de un simple objeto con dependencia, a un tipo Graph")]
        public void TestSimpleTypeWithChild()
        {

            string actualSchema = "";
            string expectedSchema = "scalar Date \ntype ChildDroid{\n id: ID!\n name: String\n}\ntype Droid{\n id: ID!\n name: String\n child: ChildDroid\n}\n";

            actualSchema = SchemaBuilder.Make()
                                .Build(typeof(Droid));

            // assert  
            Assert.AreEqual(expectedSchema, actualSchema);
        }


        [TestMethod]
        [Description("Prueba a genracion un arbol de dependencias con un tipo query de Graph")]
        public void TestRootType()
        {
         
            string actualSchema = "";
            string expectedSchema = "scalar Date \ntype ChildDroid{\n id: ID!\n name: String\n}\ntype Droid{\n id: ID!\n name: String\n child: ChildDroid\n}\ntype Query{\n droid(id: String): Droid\n}\n";

            actualSchema = SchemaBuilder.Make()
                                .Build(typeof(Query));

            // assert  
            Assert.AreEqual(expectedSchema, actualSchema);
        }

        [TestMethod]
        [Description("Prueba a genracion un arbol de dependencias con un tipo mutation de Graph")]
        public void TestRootTypeWithMutattion()
        {

            string actualSchema = "";
            string expectedSchema = "scalar Date \ntype ChildDroid{\n id: ID!\n name: String\n}\ntype Droid{\n id: ID!\n name: String\n child: ChildDroid\n}\ntype Query{\n droid(id: String): Droid\n}\ntype Mutation{\n droidAdd(id: String): Droid\n}\n";

            actualSchema = SchemaBuilder.Make()
                                .Build(typeof(Query), typeof(Mutation));

            // assert  
            Assert.AreEqual(expectedSchema, actualSchema);
        }

        [TestMethod]
        [Description("Prueba a generacion de una muatation con endpoint que retorna un array")]
        public void TestMuttationWithReturnArray()
        {

            string actualSchema = "";
            string expectedSchema = "scalar Date \ntype ChildDroid{\n id: ID!\n name: String\n}\ntype Droid{\n id: ID!\n name: String\n childs: [ChildDroid]\n}\ntype Query{\n droid(id: String): Droid\n}\ninput DroidInput{\n name: String\n}\ntype Mutation{\n droidsAdd(childs: [DroidInput]): [Droid]\n}\n";

            actualSchema = SchemaBuilder.Make()
                                .Build(typeof(GraphQL.SchemaCreator.Test.SchemaModels2.Query), typeof(GraphQL.SchemaCreator.Test.SchemaModels2.Mutation));

            // assert  
            Assert.AreEqual(expectedSchema, actualSchema);
        }


    }
}
