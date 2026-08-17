namespace Blacklabel.Domain.Entities;

public class Scan
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public AppUser User { get; set; } = null!;

    public string Barcode { get; set; } = string.Empty;
    public Guid? ProductId { get; set; }
    public Product? Product { get; set; }
    public DateTime ScannedAt { get; set; }
    public int? ScoreAtScanTime { get; set; }
}
