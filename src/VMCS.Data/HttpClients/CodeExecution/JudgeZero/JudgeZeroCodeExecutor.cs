using System.IO.Compression;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using VMCS.Core.Domains.CodeExecution.Enums;
using VMCS.Core.Domains.CodeExecution.HttpClients;
using VMCS.Data.HttpClients.Models.Responses;

namespace VMCS.Data.HttpClients.CodeExecution.JudgeZero;

public class JudgeZeroCodeExecutor : ICodeExecutor
{
    private const int LanguageId = 89;
    private const string JudgeZeroExtraApiUri = "https://judge0-extra-ce.p.rapidapi.com";
    
    private readonly HttpClient _httpClientExtra;
    private readonly HttpClient _httpClientDefault;

    public JudgeZeroCodeExecutor(IEnumerable<HttpClient> httpClients)
    {
        var httpClient = httpClients.First();
        
        if (IsExtraApi(httpClient.BaseAddress))
        {
            _httpClientExtra = httpClient;
            _httpClientDefault = httpClients.Skip(1).First();
        }
        else
        {
            _httpClientExtra = httpClients.Skip(1).First();
            _httpClientDefault = httpClient;
        }
    }
    
    public async Task<string> ExecuteAsync(ZipArchive zipArchive, Language language)
    {
        var httpClient = GetHttpClient(language);
        
        await AddAdditionalFiles(zipArchive, language);
        
        var json = new JObject
        {
            ["language_id"] = LanguageId,
            ["additional_files"] = ConvertToBase64(zipArchive)
        };
        var content = new StringContent(json.ToString(), Encoding.UTF8, "application/json");

        var submissionId = await CreateSubmission(content, language, httpClient);

        return await GetSubmission(submissionId, httpClient);
    }

    private static async Task<string> GetSubmission(string submissionId, HttpClient httpClient)
    {
        var response = await httpClient.GetAsync($"/submissions/{submissionId}");
        
        var body = await response.Content.ReadAsStringAsync();
        
        return body;
    }

    private static async Task<string> CreateSubmission(StringContent data, Language language, HttpClient httpClient)
    {
        var response = await httpClient.PostAsync("/submissions", data);

        var deserializeObject = await GetDeserializeObject<SubmissionResponse>(response);

        return deserializeObject.SubmissionId;
    }

    private static string ConvertToBase64(ZipArchive zipArchive)
    {
        using var memoryStream = new MemoryStream();
        using (var newZipArchive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
        {
            foreach (var entry in zipArchive.Entries)
            {
                using var entryStream = entry.Open();
                var newEntry = newZipArchive.CreateEntry(entry.FullName, CompressionLevel.Optimal);
                using var newEntryStream = newEntry.Open();
                entryStream.CopyTo(newEntryStream);
            }
        }

        var zipBytes = memoryStream.ToArray();
        return Convert.ToBase64String(zipBytes);
    }

    private static async Task AddAdditionalFiles(ZipArchive zipArchive, Language language)
    {
        var entryCompile = zipArchive.CreateEntry("compile");
        var entryRun = zipArchive.CreateEntry("run");

        var compile = string.Empty;
        var run = string.Empty;

        switch (language)
        {
            case Language.Csharp:
                compile = AdditionalFiles.CsharpCompile;
                run = AdditionalFiles.CsharpRun;
                break;
            case Language.Python:
                run = AdditionalFiles.PythonRun;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(language), language, null);
        }

        if (string.IsNullOrEmpty(compile))
        {
            await using var writerCompile = new StreamWriter(entryCompile.Open());
            await writerCompile.WriteAsync(compile);
        }

        if (string.IsNullOrEmpty(run))
        {
            await using var writerRun = new StreamWriter(entryRun.Open());
            await writerRun.WriteAsync(run);
        }
    }
    
    private static async Task<T> GetDeserializeObject<T>(HttpResponseMessage requestMessage)
    {
        if (!requestMessage.IsSuccessStatusCode)
            throw new Exception();
        
        var responseContent = await requestMessage.Content.ReadAsStringAsync();
        var deserializeObject = JsonConvert.DeserializeObject<T>(responseContent);

        if (deserializeObject is null)
            throw new Exception();

        return deserializeObject;
    }

    private HttpClient GetHttpClient(Language language)
    {
        return language switch
        {
            Language.Csharp => _httpClientExtra,
            Language.Python => _httpClientDefault,
            _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
        };
    }

    private static bool IsExtraApi(Uri baseAddress)
    {
        return baseAddress.ToString().Contains("extra");
    }
}