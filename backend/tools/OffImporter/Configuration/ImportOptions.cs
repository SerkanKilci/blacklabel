namespace OffImporter.Configuration;

public sealed class ImportOptions
{
    /// <summary>Local file path or http(s) URL to the OFF dump. Supports plain .jsonl and gzip-compressed .jsonl.gz.</summary>
    public string DumpSource { get; set; } = string.Empty;

    /// <summary>How many matched (Turkish) products to accumulate before calling SaveChanges.</summary>
    public int BatchSize { get; set; } = 200;

    /// <summary>How many dump lines to process between console progress updates.</summary>
    public int ProgressIntervalLines { get; set; } = 5000;
}
