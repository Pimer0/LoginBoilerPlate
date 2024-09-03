using AutoMapper;

namespace LoginBoilerPlate.Mapper;

public class UserMappingConfig : Profile
{
    public UserMappingConfig()
    {
        CreateMap<User, UserOutput>();
        CreateMap<UserInput, User>();
        CreateMap<UserOutput, User>();
    }
}