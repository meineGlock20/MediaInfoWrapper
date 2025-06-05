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

// Create an instance of MediaInfo
using var mediaInfo = new MediaInfo();

// Open the video file
mediaInfo.Open(videoFilePath);

// Retrieve video information.
// You can adjust the ParseSpeed and Complete options as needed.
// Full parsing is more comprehensive but can be very slow on large files, while fast parsing is quicker but may miss some details.
// Try using `ParseSpeed.Fast` but if you find that some information is missing, you can switch to `ParseSpeed.Full` for a more thorough analysis.
var videoInfo = VideoInfo.GetVideoInfo(videoFilePath, ParseSpeed.Fast, Complete.Yes);

// Display video information
Console.WriteLine("Video Information:");
Console.WriteLine($"Title: {videoInfo.Title}");
Console.WriteLine($"Duration: {videoInfo.Duration}");
Console.WriteLine($"Bitrate: {videoInfo.BitRate}");
Console.WriteLine($"Codec: {videoInfo.Codec}");
Console.WriteLine($"Resolution: {videoInfo.Resolution}");
Console.WriteLine($"Friendly Resolution: {videoInfo.FriendlyResolution}");
Console.WriteLine($"Frame Rate: {videoInfo.FrameRate}");
