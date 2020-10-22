
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using OnlineOrderingStore.Data;
using OnlineOrderingStore.Models;
using System.Linq;
using System.Security.Claims;

namespace OnlineOrderingStore.security
{
    public class UserService: CookieAuthenticationEvents
    {
        private readonly OnlineOrderingStoreContext _context;
        private readonly IHttpContextAccessor _accessor;

        public UserService(OnlineOrderingStoreContext context, IHttpContextAccessor accessor)
        {
            _context = context;
            _accessor = accessor;
        }

        
        public string GetRole()
        {
            var role = (from c in _accessor.HttpContext.User.Claims
                        where c.Type == ClaimTypes.Role
                        select c.Value).FirstOrDefault();
            return role;
        }
        public string GetAccount(string role)
        {
            
            //var role = _accessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role).Value;
            string account = "";
            if (role != null)
            {
                account = (from c in _accessor.HttpContext.User.Claims
                                   where c.Type == ClaimTypes.Name
                                   select c.Value).FirstOrDefault();
            }
            return account;
        }

        public OnlineUser GetOnlineUser()
        {
            var account = GetAccount(GetRole());
            
            var user = _context.OnlineUsers.Where(x => x.Account == account).FirstOrDefault();
            return user;
        }

        public StoreUser GetStoreUser()
        {
            var account = GetAccount(GetRole());

            var user = _context.StoreUsers.Where(x => x.Account == account).FirstOrDefault();
            return user;
        }

        public AdminUser GetAdmin()
        {
            var account = GetAccount(GetRole());

            var user = _context.AdminUsers.Where(x => x.Account == account).FirstOrDefault();
            return user;
        }
    }
}
