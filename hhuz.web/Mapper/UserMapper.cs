using hhuz.Dto;
using hhuz.Models;

namespace hhuz.Mapper;

public class UserMapper : BaseMapper<Users,LoginDto>
{
    public Users ToEntity(LoginDto dto)
    {
        return new Users()
            {
                username = dto.username,
                Password = dto.password,
                Email = dto.email
            };
    }

    public LoginDto ToDto(Users entity)
    {
        return new LoginDto()
        {
            username = entity.username,
            email = entity.Email,
            password = entity.Password
        };
    }

    public List<Users> ToEntities(List<LoginDto> dtos)
    {
        return dtos.Select(dto => new Users
        {
            username = dto.username,
            Password = dto.password,
            Email = dto.email,
        }).ToList();
    }

    public List<LoginDto> ToDtos(List<Users> dtos)
    {
        return dtos.Select(d => new LoginDto()
        {
            username = d.username,
            password = d.Password,
            email = d.Email
        }).ToList();
    }
}