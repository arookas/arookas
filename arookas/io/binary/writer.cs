using arookas.Reflection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace arookas.IO.Binary {
	public partial class aBinaryWriter : aBinary
	{
		public aBinaryWriter(Stream stream)
			: base(stream) {
			aError.CheckWrite(stream);
		}
		public aBinaryWriter(Stream stream, Endianness endianness)
			: base(stream, endianness) {
			aError.CheckWrite(stream);
		}
		public aBinaryWriter(Stream stream, Encoding encoding)
			: base(stream, encoding) {
			aError.CheckWrite(stream);
		}
		public aBinaryWriter(Stream stream, Endianness endianness, Encoding encoding)
			: base(stream, endianness, encoding) {
			aError.CheckWrite(stream);
		}

		// 8-bit
		public void WriteS8(sbyte value) {
			Write8(unchecked((byte)value));
		}
		public void WriteS8s(sbyte[] value) {
			aError.CheckNull(value, "value");
			WriteS8s(value, 0, value.Length);
		}
		public void WriteS8s(sbyte[] value, int count) {
			WriteS8s(value, 0, count);
		}
		public void WriteS8s(sbyte[] value, int offset, int count) {
			aError.CheckArrayRegion(value, "value", offset, count);
			Resize(count);
			unchecked {
				for (var i = 0; i < count; ++i) {
					mBuffer[i] = (byte)value[offset + i];
				}
			}
			Write(1, count);
		}

		public void Write8(byte value) {
			mStream.WriteByte(value);
		}
		public void Write8s(byte[] value) {
			aError.CheckNull(value, "value");
			Write8s(value, 0, value.Length);
		}
		public void Write8s(byte[] value, int count) {
			Write8s(value, 0, count);
		}
		public void Write8s(byte[] value, int offset, int count) {
			aError.CheckArrayRegion(value, "value", offset, count);
			mStream.Write(value, offset, count);
		}

		// 16-bit
		public void WriteS16(short value) {
			Write16(unchecked((ushort)value));
		}
		public void WriteS16s(short[] value) {
			aError.CheckNull(value, "value");
			Write(value, sizeof(short), 0, value.Length, BitConverter.GetBytes);
		}
		public void WriteS16s(short[] value, int count) {
			Write(value, sizeof(short), 0, count, BitConverter.GetBytes);
		}
		public void WriteS16s(short[] value, int offset, int count) {
			Write(value, sizeof(short), offset, count, BitConverter.GetBytes);
		}

		public void Write16(ushort value) {
			Write(value, sizeof(ushort), BitConverter.GetBytes);
		}
		public void Write16s(ushort[] value) {
			aError.CheckNull(value, "value");
			Write(value, sizeof(ushort), 0, value.Length, BitConverter.GetBytes);
		}
		public void Write16s(ushort[] value, int count) {
			Write(value, sizeof(ushort), 0, count, BitConverter.GetBytes);
		}
		public void Write16s(ushort[] value, int offset, int count) {
			Write(value, sizeof(ushort), offset, count, BitConverter.GetBytes);
		}

		// 32-bit
		public void WriteS32(int value) {
			Write32(unchecked((uint)value));
		}
		public void WriteS32s(int[] value) {
			aError.CheckNull(value, "value");
			WriteS32s(value, 0, value.Length);
			Write(value, sizeof(int), 0, value.Length, BitConverter.GetBytes);
		}
		public void WriteS32s(int[] value, int count) {
			WriteS32s(value, 0, count);
			Write(value, sizeof(int), 0, count, BitConverter.GetBytes);
		}
		public void WriteS32s(int[] value, int offset, int count) {
			Write(value, sizeof(int), offset, count, BitConverter.GetBytes);
		}

		public void Write32(uint value) {
			Write(value, sizeof(uint), BitConverter.GetBytes);
		}
		public void Write32s(uint[] value) {
			aError.CheckNull(value, "value");
			Write(value, sizeof(uint), 0, value.Length, BitConverter.GetBytes);
		}
		public void Write32s(uint[] value, int count) {
			Write(value, sizeof(uint), 0, count, BitConverter.GetBytes);
		}
		public void Write32s(uint[] value, int offset, int count) {
			Write(value, sizeof(uint), offset, count, BitConverter.GetBytes);
		}

		// 64-bit
		public void WriteS64(long value) {
			Write64(unchecked((ulong)value));
		}
		public void WriteS64s(long[] value) {
			aError.CheckNull(value, "value");
			Write(value, sizeof(long), 0, value.Length, BitConverter.GetBytes);
		}
		public void WriteS64s(long[] value, int count) {
			Write(value, sizeof(long), 0, count, BitConverter.GetBytes);
		}
		public void WriteS64s(long[] value, int offset, int count) {
			Write(value, sizeof(long), offset, count, BitConverter.GetBytes);
		}

		public void Write64(ulong value) {
			Write(value, sizeof(ulong), BitConverter.GetBytes);
		}
		public void Write64s(ulong[] value) {
			aError.CheckNull(value, "value");
			Write(value, sizeof(ulong), 0, value.Length, BitConverter.GetBytes);
		}
		public void Write64s(ulong[] value, int count) {
			Write(value, sizeof(ulong), 0, count, BitConverter.GetBytes);
		}
		public void Write64s(ulong[] value, int offset, int count) {
			Write(value, sizeof(ulong), offset, count, BitConverter.GetBytes);
		}

		// floating-point
		public void WriteF32(float value) {
			Write(value, sizeof(float), BitConverter.GetBytes);
		}
		public void WriteF32s(float[] value) {
			aError.CheckNull(value, "value");
			Write(value, sizeof(float), 0, value.Length, BitConverter.GetBytes);
		}
		public void WriteF32s(float[] value, int count) {
			Write(value, sizeof(float), 0, count, BitConverter.GetBytes);
		}
		public void WriteF32s(float[] value, int offset, int count) {
			Write(value, sizeof(float), offset, count, BitConverter.GetBytes);
		}

		public void WriteF64(double value) {
			Write(value, sizeof(double), BitConverter.GetBytes);
		}
		public void WriteF64s(double[] value) {
			aError.CheckNull(value, "value");
			Write(value, sizeof(double), 0, value.Length, BitConverter.GetBytes);
		}
		public void WriteF64s(double[] value, int count) {
			Write(value, sizeof(double), 0, count, BitConverter.GetBytes);
		}
		public void WriteF64s(double[] value, int offset, int count) {
			Write(value, sizeof(double), offset, count, BitConverter.GetBytes);
		}

		// chars
		public void WriteChar(char character) {
			var bytes = Encoding.GetBytes(character.ToString());
			Resize(bytes.Length);
			Array.Copy(bytes, 0, mBuffer, 0, bytes.Length);
			Write(EncodingStride, bytes.Length); // NOTE: use EncodingStride because it returns 1 for UTF-8 and S-JIS
		}
		public void WriteChars(char[] value) {
			aError.CheckNull(value, "value");
			WriteChars(value, 0, value.Length);
		}
		public void WriteChars(char[] value, int count) {
			WriteChars(value, 0, count);
		}
		public void WriteChars(char[] value, int offset, int count) {
			aError.CheckNull(value, "value");
			aError.CheckArrayRegion(value, "value", offset, count);
			for (var i = 0; i < count; ++i) {
				WriteChar(value[offset + i]);
			}
		}

		// strings
		public void WriteString(string value) {
			aError.CheckNull(value, "value");
			foreach (var i in value) {
				WriteChar(i);
			}
		}
		public void WriteString<T>(string value)
			where T : aBinaryString, new() {
			new T().Write(this, value); // fast version
		}
		public void WriteString<T>(string value, params object[] args)
			where T : aBinaryString {
			aReflection.CreateInstance<T>(args).Write(this, value); // slow version
		}
	}
}
