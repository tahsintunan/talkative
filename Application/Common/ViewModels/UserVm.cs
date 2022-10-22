using Application.Common.Class;

namespace Application.Common.ViewModels
{
    public class UserVm: Blockable
    {
        public string? Username { get; set; }
        public string? Email { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public IList<string?>? BlockedBy { get; set; }
        public IList<string?>? Blocked { get; set; }
        
    }
}
