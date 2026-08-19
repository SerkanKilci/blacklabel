using Blacklabel.Application.ExternalModels;
using Blacklabel.Application.Interfaces;

namespace Blacklabel.Tests.ExternalClients;

public class FakeOpenFoodFactsClient : IOpenFoodFactsClient
{
    private readonly Dictionary<string, OpenFoodFactsProduct?> _productsByBarcode = new();
    private readonly HashSet<string> _unavailableBarcodes = new();

    public int CallCount { get; private set; }

    public void SetResponse(string barcode, OpenFoodFactsProduct? product) => _productsByBarcode[barcode] = product;

    public void SetUnavailable(string barcode) => _unavailableBarcodes.Add(barcode);

    public Task<OpenFoodFactsLookupResult> GetProductAsync(string barcode, CancellationToken ct)
    {
        CallCount++;

        if (_unavailableBarcodes.Contains(barcode))
        {
            return Task.FromResult(new OpenFoodFactsLookupResult(OpenFoodFactsLookupOutcome.Unavailable, null));
        }

        var product = _productsByBarcode.GetValueOrDefault(barcode);
        var outcome = product is null ? OpenFoodFactsLookupOutcome.NotFound : OpenFoodFactsLookupOutcome.Found;
        return Task.FromResult(new OpenFoodFactsLookupResult(outcome, product));
    }
}
