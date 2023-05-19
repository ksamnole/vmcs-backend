using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMCS.Core.Domains.CodeExecution.Enums;
using VMCS.Core.Extensions;
using VMCS.Data.HttpClients.CodeExecution.JudgeZero;
using Xunit;

namespace VMCS.IntegrationTests
{
    public class JudgeZeroIntegrationTests : IntegrationTestBase
    {

        private readonly JudgeZeroCodeExecutor judgeZeroCodeExecutor;
        public JudgeZeroIntegrationTests()
        {
            var clientFactory = (IHttpClientFactory)_webApplicationFactory.Services.GetService(typeof(IHttpClientFactory));

            judgeZeroCodeExecutor = new JudgeZeroCodeExecutor(clientFactory);
        }

        [Fact]
        public async void Execute_PythonMultifile_Success()
        {
            var main = "import greeting\nprint(greeting.greeting)";
            var greeting = "greeting = \"Hello world!\"";

            using var memoryStream = new MemoryStream();
            var zipArchive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true);

            zipArchive.AddTextFile("main.py", main);
            zipArchive.AddTextFile("greeting.py", greeting);


            zipArchive.Dispose();

            var result = await judgeZeroCodeExecutor.ExecuteAsync(memoryStream.ToArray(), Language.Python);
            Assert.Equal("Hello world!\n", result);
        }

        [Fact]
        public async void Execute_CSharpMultifile_Success()
        {
            var programFile =
                @"using System;
                  namespace MyProgram{
                    class Program{
                        static void Main(string[] args){
                            Console.WriteLine(Answers.MainAnswer);
                        }
                    }
                }";

            var answer =
                @"namespace MyProgram{
                    public static class Answers{
                        public static int MainAnswer = 42;
                    }
                }
                ";

            var memoryStream = new MemoryStream();

            var zipArhive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true);

            zipArhive.AddTextFile("Program.cs", programFile);
            zipArhive.AddTextFile("Answers.cs", answer);

            zipArhive.Dispose();

            var result = await judgeZeroCodeExecutor.ExecuteAsync(memoryStream.ToArray(), Language.Csharp);

            Assert.Equal("42\n", result);
        }
    }
}
