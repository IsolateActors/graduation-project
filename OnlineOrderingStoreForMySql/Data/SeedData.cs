using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OnlineOrderingStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineOrderingStore.Data
{
    public static class SeedData
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OnlineUser>().HasData(
                new OnlineUser
                {
                    ID = 1,
                    Name = "张三",
                    Account = "123",
                    PassWord = "123456",
                    RegisterTime = DateTime.Now
                },
                new OnlineUser
                {
                    ID = 2,
                    Name = "李四",
                    Account = "456",
                    PassWord = "123456",
                    RegisterTime = DateTime.Now
                },
                new OnlineUser
                {
                    ID = 3,
                    Name = "王五",
                    Account = "789",
                    PassWord = "123456",
                    RegisterTime = DateTime.Now
                }
                );

            modelBuilder.Entity<StoreType>().HasData(
                new StoreType
                {
                    ID = 1,
                    TypeName = "餐厅",
                    CreateTime = DateTime.Now,
                    EditTime = DateTime.Now
                },
                new StoreType
                {
                    ID = 2,
                    TypeName = "奶茶",
                    CreateTime = DateTime.Now,
                    EditTime = DateTime.Now
                }
                );

            modelBuilder.Entity<StoreUser>().HasData(
                new StoreUser
                {
                    ID = 1,
                    StoreName = "惠氏",
                    Account = "147",
                    PassWord = "123456",
                    RegisterTime = DateTime.Now,
                    StoreTypeID = 1
                },
                new StoreUser
                {
                    ID = 2,
                    StoreName = "农家小厨",
                    Account = "258",
                    PassWord = "123456",
                    RegisterTime = DateTime.Now,
                    StoreTypeID = 1
                },
                new StoreUser
                {
                    ID = 3,
                    StoreName = "益和堂",
                    Account = "369",
                    PassWord = "123456",
                    RegisterTime = DateTime.Now,
                    StoreTypeID = 2
                }
                );
        }
    }
}
