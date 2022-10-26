using AutoMapper;
using findox.Domain.Models.Database;
using findox.Domain.Models.Dto;

namespace findox.Domain.Maps
{
   public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Group, GroupDto>();
            CreateMap<Group, GroupAllDto>()
                .ForMember(dest => dest.UserGroups, opt => opt.Condition(source => source.UserGroups.Count() > 0))
                .ForMember(dest => dest.Permissions, opt => opt.Condition(source => source.Permissions.Count() > 0));
            CreateMap<GroupDto, Group>();

            CreateMap<Document, DocumentDto>();
            CreateMap<DocumentDto, Document>();

            CreateMap<UserGroup, UserGroupDto>();
            CreateMap<UserGroupDto, UserGroup>();

            CreateMap<Permission, PermissionDto>();
            CreateMap<PermissionDto, Permission>();

            CreateMap<User, UserDto>()
                .ForMember(dest => dest.Password, opt => opt.Ignore()); 
            CreateMap<User, UserAllDto>()
                .ForMember(dest => dest.Password, opt => opt.Ignore())
                .ForMember(dest => dest.Documents, opt => opt.Condition(source => source.Documents.Count() > 0))
                .ForMember(dest => dest.UserGroups, opt => opt.Condition(source => source.UserGroups.Count() > 0))
                .ForMember(dest => dest.Permissions, opt => opt.Condition(source => source.Permissions.Count() > 0));
            CreateMap<UserDto, User>();
            CreateMap<UserSessionDto, User>();
        }
    }
}