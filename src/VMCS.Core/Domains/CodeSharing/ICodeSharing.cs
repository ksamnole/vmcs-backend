﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMCS.Core.Domains.CodeSharing.Models;
using VMCS.Core.Domains.FileRepositories;
using VMCS.Core.Domains.Meetings.Services;
using VMCS.Core.Domains.Messages.Services;

namespace VMCS.Core.Domains.CodeSharing
{
    public interface ICodeSharing
    {
        void Upload(TextFile file, int folderId, string repsitoryoId, string connectionId);
        Task<FileRepository> CreateRepository(string meetingId, string repositoryName, string connectionId, IMeetingService meetingService);
        void ConnectToRepository(string repositoryId, string connectionId);
        Task SaveRepository(string repositoryId);
        void CreateFolder(string folderName, string repositoryId, int parentFolderId, string connectionId);
        void Change(string text, string repositoryId, int fileId, string connectionId);
        void AddConnection(string connectionId);
        void RemoveConnection(string connectionId);
        Task<IFileRepository> GetRepositoryById(string repositoryId);
    }
}
