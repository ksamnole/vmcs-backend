namespace VMCS.Data.HttpClients.CodeExecution.JudgeZero;

public static class AdditionalFiles
{
    //public static string CsharpCompile =
    //    @"ls .
    //      mv Program.cs getbusylivingorgetbusydying
    //      ls.
    //      dotnet new console -n MyProgram -o .
    //      ls .
    //      mv -f getbusylivingorgetbusydying Program.cs
    //      dotnet build MyProgram.csproj -o Result";
    public static string CsharpCompile ="ls .\nmv Program.cs getbusylivingorgetbusydying\nls .\ndotnet new console -n MyProgram -o .\nls .\nmv -f getbusylivingorgetbusydying Program.cs\nls .\ncat Program.cs\ndotnet build MyProgram.csproj -o Result";
    public static string CsharpRun = "dotnet Result/MyProgram.dll";
    public static string PythonRun = "/usr/local/python-3.8.1/bin/python3 main.py";
}