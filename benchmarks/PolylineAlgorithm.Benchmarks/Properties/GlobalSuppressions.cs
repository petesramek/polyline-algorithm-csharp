// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores", Justification = "Benchmarks.")]
[assembly: SuppressMessage("Performance", "CA1819:Properties should not return arrays", Justification = "Benchmarks.")]
[assembly: SuppressMessage("Design", "MA0016:Prefer using collection abstraction instead of implementation", Justification = "Benchmarks.")]
[assembly: SuppressMessage("Design", "CA1002:Do not expose generic lists", Justification = "Benchmarks.")]
[assembly: SuppressMessage("Naming", "CA1720:Identifier contains type name", Justification = "Benchmarks.")]
[assembly: SuppressMessage("Maintainability", "CA1515:Consider making public types internal", Justification = "Benchmarks.")]
[assembly: SuppressMessage("Security", "CA5394:Do not use insecure randomness", Justification = "Benchmarks.")]
[assembly: SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Benchmarks need instance methods.")]