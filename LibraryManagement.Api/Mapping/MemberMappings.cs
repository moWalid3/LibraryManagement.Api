using LibraryManagement.Api.DTOs.Members;
using LibraryManagement.Api.Entities;

namespace LibraryManagement.Api.Mapping;

public static class MemberMappings
{
    public static ReadMemberDto ToReadDto(this Member member)
    {
        return new ReadMemberDto()
        {
            Id = member.Id,
            FirstName = member.FirstName,
            LastName = member.LastName,
            IsActive = member.IsActive
        };
    }
}
