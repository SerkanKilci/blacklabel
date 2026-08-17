namespace Blacklabel.Application.Dtos;

public sealed record ScanResponse(Guid Id, string Barcode, Guid? ProductId, DateTime ScannedAt, int? ScoreAtScanTime);

public sealed record ScanPageResponse(IReadOnlyList<ScanResponse> Items, int Page, int PageSize, int TotalCount);

public sealed record CreateScanRequest(string Barcode, DateTime ScannedAt, int? ScoreAtScanTime);

public sealed record CreateScansRequest(IReadOnlyList<CreateScanRequest> Scans);
