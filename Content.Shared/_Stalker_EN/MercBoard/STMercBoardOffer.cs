using Robust.Shared.Serialization;

namespace Content.Shared._Stalker_EN.MercBoard;

/// <summary>
/// Distinguishes between merc-posted services and player-posted job requests.
/// </summary>
[Serializable, NetSerializable]
public enum STMercBoardOfferType : byte
{
    /// <summary>Mercenary advertising their services (visible to all).</summary>
    Service = 0,

    /// <summary>Player requesting mercenary help (visible to mercs only).</summary>
    Job = 1,
}

/// <summary>
/// A single offer on the mercenary board — either a service or a job request.
/// Immutable record sent over the network as part of <see cref="STMercBoardUiState"/>.
/// </summary>
[Serializable, NetSerializable]
public sealed class STMercBoardOffer
{
    /// <summary>Prefix used in offer reference strings (e.g. "MB#3").</summary>
    public const string OfferRefPrefix = "MB#";

    /// <summary>Formats an offer ID as a bracketed reference string (e.g. "[MB#3]").</summary>
    public static string FormatRef(uint id) => $"[{OfferRefPrefix}{id}]";

    /// <summary>Unique offer ID within the current round.</summary>
    public readonly uint Id;

    /// <summary>Whether this is a merc service or a player job request.</summary>
    public readonly STMercBoardOfferType OfferType;

    /// <summary>In-game character name of the poster.</summary>
    public readonly string PosterName;

    /// <summary>
    /// The poster's unique messenger ID (e.g. "472-819").
    /// Used by "Contact Poster" to add them as a messenger contact.
    /// Null if the poster has no messenger ID.
    /// </summary>
    public readonly string? PosterMessengerId;

    /// <summary>
    /// Faction name of the poster (resolved at post time via BandsComponent).
    /// Null if the poster has no faction.
    /// </summary>
    public readonly string? PosterFaction;

    /// <summary>Free-text description of the offer.</summary>
    public readonly string Description;

    /// <summary>Free-text price field (e.g. "500 RU", "Negotiable").</summary>
    public readonly string Price;

    /// <summary>Free-text duration field (e.g. "2 hours", "End of round").</summary>
    public readonly string Duration;

    /// <summary>Server CurTime when the offer was posted. Used client-side for live elapsed time.</summary>
    public readonly TimeSpan Timestamp;

    public STMercBoardOffer(
        uint id,
        STMercBoardOfferType offerType,
        string posterName,
        string? posterMessengerId,
        string? posterFaction,
        string description,
        string price,
        string duration,
        TimeSpan timestamp)
    {
        Id = id;
        OfferType = offerType;
        PosterName = posterName;
        PosterMessengerId = posterMessengerId;
        PosterFaction = posterFaction;
        Description = description;
        Price = price;
        Duration = duration;
        Timestamp = timestamp;
    }
}
