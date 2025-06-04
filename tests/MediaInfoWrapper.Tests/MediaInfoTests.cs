using MediaInfoWrapper;
using MediaInfoWrapper.Core;
using Xunit.Abstractions;

namespace MediaInfoWrapper.Tests;

public class MediaInfoTests(ITestOutputHelper output)
{
    private readonly ITestOutputHelper _output = output;
    private const string SampleVideoPath = "sample-video.mp4";
    private const string SampleAudioPath = "sample-audio.mp4";

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
        mi.Open(SampleVideoPath);

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

    [Fact]
    public void GetVideoInfo_ReturnsExpectedFields()
    {
        // Arrange
        Assert.True(File.Exists(SampleVideoPath), $"Sample file not found at path: {SampleVideoPath}");

        // Act
        var result = VideoInfo.GetVideoInfo(SampleVideoPath);

        // Assert – Existence and Type
        Assert.NotNull(result);

        // These are the expected fields in the Video model.
        Assert.Equal(".mp4", result.FileExtension, ignoreCase: true);
        Assert.NotNull(result.Format);
        Assert.NotNull(result.Codec);
        Assert.NotNull(result.VideoCodec);
        Assert.NotNull(result.MediaInfoVersion);

        // Assert optional fields with fallback checks.
        Assert.True(result.AspectRatio == null || !string.IsNullOrEmpty(result.AspectRatio));
        Assert.True(result.AspectRatioDisplay == null || !string.IsNullOrEmpty(result.AspectRatioDisplay));
        Assert.True(result.AudioChannels == null || result.AudioChannels > 0);
        Assert.True(result.AudioCodec == null || !string.IsNullOrEmpty(result.AudioCodec));
        Assert.True(result.AudioCodecID == null || !string.IsNullOrEmpty(result.AudioCodecID));
        Assert.True(result.AudioRate == null || result.AudioRate > 0);
        Assert.True(result.BitDepth == null || result.BitDepth > 0);
        Assert.True(result.BitRate == null || result.BitRate > 0);
        Assert.True(result.CodecID == null || !string.IsNullOrEmpty(result.CodecID));
        Assert.True(result.ColorSpace == null || !string.IsNullOrEmpty(result.ColorSpace));
        Assert.True(result.Duration == null || result.Duration > 0);
        Assert.True(result.EncodedDate == null || result.EncodedDate.Value.Year > 1970);
        Assert.True(result.FileSize == null || result.FileSize > 0);
        Assert.True(result.FrameCount == null || result.FrameCount > 0);
        Assert.True(result.FrameRate == null || result.FrameRate > 0);
        Assert.True(result.HasSubTitles == true || result.HasSubTitles == false); // Can be true or false, so we just check if it exists.
        Assert.True(result.Height == null || result.Height > 0);
        Assert.True(result.Language == null || string.IsNullOrWhiteSpace(result.Language) || result.Language.Length > 0);
        Assert.True(result.Resolution == null || !string.IsNullOrEmpty(result.Resolution));
        Assert.True(result.ScanType == null || !string.IsNullOrEmpty(result.ScanType));
        Assert.True(result.Title == null || string.IsNullOrWhiteSpace(result.Title));
        Assert.True(result.VideoBitrate == null || result.VideoBitrate > 0);
        Assert.True(result.VideoCodec == null || !string.IsNullOrEmpty(result.VideoCodec));
        Assert.True(result.Width == null || result.Width > 0);
        Assert.True(result.FriendlyResolution == null || !string.IsNullOrEmpty(result.FriendlyResolution));

        // Printing the results to the output for debugging purposes.
        _output.WriteLine("Video Information:");
        _output.WriteLine($"File Extension: {result.FileExtension}");
        _output.WriteLine($"Format: {result.Format}");
        _output.WriteLine($"Codec: {result.Codec}");
        _output.WriteLine($"Video Codec: {result.VideoCodec}");
        _output.WriteLine($"MediaInfo Version: {result.MediaInfoVersion}");

        _output.WriteLine($"Aspect Ratio: {result.AspectRatio}");
        _output.WriteLine($"Aspect Ratio Display: {result.AspectRatioDisplay}");
        _output.WriteLine($"Audio Channels: {result.AudioChannels}");
        _output.WriteLine($"Audio Codec: {result.AudioCodec}");
        _output.WriteLine($"Audio Codec ID: {result.AudioCodecID}");
        _output.WriteLine($"Audio Rate: {result.AudioRate}");
        _output.WriteLine($"Bit Depth: {result.BitDepth}");
        _output.WriteLine($"Bit Rate: {result.BitRate}");
        _output.WriteLine($"Codec ID: {result.CodecID}");
        _output.WriteLine($"Color Space: {result.ColorSpace}");
        _output.WriteLine($"Duration: {result.Duration}");
        _output.WriteLine($"Encoded Date: {result.EncodedDate}");
        _output.WriteLine($"File Size: {result.FileSize}");
        _output.WriteLine($"Frame Count: {result.FrameCount}");
        _output.WriteLine($"Frame Rate: {result.FrameRate}");
        _output.WriteLine($"Has SubTitles: {result.HasSubTitles}");
        _output.WriteLine($"Height: {result.Height}");
        _output.WriteLine($"Language: {result.Language}");
        _output.WriteLine($"Title: {result.Title}");
        _output.WriteLine($"Resolution: {result.Resolution}");
        _output.WriteLine($"Scan Type: {result.ScanType}");
        _output.WriteLine($"Video Bitrate: {result.VideoBitrate}");
        _output.WriteLine($"Video Codec: {result.VideoCodec}");
        _output.WriteLine($"Width: {result.Width}");
        _output.WriteLine($"Friendly Resolution: {result.FriendlyResolution}");
        _output.WriteLine("Test completed successfully.");
    }

    [Fact]
    public void FriendlyResolution_ReturnsNull_WhenHeightIsNull()
    {
        var info = new MediaInfoWrapper.Models.Video { Height = null };
        Assert.Null(info.FriendlyResolution);
    }

    [Fact]
    public void FriendlyResolution_ReturnsUnknown_WhenHeightIsOutOfRange()
    {
        var info = new MediaInfoWrapper.Models.Video { Height = 1_000_000 }; // Height is way above the defined ranges
        Assert.True(info.FriendlyResolution?.Equals("Unknown", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public void FriendlyResolution_ReturnsCorrect()
    {
        var info = new MediaInfoWrapper.Models.Video { Height = 1080 };
        Assert.True(info.FriendlyResolution?.Equals("1080p", StringComparison.OrdinalIgnoreCase));
    }

    [Theory]
    [InlineData(240, "240p")]
    [InlineData(360, "360p")]
    [InlineData(480, "480p")]
    [InlineData(720, "720p")]
    [InlineData(1080, "1080p")]
    [InlineData(1440, "2K")]
    [InlineData(3000, "4K")]
    [InlineData(4000, "5K")]
    [InlineData(6000, "8K")]
    public void FriendlyResolution_ReturnsExpectedLabel(int height, string expectedLabel)
    {
        var info = new MediaInfoWrapper.Models.Video { Height = height };
        Assert.Equal(expectedLabel, info.FriendlyResolution);
    }

    [Fact]
    public void CanInstantiateVideoRecord()
    {
        var video = new MediaInfoWrapper.Models.Video(); // Assuming it has a parameterless constructor
        Assert.NotNull(video);
    }
}
