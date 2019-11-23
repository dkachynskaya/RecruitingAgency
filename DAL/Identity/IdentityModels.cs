using Microsoft.AspNet.Identity.EntityFramework;
using DAL.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Identity
{
    public class CustomUserRole: IdentityUserRole<int> { }
    public class CustomUserClaim: IdentityUserClaim<int> { }
    public class CustomUserLogin: IdentityUserLogin<int> { }

    public class CustomUserStore: UserStore<ApplicationUser, CustomRole, int, CustomUserLogin, 
        CustomUserRole, CustomUserClaim>
    {
        public CustomUserStore(ApplicationDbContext context)
            : base(context)
        { }
    }

    public class CustomRoleStore: RoleStore<CustomRole, int, CustomUserRole>
    {
        public CustomRoleStore(ApplicationDbContext context)
            : base(context)
        { }
    }
}
