// See https://aka.ms/new-console-template for more information
using Citizen17.DartSass;
using TestApp;

Console.WriteLine("Hello, World!");


var dartSass = new DartSassCompiler(DartSassNativeType.WinX64);

var tester = new Tester(dartSass);

try
{
    tester.Compile().Wait();
}
catch (Exception e)
{
    throw;
}
