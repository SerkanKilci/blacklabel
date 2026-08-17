using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blacklabel.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddAdditiveSynonyms : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Synonyms",
                table: "Additives",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E100",
                column: "Synonyms",
                value: "[\"kurkumin\",\"curcumin\"]");

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E101",
                column: "Synonyms",
                value: "[\"riboflavin\"]");

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E102",
                column: "Synonyms",
                value: "[\"tartrazin\",\"tartrazine\"]");

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E104",
                column: "Synonyms",
                value: "[]");

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E110",
                column: "Synonyms",
                value: "[\"g\\u00FCn bat\\u0131m\\u0131 sar\\u0131s\\u0131\",\"sunset yellow\"]");

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E120",
                column: "Synonyms",
                value: "[\"ko\\u015Finil\",\"cochineal\",\"karmin\",\"carmine\"]");

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E122",
                column: "Synonyms",
                value: "[\"azorubin\",\"carmoisine\"]");

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E124",
                column: "Synonyms",
                value: "[\"ponso 4r\",\"ponceau 4r\"]");

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E127",
                column: "Synonyms",
                value: "[]");

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E129",
                column: "Synonyms",
                value: "[\"allura k\\u0131rm\\u0131z\\u0131s\\u0131\",\"allura red\"]");

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E131",
                column: "Synonyms",
                value: "[]");

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E132",
                column: "Synonyms",
                value: "[]");

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E133",
                column: "Synonyms",
                value: "[]");

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E140",
                column: "Synonyms",
                value: "[]");

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E150a",
                column: "Synonyms",
                value: "[\"karamel\",\"caramel\"]");

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E150d",
                column: "Synonyms",
                value: "[]");

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E160a",
                column: "Synonyms",
                value: "[]");

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E160c",
                column: "Synonyms",
                value: "[]");

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E162",
                column: "Synonyms",
                value: "[]");

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E163",
                column: "Synonyms",
                value: "[]");

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E171",
                column: "Synonyms",
                value: "[\"titanyum dioksit\",\"titanium dioxide\"]");

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E172",
                column: "Synonyms",
                value: "[]");

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E173",
                column: "Synonyms",
                value: "[]");

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E180",
                column: "Synonyms",
                value: "[]");

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E200",
                column: "Synonyms",
                value: "[\"sorbik asit\",\"sorbic acid\"]");

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E202",
                column: "Synonyms",
                value: "[\"potasyum sorbat\",\"potassium sorbate\"]");

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E203",
                column: "Synonyms",
                value: "[]");

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E210",
                column: "Synonyms",
                value: "[]");

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E211",
                column: "Synonyms",
                value: "[\"sodyum benzoat\",\"sodium benzoate\"]");

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E212",
                column: "Synonyms",
                value: "[]");

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E220",
                column: "Synonyms",
                value: "[\"k\\u00FCk\\u00FCrt dioksit\",\"sulphur dioxide\",\"sulfur dioxide\"]");

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E221",
                column: "Synonyms",
                value: "[]");

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E222",
                column: "Synonyms",
                value: "[]");

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E223",
                column: "Synonyms",
                value: "[]");

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E224",
                column: "Synonyms",
                value: "[]");

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E249",
                column: "Synonyms",
                value: "[]");

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E250",
                column: "Synonyms",
                value: "[\"sodyum nitrit\",\"sodium nitrite\"]");

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E251",
                column: "Synonyms",
                value: "[\"sodyum nitrat\",\"sodium nitrate\"]");

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E252",
                column: "Synonyms",
                value: "[]");

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E260",
                column: "Synonyms",
                value: "[\"asetik asit\",\"acetic acid\"]");

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E261",
                column: "Synonyms",
                value: "[]");

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E262",
                column: "Synonyms",
                value: "[]");

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E263",
                column: "Synonyms",
                value: "[]");

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E270",
                column: "Synonyms",
                value: "[\"laktik asit\",\"lactic acid\"]");

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E280",
                column: "Synonyms",
                value: "[]");

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E281",
                column: "Synonyms",
                value: "[]");

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E282",
                column: "Synonyms",
                value: "[]");

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E283",
                column: "Synonyms",
                value: "[]");

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E296",
                column: "Synonyms",
                value: "[\"malik asit\",\"malic acid\"]");

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E297",
                column: "Synonyms",
                value: "[\"fumarik asit\",\"fumaric acid\"]");

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E300",
                column: "Synonyms",
                value: "[\"askorbik asit\",\"ascorbic acid\",\"c vitamini\",\"vitamin c\"]");

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E301",
                column: "Synonyms",
                value: "[]");

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E306",
                column: "Synonyms",
                value: "[]");

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E307",
                column: "Synonyms",
                value: "[]");

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E310",
                column: "Synonyms",
                value: "[]");

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E319",
                column: "Synonyms",
                value: "[\"tbhq\"]");

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E320",
                column: "Synonyms",
                value: "[\"bha\"]");

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E321",
                column: "Synonyms",
                value: "[\"bht\"]");

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E322",
                column: "Synonyms",
                value: "[\"lesitin\",\"lecithin\",\"soya lesitini\",\"soy lecithin\"]");

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E325",
                column: "Synonyms",
                value: "[]");

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E330",
                column: "Synonyms",
                value: "[\"sitrik asit\",\"citric acid\"]");

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E331",
                column: "Synonyms",
                value: "[]");

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E332",
                column: "Synonyms",
                value: "[]");

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E333",
                column: "Synonyms",
                value: "[]");

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E334",
                column: "Synonyms",
                value: "[\"tartarik asit\",\"tartaric acid\"]");

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E335",
                column: "Synonyms",
                value: "[]");

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E336",
                column: "Synonyms",
                value: "[]");

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E338",
                column: "Synonyms",
                value: "[]");

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E339",
                column: "Synonyms",
                value: "[\"sodyum fosfat\",\"sodium phosphate\"]");

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E340",
                column: "Synonyms",
                value: "[]");

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E341",
                column: "Synonyms",
                value: "[]");

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E343",
                column: "Synonyms",
                value: "[]");

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E350",
                column: "Synonyms",
                value: "[]");

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E355",
                column: "Synonyms",
                value: "[]");

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E363",
                column: "Synonyms",
                value: "[]");

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E385",
                column: "Synonyms",
                value: "[]");

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E400",
                column: "Synonyms",
                value: "[]");

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E401",
                column: "Synonyms",
                value: "[\"sodyum aljinat\",\"sodium alginate\"]");

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E402",
                column: "Synonyms",
                value: "[]");

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E404",
                column: "Synonyms",
                value: "[]");

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E406",
                column: "Synonyms",
                value: "[\"agar\",\"agar agar\"]");

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E407",
                column: "Synonyms",
                value: "[\"karragenan\",\"carrageenan\"]");

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E410",
                column: "Synonyms",
                value: "[]");

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E412",
                column: "Synonyms",
                value: "[\"guar gam\\u0131\",\"guar gum\"]");

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E413",
                column: "Synonyms",
                value: "[]");

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E414",
                column: "Synonyms",
                value: "[]");

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E415",
                column: "Synonyms",
                value: "[\"ksantan gam\\u0131\",\"xanthan gum\"]");

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E416",
                column: "Synonyms",
                value: "[]");

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E417",
                column: "Synonyms",
                value: "[]");

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E418",
                column: "Synonyms",
                value: "[]");

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E420",
                column: "Synonyms",
                value: "[\"sorbitol\"]");

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E421",
                column: "Synonyms",
                value: "[\"mannitol\"]");

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E422",
                column: "Synonyms",
                value: "[]");

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E433",
                column: "Synonyms",
                value: "[]");

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E440",
                column: "Synonyms",
                value: "[\"pektin\",\"pectin\"]");

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E442",
                column: "Synonyms",
                value: "[]");

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E460",
                column: "Synonyms",
                value: "[]");

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E461",
                column: "Synonyms",
                value: "[]");

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E463",
                column: "Synonyms",
                value: "[]");

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E464",
                column: "Synonyms",
                value: "[]");

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E466",
                column: "Synonyms",
                value: "[]");

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E470a",
                column: "Synonyms",
                value: "[]");

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E471",
                column: "Synonyms",
                value: "[\"mono ve digliseritler\",\"mono- and diglycerides\"]");

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E472e",
                column: "Synonyms",
                value: "[]");

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E473",
                column: "Synonyms",
                value: "[]");

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E475",
                column: "Synonyms",
                value: "[]");

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E476",
                column: "Synonyms",
                value: "[]");

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E477",
                column: "Synonyms",
                value: "[]");

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E481",
                column: "Synonyms",
                value: "[]");

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E491",
                column: "Synonyms",
                value: "[]");

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E492",
                column: "Synonyms",
                value: "[]");

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E495",
                column: "Synonyms",
                value: "[]");

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E500",
                column: "Synonyms",
                value: "[\"sodyum bikarbonat\",\"sodium bicarbonate\",\"karbonat\"]");

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E501",
                column: "Synonyms",
                value: "[]");

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E503",
                column: "Synonyms",
                value: "[]");

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E507",
                column: "Synonyms",
                value: "[]");

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E508",
                column: "Synonyms",
                value: "[]");

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E509",
                column: "Synonyms",
                value: "[]");

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E511",
                column: "Synonyms",
                value: "[]");

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E551",
                column: "Synonyms",
                value: "[]");

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E553b",
                column: "Synonyms",
                value: "[]");

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E574",
                column: "Synonyms",
                value: "[]");

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E575",
                column: "Synonyms",
                value: "[]");

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E576",
                column: "Synonyms",
                value: "[]");

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E577",
                column: "Synonyms",
                value: "[]");

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E578",
                column: "Synonyms",
                value: "[]");

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E620",
                column: "Synonyms",
                value: "[]");

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E621",
                column: "Synonyms",
                value: "[\"monosodyum glutamat\",\"monosodium glutamate\",\"msg\"]");

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E622",
                column: "Synonyms",
                value: "[]");

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E623",
                column: "Synonyms",
                value: "[]");

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E627",
                column: "Synonyms",
                value: "[]");

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E631",
                column: "Synonyms",
                value: "[]");

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E635",
                column: "Synonyms",
                value: "[]");

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E901",
                column: "Synonyms",
                value: "[]");

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E903",
                column: "Synonyms",
                value: "[]");

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E904",
                column: "Synonyms",
                value: "[]");

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E941",
                column: "Synonyms",
                value: "[]");

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E942",
                column: "Synonyms",
                value: "[]");

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E950",
                column: "Synonyms",
                value: "[\"ases\\u00FClfam k\",\"acesulfame k\",\"acesulfame potassium\"]");

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E951",
                column: "Synonyms",
                value: "[\"aspartam\",\"aspartame\"]");

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E952",
                column: "Synonyms",
                value: "[]");

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E954",
                column: "Synonyms",
                value: "[]");

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E955",
                column: "Synonyms",
                value: "[\"s\\u00FCkraloz\",\"sucralose\"]");

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E960",
                column: "Synonyms",
                value: "[\"stevia\",\"steviol glikozitleri\",\"steviol glycosides\"]");

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E961",
                column: "Synonyms",
                value: "[]");

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E962",
                column: "Synonyms",
                value: "[]");

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E965",
                column: "Synonyms",
                value: "[]");

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E966",
                column: "Synonyms",
                value: "[]");

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E967",
                column: "Synonyms",
                value: "[\"ksilitol\",\"xylitol\"]");

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E968",
                column: "Synonyms",
                value: "[]");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Synonyms",
                table: "Additives");
        }
    }
}
