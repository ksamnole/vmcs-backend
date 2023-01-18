using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using VMCS.Core.Domains.CodeSharing.Models;
using VMCS.Core.Domains.FileRepositories;
using VMCS.Core.Domains.Meetings.Services;

namespace VMCS.Core.Domains.CodeSharing
{
    internal class CodeSharing : ICodeSharing
    {
        private Dictionary<string, List<string>> _reposOfConnections = new Dictionary<string, List<string>>();
        private Dictionary<string, IFileRepository> _repositories = new Dictionary<string, IFileRepository>();
        private IMeetingService _meetingService;

        public CodeSharing(IMeetingService meetingService) 
        {
            _meetingService = meetingService;
        }

        public void Change(string text, string repositoryId, int fileId, string connectionId)
        {
            _repositories[repositoryId].ChangeFile(text, fileId);
            //if (!_reposOfConnections[connectionId].Contains(change.RepoId))
            //    throw new Exception("Changing files in not connected repository");

            //var chint = change.InsertedChars;
            //var text = _repositoryFiles[change.RepoId][change.FileId].TextInBytes;

            //if (change.Action == ActionEnum.Insert)
            //{
            //    text = text.Take(change.Position).Concat(chint).Concat(text.Skip(change.Position)).ToList();
            //}
            //if (change.Action == ActionEnum.Delete)
            //{
            //    text = text.Take(change.Position).Concat(text.Skip(change.Position + change.CharsDeleted)).ToList();
            //}
        }

        public void ConnectToRepository(string repositoryId, string connectionId)
        {
            _reposOfConnections[connectionId].Add(repositoryId);
        }

        public void CreateFolder(string folderName, string repositoryId, int parentFolderId, string connectionId)
        {
            if (!_reposOfConnections[connectionId].Contains(repositoryId))
                throw new Exception("Creating folder in not connected repository");

            _repositories[repositoryId].CreateFolder(folderName, parentFolderId);
        }

        public async Task<FileRepository> CreateRepository(string meetingId, string repositoryName, string connectionId)
        {
            var entity = _repositories.Values.FirstOrDefault(r => r.MeetingId == meetingId);

            if (entity is not null)
                throw new ArgumentException($"Repository in meeting with id: {meetingId} already exists");

            var repository = new FileRepository(meetingId, repositoryName);

            CancellationToken cancellationToken = new CancellationTokenSource().Token;
            await _meetingService.SetRepositoryToMeeting(repository.Id, meetingId, cancellationToken);

            _reposOfConnections[connectionId].Add(repository.Id);

            _repositories.Add(repository.Id, repository);

            return repository;
        }

        public async Task SaveRepository(string repositoryId)
        {
            await _repositories[repositoryId].Save();
        }

        public void Upload(TextFile file, int folderId, string repositoryId, string connectionId)
        {
            if (!_reposOfConnections[connectionId].Contains(repositoryId))
                throw new Exception("Uploading file to not connected repository");

            _repositories[repositoryId].UploadFile(folderId, file);
        }

        public void AddConnection(string connectionId)
        {
            if (_reposOfConnections.ContainsKey(connectionId))
                throw new ArgumentException($"Connection with id : {connectionId} already connected.");
            _reposOfConnections.Add(connectionId, new List<string>());
        }

        public void RemoveConnection(string connectionId)
        {
            if (!_reposOfConnections.ContainsKey(connectionId))
                throw new ArgumentException($"There is no connection with id : {connectionId}");

            _reposOfConnections.Remove(connectionId);
        }

        public async Task<IFileRepository> GetRepositoryById(string repositoryId)
        {
            return _repositories[repositoryId];
        }
    }
}
