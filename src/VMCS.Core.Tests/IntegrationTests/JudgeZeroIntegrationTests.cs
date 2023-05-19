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

namespace VMCS.Core.Tests.IntegrationTests
{
    public class JudgeZeroIntegrationTests
    {

        private readonly JudgeZeroCodeExecutor judgeZeroCodeExecutor;
        public JudgeZeroIntegrationTests()
        {
            var clientExtra = new HttpClient();
            clientExtra.BaseAddress = new Uri("https://judge0-extra-ce.p.rapidapi.com");
            //clientExtra.DefaultRequestHeaders.Add("X-RapidAPI-Key", );
            clientExtra.DefaultRequestHeaders.Add("X-RapidAPI-Host", "judge0-extra-ce.p.rapidapi.com");

            var clientDefault = new HttpClient();
            clientDefault.BaseAddress = new Uri("https://judge0-ce.p.rapidapi.com");
            //clientDefault.DefaultRequestHeaders.Add("X-RapidAPI-Key", );
            clientDefault.DefaultRequestHeaders.Add("X-RapidAPI-Host", "judge0-ce.p.rapidapi.com");

            judgeZeroCodeExecutor = new JudgeZeroCodeExecutor(clientExtra, clientDefault);
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
