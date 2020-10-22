using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineOrderingStore.security
{
    public class OnlineUserAuthorizeAttribute: AuthorizeAttribute
    {
        public const string OnlineUserAuthenticationScheme = "OnlineUserAuthenticationScheme";
        public OnlineUserAuthorizeAttribute()
        {
            AuthenticationSchemes = OnlineUserAuthenticationScheme;
        }
    }
}
