using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blacklabel.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddAdditiveTranslations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DescriptionDe",
                table: "Additives",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DescriptionEs",
                table: "Additives",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DescriptionFr",
                table: "Additives",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NameDe",
                table: "Additives",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NameEs",
                table: "Additives",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NameFr",
                table: "Additives",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E100",
                columns: new[] { "DescriptionDe", "DescriptionEs", "DescriptionFr", "NameDe", "NameEs", "NameFr" },
                values: new object[] { "Natürlicher gelber Farbstoff aus Kurkuma.", "Colorante amarillo natural extraído de la cúrcuma.", "Colorant jaune naturel extrait du curcuma.", "Kurkumin", "Curcumina", "Curcumine" });

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E101",
                columns: new[] { "DescriptionDe", "DescriptionEs", "DescriptionFr", "NameDe", "NameEs", "NameFr" },
                values: new object[] { "Ein gelbes Vitamin, das natürlich in vielen Lebensmitteln vorkommt; wird auch als Farbstoff verwendet.", "Vitamina amarilla presente de forma natural en muchos alimentos; también se usa como colorante.", "Vitamine jaune naturellement présente dans de nombreux aliments ; également utilisée comme colorant.", "Riboflavin (Vitamin B2)", "Riboflavina (vitamina B2)", "Riboflavine (vitamine B2)" });

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E102",
                columns: new[] { "DescriptionDe", "DescriptionEs", "DescriptionFr", "NameDe", "NameEs", "NameFr" },
                values: new object[] { "Synthetischer Azofarbstoff. Nach EU-Recht müssen Produkte, die ihn enthalten, den Warnhinweis „kann Aktivität und Aufmerksamkeit bei Kindern beeinträchtigen“ tragen.", "Colorante azoico sintético. Según la normativa de la UE, los productos que lo contienen deben llevar la advertencia «puede tener efectos negativos sobre la actividad y la atención de los niños».", "Colorant azoïque synthétique. Selon la réglementation européenne, les produits qui en contiennent doivent porter la mention « peut avoir un effet négatif sur l'activité et l'attention chez les enfants ».", "Tartrazin", "Tartracina", "Tartrazine" });

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E104",
                columns: new[] { "DescriptionDe", "DescriptionEs", "DescriptionFr", "NameDe", "NameEs", "NameFr" },
                values: new object[] { "Synthetischer gelber Farbstoff. Einer von sechs Farbstoffen, die in der EU den Warnhinweis zu Verhalten/Aufmerksamkeit bei Kindern tragen müssen.", "Colorante amarillo sintético. Uno de los seis colorantes que deben llevar la advertencia de la UE sobre el comportamiento/la atención en niños.", "Colorant jaune synthétique. L'un des six colorants devant porter la mention européenne sur le comportement/l'attention des enfants.", "Chinolingelb", "Amarillo de quinoleína", "Jaune de quinoléine" });

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E110",
                columns: new[] { "DescriptionDe", "DescriptionEs", "DescriptionFr", "NameDe", "NameEs", "NameFr" },
                values: new object[] { "Synthetischer Azofarbstoff. Muss laut EU-Recht den Warnhinweis zu Verhalten/Aufmerksamkeit bei Kindern tragen.", "Colorante azoico sintético. Debe llevar la advertencia de la UE sobre el comportamiento/la atención en niños.", "Colorant azoïque synthétique. Doit porter la mention européenne sur le comportement/l'attention des enfants.", "Gelborange S", "Amarillo anaranjado S", "Jaune orangé S" });

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E120",
                columns: new[] { "DescriptionDe", "DescriptionEs", "DescriptionFr", "NameDe", "NameEs", "NameFr" },
                values: new object[] { "Roter Farbstoff aus der Cochenilleschildlaus. Nicht vegan; allergische Reaktionen wurden selten berichtet.", "Colorante rojo obtenido de la cochinilla. No es vegano; se han notificado reacciones alérgicas en raras ocasiones.", "Colorant rouge extrait de la cochenille. Non végan ; de rares réactions allergiques ont été signalées.", "Cochenille / Karminsäure", "Cochinilla / Ácido carmínico", "Cochenille / Acide carminique" });

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E122",
                columns: new[] { "DescriptionDe", "DescriptionEs", "DescriptionFr", "NameDe", "NameEs", "NameFr" },
                values: new object[] { "Synthetischer Azofarbstoff. Muss laut EU-Recht den Warnhinweis zu Verhalten/Aufmerksamkeit bei Kindern tragen.", "Colorante azoico sintético. Debe llevar la advertencia de la UE sobre el comportamiento/la atención en niños.", "Colorant azoïque synthétique. Doit porter la mention européenne sur le comportement/l'attention des enfants.", "Azorubin (Carmoisin)", "Azorrubina (Carmoisina)", "Azorubine (Carmoisine)" });

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E124",
                columns: new[] { "DescriptionDe", "DescriptionEs", "DescriptionFr", "NameDe", "NameEs", "NameFr" },
                values: new object[] { "Synthetischer Azofarbstoff. Muss laut EU-Recht den Warnhinweis zu Verhalten/Aufmerksamkeit bei Kindern tragen.", "Colorante azoico sintético. Debe llevar la advertencia de la UE sobre el comportamiento/la atención en niños.", "Colorant azoïque synthétique. Doit porter la mention européenne sur le comportement/l'attention des enfants.", "Cochenillerot A (Ponceau 4R)", "Ponceau 4R", "Ponceau 4R" });

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E127",
                columns: new[] { "DescriptionDe", "DescriptionEs", "DescriptionFr", "NameDe", "NameEs", "NameFr" },
                values: new object[] { "Jodhaltiger roter Farbstoff mit eingeschränkten Verwendungszwecken. Vorsicht wird bei Schilddrüsenempfindlichkeit empfohlen.", "Colorante rojo yodado de uso restringido. Se recomienda precaución en personas con sensibilidad tiroidea.", "Colorant rouge iodé à usages restreints. La prudence est recommandée en cas de sensibilité thyroïdienne.", "Erythrosin", "Eritrosina", "Érythrosine" });

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E129",
                columns: new[] { "DescriptionDe", "DescriptionEs", "DescriptionFr", "NameDe", "NameEs", "NameFr" },
                values: new object[] { "Synthetischer Azofarbstoff. Muss laut EU-Recht den Warnhinweis zu Verhalten/Aufmerksamkeit bei Kindern tragen.", "Colorante azoico sintético. Debe llevar la advertencia de la UE sobre el comportamiento/la atención en niños.", "Colorant azoïque synthétique. Doit porter la mention européenne sur le comportement/l'attention des enfants.", "Allurarot AC", "Rojo allura AC", "Rouge allura AC" });

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E131",
                columns: new[] { "DescriptionDe", "DescriptionEs", "DescriptionFr", "NameDe", "NameEs", "NameFr" },
                values: new object[] { "Synthetischer blauer Farbstoff. Selten mit allergischen Reaktionen in Verbindung gebracht.", "Colorante azul sintético. Rara vez se asocia con reacciones alérgicas.", "Colorant bleu synthétique. Rarement associé à des réactions allergiques.", "Patentblau V", "Azul patente V", "Bleu patenté V" });

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E132",
                columns: new[] { "DescriptionDe", "DescriptionEs", "DescriptionFr", "NameDe", "NameEs", "NameFr" },
                values: new object[] { "Synthetischer blauer Farbstoff, im Rahmen der zulässigen Verwendung allgemein als unbedenklich anerkannt.", "Colorante azul sintético, generalmente reconocido como seguro dentro de los usos permitidos.", "Colorant bleu synthétique, généralement reconnu comme sûr dans le cadre des usages autorisés.", "Indigotin", "Indigotina", "Indigotine" });

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E133",
                columns: new[] { "DescriptionDe", "DescriptionEs", "DescriptionFr", "NameDe", "NameEs", "NameFr" },
                values: new object[] { "Synthetischer blauer Farbstoff, im Rahmen der zulässigen Verwendung allgemein als unbedenklich anerkannt.", "Colorante azul sintético, generalmente reconocido como seguro dentro de los usos permitidos.", "Colorant bleu synthétique, généralement reconnu comme sûr dans le cadre des usages autorisés.", "Brillantblau FCF", "Azul brillante FCF", "Bleu brillant FCF" });

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E140",
                columns: new[] { "DescriptionDe", "DescriptionEs", "DescriptionFr", "NameDe", "NameEs", "NameFr" },
                values: new object[] { "Natürlicher grüner Farbstoff aus Pflanzen.", "Colorante verde natural extraído de plantas.", "Colorant vert naturel extrait de végétaux.", "Chlorophylle", "Clorofilas", "Chlorophylles" });

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E150a",
                columns: new[] { "DescriptionDe", "DescriptionEs", "DescriptionFr", "NameDe", "NameEs", "NameFr" },
                values: new object[] { "Brauner Farbstoff, der durch Erhitzen von Zucker (Karamellisierung) gewonnen wird.", "Colorante marrón obtenido caramelizando azúcar mediante calor.", "Colorant brun obtenu par caramélisation du sucre sous l'effet de la chaleur.", "Zuckerkulör (einfach)", "Caramelo natural", "Caramel ordinaire" });

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E150d",
                columns: new[] { "DescriptionDe", "DescriptionEs", "DescriptionFr", "NameDe", "NameEs", "NameFr" },
                values: new object[] { "Karamelltyp, der häufig in Cola-Getränken verwendet wird. Die EFSA hat für das bei der Herstellung in Spuren entstehende 4-MEI eine tägliche Aufnahmegrenze festgelegt.", "Tipo de caramelo utilizado habitualmente en bebidas de cola. La EFSA ha establecido un límite de ingesta diaria para el 4-MEI que puede formarse en trazas durante la producción.", "Type de caramel couramment utilisé dans les boissons au cola. L'EFSA a fixé une limite d'apport journalier pour les traces de 4-MEI pouvant se former lors de la production.", "Zuckerkulör, Sulfit-Ammoniak-Verfahren", "Caramelo de sulfito amónico", "Caramel au sulfite d'ammonium" });

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E160a",
                columns: new[] { "DescriptionDe", "DescriptionEs", "DescriptionFr", "NameDe", "NameEs", "NameFr" },
                values: new object[] { "Orange-gelber Farbstoff, der natürlich in Pflanzen wie Karotten vorkommt.", "Colorante naranja-amarillo presente de forma natural en plantas como la zanahoria.", "Colorant orange-jaune naturellement présent dans des végétaux comme la carotte.", "Carotine", "Carotenos", "Carotènes" });

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E160c",
                columns: new[] { "DescriptionDe", "DescriptionEs", "DescriptionFr", "NameDe", "NameEs", "NameFr" },
                values: new object[] { "Natürlicher orange-roter Farbstoff aus Paprikaschoten.", "Colorante natural naranja-rojo extraído de pimientos.", "Colorant naturel orange-rouge extrait de piments paprika.", "Paprikaextrakt", "Extracto de pimentón", "Extrait de paprika" });

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E162",
                columns: new[] { "DescriptionDe", "DescriptionEs", "DescriptionFr", "NameDe", "NameEs", "NameFr" },
                values: new object[] { "Natürlicher roter Farbstoff aus Roter Bete.", "Colorante rojo natural extraído de la remolacha roja.", "Colorant rouge naturel extrait de la betterave rouge.", "Betenrot (Betanin)", "Rojo de remolacha (betanina)", "Rouge de betterave (bétanine)" });

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E163",
                columns: new[] { "DescriptionDe", "DescriptionEs", "DescriptionFr", "NameDe", "NameEs", "NameFr" },
                values: new object[] { "Gruppe natürlicher rot-violetter Farbstoffe aus Quellen wie Traubenschalen.", "Grupo de colorantes naturales rojo-morado extraídos de fuentes como la piel de la uva.", "Groupe de colorants naturels rouge-violet extraits de sources telles que la peau du raisin.", "Anthocyane", "Antocianinas", "Anthocyanes" });

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E171",
                columns: new[] { "DescriptionDe", "DescriptionEs", "DescriptionFr", "NameDe", "NameEs", "NameFr" },
                values: new object[] { "Weißer Farbstoff. Die EFSA-Bewertung von 2021 konnte ein genotoxisches Risiko nicht ausschließen; die EU hat die Verwendung in Lebensmitteln seit 2022 verboten.", "Colorante blanco. El dictamen de la EFSA de 2021 no pudo descartar un riesgo de genotoxicidad, y la UE ha prohibido su uso en alimentos desde 2022.", "Colorant blanc. L'avis de l'EFSA de 2021 n'a pas pu écarter un risque de génotoxicité, et l'UE en a interdit l'usage alimentaire depuis 2022.", "Titandioxid", "Dióxido de titanio", "Dioxyde de titane" });

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E172",
                columns: new[] { "DescriptionDe", "DescriptionEs", "DescriptionFr", "NameDe", "NameEs", "NameFr" },
                values: new object[] { "Farbstoff mineralischen Ursprungs für Braun-, Rot- und Schwarztöne.", "Colorante de origen mineral utilizado en tonos marrones, rojos y negros.", "Colorant d'origine minérale utilisé pour les teintes brunes, rouges et noires.", "Eisenoxide", "Óxidos de hierro", "Oxydes de fer" });

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E173",
                columns: new[] { "DescriptionDe", "DescriptionEs", "DescriptionFr", "NameDe", "NameEs", "NameFr" },
                values: new object[] { "Metallischer Farbstoff für Oberflächenüberzüge/Dekoration mit eingeschränkten Verwendungszwecken.", "Colorante metálico utilizado para recubrimiento superficial y decoración, de uso restringido.", "Colorant métallique utilisé pour l'enrobage de surface et la décoration, à usages restreints.", "Aluminium", "Aluminio", "Aluminium" });

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E180",
                columns: new[] { "DescriptionDe", "DescriptionEs", "DescriptionFr", "NameDe", "NameEs", "NameFr" },
                values: new object[] { "Synthetischer roter Farbstoff, der in einigen Käserinden verwendet wird, mit eingeschränkten Verwendungszwecken.", "Colorante rojo sintético utilizado en algunas cortezas de queso, de uso restringido.", "Colorant rouge synthétique utilisé dans certaines croûtes de fromage, à usages restreints.", "Litholrubin BK", "Litorrubina BK", "Rubis lithol BK" });

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E200",
                columns: new[] { "DescriptionDe", "DescriptionEs", "DescriptionFr", "NameDe", "NameEs", "NameFr" },
                values: new object[] { "Ein auch natürlich vorkommender Konservierungsstoff, der das Wachstum von Schimmel und Hefen hemmt.", "Un conservante también presente de forma natural que inhibe el crecimiento de mohos y levaduras.", "Un conservateur également présent à l'état naturel, qui inhibe le développement des moisissures et des levures.", "Sorbinsäure", "Ácido sórbico", "Acide sorbique" });

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E202",
                columns: new[] { "DescriptionDe", "DescriptionEs", "DescriptionFr", "NameDe", "NameEs", "NameFr" },
                values: new object[] { "Ein Salz der Sorbinsäure; ein weit verbreiteter Konservierungsstoff gegen Schimmel- und Hefebildung.", "Una sal del ácido sórbico; un conservante muy utilizado contra mohos y levaduras.", "Un sel de l'acide sorbique ; un conservateur largement utilisé contre les moisissures et les levures.", "Kaliumsorbat", "Sorbato de potasio", "Sorbate de potassium" });

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E203",
                columns: new[] { "DescriptionDe", "DescriptionEs", "DescriptionFr", "NameDe", "NameEs", "NameFr" },
                values: new object[] { "Ein Salz der Sorbinsäure; wird als Konservierungsstoff verwendet.", "Una sal del ácido sórbico utilizada como conservante.", "Un sel de l'acide sorbique utilisé comme conservateur.", "Calciumsorbat", "Sorbato de calcio", "Sorbate de calcium" });

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E210",
                columns: new[] { "DescriptionDe", "DescriptionEs", "DescriptionFr", "NameDe", "NameEs", "NameFr" },
                values: new object[] { "Ein Konservierungsstoff, der das Wachstum von Bakterien und Pilzen hemmt. Die EFSA hat eine tägliche Aufnahmegrenze festgelegt.", "Un conservante que inhibe el crecimiento de bacterias y hongos. La EFSA ha establecido un límite de ingesta diaria.", "Un conservateur qui inhibe la croissance des bactéries et des champignons. L'EFSA a fixé une limite d'apport journalier.", "Benzoesäure", "Ácido benzoico", "Acide benzoïque" });

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E211",
                columns: new[] { "DescriptionDe", "DescriptionEs", "DescriptionFr", "NameDe", "NameEs", "NameFr" },
                values: new object[] { "Ein weit verbreiteter Konservierungsstoff. Einige Studien haben gezeigt, dass in Kombination mit Vitamin C und bei Einwirkung von Hitze/Licht Spuren von Benzol entstehen können; die EFSA hat eine tägliche Aufnahmegrenze festgelegt.", "Un conservante muy utilizado. Algunos estudios han mostrado que pueden formarse trazas de benceno combinado con vitamina C bajo exposición al calor o la luz; la EFSA ha establecido un límite de ingesta diaria.", "Un conservateur largement utilisé. Certaines études ont montré que des traces de benzène peuvent se former en association avec la vitamine C sous l'effet de la chaleur ou de la lumière ; l'EFSA a fixé une limite d'apport journalier.", "Natriumbenzoat", "Benzoato de sodio", "Benzoate de sodium" });

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E212",
                columns: new[] { "DescriptionDe", "DescriptionEs", "DescriptionFr", "NameDe", "NameEs", "NameFr" },
                values: new object[] { "Ein Salz der Benzoesäure; wird als Konservierungsstoff verwendet.", "Una sal del ácido benzoico utilizada como conservante.", "Un sel de l'acide benzoïque utilisé comme conservateur.", "Kaliumbenzoat", "Benzoato de potasio", "Benzoate de potassium" });

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E220",
                columns: new[] { "DescriptionDe", "DescriptionEs", "DescriptionFr", "NameDe", "NameEs", "NameFr" },
                values: new object[] { "Ein häufiger Konservierungsstoff in Trockenfrüchten und Wein. Bei empfindlichen Personen, insbesondere Asthmatikern, wurden Atemwegsreaktionen berichtet; in der EU ist eine verpflichtende Allergenkennzeichnung vorgeschrieben.", "Un conservante habitual en frutas desecadas y vinos. Se han notificado reacciones respiratorias en personas sensibles, especialmente asmáticas; el etiquetado como alérgeno es obligatorio en la UE.", "Un conservateur courant dans les fruits secs et le vin. Des réactions respiratoires ont été signalées chez les personnes sensibles, en particulier les asthmatiques ; l'étiquetage allergène est obligatoire dans l'UE.", "Schwefeldioxid", "Dióxido de azufre", "Anhydride sulfureux" });

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E221",
                columns: new[] { "DescriptionDe", "DescriptionEs", "DescriptionFr", "NameDe", "NameEs", "NameFr" },
                values: new object[] { "Ein Konservierungsstoff aus der Sulfitgruppe. Kann bei empfindlichen Personen Reaktionen auslösen; die Allergenkennzeichnung ist verpflichtend.", "Un conservante del grupo de los sulfitos. Puede causar reacciones en personas sensibles; el etiquetado como alérgeno es obligatorio.", "Un conservateur du groupe des sulfites. Peut provoquer des réactions chez les personnes sensibles ; l'étiquetage allergène est obligatoire.", "Natriumsulfit", "Sulfito de sodio", "Sulfite de sodium" });

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E222",
                columns: new[] { "DescriptionDe", "DescriptionEs", "DescriptionFr", "NameDe", "NameEs", "NameFr" },
                values: new object[] { "Ein Konservierungsstoff aus der Sulfitgruppe. Kann bei empfindlichen Personen Reaktionen auslösen; die Allergenkennzeichnung ist verpflichtend.", "Un conservante del grupo de los sulfitos. Puede causar reacciones en personas sensibles; el etiquetado como alérgeno es obligatorio.", "Un conservateur du groupe des sulfites. Peut provoquer des réactions chez les personnes sensibles ; l'étiquetage allergène est obligatoire.", "Natriumhydrogensulfit", "Hidrogenosulfito de sodio", "Hydrogénosulfite de sodium" });

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E223",
                columns: new[] { "DescriptionDe", "DescriptionEs", "DescriptionFr", "NameDe", "NameEs", "NameFr" },
                values: new object[] { "Ein Konservierungsstoff aus der Sulfitgruppe. Kann bei empfindlichen Personen, insbesondere Asthmatikern, Reaktionen auslösen; die Allergenkennzeichnung ist verpflichtend.", "Un conservante del grupo de los sulfitos. Puede causar reacciones en personas sensibles, especialmente asmáticas; el etiquetado como alérgeno es obligatorio.", "Un conservateur du groupe des sulfites. Peut provoquer des réactions chez les personnes sensibles, en particulier les asthmatiques ; l'étiquetage allergène est obligatoire.", "Natriummetabisulfit", "Metabisulfito de sodio", "Métabisulfite de sodium" });

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E224",
                columns: new[] { "DescriptionDe", "DescriptionEs", "DescriptionFr", "NameDe", "NameEs", "NameFr" },
                values: new object[] { "Ein Konservierungsstoff aus der Sulfitgruppe. Kann bei empfindlichen Personen Reaktionen auslösen; die Allergenkennzeichnung ist verpflichtend.", "Un conservante del grupo de los sulfitos. Puede causar reacciones en personas sensibles; el etiquetado como alérgeno es obligatorio.", "Un conservateur du groupe des sulfites. Peut provoquer des réactions chez les personnes sensibles ; l'étiquetage allergène est obligatoire.", "Kaliummetabisulfit", "Metabisulfito de potasio", "Métabisulfite de potassium" });

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E249",
                columns: new[] { "DescriptionDe", "DescriptionEs", "DescriptionFr", "NameDe", "NameEs", "NameFr" },
                values: new object[] { "Ein Konservierungsstoff, der in verarbeiteten Fleischprodukten zur Vorbeugung von Botulismus verwendet wird. Die EFSA-Bewertung von 2017 senkte die zulässige Tagesdosis; die Bildung von Nitrosaminen wurde in einigen Studien untersucht.", "Un conservante utilizado en productos cárnicos procesados para prevenir el botulismo. La evaluación de la EFSA de 2017 redujo la ingesta diaria admisible; la formación de nitrosaminas se ha estudiado en algunas investigaciones.", "Un conservateur utilisé dans les produits de charcuterie pour prévenir le botulisme. L'évaluation de l'EFSA de 2017 a abaissé la dose journalière admissible ; la formation de nitrosamines a été examinée dans certaines études.", "Kaliumnitrit", "Nitrito de potasio", "Nitrite de potassium" });

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E250",
                columns: new[] { "DescriptionDe", "DescriptionEs", "DescriptionFr", "NameDe", "NameEs", "NameFr" },
                values: new object[] { "Ein Konservierungsstoff, der in verarbeiteten Fleischprodukten (Salami, Wurst) zur Vorbeugung von Botulismus verwendet wird. Die EFSA-Bewertung von 2017 senkte die zulässige Tagesdosis; die Bildung von Nitrosaminen wurde in einigen Studien untersucht.", "Un conservante utilizado en carnes procesadas (salchichón, salami) para prevenir el botulismo. La evaluación de la EFSA de 2017 redujo la ingesta diaria admisible; la formación de nitrosaminas se ha estudiado en algunas investigaciones.", "Un conservateur utilisé dans les viandes transformées (saucisson, salami) pour prévenir le botulisme. L'évaluation de l'EFSA de 2017 a abaissé la dose journalière admissible ; la formation de nitrosamines a été examinée dans certaines études.", "Natriumnitrit", "Nitrito de sodio", "Nitrite de sodium" });

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E251",
                columns: new[] { "DescriptionDe", "DescriptionEs", "DescriptionFr", "NameDe", "NameEs", "NameFr" },
                values: new object[] { "Ein Konservierungsstoff, der in verarbeitetem Fleisch und einigen Käsesorten verwendet wird. Die EFSA hat eine tägliche Aufnahmegrenze festgelegt.", "Un conservante utilizado en carnes procesadas y algunos quesos. La EFSA ha establecido un límite de ingesta diaria.", "Un conservateur utilisé dans les viandes transformées et certains fromages. L'EFSA a fixé une limite d'apport journalier.", "Natriumnitrat", "Nitrato de sodio", "Nitrate de sodium" });

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E252",
                columns: new[] { "DescriptionDe", "DescriptionEs", "DescriptionFr", "NameDe", "NameEs", "NameFr" },
                values: new object[] { "Ein Konservierungsstoff, der in verarbeiteten Fleischprodukten verwendet wird. Die EFSA hat eine tägliche Aufnahmegrenze festgelegt.", "Un conservante utilizado en productos cárnicos procesados. La EFSA ha establecido un límite de ingesta diaria.", "Un conservateur utilisé dans les produits de charcuterie. L'EFSA a fixé une limite d'apport journalier.", "Kaliumnitrat", "Nitrato de potasio", "Nitrate de potassium" });

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E260",
                columns: new[] { "DescriptionDe", "DescriptionEs", "DescriptionFr", "NameDe", "NameEs", "NameFr" },
                values: new object[] { "Der Hauptbestandteil von Essig; wird als Konservierungsstoff und Säureregulator verwendet.", "El componente principal del vinagre; se utiliza como conservante y regulador de acidez.", "Le principal composant du vinaigre ; utilisé comme conservateur et régulateur d'acidité.", "Essigsäure", "Ácido acético", "Acide acétique" });

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E261",
                columns: new[] { "DescriptionDe", "DescriptionEs", "DescriptionFr", "NameDe", "NameEs", "NameFr" },
                values: new object[] { "Ein Salz der Essigsäure; wird als Konservierungsstoff und Säureregulator verwendet.", "Una sal del ácido acético utilizada como conservante y regulador de acidez.", "Un sel de l'acide acétique utilisé comme conservateur et régulateur d'acidité.", "Kaliumacetat", "Acetato de potasio", "Acétate de potassium" });

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E262",
                columns: new[] { "DescriptionDe", "DescriptionEs", "DescriptionFr", "NameDe", "NameEs", "NameFr" },
                values: new object[] { "Salze der Essigsäure; werden als Konservierungsstoffe und Säureregulatoren verwendet.", "Sales del ácido acético utilizadas como conservantes y reguladores de acidez.", "Des sels de l'acide acétique utilisés comme conservateurs et régulateurs d'acidité.", "Natriumacetate", "Acetatos de sodio", "Acétates de sodium" });

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E263",
                columns: new[] { "DescriptionDe", "DescriptionEs", "DescriptionFr", "NameDe", "NameEs", "NameFr" },
                values: new object[] { "Ein Salz der Essigsäure; wird als Konservierungsstoff verwendet.", "Una sal del ácido acético utilizada como conservante.", "Un sel de l'acide acétique utilisé comme conservateur.", "Calciumacetat", "Acetato de calcio", "Acétate de calcium" });

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E270",
                columns: new[] { "DescriptionDe", "DescriptionEs", "DescriptionFr", "NameDe", "NameEs", "NameFr" },
                values: new object[] { "Eine Säure, die auf natürliche Weise in fermentierten Lebensmitteln entsteht und auch als Konservierungsstoff/Säureregulator zugesetzt wird.", "Un ácido que se forma de manera natural en los alimentos fermentados y que también se añade como conservante/regulador de acidez.", "Un acide qui se forme naturellement dans les aliments fermentés et qui est également ajouté comme conservateur/régulateur d'acidité.", "Milchsäure", "Ácido láctico", "Acide lactique" });

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E280",
                columns: new[] { "DescriptionDe", "DescriptionEs", "DescriptionFr", "NameDe", "NameEs", "NameFr" },
                values: new object[] { "Ein Konservierungsstoff, der in Brot und Backwaren zur Hemmung von Schimmelbildung verwendet wird.", "Un conservante utilizado en pan y productos de panadería para inhibir el crecimiento de moho.", "Un conservateur utilisé dans le pain et les produits de boulangerie pour inhiber le développement de moisissures.", "Propionsäure", "Ácido propiónico", "Acide propionique" });

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E281",
                columns: new[] { "DescriptionDe", "DescriptionEs", "DescriptionFr", "NameDe", "NameEs", "NameFr" },
                values: new object[] { "Ein Konservierungsstoff, der in Brot und Backwaren zur Hemmung von Schimmelbildung verwendet wird.", "Un conservante utilizado en pan y productos de panadería para inhibir el crecimiento de moho.", "Un conservateur utilisé dans le pain et les produits de boulangerie pour inhiber le développement de moisissures.", "Natriumpropionat", "Propionato de sodio", "Propionate de sodium" });

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E282",
                columns: new[] { "DescriptionDe", "DescriptionEs", "DescriptionFr", "NameDe", "NameEs", "NameFr" },
                values: new object[] { "Ein Konservierungsstoff, der in Brot und Backwaren zur Hemmung von Schimmelbildung verwendet wird.", "Un conservante utilizado en pan y productos de panadería para inhibir el crecimiento de moho.", "Un conservateur utilisé dans le pain et les produits de boulangerie pour inhiber le développement de moisissures.", "Calciumpropionat", "Propionato de calcio", "Propionate de calcium" });

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E283",
                columns: new[] { "DescriptionDe", "DescriptionEs", "DescriptionFr", "NameDe", "NameEs", "NameFr" },
                values: new object[] { "Ein Konservierungsstoff, der in Brot und Backwaren zur Hemmung von Schimmelbildung verwendet wird.", "Un conservante utilizado en pan y productos de panadería para inhibir el crecimiento de moho.", "Un conservateur utilisé dans le pain et les produits de boulangerie pour inhiber le développement de moisissures.", "Kaliumpropionat", "Propionato de potasio", "Propionate de potassium" });

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E296",
                columns: new[] { "DescriptionDe", "DescriptionEs", "DescriptionFr", "NameDe", "NameEs", "NameFr" },
                values: new object[] { "Eine natürlich in Früchten vorkommende Säure; wird als Säureregulator verwendet.", "Un ácido presente de forma natural en las frutas; se utiliza como regulador de acidez.", "Un acide naturellement présent dans les fruits ; utilisé comme régulateur d'acidité.", "Apfelsäure", "Ácido málico", "Acide malique" });

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E297",
                columns: new[] { "DescriptionDe", "DescriptionEs", "DescriptionFr", "NameDe", "NameEs", "NameFr" },
                values: new object[] { "Eine Säure, die als Säureregulator verwendet wird.", "Un ácido utilizado como regulador de acidez.", "Un acide utilisé comme régulateur d'acidité.", "Fumarsäure", "Ácido fumárico", "Acide fumarique" });

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E300",
                columns: new[] { "DescriptionDe", "DescriptionEs", "DescriptionFr", "NameDe", "NameEs", "NameFr" },
                values: new object[] { "Vitamin C; wird als Antioxidationsmittel eingesetzt, um Oxidation und Farbverlust zu verlangsamen.", "Vitamina C; se utiliza como antioxidante para ralentizar la oxidación y la pérdida de color.", "Vitamine C ; utilisée comme antioxydant pour ralentir l'oxydation et la dégradation de la couleur.", "Ascorbinsäure (Vitamin C)", "Ácido ascórbico (vitamina C)", "Acide ascorbique (vitamine C)" });

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E301",
                columns: new[] { "DescriptionDe", "DescriptionEs", "DescriptionFr", "NameDe", "NameEs", "NameFr" },
                values: new object[] { "Ein Salz von Vitamin C; wird als Antioxidationsmittel verwendet.", "Una sal de la vitamina C utilizada como antioxidante.", "Un sel de la vitamine C utilisé comme antioxydant.", "Natriumascorbat", "Ascorbato de sodio", "Ascorbate de sodium" });

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E306",
                columns: new[] { "DescriptionDe", "DescriptionEs", "DescriptionFr", "NameDe", "NameEs", "NameFr" },
                values: new object[] { "Natürlich gewonnene Vitamin-E-Verbindungen; Antioxidantien, die die Oxidation von Fetten verlangsamen.", "Compuestos de vitamina E de origen natural; antioxidantes que ralentizan la oxidación de las grasas.", "Composés de vitamine E d'origine naturelle ; des antioxydants qui ralentissent l'oxydation des matières grasses.", "Tocopherole (Vitamin E)", "Tocoferoles (vitamina E)", "Tocophérols (vitamine E)" });

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E307",
                columns: new[] { "DescriptionDe", "DescriptionEs", "DescriptionFr", "NameDe", "NameEs", "NameFr" },
                values: new object[] { "Eine synthetische Form von Vitamin E; wird als Antioxidationsmittel verwendet.", "Una forma sintética de vitamina E utilizada como antioxidante.", "Une forme synthétique de vitamine E utilisée comme antioxydant.", "Alpha-Tocopherol", "Alfa-tocoferol", "Alpha-tocophérol" });

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E310",
                columns: new[] { "DescriptionDe", "DescriptionEs", "DescriptionFr", "NameDe", "NameEs", "NameFr" },
                values: new object[] { "Ein Antioxidationsmittel, das Oxidation in Fetten und fetthaltigen Lebensmitteln verhindert. Die EFSA hat eine tägliche Aufnahmegrenze festgelegt.", "Un antioxidante que previene la oxidación en grasas y alimentos grasos. La EFSA ha establecido un límite de ingesta diaria.", "Un antioxydant qui prévient l'oxydation dans les matières grasses et les aliments qui en contiennent. L'EFSA a fixé une limite d'apport journalier.", "Propylgallat", "Galato de propilo", "Gallate de propyle" });

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E319",
                columns: new[] { "DescriptionDe", "DescriptionEs", "DescriptionFr", "NameDe", "NameEs", "NameFr" },
                values: new object[] { "Ein synthetisches Antioxidationsmittel, das das Ranzigwerden von Fetten verzögert. Die EFSA hat eine tägliche Aufnahmegrenze festgelegt; einige Tierstudien zu hohen Dosen werden weiterhin diskutiert.", "Un antioxidante sintético que retrasa el enranciamiento de las grasas. La EFSA ha establecido un límite de ingesta diaria; algunos estudios en animales sobre dosis altas siguen siendo objeto de debate.", "Un antioxydant synthétique qui retarde le rancissement des matières grasses. L'EFSA a fixé une limite d'apport journalier ; certaines études animales sur les effets à haute dose restent débattues.", "TBHQ (tertiär-Butylhydrochinon)", "TBHQ (terbutilhidroquinona)", "TBHQ (tertiobutylhydroquinone)" });

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E320",
                columns: new[] { "DescriptionDe", "DescriptionEs", "DescriptionFr", "NameDe", "NameEs", "NameFr" },
                values: new object[] { "Ein synthetisches Antioxidationsmittel. Von der IARC als „möglicherweise krebserregend beim Menschen“ (Gruppe 2B) eingestuft; die EFSA hält es bei den aktuellen Verwendungsmengen innerhalb der festgelegten täglichen Aufnahmegrenze für unbedenklich.", "Un antioxidante sintético. Clasificado por la IARC como «posiblemente carcinógeno para el ser humano» (grupo 2B); la EFSA lo considera seguro en los niveles de uso actuales dentro del límite de ingesta diaria establecido.", "Un antioxydant synthétique. Classé par le CIRC comme « peut-être cancérogène pour l'homme » (groupe 2B) ; l'EFSA le considère sûr aux niveaux d'utilisation actuels dans la limite de la dose journalière admissible fixée.", "BHA (Butylhydroxyanisol)", "BHA (hidroxianisol butilado)", "BHA (hydroxyanisole butylé)" });

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E321",
                columns: new[] { "DescriptionDe", "DescriptionEs", "DescriptionFr", "NameDe", "NameEs", "NameFr" },
                values: new object[] { "Ein synthetisches Antioxidationsmittel. Die EFSA hat eine tägliche Aufnahmegrenze festgelegt; endokrine Wirkungen wurden in einigen Studien diskutiert.", "Un antioxidante sintético. La EFSA ha establecido un límite de ingesta diaria; algunos estudios han debatido posibles efectos endocrinos.", "Un antioxydant synthétique. L'EFSA a fixé une limite d'apport journalier ; des effets endocriniens ont été évoqués dans certaines études.", "BHT (Butylhydroxytoluol)", "BHT (hidroxitolueno butilado)", "BHT (hydroxytoluène butylé)" });

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E322",
                columns: new[] { "DescriptionDe", "DescriptionEs", "DescriptionFr", "NameDe", "NameEs", "NameFr" },
                values: new object[] { "Ein natürlicher Emulgator, meist aus Soja oder Sonnenblumen gewonnen.", "Un emulgente natural obtenido generalmente de la soja o el girasol.", "Un émulsifiant naturel généralement extrait du soja ou du tournesol.", "Lecithin", "Lecitina", "Lécithine" });

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E325",
                columns: new[] { "DescriptionDe", "DescriptionEs", "DescriptionFr", "NameDe", "NameEs", "NameFr" },
                values: new object[] { "Ein Salz der Milchsäure; wird als Säureregulator und Feuchthaltemittel verwendet.", "Una sal del ácido láctico utilizada como regulador de acidez y humectante.", "Un sel de l'acide lactique utilisé comme régulateur d'acidité et humectant.", "Natriumlactat", "Lactato de sodio", "Lactate de sodium" });

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E330",
                columns: new[] { "DescriptionDe", "DescriptionEs", "DescriptionFr", "NameDe", "NameEs", "NameFr" },
                values: new object[] { "Eine natürlich in Zitrusfrüchten vorkommende Säure, die in Lebensmitteln weit verbreitet als Säureregulator verwendet wird.", "Un ácido presente de forma natural en los cítricos, muy utilizado como regulador de acidez en los alimentos.", "Un acide naturellement présent dans les agrumes, largement utilisé comme régulateur d'acidité dans les aliments.", "Citronensäure", "Ácido cítrico", "Acide citrique" });

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E331",
                columns: new[] { "DescriptionDe", "DescriptionEs", "DescriptionFr", "NameDe", "NameEs", "NameFr" },
                values: new object[] { "Salze der Citronensäure; werden als Säureregulator verwendet.", "Sales del ácido cítrico utilizadas como regulador de acidez.", "Des sels de l'acide citrique utilisés comme régulateur d'acidité.", "Natriumcitrate", "Citratos de sodio", "Citrates de sodium" });

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E332",
                columns: new[] { "DescriptionDe", "DescriptionEs", "DescriptionFr", "NameDe", "NameEs", "NameFr" },
                values: new object[] { "Salze der Citronensäure; werden als Säureregulator verwendet.", "Sales del ácido cítrico utilizadas como regulador de acidez.", "Des sels de l'acide citrique utilisés comme régulateur d'acidité.", "Kaliumcitrate", "Citratos de potasio", "Citrates de potassium" });

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E333",
                columns: new[] { "DescriptionDe", "DescriptionEs", "DescriptionFr", "NameDe", "NameEs", "NameFr" },
                values: new object[] { "Salze der Citronensäure; werden als Säureregulator verwendet.", "Sales del ácido cítrico utilizadas como regulador de acidez.", "Des sels de l'acide citrique utilisés comme régulateur d'acidité.", "Calciumcitrate", "Citratos de calcio", "Citrates de calcium" });

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E334",
                columns: new[] { "DescriptionDe", "DescriptionEs", "DescriptionFr", "NameDe", "NameEs", "NameFr" },
                values: new object[] { "Eine natürlich in Trauben vorkommende Säure; wird als Säureregulator verwendet.", "Un ácido presente de forma natural en la uva; se utiliza como regulador de acidez.", "Un acide naturellement présent dans le raisin ; utilisé comme régulateur d'acidité.", "Weinsäure", "Ácido tartárico", "Acide tartrique" });

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E335",
                columns: new[] { "DescriptionDe", "DescriptionEs", "DescriptionFr", "NameDe", "NameEs", "NameFr" },
                values: new object[] { "Salze der Weinsäure; werden als Säureregulator verwendet.", "Sales del ácido tartárico utilizadas como regulador de acidez.", "Des sels de l'acide tartrique utilisés comme régulateur d'acidité.", "Natriumtartrate", "Tartratos de sodio", "Tartrates de sodium" });

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E336",
                columns: new[] { "DescriptionDe", "DescriptionEs", "DescriptionFr", "NameDe", "NameEs", "NameFr" },
                values: new object[] { "Ein Salz, das natürlich in Wein entsteht und auch beim Backen als Unterstützung für Backtriebmittel verwendet wird.", "Una sal que se forma de manera natural en el vino, también utilizada en repostería como apoyo al gasificante.", "Un sel qui se forme naturellement dans le vin, également utilisé en pâtisserie comme soutien à l'agent levant.", "Kaliumtartrate (Weinstein)", "Tartratos de potasio (crémor tártaro)", "Tartrates de potassium (crème de tartre)" });

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E338",
                columns: new[] { "DescriptionDe", "DescriptionEs", "DescriptionFr", "NameDe", "NameEs", "NameFr" },
                values: new object[] { "Ein Säureregulator, der weit verbreitet in Cola-Getränken verwendet wird. Eine hohe Gesamtphosphataufnahme wurde in einigen Studien mit Knochen- und Herz-Kreislauf-Gesundheit in Verbindung gebracht; die EFSA hat eine tägliche Aufnahmegrenze festgelegt.", "Un regulador de acidez muy utilizado en las bebidas de cola. Una ingesta elevada de fosfatos totales se ha asociado en algunos estudios con la salud ósea y cardiovascular; la EFSA ha establecido un límite de ingesta diaria.", "Un régulateur d'acidité largement utilisé dans les boissons au cola. Un apport élevé en phosphates totaux a été associé dans certaines études à la santé osseuse et cardiovasculaire ; l'EFSA a fixé une limite d'apport journalier.", "Phosphorsäure", "Ácido fosfórico", "Acide phosphorique" });

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E339",
                columns: new[] { "DescriptionDe", "DescriptionEs", "DescriptionFr", "NameDe", "NameEs", "NameFr" },
                values: new object[] { "Säureregulator-/Emulgatorsalze, die in verarbeitetem Fleisch und Käse verwendet werden. Die EFSA hat eine tägliche Aufnahmegrenze für die Gesamtphosphataufnahme festgelegt.", "Sales reguladoras de acidez/emulgentes utilizadas en carnes procesadas y quesos. La EFSA ha establecido un límite de ingesta diaria para la ingesta total de fosfatos.", "Des sels régulateurs d'acidité/émulsifiants utilisés dans les viandes transformées et le fromage. L'EFSA a fixé une limite d'apport journalier pour l'apport total en phosphates.", "Natriumphosphate", "Fosfatos de sodio", "Phosphates de sodium" });

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E340",
                columns: new[] { "DescriptionDe", "DescriptionEs", "DescriptionFr", "NameDe", "NameEs", "NameFr" },
                values: new object[] { "Säureregulator-Salze, die in verarbeiteten Lebensmitteln verwendet werden. Die EFSA hat eine tägliche Aufnahmegrenze für die Gesamtphosphataufnahme festgelegt.", "Sales reguladoras de acidez utilizadas en alimentos procesados. La EFSA ha establecido un límite de ingesta diaria para la ingesta total de fosfatos.", "Des sels régulateurs d'acidité utilisés dans les aliments transformés. L'EFSA a fixé une limite d'apport journalier pour l'apport total en phosphates.", "Kaliumphosphate", "Fosfatos de potasio", "Phosphates de potassium" });

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E341",
                columns: new[] { "DescriptionDe", "DescriptionEs", "DescriptionFr", "NameDe", "NameEs", "NameFr" },
                values: new object[] { "Phosphatsalze, die als Backtriebmittel und zur Mineralstoffanreicherung verwendet werden.", "Sales de fosfato utilizadas como gasificantes y para el enriquecimiento mineral.", "Des sels de phosphate utilisés comme agents levants et pour l'enrichissement minéral.", "Calciumphosphate", "Fosfatos de calcio", "Phosphates de calcium" });

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E343",
                columns: new[] { "DescriptionDe", "DescriptionEs", "DescriptionFr", "NameDe", "NameEs", "NameFr" },
                values: new object[] { "Phosphatsalze, die als Säureregulator und Trennmittel verwendet werden.", "Sales de fosfato utilizadas como regulador de acidez y antiaglomerante.", "Des sels de phosphate utilisés comme régulateur d'acidité et antiagglomérant.", "Magnesiumphosphate", "Fosfatos de magnesio", "Phosphates de magnésium" });

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E350",
                columns: new[] { "DescriptionDe", "DescriptionEs", "DescriptionFr", "NameDe", "NameEs", "NameFr" },
                values: new object[] { "Salze der Apfelsäure; werden als Säureregulator verwendet.", "Sales del ácido málico utilizadas como regulador de acidez.", "Des sels de l'acide malique utilisés comme régulateur d'acidité.", "Natriummalate", "Malatos de sodio", "Malates de sodium" });

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E355",
                columns: new[] { "DescriptionDe", "DescriptionEs", "DescriptionFr", "NameDe", "NameEs", "NameFr" },
                values: new object[] { "Eine Säure, die als Säureregulator verwendet wird.", "Un ácido utilizado como regulador de acidez.", "Un acide utilisé comme régulateur d'acidité.", "Adipinsäure", "Ácido adípico", "Acide adipique" });

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E363",
                columns: new[] { "DescriptionDe", "DescriptionEs", "DescriptionFr", "NameDe", "NameEs", "NameFr" },
                values: new object[] { "Eine Säure, die als Säureregulator verwendet wird.", "Un ácido utilizado como regulador de acidez.", "Un acide utilisé comme régulateur d'acidité.", "Bernsteinsäure", "Ácido succínico", "Acide succinique" });

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E385",
                columns: new[] { "DescriptionDe", "DescriptionEs", "DescriptionFr", "NameDe", "NameEs", "NameFr" },
                values: new object[] { "Ein Konservierungsstoff, der Metallionen bindet und so Farb- und Geschmacksverschlechterung verlangsamt.", "Un conservante que capta iones metálicos y ralentiza el deterioro del color y el sabor.", "Un conservateur qui capte les ions métalliques pour ralentir la dégradation de la couleur et de la saveur.", "Calciumdinatrium-EDTA", "EDTA cálcico disódico", "EDTA calcium disodique" });

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E400",
                columns: new[] { "DescriptionDe", "DescriptionEs", "DescriptionFr", "NameDe", "NameEs", "NameFr" },
                values: new object[] { "Ein natürliches Verdickungsmittel aus braunen Meeresalgen.", "Un espesante natural extraído de algas pardas.", "Un épaississant naturel extrait d'algues brunes.", "Alginsäure", "Ácido algínico", "Acide alginique" });

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E401",
                columns: new[] { "DescriptionDe", "DescriptionEs", "DescriptionFr", "NameDe", "NameEs", "NameFr" },
                values: new object[] { "Ein Verdickungs- und Geliermittel aus Meeresalgen.", "Un espesante y gelificante extraído de algas.", "Un épaississant et gélifiant extrait d'algues.", "Natriumalginat", "Alginato de sodio", "Alginate de sodium" });

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E402",
                columns: new[] { "DescriptionDe", "DescriptionEs", "DescriptionFr", "NameDe", "NameEs", "NameFr" },
                values: new object[] { "Ein Verdickungsmittel aus Meeresalgen.", "Un espesante extraído de algas.", "Un épaississant extrait d'algues.", "Kaliumalginat", "Alginato de potasio", "Alginate de potassium" });

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E404",
                columns: new[] { "DescriptionDe", "DescriptionEs", "DescriptionFr", "NameDe", "NameEs", "NameFr" },
                values: new object[] { "Ein Verdickungs- und Geliermittel aus Meeresalgen.", "Un espesante y gelificante extraído de algas.", "Un épaississant et gélifiant extrait d'algues.", "Calciumalginat", "Alginato de calcio", "Alginate de calcium" });

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E406",
                columns: new[] { "DescriptionDe", "DescriptionEs", "DescriptionFr", "NameDe", "NameEs", "NameFr" },
                values: new object[] { "Ein natürliches Geliermittel aus roten Meeresalgen.", "Un gelificante natural extraído de algas rojas.", "Un gélifiant naturel extrait d'algues rouges.", "Agar", "Agar", "Agar-agar" });

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E407",
                columns: new[] { "DescriptionDe", "DescriptionEs", "DescriptionFr", "NameDe", "NameEs", "NameFr" },
                values: new object[] { "Ein Verdickungsmittel aus Meeresalgen. Einige Studien haben hohe Dosen mit Verdauungsbeschwerden in Verbindung gebracht; die EFSA hält lebensmitteltaugliches Carrageen bei den aktuellen Verwendungsmengen für unbedenklich.", "Un espesante extraído de algas. Algunos estudios han asociado dosis altas con molestias digestivas; la EFSA considera segura la carragenina de calidad alimentaria en los niveles de uso actuales.", "Un épaississant extrait d'algues. Certaines études ont associé des doses élevées à des troubles digestifs ; l'EFSA considère le carraghénane de qualité alimentaire sûr aux niveaux d'utilisation actuels.", "Carrageen", "Carragenina", "Carraghénane" });

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E410",
                columns: new[] { "DescriptionDe", "DescriptionEs", "DescriptionFr", "NameDe", "NameEs", "NameFr" },
                values: new object[] { "Ein natürliches Verdickungsmittel aus Johannisbrotkernen.", "Un espesante natural extraído de semillas de algarrobo.", "Un épaississant naturel extrait des graines de caroube.", "Johannisbrotkernmehl", "Goma garrofín", "Farine de graines de caroube" });

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E412",
                columns: new[] { "DescriptionDe", "DescriptionEs", "DescriptionFr", "NameDe", "NameEs", "NameFr" },
                values: new object[] { "Ein natürliches Verdickungsmittel aus der Guarpflanze.", "Un espesante natural extraído de la planta de guar.", "Un épaississant naturel extrait de la plante de guar.", "Guarkernmehl", "Goma guar", "Gomme de guar" });

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E413",
                columns: new[] { "DescriptionDe", "DescriptionEs", "DescriptionFr", "NameDe", "NameEs", "NameFr" },
                values: new object[] { "Ein Verdickungsmittel aus Pflanzengummi; allergische Reaktionen wurden selten berichtet.", "Un espesante extraído de goma vegetal; se han notificado reacciones alérgicas en raras ocasiones.", "Un épaississant extrait de gomme végétale ; de rares réactions allergiques ont été signalées.", "Tragant", "Goma tragacanto", "Gomme adragante" });

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E414",
                columns: new[] { "DescriptionDe", "DescriptionEs", "DescriptionFr", "NameDe", "NameEs", "NameFr" },
                values: new object[] { "Ein natürliches Verdickungs- und Stabilisierungsmittel aus dem Akazienbaum.", "Un espesante y estabilizante natural extraído de la acacia.", "Un épaississant et stabilisant naturel extrait de l'acacia.", "Akaziengummi (Gummi arabicum)", "Goma arábiga", "Gomme d'acacia (gomme arabique)" });

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E415",
                columns: new[] { "DescriptionDe", "DescriptionEs", "DescriptionFr", "NameDe", "NameEs", "NameFr" },
                values: new object[] { "Ein durch Fermentation gewonnenes natürliches Verdickungsmittel.", "Un espesante natural producido por fermentación.", "Un épaississant naturel produit par fermentation.", "Xanthan", "Goma xantana", "Gomme xanthane" });

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E416",
                columns: new[] { "DescriptionDe", "DescriptionEs", "DescriptionFr", "NameDe", "NameEs", "NameFr" },
                values: new object[] { "Ein Verdickungsmittel aus Pflanzengummi; bei hohen Dosen wurden Verdauungsbeschwerden berichtet.", "Un espesante extraído de goma vegetal; se han notificado molestias digestivas a dosis elevadas.", "Un épaississant extrait de gomme végétale ; des troubles digestifs ont été signalés à fortes doses.", "Karayagummi", "Goma karaya", "Gomme karaya" });

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E417",
                columns: new[] { "DescriptionDe", "DescriptionEs", "DescriptionFr", "NameDe", "NameEs", "NameFr" },
                values: new object[] { "Ein natürliches Verdickungsmittel aus Samen des Tarabaums.", "Un espesante natural extraído de semillas de tara.", "Un épaississant naturel extrait de graines de tara.", "Tarakernmehl", "Goma tara", "Gomme de tara" });

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E418",
                columns: new[] { "DescriptionDe", "DescriptionEs", "DescriptionFr", "NameDe", "NameEs", "NameFr" },
                values: new object[] { "Ein durch Fermentation gewonnenes Verdickungs- und Geliermittel.", "Un espesante y gelificante producido por fermentación.", "Un épaississant et gélifiant produit par fermentation.", "Gellan", "Goma gelán", "Gomme gellane" });

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E420",
                columns: new[] { "DescriptionDe", "DescriptionEs", "DescriptionFr", "NameDe", "NameEs", "NameFr" },
                values: new object[] { "Ein Süßungsmittel aus der Gruppe der Zuckeralkohole. Laut EU-Recht muss auf übermäßigen Verzehr mit abführender Wirkung hingewiesen werden.", "Un edulcorante del grupo de los polialcoholes. La normativa de la UE exige indicar que un consumo excesivo puede tener un efecto laxante.", "Un édulcorant de la famille des polyols. La réglementation européenne impose la mention qu'une consommation excessive peut avoir un effet laxatif.", "Sorbit", "Sorbitol", "Sorbitol" });

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E421",
                columns: new[] { "DescriptionDe", "DescriptionEs", "DescriptionFr", "NameDe", "NameEs", "NameFr" },
                values: new object[] { "Ein Süßungsmittel aus der Gruppe der Zuckeralkohole. Laut EU-Recht muss auf übermäßigen Verzehr mit abführender Wirkung hingewiesen werden.", "Un edulcorante del grupo de los polialcoholes. La normativa de la UE exige indicar que un consumo excesivo puede tener un efecto laxante.", "Un édulcorant de la famille des polyols. La réglementation européenne impose la mention qu'une consommation excessive peut avoir un effet laxatif.", "Mannit", "Manitol", "Mannitol" });

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E422",
                columns: new[] { "DescriptionDe", "DescriptionEs", "DescriptionFr", "NameDe", "NameEs", "NameFr" },
                values: new object[] { "Eine natürlich vorkommende Verbindung, die als Feuchthaltemittel und Lösungsmittel verwendet wird.", "Un compuesto natural utilizado como humectante y disolvente.", "Un composé naturel utilisé comme humectant et solvant.", "Glycerin", "Glicerol", "Glycérol" });

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E433",
                columns: new[] { "DescriptionDe", "DescriptionEs", "DescriptionFr", "NameDe", "NameEs", "NameFr" },
                values: new object[] { "Ein synthetischer Emulgator, der in Speiseeis und Backwaren verwendet wird. Die EFSA hat eine tägliche Aufnahmegrenze festgelegt.", "Un emulgente sintético utilizado en helados y productos de panadería. La EFSA ha establecido un límite de ingesta diaria.", "Un émulsifiant synthétique utilisé dans les glaces et les produits de boulangerie. L'EFSA a fixé une limite d'apport journalier.", "Polysorbat 80", "Polisorbato 80", "Polysorbate 80" });

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E440",
                columns: new[] { "DescriptionDe", "DescriptionEs", "DescriptionFr", "NameDe", "NameEs", "NameFr" },
                values: new object[] { "Ein natürlich in Früchten vorkommendes Verdickungsmittel, das in Konfitüren und Marmeladen als Geliermittel verwendet wird.", "Un espesante presente de forma natural en las frutas, utilizado como gelificante en mermeladas y confituras.", "Un épaississant naturellement présent dans les fruits, utilisé comme gélifiant dans les confitures et marmelades.", "Pektine", "Pectinas", "Pectines" });

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E442",
                columns: new[] { "DescriptionDe", "DescriptionEs", "DescriptionFr", "NameDe", "NameEs", "NameFr" },
                values: new object[] { "Ein Emulgator, der in Kakao- und Schokoladenprodukten verwendet wird.", "Un emulgente utilizado en productos de cacao y chocolate.", "Un émulsifiant utilisé dans les produits à base de cacao et de chocolat.", "Ammoniumphosphatide", "Fosfátidos de amonio", "Phosphatides d'ammonium" });

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E460",
                columns: new[] { "DescriptionDe", "DescriptionEs", "DescriptionFr", "NameDe", "NameEs", "NameFr" },
                values: new object[] { "Eine aus Pflanzenzellwänden gewonnene Faser, die als Verdickungsmittel und Trennmittel verwendet wird.", "Una fibra extraída de las paredes celulares vegetales, utilizada como espesante y antiaglomerante.", "Une fibre extraite des parois cellulaires végétales, utilisée comme épaississant et antiagglomérant.", "Cellulose", "Celulosa", "Cellulose" });

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E461",
                columns: new[] { "DescriptionDe", "DescriptionEs", "DescriptionFr", "NameDe", "NameEs", "NameFr" },
                values: new object[] { "Ein aus Cellulose gewonnenes Verdickungsmittel.", "Un espesante derivado de la celulosa.", "Un épaississant dérivé de la cellulose.", "Methylcellulose", "Metilcelulosa", "Méthylcellulose" });

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E463",
                columns: new[] { "DescriptionDe", "DescriptionEs", "DescriptionFr", "NameDe", "NameEs", "NameFr" },
                values: new object[] { "Ein aus Cellulose gewonnenes Verdickungsmittel.", "Un espesante derivado de la celulosa.", "Un épaississant dérivé de la cellulose.", "Hydroxypropylcellulose", "Hidroxipropilcelulosa", "Hydroxypropylcellulose" });

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E464",
                columns: new[] { "DescriptionDe", "DescriptionEs", "DescriptionFr", "NameDe", "NameEs", "NameFr" },
                values: new object[] { "Ein aus Cellulose gewonnenes Verdickungsmittel, das häufig in glutenfreien Produkten verwendet wird.", "Un espesante derivado de la celulosa, utilizado con frecuencia en productos sin gluten.", "Un épaississant dérivé de la cellulose, souvent utilisé dans les produits sans gluten.", "Hydroxypropylmethylcellulose", "Hidroxipropilmetilcelulosa", "Hydroxypropylméthylcellulose" });

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E466",
                columns: new[] { "DescriptionDe", "DescriptionEs", "DescriptionFr", "NameDe", "NameEs", "NameFr" },
                values: new object[] { "Ein aus Cellulose gewonnenes Verdickungsmittel, das weit verbreitet in Speiseeis und Soßen verwendet wird.", "Un espesante derivado de la celulosa, ampliamente utilizado en helados y salsas.", "Un épaississant dérivé de la cellulose, largement utilisé dans les glaces et les sauces.", "Carboxymethylcellulose (CMC)", "Carboximetilcelulosa (CMC)", "Carboxyméthylcellulose (CMC)" });

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E470a",
                columns: new[] { "DescriptionDe", "DescriptionEs", "DescriptionFr", "NameDe", "NameEs", "NameFr" },
                values: new object[] { "Fettsäuresalze, die als Emulgator und Trennmittel verwendet werden.", "Sales de ácidos grasos utilizadas como emulgente y antiaglomerante.", "Des sels d'acides gras utilisés comme émulsifiant et antiagglomérant.", "Salze von Speisefettsäuren", "Sales de ácidos grasos", "Sels d'acides gras" });

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E471",
                columns: new[] { "DescriptionDe", "DescriptionEs", "DescriptionFr", "NameDe", "NameEs", "NameFr" },
                values: new object[] { "Ein Emulgator, der in Brot, Margarine und vielen verarbeiteten Lebensmitteln weit verbreitet verwendet wird.", "Un emulgente ampliamente utilizado en pan, margarina y muchos alimentos procesados.", "Un émulsifiant largement utilisé dans le pain, la margarine et de nombreux aliments transformés.", "Mono- und Diglyceride von Speisefettsäuren", "Mono- y diglicéridos de ácidos grasos", "Mono- et diglycérides d'acides gras" });

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E472e",
                columns: new[] { "DescriptionDe", "DescriptionEs", "DescriptionFr", "NameDe", "NameEs", "NameFr" },
                values: new object[] { "Ein Emulgator, der in der Bäckerei als Teigverstärker verwendet wird.", "Un emulgente utilizado en panadería como reforzador de masa.", "Un émulsifiant utilisé en boulangerie comme renforçateur de pâte.", "Diacetylweinsäureester von Mono- und Diglyceriden (DATEM)", "Ésteres diacetiltartáricos de mono- y diglicéridos (DATEM)", "Esters diacétyltartriques de mono- et diglycérides (DATEM)" });

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E473",
                columns: new[] { "DescriptionDe", "DescriptionEs", "DescriptionFr", "NameDe", "NameEs", "NameFr" },
                values: new object[] { "Zuckerbasierte Verbindungen, die als Emulgator verwendet werden.", "Compuestos a base de azúcar utilizados como emulgente.", "Des composés à base de sucre utilisés comme émulsifiant.", "Zuckerester von Speisefettsäuren", "Ésteres de sacarosa de ácidos grasos", "Esters de saccharose d'acides gras" });

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E475",
                columns: new[] { "DescriptionDe", "DescriptionEs", "DescriptionFr", "NameDe", "NameEs", "NameFr" },
                values: new object[] { "Wird als Emulgator verwendet.", "Se utilizan como emulgente.", "Utilisés comme émulsifiant.", "Polyglycerinester von Speisefettsäuren", "Ésteres poliglicéricos de ácidos grasos", "Esters polyglycériques d'acides gras" });

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E476",
                columns: new[] { "DescriptionDe", "DescriptionEs", "DescriptionFr", "NameDe", "NameEs", "NameFr" },
                values: new object[] { "Ein Emulgator, der in Schokolade zur Verringerung der Viskosität verwendet wird. Die EFSA hat eine tägliche Aufnahmegrenze festgelegt.", "Un emulgente utilizado en el chocolate para reducir la viscosidad. La EFSA ha establecido un límite de ingesta diaria.", "Un émulsifiant utilisé dans le chocolat pour réduire la viscosité. L'EFSA a fixé une limite d'apport journalier.", "Polyglycerinpolyricinoleat (PGPR)", "Poliricinoleato de poliglicerol (PGPR)", "Polyricinoléate de polyglycérol (PGPR)" });

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E477",
                columns: new[] { "DescriptionDe", "DescriptionEs", "DescriptionFr", "NameDe", "NameEs", "NameFr" },
                values: new object[] { "Ein Emulgator, der in Cremes und Kuchenmischungen verwendet wird.", "Un emulgente utilizado en cremas y preparados para pasteles.", "Un émulsifiant utilisé dans les crèmes et les préparations pour gâteaux.", "Propylenglycolester von Speisefettsäuren", "Ésteres de ácidos grasos de propilenglicol", "Esters d'acides gras de propylène glycol" });

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E481",
                columns: new[] { "DescriptionDe", "DescriptionEs", "DescriptionFr", "NameDe", "NameEs", "NameFr" },
                values: new object[] { "Ein Emulgator, der in der Bäckerei zur Verstärkung des Teigs verwendet wird.", "Un emulgente utilizado en panadería para reforzar la masa.", "Un émulsifiant utilisé en boulangerie pour renforcer la pâte.", "Natriumstearoyllactylat", "Estearoil-2-lactilato de sodio", "Stéaroyl-2-lactylate de sodium" });

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E491",
                columns: new[] { "DescriptionDe", "DescriptionEs", "DescriptionFr", "NameDe", "NameEs", "NameFr" },
                values: new object[] { "Wird als Emulgator verwendet.", "Se utiliza como emulgente.", "Utilisé comme émulsifiant.", "Sorbitanmonostearat", "Monoestearato de sorbitán", "Monostéarate de sorbitanne" });

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E492",
                columns: new[] { "DescriptionDe", "DescriptionEs", "DescriptionFr", "NameDe", "NameEs", "NameFr" },
                values: new object[] { "Wird als Emulgator verwendet.", "Se utiliza como emulgente.", "Utilisé comme émulsifiant.", "Sorbitantristearat", "Triestearato de sorbitán", "Tristéarate de sorbitanne" });

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E495",
                columns: new[] { "DescriptionDe", "DescriptionEs", "DescriptionFr", "NameDe", "NameEs", "NameFr" },
                values: new object[] { "Wird als Emulgator verwendet.", "Se utiliza como emulgente.", "Utilisé comme émulsifiant.", "Sorbitanmonopalmitat", "Monopalmitato de sorbitán", "Monopalmitate de sorbitanne" });

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E500",
                columns: new[] { "DescriptionDe", "DescriptionEs", "DescriptionFr", "NameDe", "NameEs", "NameFr" },
                values: new object[] { "Werden als Backtriebmittel und Säureregulator verwendet; auch in kohlensäurehaltigen Getränken enthalten.", "Se utilizan como gasificante y regulador de acidez; también presentes en bebidas carbonatadas.", "Utilisés comme agent levant et régulateur d'acidité ; également présents dans les boissons gazeuses.", "Natriumcarbonate", "Carbonatos de sodio", "Carbonates de sodium" });

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E501",
                columns: new[] { "DescriptionDe", "DescriptionEs", "DescriptionFr", "NameDe", "NameEs", "NameFr" },
                values: new object[] { "Werden als Säureregulator und Backtriebmittel verwendet.", "Se utilizan como regulador de acidez y gasificante.", "Utilisés comme régulateur d'acidité et agent levant.", "Kaliumcarbonate", "Carbonatos de potasio", "Carbonates de potassium" });

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E503",
                columns: new[] { "DescriptionDe", "DescriptionEs", "DescriptionFr", "NameDe", "NameEs", "NameFr" },
                values: new object[] { "Ein Backtriebmittel, das in traditionellen Keksrezepten verwendet wird.", "Un gasificante utilizado en recetas tradicionales de galletas.", "Un agent levant utilisé dans les recettes traditionnelles de biscuits.", "Ammoniumcarbonate", "Carbonatos de amonio", "Carbonates d'ammonium" });

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E507",
                columns: new[] { "DescriptionDe", "DescriptionEs", "DescriptionFr", "NameDe", "NameEs", "NameFr" },
                values: new object[] { "Ein Verarbeitungshilfsstoff, der in begrenzter Menge als Säureregulator verwendet wird.", "Un coadyuvante tecnológico utilizado en cantidades limitadas como regulador de acidez.", "Un auxiliaire technologique utilisé en quantité limitée comme régulateur d'acidité.", "Salzsäure", "Ácido clorhídrico", "Acide chlorhydrique" });

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E508",
                columns: new[] { "DescriptionDe", "DescriptionEs", "DescriptionFr", "NameDe", "NameEs", "NameFr" },
                values: new object[] { "Ein Mineralsalz, das in Kochsalzersatzprodukten und als Geliermittel verwendet wird.", "Una sal mineral utilizada en sustitutos de la sal de mesa y como gelificante.", "Un sel minéral utilisé dans les substituts de sel de table et comme gélifiant.", "Kaliumchlorid", "Cloruro de potasio", "Chlorure de potassium" });

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E509",
                columns: new[] { "DescriptionDe", "DescriptionEs", "DescriptionFr", "NameDe", "NameEs", "NameFr" },
                values: new object[] { "Ein Mineralsalz, das als Festigungsmittel und Stabilisator verwendet wird.", "Una sal mineral utilizada como endurecedor y estabilizante.", "Un sel minéral utilisé comme affermissant et stabilisant.", "Calciumchlorid", "Cloruro de calcio", "Chlorure de calcium" });

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E511",
                columns: new[] { "DescriptionDe", "DescriptionEs", "DescriptionFr", "NameDe", "NameEs", "NameFr" },
                values: new object[] { "Ein Mineralsalz, das bei der Tofuherstellung als Gerinnungsmittel verwendet wird.", "Una sal mineral utilizada como coagulante en la producción de tofu.", "Un sel minéral utilisé comme coagulant dans la production de tofu.", "Magnesiumchlorid", "Cloruro de magnesio", "Chlorure de magnésium" });

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E551",
                columns: new[] { "DescriptionDe", "DescriptionEs", "DescriptionFr", "NameDe", "NameEs", "NameFr" },
                values: new object[] { "Ein Mineral, das zur Vermeidung von Verklumpung in Pulver- und Granulatlebensmitteln verwendet wird.", "Un mineral utilizado para prevenir la aglomeración en alimentos en polvo y granulados.", "Un minéral utilisé pour prévenir l'agglomération dans les aliments en poudre et en granulés.", "Siliciumdioxid", "Dióxido de silicio", "Dioxyde de silicium" });

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E553b",
                columns: new[] { "DescriptionDe", "DescriptionEs", "DescriptionFr", "NameDe", "NameEs", "NameFr" },
                values: new object[] { "Ein Mineral, das in Kaugummi und Süßwaren als Trennmittel verwendet wird, mit eingeschränkten Verwendungszwecken.", "Un mineral utilizado como antiaglomerante en chicles y confitería, de uso restringido.", "Un minéral utilisé comme antiagglomérant dans les chewing-gums et la confiserie, à usages restreints.", "Talkum", "Talco", "Talc" });

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E574",
                columns: new[] { "DescriptionDe", "DescriptionEs", "DescriptionFr", "NameDe", "NameEs", "NameFr" },
                values: new object[] { "Eine Säure, die als Säureregulator verwendet wird.", "Un ácido utilizado como regulador de acidez.", "Un acide utilisé comme régulateur d'acidité.", "Gluconsäure", "Ácido glucónico", "Acide gluconique" });

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E575",
                columns: new[] { "DescriptionDe", "DescriptionEs", "DescriptionFr", "NameDe", "NameEs", "NameFr" },
                values: new object[] { "Ein Säureregulator, der in Fleischerzeugnissen und bei der Tofuherstellung verwendet wird.", "Un regulador de acidez utilizado en productos cárnicos y en la producción de tofu.", "Un régulateur d'acidité utilisé dans les produits carnés et la production de tofu.", "Glucono-delta-lacton", "Gluconodeltalactona", "Glucono-delta-lactone" });

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E576",
                columns: new[] { "DescriptionDe", "DescriptionEs", "DescriptionFr", "NameDe", "NameEs", "NameFr" },
                values: new object[] { "Ein Salz der Gluconsäure; wird als Komplexbildner verwendet.", "Una sal del ácido glucónico utilizada como secuestrante.", "Un sel de l'acide gluconique utilisé comme séquestrant.", "Natriumgluconat", "Gluconato de sodio", "Gluconate de sodium" });

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E577",
                columns: new[] { "DescriptionDe", "DescriptionEs", "DescriptionFr", "NameDe", "NameEs", "NameFr" },
                values: new object[] { "Ein Salz der Gluconsäure; wird als Säureregulator verwendet.", "Una sal del ácido glucónico utilizada como regulador de acidez.", "Un sel de l'acide gluconique utilisé comme régulateur d'acidité.", "Kaliumgluconat", "Gluconato de potasio", "Gluconate de potassium" });

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E578",
                columns: new[] { "DescriptionDe", "DescriptionEs", "DescriptionFr", "NameDe", "NameEs", "NameFr" },
                values: new object[] { "Ein Salz der Gluconsäure; wird als Festigungsmittel verwendet.", "Una sal del ácido glucónico utilizada como endurecedor.", "Un sel de l'acide gluconique utilisé comme affermissant.", "Calciumgluconat", "Gluconato de calcio", "Gluconate de calcium" });

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E620",
                columns: new[] { "DescriptionDe", "DescriptionEs", "DescriptionFr", "NameDe", "NameEs", "NameFr" },
                values: new object[] { "Eine Aminosäure, die natürlich in vielen Lebensmitteln vorkommt; wird als Geschmacksverstärker verwendet.", "Un aminoácido presente de forma natural en muchos alimentos; se utiliza como potenciador del sabor.", "Un acide aminé naturellement présent dans de nombreux aliments ; utilisé comme exhausteur de goût.", "Glutaminsäure", "Ácido glutámico", "Acide glutamique" });

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E621",
                columns: new[] { "DescriptionDe", "DescriptionEs", "DescriptionFr", "NameDe", "NameEs", "NameFr" },
                values: new object[] { "Ein weit verbreiteter Geschmacksverstärker. Bei manchen Personen wurden vorübergehende Symptome wie Kopfschmerzen berichtet, doch kontrollierte Studien haben diesen Zusammenhang nicht bestätigt; die JECFA hat keine tägliche Aufnahmegrenze festgelegt.", "Un potenciador del sabor muy utilizado. Algunas personas han referido síntomas transitorios como dolor de cabeza, aunque los estudios controlados no han confirmado esta relación; el JECFA no ha establecido un límite de ingesta diaria.", "Un exhausteur de goût largement utilisé. Certaines personnes ont rapporté des symptômes passagers comme des maux de tête, mais des études contrôlées n'ont pas confirmé ce lien ; le JECFA n'a pas fixé de limite d'apport journalier.", "Mononatriumglutamat (MSG)", "Glutamato monosódico (GMS)", "Glutamate monosodique (GMS)" });

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E622",
                columns: new[] { "DescriptionDe", "DescriptionEs", "DescriptionFr", "NameDe", "NameEs", "NameFr" },
                values: new object[] { "Ein Geschmacksverstärker, der ähnlich wie MSG verwendet wird.", "Un potenciador del sabor utilizado de forma similar al GMS.", "Un exhausteur de goût utilisé de manière similaire au GMS.", "Monokaliumglutamat", "Glutamato monopotásico", "Glutamate monopotassique" });

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E623",
                columns: new[] { "DescriptionDe", "DescriptionEs", "DescriptionFr", "NameDe", "NameEs", "NameFr" },
                values: new object[] { "Wird als Geschmacksverstärker verwendet.", "Se utiliza como potenciador del sabor.", "Utilisé comme exhausteur de goût.", "Calciumdiglutamat", "Diglutamato de calcio", "Diglutamate de calcium" });

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E627",
                columns: new[] { "DescriptionDe", "DescriptionEs", "DescriptionFr", "NameDe", "NameEs", "NameFr" },
                values: new object[] { "Ein Geschmacksverstärker, der meist zusammen mit MSG verwendet wird.", "Un potenciador del sabor utilizado a menudo junto con el GMS.", "Un exhausteur de goût souvent utilisé avec le GMS.", "Dinatriumguanylat", "Guanilato disódico", "Guanylate disodique" });

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E631",
                columns: new[] { "DescriptionDe", "DescriptionEs", "DescriptionFr", "NameDe", "NameEs", "NameFr" },
                values: new object[] { "Ein Geschmacksverstärker, der meist zusammen mit MSG verwendet wird.", "Un potenciador del sabor utilizado a menudo junto con el GMS.", "Un exhausteur de goût souvent utilisé avec le GMS.", "Dinatriuminosinat", "Inosinato disódico", "Inosinate disodique" });

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E635",
                columns: new[] { "DescriptionDe", "DescriptionEs", "DescriptionFr", "NameDe", "NameEs", "NameFr" },
                values: new object[] { "Eine Geschmacksverstärkermischung, die meist zusammen mit MSG verwendet wird.", "Una mezcla de potenciadores del sabor utilizada a menudo junto con el GMS.", "Un mélange d'exhausteurs de goût souvent utilisé avec le GMS.", "Dinatrium-5′-ribonukleotide", "Ribonucleótidos disódicos de 5′", "Ribonucléotides disodiques de 5′" });

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E901",
                columns: new[] { "DescriptionDe", "DescriptionEs", "DescriptionFr", "NameDe", "NameEs", "NameFr" },
                values: new object[] { "Ein natürlicher Stoff, der als Überzugsmittel bei Süßwaren und Obst verwendet wird; nicht vegan.", "Una sustancia natural utilizada como agente de recubrimiento en confitería y fruta; no es vegana.", "Une substance naturelle utilisée comme agent d'enrobage sur la confiserie et les fruits ; non végane.", "Bienenwachs", "Cera de abeja", "Cire d'abeille" });

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E903",
                columns: new[] { "DescriptionDe", "DescriptionEs", "DescriptionFr", "NameDe", "NameEs", "NameFr" },
                values: new object[] { "Ein natürliches Überzugsmittel aus der Carnaubapalme.", "Un agente de recubrimiento natural extraído de la palma de carnauba.", "Un agent d'enrobage naturel extrait du palmier carnauba.", "Carnaubawachs", "Cera de carnauba", "Cire de carnauba" });

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E904",
                columns: new[] { "DescriptionDe", "DescriptionEs", "DescriptionFr", "NameDe", "NameEs", "NameFr" },
                values: new object[] { "Ein natürliches Überzugsmittel aus Insektensekret; nicht vegan.", "Un agente de recubrimiento natural obtenido de secreciones de insectos; no es vegana.", "Un agent d'enrobage naturel issu de sécrétions d'insectes ; non végane.", "Schellack", "Goma laca", "Gomme laque" });

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E941",
                columns: new[] { "DescriptionDe", "DescriptionEs", "DescriptionFr", "NameDe", "NameEs", "NameFr" },
                values: new object[] { "Ein Gas, das zur Schaffung einer Schutzatmosphäre in verpackten Lebensmitteln verwendet wird.", "Un gas utilizado para crear una atmósfera protectora en alimentos envasados.", "Un gaz utilisé pour créer une atmosphère protectrice dans les aliments emballés.", "Stickstoff", "Nitrógeno", "Azote" });

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E942",
                columns: new[] { "DescriptionDe", "DescriptionEs", "DescriptionFr", "NameDe", "NameEs", "NameFr" },
                values: new object[] { "Wird als Treibgas in Sahneprodukten verwendet.", "Se utiliza como gas propulsor en productos de nata montada.", "Utilisé comme gaz propulseur dans les produits de crème chantilly.", "Distickstoffmonoxid", "Óxido nitroso", "Protoxyde d'azote" });

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E950",
                columns: new[] { "DescriptionDe", "DescriptionEs", "DescriptionFr", "NameDe", "NameEs", "NameFr" },
                values: new object[] { "Ein kalorienfreies synthetisches Süßungsmittel. Die EFSA hat eine tägliche Aufnahmegrenze festgelegt.", "Un edulcorante sintético sin calorías. La EFSA ha establecido un límite de ingesta diaria.", "Un édulcorant synthétique sans calorie. L'EFSA a fixé une limite d'apport journalier.", "Acesulfam K", "Acesulfamo K", "Acésulfame K" });

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E951",
                columns: new[] { "DescriptionDe", "DescriptionEs", "DescriptionFr", "NameDe", "NameEs", "NameFr" },
                values: new object[] { "Ein kalorienfreies synthetisches Süßungsmittel. 2023 stufte die IARC Aspartam als „möglicherweise krebserregend beim Menschen“ (Gruppe 2B) ein; die JECFA änderte im selben Jahr die bestehende tägliche Aufnahmegrenze nicht. Für Menschen mit Phenylketonurie muss angegeben werden, dass eine Phenylalaninquelle enthalten ist.", "Un edulcorante sintético sin calorías. En 2023 la IARC clasificó el aspartamo como «posiblemente carcinógeno para el ser humano» (grupo 2B), mientras que el JECFA mantuvo ese mismo año el límite de ingesta diaria vigente. El etiquetado debe indicar que contiene una fuente de fenilalanina para las personas con fenilcetonuria.", "Un édulcorant synthétique sans calorie. En 2023, le CIRC a classé l'aspartame comme « peut-être cancérogène pour l'homme » (groupe 2B), tandis que le JECFA a maintenu la même année la limite d'apport journalier existante. L'étiquetage doit indiquer une source de phénylalanine pour les personnes atteintes de phénylcétonurie.", "Aspartam", "Aspartamo", "Aspartame" });

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E952",
                columns: new[] { "DescriptionDe", "DescriptionEs", "DescriptionFr", "NameDe", "NameEs", "NameFr" },
                values: new object[] { "Ein kalorienfreies synthetisches Süßungsmittel. In der Vergangenheit gab es Bedenken aus Tierstudien mit hohen Dosen, doch die EFSA hält es innerhalb der aktuellen ADI für unbedenklich.", "Un edulcorante sintético sin calorías. En el pasado surgieron dudas a partir de estudios en animales con dosis altas, pero la EFSA lo considera seguro dentro de la IDA actual.", "Un édulcorant synthétique sans calorie. Des inquiétudes issues d'études animales à haute dose ont été soulevées par le passé, mais l'EFSA le considère sûr dans la limite de la DJA actuelle.", "Cyclamat", "Ciclamato", "Cyclamate" });

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E954",
                columns: new[] { "DescriptionDe", "DescriptionEs", "DescriptionFr", "NameDe", "NameEs", "NameFr" },
                values: new object[] { "Ein kalorienfreies synthetisches Süßungsmittel. Frühere Bedenken aus Tierstudien wurden in späteren Bewertungen als für den Menschen nicht relevant eingestuft; die EFSA hat eine tägliche Aufnahmegrenze festgelegt.", "Un edulcorante sintético sin calorías. Las dudas planteadas por antiguos estudios en animales se consideraron posteriormente no aplicables al ser humano; la EFSA ha establecido un límite de ingesta diaria.", "Un édulcorant synthétique sans calorie. Les inquiétudes soulevées par d'anciennes études animales ont ensuite été jugées non pertinentes pour l'homme ; l'EFSA a fixé une limite d'apport journalier.", "Saccharin", "Sacarina", "Saccharine" });

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E955",
                columns: new[] { "DescriptionDe", "DescriptionEs", "DescriptionFr", "NameDe", "NameEs", "NameFr" },
                values: new object[] { "Ein aus Zucker hergestelltes kalorienfreies synthetisches Süßungsmittel. Die EFSA hat eine tägliche Aufnahmegrenze festgelegt.", "Un edulcorante sintético sin calorías producido a partir del azúcar. La EFSA ha establecido un límite de ingesta diaria.", "Un édulcorant synthétique sans calorie produit à partir de sucre. L'EFSA a fixé une limite d'apport journalier.", "Sucralose", "Sucralosa", "Sucralose" });

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E960",
                columns: new[] { "DescriptionDe", "DescriptionEs", "DescriptionFr", "NameDe", "NameEs", "NameFr" },
                values: new object[] { "Ein natürliches, kalorienfreies Süßungsmittel aus der Steviapflanze.", "Un edulcorante natural y sin calorías extraído de la planta de estevia.", "Un édulcorant naturel et sans calorie extrait de la plante de stévia.", "Steviolglycoside (Stevia)", "Glucósidos de esteviol (estevia)", "Glycosides de stéviol (stévia)" });

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E961",
                columns: new[] { "DescriptionDe", "DescriptionEs", "DescriptionFr", "NameDe", "NameEs", "NameFr" },
                values: new object[] { "Ein hochintensives synthetisches Süßungsmittel. Die EFSA hat eine tägliche Aufnahmegrenze festgelegt.", "Un edulcorante sintético de alta intensidad. La EFSA ha establecido un límite de ingesta diaria.", "Un édulcorant synthétique de haute intensité. L'EFSA a fixé une limite d'apport journalier.", "Neotam", "Neotamo", "Néotame" });

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E962",
                columns: new[] { "DescriptionDe", "DescriptionEs", "DescriptionFr", "NameDe", "NameEs", "NameFr" },
                values: new object[] { "Ein Süßungsmittel aus einer Kombination von Aspartam und Acesulfam K; die für den Aspartam-Bestandteil geltenden Warnhinweise sind zu beachten.", "Un edulcorante formado por una combinación de aspartamo y acesulfamo K; se aplican las advertencias correspondientes al componente de aspartamo.", "Un édulcorant formé d'une combinaison d'aspartame et d'acésulfame K ; les mises en garde applicables à l'aspartame s'appliquent également.", "Salz aus Aspartam und Acesulfam", "Sal de aspartamo-acesulfamo", "Sel d'aspartame-acésulfame" });

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E965",
                columns: new[] { "DescriptionDe", "DescriptionEs", "DescriptionFr", "NameDe", "NameEs", "NameFr" },
                values: new object[] { "Ein Süßungsmittel aus der Gruppe der Zuckeralkohole. Laut EU-Recht muss auf übermäßigen Verzehr mit abführender Wirkung hingewiesen werden.", "Un edulcorante del grupo de los polialcoholes. La normativa de la UE exige indicar que un consumo excesivo puede tener un efecto laxante.", "Un édulcorant de la famille des polyols. La réglementation européenne impose la mention qu'une consommation excessive peut avoir un effet laxatif.", "Maltit", "Maltitol", "Maltitol" });

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E966",
                columns: new[] { "DescriptionDe", "DescriptionEs", "DescriptionFr", "NameDe", "NameEs", "NameFr" },
                values: new object[] { "Ein Süßungsmittel aus der Gruppe der Zuckeralkohole. Laut EU-Recht muss auf übermäßigen Verzehr mit abführender Wirkung hingewiesen werden.", "Un edulcorante del grupo de los polialcoholes. La normativa de la UE exige indicar que un consumo excesivo puede tener un efecto laxante.", "Un édulcorant de la famille des polyols. La réglementation européenne impose la mention qu'une consommation excessive peut avoir un effet laxatif.", "Lactit", "Lactitol", "Lactitol" });

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E967",
                columns: new[] { "DescriptionDe", "DescriptionEs", "DescriptionFr", "NameDe", "NameEs", "NameFr" },
                values: new object[] { "Ein Süßungsmittel aus der Gruppe der Zuckeralkohole, das häufig in Kaugummi verwendet wird. Laut EU-Recht muss auf übermäßigen Verzehr mit abführender Wirkung hingewiesen werden.", "Un edulcorante del grupo de los polialcoholes, utilizado con frecuencia en chicles. La normativa de la UE exige indicar que un consumo excesivo puede tener un efecto laxante.", "Un édulcorant de la famille des polyols, souvent utilisé dans les chewing-gums. La réglementation européenne impose la mention qu'une consommation excessive peut avoir un effet laxatif.", "Xylit", "Xilitol", "Xylitol" });

            migrationBuilder.UpdateData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E968",
                columns: new[] { "DescriptionDe", "DescriptionEs", "DescriptionFr", "NameDe", "NameEs", "NameFr" },
                values: new object[] { "Ein Süßungsmittel aus der Gruppe der Zuckeralkohole mit im Vergleich zu anderen geringerer Verdauungswirkung.", "Un edulcorante del grupo de los polialcoholes con un efecto digestivo menor que otros de su grupo.", "Un édulcorant de la famille des polyols ayant un effet digestif moindre que les autres.", "Erythrit", "Eritritol", "Érythritol" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DescriptionDe",
                table: "Additives");

            migrationBuilder.DropColumn(
                name: "DescriptionEs",
                table: "Additives");

            migrationBuilder.DropColumn(
                name: "DescriptionFr",
                table: "Additives");

            migrationBuilder.DropColumn(
                name: "NameDe",
                table: "Additives");

            migrationBuilder.DropColumn(
                name: "NameEs",
                table: "Additives");

            migrationBuilder.DropColumn(
                name: "NameFr",
                table: "Additives");
        }
    }
}
