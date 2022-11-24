using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Citizen17.DartSass.Tests
{
    internal static class ScssTestFiles
    {
        internal const string TestSheetScss = "./TestSourceFiles/TestSheet.scss";

        internal static readonly Dictionary<TestFileKey, string> ExpectedResults = new()
        {
            { 
                new(TestSheetScss, false),
                """
                .my-class {
                  color: red;
                }
                .my-class.active {
                  color: green;
                }
                .my-class .text {
                  text-align: right;
                }
                """
            },
            {
                new(TestSheetScss, true),
                $$"""
                .my-class {
                  color: red;
                }
                .my-class.active {
                  color: green;
                }
                .my-class .text {
                  text-align: right;
                }

                /*# sourceMappingURL=data:application/json;charset=utf-8,%7B%22version%22:3,%22sourceRoot%22:%22%22,%22sources%22:%5B%22file:///{{Path.Combine(Environment.CurrentDirectory,"TestSourceFiles/TestSheet.scss").Replace("\\","/")}}%22%5D,%22names%22:%5B%5D,%22mappings%22:%22AAAA;EACI;;AAEA;EACI;;AAGJ;EACI%22%7D */
                """
            }
        };
    }
}
