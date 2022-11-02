using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using findox.Domain.Models.Dto;

namespace findox.Test.TestObjects
{
    public class ObjectsDtoTest
    {
        public UserDto TestUserAdminDto
        {
            get
            {
                return new UserDto()
                {
                    Id = 1,
                    Name = "admin",
                    Password = "passwordadmin",
                    Email = "admin@storage.com",
                    Role = "admin",
                    CreatedDate = DateTime.Now.AddDays(-7)
                };
            }
        }

        public UserDto TestUserManagerDto
        {
            get
            {
                return new UserDto()
                {
                    Id = 2,
                    Name = "manager",
                    Password = "passwordmanager",
                    Email = "manager@storage.com",
                    Role = "manager",
                    CreatedDate = DateTime.Now.AddDays(-7)
                };
            }
        }

        public UserDto TestUserRegularDto
        {
            get
            {
                return new UserDto()
                {
                    Id = 3,
                    Name = "regular",
                    Password = "passwordregular",
                    Email = "regular@storage.com",
                    Role = "regular",
                    CreatedDate = DateTime.Now.AddDays(-7)
                };
            }
        }

        public UserDto TestUserIncompleteDto
        {
            get
            {
                return new UserDto()
                {
                    Name = "incomplete",
                };
            }
        }

        public UserDto TestUserEmptyStringsDto
        {
            get
            {
                return new UserDto()
                {
                    Name = "",
                    Password = "",
                    Email = "",
                    Role = ""
                };
            }
        }

        public UserDto TestUserNewRegularDto
        {
            get
            {
                return new UserDto()
                {
                    Name = "newregular",
                    Password = "passwordnewregular",
                    Email = "newbasic@storage.com",
                    Role = "regular"
                };
            }
        }

        public List<UserDto> TestUserDtoList
        {
            get
            {
                return new List<UserDto>()
                {
                    TestUserRegularDto,
                    TestUserManagerDto,
                    TestUserAdminDto
                };
            }
        }

        public UserSessionDto TestUserSessionAdminDto
        {
            get
            {
                return new UserSessionDto("admin@storagedocument.com", "passwordadmin");
            }
        }

        public UserSessionDto TestUserSessionManagerDto
        {
            get
            {
                return new UserSessionDto("manager@storagedocument.com", "passwordmanager");
            }
        }

        public UserSessionDto TestUserSessionBasicDto
        {
            get
            {
                return new UserSessionDto("regular@storagedocument.com", "passwordregular");
            }
        }

        public UserGroupDto TestUserGroupDto
        {
            get
            {
                return new UserGroupDto()
                {
                    Id = 1,
                    GroupId = 1,
                    UserId = 1
                };
            }
        }

        public IPermissionDto TestPermitDto
        {
            get
            {
                return new PermissionDto()
                {
                    DocumentId = 1,
                    UserId = 1,
                    GroupId = 1
                };
            }
        }

        public GroupDto TestGroupNewDto
        {
            get
            {
                return new GroupDto()
                {
                    Name = "New Group",
                    Description = "A new group for users to join.",
                };
            }
        }
    }
}