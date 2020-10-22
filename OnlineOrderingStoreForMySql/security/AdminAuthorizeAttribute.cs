using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineOrderingStore.security
{
    public class AdminAuthorizeAttribute: AuthorizeAttribute
    {
        public const string AdminAuthenticationScheme = "AdminAuthenticationScheme";
        public AdminAuthorizeAttribute()
        {
            AuthenticationSchemes = AdminAuthenticationScheme;
        }
    }
}
