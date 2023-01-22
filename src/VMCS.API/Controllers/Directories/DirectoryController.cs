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
    public async Task<DirectoryDto> Create(CreateDirectoryDto directoryDto)
    {
       var directory = await _directoryService.Create(new Directory()
       {
           Name = directoryDto.Name,
           MeetingId = directoryDto.MeetingId,
           DirectoryInJson = directoryDto.DirectoryInJson
       });
       return _mapper.Map<DirectoryDto>(directory);
    }

    [HttpGet]
    public async Task<DirectoryDto> Get(string directoryId)
    {
        return _mapper.Map<DirectoryDto>(await _directoryService.Get(directoryId));
    }

    [HttpDelete]
    public async Task Delete(string directoryId)
    {
        await _directoryService.Delete(directoryId);
    }
}