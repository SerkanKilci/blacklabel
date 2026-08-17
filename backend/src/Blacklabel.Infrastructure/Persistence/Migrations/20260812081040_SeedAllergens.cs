using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Blacklabel.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class SeedAllergens : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Allergens",
                columns: new[] { "Code", "NameEn", "NameTr" },
                values: new object[,]
                {
                    { "celery", "Celery", "Kereviz" },
                    { "crustaceans", "Crustaceans", "Kabuklu Deniz Ürünleri" },
                    { "eggs", "Eggs", "Yumurta" },
                    { "fish", "Fish", "Balık" },
                    { "gluten", "Gluten (Wheat, Rye, Barley, Oats)", "Glüten (Buğday, Çavdar, Arpa, Yulaf)" },
                    { "lupin", "Lupin", "Acı Bakla (Lupin)" },
                    { "milk", "Milk (Including Lactose)", "Süt (Laktoz Dahil)" },
                    { "molluscs", "Molluscs", "Yumuşakçalar" },
                    { "mustard", "Mustard", "Hardal" },
                    { "nuts", "Tree Nuts", "Sert Kabuklu Yemişler (Fındık, Badem, Ceviz vb.)" },
                    { "peanuts", "Peanuts", "Yer Fıstığı" },
                    { "sesame-seeds", "Sesame Seeds", "Susam" },
                    { "soybeans", "Soybeans", "Soya" },
                    { "sulphur-dioxide-and-sulphites", "Sulphur Dioxide and Sulphites", "Kükürt Dioksit ve Sülfitler" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Allergens",
                keyColumn: "Code",
                keyValue: "celery");

            migrationBuilder.DeleteData(
                table: "Allergens",
                keyColumn: "Code",
                keyValue: "crustaceans");

            migrationBuilder.DeleteData(
                table: "Allergens",
                keyColumn: "Code",
                keyValue: "eggs");

            migrationBuilder.DeleteData(
                table: "Allergens",
                keyColumn: "Code",
                keyValue: "fish");

            migrationBuilder.DeleteData(
                table: "Allergens",
                keyColumn: "Code",
                keyValue: "gluten");

            migrationBuilder.DeleteData(
                table: "Allergens",
                keyColumn: "Code",
                keyValue: "lupin");

            migrationBuilder.DeleteData(
                table: "Allergens",
                keyColumn: "Code",
                keyValue: "milk");

            migrationBuilder.DeleteData(
                table: "Allergens",
                keyColumn: "Code",
                keyValue: "molluscs");

            migrationBuilder.DeleteData(
                table: "Allergens",
                keyColumn: "Code",
                keyValue: "mustard");

            migrationBuilder.DeleteData(
                table: "Allergens",
                keyColumn: "Code",
                keyValue: "nuts");

            migrationBuilder.DeleteData(
                table: "Allergens",
                keyColumn: "Code",
                keyValue: "peanuts");

            migrationBuilder.DeleteData(
                table: "Allergens",
                keyColumn: "Code",
                keyValue: "sesame-seeds");

            migrationBuilder.DeleteData(
                table: "Allergens",
                keyColumn: "Code",
                keyValue: "soybeans");

            migrationBuilder.DeleteData(
                table: "Allergens",
                keyColumn: "Code",
                keyValue: "sulphur-dioxide-and-sulphites");
        }
    }
}
