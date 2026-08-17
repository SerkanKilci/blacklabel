using Blacklabel.Application.Barcode;
using Blacklabel.Application.Dtos;
using Blacklabel.Application.Interfaces;
using Blacklabel.Domain.Entities;

namespace Blacklabel.Application.Services;

public class ScanService : IScanService
{
    private const int FreeHistoryLimit = 20;

    private readonly IScanRepository _scanRepository;
    private readonly IProductRepository _productRepository;
    private readonly IAppUserRepository _appUserRepository;

    public ScanService(IScanRepository scanRepository, IProductRepository productRepository, IAppUserRepository appUserRepository)
    {
        _scanRepository = scanRepository;
        _productRepository = productRepository;
        _appUserRepository = appUserRepository;
    }

    public async Task<ScanPageResponse> GetHistoryAsync(Guid userId, int page, int pageSize, CancellationToken ct)
    {
        var safePage = Math.Max(1, page);
        var safePageSize = Math.Clamp(pageSize, 1, 100);

        var (items, totalCount) = await _scanRepository.GetPagedByUserAsync(userId, safePage, safePageSize, ct);

        var user = await _appUserRepository.GetByIdAsync(userId, ct);
        var isPremium = user?.IsPremium ?? false;

        if (!isPremium)
        {
            // Free tier only ever sees its most recent 20 scans (§11).
            var offset = (safePage - 1) * safePageSize;
            var remaining = Math.Max(0, FreeHistoryLimit - offset);
            items = items.Take(remaining).ToList();
            totalCount = Math.Min(totalCount, FreeHistoryLimit);
        }

        var responseItems = items
            .Select(s => new ScanResponse(s.Id, s.Barcode, s.ProductId, s.ScannedAt, s.ScoreAtScanTime))
            .ToList();

        return new ScanPageResponse(responseItems, safePage, safePageSize, totalCount);
    }

    public async Task<IReadOnlyList<ScanResponse>> RecordScansAsync(
        Guid userId, IReadOnlyList<CreateScanRequest> scans, CancellationToken ct)
    {
        var entities = new List<Scan>();

        foreach (var request in scans)
        {
            var barcode = BarcodeNormalizer.Normalize(request.Barcode);
            if (barcode is null)
            {
                continue;
            }

            var productId = await _productRepository.GetIdByBarcodeAsync(barcode, ct);

            entities.Add(new Scan
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Barcode = barcode,
                ProductId = productId,
                ScannedAt = request.ScannedAt,
                ScoreAtScanTime = request.ScoreAtScanTime
            });
        }

        if (entities.Count > 0)
        {
            await _scanRepository.AddRangeAsync(entities, ct);
            await _scanRepository.SaveChangesAsync(ct);
        }

        return entities
            .Select(s => new ScanResponse(s.Id, s.Barcode, s.ProductId, s.ScannedAt, s.ScoreAtScanTime))
            .ToList();
    }
}
