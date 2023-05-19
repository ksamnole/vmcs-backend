using System.Buffers.Text;
using System.IO.Compression;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using VMCS.Core.Domains.CodeExecution.Enums;
using VMCS.Core.Domains.CodeExecution.HttpClients;
using VMCS.Core.Extensions;
using VMCS.Data.HttpClients.Models.Responses;

namespace VMCS.Data.HttpClients.CodeExecution.JudgeZero;

public class JudgeZeroCodeExecutor : ICodeExecutor
{
    private const int LanguageId = 89;

    private readonly HttpClient _httpClientExtra;
    private readonly HttpClient _httpClientDefault;
    
    private readonly IHttpClientFactory _httpClientFactory;

    public JudgeZeroCodeExecutor(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
        _httpClientExtra = httpClientFactory.CreateClient("JudgeZeroExtra");
        _httpClientDefault = httpClientFactory.CreateClient("JudgeZeroDefault");
    }
    
    public async Task<string> ExecuteAsync(byte[] zipArchiveInBytes, Language language)
    {
        var memoryStream = new MemoryStream();
        memoryStream.Write(zipArchiveInBytes);
        var zipArchive = new ZipArchive(memoryStream, ZipArchiveMode.Update, true);

        var httpClient = GetHttpClient(language);
        
        await AddAdditionalFiles(zipArchive, language);

        zipArchive.Dispose();

        var newZipArchive = new ZipArchive(memoryStream);

        var json = new JObject
        {
            ["language_id"] = LanguageId,
            ["additional_files"] = Convert.ToBase64String(memoryStream.ToArray())
    };
        var content = new StringContent(json.ToString(), Encoding.UTF8, "application/json");

        var submissionId = await CreateSubmission(content, language, httpClient);

        return await GetSubmission(submissionId, httpClient);
    }

    private static async Task<string> GetSubmission(string submissionId, HttpClient httpClient)
    {
        var response = await httpClient.GetAsync($"/submissions/{submissionId}");

        var obj = new { Stdout = "", Status = new { Id = 0, Description = "" } };

        var deserializeObject = JsonConvert.DeserializeAnonymousType(await response.Content.ReadAsStringAsync(), obj);

        if (deserializeObject.Status.Id == 2)
        {
            await Task.Delay(2000);
            return await GetSubmission(submissionId, httpClient);
        }

        return deserializeObject.Stdout;
    }

    private static async Task<string> CreateSubmission(StringContent data, Language language, HttpClient httpClient)
    {
        var response = await httpClient.PostAsync("/submissions", data);

        var deserializeObject = await GetDeserializeObject<SubmissionTokenResponse>(response);

        return deserializeObject.Token;
    }

    private static async Task AddAdditionalFiles(ZipArchive zipArchive, Language language)
    {

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


        if (!string.IsNullOrEmpty(compile))
        {
            zipArchive.AddTextFile("compile", compile);
        }
        if (!string.IsNullOrEmpty(run))
        {
            zipArchive.AddTextFile("run", run);
        }
    }
    
    private static async Task<T> GetDeserializeObject<T>(HttpResponseMessage responseMessage)
    {
        var responseContent = await responseMessage.Content.ReadAsStringAsync();

        if (!responseMessage.IsSuccessStatusCode)
            throw new Exception($"Judge0 bad request {responseContent}");

        var deserializeObject = JsonConvert.DeserializeObject<T>(responseContent);

        if (deserializeObject is null)
            throw new Exception($"Failed to deserialize response {responseContent}");

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
}