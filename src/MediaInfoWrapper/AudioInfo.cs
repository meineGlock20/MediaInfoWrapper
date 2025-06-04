// SPDX-FileCopyrightText: 2025 meineGlock20
// SPDX-License-Identifier: MIT
// ----------------------------------------------------------------------------------------------
// This wrapper (MediaInfoWrapper) is MIT‐licensed.
// This product uses MediaInfo library, Copyright (c) 2002-2025 MediaArea.net SARL.
// ----------------------------------------------------------------------------------------------

using MediaInfoWrapper.Core;

namespace MediaInfoWrapper;

public class AudioInfo
{
    public static Models.Audio GetAudioInfo(string filePath, ParseSpeed parseSpeed = ParseSpeed.Fast, Complete complete = Complete.No)
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

        var audio = new Models.Audio
        {
            BitDepth = int.TryParse(mediaInfo.Get(StreamKind.Audio, 0, "BitDepth"), out var bitDepth) ? bitDepth : null,
            Channels = mediaInfo.Count(StreamKind.Audio) > 0
                ? int.TryParse(mediaInfo.Get(StreamKind.Audio, 0, "Channel(s)") ?? mediaInfo.Get(StreamKind.Audio, 0, "Channels"), out var channels) ? channels : null
                : null,
            Codec = mediaInfo.Get(StreamKind.Audio, 0, "Format"),
            CodecID = mediaInfo.Get(StreamKind.Audio, 0, "CodecID"),
            CompressionMode = mediaInfo.Get(StreamKind.Audio, 0, "Compression_Mode"),
            ChannelPositions = mediaInfo.Get(StreamKind.Audio, 0, "ChannelPositions"),
            Duration = TimeSpan.FromMilliseconds(long.TryParse(mediaInfo.Get(StreamKind.General, 0, "Duration"), out var duration) ? duration : 0),
            FormatProfile = mediaInfo.Get(StreamKind.Audio, 0, "Format_Profile"),
            Language = mediaInfo.Get(StreamKind.General, 0, "Language"),
            Bitrate = long.TryParse(mediaInfo.Get(StreamKind.Audio, 0, "BitRate"), out var bitrate) ? bitrate : null,
            BitrateMode = mediaInfo.Get(StreamKind.Audio, 0, "BitRate_Mode"),
            SamplingRate = int.TryParse(mediaInfo.Get(StreamKind.Audio, 0, "SamplingRate"), out var samplingRate) ? samplingRate : null,
            ServiceKind = mediaInfo.Get(StreamKind.Audio, 0, "Service_kind"),
            Title = mediaInfo.Get(StreamKind.General, 0, "Title")
        };

        return audio;
    }
}