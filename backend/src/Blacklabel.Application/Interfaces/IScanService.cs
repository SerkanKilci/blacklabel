using Blacklabel.Application.Dtos;

namespace Blacklabel.Application.Interfaces;

public interface IScanService
{
    Task<ScanPageResponse> GetHistoryAsync(Guid userId, int page, int pageSize, CancellationToken ct);
    Task<IReadOnlyList<ScanResponse>> RecordScansAsync(Guid userId, IReadOnlyList<CreateScanRequest> scans, CancellationToken ct);
}
