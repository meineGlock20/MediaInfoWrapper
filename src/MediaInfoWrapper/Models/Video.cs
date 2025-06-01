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
    public string? AudioCodec { get; init; }
    public int? AudioChannels { get; init; }
    public int? AudioRate { get; init; }
    public string? BitDepth { get; init; }
    public long? BitRate { get; init; }
    public string? Codec { get; init; }
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
    public int? VideoBitrate { get; init; }
    public string? VideoCodec { get; init; }
    public int? Width { get; init; }
}