// SPDX-FileCopyrightText: 2025 meineGlock20
// SPDX-License-Identifier: MIT
// ----------------------------------------------------------------------------------------------
// This wrapper (MediaInfoWrapper) is MIT‐licensed.
// This product uses MediaInfo library, Copyright (c) 2002-2025 MediaArea.net SARL.
// ----------------------------------------------------------------------------------------------

namespace MediaInfoWrapper.Core;

/// <summary>
/// Represents the speed of parsing media information.
/// </summary>
public enum ParseSpeed
{
    /// <summary>
    /// Fast is the default and is suitable for most cases.
    /// </summary>
    Fast = 0,
    /// <summary>
    /// Full is more thorough and will take longer, but provides more detailed and accurate information.
    /// <para>
    /// 🚩 This will impact performance, so it should only be used when necessary. It can take significantly longer to parse large media files.
    /// </para>
    /// </summary>
    Full = 1,
}

/// <summary>
/// Represents whether the MediaInfo should return complete information.
/// </summary>
public enum Complete
{
    /// <summary>
    /// No is the default and means the MediaInfo will return only essential information.
    /// </summary>
    No = 0,
    /// <summary>
    /// Yes will return more detailed and verbose information.
    /// This is useful for detailed analysis but may not be necessary for all use cases.
    /// </summary>
    Yes = 1,
}

/// <summary>
/// Represents the kind of stream.
/// </summary>
public enum StreamKind
{
    General,
    Video,
    Audio,
    Text,
    Other,
    Image,
    Menu,
}

/// <summary>
/// Represents the kind of information to retrieve from MediaInfo.
/// </summary>
public enum InfoKind
{
    Name,
    Text,
    Measure,
    Options,
    NameText,
    MeasureText,
    Info,
    HowTo
}

/// <summary>
/// Represents the options for retrieving information from MediaInfo.
/// </summary>
public enum InfoOptions
{
    ShowInInform,
    Support,
    ShowInSupported,
    TypeOfValue
}

/// <summary>
/// Represents the kind of search to perform when retrieving information.
/// </summary>
[Flags]
public enum InfoFileOptions
{
    FileOption_Nothing = 0x00,
    FileOption_NoRecursive = 0x01,
    FileOption_CloseAll = 0x02,
    FileOption_Max = 0x04
};

/// <summary>
/// Represents the status of a media file.
/// </summary>
[Flags]
public enum Status
{
    None = 0x00,
    Accepted = 0x01,
    Filled = 0x02,
    Updated = 0x04,
    Finalized = 0x08,
}