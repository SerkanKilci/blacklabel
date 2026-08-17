namespace Blacklabel.Application.ExternalModels;

public sealed record ContributionImage(string Slot, string FileName, string ContentType, byte[] Content);
