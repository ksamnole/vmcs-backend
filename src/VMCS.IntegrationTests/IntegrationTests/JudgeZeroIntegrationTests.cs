using System.IO.Compression;
using VMCS.Core.Domains.CodeExecution.Enums;
using VMCS.Core.Extensions;
using VMCS.Data.HttpClients.CodeExecution.JudgeZero;

namespace VMCS.IntegrationTests.IntegrationTests;

public class JudgeZeroIntegrationTests : IntegrationTestBase
{
    private readonly JudgeZeroCodeExecutor _judgeZeroCodeExecutor;

    public JudgeZeroIntegrationTests()
    {
        var clientFactory = (IHttpClientFactory)_webApplicationFactory.Services.GetService(typeof(IHttpClientFactory));

        _judgeZeroCodeExecutor = new JudgeZeroCodeExecutor(clientFactory);
    }

    [Fact]
    public async void Execute_PythonCodeWithMultipleFiles_Success()
    {
        const string main = "import greeting\nprint(greeting.greeting)";
        const string greeting = "greeting = \"Hello world!\"";

        var memoryStream = GetMemoryStreamFromFiles(new List<File>
        {
            new() { Name = "main.py", Text = main },
            new() { Name = "greeting.py", Text = greeting }
        });

        var result = await _judgeZeroCodeExecutor.ExecuteAsync(memoryStream.ToArray(), Language.Python);
        Assert.Equal("Hello world!\n", result);
    }

    [Fact]
    public async void Execute_CSharpCodeWithMultipleFiles_Success()
    {
        const string programFile = @"using System;
                  namespace MyProgram{
                    class Program{
                        static void Main(string[] args){
                            Console.WriteLine(Answers.MainAnswer);
                        }
                    }
                }";

        const string answer = @"namespace MyProgram{
                    public static class Answers{
                        public static int MainAnswer = 42;
                    }
                }
                ";

        var memoryStream = GetMemoryStreamFromFiles(new List<File>
        {
            new() { Name = "Program.cs", Text = programFile },
            new() { Name = "Answers.cs", Text = answer }
        });

        var result = await _judgeZeroCodeExecutor.ExecuteAsync(memoryStream.ToArray(), Language.Csharp);

        Assert.Equal("42\n", result);
    }

    private static MemoryStream GetMemoryStreamFromFiles(IEnumerable<File> files)
    {
        var memoryStream = new MemoryStream();

        var zipArchive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true);

        foreach (var file in files)
            zipArchive.AddTextFile(file.Name, file.Text);

        zipArchive.Dispose();

        return memoryStream;
    }
}