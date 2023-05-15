using System.IO.Compression;
using VMCS.Core.Domains.CodeExecution.Enums;

namespace VMCS.Core.Domains.CodeExecution.HttpClients;

public interface ICodeExecutor
{
    public Task<string> ExecuteAsync(ZipArchive zipArchive, Language language);
}