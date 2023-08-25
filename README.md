# ArgumentString
![Logo](logo.png)  
[![.NET 6 Build and Test](https://github.com/jordi1988/ArgumentString/actions/workflows/dotnet6-build-and-test.yml/badge.svg)](https://github.com/jordi1988/ArgumentString/actions/workflows/dotnet6-build-and-test.yml)  

**What is an argument string, you might ask? The idea is borrowed from connection strings.  
So an argument string literal looks like this: `"foo=bar;version=1"`. The library allows you to access these arguments easily by key or by index. You can customize the library's behavior and easily deal with faulty and default values in case the key was not found.**  

By providing options you can customize this library to your needs, e. g. 
- setting **mandatory fields** that are checked on object instantiation
- change **argument and key-value separators** (like the `"foo->bar|version->1"` syntax more?)
- accessing faulty keys will **not throw an exception** by default, but you can throw one if you like to
- accessing faulty keys will **always return an empty string** by default, but you can return `null` if you like to (unless you are providing a default value)

This is a tiny but fully tested and stable library.

## Installation
- Install via [NuGet](https://www.nuget.org/packages/ArgumentString): `PM> Install-Package ArgumentString`
- Build from your own

## Object creation
Simplest examples:
``` **csharp**
var arguments = new ArgumentString("foo=bar");
var arguments = new ArgumentString("foo=bar;version=1");
```

Examples with options:
``` csharp
var arguments = new ArgumentString("foo=bar", new ParseOptions("foo"));

var arguments = new ArgumentString("foo->bar|version->1", options => { 
    options.MandatoryKeys = new List<string> { "foo" };
    options.ArgumentSeparator = "|";
    options.KeyValueSeparator = "->";
    options.ThrowOnAccessIfKeyNotFound = true;
    options.ReturnEmptyStringInsteadOfNull = false;
});
```
  
## Getting values
- When getting values you can choose from using the `Get()` or `Get<T>()` method or the `indexer[]`. 
- All methods and indexers behave the same way 
- If a default value other than `null` gets provided the `ReturnEmptyStringInsteadOfNull` option has no effect.
- See `Big O notation` (complexity) in the methods description

**Signatures**:
- `Get(string key, string? defaultValue = null)`
- `Get(int index, string? defaultValue = null)`
- `Get<T>(string key, T? defaultValue = null)`
- `Get<T>(int index, T? defaultValue = null)`
- Indexer `this[string key, T? defaultValue = null]`
- Indexer `this[int index, T? defaultValue = null]`

**Accessing values** is the most fun part:
``` csharp
var arguments = new ArgumentString("foo=bar;version=1");

string foo = arguments.Get("foo"); // -> bar
string foo = arguments["foo"]; // -> bar

string foo = arguments.Get(0); // -> bar
string foo = arguments[0]; // -> bar
```

Dealing with **faulty values**:
``` csharp
var arguments = new ArgumentString("foo=bar;version=1");

string foo = arguments.Get("missing"); // -> `string.Empty` if `ThrowOnAccessIfKeyNotFound` is false (default)
string foo = arguments.Get("missing"); // -> `MissingArgumentException` if `ThrowOnAccessIfKeyNotFound` is true
string foo = arguments["missing"]; // -> same as above

string foo = arguments.Get(2); // -> `string.Empty` if `ThrowOnAccessIfKeyNotFound` is false (default)
string foo = arguments.Get(2); // -> `MissingArgumentException` if `ThrowOnAccessIfKeyNotFound` is true
string foo = arguments[2]; // -> same as above
```

Dealing with **default values** (second parameter `defaultValue` on `Get()` method):
``` csharp
var arguments = new ArgumentString("foo=bar;version=1");

string foo = arguments.Get("missing", "bar"); // -> `bar` if `ThrowOnAccessIfKeyNotFound` is false (default)
string foo = arguments.Get("missing", "bar"); // -> `MissingArgumentException` if `ThrowOnAccessIfKeyNotFound` is true
string foo = arguments["missing", "bar"]; // -> same as above

string foo = arguments.Get(2, "bar"); // -> `bar` if `ThrowOnAccessIfKeyNotFound` is false (default)
string foo = arguments.Get(2, "bar"); // -> `MissingArgumentException` if `ThrowOnAccessIfKeyNotFound` is true
string foo = arguments[2, "bar"]; // -> same as above
```

Need to work with a **specific format (type conversion)**?  
*You should pay attention to pass correct values for the conversion to work. For that reason there are some more exceptions that will be thrown.*
``` csharp
var arguments = new ArgumentString("foo=bar;version=1");

float version = arguments.Get<float>("version"); // -> (float)1 
float version = arguments.Get<float>("missing", 99); // -> (float)99 

float version = arguments.Get<float>(1); // -> (float)1 
float version = arguments.Get<float>(2, 99); // -> (float)99 
```
