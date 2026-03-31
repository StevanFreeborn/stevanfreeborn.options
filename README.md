# StevanFreeborn.Options

[![NuGet](https://img.shields.io/nuget/v/StevanFreeborn.Options.svg)](https://www.nuget.org/packages/StevanFreeborn.Options)
[![NuGet](https://img.shields.io/nuget/dt/StevanFreeborn.Options.svg)](https://www.nuget.org/packages/StevanFreeborn.Options)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

A minimalistic, AOT-compatible Option type library for functional programming in .NET.

## Features

- **Functional Programming**: Chain operations with Map, Bind, and Match
- **AOT Compatible**: Works with Native AOT and trimming
- **Nullable Reference Types**: Full support for nullable reference types
- **No External Dependencies**: Lightweight with zero dependencies
- **Pattern Matching Support**: Deconstruction support for modern C# pattern matching
- **Comprehensive XML Documentation**: Full IntelliSense support

## Installation

Install via NuGet:

```bash
dotnet add package StevanFreeborn.Options
```

## Quick Start

```csharp
using StevanFreeborn.Options;

// Create an option with a value
Option<string> some = Option<string>.Some("Hello");

// Create an empty option
Option<string> none = Option<string>.None();

// Implicit conversion from value
Option<string> implicitSome = "World";

// Use functional operations
string result = some
  .Map(s => s.ToUpper())
  .Match(
    onSome: s => $"Value: {s}",
    onNone: () => "No value"
  );

// Pattern matching
if (some is (true, var value))
{
  Console.WriteLine(value);
}
```

## Requirements

- .NET Standard 2.1
- .NET 10.0+

## License

MIT License - see [LICENSE.md](LICENSE.md) for details.
