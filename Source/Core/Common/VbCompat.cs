using System;
using System.Collections.Generic;
using System.Globalization;

// Lightweight compatibility shims to remove Microsoft.VisualBasic dependency.
// Intent: unblock compilation while we migrate call sites to idiomatic C#.
// Note: VB functions are 1-based where applicable; we mirror that behavior.

public static class Strings
{
	public static int Len(string? s) => s?.Length ?? 0;

	public static string Space(int count)
	{
		if (count <= 0) return string.Empty;
		return new string(' ', count);
	}

	public static string Left(string? s, int length)
	{
		if (string.IsNullOrEmpty(s) || length <= 0) return string.Empty;
		if (length >= s!.Length) return s;
		return s.Substring(0, length);
	}

	public static string Right(string? s, int length)
	{
		if (string.IsNullOrEmpty(s) || length <= 0) return string.Empty;
		if (length >= s!.Length) return s;
		return s.Substring(s.Length - length);
	}

	// VB Mid is 1-based; start at 1 means first character
	public static string Mid(string? s, int start, int length)
	{
		if (string.IsNullOrEmpty(s)) return string.Empty;
		if (start <= 0) start = 1;
		int zeroStart = start - 1;
		if (zeroStart >= s!.Length) return string.Empty;
		if (length <= 0) return string.Empty;
		int maxLen = Math.Min(length, s.Length - zeroStart);
		return s.Substring(zeroStart, maxLen);
	}

	public static string Mid(string? s, int start)
	{
		if (string.IsNullOrEmpty(s)) return string.Empty;
		if (start <= 0) start = 1;
		int zeroStart = start - 1;
		if (zeroStart >= s!.Length) return string.Empty;
		return s.Substring(zeroStart);
	}

	// Returns 1-based index, 0 if not found (VB semantics)
	public static int InStr(string? string1, string? string2)
	{
		if (string1 == null || string2 == null) return 0;
		int idx = string1.IndexOf(string2, StringComparison.Ordinal);
		return idx < 0 ? 0 : idx + 1;
	}

	public static int InStr(int start, string? string1, string? string2)
	{
		if (string1 == null || string2 == null) return 0;
		if (start <= 0) start = 1;
		int zeroStart = Math.Min(Math.Max(start - 1, 0), string1.Length);
		int idx = string1.IndexOf(string2, zeroStart, StringComparison.Ordinal);
		return idx < 0 ? 0 : idx + 1;
	}

	public static string Replace(string? expression, string? find, string? replacement)
	{
		if (string.IsNullOrEmpty(expression) || string.IsNullOrEmpty(find)) return expression ?? string.Empty;
		return expression!.Replace(find!, replacement ?? string.Empty);
	}

	public static string UCase(string? s) => (s ?? string.Empty).ToUpperInvariant();
	public static string LCase(string? s) => (s ?? string.Empty).ToLowerInvariant();
	public static string Trim(string? s) => (s ?? string.Empty).Trim();
	public static int AscW(char c) => c;
	public static char ChrW(int code) => Convert.ToChar(code);
	public static string[] Split(string? s, string? delimiter)
	{
		if (s == null) return Array.Empty<string>();
		if (string.IsNullOrEmpty(delimiter)) return new[] { s };
		return s.Split(new[] { delimiter! }, StringSplitOptions.None);
	}
	public static string Join(string[]? arr, string? delimiter) => string.Join(delimiter ?? string.Empty, arr ?? Array.Empty<string>());
	public static string StrReverse(string? s)
	{
		if (string.IsNullOrEmpty(s)) return string.Empty;
		var chars = s!.ToCharArray();
		Array.Reverse(chars);
		return new string(chars);
	}

	public static string Format(object? expression, string? format)
	{
		if (expression == null) return string.Empty;
		if (string.IsNullOrEmpty(format)) return Conversions.ToString(expression);
		try
		{
			// Try numeric formatting first
			return format!.Contains("#") || format.Contains("0")
				? Convert.ToDecimal(expression, CultureInfo.InvariantCulture).ToString(format, CultureInfo.InvariantCulture)
				: string.Format(CultureInfo.InvariantCulture, "{0:" + format + "}", expression);
		}
		catch
		{
			try { return string.Format(CultureInfo.InvariantCulture, "{0:" + format + "}", expression); }
			catch { return Conversions.ToString(expression); }
		}
	}
}

public static class Information
{
	public static int UBound(Array? array, int dimension = 1)
	{
		if (array == null) return -1;
		dimension = Math.Max(1, dimension) - 1;
		return array.GetUpperBound(dimension);
	}

	public static int LBound(Array? array, int dimension = 1)
	{
		if (array == null) return 0;
		dimension = Math.Max(1, dimension) - 1;
		return array.GetLowerBound(dimension);
	}

	public static bool IsNumeric(object? value)
	{
		if (value == null) return false;
		switch (value)
		{
			case sbyte or byte or short or ushort or int or uint or long or ulong or float or double or decimal:
				return true;
			case string s:
				return double.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out _);
			default:
				return double.TryParse(Convert.ToString(value, CultureInfo.InvariantCulture), NumberStyles.Any, CultureInfo.InvariantCulture, out _);
		}
	}
}

public static class Conversions
{
	public static string ToString(object? value) => Convert.ToString(value, CultureInfo.InvariantCulture) ?? string.Empty;

	public static long ToLongFloor(double value)
	{
		// Helper for VB Int behavior (floor toward -infinity)
		return (long)Math.Floor(value);
	}
	public static int ToInteger(object? value)
	{
		if (value == null) return 0;
		if (value is bool b) return b ? -1 : 0; // VB True=-1
		if (value is IConvertible conv)
		{
			try { return conv.ToInt32(CultureInfo.InvariantCulture); } catch { }
		}
		if (int.TryParse(ToString(value), NumberStyles.Any, CultureInfo.InvariantCulture, out var i)) return i;
		if (double.TryParse(ToString(value), NumberStyles.Any, CultureInfo.InvariantCulture, out var d)) return (int)d;
		return 0;
	}
	public static bool ToBoolean(object? value)
	{
		if (value == null) return false;
		if (value is bool b) return b;
		if (value is string s)
		{
			if (bool.TryParse(s, out var bt)) return bt;
			if (double.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out var dn)) return dn != 0.0;
		}
		try { return Convert.ToDouble(value, CultureInfo.InvariantCulture) != 0.0; } catch { return false; }
	}
	public static byte ToByte(object? value)
	{
		if (value == null) return 0;
		try { return Convert.ToByte(value, CultureInfo.InvariantCulture); } catch { return 0; }
	}
	public static short ToShort(object? value)
	{
		if (value == null) return 0;
		try { return Convert.ToInt16(value, CultureInfo.InvariantCulture); } catch { return 0; }
	}
	public static long ToLong(object? value)
	{
		if (value == null) return 0L;
		try { return Convert.ToInt64(value, CultureInfo.InvariantCulture); } catch { return 0L; }
	}
	public static float ToSingle(object? value)
	{
		if (value == null) return 0f;
		try { return Convert.ToSingle(value, CultureInfo.InvariantCulture); } catch { return 0f; }
	}
	public static double ToDouble(object? value)
	{
		if (value == null) return 0d;
		try { return Convert.ToDouble(value, CultureInfo.InvariantCulture); } catch { return 0d; }
	}
}

// Some code references singular `Conversion` too; map to Conversions.
public static class Conversion
{
	public static string ToString(object? v) => Conversions.ToString(v);
	public static int ToInteger(object? v) => Conversions.ToInteger(v);
	public static bool ToBoolean(object? v) => Conversions.ToBoolean(v);
	public static byte ToByte(object? v) => Conversions.ToByte(v);
	public static short ToShort(object? v) => Conversions.ToShort(v);
	public static long ToLong(object? v) => Conversions.ToLong(v);
	public static float ToSingle(object? v) => Conversions.ToSingle(v);
	public static double ToDouble(object? v) => Conversions.ToDouble(v);

	// VB Int function: returns the greatest integer less than or equal to a number
	public static int Int(double number) => (int)Math.Floor(number);
	public static int Int(object? value)
	{
		if (value == null) return 0;
		if (value is double d) return (int)Math.Floor(d);
		if (double.TryParse(Conversions.ToString(value), NumberStyles.Any, CultureInfo.InvariantCulture, out var dd))
			return (int)Math.Floor(dd);
		return 0;
	}

	// VB Str function: converts number to string with leading space for positives
	public static string Str(double number)
	{
		var s = number.ToString(CultureInfo.InvariantCulture);
		if (number >= 0) return " " + s;
		return s;
	}
	public static string Str(object? value)
	{
		if (value == null) return string.Empty;
		if (value is IFormattable f)
		{
			var s = f.ToString(null, CultureInfo.InvariantCulture);
			// Try to determine sign
			if (double.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out var d))
				return d >= 0 ? " " + s : s;
			return s;
		}
		return Conversions.ToString(value);
	}
	public static double Val(string? s)
	{
		if (string.IsNullOrEmpty(s)) return 0d;
		// Parse leading numeric portion like VB.Val
		s = s!.TrimStart();
		int i = 0;
		bool seenAny = false;
		while (i < s.Length && (char.IsDigit(s[i]) || s[i] == '+' || s[i] == '-' || s[i] == '.' || s[i] == 'e' || s[i] == 'E'))
		{
			seenAny = true;
			i++;
		}
		if (!seenAny) return 0d;
		var num = s.Substring(0, i);
		return double.TryParse(num, NumberStyles.Any, CultureInfo.InvariantCulture, out var d) ? d : 0d;
	}
}

public static class Interaction
{
	public static int MsgBox(string? prompt)
	{
		Console.WriteLine(prompt ?? string.Empty);
		return 0;
	}
	public static string InputBox(string prompt, string title = "", string defaultResponse = "")
	{
		// Non-interactive stub; returns defaultResponse.
		return defaultResponse;
	}
}
