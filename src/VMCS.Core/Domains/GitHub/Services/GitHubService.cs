using System.Net.Http.Json;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using VMCS.Core.Domains.CodeSharing.Models;
using VMCS.Core.Domains.Directories.Services;
using VMCS.Core.Domains.GitHub.HttpClients;
using VMCS.Core.Domains.GitHub.HttpClients.Models;
using VMCS.Core.Domains.GitHub.Models;
using VMCS.Core.Domains.GitHub.Repositories;

namespace VMCS.Core.Domains.GitHub.Services;

public class GitHubService : IGitHubService
{
    private readonly IGitHubRepository _gitHubRepository;
    private readonly IDirectoryService _directoryService;
    private readonly IGitHubSignIn _gitHubSignIn;
    private readonly IGitHubApi _gitHubApi;
    private readonly IUnitOfWork _unitOfWork;

    public GitHubService(IGitHubRepository gitHubRepository, IUnitOfWork unitOfWork, IGitHubSignIn gitHubSignIn, IGitHubApi gitHubApi, IDirectoryService directoryService)
    {
        _gitHubRepository = gitHubRepository;
        _unitOfWork = unitOfWork;
        _gitHubSignIn = gitHubSignIn;
        _gitHubApi = gitHubApi;
        _directoryService = directoryService;
    }

    public async Task Create(AccessToken accessToken)
    {
        await _gitHubRepository.Create(accessToken);
        await _unitOfWork.SaveChange();
    }

    public async Task SignIn(string userId, FormUrlEncodedContent data)
    {
        var token = await _gitHubSignIn.SignIn(data);

        await Create(new AccessToken()
        {
            Token = token,
            UserId = userId
        });
    }

    private async Task<AccessToken> GetToken(string userId)
    {
        return await _gitHubRepository.GetToken(userId);
    }

    public async Task CreateRepository(CreateRepository createRepository)
    {
        var accessToken = await GetToken(createRepository.UserId);
        var json = new JObject
        {
            ["name"] = createRepository.Name,
            ["auto_init"] = true
        };
        var content = new StringContent(json.ToString(), Encoding.UTF8, "application/json");
        await _gitHubApi.CreateRepository("/user/repos", accessToken.Token, content);
    }

    public async Task PushToRepository(PushToRepository pushToRepository)
    {
        var accessToken = await GetToken(pushToRepository.UserId);
        var repo = GetRepositoryNameGitHubStyle(pushToRepository.RepositoryName);
        
        var isRepositoryCreated = await IsRepositoryCreated(repo, accessToken.Token);
        
        if (!isRepositoryCreated)
        {
            await CreateRepository(new CreateRepository()
            {
                Name = pushToRepository.RepositoryName,
                UserId = pushToRepository.UserId
            });
        }
        
        var owner = await _gitHubApi.GetUserLogin("/user", accessToken.Token);
        var branch = await _gitHubApi.GetMainBranchName($"/repos/{owner}/{repo}/branches", accessToken.Token);
        var directory = await _directoryService.Get(pushToRepository.DirectoryId);

        if (string.IsNullOrEmpty(directory.DirectoryInJson))
            return;

        var folder = JsonConvert.DeserializeObject<Folder>(directory.DirectoryInJson);
        var folderTrees = CreateFolderTrees(folder);
        
        var shaBaseTree = await _gitHubApi.GetShaBaseTree($"/repos/{owner}/{repo}/git/trees/{branch}", accessToken.Token);
        var treeContent = await CreateTree($"/repos/{owner}/{repo}/git/blobs", shaBaseTree, folderTrees, accessToken.Token);
        var shaTree = await _gitHubApi.GetShaTree($"/repos/{owner}/{repo}/git/trees", treeContent, accessToken.Token);
        var shaParent = await _gitHubApi.GetShaParent($"/repos/{owner}/{repo}/git/refs/heads/{branch}", accessToken.Token);
        
        var commitData = JsonContent.Create(new
        {
            tree=shaTree,
            message=pushToRepository.Message,
            parents=new[] {shaParent}
        });
        var shaCommit = await _gitHubApi.GetShaCommit($"/repos/{owner}/{repo}/git/commits", commitData, accessToken.Token);
        
        var patchData = JsonContent.Create(new
        {
            sha=shaCommit
        });
        await _gitHubApi.UpdateRef($"/repos/{owner}/{repo}/git/refs/heads/{branch}", patchData, accessToken.Token);
    }

    private string GetRepositoryNameGitHubStyle(string repositoryName)
    {
        return repositoryName.Replace(' ', '-');
    }

    private async Task<bool> IsRepositoryCreated(string repositoryName, string token)
    {
        var userRepositoriesNames = await _gitHubApi.GetAllUserRepositoriesNames("/users/ksamnole/repos", token);

        return userRepositoriesNames.Any(x => x == repositoryName);
    }
    
    public static List<FolderTree> CreateFolderTrees(Folder folder)
    {
        var folderTrees = new List<FolderTree>();
        CreateFolderTreesFromFolders(folderTrees, folder, "");
        return folderTrees;
    }

    private async Task<StringContent> CreateTree(string url, string shaBaseTree, List<FolderTree> folderTrees, string token)
    {
        var files = new JArray { await GetCustomReadmeFile(url, token) };

        foreach (var folderTree in folderTrees)
        {
            var blob = JsonContent.Create(new { content = folderTree.Content, encoding = "utf-8" });
            var path = folderTree.Path[0] == '/'
                ? folderTree.Path.Substring(1, folderTree.Path.Length - 1)
                : folderTree.Path;
            var newFile = new JObject
            {
                ["path"] = path,
                ["mode"] = "100644",
                ["type"] = "blob",
                ["sha"] = await _gitHubApi.GetShaBlob(url, blob, token)
            };
            files.Add(newFile);
        }
        var json = new JObject
        {
            ["base_tree"] = shaBaseTree,
            ["tree"] = files
        };

        var content = new StringContent(json.ToString(), Encoding.UTF8, "application/json");
        
        return content;
    }

    private async Task<JObject> GetCustomReadmeFile(string url, string token)
    {
        const string text = "<h1 align=\"center\">Репозиторий был создан при помощи приложения <a href=\"https://vmcs.space\">VMCS</a></h1>";
        var blob = JsonContent.Create(new { content = text, encoding = "utf-8" });
        var file = new JObject
        {
            ["path"] = "README.md",
            ["mode"] = "100644",
            ["type"] = "blob",
            ["sha"] = await _gitHubApi.GetShaBlob(url, blob, token)
        };
        return file;
    }

    private static void CreateFolderTreesFromFiles(List<FolderTree> folderTrees, IEnumerable<TextFile> files, string path)
    {
        folderTrees.AddRange(files.Select(file => new FolderTree() { Content = file.Text, Path = path + $"/{file.Name}" }));
    }
    
    private static void CreateFolderTreesFromFolders(List<FolderTree> folderTrees, Folder folder, string parentPath)
    {
        CreateFolderTreesFromFiles(folderTrees, folder.Files, parentPath);
        
        foreach (var subFolder in folder.Folders)
        {
            CreateFolderTreesFromFolders(folderTrees, subFolder, parentPath + $"/{subFolder.Name}");
        }
    }
}