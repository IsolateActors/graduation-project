using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineOrderingStore.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AdminUsers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Account = table.Column<string>(nullable: true),
                    PassWord = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdminUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OnlineUsers",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 10, nullable: false),
                    Account = table.Column<string>(maxLength: 11, nullable: false),
                    PassWord = table.Column<string>(maxLength: 15, nullable: false),
                    RegisterTime = table.Column<DateTime>(nullable: false),
                    EditTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OnlineUsers", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "StoreTypes",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TypeName = table.Column<string>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    EditTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreTypes", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "StoreUsers",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StoreName = table.Column<string>(maxLength: 10, nullable: false),
                    Account = table.Column<string>(maxLength: 11, nullable: false),
                    PassWord = table.Column<string>(maxLength: 15, nullable: false),
                    RegisterTime = table.Column<DateTime>(nullable: false),
                    StoreTypeID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreUsers", x => x.ID);
                    table.ForeignKey(
                        name: "FK_StoreUsers_StoreTypes_StoreTypeID",
                        column: x => x.StoreTypeID,
                        principalTable: "StoreTypes",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GoodsTypes",
                columns: table => new
                {
                    ID = table.Column<Guid>(nullable: false),
                    GoodsTypeName = table.Column<string>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    EditTime = table.Column<DateTime>(nullable: false),
                    StoreUserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoodsTypes", x => x.ID);
                    table.ForeignKey(
                        name: "FK_GoodsTypes_StoreUsers_StoreUserId",
                        column: x => x.StoreUserId,
                        principalTable: "StoreUsers",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Goods",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Price = table.Column<decimal>(nullable: false),
                    Stock = table.Column<int>(nullable: false),
                    ReleaseTime = table.Column<DateTime>(nullable: false),
                    EditTime = table.Column<DateTime>(nullable: false),
                    PhotoPath = table.Column<string>(nullable: true),
                    GoodsTypeId = table.Column<Guid>(nullable: false),
                    StoreUserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Goods", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Goods_GoodsTypes_GoodsTypeId",
                        column: x => x.GoodsTypeId,
                        principalTable: "GoodsTypes",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Goods_StoreUsers_StoreUserId",
                        column: x => x.StoreUserId,
                        principalTable: "StoreUsers",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ShoppingCarts",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    BuyCount = table.Column<int>(nullable: false),
                    GoodsID = table.Column<Guid>(nullable: false),
                    OnlineUserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShoppingCarts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShoppingCarts_Goods_GoodsID",
                        column: x => x.GoodsID,
                        principalTable: "Goods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ShoppingCarts_OnlineUsers_OnlineUserId",
                        column: x => x.OnlineUserId,
                        principalTable: "OnlineUsers",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    ConsigneeName = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    Phone = table.Column<long>(nullable: false),
                    Pay = table.Column<decimal>(nullable: false),
                    OnlineUserId = table.Column<int>(nullable: false),
                    ShoppingCartId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_OnlineUsers_OnlineUserId",
                        column: x => x.OnlineUserId,
                        principalTable: "OnlineUsers",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Orders_ShoppingCarts_ShoppingCartId",
                        column: x => x.ShoppingCartId,
                        principalTable: "ShoppingCarts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GoodsWithOrders",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    BuyCount = table.Column<int>(nullable: false),
                    DeliveredTime = table.Column<DateTime>(nullable: false),
                    Delivered = table.Column<bool>(nullable: false),
                    GoodsId = table.Column<Guid>(nullable: false),
                    OrderId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoodsWithOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GoodsWithOrders_Goods_GoodsId",
                        column: x => x.GoodsId,
                        principalTable: "Goods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GoodsWithOrders_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Goods_GoodsTypeId",
                table: "Goods",
                column: "GoodsTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Goods_StoreUserId",
                table: "Goods",
                column: "StoreUserId");

            migrationBuilder.CreateIndex(
                name: "IX_GoodsTypes_StoreUserId",
                table: "GoodsTypes",
                column: "StoreUserId");

            migrationBuilder.CreateIndex(
                name: "IX_GoodsWithOrders_GoodsId",
                table: "GoodsWithOrders",
                column: "GoodsId");

            migrationBuilder.CreateIndex(
                name: "IX_GoodsWithOrders_OrderId",
                table: "GoodsWithOrders",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_OnlineUserId",
                table: "Orders",
                column: "OnlineUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ShoppingCartId",
                table: "Orders",
                column: "ShoppingCartId");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingCarts_GoodsID",
                table: "ShoppingCarts",
                column: "GoodsID");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingCarts_OnlineUserId",
                table: "ShoppingCarts",
                column: "OnlineUserId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreUsers_StoreTypeID",
                table: "StoreUsers",
                column: "StoreTypeID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdminUsers");

            migrationBuilder.DropTable(
                name: "GoodsWithOrders");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "ShoppingCarts");

            migrationBuilder.DropTable(
                name: "Goods");

            migrationBuilder.DropTable(
                name: "OnlineUsers");

            migrationBuilder.DropTable(
                name: "GoodsTypes");

            migrationBuilder.DropTable(
                name: "StoreUsers");

            migrationBuilder.DropTable(
                name: "StoreTypes");
        }
    }
}
