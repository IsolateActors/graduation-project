using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineOrderingStore.Data;
using OnlineOrderingStore.Models;
using OnlineOrderingStore.security;
using OnlineOrderingStore.ViewModels;

namespace OnlineOrderingStore.Controllers
{

    [StoreUserAuthorize]
    public class StoreController : Controller
    {
        private readonly OnlineOrderingStoreContext _context;
        private readonly UserService _userService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public StoreController(OnlineOrderingStoreContext context, UserService userService, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _userService = userService;
            _webHostEnvironment = webHostEnvironment;
        }
        
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 商品分类
        /// </summary>
        
        public async Task<IActionResult> GoodsTypes()
        {
            var storeUser = _userService.GetStoreUser();
            return View(await _context.GoodsTypes.Where(x => x.StoreUserId == storeUser.ID).AsNoTracking().ToListAsync());
        }
        [HttpGet]
        public async Task<GoodsType> GoodsTypeCreateOrEdit(Guid? id)
        {
            GoodsType goodsType = null;
            if (id == null)
            {
                goodsType = new GoodsType();
            }
            else
            {
                goodsType = await _context.GoodsTypes.AsNoTracking().FirstOrDefaultAsync(m => m.ID == id);
            }

            return goodsType;
        }
        [HttpPost]
        public async Task<string> GoodsTypeCreateOrEdit(Guid? id, string goodTypeName)
        {
            var storeUser = _userService.GetStoreUser();
            if (goodTypeName.Length > 5 || goodTypeName.Length == 0)
            {
                return "不能少于1个并且超过5个字符";
            }
            bool typeExist = _context.GoodsTypes.Any(e => (e.GoodsTypeName == goodTypeName && e.ID != id));
            if (typeExist)
            {
                return "分类型已存在！";
            }

            if (id.ToString().Equals("00000000-0000-0000-0000-000000000000") || id == null)
            {
                var goodsTypeCreate = new GoodsType()
                {
                    CreateTime = DateTime.Now,
                    EditTime = DateTime.Now,
                    GoodsTypeName = goodTypeName,
                    StoreUserId = storeUser.ID
                };
                try
                {
                    _context.GoodsTypes.Add(goodsTypeCreate);
                    await _context.SaveChangesAsync();
                }
                catch (Exception)
                {

                    return "添加失败，请重试！";
                }
               
                return "添加成功！";
            }

            var goodsType = await _context.GoodsTypes.FirstOrDefaultAsync(e => e.ID == id);
            if (goodsType == null)
            {
                return "找不到ID:" + id;
            }
            if (goodsType.GoodsTypeName != goodTypeName)
            {
                goodsType.EditTime = DateTime.Now;
                goodsType.GoodsTypeName = goodTypeName;
                _context.GoodsTypes.Update(goodsType);
                await _context.SaveChangesAsync();
                return "修改成功！";
            }
            return "未修改！";
        }

        public async Task<string> GoodsTypeDelete(Guid id)
        {
            var typeHasGoods = await _context.Goods.AsNoTracking().AnyAsync(m => m.GoodsTypeId == id);
            if (typeHasGoods)
            {
                return "该分类下存在商品，请先删除商品！";
            }

            var goodsType = await _context.GoodsTypes.FindAsync(id);
            _context.GoodsTypes.Remove(goodsType);
            _context.SaveChanges();

            return "删除成功";
        }



        /// <summary>
        /// 商品
        /// </summary>
        
        public async Task<IActionResult> GoodsList(string goodsTypeName, string searchString)
        {
            var storeUser = _userService.GetStoreUser();
            var allGoods = await _context.Goods.Include(s => s.GoodsType)
                .Where(x => x.StoreUserId == storeUser.ID)
                .AsNoTracking().ToListAsync();
            
            if (!string.IsNullOrEmpty(goodsTypeName))
            {
                allGoods = allGoods.Where(x => x.GoodsType.GoodsTypeName == goodsTypeName).ToList();
            }

            if (!String.IsNullOrEmpty(searchString))
            {
                allGoods = allGoods.Where(s => s.Name.Contains(searchString)).ToList();
            }
            //VM为保留输入字符串
            var goodsSearchWIthTypeSearchViewModel = new GoodsSearchWIthTypeSearchViewModel
            {
                Goods = allGoods,
                GoodsTypes = new SelectList(_context.GoodsTypes, "GoodsTypeName", "GoodsTypeName")
            };
            return View(goodsSearchWIthTypeSearchViewModel);
        }

        public IActionResult GoodsCreate()
        {
            var storeUser = _userService.GetStoreUser();
            ViewBag.GoodsTypeList = new SelectList(_context.GoodsTypes.Where(gt => gt.StoreUserId == storeUser.ID), "ID", "GoodsTypeName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GoodsCreate(GoodsCreateViewModel model)
        {
            var storeUser = _userService.GetStoreUser();
            bool nameExist = _context.Goods.Any(e => e.Name == model.Name && e.StoreUserId == storeUser.ID);

            ViewBag.GoodsTypeList = new SelectList(_context.GoodsTypes.Where(g => g.StoreUserId == storeUser.ID), 
                "ID", "GoodsTypeName", model.GoodsTypeId);


            if (nameExist)
            {
                ViewData["Exist"] = "商品名已存在！";
                return View(model);
            }
            else
            {
                ViewData["Exist"] = "";
            }

            if (ModelState.IsValid)
            {
                string uniqueFileName = null;
                if (model.Photo != null)
                {
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");

                    uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Photo.FileName;

                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    model.Photo.CopyTo(new FileStream(filePath, FileMode.Create));
                }

                Goods newGoods = new Goods
                {
                    Name = model.Name,
                    Price = model.Price,
                    Stock = model.Stock,
                    GoodsTypeId = model.GoodsTypeId,
                    ReleaseTime = DateTime.Now,
                    EditTime = DateTime.Now,
                    StoreUserId = storeUser.ID,

                    PhotoPath = uniqueFileName
                };

                _context.Goods.Add(newGoods);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(GoodsList));
            }
            return View(model);
        }

        public async Task<IActionResult> GoodsDetails(Guid goodsId)
        {
            if (goodsId == null)
            {
                return NotFound();
            }
            var goods = await _context.Goods.Include(gt => gt.GoodsType).AsNoTracking().FirstOrDefaultAsync(g => g.Id == goodsId);
            if (goods == null)
            {
                return NotFound();
            }
            return View(goods);
        }

        public async Task<IActionResult> GoodsEdit(Guid goodsId)
        {
            if (goodsId == null)
            {
                return NotFound();
            }
            var goods = await _context.Goods.AsNoTracking().FirstOrDefaultAsync(g => g.Id == goodsId);
            if (goods == null)
            {
                return NotFound();
            }
            var storeUser = _userService.GetStoreUser();
            ViewBag.GoodsTypeList = new SelectList(_context.GoodsTypes.Where(x => x.StoreUserId == storeUser.ID),
                "ID", "GoodsTypeName", 
                goods.GoodsTypeId);

            GoodsEditViewModel goodsEditViewModel = new GoodsEditViewModel
            {
                Goods = goods
            };
            return View(goodsEditViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GoodsEdit(Guid goodsId, GoodsEditViewModel model)
        {
            bool exists = _context.Goods.Any(e => e.Id == goodsId);
            if (exists && model.Goods.Id != goodsId)
            {
                return NotFound();
            }

            var storeUser = _userService.GetStoreUser();
            ViewBag.GoodsTypeList = new SelectList(_context.GoodsTypes.Where(x => x.StoreUserId == storeUser.ID), 
                "ID", "GoodsTypeName", 
                model.Goods.GoodsTypeId);

            bool nameExist = _context.Goods.Any(e => e.Name == model.Goods.Name && e.Id != model.Goods.Id && e.StoreUserId == model.Goods.StoreUserId);
            if (nameExist)
            {
                ViewData["Exist"] = "商品名已存在！";
                return View(model);
            }
            else
            {
                ViewData["Exist"] = "";
            }
            if (ModelState.IsValid)
            {
                string uniqueFileName = null;
                if (model.Photo != null)
                {
                    if (model.Goods.PhotoPath != null)
                    {
                        string filePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", model.Goods.PhotoPath);
                        System.IO.File.Delete(filePath);
                    }

                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");

                    uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Photo.FileName;

                    string newfilePath = Path.Combine(uploadsFolder, uniqueFileName);

                    model.Photo.CopyTo(new FileStream(newfilePath, FileMode.Create));
                    
                }
                model.Goods.EditTime = DateTime.Now;
                model.Goods.PhotoPath = uniqueFileName;
                _context.Goods.Update(model.Goods);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(GoodsList));
            }

            return View(model.Goods);
        }

        public async Task<IActionResult> GoodsDelete(Guid? goodsId)
        {
            var goods = await _context.Goods.Include(gt => gt.GoodsType).FirstOrDefaultAsync(m => m.Id == goodsId);
            if (goods == null)
            {
                return NotFound();
            }
            return View(goods);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GoodsDelete(Guid id)
        {
            var goods = await _context.Goods.FindAsync(id);
            if (goods == null)
            {
                return NotFound();
            }
            _context.Goods.Remove(goods);
            _context.SaveChanges();
            
            return RedirectToAction(nameof(GoodsList));
        }

        public async Task<bool> GoodsStockMakeZero(Guid id)
        {
            var goods = await _context.Goods.FindAsync(id);
            if (goods == null)
            {
                return false;
            }

            goods.Stock = 0;
            
            _context.Goods.Update(goods);
            _context.SaveChanges();
            return true;
        }


        /// <summary>
        /// 订单
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> StoreOrderIndex()
        {
            var storeUser = _userService.GetStoreUser();

            var delivered = await _context.GoodsWithOrders
                .Include(x => x.Order)
                .Include(x => x.Goods)
                .Where(i => i.Delivered == true && i.Goods.StoreUserId == storeUser.ID)
                .OrderByDescending(x => x.DeliveredTime)
                .ToListAsync();

            var notDelivered = await _context.GoodsWithOrders
                .Include(x => x.Order)
                .Include(x => x.Goods)
                .Where(i => i.Delivered == false && i.Goods.StoreUserId == storeUser.ID)
                .OrderByDescending(x => x.Order.CreateTime)
                .ToListAsync();

            var deliveredOrder = delivered.GroupBy(x => x.Order).Select(x => x.Key).ToList();
            var notDeliveredOrder = notDelivered.GroupBy(x => x.Order).Select(x => x.Key).ToList();

            OrderListForStoreUserViewModel vm = new OrderListForStoreUserViewModel
            {
                DeliveredOrder = deliveredOrder,
                NotDeliveredOrder = notDeliveredOrder
            };

            return View(vm);
        }

        
        public async Task<IActionResult> GoodsWithOrderDetails(Guid id)
        {
            var storeUser = _userService.GetStoreUser();

            var deliveredGoods = await _context.GoodsWithOrders
                .Include(x => x.Order)
                .Include(x => x.Goods)
                .Where(i => i.Delivered == true && i.Goods.StoreUserId == storeUser.ID && i.OrderId == id)
                .ToListAsync();


            var notDeliveredGoods = await _context.GoodsWithOrders
                .Include(x => x.Order)
                .Include(x => x.Goods)
                .Where(i => i.Delivered == false && i.Goods.StoreUserId == storeUser.ID && i.OrderId == id)
                .ToListAsync();

            StoreUserGoodsWithOrderDetailsViewModel vm = new StoreUserGoodsWithOrderDetailsViewModel();
            if (deliveredGoods.Count == 0 && notDeliveredGoods.Count != 0)
            {
                vm.NotDeliveredGoods = notDeliveredGoods;
            }
            if (deliveredGoods.Count != 0 && notDeliveredGoods.Count == 0)
            {
                vm.DeliveredGoods = deliveredGoods;
            }
            

            return View(vm);
        }

        public async Task<IActionResult> GoodsWithOrderForDelivered(Guid id)
        {
            var storeUser = _userService.GetStoreUser();

            var notDeliveredGoods = _context.GoodsWithOrders
                .Include(x => x.Order)
                .Include(x => x.Goods)
                .Where(i => i.Delivered == false && i.Goods.StoreUserId == storeUser.ID && i.OrderId == id);

            foreach (var item in notDeliveredGoods)
            {
                item.Delivered = true;
                item.DeliveredTime = DateTime.Now;
                _context.GoodsWithOrders.Update(item);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("StoreOrderIndex", "Store");
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
                var user = _userService.GetStoreUser();
                if (user.PassWord.ToString().Equals(model.OldPassWord.Trim()))
                {
                    user.PassWord = model.NewPassWord;
                    _context.StoreUsers.Update(user);
                    await _context.SaveChangesAsync();
                    await HttpContext.SignOutAsync(StoreUserAuthorizeAttribute.StoreUserAuthenticationScheme);
                    return RedirectToAction("Login", "home");
                }
                else
                {
                    return BadRequest("密码错误！");
                }
            }

            return View();
        }
    }
}