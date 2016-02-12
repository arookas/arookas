using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace arookas.IO.Binary {
	public abstract class aBinary {
		protected Stream mStream;
		protected Stack<Anchor> mAnchors;
		protected byte[] mBuffer;
		protected Encoding mEncoding;
		protected Endianness mEndianness;

		protected Anchor CurrentAnchor {
			get { return mAnchors.Peek(); } // stupid C# not supporting FamilyAndAssembly
		}

		protected int BufferSize {
			get { return (mBuffer == null ? 0 : mBuffer.Length); }
		}

		long AbsolutePosition {
			get { return mStream.Position; }
			set { mStream.Position = value; }
		}
		long AbsoluteLength {
			get { return mStream.Length; }
		}

		public long Position {
			get { return CurrentAnchor.GetRelative(AbsolutePosition); }
		}
		public long Length {
			get { return CurrentAnchor.End - CurrentAnchor.Start; }
		}

		long AbsoluteBytesRemaining {
			get { return (AbsoluteLength - AbsolutePosition); }
		}
		public long BytesRemaining {
			get { return (Length - Position); }
		}

		public bool IsAtBeginningOfStream {
			get { return (Position == 0); }
		}
		public bool IsAtEndOfStream {
			get { return (Position >= Length); }
		}

		public Encoding Encoding {
			get { return mEncoding; }
			set {
				aError.CheckNull(value, "value");
				mEncoding = value;
			}
		}
		protected int EncodingStride {
			get {
				switch (Encoding.WebName) {
					case "us-ascii": return 1;
					case "utf-16":
					case "utf-16BE": return 2;
					case "utf-32":
					case "utf-32BE": return 4;
				}
				return 1; // fallback
			}
		}

		public Endianness Endianness {
			get { return mEndianness; }
			set {
				aError.CheckDefined(value, "value");
				mEndianness = value;
			}
		}
		public static Endianness SystemEndianness {
			get { return (BitConverter.IsLittleEndian ? Endianness.Little : Endianness.Big); }
		}
		protected bool Reverse {
			get { return (SystemEndianness != Endianness); }
		}

		internal aBinary(Stream stream)
			: this (stream, SystemEndianness, Encoding.ASCII) { }
		internal aBinary(Stream stream, Endianness endianness)
			: this(stream, endianness, Encoding.ASCII) { }
		internal aBinary(Stream stream, Encoding encoding)
			: this(stream, SystemEndianness, encoding) { }
		internal aBinary(Stream stream, Endianness endianness, Encoding encoding) {
			aError.CheckNull(stream, "stream");
			aError.CheckDefined(endianness, "endianness");
			aError.CheckNull(encoding, "encoding");
			mStream = stream;
			mEndianness = endianness;
			mEncoding = encoding;
			mAnchors = new Stack<Anchor>(10);
			PushAnchor();
		}

		protected void Read(int stride) {
			Read(stride, 1);
		}
		protected void Read(int stride, int count) {
			var bytes = (stride * count);
			Resize(bytes);
			if (mStream.Read(mBuffer, 0, bytes) < bytes || BytesRemaining < 0) {
				throw new EndOfStreamException(String.Format("Failed to read {0} byte(s) before encountering the end of the stream.", bytes));
			}
			if (Reverse) {
				for (var i = 0; i < bytes; i += stride) {
					Array.Reverse(mBuffer, i, stride);
				}
			}
		}
		protected void Write(int stride) {
			Write(stride, 1);
		}
		protected void Write(int stride, int count) {
			var bytes = (stride * count);
			if (Reverse) {
				for (var i = 0; i < bytes; i += stride) {
					Array.Reverse(mBuffer, i, stride);
				}
			}
			mStream.Write(mBuffer, 0, bytes);
		}
		protected void Resize(int size) {
			if (BufferSize < size) {
				mBuffer = new byte[size];
			}
		}

		public void Goto(long position) {
			Seek(position, SeekOrigin.Begin);
		}
		public void Step(long position) {
			Seek(position, SeekOrigin.Current);
		}
		public void Peek(long position) {
			Seek(position, SeekOrigin.End);
		}
		public void Skip() {
			Skip(32);
		}
		public void Skip(int multiple) {
			long remainder = (Position % multiple);
			if (remainder != 0) {
				Step(multiple - remainder);
			}
		}

		void Seek(long position, SeekOrigin origin) {
			switch (origin) {
				case SeekOrigin.Begin: position = CurrentAnchor.GetAbsolute(position); break;
				case SeekOrigin.Current: position = AbsolutePosition + position; break;
				case SeekOrigin.End: position = CurrentAnchor.End - position; break;
			}
			if (position < CurrentAnchor.Start) {
				position = CurrentAnchor.Start;
			}
			else if (position > CurrentAnchor.End && !mStream.CanWrite) {
				position = CurrentAnchor.End;
			}
			mStream.Seek(position, SeekOrigin.Begin);
		}

		public void Keep() {
			CurrentAnchor.Push(AbsolutePosition);
		}
		public void Back() {
			AbsolutePosition = CurrentAnchor.Pop();
		}

		public void PushAnchor() {
			mAnchors.Push(new Anchor(mStream, AbsolutePosition));
		}
		public void PushAnchor(long length) {
			mAnchors.Push(new Anchor(mStream, AbsolutePosition, length));
		}
		public void PopAnchor() {
			if (mAnchors.Count > 1) {
				mAnchors.Pop();
			}
		}
		public void PopAnchors() {
			while (mAnchors.Count > 1) {
				mAnchors.Pop();
			}
		}

		protected T Read<T>(int stride, Func<byte[], int, T> converter) {
			aError.CheckNull(converter, "converter");
			Read(stride);
			return converter(mBuffer, 0);
		}
		protected T[] Read<T>(int stride, int count, Func<byte[], int, T> converter) {
			aError.CheckNull(converter, "converter");
			aError.CheckMin(count, "count", 0);
			T[] array = new T[count];
			Read(stride, count);
			for (var i = 0; i < count; ++i) {
				array[i] = converter(mBuffer, i * stride);
			}
			return array;
		}

		protected void Write<T>(T value, int stride, Func<T, byte[]> converter) {
			aError.CheckNull(converter, "converter");
			Resize(stride);
			Array.Copy(converter(value), 0, mBuffer, 0, stride);
			Write(stride);
		}
		protected void Write<T>(T[] value, int stride, int offset, int count, Func<T, byte[]> converter) {
			aError.CheckNull(converter, "converter");
			aError.CheckArrayRegion(value, "value", offset, count);
			Resize(count * stride);
			for (var i = 0; i < count; ++i) {
				Array.Copy(converter(value[offset + i]), 0, mBuffer, i * stride, stride);
			}
			Write(stride, count);
		}
		protected void Write<T>(T[] value, int stride, int offset, int count, Func<T[], int, int, byte[]> converter) {
			aError.CheckNull(converter, "converter");
			aError.CheckArrayRegion(value, "value", offset, count);
			Resize(count * stride);
			Array.Copy(converter(value, offset, count), 0, mBuffer, 0, count * stride);
			Write(stride, count);
		}

		protected sealed class Anchor {
			Stack<long> mJumps;
			Stream mStream;
			long? mLength;
			long mStart;

			public long Start {
				get { return mStart; }
			}
			public long Length {
				get { return mLength ?? mStream.Length; }
			}
			public long End {
				get { return Start + Length; }
			}

			public int Count {
				get { return mJumps.Count; }
			}

			public Anchor(Stream stream, long start) {
				Init(stream, start, null);
			}
			public Anchor(Stream stream, long start, long length) {
				Init(stream, start, length);
			}

			void Init(Stream stream, long offset, long? length) {
				aError.CheckNull(stream, "stream");
				aError.CheckOffsetLength(offset, length ?? 0, stream.Length);
				mStart = offset;
				mLength = length;
				mStream = stream;
				mJumps = new Stack<long>(10);
			}

			public long GetAbsolute(long position) {
				return position + Start;
			}
			public long GetRelative(long position) {
				return position - Start;
			}

			public void Push(long position) {
				mJumps.Push(position);
			}
			public long Pop() {
				return Count > 0 ? mJumps.Pop() : 0;
			}
			public long Peek() {
				return Count > 0 ? mJumps.Peek() : 0;
			}

#if DEBUG
			public override string ToString() {
				return String.Format("Offset: {0}, Jumps: {1}", Start, Count);
			}
#endif
		}
	}

	public enum Endianness {
		Little,
		Big,
	}
}
