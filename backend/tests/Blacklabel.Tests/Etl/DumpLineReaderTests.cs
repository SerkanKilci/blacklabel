using System.IO.Compression;
using System.Text;
using OffImporter.Dump;
using Xunit;

namespace Blacklabel.Tests.Etl;

public class DumpLineReaderTests
{
    private static readonly string[] SampleLines =
    {
        "{\"code\":\"8690504010104\"}",
        "{\"code\":\"3017620422003\"}"
    };

    [Fact]
    public async Task ReadLinesAsync_Reads_Plain_Jsonl_File()
    {
        var path = Path.Combine(Path.GetTempPath(), $"off-{Guid.NewGuid()}.jsonl");
        await File.WriteAllLinesAsync(path, SampleLines);

        try
        {
            var lines = new List<string>();
            await foreach (var line in DumpLineReader.ReadLinesAsync(path, new HttpClient(), CancellationToken.None))
            {
                lines.Add(line);
            }

            Assert.Equal(SampleLines, lines);
        }
        finally
        {
            File.Delete(path);
        }
    }

    [Fact]
    public async Task ReadLinesAsync_Decompresses_Gzip_Jsonl_File()
    {
        var path = Path.Combine(Path.GetTempPath(), $"off-{Guid.NewGuid()}.jsonl.gz");
        await using (var fileStream = File.Create(path))
        await using (var gzipStream = new GZipStream(fileStream, CompressionMode.Compress))
        await using (var writer = new StreamWriter(gzipStream, Encoding.UTF8))
        {
            foreach (var line in SampleLines)
            {
                await writer.WriteLineAsync(line);
            }
        }

        try
        {
            var lines = new List<string>();
            await foreach (var line in DumpLineReader.ReadLinesAsync(path, new HttpClient(), CancellationToken.None))
            {
                lines.Add(line);
            }

            Assert.Equal(SampleLines, lines);
        }
        finally
        {
            File.Delete(path);
        }
    }

    [Fact]
    public async Task ReadLinesAsync_Throws_When_Local_File_Missing()
    {
        var missingPath = Path.Combine(Path.GetTempPath(), $"off-missing-{Guid.NewGuid()}.jsonl");

        await Assert.ThrowsAsync<FileNotFoundException>(async () =>
        {
            await foreach (var _ in DumpLineReader.ReadLinesAsync(missingPath, new HttpClient(), CancellationToken.None))
            {
            }
        });
    }
}
