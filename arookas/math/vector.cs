using System;
using System.Runtime.InteropServices;
using SystemMath = System.Math;

namespace arookas.Math {
	[StructLayout(LayoutKind.Sequential)]
	public struct aVec3 {
		public float x, y, z;

		public float Magnitude {
			get { return (float)SystemMath.Sqrt(MagnitudeSquared); }
		}
		public float MagnitudeSquared {
			get { return (x * x + y * y + z * z); }
		}

		public aVec3 Normalized {
			get {
				if (x == 0.0f && y == 0.0f && z == 0.0f) {
					return Zero;
				}
				return this / Magnitude;
			}
		}

		public static aVec3 Zero {
			get { return new aVec3(); }
		}
		public static aVec3 UnitX {
			get { return new aVec3(1.0f, 0.0f, 0.0f); }
		}
		public static aVec3 UnitY {
			get { return new aVec3(0.0f, 1.0f, 0.0f); }
		}
		public static aVec3 UnitZ {
			get { return new aVec3(0.0f, 0.0f, 1.0f); }
		}
		public static aVec3 NegateX {
			get { return new aVec3(-1.0f, 1.0f, 1.0f); }
		}
		public static aVec3 NegateY {
			get { return new aVec3(1.0f, -1.0f, 1.0f); }
		}
		public static aVec3 NegateZ {
			get { return new aVec3(1.0f, -1.0f, 1.0f); }
		}
		public static aVec3 One {
			get { return new aVec3(1.0f); }
		}

		public aVec3(float scalar) {
			x = scalar;
			y = scalar;
			z = scalar;
		}
		public aVec3(float x, float y, float z) {
			this.x = x;
			this.y = y;
			this.z = z;
		}

		public void InvertHandedness() {
			z = -z;
		}

		public override bool Equals(object obj) {
			if (obj is aVec3) {
				return ((aVec3)obj == this);
			}
			return false;
		}
		public override int GetHashCode() {
			return (x.GetHashCode() ^ y.GetHashCode() ^ z.GetHashCode());
		}
		public override string ToString() {
			return String.Format("({0}, {1}, {2})", x, y, z);
		}

		public static aVec3 CrossProduct(aVec3 first, aVec3 second) {
			return new aVec3(
				first.y * second.z - first.z * second.y,
				first.z * second.x - first.x * second.z,
				first.x * second.y - first.y * second.x
			);
		}
		public static float Distance(aVec3 from, aVec3 to) {
			return (from - to).Magnitude;
		}
		public static float DotProduct(aVec3 first, aVec3 second) {
			return (first.x * second.x + first.y * second.y + first.z * second.z);
		}

		public static aVec3 Lerp(aVec3 start, aVec3 end, float percentage) {
			return (start * (1.0f - percentage)) + (end * percentage);
		}

		public static aVec3 operator +(aVec3 a, aVec3 b) {
			return new aVec3(a.x + b.x, a.y + b.y, a.z + b.z);
		}
		public static aVec3 operator -(aVec3 vec) {
			return new aVec3(-vec.x, -vec.y, -vec.z);
		}
		public static aVec3 operator -(aVec3 a, aVec3 b) {
			return new aVec3(a.x - b.x, a.y - b.y, a.z - b.z);
		}
		public static aVec3 operator *(aVec3 a, aVec3 b) {
			return new aVec3(a.x * b.x, a.y * b.y, a.z * b.z);
		}
		public static aVec3 operator *(aVec3 a, float b) {
			return new aVec3(a.x * b, a.y * b, a.z * b);
		}
		public static aVec3 operator *(float a, aVec3 b) {
			return new aVec3(b.x * a, b.y * a, b.z * a);
		}
		public static aVec3 operator /(aVec3 a, aVec3 b) {
			return new aVec3(a.x / b.x, a.y / b.y, a.z / b.z);
		}
		public static aVec3 operator /(aVec3 a, float b) {
			return new aVec3(a.x / b, a.y / b, a.z / b);
		}

		public static bool operator ==(aVec3 a, aVec3 b) {
			return (
				a.x == b.x &&
				a.y == b.y &&
				a.z == b.z
			);
		}
		public static bool operator !=(aVec3 a, aVec3 b) {
			return !(a == b);
		}
	}
}
