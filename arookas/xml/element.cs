using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace arookas.Xml {
	public sealed class xElement : xContainer {
		XElement mElement;

		internal XElement XElement {
			get { return mElement; }
		}

		public string Name {
			get { return XElement.Name.LocalName; }
		}
		public string Value {
			get { return XElement.Value; }
		}
		public int LineNumber {
			get {
				var lineinfo = (mElement as IXmlLineInfo);

				if (!lineinfo.HasLineInfo()) {
					return -1;
				}

				return lineinfo.LineNumber;
			}
		}
		public int LinePosition {
			get {
				var lineinfo = (mElement as IXmlLineInfo);

				if (!lineinfo.HasLineInfo()) {
					return -1;
				}

				return lineinfo.LinePosition;
			}
		}

		public xAttribute FirstAttribute {
			get { return new xAttribute(_Document, this, XElement.FirstAttribute); }
		}
		public xAttribute LastAttribute {
			get { return new xAttribute(_Document, this, XElement.LastAttribute); }
		}

		public bool HasAttributes {
			get { return XElement.HasAttributes; }
		}

		internal xElement(xDocument document, xElement parent, XElement element)
			: base(document, parent, element) {
			mElement = element;
		}

		public static implicit operator bool(xElement element) {
			return element | default(bool);
		}
		public static implicit operator byte(xElement element) {
			return element | default(byte);
		}
		public static implicit operator sbyte(xElement element) {
			return element | default(sbyte);
		}
		public static implicit operator short(xElement element) {
			return element | default(short);
		}
		public static implicit operator ushort(xElement element) {
			return element | default(ushort);
		}
		public static implicit operator int(xElement element) {
			return element | default(int);
		}
		public static implicit operator uint(xElement element) {
			return element | default(uint);
		}
		public static implicit operator long(xElement element) {
			return element | default(long);
		}
		public static implicit operator ulong(xElement element) {
			return element | default(ulong);
		}
		public static implicit operator float(xElement element) {
			return element | default(float);
		}
		public static implicit operator double(xElement element) {
			return element | default(double);
		}
		public static implicit operator decimal(xElement element) {
			return element | default(decimal);
		}
		public static implicit operator string(xElement element) {
			return element | default(string);
		}
		public static implicit operator Guid(xElement element) {
			return element | Guid.NewGuid(); // default(Guid) is pretty useless
		}

		public static bool operator |(xElement element, bool defaultValue) {
			return Convert(element, bool.TryParse, defaultValue);
		}
		public static byte operator |(xElement element, byte defaultValue) {
			return Convert(element, byte.TryParse, defaultValue);
		}
		public static sbyte operator |(xElement element, sbyte defaultValue) {
			return Convert(element, sbyte.TryParse, defaultValue);
		}
		public static short operator |(xElement element, short defaultValue) {
			return Convert(element, short.TryParse, defaultValue);
		}
		public static ushort operator |(xElement element, ushort defaultValue) {
			return Convert(element, ushort.TryParse, defaultValue);
		}
		public static int operator |(xElement element, int defaultValue) {
			return Convert(element, int.TryParse, defaultValue);
		}
		public static uint operator |(xElement element, uint defaultValue) {
			return Convert(element, uint.TryParse, defaultValue);
		}
		public static long operator |(xElement element, long defaultValue) {
			return Convert(element, long.TryParse, defaultValue);
		}
		public static ulong operator |(xElement element, ulong defaultValue) {
			return Convert(element, ulong.TryParse, defaultValue);
		}
		public static float operator |(xElement element, float defaultValue) {
			return Convert(element, float.TryParse, defaultValue);
		}
		public static double operator |(xElement element, double defaultValue) {
			return Convert(element, double.TryParse, defaultValue);
		}
		public static decimal operator |(xElement element, decimal defaultValue) {
			return Convert(element, decimal.TryParse, defaultValue);
		}
		public static string operator |(xElement element, string defaultValue) {
			return element != null ? element.Value : defaultValue;
		}
		public static Guid operator |(xElement element, Guid defaultValue) {
			return Convert(element, Guid.TryParse, defaultValue);
		}

		internal static T Convert<T>(xElement element, TryParse<T> parser, T defaultValue) {
			T value;
			if (element != null && parser(element.Value, out value)) {
				return value;
			}
			return defaultValue;
		}

		public override bool Equals(object obj) {
			var element = (obj as xElement);
			return (element != null) && (element == this);
		}
		public override int GetHashCode() {
			return XElement.GetHashCode();
		}
		public override string ToString() {
			return XElement.ToString();
		}

		public static bool operator ==(xElement a, xElement b)
		{
			return (object.ReferenceEquals(a, null) ? null : a.XElement) == (object.ReferenceEquals(b, null) ? null : b.XElement);
		}
		public static bool operator !=(xElement a, xElement b) {
			return !(a == b);
		}
	}

	public sealed class xElements : IEnumerable<xElement> {
		IEnumerable<xElement> mCollection;

		internal xElements(IEnumerable<xElement> collection) {
			mCollection = collection;
		}

		public static implicit operator bool[](xElements collection) {
			return collection | default(bool);
		}
		public static implicit operator byte[](xElements collection) {
			return collection | default(byte);
		}
		public static implicit operator sbyte[](xElements collection) {
			return collection | default(sbyte);
		}
		public static implicit operator short[](xElements collection) {
			return collection | default(short);
		}
		public static implicit operator ushort[](xElements collection) {
			return collection | default(ushort);
		}
		public static implicit operator int[](xElements collection) {
			return collection | default(int);
		}
		public static implicit operator uint[](xElements collection) {
			return collection | default(uint);
		}
		public static implicit operator long[](xElements collection) {
			return collection | default(long);
		}
		public static implicit operator ulong[](xElements collection) {
			return collection | default(ulong);
		}
		public static implicit operator float[](xElements collection) {
			return collection | default(float);
		}
		public static implicit operator double[](xElements collection) {
			return collection | default(double);
		}
		public static implicit operator decimal[](xElements collection) {
			return collection | default(decimal);
		}
		public static implicit operator string[](xElements collection) {
			return collection | default(string);
		}
		public static implicit operator Guid[](xElements collection) {
			return collection | Guid.NewGuid(); // default(Guid) is pretty useless
		}

		public static bool[] operator |(xElements collection, bool defaultValue) {
			return Convert(collection, bool.TryParse, defaultValue);
		}
		public static byte[] operator |(xElements collection, byte defaultValue) {
			return Convert(collection, byte.TryParse, defaultValue);
		}
		public static sbyte[] operator |(xElements collection, sbyte defaultValue) {
			return Convert(collection, sbyte.TryParse, defaultValue);
		}
		public static short[] operator |(xElements collection, short defaultValue) {
			return Convert(collection, short.TryParse, defaultValue);
		}
		public static ushort[] operator |(xElements collection, ushort defaultValue) {
			return Convert(collection, ushort.TryParse, defaultValue);
		}
		public static int[] operator |(xElements collection, int defaultValue) {
			return Convert(collection, int.TryParse, defaultValue);
		}
		public static uint[] operator |(xElements collection, uint defaultValue) {
			return Convert(collection, uint.TryParse, defaultValue);
		}
		public static long[] operator |(xElements collection, long defaultValue) {
			return Convert(collection, long.TryParse, defaultValue);
		}
		public static ulong[] operator |(xElements collection, ulong defaultValue) {
			return Convert(collection, ulong.TryParse, defaultValue);
		}
		public static float[] operator |(xElements collection, float defaultValue) {
			return Convert(collection, float.TryParse, defaultValue);
		}
		public static double[] operator |(xElements collection, double defaultValue) {
			return Convert(collection, double.TryParse, defaultValue);
		}
		public static decimal[] operator |(xElements collection, decimal defaultValue) {
			return Convert(collection, decimal.TryParse, defaultValue);
		}
		public static string[] operator |(xElements collection, string defaultValue) {
			if (collection == null) {
				return new string[0];
			}
			return collection.Where(e => e != null).Select(e => e.Value).ToArray();
		}
		public static Guid[] operator |(xElements collection, Guid defaultValue) {
			return Convert(collection, Guid.TryParse, defaultValue);
		}

		internal static T[] Convert<T>(xElements collection, TryParse<T> parser, T defaultValue) {
			if (collection == null) {
				return new T[0];
			}
			return collection.Select(e => xElement.Convert(e, parser, defaultValue)).ToArray();
		}

		public IEnumerator<xElement> GetEnumerator() {
			return mCollection.GetEnumerator();
		}
		IEnumerator IEnumerable.GetEnumerator() {
			return GetEnumerator();
		}
	}
}
