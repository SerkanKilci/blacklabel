namespace Blacklabel.Application.Dtos;

/// <summary>
/// "Provider" is "apple" or "google". The provider user id and email are never taken from the
/// client directly — they're derived server-side from the verified claims inside IdentityToken
/// (the raw id_token returned by Sign in with Apple / Google), so a client can't claim someone
/// else's account.
/// </summary>
public sealed record LinkAccountRequest(string Provider, string IdentityToken);
