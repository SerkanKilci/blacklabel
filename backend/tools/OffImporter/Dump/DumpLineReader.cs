using System.IO.Compression;
using System.Runtime.CompilerServices;

namespace OffImporter.Dump;

/// <summary>
/// Streams a JSONL dump one line at a time so the whole file never has to sit in memory (§8:
/// "dosya büyük, belleğe komple alma"). Source can be a local path or an http(s) URL, and
/// ".gz" sources are decompressed on the fly.
/// </summary>
public static class DumpLineReader
{
    public static async IAsyncEnumerable<string> ReadLinesAsync(
        string source, HttpClient httpClient, [EnumeratorCancellation] CancellationToken ct)
    {
        var rawStream = await OpenAsync(source, httpClient, ct);
        var isGzip = source.EndsWith(".gz", StringComparison.OrdinalIgnoreCase);
        var contentStream = isGzip ? new GZipStream(rawStream, CompressionMode.Decompress) : rawStream;

        await using var streamHandle = contentStream;
        using var reader = new StreamReader(contentStream, leaveOpen: true);

        while (true)
        {
            ct.ThrowIfCancellationRequested();
            var line = await reader.ReadLineAsync(ct);
            if (line is null)
            {
                yield break;
            }

            if (!string.IsNullOrWhiteSpace(line))
            {
                yield return line;
            }
        }
    }

    private static async Task<Stream> OpenAsync(string source, HttpClient httpClient, CancellationToken ct)
    {
        if (source.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ||
            source.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
        {
            return await httpClient.GetStreamAsync(source, ct);
        }

        if (!File.Exists(source))
        {
            throw new FileNotFoundException($"OFF dump source file not found: {source}", source);
        }

        return new FileStream(source, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize: 1 << 20, useAsync: true);
    }
}
