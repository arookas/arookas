using arookas.Collections;
using arookas.Reflection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace arookas.IO.Binary {
	public partial class aBinaryReader : aBinary {
		public aBinaryReader(Stream stream)
			: base(stream) {
			aError.CheckRead(stream);
		}
		public aBinaryReader(Stream stream, Endianness endianness)
			: base(stream, endianness) {
			aError.CheckRead(stream);
		}
		public aBinaryReader(Stream stream, Encoding encoding)
			: base(stream, encoding) {
			aError.CheckRead(stream);
		}
		public aBinaryReader(Stream stream, Endianness endianness, Encoding encoding)
			: base(stream, endianness, encoding) {
			aError.CheckRead(stream);
		}

		// raw
		public int Read(byte[] buffer) {
			aError.CheckNull(buffer, "buffer");
			return mStream.Read(buffer, 0, buffer.Length);
		}
		public int Read(byte[] buffer, int count) {
			return Read(buffer, 0, count);
		}
		public int Read(byte[] buffer, int startingIndex, int count) {
			aError.CheckNull(buffer, "buffer");
			aError.CheckRange(startingIndex, "startingIndex", 0, buffer.Length - 1);
			aError.CheckRange(count, "count", 0, buffer.Length);
			aError.Check<ArgumentException>(startingIndex + count <= buffer.Length, "The sum of the specified starting index and count is larger than the buffer size.");
			return mStream.Read(buffer, startingIndex, count);
		}

		// 8-bit
		public sbyte ReadS8() {
			return unchecked((sbyte)Read8());
		}
		public byte Read8() {
			var read = mStream.ReadByte();
			aError.Check<EndOfStreamException>(read >= 0);
			return (byte)read;
		}
		public sbyte[] ReadS8s(int count) {
			Read(1, count);
			return aCollection.Initialize(count, index => unchecked((sbyte)mBuffer[index]));
		}
		public byte[] Read8s(int count) {
			aError.CheckMin(count, "count", 0);
			var buffer = new byte[count];
			var read = Read(buffer);
			aError.Check<EndOfStreamException>(read >= count);
			return buffer;
		}

		// 16-bit
		public short ReadS16() {
			return Read(sizeof(short), BitConverter.ToInt16);
		}
		public ushort Read16() {
			return Read(sizeof(ushort), BitConverter.ToUInt16);
		}
		public short[] ReadS16s(int count) {
			return Read(sizeof(short), count, BitConverter.ToInt16);
		}
		public ushort[] Read16s(int count) {
			return Read(sizeof(ushort), count, BitConverter.ToUInt16);
		}

		// 32-bit
		public int ReadS32() {
			return Read(sizeof(int), BitConverter.ToInt32);
		}
		public uint Read32() {
			return Read(sizeof(uint), BitConverter.ToUInt32);
		}
		public int[] ReadS32s(int count) {
			return Read(sizeof(int), count, BitConverter.ToInt32);
		}
		public uint[] Read32s(int count) {
			return Read(sizeof(uint), count, BitConverter.ToUInt32);
		}

		// 64-bit
		public long ReadS64() {
			return Read(sizeof(long), BitConverter.ToInt64);
		}
		public ulong Read64() {
			return Read(sizeof(ulong), BitConverter.ToUInt64);
		}
		public long[] ReadS64s(int count) {
			return Read(sizeof(long), count, BitConverter.ToInt64);
		}
		public ulong[] Read64s(int count) {
			return Read(sizeof(ulong), count, BitConverter.ToUInt64);
		}

		// floating-point
		public float ReadF32() {
			return Read(sizeof(float), BitConverter.ToSingle);
		}
		public double ReadF64() {
			return Read(sizeof(double), BitConverter.ToDouble);
		}
		public float[] ReadF32s(int count) {
			return Read(sizeof(float), count, BitConverter.ToSingle);
		}
		public double[] ReadF64s(int count) {
			return Read(sizeof(double), count, BitConverter.ToDouble);
		}

		// char
		public char ReadChar() {
			if (Encoding == Encoding.UTF8) { // NOTE: UTF-8 needs special handling for multi-byte characters
				var leading = Read8();
				var count = GetUTF8CharByteCount(leading);
				if (count == 0) {
					return (char)leading;
				}
				var c = unchecked((uint)(leading & ((1 << (6 - count)) - 1)));
				for (var i = 0; i < count; ++i) {
					c <<= 6;
					c |= unchecked((uint)(Read8() & 0x3F));
				}
				return (char)c;
			}
			else if (Encoding.CodePage == 932) { // NOTE: S-JIS needs special handling for multi-byte characters
				var chars = new byte[2];
				chars[0] = Read8();
				if ((chars[0] >= 0x81 && chars[0] <= 0x9F) || (chars[0] >= 0xE0 && chars[0] <= 0xEF)) {
					chars[1] = Read8();
					return Encoding.GetChars(chars, 0, 2)[0];
				}
				return Encoding.GetChars(chars, 0, 1)[0];
			}
			var stride = EncodingStride;
			Read(stride);
			return Encoding.GetChars(mBuffer, 0, stride)[0];
		}
		public char[] ReadChars(int count) {
			if (Encoding == Encoding.UTF8 || Encoding.CodePage == 932) { // NOTE: UTF-8 and S-JIS needs special handling for multi-byte characters
				return aCollection.Initialize(count, () => ReadChar());
			}
			var stride = EncodingStride;
			Read(stride, count);
			return Encoding.GetChars(mBuffer, 0, stride * count);
		}

		static int GetUTF8CharByteCount(byte leading) {
			if ((leading & 0x80) == 0) {
				return 0;
			}
			var count = 0;
			while (((leading <<= 1) & 0x80) != 0) {
				++count;
			}
			return count;
		}

		// strings
		public string ReadString(int length) {
			aError.CheckMin(length, "length", 0);
			return new String(ReadChars(length));
		}
		public string ReadString<T>()
			where T : aBinaryString, new() {
			return new T().Read(this); // faster version
		}
		public string ReadString<T>(params object[] args)
			where T : aBinaryString {
			return aReflection.CreateInstance<T>(args).Read(this); // slow version
		}

		public string[] ReadStrings(int count, int length) {
			aError.CheckMin(count, "count", 0);
			return aCollection.Initialize(count, () => ReadString(length));
		}
		public string[] ReadStrings<T>(int count)
			where T : aBinaryString, new() {
			aError.CheckMin(count, "count", 0);
			var str = new T();
			return aCollection.Initialize(count, () => str.Read(this));
		}
		public string[] ReadStrings<T>(int count, params object[] args)
			where T : aBinaryString {
			aError.CheckMin(count, "count", 0);
			var str = aReflection.CreateInstance<T>(args);
			return aCollection.Initialize(count, () => str.Read(this));
		}
	}
}
