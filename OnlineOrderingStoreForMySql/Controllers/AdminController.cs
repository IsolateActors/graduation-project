using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineOrderingStore.Data;
using OnlineOrderingStore.Models;
using OnlineOrderingStore.security;
using OnlineOrderingStore.ViewModels;

namespace OnlineOrderingStore.Controllers
{

    [AdminAuthorize]
    public class AdminController : Controller
    {
        private readonly OnlineOrderingStoreContext _context;

        public AdminController(OnlineOrderingStoreContext context)
        {
            _context = context;
        }

        public ViewResult Index()
        {
            return View();
        }

        /// <summary>
        /// 商家管理
        /// </summary>
        /// <returns></returns>

        public async Task<IActionResult> StoreUsersIndex(string storeTypeName, string searchString)
        {
            var allStores = _context.StoreUsers.Include(s => s.StoreType).AsNoTracking();
            List<StoreUser> storeList = null;

            if (!string.IsNullOrEmpty(storeTypeName))
            {
                allStores = allStores.Where(t => t.StoreType.TypeName == storeTypeName);
            }
            
            if (!String.IsNullOrEmpty(searchString))
            {
                storeList = await allStores.Where(s => s.StoreName.Contains(searchString)).ToListAsync();
                if (storeList.Count == 0)
                {
                    storeList = await allStores.Where(s => s.Account.Contains(searchString)).ToListAsync();
                }
            }
            else
            {
                storeList = await allStores.ToListAsync();
            }
            
            //VM为保留输入字符串
            var storeUserStoreTypeVM = new StoreUserStoreTypeViewModel
            {
                StoreUsers = storeList,
                StoreTypes = new SelectList(_context.StoreTypes, "TypeName", "TypeName")
            };

            return View(storeUserStoreTypeVM);
            
        }


        [HttpGet]
        public IActionResult StoreUserCreate()
        {
            ViewBag.TypeList = new SelectList(_context.StoreTypes, "ID", "TypeName");

            //var NameList = (from StoreType
            //               in _context.StoreTypes
            //               select StoreType).ToList();
            //ViewBag.Type  = NameList;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> StoreUserCreate([Bind("StoreName,Account,PassWord,StoreTypeID")] StoreUser storeUser)
        {
            bool nameExist = _context.StoreUsers.Any(e => e.StoreName == storeUser.StoreName);
            bool accountExist = _context.StoreUsers.Any(e => e.Account == storeUser.Account);

            ViewBag.TypeList = new SelectList(_context.StoreTypes, "ID", "TypeName", storeUser.StoreTypeID);


            if (nameExist)
            {
                ViewData["Exist"] = "店铺名已存在！";
                return View(storeUser);
            }
            else if (accountExist)
            {
                ViewData["Exist"] = "账户已存在！";
                return View(storeUser);
            }
            else
            {
                ViewData["Exist"] = "";
            }


            if (ModelState.IsValid)
            {
                storeUser.RegisterTime = DateTime.Now;
                _context.StoreUsers.Add(storeUser);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(StoreUsersIndex));
            }

            
            return View(storeUser);
        }

        public async Task<IActionResult> StoreUserDetial(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var storeUser = await _context.StoreUsers
                .Include(s => s.StoreType)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);

            if (storeUser == null)
            {
                return NotFound();
            }

            return View(storeUser);
        }

        [HttpGet]
        public async Task<IActionResult> StoreUserEdit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var storeUser = await _context.StoreUsers.AsNoTracking().FirstOrDefaultAsync(m => m.ID == id);
            if (storeUser == null)
            {
                return NotFound();
            }
            ViewBag.TypeList = new SelectList(_context.StoreTypes, "ID", "TypeName", storeUser.StoreTypeID);
            //return View("StoreUserCreate",storeUser);
            return View(storeUser);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> StoreUserEdit(int id, [Bind("ID,RegisterTime,StoreName,Account,PassWord,StoreTypeID")] StoreUser storeUser)
        {
            if (id != storeUser.ID)
            {
                return NotFound();
            }

            ViewBag.TypeList = new SelectList(_context.StoreTypes, "ID", "TypeName", storeUser.StoreTypeID);

            bool nameExist = _context.StoreUsers.Any(e => (e.StoreName == storeUser.StoreName && e.ID != storeUser.ID));
            bool accountExist = _context.StoreUsers.Any(e => (e.Account == storeUser.Account && e.ID != storeUser.ID));

            if (nameExist)
            {
                ViewData["Exist"] = "店铺名已存在！";
                return View(storeUser);
            }
            else if (accountExist)
            {
                ViewData["Exist"] = "账户已存在！";
                return View(storeUser);
            }
            else
            {
                ViewData["Exist"] = "";
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.StoreUsers.Update(storeUser);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    bool exists = _context.StoreUsers.Any(e => e.ID == storeUser.ID);
                    if (!exists)
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction(nameof(StoreUsersIndex));

            }

            return View(storeUser);
        }

        [HttpGet]
        public async Task<IActionResult> StoreUserDelete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var storeUser = await _context.StoreUsers
                .Include(s => s.StoreType)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);

            if (storeUser == null)
            {
                return NotFound();
            }

            return View(storeUser);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> StoreUserDelete(int id)
        {
            var storeUser = await _context.StoreUsers.FindAsync(id);
            _context.StoreUsers.Remove(storeUser);
            _context.SaveChanges();

            return RedirectToAction(nameof(StoreUsersIndex));
        }



        /// <summary>
        /// 用户管理
        /// </summary>
        /// <returns></returns>

        public async Task<IActionResult> OnlineUsersIndex(string searchString)
        {
            var allUsers = _context.OnlineUsers.AsNoTracking();
            List<OnlineUser> userList = null;

            if (!String.IsNullOrEmpty(searchString))
            {
                userList = await allUsers.Where(s => s.Name.Contains(searchString)).ToListAsync();
                if (userList.Count == 0)
                {
                    userList = await allUsers.Where(s => s.Account.Contains(searchString)).ToListAsync();
                }
            }
            else
            {
                userList = await allUsers.ToListAsync();
            }

            var onlineUserSearchStringVM = new OnlineUserSearchStringViewModel
            {
                OnlineUsers = userList
            };

            return View(onlineUserSearchStringVM);
        }

        [HttpGet]
        [AllowAnonymous]
        public ViewResult OnlineUserCreate()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> OnlineUserCreate([Bind("Name,Account,PassWord")] OnlineUser onlineUser)
        {
            bool nameExist = _context.OnlineUsers.Any(e => e.Name == onlineUser.Name);
            bool accountExist = _context.OnlineUsers.Any(e => e.Account == onlineUser.Account);

            if (nameExist)
            {
                ViewData["Exist"] = "用户名已存在！";
                return View(onlineUser);
            }
            else if (accountExist)
            {
                ViewData["Exist"] = "账户已存在！";
                return View(onlineUser);
            }
            else
            {
                ViewData["Exist"] = "";
            }
            if (ModelState.IsValid)
            {
                onlineUser.RegisterTime = DateTime.Now;
                onlineUser.EditTime = DateTime.Now;
                _context.OnlineUsers.Add(onlineUser);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(OnlineUsersIndex));
            }
            return View(onlineUser);
        }

        [HttpGet]
        public async Task<IActionResult> OnlineUserEdit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var onlineUser = await _context.OnlineUsers.AsNoTracking().FirstOrDefaultAsync(m => m.ID == id);
            if (onlineUser == null)
            {
                return NotFound();
            }
            return View(onlineUser);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnlineUserEdit(int id, [Bind("ID,RegisterTime,Name,Account,PassWord")] OnlineUser onlineUser)
        {
            if (id != onlineUser.ID)
            {
                return NotFound();
            }
            bool nameExist = _context.OnlineUsers.Any(e => (e.Name == onlineUser.Name && e.ID != onlineUser.ID));
            bool accountExist = _context.OnlineUsers.Any(e => (e.Account == onlineUser.Account && e.ID != onlineUser.ID));

            if (nameExist)
            {
                ViewData["Exist"] = "用户名已存在！";
                return View(onlineUser);
            }
            else if (accountExist)
            {
                ViewData["Exist"] = "账户已存在！";
                return View(onlineUser);
            }
            else
            {
                ViewData["Exist"] = "";
            }

            if (ModelState.IsValid)
            {
                try
                {
                    onlineUser.EditTime = DateTime.Now;
                    _context.OnlineUsers.Update(onlineUser);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    bool exists = _context.StoreUsers.Any(e => e.ID == onlineUser.ID);
                    if (!exists)
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction(nameof(OnlineUsersIndex));

            }

            return View(onlineUser);
        }

        public async Task<OnlineUser> OnlineUserDetial(int id)
        {
            var onlineUser = await _context.OnlineUsers
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);

            return onlineUser;
        }
        public async Task<string> OnlineUserDelete(int id)
        {
            var onlineUser = await _context.OnlineUsers.FindAsync(id);
            try
            {
                _context.OnlineUsers.Remove(onlineUser);
                _context.SaveChanges();
            }
            catch (Exception)
            {
                return "删除失败！";
            }
            
            return "删除成功！";
        }


        /// <summary>
        /// 商店类型管理
        /// </summary>
        /// <returns></returns>

        public async Task<IActionResult> StoreTypesIndex()
        {
            return View(await _context.StoreTypes.AsNoTracking().ToListAsync());
        }

        [HttpGet]
        public async Task<StoreType> StoreTypeCreateOrEdit(int? id)
        {
            StoreType storeType = null;
            if (id == null || id == 0)
            {
                 storeType = new StoreType();
            }
            else
            {
                storeType = await _context.StoreTypes.AsNoTracking().FirstOrDefaultAsync(m => m.ID == id);
            }
            
            return storeType;
        }
        [HttpPost]
        public async Task<string> StoreTypeCreateOrEdit(int? id, string typeName)
        {
            if (typeName.Length > 5 || typeName.Length == 0)
            {
                return "不能少于1个并且超过5个字符";
            }
            bool typeExist = _context.StoreTypes.Any(e => (e.TypeName == typeName && e.ID != id));
            if (typeExist)
            {
                return "店铺类型已存在！";
            }

            if (id == null || id == 0)
            {
                var storeTypeCreate = new StoreType()
                {
                    CreateTime = DateTime.Now,
                    EditTime = DateTime.Now,
                    TypeName = typeName
                };

                _context.StoreTypes.Add(storeTypeCreate);
                await _context.SaveChangesAsync();
                return "添加成功！";
            }

            var storeType = await _context.StoreTypes.FirstOrDefaultAsync(e => e.ID == id);
            if (storeType ==null)
            {
                return "找不到ID:" + id;
            }
            if (storeType.TypeName != typeName)
            {
                storeType.EditTime = DateTime.Now;
                storeType.TypeName = typeName;
                _context.StoreTypes.Update(storeType);
                await _context.SaveChangesAsync();
                return "修改成功！";
            }
            return "未修改！";
        }
       
        public async Task<string> StoreTypeDelete(int id)
        {
            var typeHasUser = await _context.StoreUsers.AsNoTracking().AnyAsync(m => m.StoreTypeID == id);
            if (typeHasUser)
            {
                return "该类型下存在商店，请先删除商店！";
            }
            var storeType = await _context.StoreTypes.FindAsync(id);
            _context.StoreTypes.Remove(storeType);
            _context.SaveChanges();

            return "删除成功";
        }


    }
}