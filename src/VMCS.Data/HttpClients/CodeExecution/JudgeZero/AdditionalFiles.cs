namespace VMCS.Data.HttpClients.CodeExecution.JudgeZero;

public static class AdditionalFiles
{
    public static string CsharpCompile = "dotnet build -o \"./Result\"";
    public static string CsharpRun = "dotnet Result/ConsoleApp1.dll";
}