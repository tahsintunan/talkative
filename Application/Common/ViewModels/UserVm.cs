using System.Text.Json.Serialization;
using Application.Common.Class;

namespace Application.Common.ViewModels;

public class UserVm : Blockable
{
    public string? Username { get; set; }
    public string? Email { get; set; }
    public DateTime? DateOfBirth { get; set; }

    [JsonIgnore] public IList<string?>? BlockedBy { get; set; }

    [JsonIgnore] public IList<string?>? Blocked { get; set; }
    [JsonIgnore] public bool IsAdmin { get; set; }

    public bool IsBanned { get; set; }
}