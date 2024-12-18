using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialOrderImporterDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OnlineOrder",
                columns: table => new
                {
                    OrderId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Uuid = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Id = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    Region = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Country = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    ItemType = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    SalesChannel = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Priority = table.Column<string>(type: "varchar(1)", unicode: false, maxLength: 1, nullable: false),
                    Date = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
                    ShipDate = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
                    UnitsSold = table.Column<double>(type: "float", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    UnitCost = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    TotalRevenue = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    TotalCost = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    TotalProfit = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OnlineOrder", x => x.OrderId);
                });

            migrationBuilder.CreateTable(
                name: "Link",
                columns: table => new
                {
                    OrderId = table.Column<long>(type: "bigint", nullable: false),
                    Self = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Link", x => x.OrderId);
                    table.ForeignKey(
                        name: "FK_Link_OnlineOrder_OrderId",
                        column: x => x.OrderId,
                        principalTable: "OnlineOrder",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Link_OrderId",
                table: "Link",
                column: "OrderId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OnlineOrder_OrderId",
                table: "OnlineOrder",
                column: "OrderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Link");

            migrationBuilder.DropTable(
                name: "OnlineOrder");
        }
    }
}
