﻿namespace VMCS.Core.Domains.GitHub.Models;

public class PushToRepository
{
    public string RepositoryName { get; set; }
    public string DirectoryId { get; set; }
    public string Message { get; set; }
    public string UserId { get; set; }
}