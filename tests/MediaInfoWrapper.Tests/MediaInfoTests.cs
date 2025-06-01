using MediaInfoWrapper;
using MediaInfoWrapper.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaInfoWrapper.Tests;

public class MediaInfoTests
{
    [Fact]
    public void CanCreateInstance()
    {
        using var mi = new MediaInfo();
        Assert.NotNull(mi);
    }

    [Fact]
    public void Open_And_Get_BasicFields_ShouldReturnNonNull()
    {
        using var mi = new MediaInfo();
        mi.Option("ParseSpeed", "1");       // full scan
        mi.Option("Complete", "1");         // verbose fields
        mi.Open("sample-video.mp4");

        // “General” fields
        string? format = mi.Get(StreamKind.General, 0, "Format");
        Assert.False(string.IsNullOrEmpty(format));

        // Audio: Count should at least be ≥ 1
        int audioCount = mi.Count(StreamKind.Audio);
        Assert.True(audioCount >= 1);

        // Get a known numeric field (e.g. “BitRate”) and parse
        string? bps = mi.Get(StreamKind.General, 0, "OverallBitRate");
        Assert.False(string.IsNullOrEmpty(bps));
        Assert.True(long.TryParse(bps, out _));
    }
}
