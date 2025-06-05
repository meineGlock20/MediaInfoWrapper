# MediaInfoWrapper

**A modern, cross-platform .NET 8 wrapper for the [MediaInfo](https://mediaarea.net/en/MediaInfo) library.**  
Extracts audio and video metadata (codec, bitrate, resolution, duration, etc.) from media files — with support for **Windows**, **macOS**, and **Linux**.

---

## ✨ Features

- 🎥 Extracts media metadata (codec, bitrate, resolution, etc.)
- 🖥️ Cross-platform support: Windows, macOS, and Linux
- 🔁 Thin and focused wrapper around native MediaInfo
- ⚡ Fast parsing options (`ParseSpeed.Fast`, `Complete.No`)
- 📦 Ready-to-use native binaries included via `runtimes/`

---

## 📦 Installation

[![NuGet version](https://img.shields.io/nuget/v/MeineGlock.MediaInfoWrapperDotNet.svg?label=NuGet)](https://www.nuget.org/packages/MeineGlock.MediaInfoWrapperDotNet/)

```bash
dotnet add package MeineGlock.MediaInfoWrapperDotNet --version 1.0.1
```

## 📄 Usage

```csharp
using MediaInfoWrapper;

// Get audio metadata from a file
var audioInfo = AudioInfo.GetAudioInfo("myfile.mp3", ParseSpeed.Maximum, Complete.Yes);
Console.WriteLine(audioInfo.Codec);       // e.g., AAC
Console.WriteLine(audioInfo.Bitrate);     // e.g., 128000

// Get video metadata from a file
var videoInfo = VideoInfo.GetVideoInfo("myfile.mp4", ParseSpeed.Maximum, Complete.Yes);
Console.WriteLine(videoInfo.Resolution);  // e.g., 1920x1080
Console.WriteLine(videoInfo.FriendlyResolution); // e.g., 1080p
```

## 📜 License
This wrapper is MIT licensed.

![GitHub](https://img.shields.io/github/license/MeineGlock20/MediaInfoWrapper)

This product uses MediaInfo library, Copyright (c) 2002-2025 MediaArea.net SARL.


