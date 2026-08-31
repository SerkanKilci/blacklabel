using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blacklabel.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddProductUsdaEnrichedAt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "UsdaEnrichedAt",
                table: "Products",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UsdaEnrichedAt",
                table: "Products");
        }
    }
}
