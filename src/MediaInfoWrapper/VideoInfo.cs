// SPDX-FileCopyrightText: 2025 meineGlock20
// SPDX-License-Identifier: MIT
// ----------------------------------------------------------------------------------------------
// This wrapper (MediaInfoWrapper) is MIT‐licensed.
// This product uses MediaInfo library, Copyright (c) 2002-2025 MediaArea.net SARL.
// ----------------------------------------------------------------------------------------------

using MediaInfoWrapper.Core;

namespace MediaInfoWrapper;

public class VideoInfo
{
    public static Models.Video GetVideoInfo(string filePath, ParseSpeed parseSpeed = ParseSpeed.Fast, Complete complete = Complete.No)
    {
        // Validate file existence.
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException("The specified file does not exist.", filePath);
        }

        // Create a new instance of MediaInfo.
        // and set options for parsing speed and completeness.
        using var mediaInfo = new MediaInfo();
        mediaInfo.Option("ParseSpeed", ((int)parseSpeed).ToString());
        mediaInfo.Option("Complete", ((int)complete).ToString());

        mediaInfo.Open(filePath);

        var video = new Models.Video
        {
            AspectRatio = mediaInfo.Get(StreamKind.Video, 0, "AspectRatio"),
            AspectRatioDisplay = mediaInfo.Get(StreamKind.Video, 0, "DisplayAspectRatio/String"),
            AudioCodec = mediaInfo.Get(StreamKind.Audio, 0, "Format"),
            AudioCodecID = mediaInfo.Get(StreamKind.Audio, 0, "CodecID"),

            AudioChannels = mediaInfo.Count(StreamKind.Audio) > 0
                ? int.TryParse(mediaInfo.Get(StreamKind.Audio, 0, "Channel(s)") ?? mediaInfo.Get(StreamKind.Audio, 0, "Channels"), out var channels) ? channels : null
                : null,

            AudioRate = mediaInfo.Count(StreamKind.Audio) > 0 ?
                (int.TryParse(mediaInfo.Get(StreamKind.Audio, 0, "SamplingRate"), out var rate) ? rate : null) : null,

            BitDepth = int.TryParse(mediaInfo.Get(StreamKind.Video, 0, "BitDepth"), out var bitDepth) ? bitDepth : null,
            BitRate = long.TryParse(mediaInfo.Get(StreamKind.Video, 0, "BitRate"), out var bitrate) ? bitrate : null,
            Codec = mediaInfo.Get(StreamKind.Video, 0, "Format"),
            CodecID = mediaInfo.Get(StreamKind.Video, 0, "CodecID"),
            ColorSpace = mediaInfo.Get(StreamKind.Video, 0, "ColorSpace"),
            Duration = long.TryParse(mediaInfo.Get(StreamKind.General, 0, "Duration"), out var duration) ? duration : null,

            // REM: Pre-process the string to remove "UTC " or "Z " before parsing
            EncodedDate = TryParseEncodedDate(mediaInfo.Get(StreamKind.General, 0, "Encoded_Date")),

            FileExtension = Path.GetExtension(filePath),
            FileSize = long.TryParse(mediaInfo.Get(StreamKind.General, 0, "FileSize"), out var fileSize) ? fileSize : null,
            Format = mediaInfo.Get(StreamKind.General, 0, "Format"),
            FrameCount = int.TryParse(mediaInfo.Get(StreamKind.Video, 0, "FrameCount"), out var frameCount) ? frameCount : null,
            FrameRate = double.TryParse(mediaInfo.Get(StreamKind.Video, 0, "FrameRate"), out var frameRate) ? frameRate : null,
            HasSubTitles = mediaInfo.Count(StreamKind.Text) > 0,
            Height = int.TryParse(mediaInfo.Get(StreamKind.Video, 0, "Height"), out var height) ? height : null,
            Language = mediaInfo.Get(StreamKind.Audio, 0, "Language") ?? mediaInfo.Get(StreamKind.Text, 0, "Language"),
            MediaInfoVersion = mediaInfo.Option("MediaInfo_Version"),

            // REM: Resolution may not always be available so fallback to "WidthxHeight" or null if neither are available.
            Resolution = GetResolution(mediaInfo),

            ScanType = mediaInfo.Get(StreamKind.Video, 0, "ScanType"),
            Title = mediaInfo.Get(StreamKind.General, 0, "Title"),
            VideoBitrate = long.TryParse(mediaInfo.Get(StreamKind.Video, 0, "BitRate"), out var videoBitrate) ? videoBitrate : null,
            VideoCodec = mediaInfo.Get(StreamKind.Video, 0, "Format"),
            Width = int.TryParse(mediaInfo.Get(StreamKind.Video, 0, "Width"), out var width) ? width : null
        };

        static string? GetResolution(MediaInfo mi) =>
                string.IsNullOrWhiteSpace(mi.Get(StreamKind.Video, 0, "Resolution"))
                    ? (mi.Get(StreamKind.Video, 0, "Width") is { Length: > 0 } w &&
                    mi.Get(StreamKind.Video, 0, "Height") is { Length: > 0 } h
                        ? $"{w}x{h}" : null)
                    : mi.Get(StreamKind.Video, 0, "Resolution");

        static DateTimeOffset? TryParseEncodedDate(string? raw)
        {
            if (string.IsNullOrWhiteSpace(raw)) return null;

            var cleaned = raw
                .Replace("UTC ", "", StringComparison.OrdinalIgnoreCase)
                .Replace("Z ", "", StringComparison.OrdinalIgnoreCase)
                .Trim();

            return DateTimeOffset.TryParse(cleaned, out var parsed) ? parsed : null;
        }

        return video;
    }
}
