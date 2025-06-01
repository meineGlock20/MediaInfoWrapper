namespace MediaInfoWrapper.Models;

/// <summary>
/// Data transport object for Video.
/// </summary>
public sealed record Video
{
    public string? AspectRatio { get; init; }
    public int? AudioChannels { get; init; }
    public int? AudioRate { get; init; }
    public long? Duration { get; init; }
    public long? FileSize { get; init; }
    public string? Format { get; init; }
    public double? FrameRate { get; init; }
    public bool HasSubTitles { get; init; }
    public int? Height { get; init; }
    public string? MediaInfoVersion { get; init; }
    public string? Codec { get; init; }
    public long? Bitrate { get; init; }
    public string? Resolution { get; init; }
    public int? Width { get; init; }
}