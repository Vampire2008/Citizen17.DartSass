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
            foreach (var item in result)
                Console.WriteLine(item);

            result = await _compiler.CompileToFileAsync("./TestSheet.scss", "./MyOutput.css", new SassCompileOptions { });
            foreach (var item in result)
                Console.WriteLine(item);

            var code = File.ReadAllText("./TestSheet.scss");
            Console.WriteLine(await _compiler.CompileCodeAsync(code));

            try
            {
                await _compiler.CompileAsync("./ErrorSheet.scss");
            }
            catch (SassCompileException ex)
            {
                Console.WriteLine(ex.ErrorMessage);
                Console.WriteLine(ex.ErrorPosition);
            }


            result = await _compiler.CompileToFilesAsync(new[] { "./TestSheet.scss", "./TestSheet2.scss" }, "./out");
            foreach (var item in result)
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
            foreach (var item in result)
                Console.WriteLine(item);

            Console.WriteLine();
        }
    }
}
