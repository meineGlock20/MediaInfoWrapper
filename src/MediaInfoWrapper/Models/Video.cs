// SPDX-FileCopyrightText: 2025 meineGlock20
// SPDX-License-Identifier: MIT
// ----------------------------------------------------------------------------------------------
// This wrapper (MediaInfoWrapper) is MIT‐licensed.
// This product uses MediaInfo library, Copyright (c) 2002-2025 MediaArea.net SARL.
// ----------------------------------------------------------------------------------------------

namespace MediaInfoWrapper.Models;

/// <summary>
/// Data transport object for Video.
/// </summary>
public sealed record Video
{
    public string? AspectRatio { get; init; }
    public string? AspectRatioDisplay { get; init; }
    public string? AudioCodec { get; init; }
    public string? AudioCodecID { get; init; }
    public int? AudioChannels { get; init; }
    public int? AudioRate { get; init; }
    public int? BitDepth { get; init; }
    public long? BitRate { get; init; }
    public string? Codec { get; init; }
    public string? CodecID { get; init; }
    public string? ColorSpace { get; init; }
    public long? Duration { get; init; }
    public DateTimeOffset? EncodedDate { get; init; }
    public string? FileExtension { get; init; }
    public long? FileSize { get; init; }
    public string? Format { get; init; }
    public int? FrameCount { get; init; }
    public double? FrameRate { get; init; }
    public bool HasSubTitles { get; init; }
    public int? Height { get; init; }
    public string? Language { get; init; }
    public string? MediaInfoVersion { get; init; }
    public string? Resolution { get; init; }
    public string? ScanType { get; init; }
    public string? Title { get; init; }
    public long? VideoBitrate { get; init; }
    public string? VideoCodec { get; init; }
    public int? Width { get; init; }

    public string? FriendlyResolution => Height.HasValue ? GetFriendlyResolution(Height.Value) : null;

    private static readonly Func<int, string> GetFriendlyResolution = height => height switch
    {
        >= 180 and <= 300 => "240p",
        >= 301 and <= 430 => "360p",
        >= 431 and <= 600 => "480p",
        >= 601 and <= 980 => "720p",
        >= 981 and <= 1200 => "1080p",
        >= 1201 and <= 2260 => "2K",
        >= 2261 and <= 3340 => "4K",
        >= 3341 and <= 4420 => "5K",
        >= 4421 and <= 6580 => "8K",
        _ => "Unknown"
    };
}