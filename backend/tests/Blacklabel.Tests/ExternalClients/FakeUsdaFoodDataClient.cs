using Blacklabel.Application.ExternalModels;
using Blacklabel.Application.Interfaces;

namespace Blacklabel.Tests.ExternalClients;

public class FakeUsdaFoodDataClient : IUsdaFoodDataClient
{
    private readonly Dictionary<string, UsdaFoodItem?> _productsByBarcode = new();
    private readonly HashSet<string> _unavailableBarcodes = new();

    public int CallCount { get; private set; }

    public void SetResponse(string barcode, UsdaFoodItem? product) => _productsByBarcode[barcode] = product;

    public void SetUnavailable(string barcode) => _unavailableBarcodes.Add(barcode);

    public Task<UsdaLookupResult> GetProductAsync(string barcode, CancellationToken ct)
    {
        CallCount++;

        if (_unavailableBarcodes.Contains(barcode))
        {
            return Task.FromResult(new UsdaLookupResult(UsdaLookupOutcome.Unavailable, null));
        }

        var product = _productsByBarcode.GetValueOrDefault(barcode);
        var outcome = product is null ? UsdaLookupOutcome.NotFound : UsdaLookupOutcome.Found;
        return Task.FromResult(new UsdaLookupResult(outcome, product));
    }
}
