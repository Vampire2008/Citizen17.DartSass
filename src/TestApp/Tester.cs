using Citizen17.DartSass;

namespace TestApp
{
    internal class Tester
    {
        private readonly DartSassCompiler _compiler;

        public Tester(DartSassCompiler compiler)
        {
            _compiler = compiler;
        }

        public async Task Compile()
        {

            Console.WriteLine(await _compiler.GetVersionAsync());

            var result = await _compiler.CompileToFileAsync("./TestSheet.scss");
            foreach (var item in result.Files)
                Console.WriteLine(item);

            Console.WriteLine();

            result = await _compiler.CompileToFileAsync("./TestSheet.scss", "./MyOutput.css", new SassCompileOptions
            {
                ImportPaths = ["path1", "path2"],
                SilenceDeprecation = [SassDeprecations.CssFunctionMixing],
                FutureDeprecation = [SassDeprecations.Future.Import]
            });

            foreach (var item in result.Files)
                Console.WriteLine(item);

            Console.WriteLine();

            var code = File.ReadAllText("./TestSheet.scss");
            Console.WriteLine((await _compiler.CompileCodeAsync(code, new SassCompileOptions() { EmitCharset = false })).Code);

            Console.WriteLine();

            try
            {
                await _compiler.CompileAsync("./ErrorSheet.scss");
            }
            catch (SassCompileException ex)
            {
                foreach (var error in ex.Errors)
                {
                    Console.WriteLine(error.Message);
                    Console.WriteLine(error.StackTrace);
                }
            }

            Console.WriteLine();


            result = await _compiler.CompileToFilesAsync(new[] { "./TestSheet.scss", "./TestSheet2.scss" }, "./out");
            foreach (var item in result.Files)
                Console.WriteLine(item);

            Console.WriteLine();

            result = await _compiler.CompileToFilesAsync(new Dictionary<string, string>
            {
                {"./TestSheet.scss", null },
                {"./TestSheet2.scss", "MySheet2.css" },
                {"./Testsheet3.scss", "./out2/" },
                {"./TestSheet4.scss", "./out3/MySheet2.css" }
            },
            "./out");
            foreach (var item in result.Files)
                Console.WriteLine(item);

            Console.WriteLine();

            var result2 = await _compiler.CompileAsync("./WarnSheet.scss");

            Console.WriteLine("Warnings:");
            foreach (var warn in result2.Warnings)
            {
                Console.WriteLine(warn.Message);
                Console.WriteLine(warn.StackTrace);
                Console.WriteLine();
            }

            Console.WriteLine("Deprecation warnings:");
            foreach (var deprecationWarning in result2.DeprecationWarnings)
            {
                Console.WriteLine(deprecationWarning.Message);
                Console.WriteLine(deprecationWarning.Recommendation);
                Console.WriteLine(deprecationWarning.StackTrace);
                Console.WriteLine();
            }

            Console.WriteLine("Debug");
            foreach (var message in result2.Debug)
            {
                Console.WriteLine(message.Message);
                Console.WriteLine(message.StackTrace);
                Console.WriteLine();
            }

            Console.WriteLine();

            try
            {
                await _compiler.CompileToFilesAsync(new [] { "./TestSheet.scss", "./ErrorSheet.scss", "./ErrorSheet2.scss" });
            }
            catch (SassCompileException ex)
            {
                foreach (var error in ex.Errors)
                {
                    Console.WriteLine(error.Message);
                    Console.WriteLine(error.StackTrace);
                }
            }
        }
    }
}
