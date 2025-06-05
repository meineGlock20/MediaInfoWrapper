using MediaInfoWrapper;
using MediaInfoWrapper.Core;

Console.WriteLine("This demo will show you how to use the MediaInfoWrapper!");

// Enter the path to a video file.
Console.WriteLine("Enter the path to a video file:");
string? videoFilePath = Console.ReadLine()?.Trim('\"');

if (string.IsNullOrWhiteSpace(videoFilePath))
{
    Console.WriteLine("No video file path provided. Exiting.");
    return;
}

if (!File.Exists(videoFilePath))
{
    Console.WriteLine("The provided file does not exist. Exiting.");
    return;
}

// Time the operation.
var stopwatch = System.Diagnostics.Stopwatch.StartNew();

// Create an instance of MediaInfo
using var mediaInfo = new MediaInfo();

// Open the video file
mediaInfo.Open(videoFilePath);

// Retrieve video information.
// You can adjust the ParseSpeed and Complete options as needed.
// Full parsing is more comprehensive but can be very slow on large files, while fast parsing is quicker but may miss some details.
// Try using `ParseSpeed.Fast` but if you find that some information is missing, you can switch to `ParseSpeed.Full` for a more thorough analysis.
var videoInfo = VideoInfo.GetVideoInfo(videoFilePath, ParseSpeed.Full, Complete.Yes);

// Display video information.
Console.WriteLine("== Video Information ==");
Console.WriteLine($"Aspect Ratio: {videoInfo.AspectRatio}.");
Console.WriteLine($"Aspect Ratio Display: {videoInfo.AspectRatioDisplay}");
Console.WriteLine($"Audio Codec: {videoInfo.AudioCodec}");
Console.WriteLine($"Audio Codec ID: {videoInfo.AudioCodecID}");
Console.WriteLine($"Audio Channels: {videoInfo.AudioChannels}");
Console.WriteLine($"Audio Rate: {videoInfo.AudioRate}");
Console.WriteLine($"Bit Depth: {videoInfo.BitDepth}");
Console.WriteLine($"Bit Rate: {videoInfo.BitRate}");
Console.WriteLine($"Codec: {videoInfo.Codec}");
Console.WriteLine($"Codec ID: {videoInfo.CodecID}");
Console.WriteLine($"Color Space: {videoInfo.ColorSpace}");
Console.WriteLine($"Duration: {videoInfo.Duration} ms");
Console.WriteLine($"Encoded Date: {videoInfo.EncodedDate}");
Console.WriteLine($"File Extension: {videoInfo.FileExtension}");
Console.WriteLine($"File Size: {videoInfo.FileSize} bytes");
Console.WriteLine($"Format: {videoInfo.Format}");
Console.WriteLine($"Frame Count: {videoInfo.FrameCount}");
Console.WriteLine($"Frame Rate: {videoInfo.FrameRate} fps");
Console.WriteLine($"Has Subtitles: {videoInfo.HasSubTitles}");
Console.WriteLine($"Height: {videoInfo.Height} pixels");
Console.WriteLine($"Language: {videoInfo.Language}");
Console.WriteLine($"MediaInfo Version: {videoInfo.MediaInfoVersion}");
Console.WriteLine($"Resolution: {videoInfo.Resolution}");
Console.WriteLine($"Scan Type: {videoInfo.ScanType}");
Console.WriteLine($"Title: {videoInfo.Title}");
Console.WriteLine($"Video Bitrate: {videoInfo.VideoBitrate}");
Console.WriteLine($"Video Codec: {videoInfo.VideoCodec}");
Console.WriteLine($"Width: {videoInfo.Width}");
Console.WriteLine($"Resolution: {videoInfo.Resolution}");
Console.WriteLine($"Friendly Resolution: {videoInfo.FriendlyResolution}");

stopwatch.Stop();
Console.WriteLine($"Time taken to retrieve video information: {stopwatch.ElapsedMilliseconds} ms");

// ============= END OF DEMO =============

