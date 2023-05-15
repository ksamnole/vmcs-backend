using Moq;
using VMCS.Core.Domains.CodeSharing.Directories;
using VMCS.Core.Domains.CodeSharing.Models;
using VMCS.Core.Domains.Directories.Services;
using Xunit;
using Directory = VMCS.Core.Domains.CodeSharing.Directories.Directory;

namespace VMCS.Core.Tests.Tests
{
    public class CodeSharingTests : TestBaseCodeSharing
    {
        [Fact]
        public void CreateFile_SuccessPath_ShouldCreateFile()
        {
            directory.CreateFile(file, 0);

            Assert.Contains(file, directory.RootFolder.Files);
        }

        [Fact]
        public void CreateFolder_SuccessPath_ShouldCreateFolder()
        {
            directory.CreateFolder(folder, 0);

            Assert.Contains(folder, directory.RootFolder.Folders);
        }

        [Fact]
        public void DeleteFolder_SuccessPath_ShouldCreateFolder()
        {
            directory.RootFolder.Folders.Add(folder);

            folder.Id = -1;

            directory.DeleteFolder(folder.Id);

            Assert.DoesNotContain(folder, directory.RootFolder.Folders);
        }

        [Fact]
        public void DeleteFile_SuccessPath_ShouldCreateFolder()
        {
            file.Id = -1;

            directory.RootFolder.Files.Add(file);

            directory.DeleteFile(file.Id);

            Assert.DoesNotContain(file, directory.RootFolder.Files);
        }

        [Fact]
        public void Save_SuccessPath_SaveCalledOneTime()
        {
            var directoryService = new Mock<IDirectoryService>();

            directory.Save(directoryService.Object);

            directoryService.Verify(s => s.Save(It.IsAny<Domains.Directories.Directory>()), Times.Once);
        }
    }

    public class TestBaseCodeSharing
    {
        public IDirectory directory = new Directory("testId", "testName");

        public TextFile file = new TextFile("fileName", "text");

        public Folder folder = new Folder("testName");
    }
}
