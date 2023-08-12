# ArgumentString
What is an argument string, you might ask? The idea is borrowed from connection strings.  
So an argument string literal looks like this: `"foo=bar;version=1"`. The library allows you to access these arguments easily by key or by index.  

By providing options you can customize this library to your needs, e. g. 
- setting **mandatory fields** that are checked on object instantiation
- change **argument and key-value separators** (like the `"foo->bar|version->1"` syntax more?)
- accessing faulty keys will not **throw an exception** by default, but you can throw one if you like to
- accessing faulty keys will always **return an empty string** by default, but you can return `null` if you like to

This is a tiny but fully tested and stable library.

## Installation
- Install via [NuGet](https://www.nuget.org/packages/ArgumentString): `PM> Install-Package ArgumentString`
- Build from your own

## Object creation
Simplest examples:
``` csharp
var example = new ArgumentString("foo=bar");
var example = new ArgumentString("foo=bar;version=1");
```

Examples with options:
``` csharp
var example = new ArgumentString("foo=bar", new ParseOptions("foo"));

var example = new ArgumentString("foo=bar;version=1", 
    new ParseOptions("foo") { /* ... */ });

var example = new ArgumentString("foo=bar;version=1", options => { 
    options.MandatoryKeys = new List<string> { "foo" };
});

var example = new ArgumentString("foo->bar|version->1", options => { 
    options.ArgumentSeparator = "|";
    options.KeyValueSeparator = "->";
    options.ThrowOnAccessIfKeyNotFound = true;
});
```
  
## Getting values
- `Get(string key)` and `Get(int index)` methods behave the same way 
- See `Big O notation` (complexity) in the methods description

Accessing values is the most fun part:
``` csharp
string foo = example.Get("foo"); // -> bar
string foo = example["foo"]; // -> bar
string foo = example.Get(0); // -> bar
string foo = example[0]; // -> bar
```

Dealing with faulty values:
``` csharp
string foo = example.Get("missing"); // -> string.Empty if `ThrowOnAccessIfKeyNotFound` is false (default)
string foo = example.Get("missing"); // -> MissingArgumentException if `ThrowOnAccessIfKeyNotFound` is true
string foo = example["missing"]; // -> same as above

string foo = example.Get(2); // -> string.Empty if `ThrowOnAccessIfKeyNotFound` is false (default)
string foo = example.Get(2); // -> MissingArgumentException if `ThrowOnAccessIfKeyNotFound` is true
string foo = example[2]; // -> same as above
```

Need to work with a specific format?  
*You should pay attention to pass correct values for the conversion to work. For that reason there are some more exceptions that will be thrown.*
``` csharp
float version = example.Get<float>("version"); // -> (float)1 
float version = example.Get<float>(1); // -> (float)1 
```