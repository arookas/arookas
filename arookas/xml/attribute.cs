using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace arookas.Xml {
	public sealed class xAttribute : xObject {
		XAttribute mAttribute;

		internal XAttribute Attribute {
			get { return mAttribute; }
		}

		public string Name {
			get { return Attribute.Name.LocalName; }
		}
		public string Value {
			get { return Attribute.Value; }
		}

		public xAttribute Next {
			get { return new xAttribute(_Document, _Parent, Attribute.NextAttribute); }
		}
		public xAttribute Previous {
			get { return new xAttribute(_Document, _Parent, Attribute.PreviousAttribute); }
		}

		internal xAttribute(xDocument document, xElement parent, XAttribute attribute)
			: base(document, parent) {
			aError.CheckNull(attribute, "attribute");
			mAttribute = attribute;
		}

		public static implicit operator bool(xAttribute attribute) {
			return attribute | default(bool);
		}
		public static implicit operator byte(xAttribute attribute) {
			return attribute | default(byte);
		}
		public static implicit operator sbyte(xAttribute attribute) {
			return attribute | default(sbyte);
		}
		public static implicit operator short(xAttribute attribute) {
			return attribute | default(short);
		}
		public static implicit operator ushort(xAttribute attribute) {
			return attribute | default(ushort);
		}
		public static implicit operator int(xAttribute attribute) {
			return attribute | default(int);
		}
		public static implicit operator uint(xAttribute attribute) {
			return attribute | default(uint);
		}
		public static implicit operator long(xAttribute attribute) {
			return attribute | default(long);
		}
		public static implicit operator ulong(xAttribute attribute) {
			return attribute | default(ulong);
		}
		public static implicit operator float(xAttribute attribute) {
			return attribute | default(float);
		}
		public static implicit operator double(xAttribute attribute) {
			return attribute | default(double);
		}
		public static implicit operator decimal(xAttribute attribute) {
			return attribute | default(decimal);
		}
		public static implicit operator string(xAttribute attribute) {
			return attribute | default(string);
		}
		public static implicit operator Guid(xAttribute attribute) {
			return attribute | Guid.NewGuid();
		}

		public static bool operator |(xAttribute attribute, bool defaultValue) {
			return Convert(attribute, bool.TryParse, defaultValue); }
		public static byte operator |(xAttribute attribute, byte defaultValue) {
			return Convert(attribute, byte.TryParse, defaultValue); }
		public static sbyte operator |(xAttribute attribute, sbyte defaultValue) {
			return Convert(attribute, sbyte.TryParse, defaultValue); }
		public static short operator |(xAttribute attribute, short defaultValue) {
			return Convert(attribute, short.TryParse, defaultValue); }
		public static ushort operator |(xAttribute attribute, ushort defaultValue) {
			return Convert(attribute, ushort.TryParse, defaultValue);
		}
		public static int operator |(xAttribute attribute, int defaultValue) {
			return Convert(attribute, int.TryParse, defaultValue);
		}
		public static uint operator |(xAttribute attribute, uint defaultValue) {
			return Convert(attribute, uint.TryParse, defaultValue);
		}
		public static long operator |(xAttribute attribute, long defaultValue) {
			return Convert(attribute, long.TryParse, defaultValue);
		}
		public static ulong operator |(xAttribute attribute, ulong defaultValue) {
			return Convert(attribute, ulong.TryParse, defaultValue);
		}
		public static float operator |(xAttribute attribute, float defaultValue) {
			return Convert(attribute, float.TryParse, defaultValue);
		}
		public static double operator |(xAttribute attribute, double defaultValue) {
			return Convert(attribute, double.TryParse, defaultValue);
		}
		public static decimal operator |(xAttribute attribute, decimal defaultValue) {
			return Convert(attribute, decimal.TryParse, defaultValue);
		}
		public static string operator |(xAttribute attribute, string defaultValue) {
			return attribute != null ? attribute.Value : defaultValue;
		}
		public static Guid operator |(xAttribute attribute, Guid defaultValue) {
			return Convert(attribute, Guid.TryParse, defaultValue);
		}

		internal static T Convert<T>(xAttribute attribute, TryParse<T> parser, T defaultValue) {
			T value;
			if (attribute != null && parser(attribute.Value, out value)) {
				return value;
			}
			return defaultValue;
		}

		public override bool Equals(object obj) {
			var attribute = (obj as xAttribute);
			return (attribute != null) && (attribute == this);
		}
		public override int GetHashCode() {
			return Attribute.GetHashCode();
		}
		public override string ToString() {
			return Attribute.ToString();
		}

		public static bool operator ==(xAttribute a, xAttribute b) {
			// can't use == null because recursion
			var aa = Object.ReferenceEquals(a, null) ? null : a.mAttribute;
			var bb = Object.ReferenceEquals(b, null) ? null : b.mAttribute;
			return aa == bb;
		}
		public static bool operator !=(xAttribute a, xAttribute b) {
			return !(a == b);
		}
	}

	public sealed class xAttributes : IEnumerable<xAttribute> {
		IEnumerable<xAttribute> mCollection;

		internal xAttributes(IEnumerable<xAttribute> collection) {
			mCollection = collection;
		}

		public static implicit operator bool[](xAttributes collection) {
			return collection | default(bool);
		}
		public static implicit operator byte[](xAttributes collection) {
			return collection | default(byte);
		}
		public static implicit operator sbyte[](xAttributes collection) {
			return collection | default(sbyte);
		}
		public static implicit operator short[](xAttributes collection) {
			return collection | default(short);
		}
		public static implicit operator ushort[](xAttributes collection) {
			return collection | default(ushort);
		}
		public static implicit operator int[](xAttributes collection) {
			return collection | default(int);
		}
		public static implicit operator uint[](xAttributes collection) {
			return collection | default(uint);
		}
		public static implicit operator long[](xAttributes collection) {
			return collection | default(long);
		}
		public static implicit operator ulong[](xAttributes collection) {
			return collection | default(ulong);
		}
		public static implicit operator float[](xAttributes collection) {
			return collection | default(float);
		}
		public static implicit operator double[](xAttributes collection) {
			return collection | default(double);
		}
		public static implicit operator decimal[](xAttributes collection) {
			return collection | default(decimal);
		}
		public static implicit operator string[](xAttributes collection) {
			return collection | default(string);
		}
		public static implicit operator Guid[](xAttributes collection) {
			return collection | Guid.NewGuid(); // default(Guid) is pretty useless
		}

		public static bool[] operator |(xAttributes collection, bool defaultValue) {
			return Convert(collection, bool.TryParse, defaultValue);
		}
		public static byte[] operator |(xAttributes collection, byte defaultValue) {
			return Convert(collection, byte.TryParse, defaultValue);
		}
		public static sbyte[] operator |(xAttributes collection, sbyte defaultValue) {
			return Convert(collection, sbyte.TryParse, defaultValue);
		}
		public static short[] operator |(xAttributes collection, short defaultValue) {
			return Convert(collection, short.TryParse, defaultValue);
		}
		public static ushort[] operator |(xAttributes collection, ushort defaultValue) {
			return Convert(collection, ushort.TryParse, defaultValue);
		}
		public static int[] operator |(xAttributes collection, int defaultValue) {
			return Convert(collection, int.TryParse, defaultValue);
		}
		public static uint[] operator |(xAttributes collection, uint defaultValue) {
			return Convert(collection, uint.TryParse, defaultValue);
		}
		public static long[] operator |(xAttributes collection, long defaultValue) {
			return Convert(collection, long.TryParse, defaultValue);
		}
		public static ulong[] operator |(xAttributes collection, ulong defaultValue) {
			return Convert(collection, ulong.TryParse, defaultValue);
		}
		public static float[] operator |(xAttributes collection, float defaultValue) {
			return Convert(collection, float.TryParse, defaultValue);
		}
		public static double[] operator |(xAttributes collection, double defaultValue) {
			return Convert(collection, double.TryParse, defaultValue);
		}
		public static decimal[] operator |(xAttributes collection, decimal defaultValue) {
			return Convert(collection, decimal.TryParse, defaultValue);
		}
		public static string[] operator |(xAttributes collection, string defaultValue) {
			return collection != null ? collection.Where(e => e != null).Select(e => e.Value).ToArray() : new string[0];
		}
		public static Guid[] operator |(xAttributes collection, Guid defaultValue) {
			return Convert(collection, Guid.TryParse, defaultValue);
		}

		internal static T[] Convert<T>(xAttributes collection, TryParse<T> parser, T defaultValue) {
			return collection != null ? collection.Select(e => xAttribute.Convert(e, parser, defaultValue)).ToArray() : new T[0];
		}

		public IEnumerator<xAttribute> GetEnumerator() {
			return mCollection.GetEnumerator();
		}
		IEnumerator IEnumerable.GetEnumerator() {
			return GetEnumerator();
		}
	}
}
