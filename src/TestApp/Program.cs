// See https://aka.ms/new-console-template for more information
using Citizen17.DartSass;
using TestApp;

Console.WriteLine("Hello, World!");


var dartSass = new DartSassCompiler();

var tester = new Tester(dartSass);

tester.Compile().Wait();
