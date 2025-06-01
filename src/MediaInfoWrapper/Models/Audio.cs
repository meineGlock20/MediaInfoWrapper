// SPDX-FileCopyrightText: 2025 meineGlock20
// SPDX-License-Identifier: MIT
// ----------------------------------------------------------------------------------------------
// This wrapper (MediaInfoWrapper) is MIT‐licensed.
// This product uses MediaInfo library, Copyright (c) 2002-2025 MediaArea.net SARL.
// ----------------------------------------------------------------------------------------------

namespace MediaInfoWrapper.Models;

/// <summary>
/// Data transport object for Audio.
/// </summary>
public sealed record Audio
{
    public string? Codec { get; init; }
    public string? CompressionMode { get; init; }
    public string? ChannelPositions { get; init; }
    public TimeSpan Duration { get; init; }
    public int? Bitrate { get; init; }
    public string? BitrateMode { get; init; }
    public int? SamplingRate { get; init; }
}