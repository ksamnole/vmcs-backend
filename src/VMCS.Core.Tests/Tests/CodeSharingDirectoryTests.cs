using Moq;
using System.Collections;
using System.IO.Compression;
using VMCS.Core.Domains.CodeSharing.Directories;
using VMCS.Core.Domains.CodeSharing.Models;
using VMCS.Core.Domains.Directories;
using VMCS.Core.Domains.Directories.Services;
using Xunit;
using Directory = VMCS.Core.Domains.CodeSharing.Directories.Directory;

namespace VMCS.Core.Tests.Tests
{
    public class CodeSharingDirectoryTests : TestBaseCodeSharing
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
        public void Save_SuccessPath_SaveCalledOneTime()
        {
            var directoryService = new Mock<IDirectoryService>();

            directory.Save(directoryService.Object);

            directoryService.Verify(s => s.Save(It.IsAny<DirectoryDataModel>()), Times.Once);
        }

        [Theory]
        [MemberData(nameof(TestDirectories.GetTestDirectories), MemberType = typeof(TestDirectories))]
        public void Save_SuccessPath_ArchiveContainesNFiles(IDirectory directory, int n)
        {
            var directoryService = new Mock<IDirectoryService>();

            byte[] zipInBytes = null;
            directoryService.Setup(s => s.Save(It.IsAny<DirectoryDataModel>()))
                .Callback<DirectoryDataModel>(d => zipInBytes = d.DirectoryZip);

            directory.Save(directoryService.Object);

            var zipArchive = new ZipArchive(new MemoryStream(zipInBytes));
            Assert.Equal(n, zipArchive.Entries.Count);
        }
    }

    public class TestBaseCodeSharing
    {
        public IDirectory directory = new Directory("testId", "testName");
        public TextFile file = new TextFile("fileName", "text");
        public Folder folder = new Folder("testName");

        public TestBaseCodeSharing() {

        }
    }

    public class TestDirectories
    {
        public static IEnumerable<object[]> GetTestDirectories()
        {
            var file1 = new TextFile("file1", "text1");
            var file2 = new TextFile("file2", "text2");

            var directoryWithFiles = new Directory("test1", "test1");
            directoryWithFiles.CreateFile(file1, 0);
            directoryWithFiles.CreateFile(file2, 0);

            var folder = new Folder("folder");
            var directoryWithFolder = new Directory("test2", "test2");
            directoryWithFolder.CreateFolder(folder, 0);

            var folder2 = new Folder("folder");
            var file3 = new TextFile("file3", "text3");
            var directoryWithFileInFolder = new Directory("test3", "test3");
            directoryWithFileInFolder.CreateFolder(folder2, 0);
            directoryWithFileInFolder.CreateFile(file3, folder2.Id);

            yield return new object[] { directoryWithFiles, 2 };
            yield return new object[] { directoryWithFolder, 1 };
            yield return new object[] { directoryWithFileInFolder, 2 };
        }
    }
}
