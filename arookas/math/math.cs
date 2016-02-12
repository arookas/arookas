using System;
using SystemMath = System.Math;

namespace arookas.Math {
	public static class aMath {
		public const double cDegToRad = 0.01745329251994329576923690768489d;
		public const double cRadToDeg = 57.295779513082320876798154814092d;

		public static T Clamp<T>(this T x, T min, T max)
			where T : IComparable<T> {
			if (x.CompareTo(min) < 0) {
				return min;
			}
			else if (x.CompareTo(max) > 0) {
				return max;
			}
			return x;
		}

		// single
		public static bool Approximately(this float first, float second) {
			return (SystemMath.Abs(first - second) <= Single.Epsilon);
		}
		public static bool Approximately(this float first, float second, float maximumDeviation) {
			return (SystemMath.Abs(first - second) <= maximumDeviation);
		}

		public static float GetFractional(this float value) {
			return (value - (float)SystemMath.Floor(value));
		}
		public static int GetIntegral(this float value) {
			return (int)SystemMath.Floor(value);
		}

		public static float Lerp(float start, float end, float percent) {
			return (start * (1.0f - percent)) + (end * percent);
		}

		public static float ToDegrees(this float radians) {
			return (radians * (float)cRadToDeg);
		}
		public static float ToRadians(this float degrees) {
			return (degrees * (float)cDegToRad);
		}

		// double
		public static bool Approximately(this double first, double second) {
			return (SystemMath.Abs(first - second) <= Double.Epsilon);
		}
		public static bool Approximately(this double first, double second, double maximumDeviation) {
			return (SystemMath.Abs(first - second) <= maximumDeviation);
		}

		public static double GetFractional(this double value) {
			return (value - SystemMath.Floor(value));
		}
		public static long GetIntegral(this double value) {
			return (long)SystemMath.Floor(value);
		}

		public static double Lerp(double start, double end, float percent)
		{
			return (start * (1.0f - percent)) + (end * percent);
		}

		public static double ToDegrees(this double radians) {
			return (radians * cRadToDeg);
		}
		public static double ToRadians(this double degrees) {
			return (degrees * cDegToRad);
		}
	}
}