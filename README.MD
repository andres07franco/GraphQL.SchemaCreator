# GraphQL Schema Creator

Generador de schema para graph a base objetos c#.

## Usage




## Roadmap

### Grammar / AST
- Grammar and AST for the GraphQL language should be compatible with the [June 2018 specification](http://facebook.github.io/graphql/June2018/).

### Operation Execution
- [x] Scalars
- [x] Objects
- [x] Lists of objects/interfaces
- [] Interfaces
- [] Unions
- [x] Arguments
- [x] Variables
- [] Fragments
- [] Directives
  - [] Include
  - [] Skip
  - [] Custom
- [] Enumerations
- [x] Input Objects
- [x] Mutations
- [] Subscriptions
- [] Async execution


### Schema Introspection
- [x] __typename
- [x] __type
  - [x] name
  - [x] kind
  - [x] description
  - [x] fields
  - [x] interfaces
  - [x] possibleTypes
  - [x] enumValues
  - [x] inputFields
  - [x] ofType
- [x] __schema
  - [x] types
  - [x] queryType
  - [x] mutationType
  - [x] subscriptionType
  - [x] directives
