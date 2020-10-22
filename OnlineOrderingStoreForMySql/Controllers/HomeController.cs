using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OnlineOrderingStore.Data;
using OnlineOrderingStore.Models;
using OnlineOrderingStore.security;
using OnlineOrderingStore.ViewModels;

namespace OnlineOrderingStore.Controllers
{
    [OnlineUserAuthorize]
    public class HomeController : Controller
    {
        private readonly OnlineOrderingStoreContext _context;
        private readonly UserService _userService;

        public HomeController(OnlineOrderingStoreContext context, UserService userService)
        {
            _context = context;
            _userService = userService;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index(HomeIndexViewModel model)
        {
            List<Goods> goodsList = null;
            HomeIndexViewModel homeIndexViewModel = new HomeIndexViewModel();
            if (string.IsNullOrEmpty(model.StoreTypeName))
            {
                ViewBag.StoreTypeName = new SelectList(_context.StoreTypes, "TypeName", "TypeName");
                if(!string.IsNullOrEmpty(model.SearchingString))
                {
                    goodsList = await _context.Goods.Include(x => x.StoreUser)
                        .Where(x => x.Name.Contains(model.SearchingString)).ToListAsync();
                }
                else
                {
                    goodsList = await _context.Goods.Include(x => x.StoreUser).ToListAsync();
                }
                homeIndexViewModel.Goods = goodsList;
                homeIndexViewModel.SearchingString = model.SearchingString;
            }
            else if (string.IsNullOrEmpty(model.StoreName))
            {
                ViewBag.StoreTypeName = new SelectList(_context.StoreTypes, "TypeName", "TypeName");
                ViewBag.StoreName = new SelectList(_context.StoreUsers.Where(x => x.StoreType.TypeName == model.StoreTypeName), "StoreName", "StoreName");
                if (!string.IsNullOrEmpty(model.SearchingString))
                {
                    goodsList = await _context.Goods
                        .Include(x => x.StoreUser)
                        .Where(x => x.StoreUser.StoreType.TypeName == model.StoreTypeName
                            && x.Name.Contains(model.SearchingString))
                        .ToListAsync();
                }
                else
                {
                    goodsList = await _context.Goods
                    .Include(x => x.StoreUser)
                    .Where(x => x.StoreUser.StoreType.TypeName == model.StoreTypeName)
                    .ToListAsync();
                }
                homeIndexViewModel.Goods = goodsList;
                homeIndexViewModel.StoreTypeName = model.StoreTypeName;
                homeIndexViewModel.SearchingString = model.SearchingString;
            }
            else if (string.IsNullOrEmpty(model.GoodsTypeName))
            {
                ViewBag.StoreTypeName = new SelectList(_context.StoreTypes, "TypeName", "TypeName");
                ViewBag.StoreName = new SelectList(_context.StoreUsers.Where(x => x.StoreType.TypeName == model.StoreTypeName), "StoreName", "StoreName");
                ViewBag.GoodsTypeName = new SelectList(_context.GoodsTypes.Where(x => x.StoreUser.StoreName == model.StoreName), "GoodsTypeName", "GoodsTypeName");
                if (!string.IsNullOrEmpty(model.SearchingString))
                {
                    goodsList = await _context.Goods
                        .Include(x => x.StoreUser)
                        .Where(x => x.StoreUser.StoreType.TypeName == model.StoreTypeName
                            && x.StoreUser.StoreName == model.StoreName
                            && x.Name.Contains(model.SearchingString))
                        .ToListAsync();
                }
                else
                {
                    goodsList = await _context.Goods
                    .Include(x => x.StoreUser)
                    .Where(x => x.StoreUser.StoreType.TypeName == model.StoreTypeName
                        && x.StoreUser.StoreName == model.StoreName)
                    .ToListAsync();
                }
                homeIndexViewModel.Goods = goodsList;
                homeIndexViewModel.StoreTypeName = model.StoreTypeName;
                homeIndexViewModel.StoreName = model.StoreName;
                homeIndexViewModel.SearchingString = model.SearchingString;
            }
            else
            {
                ViewBag.StoreTypeName = new SelectList(_context.StoreTypes, "TypeName", "TypeName");
                ViewBag.StoreName = new SelectList(_context.StoreUsers.Where(x => x.StoreType.TypeName == model.StoreTypeName), "StoreName", "StoreName");
                ViewBag.GoodsTypeName = new SelectList(_context.GoodsTypes.Where(x => x.StoreUser.StoreName == model.StoreName), "GoodsTypeName", "GoodsTypeName");
                if (!string.IsNullOrEmpty(model.SearchingString))
                {
                    goodsList = await _context.Goods
                        .Include(x => x.StoreUser)
                        .Where(x => x.StoreUser.StoreType.TypeName == model.StoreTypeName
                            && x.StoreUser.StoreName == model.StoreName
                            && x.GoodsType.GoodsTypeName == model.GoodsTypeName
                            && x.Name.Contains(model.SearchingString))
                        .ToListAsync();
                }
                else
                {
                    goodsList = await _context.Goods
                    .Include(x => x.StoreUser)
                    .Where(x => x.StoreUser.StoreType.TypeName == model.StoreTypeName
                        && x.StoreUser.StoreName == model.StoreName
                        && x.GoodsType.GoodsTypeName == model.GoodsTypeName)
                    .ToListAsync();
                }
                homeIndexViewModel.Goods = goodsList;
                homeIndexViewModel.StoreTypeName = model.StoreTypeName;
                homeIndexViewModel.StoreName = model.StoreName;
                homeIndexViewModel.GoodsTypeName = model.GoodsTypeName;
                homeIndexViewModel.SearchingString = model.SearchingString;

            }

            return View(homeIndexViewModel);
        }

        //添加购物车
        [AllowAnonymous]
        [HttpPost]
        public async Task<bool> AddGoodsInShoppingCart(Guid goodsId)
        {
            var onlineUser = _userService.GetOnlineUser();
            if (onlineUser == null)
            {
                return false;
            }
            var goodsForShoppingCart = await _context.ShoppingCarts
                .FirstOrDefaultAsync(g => g.GoodsID == goodsId && g.OnlineUserId == onlineUser.ID);

            if (goodsForShoppingCart == null)
            {
                var shopping = new ShoppingCart
                {
                    BuyCount = 1,
                    GoodsID = goodsId,
                    OnlineUserId = onlineUser.ID
                };
                _context.ShoppingCarts.Add(shopping);
                await _context.SaveChangesAsync();

                return true;
            }
            else
            {
                goodsForShoppingCart.BuyCount += 1;
                _context.ShoppingCarts.Update(goodsForShoppingCart);
                await _context.SaveChangesAsync();
                return true;
            }
        }

        //购物车中商品删除
        public async Task<bool> ShoppingCartForDelete(Guid id)
        {
            var shoppingCartForGoods = _context.ShoppingCarts.Find(id);
            if (shoppingCartForGoods == null)
            {
                return false;
            }
            else
            {
                _context.ShoppingCarts.Remove(shoppingCartForGoods);
                await _context.SaveChangesAsync();
                return true;
            }
            
        }

        //购物车
        public async Task<IActionResult> ShoppingCart()
        {
            var onlineUser = _userService.GetOnlineUser();
            var cart = await _context.ShoppingCarts.Include(g => g.Goods)
                .Include(s => s.Goods.StoreUser)
                .Where(ou => ou.OnlineUserId == onlineUser.ID).ToListAsync();
            if (cart == null)
            {
                return NotFound();
            }
            List<ShoppingBuyCount> shoppingBuyCountList = new List<ShoppingBuyCount>();
            foreach (var item in cart)
            {
                shoppingBuyCountList.Add(new ShoppingBuyCount
                {
                    Id = item.Id,
                    BuyCount = item.BuyCount
                });
                
            }
            
            
            ShoppingCartForPayVIewModel vm = new ShoppingCartForPayVIewModel
            {
                ShoppingCarts = cart,
                ShoppingGoodsWithBuyCounts = shoppingBuyCountList
            };
            
            return View(vm);
        }

        //从购物车中支付
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ShoppingCart(ShoppingCartForPayVIewModel model)
        {
            var user = _userService.GetOnlineUser();
            if (ModelState.IsValid)
            {
                var newOrder = new Order
                {
                    Id = new Guid(),
                    CreateTime = DateTime.Now,
                    ConsigneeName = model.ConsigneeName,
                    Address = model.Address,
                    Phone = long.Parse(model.Phone),
                    Pay = decimal.Parse(model.AllPay),
                    OnlineUserId = user.ID
                };
                _context.Orders.Add(newOrder);
                _context.SaveChanges();

                for (int i = 0; i < model.ShoppingGoodsWithBuyCounts.Count(); i++)
                {
                    var newGoodsWithOrder = new GoodsWithOrder
                    {
                        Id = new Guid(),
                        BuyCount = model.ShoppingGoodsWithBuyCounts[i].BuyCount,
                        Delivered = false,
                        GoodsId = model.ShoppingGoodsWithBuyCounts[i].Id,
                        OrderId = newOrder.Id
                    };
                    _context.GoodsWithOrders.Add(newGoodsWithOrder);
                    var removeShoppingCart = _context.ShoppingCarts.Where(x => x.OnlineUserId == user.ID);
                    _context.ShoppingCarts.RemoveRange(removeShoppingCart);
                }
                
                await _context.SaveChangesAsync();

                return RedirectToAction("Paid", "Home");
            }


            //更新提交前购买商品的件数
            for (int i = 0; i < model.ShoppingGoodsWithBuyCounts.Count(); i++)
            {
                var goodsForShoppingCart = await _context.ShoppingCarts
                .FirstOrDefaultAsync(g => g.GoodsID == model.ShoppingGoodsWithBuyCounts[i].Id && g.OnlineUserId == user.ID);
                if (goodsForShoppingCart == null)
                {
                    continue;
                }
                goodsForShoppingCart.BuyCount = model.ShoppingGoodsWithBuyCounts[i].BuyCount;
                _context.ShoppingCarts.Update(goodsForShoppingCart);
            }
            await _context.SaveChangesAsync();

            //ModelState无效返回model数据
            var cart = await _context.ShoppingCarts.Include(g => g.Goods)
                .Include(s => s.Goods.StoreUser)
                .Where(ou => ou.OnlineUserId == user.ID).ToListAsync();
            if (cart == null)
            {
                return NotFound();
            }

            model.ShoppingCarts = cart;

            return View(model);
        }

        //立即购买
        public async Task<IActionResult> ShoppingForPay(Guid goodsId)
        {
            if (goodsId == null)
            {
                return NotFound();
            }
            var goods = await _context.Goods.Include(st => st.StoreUser).FirstOrDefaultAsync(g => g.Id == goodsId);
            if (goods == null)
            {
                return NotFound();
            }
            ShoppingForPayViewModel vModel = new ShoppingForPayViewModel
            {
                Goods = goods
            };
            return View(vModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ShoppingForPay(Guid goodsId, ShoppingForPayViewModel model)
        {
            var goodsExist = await _context.Goods.AnyAsync(x => x.Id == goodsId);
            if (!goodsExist)
            {
                return BadRequest("该商品不存在");
            }
            var user = _userService.GetOnlineUser();

            if (ModelState.IsValid)
            {
                var newOrder = new Order
                {
                    Id = new Guid(),
                    CreateTime = DateTime.Now,
                    ConsigneeName = model.ConsigneeName,
                    Address = model.Address,
                    Phone = long.Parse(model.Phone),
                    Pay = decimal.Parse(model.AllPay),
                    OnlineUserId = user.ID
                };
                _context.Orders.Add(newOrder);
                _context.SaveChanges();


                var newGoodsWithOrder = new GoodsWithOrder
                {
                    Id = new Guid(),
                    BuyCount = int.Parse(model.GoodsCount),
                    Delivered = false,
                    GoodsId = goodsId,
                    OrderId = newOrder.Id
                };
                _context.GoodsWithOrders.Add(newGoodsWithOrder);
                await _context.SaveChangesAsync();


                return RedirectToAction("Paid", "Home");
            }


            var goods = await _context.Goods.Include(st => st.StoreUser).FirstOrDefaultAsync(g => g.Id == goodsId);
            if (goods == null)
            {
                return NotFound();
            }
            model.Goods = goods; 
            return View(model);
        }

        //支付后跳转
        public IActionResult Paid()
        {
            return View();
        }

        public async Task<IActionResult> OnlineUserOrderHistory()
        {
            var user = _userService.GetOnlineUser();
            var orderList = await _context.Orders
                .Where(x => x.OnlineUserId == user.ID)
                .OrderByDescending(x => x.CreateTime)
                .ToListAsync();
            return View(orderList);
        }

        //用户订单详情
        public async Task<IActionResult> OnlineUserOrderHistoryDetail(Guid id)
        {
            var goodsWithOrder = await _context.GoodsWithOrders
                .Where(x => x.OrderId == id)
                .Include(x => x.Order)
                .Include(x => x.Goods).ToListAsync();
            if (goodsWithOrder.Count == 0)
            {
                return NotFound();
            }
            return View(goodsWithOrder);
        }

        public IActionResult EditPassWord()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPassWord(EditPassWordViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (!model.NewPassWord.Trim().Equals(model.RepeatPassword.Trim()))
                {
                    return BadRequest("输入密码不一致！");
                }
                var user = _userService.GetOnlineUser();
                if (user.PassWord.ToString().Equals(model.OldPassWord.Trim()))
                {
                    user.PassWord = model.NewPassWord;
                    user.EditTime = DateTime.Now;
                    _context.OnlineUsers.Update(user);
                    await _context.SaveChangesAsync();
                    await HttpContext.SignOutAsync(OnlineUserAuthorizeAttribute.OnlineUserAuthenticationScheme);
                    return RedirectToAction("Login", "home");
                }
                else
                {
                    return BadRequest("密码错误！");
                }
            }
            
            return View();
        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (model == null)
            {
                return NotFound();
            }

            if ("onlineUser".Equals(model.Role))
            {
                var login = await _context.OnlineUsers.FirstOrDefaultAsync(o => o.Account == model.Account);
                if (login == null)
                {
                    return BadRequest("没有此用户，请注册！");
                }
                if (login.PassWord != model.PassWord)
                {
                    return BadRequest("密码错误！");
                }
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, model.Account),
                    new Claim(ClaimTypes.Role, model.Role)
                };

                var claimsIdentity = new ClaimsIdentity(claims, OnlineUserAuthorizeAttribute.OnlineUserAuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {
                    AllowRefresh = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(60),
                    IsPersistent = true,
                    IssuedUtc = DateTime.UtcNow,
                    RedirectUri = "/home/login"
                };
                await HttpContext.SignInAsync(
                OnlineUserAuthorizeAttribute.OnlineUserAuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
                return RedirectToAction("Index", "Home");
            }
            else if ("storeUser".Equals(model.Role))
            {
                var login = await _context.StoreUsers.FirstOrDefaultAsync(o => o.Account == model.Account);
                if (login == null)
                {
                    return BadRequest("没有此商家，请联系管理员注册！");
                }
                if (login.PassWord != model.PassWord)
                {
                    return BadRequest("密码错误！");
                }
                var claims = new List<Claim>
                {
                new Claim(ClaimTypes.Name, model.Account),
                new Claim(ClaimTypes.Role, model.Role)
                };
                var claimsIdentity = new ClaimsIdentity(claims, StoreUserAuthorizeAttribute.StoreUserAuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {
                    AllowRefresh = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(60),
                    IsPersistent = true,
                    IssuedUtc = DateTime.UtcNow,
                    RedirectUri = "/home/login"
                };
                await HttpContext.SignInAsync(
                StoreUserAuthorizeAttribute.StoreUserAuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
                return RedirectToAction("Index", "Store");
            }
            else if ("administration".Equals(model.Role))
            {
                var login = await _context.AdminUsers.FirstOrDefaultAsync(o => o.Account == model.Account);
                if (login == null)
                {
                    return BadRequest("没有此管理！");
                }
                if (login.PassWord != model.PassWord)
                {
                    return BadRequest("密码错误！");
                }
                var claims = new List<Claim>
                {
                new Claim(ClaimTypes.Name, model.Account),
                new Claim(ClaimTypes.Role, model.Role)
                };

                var claimsIdentity = new ClaimsIdentity(claims, AdminAuthorizeAttribute.AdminAuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {
                    AllowRefresh = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(60),
                    IsPersistent = true,
                    IssuedUtc = DateTime.UtcNow,
                    RedirectUri = "/home/login"
                };
                await HttpContext.SignInAsync(
                AdminAuthorizeAttribute.AdminAuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
                return RedirectToAction("Index", "Admin");
            }
            else
            {
                return NotFound();
            }

            //声明对象创建
            //var claims = new List<Claim>
            //{
            //    new Claim(ClaimTypes.Name, model.Account),
            //    new Claim(ClaimTypes.Role, model.Role)
            //};
            //var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            //var authProperties = new AuthenticationProperties
            //{
            //    AllowRefresh = true,
            //    // Refreshing the authentication session should be allowed.

            //    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(60),
            //    // The time at which the authentication ticket expires. A 
            //    // value set here overrides the ExpireTimeSpan option of 
            //    // CookieAuthenticationOptions set with AddCookie.

            //    IsPersistent = true,
            //    // Whether the authentication session is persisted across 
            //    // multiple requests. When used with cookies, controls
            //    // whether the cookie's lifetime is absolute (matching the
            //    // lifetime of the authentication ticket) or session-based.

            //    IssuedUtc = DateTime.UtcNow,
            //    // The time at which the authentication ticket was issued.

            //    RedirectUri = "/home/login"
            //    // The full path or absolute URI to be used as an http 
            //    // redirect response value.
            //};
            ////写入HttpContext
            //await HttpContext.SignInAsync(
            //CookieAuthenticationDefaults.AuthenticationScheme,
            //new ClaimsPrincipal(claimsIdentity),
            //authProperties);

        }

        [AdminAuthorize]
        [StoreUserAuthorize]
        public async Task<IActionResult> Logout()
        {
            var role = _userService.GetRole();
            if (role.Equals("onlineUser"))
            {
                await HttpContext.SignOutAsync(OnlineUserAuthorizeAttribute.OnlineUserAuthenticationScheme);
                return RedirectToAction("index", "home");
            }
            else if (role.Equals("storeUser"))
            {
                await HttpContext.SignOutAsync(StoreUserAuthorizeAttribute.StoreUserAuthenticationScheme);
                return RedirectToAction("login", "home");
            }
            else if (role.Equals("administration"))
            {
                await HttpContext.SignOutAsync(AdminAuthorizeAttribute.AdminAuthenticationScheme);
                return RedirectToAction("login", "home");
            }
            else
            {
                return BadRequest("退出失败！");
            }
            
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
