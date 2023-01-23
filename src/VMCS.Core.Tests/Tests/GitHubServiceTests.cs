using VMCS.Core.Domains.CodeSharing.Models;
using VMCS.Core.Domains.GitHub.Services;
using Xunit;

namespace VMCS.Core.Tests.Tests;

public class GitHubServiceTests
{
    [Fact]
    public void CreateFolderTrees_WithNotEmptyFolder_SuccessPath()
    {
        var folder = new Folder()
        {
            Id = 1,
            IsDeleted = false,
            Name = "Folder",
            Files = new List<TextFile>()
            {
                new TextFile()
                {
                    Id = 2,
                    IsDeleted = false,
                    Name = "File1.txt",
                    Text = "Hello, world!"
                },
                new TextFile()
                {
                    Id = 3,
                    IsDeleted = false,
                    Name = "File2.txt",
                    Text = "Goodbye, heaven!"
                }
            },
            Folders = new List<Folder>()
            {
                new Folder()
                {
                    Id = 4,
                    IsDeleted = false,
                    Name = "Folder2",
                    Files = new List<TextFile>()
                    {
                        new TextFile()
                        {
                            Id = 5,
                            IsDeleted = false,
                            Name = "File3.txt",
                            Text = ":)"
                        }
                    }
                }
            }
        };

        var result = GitHubService.CreateFolderTrees(folder);
    }
}