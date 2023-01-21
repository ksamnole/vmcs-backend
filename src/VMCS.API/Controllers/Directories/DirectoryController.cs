using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Directory = VMCS.Core.Domains.Directories.Directory;
using VMCS.Core.Domains.Directories.Services;

namespace VMCS.API.Controllers.Directories
{

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
        public async Task<string> Create(CreateDirectoryDto directoryDTO)
        {
            return await _directoryService.Create(_mapper.Map<Directory>(directoryDTO));
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
}
