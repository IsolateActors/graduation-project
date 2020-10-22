using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineOrderingStore.security
{
    public class StoreUserAuthorizeAttribute: AuthorizeAttribute
    {
        public const string StoreUserAuthenticationScheme = "StoreUserAuthenticationScheme";
        public StoreUserAuthorizeAttribute()
        {
            AuthenticationSchemes = StoreUserAuthenticationScheme;
        }
    }
}
