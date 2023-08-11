# ArgumentString
What is an argument string, you might ask? The idea is borrowed from command line arguments and connection strings.  
So an argument string literal looks like this: `"foo=bar;version=1"`.  

You have some options, e. g. setting mandatory fields that are checked on object instantiation and an easy way to access the arguments by method or by index.

## Installation
- Install via [NuGet](https://www.nuget.org/packages/ArgumentString): `PM> Install-Package ArgumentString`
- Build from your own

## Instantiation
Simplest examples:
``` csharp
var argumentsExample1 = new ArgumentString("foo=bar");
var argumentsExample2 = new ArgumentString("foo=bar;version=1");
```

Examples with options:
``` csharp
var argumentsExample = new ArgumentString("foo=bar", new ParseOptions("foo"));

var argumentsExample2 = new ArgumentString("foo=bar;version=1", 
    new ParseOptions("foo") { /* ... */ });

var argumentsExample3 = new ArgumentString("foo=bar;version=1", options => { 
    options.MandatoryKeys = new List<string> { "foo" };
});

var argumentsExample4 = new ArgumentString("foo->bar|version->1", options => { 
    options.ArgumentSeparator = "|";
    options.KeyValueSeparator = "->";
    options.ThrowOnAccessIfKeyNotFound = true;
});
```
  
## Getting values
Accessing values is the most fun part:
``` csharp
string foo = argumentsExample.Get("foo"); // -> bar
string foo = argumentsExample["foo"]; // -> bar
string foo = argumentsExample.Get(0); // -> bar
string foo = argumentsExample[0]; // -> bar
```

Dealing with falsy values:
``` csharp
string foo = argumentsExample.Get("missing"); // -> string.Empty if `ThrowOnAccessIfKeyNotFound` is false (default)
string foo = argumentsExample.Get("missing"); // -> MissingArgumentException if `ThrowOnAccessIfKeyNotFound` is true
string foo = argumentsExample["missing"]; // -> same as above

string foo = argumentsExample.Get(2); // -> string.Empty if `ThrowOnAccessIfKeyNotFound` is false (default)
string foo = argumentsExample.Get(2); // -> MissingArgumentException if `ThrowOnAccessIfKeyNotFound` is true
string foo = argumentsExample[2]; // -> same as above
```

Need to work with a specific format?  
*You should pay attention to pass correct values for the conversion to work. For that reason there are some more exceptions that will be thrown.*
``` csharp
string version = argumentsExample.Get<float>("version"); // -> (float)1 
string version = argumentsExample.Get<float>(1); // -> (float)1 
```