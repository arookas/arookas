using System;

namespace arookas {
	public static class aEnum {
		public static bool IsDefined<T>(this T enumValue)
			where T : struct, IComparable, IFormattable, IConvertible {
			return Enum.IsDefined(typeof(T), enumValue);
		}
	}
}
