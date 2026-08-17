using Blacklabel.Application.ExternalModels;
using Blacklabel.Application.Interfaces;

namespace Blacklabel.Tests.ExternalClients;

public class FakeOpenFoodFactsClient : IOpenFoodFactsClient
{
    private readonly Dictionary<string, OpenFoodFactsProduct?> _productsByBarcode = new();

    public int CallCount { get; private set; }

    public void SetResponse(string barcode, OpenFoodFactsProduct? product) => _productsByBarcode[barcode] = product;

    public Task<OpenFoodFactsProduct?> GetProductAsync(string barcode, CancellationToken ct)
    {
        CallCount++;
        return Task.FromResult(_productsByBarcode.GetValueOrDefault(barcode));
    }
}
