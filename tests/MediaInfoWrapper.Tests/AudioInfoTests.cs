using Xunit.Abstractions;

namespace MediaInfoWrapper.Tests;

public class AudioInfoTests(ITestOutputHelper output)
{
    private readonly ITestOutputHelper _output = output;
    private const string SampleAudioPath = "sample-audio.mp3";

    [Fact]
    public void GetAudioInfo_DoesNotThrow_ForValidFile()
    {
        var audio = AudioInfo.GetAudioInfo(SampleAudioPath);
        Assert.NotNull(audio);
    }

    [Fact]
    public void GetAudioInfo_Throws_ForMissingFile()
    {
        var ex = Assert.Throws<FileNotFoundException>(() =>
            AudioInfo.GetAudioInfo("nonexistent-file.mp4"));
        Assert.Contains("does not exist", ex.Message);
    }

    [Fact]
    public void GetAudioInfo_Returns_ExpectedProperties()
    {
        var audio = AudioInfo.GetAudioInfo(SampleAudioPath);

        // Nullable numeric values: should be null or > 0
        Assert.True(audio.BitDepth == null || audio.BitDepth > 0);
        Assert.True(audio.Channels == null || audio.Channels > 0);
        Assert.True(audio.SamplingRate == null || audio.SamplingRate > 0);
        Assert.True(audio.Bitrate == null || audio.Bitrate > 0);

        // Non-null strings: allow null/empty but no crash
        _ = audio.Codec;
        _ = audio.CodecID;
        _ = audio.CompressionMode;
        _ = audio.ChannelPositions;
        _ = audio.FormatProfile;
        _ = audio.Language;
        _ = audio.BitrateMode;
        _ = audio.ServiceKind;
        _ = audio.Title;

        // Duration should never be negative
        Assert.True(audio.Duration >= TimeSpan.Zero);
    }

    [Fact]
    public void GetAudioInfo_ParsesAudioStreamOnly()
    {
        var audio = AudioInfo.GetAudioInfo(SampleAudioPath);
        // At minimum, it should contain either codec or sampling rate or channels
        Assert.True(!string.IsNullOrWhiteSpace(audio.Codec) || audio.Channels != null || audio.SamplingRate != null);
    }

    [Fact]
    public void GetAudioInfo_Duration_IsNonZero()
    {
        var audio = AudioInfo.GetAudioInfo(SampleAudioPath);
        Assert.True(audio.Duration.TotalMilliseconds > 0);
    }
}
