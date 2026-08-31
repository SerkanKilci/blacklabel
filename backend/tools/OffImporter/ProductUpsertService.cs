using Blacklabel.Application.Barcode;
using Blacklabel.Application.Etl;
using Blacklabel.Application.Interfaces;
using Blacklabel.Application.Mapping;
using Blacklabel.Application.Scoring;
using Blacklabel.Domain.Entities;
using Blacklabel.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using OffImporter.Dump;

namespace OffImporter;

/// <summary>
/// Applies one raw dump line to the Product table: normalizes the barcode, filters to our
/// target markets (Turkey, US, Canada, Europe — see TargetMarketFilter), and upserts by barcode
/// so re-running the importer on the same dump is idempotent — an existing product is updated
/// in place rather than duplicated.
/// </summary>
public sealed class ProductUpsertService
{
    private readonly BlacklabelDbContext _context;
    private readonly IAdditiveRepository _additiveRepository;
    private readonly IAllergenRepository _allergenRepository;
    private readonly ScoreCalculator _scoreCalculator;

    public ProductUpsertService(
        BlacklabelDbContext context,
        IAdditiveRepository additiveRepository,
        IAllergenRepository allergenRepository,
        ScoreCalculator scoreCalculator)
    {
        _context = context;
        _additiveRepository = additiveRepository;
        _allergenRepository = allergenRepository;
        _scoreCalculator = scoreCalculator;
    }

    public enum Result
    {
        SkippedInvalidBarcode,
        SkippedNotTargetMarket,
        Created,
        Updated
    }

    public async Task<Result> UpsertAsync(OffDumpProduct raw, CancellationToken ct)
    {
        var barcode = BarcodeNormalizer.Normalize(raw.Code);
        if (barcode is null)
        {
            return Result.SkippedInvalidBarcode;
        }

        var countriesTags = raw.CountriesTags ?? new List<string>();
        if (!TargetMarketFilter.IsTargetMarketProduct(barcode, countriesTags))
        {
            return Result.SkippedNotTargetMarket;
        }

        // Check already-tracked-but-not-yet-saved entities first: the OFF dump can repeat the
        // same barcode within a single batch (re-normalized duplicates, re-exports), and a
        // DB-only lookup can't see a row that was Added to the ChangeTracker but not yet flushed
        // by SaveChanges, which previously caused a second insert of the same barcode and a
        // unique-index violation.
        var existing = _context.ChangeTracker.Entries<Product>()
            .Select(e => e.Entity)
            .FirstOrDefault(p => p.Barcode == barcode)
            ?? await _context.Products
                .Include(p => p.ProductAdditives)
                .Include(p => p.ProductAllergens)
                .FirstOrDefaultAsync(p => p.Barcode == barcode, ct);

        var isNew = existing is null;
        var product = existing ?? new Product
        {
            Id = Guid.NewGuid(),
            Barcode = barcode,
            CreatedAt = DateTime.UtcNow,
            ProductAdditives = new List<ProductAdditive>(),
            ProductAllergens = new List<ProductAllergen>()
        };

        var off = OffDumpMapper.ToOpenFoodFactsProduct(raw);
        await ProductFromOffMapper.ApplyAsync(product, off, _additiveRepository, _allergenRepository, _scoreCalculator, ct);

        if (isNew)
        {
            _context.Products.Add(product);
        }

        return isNew ? Result.Created : Result.Updated;
    }
}
