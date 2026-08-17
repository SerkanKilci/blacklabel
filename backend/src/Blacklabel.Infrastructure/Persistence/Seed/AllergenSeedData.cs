using Blacklabel.Domain.Entities;

namespace Blacklabel.Infrastructure.Persistence.Seed;

public static class AllergenSeedData
{
    public static IReadOnlyList<Allergen> Get() => new List<Allergen>
    {
        new() { Code = "gluten", NameTr = "Glüten (Buğday, Çavdar, Arpa, Yulaf)", NameEn = "Gluten (Wheat, Rye, Barley, Oats)" },
        new() { Code = "crustaceans", NameTr = "Kabuklu Deniz Ürünleri", NameEn = "Crustaceans" },
        new() { Code = "eggs", NameTr = "Yumurta", NameEn = "Eggs" },
        new() { Code = "fish", NameTr = "Balık", NameEn = "Fish" },
        new() { Code = "peanuts", NameTr = "Yer Fıstığı", NameEn = "Peanuts" },
        new() { Code = "soybeans", NameTr = "Soya", NameEn = "Soybeans" },
        new() { Code = "milk", NameTr = "Süt (Laktoz Dahil)", NameEn = "Milk (Including Lactose)" },
        new() { Code = "nuts", NameTr = "Sert Kabuklu Yemişler (Fındık, Badem, Ceviz vb.)", NameEn = "Tree Nuts" },
        new() { Code = "celery", NameTr = "Kereviz", NameEn = "Celery" },
        new() { Code = "mustard", NameTr = "Hardal", NameEn = "Mustard" },
        new() { Code = "sesame-seeds", NameTr = "Susam", NameEn = "Sesame Seeds" },
        new() { Code = "sulphur-dioxide-and-sulphites", NameTr = "Kükürt Dioksit ve Sülfitler", NameEn = "Sulphur Dioxide and Sulphites" },
        new() { Code = "lupin", NameTr = "Acı Bakla (Lupin)", NameEn = "Lupin" },
        new() { Code = "molluscs", NameTr = "Yumuşakçalar", NameEn = "Molluscs" },
    };
}
