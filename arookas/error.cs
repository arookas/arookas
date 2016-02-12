using arookas.Reflection;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;

namespace arookas {
	internal static class aError {
		[Conditional("DEBUG")]
		public static void Check<TException>(bool valid, params object[] args)
			where TException : Exception {
			if (!valid) {
				throw aReflection.CreateInstance<TException>(args);
			}
		}

		[Conditional("DEBUG")]
		public static void CheckDisposed(aDisposable obj) {
			if (obj.IsDisposed) {
				throw new ObjectDisposedException(obj.GetType().Name);
			}
		}

		[Conditional("DEBUG")]
		public static void CheckNull<T>(T value, string name)
			where T : class {
			if (value == null) {
				throw new ArgumentNullException(name);
			}
		}

		[Conditional("DEBUG")]
		public static void CheckDefined<TEnum>(TEnum value, string name)
			where TEnum : struct, IComparable, IFormattable, IConvertible {
			if (!Enum.IsDefined(typeof(TEnum), value)) {
				throw new InvalidEnumArgumentException(name, (int)(object)value, typeof(TEnum));
			}
		}

		[Conditional("DEBUG")]
		public static void CheckMin<T>(T value, string name, T min)
			where T : IComparable<T> {
			if (value.CompareTo(min) < 0) {
				throw new ArgumentOutOfRangeException(name);
			}
		}
		[Conditional("DEBUG")]
		public static void CheckMax<T>(T value, string name, T max)
			where T : IComparable<T> {
			if (value.CompareTo(max) > 0) {
				throw new ArgumentOutOfRangeException(name);
			}
		}
		[Conditional("DEBUG")]
		public static void CheckRange<T>(T value, string name, T min, T max)
			where T : IComparable<T> {
			if (value.CompareTo(min) < 0 || value.CompareTo(max) > 0) {
				throw new ArgumentOutOfRangeException(name);
			}
		}

		[Conditional("DEBUG")]
		public static void CheckArrayRegion(Array array, string name, int offset, int count) {
			aError.CheckNull(array, name);
			if (array.Length == 0 && offset == 0 && count == 0) {
				return;
			}
			if ((offset < 0 || offset >= array.Length) || (count < 0 || count > array.Length) || (offset + count > array.Length)) {
				throw new ArgumentException(String.Format("The specified region ([{0}] - [{1}]) of the array '{2}' is out-of-bounds.", offset, offset + count, name));
			}
		}
		[Conditional("DEBUG")]
		public static void CheckOffsetLength(long offset, long length, long limit) {
			if (offset < 0 || length < 0 || offset + length > limit) {
				throw new ArgumentException(String.Format("The specified region ([{0}] - [{1}]) is out-of-bounds.", offset, offset + length));
			}
		}

		[Conditional("DEBUG")]
		public static void CheckRead(Stream stream) {
			if (!stream.CanRead) {
				throw new NotSupportedException("The specified stream does not support reading.");
			}
		}
		[Conditional("DEBUG")]
		public static void CheckWrite(Stream stream) {
			if (!stream.CanWrite) {
				throw new NotSupportedException("The specified stream does not support writing.");
			}
		}
	}
}
