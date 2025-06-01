// SPDX-FileCopyrightText: 2025 meineGlock20
// SPDX-License-Identifier: MIT
// ----------------------------------------------------------------------------------------------
// This wrapper (MediaInfoWrapper) is MIT‐licensed.
// This product uses MediaInfo library, Copyright (c) 2002-2025 MediaArea.net SARL.
// ----------------------------------------------------------------------------------------------

namespace MediaInfoWrapper.Core;

internal class Convert
{
    /// <summary>
    /// Converts a string to an int.
    /// </summary>
    /// <param name="s">The string to convert.</param>
    /// <returns>The result or NULL if the conversion fails.</returns>
    public static int? StringToInt(string? s)
    {
        if (!Single.TryParse(s, out Single result))
            return null;
        else
            return (int)result;
    }

    /// <summary>
    /// Converts a string to an long.
    /// </summary>
    /// <param name="s">The string to convert.</param>
    /// <returns>The result or NULL if the conversion fails.</returns>
    public static long? StringToLong(string? s)
    {
        if (!double.TryParse(s, out double result))
            return null;
        else
            return (long)result;
    }

    /// <summary>
    /// Converts a string to a double.
    /// </summary>
    /// <param name="s">The string to convert.</param>
    /// <returns>The result or NULL if the conversion fails.</returns>
    public static double? StringToDouble(string? s)
    {
        if (!double.TryParse(s, out double result))
            return null;
        else
            return result;
    }
}
