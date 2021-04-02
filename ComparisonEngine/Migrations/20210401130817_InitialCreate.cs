using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ComparisonEngine.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "comparison_engine");

            migrationBuilder.CreateTable(
                name: "eshops",
                schema: "comparison_engine",
                columns: table => new
                {
                    Id = table.Column<double>(type: "double precision", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: true),
                    Url = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_eshops", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "products",
                schema: "comparison_engine",
                columns: table => new
                {
                    Id = table.Column<double>(type: "double precision", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: true),
                    Popularity = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_products", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "real_products",
                schema: "comparison_engine",
                columns: table => new
                {
                    Id = table.Column<double>(type: "double precision", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: true),
                    Price = table.Column<decimal>(type: "numeric", nullable: false),
                    Url = table.Column<string>(type: "text", nullable: true),
                    LastCheck = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    AbstractProductId = table.Column<double>(type: "double precision", nullable: false),
                    EShopId = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_real_products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_real_products_eshops_EShopId",
                        column: x => x.EShopId,
                        principalSchema: "comparison_engine",
                        principalTable: "eshops",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_real_products_products_AbstractProductId",
                        column: x => x.AbstractProductId,
                        principalSchema: "comparison_engine",
                        principalTable: "products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_eshops_Title",
                schema: "comparison_engine",
                table: "eshops",
                column: "Title",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_products_Title",
                schema: "comparison_engine",
                table: "products",
                column: "Title",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_real_products_AbstractProductId",
                schema: "comparison_engine",
                table: "real_products",
                column: "AbstractProductId");

            migrationBuilder.CreateIndex(
                name: "IX_real_products_EShopId",
                schema: "comparison_engine",
                table: "real_products",
                column: "EShopId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "real_products",
                schema: "comparison_engine");

            migrationBuilder.DropTable(
                name: "eshops",
                schema: "comparison_engine");

            migrationBuilder.DropTable(
                name: "products",
                schema: "comparison_engine");
        }
    }
}
