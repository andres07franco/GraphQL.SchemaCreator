using GraphQL.SchemaCreator.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace GraphQL.SchemaCreator
{
    /// <summary>
    /// Generador de schemas GraphQL
    /// </summary>
    public class SchemaBuilder
    {
        /// <summary>
        /// Alamacena lo typos convertidos a GraphQl
        /// para no repetir la transformación de estos
        /// al esclar en el arbol del rooType
        /// </summary>
        private List<Type> types;
        private SchemaBuilder()
        {
            types = new List<Type>();
        }

        /// <summary>
        /// Crea un objeto del tipo SchemaBuilder
        /// </summary>
        /// <returns>Contructor de Schema</returns>
        public static SchemaBuilder Make()
        {
            return new SchemaBuilder();
        }

        /// <summary>
        /// Genera un string con el schema para GrapQL
        /// </summary>
        /// <param name="rootType">Tipo Principal</param>
        /// <param name="mutationType">Motaciones</param>
        /// <returns>Schema</returns>
        public string Build(Type rootType, Type mutationType = null)
        {
            string rootTypeDefinition = "";

            rootTypeDefinition = ResolveType(rootType);

            if (mutationType != null)
                rootTypeDefinition += ResolveType(mutationType);

            return "scalar Date \n" + rootTypeDefinition;
        }

        /// <summary>
        /// Convierte el tipo de datos en un tipo GraphQL
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public string ResolveType(Type type)
        {
            //Alamcenando type para no volver a definir
            types.Add(type);

            var propertis = type.GetProperties().Where(mb => mb.MemberType.Equals(MemberTypes.Property)).ToList();
            var rootAttr = type.GetCustomAttributes(typeof(ObjectGraphTypeAttribute), true)
                 .OfType<ObjectGraphTypeAttribute>()
                 .FirstOrDefault();

            if (rootAttr == null) return string.Empty;

            var queryBuilder = "";

            var nameType = rootAttr != null && !string.IsNullOrEmpty(rootAttr.Name) ? rootAttr.Name : type.Name;

            if(rootAttr.IsInputType)
                queryBuilder += $"input {nameType}";
            else
                queryBuilder +=  $"type {nameType}";
            queryBuilder += "{";

            //Recorremos las propiedades
            propertis.ForEach((member) => {

                var attr = member.GetCustomAttributes(typeof(FieldGraphTypeAttribute), true)
                                 .OfType<FieldGraphTypeAttribute>()
                                 .FirstOrDefault();

                bool isEnumerable = false;
                var memberPropertyType = ResoveEnumerableType(member.PropertyType, out isEnumerable);
               

                if (!IsPrimitiveType(memberPropertyType) && !types.Exists(t => t.Equals(memberPropertyType)))
                {
                    queryBuilder = ResolveType(memberPropertyType) + queryBuilder;
                }

                var nameProperty = attr != null && !string.IsNullOrEmpty(attr.Name) ? attr.Name : member.Name;
                //Poniendo primera letra en minusculas
                nameProperty = nameProperty[0].ToString().ToLower() + nameProperty.Substring(1, nameProperty.Length - 1);
                var typeProperty = attr != null && attr.IsId ? "ID" : ConvertToGraphType(memberPropertyType);
                var isRequiered = attr != null && (!attr.Nullable || attr.IsId) ? "!" : "";
                typeProperty = (isEnumerable ? "[" : "") + typeProperty + (isEnumerable ? "]" : "") ;
                if (!string.IsNullOrEmpty(typeProperty))
                    queryBuilder += $"\n { nameProperty}: {typeProperty}{isRequiered}";

            });

            //Resolviendo Endponts
            var methods = type.GetMethods().ToList();

            methods.ForEach((method) =>
            {

                bool isEnumerableReturn = false;
                var methodReturnType = ResoveEnumerableType(method.ReturnType, out isEnumerableReturn);

                //Resolviendo returns
                if (!IsPrimitiveType(methodReturnType) && !types.Exists(t => t.Equals(methodReturnType)))
                {
                    queryBuilder = ResolveType(methodReturnType) + queryBuilder;
                }

                var attr = method.GetCustomAttributes(typeof(EndPointGraphTypeAttribute), true)
                                 .OfType<EndPointGraphTypeAttribute>()
                                 .FirstOrDefault();

                if (attr != null)
                {
                    var patameters = new List<string>();
                    //Resolviendo Params
                    method.GetParameters().ToList().ForEach(param =>
                    {

                        bool isEnumerable = false;
                        var paramType = ResoveEnumerableType(param.ParameterType, out isEnumerable);


                        if (!IsPrimitiveType(paramType) && !types.Exists(t => t.Equals(paramType)))
                        {
                            queryBuilder = ResolveType(paramType) + queryBuilder;
                        }

                        var nullAttr = param.GetCustomAttributes(typeof(NullableGraphTypeAttribute), true)
                                         .OfType<NullableGraphTypeAttribute>()
                                         .FirstOrDefault();

                        var nullableParam = (nullAttr == null ? "" : "!");
                        var paramParameterType = (isEnumerable ? "[" : "") + ConvertToGraphType(paramType) + (isEnumerable ? "]" : "");
                        patameters.Add(param.Name + ": " + paramParameterType + nullableParam);
                    });


                    var nameProperty = attr != null && !string.IsNullOrEmpty(attr.Name) ? attr.Name : method.Name;
                    //Poniendo primera letra en minusculas
                    nameProperty = nameProperty[0].ToString().ToLower() + nameProperty.Substring(1, nameProperty.Length - 1);
                    var returnTypeGraph = (isEnumerableReturn ? "[" : "") + ConvertToGraphType(methodReturnType) + (isEnumerableReturn ? "]" : "");
                    var parameterString = string.Join(",", patameters);

                    var metaAttr = method.GetCustomAttributes(typeof(EndPointGraphTypeAttribute), true)
                                     .OfType<EndPointGraphTypeAttribute>()
                                     .FirstOrDefault();

                    nameProperty = metaAttr != null && !string.IsNullOrEmpty(metaAttr.Name) ? metaAttr.Name : nameProperty;

                    queryBuilder += $"\n {nameProperty}({parameterString}): {returnTypeGraph}";
                }

            });

            queryBuilder += "\n}\n";
            return queryBuilder;
        }

        /// <summary>
        /// Mapea los típos de datos primitvos de C# a scalar de GraphQL
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private string ConvertToGraphType(Type type)
        {
            string typeGraph = type.Name;
            if (type == typeof(Int32) || type == typeof(Nullable<Int32>))
            {
                typeGraph = "Int";
            }
            else if (type == typeof(Decimal) || type == typeof(float) || type == typeof(Nullable<float>) || type == typeof(Nullable<Decimal>))
            {
                typeGraph = "Float";
            }
            else if (type == typeof(String))
            {
                typeGraph = "String";
            }
            else if (type == typeof(Boolean) || type == typeof(Nullable<Boolean>))
            {
                typeGraph = "Boolean";
            }
            else if (type == typeof(DateTime))
            {
                typeGraph = "Date";
            }
            return typeGraph;
        }

        /// <summary>
        /// Verifica si el tipo de datos es primitivo
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private bool IsPrimitiveType(Type type)
        {
            return  type.IsPrimitive || 
                    type == typeof(Decimal) || 
                    type == typeof(String) || 
                    type == typeof(Int32) || 
                    type == typeof(DateTime) || 
                    type == typeof(Boolean) || 
                    type == typeof(Nullable<float>) || 
                    type == typeof(Nullable<Decimal>) || 
                    type == typeof(Nullable<Boolean>);
        }


        private Type ResoveEnumerableType(Type type,out bool isEnumerable)
        {
            isEnumerable = false;

           /* if (type.GetGenericArguments().Length > 0 && !typeof(IEnumerable<object>).IsAssignableFrom(type))
            {
                throw new Exception("Generict type only valid with IEnumerable");
            }*/
            //Si es un Enumerable
            if (type.GetGenericArguments().Length > 0 && typeof(IEnumerable<object>).IsAssignableFrom(type))
            {
                type = type.GetGenericArguments()[0];
                isEnumerable = true;
            }
            else if (type.GetGenericArguments().Length > 0 )
            {
                type = type.GetGenericArguments()[0];               
            }

            return type;
        }

    }
}
