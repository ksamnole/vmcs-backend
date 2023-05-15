using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VMCS.API.Controllers.Directories.Dto;
using VMCS.Core.Domains.Directories;
using VMCS.Core.Domains.Directories.Services;

namespace VMCS.API.Controllers.Directories;

[ApiController]
[Route("directories")]
public class DirectoryController : ControllerBase
{
    private readonly IDirectoryService _directoryService;
    private readonly IMapper _mapper;

    public DirectoryController(IDirectoryService directoryService, IMapper mapper)
    {
        _directoryService = directoryService;
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<string> Create(CreateDirectoryDto directoryDto)
    {
       return await _directoryService.Create(new Directory()
       {
           Name = directoryDto.Name,
           MeetingId = directoryDto.MeetingId,
           DirectoryInJson = directoryDto.DirectoryInJson
       });
    }

    [HttpGet("{directoryId}")]
    public async Task<DirectoryDto> Get(string directoryId)
    {
        var directory = await _directoryService.Get(directoryId);
        var directoryZip = Convert.ToBase64String(directory.DirectoryZip);
        
        return new DirectoryDto
        {
            DirectoryZip = directoryZip,
            DirectoryInJson = directory.DirectoryInJson,
            MeetingId = directory.MeetingId,
            Name = directory.Name
        };
    }

    [HttpDelete]
    public async Task Delete(string directoryId)
    {
        await _directoryService.Delete(directoryId);
    }

    [HttpGet("{directoryId}/execute")]
    public async Task<string> Execute(string directoryId)
    {
        return await _directoryService.Execute(directoryId);
    }
}