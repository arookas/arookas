using arookas.Collections;
using System;
using System.Drawing;
using System.Globalization;
using System.Runtime.InteropServices;

namespace arookas {
	[StructLayout(LayoutKind.Sequential)]
	public struct aRGBA {
		public byte r;
		public byte g;
		public byte b;
		public byte a;
		
		public uint RGB {
			get { return unchecked((uint)((r << 16) | (g << 8) | b)); }
		}
		public uint ARGB {
			get { return unchecked((uint)((a << 24) | (r << 16) | (g << 8) | b)); }
		}
		public uint RGBA {
			get { return unchecked((uint)((r << 24) | (g << 16) | (b << 8) | (a))); }
		}

		public aRGBA Inverse {
			get { return new aRGBA(255 - r, 255 - g, 255 - b, a); }
		}

		public static aRGBA Black {
			get { return new aRGBA(0, 0, 0); }
		}
		public static aRGBA Blue {
			get { return new aRGBA(0, 0, 255); }
		}
		public static aRGBA Cyan {
			get { return new aRGBA(0, 255, 255); }
		}
		public static aRGBA Green {
			get { return new aRGBA(0, 255, 0); }
		}
		public static aRGBA Red {
			get { return new aRGBA(255, 0, 0); }
		}
		public static aRGBA White {
			get { return new aRGBA(255); }
		}
		public static aRGBA Yellow {
			get { return new aRGBA(255, 255, 0); }
		}
		public static aRGBA Magenta {
			get { return new aRGBA(255, 0, 255); }
		}

		static int[] sLookup2Bit;
		static int[] sLookup3Bit;
		static int[] sLookup4Bit;
		static int[] sLookup5Bit;
		static int[] sLookup6Bit;
		static int[] sLookup7Bit;

		static aRGBA() {
			sLookup2Bit = aCollection.Initialize(4, i => (int)(255.0d / 3.0d * i));
			sLookup3Bit = aCollection.Initialize(8, i => (int)(255.0d / 7.0d * i));
			sLookup4Bit = aCollection.Initialize(16, i => (int)(255.0d / 15.0d * i));
			sLookup5Bit = aCollection.Initialize(32, i => (int)(255.0d / 31.0d * i));
			sLookup6Bit = aCollection.Initialize(64, i => (int)(255.0d / 63.0d * i));
			sLookup7Bit = aCollection.Initialize(128, i => (int)(255.0d / 127.0d * i));
		}

		public const int cOpaque = 255;

		public aRGBA(int intensity)
			: this(intensity, intensity, intensity, intensity) { }
		public aRGBA(int intensity, int alpha)
			: this(intensity, intensity, intensity, alpha) { }
		public aRGBA(int red, int green, int blue)
			: this(red, green, blue, cOpaque) { }
		public aRGBA(aRGBA color)
			: this(color.r, color.g, color.b, color.a) { }
		public aRGBA(aRGBA color, int alpha)
			: this(color.r, color.g, color.b, alpha) { }
		public aRGBA(Color color)
			: this(color.R, color.G, color.B, color.A) { }
		public aRGBA(Color color, int alpha)
			: this(color.R, color.G, color.B, alpha) { }
		public aRGBA(int red, int green, int blue, int alpha) {
			aError.CheckRange(red, "red", 0, 255);
			aError.CheckRange(green, "green", 0, 255);
			aError.CheckRange(blue, "blue", 0, 255);
			aError.CheckRange(alpha, "alpha", 0, 255);
			r = (byte)red;
			g = (byte)green;
			b = (byte)blue;
			a = (byte)alpha;
		}

		public override bool Equals(object obj) {
			if (obj is aRGBA) {
				return (this == (aRGBA)obj);
			}
			return false;
		}
		public override int GetHashCode() {
			return unchecked((int)(RGBA));
		}
		public override string ToString() {
			return RGBA.ToString("X8");
		}

		static int FromLookup(int value, int bits) {
			switch (bits) {
				case 1: return value != 0 ? 255 : 0;
				case 2: return sLookup2Bit[value];
				case 3: return sLookup3Bit[value];
				case 4: return sLookup4Bit[value];
				case 5: return sLookup5Bit[value];
				case 6: return sLookup6Bit[value];
				case 7: return sLookup7Bit[value];
				case 8: return value;
			}
			return 255;
		}

		public static aRGBA FromARGB8(uint argb8) {
			return new aRGBA(
				(int)((argb8 >> 16) & 0xFF),
				(int)((argb8 >> 8) & 0xFF),
				(int)(argb8 & 0xFF),
				(int)((argb8 >> 24) & 0xFF)
			);
		}
		public static aRGBA FromRGB4A3(ushort rgb4a3) {
			return new aRGBA(
				sLookup4Bit[(rgb4a3 >> 8) & 0xF],
				sLookup4Bit[(rgb4a3 >> 4) & 0xF],
				sLookup4Bit[rgb4a3 & 0xF],
				sLookup3Bit[(rgb4a3 >> 12) & 0x7]
			);
		}
		public static aRGBA FromRGB5(ushort rgb5) {
			return new aRGBA(
				sLookup5Bit[(rgb5 >> 10) & 0x1F],
				sLookup5Bit[(rgb5 >> 5) & 0x1F],
				sLookup5Bit[rgb5 & 0x1F]
			);
		}
		public static aRGBA FromRGB5A1(ushort rgb565a1) {
			return new aRGBA(
				sLookup5Bit[(rgb565a1 >> 10) & 0x1F],
				sLookup5Bit[(rgb565a1 >> 5) & 0x1F],
				sLookup5Bit[rgb565a1 & 0x1F],
				255 * (rgb565a1 >> 15)
			);
		}
		public static aRGBA FromRGB565(ushort rgb565) {
			return new aRGBA(
				sLookup5Bit[(rgb565 >> 11) & 0x1F],
				sLookup6Bit[(rgb565 >> 5) & 0x3F],
				sLookup5Bit[rgb565 & 0x1F]
			);
		}
		public static aRGBA FromRGB8(uint rgb8) {
			return new aRGBA(
				(int)((rgb8 & 0xFF0000) >> 16),
				(int)((rgb8 & 0xFF00) >> 8),
				(int)(rgb8 & 0xFF)
			);
		}
		public static aRGBA FromRGBA8(uint rgba8) {
			return new aRGBA(
				(int)((rgba8 >> 24) & 0xFF),
				(int)((rgba8 >> 16) & 0xFF),
				(int)((rgba8 >> 8) & 0xFF),
				(int)(rgba8 & 0xFF)
			);
		}
		public static aRGBA FromRGBA4(ushort rgba4) {
			return new aRGBA(
				sLookup4Bit[(rgba4 >> 8) & 0xF],
				sLookup4Bit[(rgba4 >> 4) & 0xF],
				sLookup4Bit[rgba4 & 0xF],
				sLookup4Bit[rgba4 & 0xF]
			);
		}
		public static aRGBA FromRGBA6(uint rgba6) {
			return new aRGBA(
				sLookup6Bit[(rgba6 >> 18) & 0x3F],
				sLookup6Bit[(rgba6 >> 12) & 0x3F],
				sLookup6Bit[(rgba6 & 6) & 0x3F],
				sLookup6Bit[rgba6 & 0x3F]
			);
		}
		public static aRGBA FromRGBA(uint color, int rbits, int gbits, int bbits, int abits) {
			aError.CheckRange(rbits, "rbits", 0, 8);
			aError.CheckRange(gbits, "gbits", 0, 8);
			aError.CheckRange(bbits, "bbits", 0, 8);
			aError.CheckRange(abits, "abits", 0, 8);
			return new aRGBA(
				FromLookup((int)(color >> (abits + bbits + gbits)) & ((1 << rbits) - 1), rbits),
				FromLookup((int)(color >> (abits + bbits)) & ((1 << gbits) - 1), gbits),
				FromLookup((int)(color >> (abits)) & ((1 << bbits) - 1), bbits),
				FromLookup((int)(color & ((1 << abits) - 1)), abits)
			);
		}
		public static aRGBA[] FromST3C1(ulong st3c1) {
			var colors = new aRGBA[4];
			colors[0] = FromRGB565((ushort)((st3c1 >> 48) & 0xFFFF));
			colors[1] = FromRGB565((ushort)((st3c1 >> 32) & 0xFFFF));
			if (colors[0].ARGB > colors[1].ARGB) {
				colors[2] = Lerp(colors[0], colors[1], 0.333333f);
				colors[3] = Lerp(colors[0], colors[1], 0.666666f);
			}
			else {
				colors[2] = Lerp(colors[0], colors[1], 0.5f);
				colors[3] = new aRGBA(0, 0);
			}
			var data = new aRGBA[16];
			var bits = 30;
			for (var y = 0; y < 4; y++) {
				for (var x = 0; x < 4; x++) {
					data[4 * y + x] = colors[(st3c1 >> bits) & 0x3];
					bits -= 2;
				}
			}
			return data;
		}

		public static aRGBA Lerp(aRGBA from, aRGBA to, float percent)
		{
			return new aRGBA
			(
				(int)(from.r + (to.r - from.r) * percent),
				(int)(from.g + (to.g - from.g) * percent),
				(int)(from.b + (to.b - from.b) * percent),
				(int)(from.a + (to.a - from.a) * percent)
			);
		}

		public static aRGBA Parse(string colorString) {
			return Parse(colorString, "RGBA");
		}
		public static aRGBA Parse(string colorString, string format) {
			aError.CheckNull(colorString, "colorString");
			aError.CheckNull(colorString, "format");
			var index = 0;
			var r = 0;
			var g = 0;
			var b = 0;
			var a = cOpaque;
#if DEBUG
			var rr = false;
			var gg = false;
			var bb = false;
			var aa = false;
#endif
			foreach (var c in format) {
				aError.Check<ArgumentException>(colorString.Length >= index + 2, "The specified color string was not long enough for the specified format.", "colorString");
				switch (c) {
					case 'R':
					case 'r': {
#if DEBUG
						aError.Check<FormatException>(!rr, "The specified format string has more than one red component.");
						rr = true;
#endif
						r = Int32.Parse(colorString.Substring(index, 2), NumberStyles.AllowHexSpecifier);
						break;
					}
					case 'G':
					case 'g': {
#if DEBUG
						aError.Check<FormatException>(!gg, "The specified format string has more than one green component.");
						gg = true;
#endif
						g = Int32.Parse(colorString.Substring(index, 2), NumberStyles.AllowHexSpecifier);
						break;
					}
					case 'B':
					case 'b': {
#if DEBUG
						aError.Check<FormatException>(!bb, "The specified format string has more than one blue component.");
						bb = true;
#endif
						b = Int32.Parse(colorString.Substring(index, 2), NumberStyles.AllowHexSpecifier);
						break;
					}
					case 'A':
					case 'a': {
#if DEBUG
						aError.Check<FormatException>(!aa, "The specified format string has more than one alpha component.");
						aa = true;
#endif
						a = Int32.Parse(colorString.Substring(index, 2), NumberStyles.AllowHexSpecifier);
						break;
					}
#if DEBUG
					default: {
						throw new FormatException(String.Format("Invalid character '{0}' in color format string.", c));
					}
#endif
				}
				index += 2;
			}
			return new aRGBA(r, g, b, a);
		}

		public static aRGBA operator /(aRGBA color, int divisor)
		{
			return new aRGBA(color.r / divisor, color.g / divisor, color.b / divisor, color.a / divisor);
		}
		public static aRGBA operator /(aRGBA color, float divisor)
		{
			return new aRGBA((int)(color.r / divisor), (int)(color.g / divisor), (int)(color.b / divisor), (int)(color.a / divisor));
		}
		public static aRGBA operator *(aRGBA color, int multiplier)
		{
			return new aRGBA(color.r * multiplier, color.g * multiplier, color.b * multiplier, color.a * multiplier);
		}
		public static aRGBA operator *(aRGBA color, float multiplier)
		{
			return new aRGBA((int)(color.r * multiplier), (int)(color.g * multiplier), (int)(color.b * multiplier), (int)(color.a * multiplier));
		}
		public static aRGBA operator +(aRGBA color, int addend)
		{
			return new aRGBA(color.r + addend, color.g + addend, color.b + addend, color.a + addend);
		}
		public static aRGBA operator -(aRGBA color, int subtrahend)
		{
			return new aRGBA(color.r - subtrahend, color.g - subtrahend, color.b - subtrahend, color.a - subtrahend);
		}

		public static aRGBA operator -(aRGBA color)
		{
			return color.Inverse;
		}

		public static bool operator ==(aRGBA lhs, aRGBA rhs) {
			return (lhs.r == rhs.r && lhs.g == rhs.g && lhs.b == rhs.b && lhs.a == rhs.a);
		}
		public static bool operator !=(aRGBA lhs, aRGBA rhs) {
			return !(lhs == rhs);
		}

		public static implicit operator Color(aRGBA color)
		{
			return System.Drawing.Color.FromArgb(color.a, color.r, color.g, color.b);
		}
		public static implicit operator aRGBA(Color color)
		{
			return new aRGBA(color.R, color.G, color.B, color.A);
		}
	}
}
