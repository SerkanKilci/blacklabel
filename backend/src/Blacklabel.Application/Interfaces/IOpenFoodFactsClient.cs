using Blacklabel.Application.ExternalModels;

namespace Blacklabel.Application.Interfaces;

public interface IOpenFoodFactsClient
{
    Task<OpenFoodFactsProduct?> GetProductAsync(string barcode, CancellationToken ct);
}
