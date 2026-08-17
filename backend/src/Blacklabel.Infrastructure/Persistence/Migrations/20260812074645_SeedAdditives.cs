using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Blacklabel.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class SeedAdditives : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Additives",
                columns: new[] { "Code", "Category", "DescriptionEn", "DescriptionTr", "NameEn", "NameTr", "RiskLevel", "SourceNote" },
                values: new object[,]
                {
                    { "E100", 0, "Natural yellow colorant derived from turmeric.", "Zerdeçaldan elde edilen doğal sarı renklendirici.", "Curcumin", "Kurkumin", 0, "EFSA ADI: belirtilmemiş" },
                    { "E101", 0, "A yellow vitamin naturally present in many foods; also used as a colorant.", "Doğal olarak birçok gıdada bulunan sarı renkli bir vitamin; renklendirici olarak da kullanılır.", "Riboflavin (Vitamin B2)", "Riboflavin (B2 Vitamini)", 0, null },
                    { "E102", 0, "Synthetic azo dye. Under EU regulation, products containing it must carry the warning 'may have an adverse effect on activity and attention in children'.", "Sentetik azo boyar madde. AB mevzuatına göre ürün etiketinde 'çocukların davranış ve dikkat düzeyi üzerinde olumsuz etkileri olabilir' uyarısı bulunması zorunludur.", "Tartrazine", "Tartrazin", 3, "AB Yönetmeliği 1333/2008, Ek V" },
                    { "E104", 0, "Synthetic yellow colorant. One of the six dyes that must carry the EU child behaviour/attention warning label.", "Sentetik sarı renklendirici. AB mevzuatına göre çocuklarda davranış/dikkat uyarısı taşıması zorunlu altı boyardan biridir.", "Quinoline Yellow", "Kinolin Sarısı", 3, "AB Yönetmeliği 1333/2008, Ek V" },
                    { "E110", 0, "Synthetic azo dye. Must carry the EU child behaviour/attention warning label.", "Sentetik azo boyar madde. AB mevzuatına göre çocuklarda davranış/dikkat uyarısı taşıması zorunludur.", "Sunset Yellow FCF", "Gün Batımı Sarısı FCF", 3, "AB Yönetmeliği 1333/2008, Ek V" },
                    { "E120", 0, "Red colorant derived from the cochineal insect. Not vegan; allergic reactions have rarely been reported.", "Koşinil böceğinden elde edilen kırmızı renklendirici. Vegan değildir; nadiren alerjik reaksiyon bildirilmiştir.", "Cochineal / Carminic Acid", "Koşinil / Karminik Asit", 1, null },
                    { "E122", 0, "Synthetic azo dye. Must carry the EU child behaviour/attention warning label.", "Sentetik azo boyar madde. AB mevzuatına göre çocuklarda davranış/dikkat uyarısı taşıması zorunludur.", "Azorubine (Carmoisine)", "Azorubin (Karmoisin)", 3, "AB Yönetmeliği 1333/2008, Ek V" },
                    { "E124", 0, "Synthetic azo dye. Must carry the EU child behaviour/attention warning label.", "Sentetik azo boyar madde. AB mevzuatına göre çocuklarda davranış/dikkat uyarısı taşıması zorunludur.", "Ponceau 4R", "Ponso 4R", 3, "AB Yönetmeliği 1333/2008, Ek V" },
                    { "E127", 0, "Iodine-containing red colorant with restricted permitted uses. Caution is advised for those with thyroid sensitivity.", "İyot içeren kırmızı renklendirici; kullanım alanı sınırlıdır. Tiroid hassasiyeti olanlarda dikkat edilmesi önerilir.", "Erythrosine", "Eritrosin", 2, "EFSA kullanım kısıtlaması mevcuttur" },
                    { "E129", 0, "Synthetic azo dye. Must carry the EU child behaviour/attention warning label.", "Sentetik azo boyar madde. AB mevzuatına göre çocuklarda davranış/dikkat uyarısı taşıması zorunludur.", "Allura Red AC", "Allura Kırmızısı AC", 3, "AB Yönetmeliği 1333/2008, Ek V" },
                    { "E131", 0, "Synthetic blue colorant. Rarely associated with allergic reactions.", "Sentetik mavi renklendirici. Nadiren alerjik reaksiyonlarla ilişkilendirilmiştir.", "Patent Blue V", "Patent Mavisi V", 1, null },
                    { "E132", 0, "Synthetic blue colorant, generally recognized as safe within permitted uses.", "Sentetik mavi renklendirici, genel kullanımda güvenli kabul edilir.", "Indigotine", "İndigotin", 1, null },
                    { "E133", 0, "Synthetic blue colorant, generally recognized as safe within permitted uses.", "Sentetik mavi renklendirici, genel kullanımda güvenli kabul edilir.", "Brilliant Blue FCF", "Parlak Mavi FCF", 1, null },
                    { "E140", 0, "Natural green colorant extracted from plants.", "Bitkilerden elde edilen doğal yeşil renklendirici.", "Chlorophylls", "Klorofiller", 0, null },
                    { "E150a", 0, "Brown colorant produced by heat-caramelizing sugar.", "Şekerin ısıyla karamelize edilmesiyle elde edilen kahverengi renklendirici.", "Plain Caramel", "Sade Karamel", 0, null },
                    { "E150d", 0, "Caramel type commonly used in cola drinks. EFSA has set a daily intake limit for trace 4-MEI that may form during production.", "Kolalı içeceklerde yaygın kullanılan karamel türü. EFSA, üretim sürecinde iz miktarda oluşabilecek 4-MEI bileşiği için günlük alım limiti belirlemiştir.", "Sulphite Ammonia Caramel", "Sülfitli-Amonyaklı Karamel", 1, "EFSA ADI: 4-MEI için 0.2 mg/kg vücut ağırlığı" },
                    { "E160a", 0, "Orange-yellow colorant naturally found in plants such as carrots.", "Havuç gibi bitkilerde doğal olarak bulunan turuncu-sarı renklendirici.", "Carotenes", "Karotenler", 0, null },
                    { "E160c", 0, "Natural orange-red colorant extracted from paprika peppers.", "Kırmızı biberden elde edilen doğal turuncu-kırmızı renklendirici.", "Paprika Extract", "Paprika Ekstraktı", 0, null },
                    { "E162", 0, "Natural red colorant extracted from red beetroot.", "Kırmızı pancardan elde edilen doğal kırmızı renklendirici.", "Beetroot Red (Betanin)", "Pancar Kırmızısı (Betanin)", 0, null },
                    { "E163", 0, "Group of natural red-purple colorants extracted from sources such as grape skin.", "Üzüm kabuğu gibi kaynaklardan elde edilen doğal kırmızı-mor renklendirici grubu.", "Anthocyanins", "Antosiyaninler", 0, null },
                    { "E171", 0, "White colorant. EFSA's 2021 opinion could not rule out a genotoxicity concern, and the EU has prohibited its use in food since 2022.", "Beyaz renklendirici. EFSA'nın 2021 değerlendirmesinde genotoksisite riski dışlanamadığı belirtilmiş ve AB'de 2022 yılından itibaren gıdalarda kullanımı yasaklanmıştır.", "Titanium Dioxide", "Titanyum Dioksit", 2, "EFSA 2021 görüşü; AB Yönetmeliği 2022/63" },
                    { "E172", 0, "Mineral-derived colorant used for brown, red and black tones.", "Kahverengi-kırmızı-siyah tonlarda kullanılan mineral kökenli renklendirici.", "Iron Oxides", "Demir Oksitleri", 0, null },
                    { "E173", 0, "Metallic colorant used for surface coating and decoration, with restricted permitted uses.", "Yüzey kaplama/dekorasyon amaçlı kullanılan metalik renklendirici. Kullanım alanı sınırlıdır.", "Aluminium", "Alüminyum", 1, null },
                    { "E180", 0, "Synthetic red colorant used in some cheese rinds, with restricted permitted uses.", "Bazı peynir kabuklarında kullanılan sentetik kırmızı renklendirici; kullanım alanı sınırlıdır.", "Litholrubine BK", "Litolrubin BK", 1, null },
                    { "E200", 1, "A naturally occurring preservative that inhibits the growth of mould and yeast.", "Küf ve mayaların üremesini engelleyen, doğada da bulunan bir koruyucu.", "Sorbic Acid", "Sorbik Asit", 0, null },
                    { "E202", 1, "A salt of sorbic acid; a widely used preservative against mould and yeast.", "Sorbik asidin tuzu; küf ve maya oluşumunu önlemek için yaygın kullanılan bir koruyucu.", "Potassium Sorbate", "Potasyum Sorbat", 0, null },
                    { "E203", 1, "A salt of sorbic acid used as a preservative.", "Sorbik asidin tuzu; koruyucu olarak kullanılır.", "Calcium Sorbate", "Kalsiyum Sorbat", 0, null },
                    { "E210", 1, "A preservative that inhibits bacterial and fungal growth. EFSA has set a daily intake limit.", "Bakteri ve mantar üremesini engelleyen bir koruyucu. EFSA günlük alım limiti belirlemiştir.", "Benzoic Acid", "Benzoik Asit", 1, "EFSA ADI: 5 mg/kg vücut ağırlığı" },
                    { "E211", 1, "A widely used preservative. Some studies have shown trace benzene can form when combined with vitamin C under heat or light exposure; EFSA has set a daily intake limit.", "Yaygın kullanılan bir koruyucu. C vitamini ile birlikte ve ısı/ışığa maruz kalındığında iz miktarda benzen oluşabileceği bazı çalışmalarda gösterilmiştir; EFSA günlük alım limiti belirlemiştir.", "Sodium Benzoate", "Sodyum Benzoat", 2, "EFSA ADI: 5 mg/kg vücut ağırlığı" },
                    { "E212", 1, "A salt of benzoic acid used as a preservative.", "Benzoik asidin tuzu; koruyucu olarak kullanılır.", "Potassium Benzoate", "Potasyum Benzoat", 1, "EFSA ADI: 5 mg/kg vücut ağırlığı" },
                    { "E220", 1, "A common preservative in dried fruit and wine. Respiratory reactions have been reported in sensitive individuals, particularly people with asthma; mandatory allergen labelling applies in the EU.", "Kuru meyve ve şaraplarda yaygın koruyucu. Hassasiyeti olan kişilerde, özellikle astımlılarda solunum yolu reaksiyonları bildirilmiştir; AB'de zorunlu alerjen etiketlemesi gerektirir.", "Sulphur Dioxide", "Kükürt Dioksit", 2, "AB alerjen listesi" },
                    { "E221", 1, "A sulphite-group preservative. May cause reactions in sensitive individuals; allergen labelling is mandatory.", "Sülfit grubu bir koruyucu. Hassasiyeti olanlarda reaksiyonlara neden olabilir; alerjen etiketlemesi zorunludur.", "Sodium Sulphite", "Sodyum Sülfit", 2, "AB alerjen listesi" },
                    { "E222", 1, "A sulphite-group preservative. May cause reactions in sensitive individuals; allergen labelling is mandatory.", "Sülfit grubu bir koruyucu. Hassasiyeti olanlarda reaksiyonlara neden olabilir; alerjen etiketlemesi zorunludur.", "Sodium Hydrogen Sulphite", "Sodyum Hidrojen Sülfit", 2, "AB alerjen listesi" },
                    { "E223", 1, "A sulphite-group preservative. May cause reactions in sensitive individuals, particularly people with asthma; allergen labelling is mandatory.", "Sülfit grubu bir koruyucu. Hassasiyeti olanlarda, özellikle astımlılarda reaksiyonlara neden olabilir; alerjen etiketlemesi zorunludur.", "Sodium Metabisulphite", "Sodyum Metabisülfit", 2, "AB alerjen listesi" },
                    { "E224", 1, "A sulphite-group preservative. May cause reactions in sensitive individuals; allergen labelling is mandatory.", "Sülfit grubu bir koruyucu. Hassasiyeti olanlarda reaksiyonlara neden olabilir; alerjen etiketlemesi zorunludur.", "Potassium Metabisulphite", "Potasyum Metabisülfit", 2, "AB alerjen listesi" },
                    { "E249", 1, "A preservative used in processed meat products to prevent botulism. EFSA's 2017 review lowered the acceptable daily intake; nitrosamine formation has been examined in some studies.", "İşlenmiş et ürünlerinde botulizmi önlemek için kullanılan bir koruyucu. EFSA 2017 değerlendirmesinde günlük alım limitini düşürmüştür; nitrozamin oluşumu bazı çalışmalarda incelenmiştir.", "Potassium Nitrite", "Potasyum Nitrit", 2, "EFSA 2017 değerlendirmesi" },
                    { "E250", 1, "A preservative used in processed meats (sausage, salami) to prevent botulism. EFSA's 2017 review lowered the acceptable daily intake; nitrosamine formation has been examined in some studies.", "İşlenmiş et ürünlerinde (sucuk, salam, sosis) botulizmi önlemek için kullanılan bir koruyucu. EFSA 2017 değerlendirmesinde günlük alım limitini düşürmüştür; nitrozamin oluşumu bazı çalışmalarda incelenmiştir.", "Sodium Nitrite", "Sodyum Nitrit", 2, "EFSA 2017 değerlendirmesi" },
                    { "E251", 1, "A preservative used in processed meats and some cheeses. EFSA has set a daily intake limit.", "İşlenmiş et ve bazı peynirlerde kullanılan bir koruyucu. EFSA günlük alım limiti belirlemiştir.", "Sodium Nitrate", "Sodyum Nitrat", 2, "EFSA 2017 değerlendirmesi" },
                    { "E252", 1, "A preservative used in processed meat products. EFSA has set a daily intake limit.", "İşlenmiş et ürünlerinde kullanılan bir koruyucu. EFSA günlük alım limiti belirlemiştir.", "Potassium Nitrate", "Potasyum Nitrat", 2, "EFSA 2017 değerlendirmesi" },
                    { "E260", 1, "The main component of vinegar; used as a preservative and acidity regulator.", "Sirkenin ana bileşeni; koruyucu ve asitlik düzenleyici olarak kullanılır.", "Acetic Acid", "Asetik Asit", 0, null },
                    { "E261", 1, "A salt of acetic acid used as a preservative and acidity regulator.", "Asetik asidin tuzu; koruyucu ve asitlik düzenleyici olarak kullanılır.", "Potassium Acetate", "Potasyum Asetat", 0, null },
                    { "E262", 1, "Salts of acetic acid used as preservatives and acidity regulators.", "Asetik asidin tuzları; koruyucu ve asitlik düzenleyici olarak kullanılır.", "Sodium Acetates", "Sodyum Asetatlar", 0, null },
                    { "E263", 1, "A salt of acetic acid used as a preservative.", "Asetik asidin tuzu; koruyucu olarak kullanılır.", "Calcium Acetate", "Kalsiyum Asetat", 0, null },
                    { "E270", 1, "An acid naturally formed in fermented foods and also added as a preservative/acidity regulator.", "Fermente gıdalarda doğal olarak oluşan ve koruyucu/asitlik düzenleyici olarak eklenen bir asit.", "Lactic Acid", "Laktik Asit", 0, null },
                    { "E280", 1, "A preservative used in bread and baked goods to inhibit mould growth.", "Ekmek ve unlu mamullerde küf oluşumunu önlemek için kullanılan bir koruyucu.", "Propionic Acid", "Propiyonik Asit", 0, null },
                    { "E281", 1, "A preservative used in bread and baked goods to inhibit mould growth.", "Ekmek ve unlu mamullerde küf oluşumunu önlemek için kullanılan bir koruyucu.", "Sodium Propionate", "Sodyum Propiyonat", 0, null },
                    { "E282", 1, "A preservative used in bread and baked goods to inhibit mould growth.", "Ekmek ve unlu mamullerde küf oluşumunu önlemek için kullanılan bir koruyucu.", "Calcium Propionate", "Kalsiyum Propiyonat", 0, null },
                    { "E283", 1, "A preservative used in bread and baked goods to inhibit mould growth.", "Ekmek ve unlu mamullerde küf oluşumunu önlemek için kullanılan bir koruyucu.", "Potassium Propionate", "Potasyum Propiyonat", 0, null },
                    { "E296", 7, "An acid naturally found in fruit; used as an acidity regulator.", "Meyvelerde doğal olarak bulunan bir asit; asitlik düzenleyici olarak kullanılır.", "Malic Acid", "Malik Asit", 0, null },
                    { "E297", 7, "An acid used as an acidity regulator.", "Asitlik düzenleyici olarak kullanılan bir asit.", "Fumaric Acid", "Fumarik Asit", 0, null },
                    { "E300", 2, "Vitamin C; used as an antioxidant to slow oxidation and colour degradation.", "C vitamini; antioksidan olarak oksidasyonu ve renk bozulmasını yavaşlatır.", "Ascorbic Acid (Vitamin C)", "Askorbik Asit (C Vitamini)", 0, null },
                    { "E301", 2, "A salt of vitamin C used as an antioxidant.", "C vitamininin tuzu; antioksidan olarak kullanılır.", "Sodium Ascorbate", "Sodyum Askorbat", 0, null },
                    { "E306", 2, "Naturally sourced vitamin E compounds; antioxidants that slow the oxidation of fats.", "Doğal kaynaklı E vitamini bileşikleri; yağların oksidasyonunu yavaşlatan antioksidanlardır.", "Tocopherols (Vitamin E)", "Tokoferoller (E Vitamini)", 0, null },
                    { "E307", 2, "A synthetic form of vitamin E used as an antioxidant.", "Sentetik E vitamini formu; antioksidan olarak kullanılır.", "Alpha-Tocopherol", "Alfa-Tokoferol", 0, null },
                    { "E310", 2, "An antioxidant that prevents oxidation in fats and fat-containing foods. EFSA has set a daily intake limit.", "Yağ ve yağ içeren gıdalarda oksidasyonu önleyen bir antioksidan. EFSA günlük alım limiti belirlemiştir.", "Propyl Gallate", "Propil Gallat", 1, "EFSA ADI: 0.5 mg/kg vücut ağırlığı" },
                    { "E319", 2, "A synthetic antioxidant that delays fat rancidity. EFSA has set a daily intake limit; some animal studies on high-dose effects remain debated.", "Yağların bozulmasını geciktiren sentetik bir antioksidan. EFSA günlük alım limiti belirlemiştir; yüksek dozlarla ilgili bazı hayvan çalışmaları tartışmalıdır.", "TBHQ (Tertiary Butylhydroquinone)", "TBHQ (Tersiyer Bütilhidrokinon)", 2, "EFSA ADI: 0.7 mg/kg vücut ağırlığı" },
                    { "E320", 2, "A synthetic antioxidant. Classified by IARC as 'possibly carcinogenic to humans' (Group 2B); EFSA considers it safe within its set daily intake limit at current use levels.", "Sentetik bir antioksidan. IARC tarafından 'insanlar için olası kanserojen' (Grup 2B) olarak sınıflandırılmıştır; EFSA mevcut kullanım seviyelerinde günlük alım limiti dahilinde güvenli bulmuştur.", "BHA (Butylated Hydroxyanisole)", "BHA (Bütillenmiş Hidroksianisol)", 2, "IARC Monografları; EFSA ADI: 1 mg/kg vücut ağırlığı" },
                    { "E321", 2, "A synthetic antioxidant. EFSA has set a daily intake limit; endocrine effects have been discussed in some studies.", "Sentetik bir antioksidan. EFSA günlük alım limiti belirlemiştir; bazı çalışmalarda endokrin etkileri tartışılmıştır.", "BHT (Butylated Hydroxytoluene)", "BHT (Bütillenmiş Hidroksitoluen)", 2, "EFSA ADI: 0.25 mg/kg vücut ağırlığı" },
                    { "E322", 3, "A natural emulsifier commonly derived from soy or sunflower.", "Genellikle soya veya ayçiçeğinden elde edilen doğal bir emülgatör.", "Lecithin", "Lesitin", 0, null },
                    { "E325", 7, "A salt of lactic acid used as an acidity regulator and humectant.", "Laktik asidin tuzu; asitlik düzenleyici ve nem tutucu olarak kullanılır.", "Sodium Lactate", "Sodyum Laktat", 0, null },
                    { "E330", 7, "An acid naturally found in citrus fruit, widely used as an acidity regulator in food.", "Turunçgillerde doğal olarak bulunan, gıdalarda yaygın kullanılan bir asitlik düzenleyici.", "Citric Acid", "Sitrik Asit", 0, null },
                    { "E331", 7, "Salts of citric acid used as an acidity regulator.", "Sitrik asidin tuzları; asitlik düzenleyici olarak kullanılır.", "Sodium Citrates", "Sodyum Sitratlar", 0, null },
                    { "E332", 7, "Salts of citric acid used as an acidity regulator.", "Sitrik asidin tuzları; asitlik düzenleyici olarak kullanılır.", "Potassium Citrates", "Potasyum Sitratlar", 0, null },
                    { "E333", 7, "Salts of citric acid used as an acidity regulator.", "Sitrik asidin tuzları; asitlik düzenleyici olarak kullanılır.", "Calcium Citrates", "Kalsiyum Sitratlar", 0, null },
                    { "E334", 7, "An acid naturally found in grapes; used as an acidity regulator.", "Üzümde doğal olarak bulunan bir asit; asitlik düzenleyici olarak kullanılır.", "Tartaric Acid", "Tartarik Asit", 0, null },
                    { "E335", 7, "Salts of tartaric acid used as an acidity regulator.", "Tartarik asidin tuzları; asitlik düzenleyici olarak kullanılır.", "Sodium Tartrates", "Sodyum Tartaratlar", 0, null },
                    { "E336", 7, "A salt that occurs naturally in wine, also used in baking as a raising-agent support.", "Şaraplarda doğal olarak oluşan, pasta yapımında kabartıcı destekleyici olarak da kullanılan bir tuz.", "Potassium Tartrates (Cream of Tartar)", "Potasyum Tartaratlar (Krem Tartar)", 0, null },
                    { "E338", 7, "An acidity regulator widely used in cola drinks. High total phosphate intake has been linked to bone and cardiovascular health in some studies; EFSA has set a daily intake limit.", "Kolalı içeceklerde yaygın kullanılan bir asitlik düzenleyici. Yüksek toplam fosfat alımı bazı çalışmalarda kemik ve kardiyovasküler sağlıkla ilişkilendirilmiştir; EFSA günlük alım limiti belirlemiştir.", "Phosphoric Acid", "Fosforik Asit", 2, "EFSA ADI: 40 mg/kg vücut ağırlığı (toplam fosfat)" },
                    { "E339", 7, "Acidity regulator/emulsifier salts used in processed meat and cheese. EFSA has set a daily intake limit for total phosphate intake.", "İşlenmiş et ve peynirlerde kullanılan asitlik düzenleyici/emülgatör tuzları. EFSA toplam fosfat alımı için günlük limit belirlemiştir.", "Sodium Phosphates", "Sodyum Fosfatlar", 2, "EFSA ADI: 40 mg/kg vücut ağırlığı (toplam fosfat)" },
                    { "E340", 7, "Acidity regulator salts used in processed foods. EFSA has set a daily intake limit for total phosphate intake.", "İşlenmiş gıdalarda kullanılan asitlik düzenleyici tuzları. EFSA toplam fosfat alımı için günlük limit belirlemiştir.", "Potassium Phosphates", "Potasyum Fosfatlar", 2, "EFSA ADI: 40 mg/kg vücut ağırlığı (toplam fosfat)" },
                    { "E341", 7, "Phosphate salts used as raising agents and mineral fortification.", "Kabartıcı ve mineral takviyesi amacıyla kullanılan fosfat tuzları.", "Calcium Phosphates", "Kalsiyum Fosfatlar", 1, null },
                    { "E343", 7, "Phosphate salts used as an acidity regulator and anti-caking agent.", "Asitlik düzenleyici ve topaklanma önleyici olarak kullanılan fosfat tuzları.", "Magnesium Phosphates", "Magnezyum Fosfatlar", 1, null },
                    { "E350", 7, "Salts of malic acid used as an acidity regulator.", "Malik asidin tuzları; asitlik düzenleyici olarak kullanılır.", "Sodium Malates", "Sodyum Malatlar", 0, null },
                    { "E355", 7, "An acid used as an acidity regulator.", "Asitlik düzenleyici olarak kullanılan bir asit.", "Adipic Acid", "Adipik Asit", 0, null },
                    { "E363", 7, "An acid used as an acidity regulator.", "Asitlik düzenleyici olarak kullanılan bir asit.", "Succinic Acid", "Süksinik Asit", 0, null },
                    { "E385", 1, "A preservative that binds metal ions to slow colour and flavour degradation.", "Metal iyonlarını bağlayarak renk ve tat bozulmasını yavaşlatan bir koruyucu.", "Calcium Disodium EDTA", "Kalsiyum Disodyum EDTA", 1, null },
                    { "E400", 6, "A natural thickener extracted from brown seaweed.", "Kahverengi deniz yosunlarından elde edilen doğal bir kıvam arttırıcı.", "Alginic Acid", "Aljinik Asit", 0, null },
                    { "E401", 6, "A thickener and gelling agent derived from seaweed.", "Deniz yosunundan elde edilen bir kıvam arttırıcı ve jelleştirici.", "Sodium Alginate", "Sodyum Aljinat", 0, null },
                    { "E402", 6, "A thickener derived from seaweed.", "Deniz yosunundan elde edilen bir kıvam arttırıcı.", "Potassium Alginate", "Potasyum Aljinat", 0, null },
                    { "E404", 6, "A thickener and gelling agent derived from seaweed.", "Deniz yosunundan elde edilen bir kıvam arttırıcı ve jelleştirici.", "Calcium Alginate", "Kalsiyum Aljinat", 0, null },
                    { "E406", 6, "A natural gelling agent extracted from red seaweed.", "Kırmızı deniz yosunlarından elde edilen doğal bir jelleştirici.", "Agar", "Agar", 0, null },
                    { "E407", 6, "A thickener derived from seaweed. Some studies have associated high doses with digestive discomfort; EFSA considers food-grade carrageenan safe at current use levels.", "Deniz yosunundan elde edilen bir kıvam arttırıcı. Bazı çalışmalarda yüksek dozda sindirim sistemi rahatsızlığıyla ilişkilendirilmiştir; gıda sınıfı karragenan için EFSA mevcut kullanım seviyelerini güvenli bulmuştur.", "Carrageenan", "Karragenan", 1, "EFSA değerlendirmesi" },
                    { "E410", 6, "A natural thickener derived from carob seeds.", "Keçiboynuzu tohumundan elde edilen doğal bir kıvam arttırıcı.", "Locust Bean Gum", "Keçiboynuzu Gamı", 0, null },
                    { "E412", 6, "A natural thickener derived from the guar plant.", "Guar bitkisinden elde edilen doğal bir kıvam arttırıcı.", "Guar Gum", "Guar Gamı", 0, null },
                    { "E413", 6, "A thickener derived from plant gum; allergic reactions have rarely been reported.", "Bitkisel sakızdan elde edilen bir kıvam arttırıcı; nadiren alerjik reaksiyon bildirilmiştir.", "Tragacanth", "Tragakant", 1, null },
                    { "E414", 6, "A natural thickener and stabilizer derived from the acacia tree.", "Akasya ağacından elde edilen doğal bir kıvam arttırıcı ve stabilizatör.", "Acacia Gum (Gum Arabic)", "Arap Gamı (Gum Arabic)", 0, null },
                    { "E415", 6, "A natural thickener produced through fermentation.", "Fermantasyonla üretilen doğal bir kıvam arttırıcı.", "Xanthan Gum", "Ksantan Gamı", 0, null },
                    { "E416", 6, "A thickener derived from plant gum; digestive discomfort has been reported at high doses.", "Bitkisel sakızdan elde edilen bir kıvam arttırıcı; yüksek dozda sindirim rahatsızlığı bildirilmiştir.", "Karaya Gum", "Karaya Gamı", 1, null },
                    { "E417", 6, "A natural thickener derived from tara tree seeds.", "Tara ağacı tohumundan elde edilen doğal bir kıvam arttırıcı.", "Tara Gum", "Tara Gamı", 0, null },
                    { "E418", 6, "A thickener and gelling agent produced through fermentation.", "Fermantasyonla üretilen bir kıvam arttırıcı ve jelleştirici.", "Gellan Gum", "Gellan Gamı", 0, null },
                    { "E420", 4, "A sugar alcohol sweetener. EU regulation requires labelling that excessive consumption may have a laxative effect.", "Şeker alkolü grubunda bir tatlandırıcı. AB mevzuatına göre aşırı tüketiminin laksatif etki yapabileceği etikette belirtilmelidir.", "Sorbitol", "Sorbitol", 1, "AB Yönetmeliği 1333/2008" },
                    { "E421", 4, "A sugar alcohol sweetener. EU regulation requires labelling that excessive consumption may have a laxative effect.", "Şeker alkolü grubunda bir tatlandırıcı. AB mevzuatına göre aşırı tüketiminin laksatif etki yapabileceği etikette belirtilmelidir.", "Mannitol", "Mannitol", 1, "AB Yönetmeliği 1333/2008" },
                    { "E422", 8, "A naturally occurring compound used as a humectant and solvent.", "Nem tutucu ve çözücü olarak kullanılan doğal bir bileşik.", "Glycerol", "Gliserol", 0, null },
                    { "E433", 3, "A synthetic emulsifier used in ice cream and baked goods. EFSA has set a daily intake limit.", "Dondurma ve unlu mamullerde kullanılan sentetik bir emülgatör. EFSA günlük alım limiti belirlemiştir.", "Polysorbate 80", "Polisorbat 80", 1, "EFSA ADI: 25 mg/kg vücut ağırlığı" },
                    { "E440", 6, "A thickener naturally found in fruit, used as a gelling agent in jams and marmalades.", "Meyvelerde doğal olarak bulunan, reçel ve marmelatlarda jelleştirici olarak kullanılan bir kıvam arttırıcı.", "Pectins", "Pektin", 0, null },
                    { "E442", 3, "An emulsifier used in cocoa and chocolate products.", "Kakao ve çikolata ürünlerinde kullanılan bir emülgatör.", "Ammonium Phosphatides", "Amonyum Fosfatidler", 0, null },
                    { "E460", 6, "A fibre derived from plant cell walls, used as a thickener and anti-caking agent.", "Bitki hücre duvarından elde edilen, kıvam arttırıcı ve topaklanma önleyici olarak kullanılan lif.", "Cellulose", "Selüloz", 0, null },
                    { "E461", 6, "A thickener derived from cellulose.", "Selülozdan elde edilen bir kıvam arttırıcı.", "Methyl Cellulose", "Metil Selüloz", 0, null },
                    { "E463", 6, "A thickener derived from cellulose.", "Selülozdan elde edilen bir kıvam arttırıcı.", "Hydroxypropyl Cellulose", "Hidroksipropil Selüloz", 0, null },
                    { "E464", 6, "A thickener derived from cellulose, often used in gluten-free products.", "Selülozdan elde edilen bir kıvam arttırıcı, sıkça glütensiz ürünlerde kullanılır.", "Hydroxypropyl Methyl Cellulose", "Hidroksipropil Metil Selüloz", 0, null },
                    { "E466", 6, "A cellulose-derived thickener widely used in ice cream and sauces.", "Selülozdan elde edilen, dondurma ve soslarda yaygın kullanılan bir kıvam arttırıcı.", "Carboxymethyl Cellulose (CMC)", "Karboksimetil Selüloz (CMC)", 0, null },
                    { "E470a", 3, "Fatty acid salts used as an emulsifier and anti-caking agent.", "Emülgatör ve topaklanma önleyici olarak kullanılan yağ asidi tuzları.", "Salts of Fatty Acids", "Yağ Asitlerinin Tuzları", 0, null },
                    { "E471", 3, "An emulsifier widely used in bread, margarine and many processed foods.", "Ekmek, margarin ve birçok işlenmiş gıdada yaygın kullanılan bir emülgatör.", "Mono- and Diglycerides of Fatty Acids", "Yağ Asitlerinin Mono- ve Digliseritleri", 0, null },
                    { "E472e", 3, "An emulsifier used in baking as a dough strengthener.", "Ekmekçilikte hamur güçlendirici olarak kullanılan bir emülgatör.", "DATEM", "Yağ Asitlerinin Mono- ve Digliseritlerinin Diasetil Tartarik Asit Esterleri (DATEM)", 0, null },
                    { "E473", 3, "Sugar-based compounds used as an emulsifier.", "Emülgatör olarak kullanılan şeker bazlı bileşikler.", "Sucrose Esters of Fatty Acids", "Sükroz Yağ Asidi Esterleri", 0, null },
                    { "E475", 3, "Used as an emulsifier.", "Emülgatör olarak kullanılır.", "Polyglycerol Esters of Fatty Acids", "Yağ Asitlerinin Poligliserol Esterleri", 0, null },
                    { "E476", 3, "An emulsifier used in chocolate to reduce viscosity. EFSA has set a daily intake limit.", "Çikolatada viskoziteyi azaltmak için kullanılan bir emülgatör. EFSA günlük alım limiti belirlemiştir.", "Polyglycerol Polyricinoleate (PGPR)", "Poligliserol Poliricinoleat (PGPR)", 1, "EFSA ADI: 7.5 mg/kg vücut ağırlığı" },
                    { "E477", 3, "An emulsifier used in creams and cake mixes.", "Krema ve pasta karışımlarında kullanılan bir emülgatör.", "Propylene Glycol Esters of Fatty Acids", "Propilen Glikol Yağ Asidi Esterleri", 1, null },
                    { "E481", 3, "An emulsifier used in baking to strengthen dough.", "Ekmekçilikte hamuru güçlendirmek için kullanılan bir emülgatör.", "Sodium Stearoyl Lactylate", "Sodyum Stearoil Laktilat", 0, null },
                    { "E491", 3, "Used as an emulsifier.", "Emülgatör olarak kullanılır.", "Sorbitan Monostearate", "Sorbitan Monostearat", 0, null },
                    { "E492", 3, "Used as an emulsifier.", "Emülgatör olarak kullanılır.", "Sorbitan Tristearate", "Sorbitan Tristearat", 0, null },
                    { "E495", 3, "Used as an emulsifier.", "Emülgatör olarak kullanılır.", "Sorbitan Monopalmitate", "Sorbitan Monopalmitat", 0, null },
                    { "E500", 7, "Used as a raising agent and acidity regulator; also present in carbonated drinks.", "Kabartıcı ve asitlik düzenleyici olarak kullanılır; karbonatlı içeceklerde de bulunur.", "Sodium Carbonates", "Sodyum Karbonatlar", 0, null },
                    { "E501", 7, "Used as an acidity regulator and raising agent.", "Asitlik düzenleyici ve kabartıcı olarak kullanılır.", "Potassium Carbonates", "Potasyum Karbonatlar", 0, null },
                    { "E503", 7, "A raising agent used in traditional biscuit recipes.", "Geleneksel bisküvi tariflerinde kullanılan bir kabartıcı.", "Ammonium Carbonates", "Amonyum Karbonatlar", 0, null },
                    { "E507", 7, "A processing aid used in limited amounts as an acidity regulator.", "Sınırlı miktarda asitlik düzenleyici olarak kullanılan bir işlem yardımcı maddesi.", "Hydrochloric Acid", "Hidroklorik Asit", 0, null },
                    { "E508", 7, "A mineral salt used in salt-substitute products and as a gelling agent.", "Sofra tuzu yerine geçen ürünlerde ve kıvam arttırıcı olarak kullanılan bir mineral tuz.", "Potassium Chloride", "Potasyum Klorür", 0, null },
                    { "E509", 7, "A mineral salt used as a firming agent and stabilizer.", "Sertleştirici ve stabilizatör olarak kullanılan bir mineral tuz.", "Calcium Chloride", "Kalsiyum Klorür", 0, null },
                    { "E511", 7, "A mineral salt used as a coagulant in tofu production.", "Tofu üretiminde pıhtılaştırıcı olarak kullanılan bir mineral tuz.", "Magnesium Chloride", "Magnezyum Klorür", 0, null },
                    { "E551", 8, "A mineral used to prevent caking in powdered and granulated foods.", "Toz ve granül gıdalarda topaklanmayı önlemek için kullanılan bir mineral.", "Silicon Dioxide", "Silikon Dioksit", 0, null },
                    { "E553b", 8, "A mineral used as an anti-caking agent in chewing gum and confectionery, with restricted permitted uses.", "Sakız ve şekerlemelerde topaklanma önleyici olarak kullanılan bir mineral; kullanım alanı sınırlıdır.", "Talc", "Talk", 1, null },
                    { "E574", 7, "An acid used as an acidity regulator.", "Asitlik düzenleyici olarak kullanılan bir asit.", "Gluconic Acid", "Glukonik Asit", 0, null },
                    { "E575", 7, "An acidity regulator used in meat products and tofu production.", "Et ürünlerinde ve tofu üretiminde kullanılan bir asitlik düzenleyici.", "Glucono Delta-Lactone", "Glukono Delta-Lakton", 0, null },
                    { "E576", 7, "A salt of gluconic acid used as a sequestrant.", "Glukonik asidin tuzu; sekestran olarak kullanılır.", "Sodium Gluconate", "Sodyum Glukonat", 0, null },
                    { "E577", 7, "A salt of gluconic acid used as an acidity regulator.", "Glukonik asidin tuzu; asitlik düzenleyici olarak kullanılır.", "Potassium Gluconate", "Potasyum Glukonat", 0, null },
                    { "E578", 7, "A salt of gluconic acid used as a firming agent.", "Glukonik asidin tuzu; sertleştirici olarak kullanılır.", "Calcium Gluconate", "Kalsiyum Glukonat", 0, null },
                    { "E620", 5, "An amino acid naturally present in many foods; used as a flavour enhancer.", "Doğal olarak birçok gıdada bulunan bir amino asit; lezzet arttırıcı olarak kullanılır.", "Glutamic Acid", "Glutamik Asit", 1, null },
                    { "E621", 5, "A widely used flavour enhancer. Some individuals have reported transient symptoms such as headache, though controlled studies have not confirmed the link; JECFA has not specified a daily intake limit.", "Yaygın kullanılan bir lezzet arttırıcı. Bazı bireylerde geçici baş ağrısı gibi semptomlar bildirilmiştir, ancak kontrollü çalışmalar bu ilişkiyi doğrulamamıştır; JECFA günlük alım sınırı belirtmemiştir.", "Monosodium Glutamate (MSG)", "Monosodyum Glutamat (MSG)", 1, "JECFA ADI: belirtilmemiş" },
                    { "E622", 5, "A flavour enhancer used similarly to MSG.", "MSG'ye benzer şekilde kullanılan bir lezzet arttırıcı.", "Monopotassium Glutamate", "Monopotasyum Glutamat", 1, null },
                    { "E623", 5, "Used as a flavour enhancer.", "Lezzet arttırıcı olarak kullanılır.", "Calcium Diglutamate", "Kalsiyum Diglutamat", 1, null },
                    { "E627", 5, "A flavour enhancer often used alongside MSG.", "Genellikle MSG ile birlikte kullanılan bir lezzet arttırıcı.", "Disodium Guanylate", "Disodyum Guanilat", 1, null },
                    { "E631", 5, "A flavour enhancer often used alongside MSG.", "Genellikle MSG ile birlikte kullanılan bir lezzet arttırıcı.", "Disodium Inosinate", "Disodyum İnosinat", 1, null },
                    { "E635", 5, "A flavour enhancer blend often used alongside MSG.", "Genellikle MSG ile birlikte kullanılan bir lezzet arttırıcı karışımı.", "Disodium 5'-Ribonucleotides", "Disodyum 5'-Ribonükleotidler", 1, null },
                    { "E901", 8, "A natural substance used as a glazing agent on confectionery and fruit; not vegan.", "Şekerleme ve meyvelerde parlatıcı olarak kullanılan doğal bir madde; vegan değildir.", "Beeswax", "Arı Balmumu", 0, null },
                    { "E903", 8, "A natural glazing agent derived from the carnauba palm.", "Karnauba palmiyesinden elde edilen doğal bir parlatıcı.", "Carnauba Wax", "Karnauba Balmumu", 0, null },
                    { "E904", 8, "A natural glazing agent derived from insect secretions; not vegan.", "Böcek salgısından elde edilen doğal bir parlatıcı; vegan değildir.", "Shellac", "Şellak", 1, null },
                    { "E941", 8, "A gas used to create a protective atmosphere in packaged food.", "Ambalajlı gıdalarda koruyucu atmosfer sağlamak için kullanılan bir gaz.", "Nitrogen", "Azot", 0, null },
                    { "E942", 8, "Used as a propellant gas in whipped cream products.", "Krem şantilerde itici gaz olarak kullanılır.", "Nitrous Oxide", "Diazot Monoksit", 1, null },
                    { "E950", 4, "A calorie-free synthetic sweetener. EFSA has set a daily intake limit.", "Kalorisiz sentetik bir tatlandırıcı. EFSA günlük alım limiti belirlemiştir.", "Acesulfame K", "Asesülfam K", 1, "EFSA ADI: 9 mg/kg vücut ağırlığı" },
                    { "E951", 4, "A calorie-free synthetic sweetener. In 2023 IARC classified aspartame as 'possibly carcinogenic to humans' (Group 2B), while JECFA maintained the existing daily intake limit the same year. Labelling must state it contains a source of phenylalanine for people with phenylketonuria.", "Kalorisiz sentetik bir tatlandırıcı. 2023 yılında IARC aspartamı 'insanlar için olası kanserojen' (Grup 2B) olarak sınıflandırmış, aynı yıl JECFA mevcut günlük alım limitini değiştirmemiştir. Fenilketonürisi olan bireyler için fenilalanin kaynağı içerdiği etikette belirtilmelidir.", "Aspartame", "Aspartam", 2, "IARC 2023; JECFA ADI: 40 mg/kg vücut ağırlığı" },
                    { "E952", 4, "A calorie-free synthetic sweetener. Concerns about high-dose animal studies arose historically, but EFSA considers it safe within its current ADI.", "Kalorisiz sentetik bir tatlandırıcı. Geçmişte hayvan çalışmalarında yüksek dozlarla ilgili endişeler gündeme gelmiş, ancak EFSA mevcut ADI dahilinde güvenli bulmuştur.", "Cyclamate", "Siklamat", 1, "EFSA ADI: 7 mg/kg vücut ağırlığı" },
                    { "E954", 4, "A calorie-free synthetic sweetener. Concerns raised by historical animal studies were later found not applicable to humans; EFSA has set a daily intake limit.", "Kalorisiz sentetik bir tatlandırıcı. Geçmişte hayvan çalışmalarında endişeler gündeme gelmiş, sonraki değerlendirmelerde insanlar için geçerli bulunmamıştır; EFSA günlük alım limiti belirlemiştir.", "Saccharin", "Sakarin", 1, "EFSA ADI: 5 mg/kg vücut ağırlığı" },
                    { "E955", 4, "A calorie-free synthetic sweetener produced from sugar. EFSA has set a daily intake limit.", "Şekerden üretilen kalorisiz sentetik bir tatlandırıcı. EFSA günlük alım limiti belirlemiştir.", "Sucralose", "Sükraloz", 1, "EFSA ADI: 15 mg/kg vücut ağırlığı" },
                    { "E960", 4, "A natural, calorie-free sweetener extracted from the stevia plant.", "Stevia bitkisinden elde edilen doğal, kalorisiz bir tatlandırıcı.", "Steviol Glycosides (Stevia)", "Steviol Glikozitleri (Stevia)", 0, null },
                    { "E961", 4, "A high-intensity synthetic sweetener. EFSA has set a daily intake limit.", "Yüksek yoğunluklu sentetik bir tatlandırıcı. EFSA günlük alım limiti belirlemiştir.", "Neotame", "Neotam", 1, "EFSA ADI: 2 mg/kg vücut ağırlığı" },
                    { "E962", 4, "A sweetener formed from a combination of aspartame and acesulfame K; the warnings applicable to aspartame also apply.", "Aspartam ve asesülfam K bileşiminden oluşan bir tatlandırıcı; aspartam bileşenine ilişkin uyarılar geçerlidir.", "Salt of Aspartame-Acesulfame", "Aspartam-Asesülfam Tuzu", 2, null },
                    { "E965", 4, "A sugar alcohol sweetener. EU regulation requires labelling that excessive consumption may have a laxative effect.", "Şeker alkolü grubunda bir tatlandırıcı. AB mevzuatına göre aşırı tüketiminin laksatif etki yapabileceği etikette belirtilmelidir.", "Maltitol", "Maltitol", 1, "AB Yönetmeliği 1333/2008" },
                    { "E966", 4, "A sugar alcohol sweetener. EU regulation requires labelling that excessive consumption may have a laxative effect.", "Şeker alkolü grubunda bir tatlandırıcı. AB mevzuatına göre aşırı tüketiminin laksatif etki yapabileceği etikette belirtilmelidir.", "Lactitol", "Laktitol", 1, "AB Yönetmeliği 1333/2008" },
                    { "E967", 4, "A sugar alcohol sweetener, often used in chewing gum. EU regulation requires labelling that excessive consumption may have a laxative effect.", "Şeker alkolü grubunda bir tatlandırıcı, sıkça sakızlarda kullanılır. AB mevzuatına göre aşırı tüketiminin laksatif etki yapabileceği etikette belirtilmelidir.", "Xylitol", "Ksilitol", 1, "AB Yönetmeliği 1333/2008" },
                    { "E968", 4, "A sugar alcohol sweetener with a lower digestive impact compared to others in its group.", "Şeker alkolü grubunda, diğerlerine kıyasla sindirim sistemi etkisi daha düşük olan bir tatlandırıcı.", "Erythritol", "Eritritol", 0, null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E100");

            migrationBuilder.DeleteData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E101");

            migrationBuilder.DeleteData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E102");

            migrationBuilder.DeleteData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E104");

            migrationBuilder.DeleteData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E110");

            migrationBuilder.DeleteData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E120");

            migrationBuilder.DeleteData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E122");

            migrationBuilder.DeleteData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E124");

            migrationBuilder.DeleteData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E127");

            migrationBuilder.DeleteData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E129");

            migrationBuilder.DeleteData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E131");

            migrationBuilder.DeleteData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E132");

            migrationBuilder.DeleteData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E133");

            migrationBuilder.DeleteData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E140");

            migrationBuilder.DeleteData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E150a");

            migrationBuilder.DeleteData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E150d");

            migrationBuilder.DeleteData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E160a");

            migrationBuilder.DeleteData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E160c");

            migrationBuilder.DeleteData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E162");

            migrationBuilder.DeleteData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E163");

            migrationBuilder.DeleteData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E171");

            migrationBuilder.DeleteData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E172");

            migrationBuilder.DeleteData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E173");

            migrationBuilder.DeleteData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E180");

            migrationBuilder.DeleteData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E200");

            migrationBuilder.DeleteData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E202");

            migrationBuilder.DeleteData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E203");

            migrationBuilder.DeleteData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E210");

            migrationBuilder.DeleteData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E211");

            migrationBuilder.DeleteData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E212");

            migrationBuilder.DeleteData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E220");

            migrationBuilder.DeleteData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E221");

            migrationBuilder.DeleteData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E222");

            migrationBuilder.DeleteData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E223");

            migrationBuilder.DeleteData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E224");

            migrationBuilder.DeleteData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E249");

            migrationBuilder.DeleteData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E250");

            migrationBuilder.DeleteData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E251");

            migrationBuilder.DeleteData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E252");

            migrationBuilder.DeleteData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E260");

            migrationBuilder.DeleteData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E261");

            migrationBuilder.DeleteData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E262");

            migrationBuilder.DeleteData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E263");

            migrationBuilder.DeleteData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E270");

            migrationBuilder.DeleteData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E280");

            migrationBuilder.DeleteData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E281");

            migrationBuilder.DeleteData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E282");

            migrationBuilder.DeleteData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E283");

            migrationBuilder.DeleteData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E296");

            migrationBuilder.DeleteData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E297");

            migrationBuilder.DeleteData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E300");

            migrationBuilder.DeleteData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E301");

            migrationBuilder.DeleteData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E306");

            migrationBuilder.DeleteData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E307");

            migrationBuilder.DeleteData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E310");

            migrationBuilder.DeleteData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E319");

            migrationBuilder.DeleteData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E320");

            migrationBuilder.DeleteData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E321");

            migrationBuilder.DeleteData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E322");

            migrationBuilder.DeleteData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E325");

            migrationBuilder.DeleteData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E330");

            migrationBuilder.DeleteData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E331");

            migrationBuilder.DeleteData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E332");

            migrationBuilder.DeleteData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E333");

            migrationBuilder.DeleteData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E334");

            migrationBuilder.DeleteData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E335");

            migrationBuilder.DeleteData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E336");

            migrationBuilder.DeleteData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E338");

            migrationBuilder.DeleteData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E339");

            migrationBuilder.DeleteData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E340");

            migrationBuilder.DeleteData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E341");

            migrationBuilder.DeleteData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E343");

            migrationBuilder.DeleteData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E350");

            migrationBuilder.DeleteData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E355");

            migrationBuilder.DeleteData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E363");

            migrationBuilder.DeleteData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E385");

            migrationBuilder.DeleteData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E400");

            migrationBuilder.DeleteData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E401");

            migrationBuilder.DeleteData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E402");

            migrationBuilder.DeleteData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E404");

            migrationBuilder.DeleteData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E406");

            migrationBuilder.DeleteData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E407");

            migrationBuilder.DeleteData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E410");

            migrationBuilder.DeleteData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E412");

            migrationBuilder.DeleteData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E413");

            migrationBuilder.DeleteData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E414");

            migrationBuilder.DeleteData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E415");

            migrationBuilder.DeleteData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E416");

            migrationBuilder.DeleteData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E417");

            migrationBuilder.DeleteData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E418");

            migrationBuilder.DeleteData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E420");

            migrationBuilder.DeleteData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E421");

            migrationBuilder.DeleteData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E422");

            migrationBuilder.DeleteData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E433");

            migrationBuilder.DeleteData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E440");

            migrationBuilder.DeleteData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E442");

            migrationBuilder.DeleteData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E460");

            migrationBuilder.DeleteData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E461");

            migrationBuilder.DeleteData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E463");

            migrationBuilder.DeleteData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E464");

            migrationBuilder.DeleteData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E466");

            migrationBuilder.DeleteData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E470a");

            migrationBuilder.DeleteData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E471");

            migrationBuilder.DeleteData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E472e");

            migrationBuilder.DeleteData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E473");

            migrationBuilder.DeleteData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E475");

            migrationBuilder.DeleteData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E476");

            migrationBuilder.DeleteData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E477");

            migrationBuilder.DeleteData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E481");

            migrationBuilder.DeleteData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E491");

            migrationBuilder.DeleteData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E492");

            migrationBuilder.DeleteData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E495");

            migrationBuilder.DeleteData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E500");

            migrationBuilder.DeleteData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E501");

            migrationBuilder.DeleteData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E503");

            migrationBuilder.DeleteData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E507");

            migrationBuilder.DeleteData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E508");

            migrationBuilder.DeleteData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E509");

            migrationBuilder.DeleteData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E511");

            migrationBuilder.DeleteData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E551");

            migrationBuilder.DeleteData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E553b");

            migrationBuilder.DeleteData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E574");

            migrationBuilder.DeleteData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E575");

            migrationBuilder.DeleteData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E576");

            migrationBuilder.DeleteData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E577");

            migrationBuilder.DeleteData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E578");

            migrationBuilder.DeleteData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E620");

            migrationBuilder.DeleteData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E621");

            migrationBuilder.DeleteData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E622");

            migrationBuilder.DeleteData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E623");

            migrationBuilder.DeleteData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E627");

            migrationBuilder.DeleteData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E631");

            migrationBuilder.DeleteData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E635");

            migrationBuilder.DeleteData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E901");

            migrationBuilder.DeleteData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E903");

            migrationBuilder.DeleteData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E904");

            migrationBuilder.DeleteData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E941");

            migrationBuilder.DeleteData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E942");

            migrationBuilder.DeleteData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E950");

            migrationBuilder.DeleteData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E951");

            migrationBuilder.DeleteData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E952");

            migrationBuilder.DeleteData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E954");

            migrationBuilder.DeleteData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E955");

            migrationBuilder.DeleteData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E960");

            migrationBuilder.DeleteData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E961");

            migrationBuilder.DeleteData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E962");

            migrationBuilder.DeleteData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E965");

            migrationBuilder.DeleteData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E966");

            migrationBuilder.DeleteData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E967");

            migrationBuilder.DeleteData(
                table: "Additives",
                keyColumn: "Code",
                keyValue: "E968");
        }
    }
}
