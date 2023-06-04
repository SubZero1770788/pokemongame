using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace API.Entities
{
    public class UserDataUserRole : IdentityUserRole<int>
    {
        public UserData User { get; set; }
        public UserRoles Role { get; set; }
    }
}