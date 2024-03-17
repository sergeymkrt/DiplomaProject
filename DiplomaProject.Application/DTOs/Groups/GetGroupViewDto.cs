using DiplomaProject.Application.DTOs.Authentication;

namespace DiplomaProject.Application.DTOs.Groups;

public class GetGroupViewDto
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int AccessLevelId { get; set; }
    public List<ShortUserDto> Users { get; set; }
}
