using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using findox.Domain.Models.Database;

namespace findox.Test.TestObjects
{
    public class DomainTestObjects
    {
        public User TestRegularUser
        {
            get
            {
                return new User()
                {
                    Id = 1,
                    Name = "regular",
                    Password = "passwordregular",
                    Email = "regular@storage.com",
                    Role = "regular",
                    CreatedDate = DateTime.Now.AddDays(-7)      
                };
            }
        }

        public User TestManagerUser
        {
            get
            {
                return new User()
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

        public User TestAdminUser
        {
            get
            {
                return new User()
                {
                    Id = 3,
                    Name = "admin",
                    Password = "passwordadmin",
                    Email = "admin@storage.com",
                    Role = "admin",
                    CreatedDate = DateTime.Now.AddDays(-7)
                };
            }
        }
    }
}