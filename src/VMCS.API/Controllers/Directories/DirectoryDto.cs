namespace VMCS.API.Controllers.Directories
{
    public class DirectoryDto
    {
        public string Name { get; set; }
        public string DirectoryInJson { get; set; }
        public string MeetingId { get; set; }
        public byte[] DirectoryZip { get; set; }
    }
}