using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blacklabel.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddHouseholdProfiles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserPreferences");

            migrationBuilder.CreateTable(
                name: "HouseholdProfiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    AvoidedAdditiveCodes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AllergenCodes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DietFlags = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HouseholdProfiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HouseholdProfiles_AppUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HouseholdProfiles_UserId",
                table: "HouseholdProfiles",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HouseholdProfiles");

            migrationBuilder.CreateTable(
                name: "UserPreferences",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AllergenCodes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AvoidedAdditiveCodes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DietFlags = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPreferences", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_UserPreferences_AppUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }
    }
}
