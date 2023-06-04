using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace API.Entities
{
    public class UserRoles : IdentityRole<int>
    {
        public ICollection<UserDataUserRole> UserRolesCol { get; set; }
    }
}