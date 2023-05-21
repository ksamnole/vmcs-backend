using System.Net.Http.Json;
using Newtonsoft.Json;
using VMCS.Data.HttpClients.Models.Responses;

namespace VMCS.Data.HttpClients.CodeSharing;

public class CodeSharingApi
{
    private readonly HttpClient _httpClient;

    public CodeSharingApi(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string> GetDoc(string url, JsonContent data)
    {
        var response = await _httpClient.PostAsync(url, data);

        var doc = await GetDeserializeObject<GetDocResponse>(response);

        return doc.Doc;
    }

    public async Task<string> GetChanges(string url, StringContent data)
    {
        var response = await _httpClient.PostAsync(url, data);

        return await GetDeserializeObject<string>(response);
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
}