using System.Reflection;
using System.Runtime.InteropServices;
using Test = Microsoft.VisualStudio.TestTools.UnitTesting;
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

[assembly: ComVisible(false)]

[assembly: Guid("30324a08-aa42-425d-87da-8f9c6af60454")]

[assembly: Test.Parallelize(Scope = Test.ExecutionScope.MethodLevel)]
