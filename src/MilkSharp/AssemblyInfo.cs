using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("MilkSharp.Test")]

// for mocking internal interfaces with Moq
// see. https://stackoverflow.com/questions/28234369/how-to-do-internal-interfaces-visible-for-moq
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]