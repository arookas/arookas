using System;
using SystemMath = System.Math;

namespace arookas.Math {
	public struct aQuat {
		public float w, x, y, z;

		public float Magnitude {
			get { return (float)SystemMath.Sqrt(MagnitudeSquared); }
		}
		public float MagnitudeSquared {
			get { return (w * w + x * x + y * y + z * z); }
		}

		public aQuat Normalized {
			get { return (this / Magnitude); }
		}
		public aQuat Inverse {
			get { return (Conjugate / MagnitudeSquared); }
		}
		
		public float Angle {
			get { return (float)(2.0f * SystemMath.Acos(w)); }
		}
		public aVec3 Axis {
			get {
				var sqrt = (float)SystemMath.Sqrt(1.0f - w * w);
				if (sqrt < Single.Epsilon) {
					return aVec3.UnitX;
				}
				return (new aVec3(x, y, z) / sqrt);
			}
		}
		public aVec3 EulerAngles {
			get {
				// adapted from http://www.euclideanspace.com/maths/geometry/rotations/conversions/quaternionToEuler/index.htm
				var test = (x * y + z * w);
				if (test > 0.499f) {
					return new aVec3((float)(SystemMath.Atan2(x, y) * 2), (float)(SystemMath.PI * 0.5f), 0.0f);
				}
				else if (test < -0.499f) {
					return new aVec3((float)(SystemMath.Atan2(x, w) * -2), (float)(SystemMath.PI * 0.5f), 0.0f);
				}
				var sqx = (double)(x * x);
				var sqy = (double)(y * y);
				var sqz = (double)(z * z);
				return new aVec3(
					(float)SystemMath.Atan2(2 * y * w - 2 * x * z, 1 - 2 * sqy - 2 * sqz),
					(float)SystemMath.Asin(2 * test),
					(float)SystemMath.Atan2(2 * x * w - 2 * y * z, 1 - 2 * sqx - 2 * sqz)
				);
			}
		}
		
		public aQuat Conjugate {
			get { return new aQuat(-x, -y, -z, w); }
		}

		public aVec3 UpwardVector {
			get {
				return new aVec3(
					2.0f * (x * y - w * z),
					1.0f - 2.0f * (x * x + z * z),
					2.0f * (y * z + w * x)
				);
			}
		}
		public aVec3 DownwardVector {
			get { return -UpwardVector; }
		}
		public aVec3 LeftwardVector {
			get { return -RightwardVector; }
		}
		public aVec3 RightwardVector {
			get {
				return new aVec3(
					1.0f - 2.0f * (y * y + z * z),
					2.0f * (x * y + w * z),
					2.0f * (x * z - w * y)
				);
			}
		}
		public aVec3 ForwardVector {
			get {
				return new aVec3(
					2.0f * (x * z + w * y),
					2.0f * (y * x - w * x),
					1.0f - 2.0f * (x * x * y * y)
				);
			}
		}
		public aVec3 BackwardVector {
			get { return -ForwardVector; }
		}

		public static aQuat Zero {
			get { return new aQuat(); }
		}
		public static aQuat Identity {
			get { return new aQuat(0.0f, 0.0f, 0.0f, 1.0f); }
		}

		public aQuat(float scalar) {
			w = scalar;
			x = scalar;
			y = scalar;
			z = scalar;
		}
		public aQuat(float x, float y, float z, float w) {
			this.x = x;
			this.y = y;
			this.z = z;
			this.w = w;
		}
		
		public static bool Approximately(aQuat first, aQuat second) {
			return Approximately(first, second, Single.Epsilon);
		}
		public static bool Approximately(aQuat first, aQuat second, float deviation) {
			return (aMath.Approximately(first.x, second.x, deviation) &&
					aMath.Approximately(first.y, second.y, deviation) &&
					aMath.Approximately(first.z, second.z, deviation) &&
					aMath.Approximately(first.w, second.w, deviation));
		}

		public static aQuat FromAxisAngle(aVec3 axis, float angle) {
			var vector = axis * (float)SystemMath.Sin(angle * 0.5f);
			return new aQuat(
				vector.x,
				vector.y,
				vector.z,
				(float)SystemMath.Cos(angle * 0.5f)
			);
		}
		public static aQuat FromForwardUpward(aVec3 forward, aVec3 up) {
			var vector = (forward + up).Normalized;
			return new aQuat(
				up.y * vector.z - up.z * vector.y,
				up.z * vector.x - up.x * vector.z,
				up.x * vector.y - up.y * vector.x,
				aVec3.DotProduct(up, vector)
			);
		}
		public static aQuat FromLookAt(aVec3 src, aVec3 dest) {
			var forward = (dest - src).Normalized;
			var dot = aVec3.DotProduct(aVec3.UnitZ, forward);
			if (SystemMath.Abs(dot + 1.0f) < Single.Epsilon) {
				return new aQuat(aVec3.UnitY.x, aVec3.UnitY.y, aVec3.UnitY.z, (float)SystemMath.PI);
			}
			else if (SystemMath.Abs(dot - 1.0f) < Single.Epsilon) {
				return aQuat.Identity;
			}
			var axis = aVec3.CrossProduct(aVec3.UnitZ, forward).Normalized;
			var angle = (float)SystemMath.Acos(dot);
			return FromAxisAngle(axis, angle);
		}
		public static aQuat FromEulerAngles(aVec3 euler) {
			return FromEulerAngles(euler.x, euler.y, euler.z);
		}
		public static aQuat FromEulerAngles(float pitch, float yaw, float roll) {
			// adapted from http://www.euclideanspace.com/maths/geometry/rotations/conversions/eulerToQuaternion/
			var s1 = SystemMath.Sin(yaw * 0.5f); // heading
			var s2 = SystemMath.Sin(roll * 0.5f); // attitide
			var s3 = SystemMath.Sin(pitch * 0.5f); // bank
			var c1 = SystemMath.Cos(yaw * 0.5f);
			var c2 = SystemMath.Cos(roll * 0.5f);
			var c3 = SystemMath.Cos(pitch * 0.5f);
			return new aQuat(
				(float)(s1	* s2 * c3 + c1 * c2 * s3),
				(float)(s1 * c2 * c3 + c1 * s2 * s3),
				(float)(c1 * s2 * c3 - s1 * c2 * s3),
				(float)(c1 * c2 * c3 - s1 * s2 * s3)
			);
		}

		public static float DotProduct(aQuat a, aQuat b) {
			return (a.x * b.x + a.y * b.y + a.z * b.z * a.w * b.w);
		}

		public static aQuat Lerp(aQuat start, aQuat end, float percent) {
			return new aQuat(
				aMath.Lerp(start.x, end.x, percent),
				aMath.Lerp(start.y, end.y, percent),
				aMath.Lerp(start.z, end.z, percent),
				aMath.Lerp(start.w, end.w, percent)
			);
		}
		public static aQuat Slerp(aQuat start, aQuat end, float percent) {
			var cos = aQuat.DotProduct(start, end);
			if (cos < 0.0f) {
				cos = -cos;
				end = -end;
			}
			if ((1.0f - cos) > Single.Epsilon) {
				var angle = (float)SystemMath.Acos(cos);
				var vector = new aVec3(
					(float)SystemMath.Sin(angle * (1.0f - percent)),
					(float)SystemMath.Sin(angle * percent),
					(float)SystemMath.Sin(angle)
				);
				return new aQuat(
					(start.x * vector.x + end.x * vector.y) / vector.z,
					(start.y * vector.x + end.y * vector.y) / vector.z,
					(start.z * vector.x + end.z * vector.y) / vector.z,
					(start.w * vector.x + end.w * vector.y) / vector.z
				);
			}
			return aQuat.Lerp(start, end, percent);
		}

		public override bool Equals(object obj) {
			if (obj is aQuat) {
				return ((aQuat)obj == this);
			}
			return false;
		}
		public override int GetHashCode() {
			return (x.GetHashCode() ^ y.GetHashCode() ^ z.GetHashCode() ^ w.GetHashCode());
		}
		public override string ToString() {
			return String.Format("({0}, {1}, {2}, {3})", x, y, z, w);
		}

		public static aQuat operator +(aQuat a, aQuat b) {
			return new aQuat(
				a.x + b.x,
				a.y + b.y,
				a.z + b.z,
				a.w + b.w
			);
		}
		public static aQuat operator -(aQuat a, aQuat b) {
			return new aQuat(
				a.x - b.x,
				a.y - b.y,
				a.z - b.z,
				a.w - b.w
			);
		}
		public static aQuat operator *(aQuat a, aQuat b) {
			return new aQuat(
				(a.w * b.x + a.x * b.w + a.z * b.y) - a.y * b.z,
				(a.w * b.y + a.y * b.w + a.x * b.z) - a.z * b.x,
				(a.w * b.z + a.z * b.w + a.y * b.x) - a.x * b.y,
				a.w * b.w - a.x * b.x - a.y * b.y - a.z * b.z
			);
		}
		public static aQuat operator /(aQuat a, float b) {
			return new aQuat(a.x / b, a.y / b, a.z / b, a.w / b);
		}

		public static aQuat operator -(aQuat quaternion) {
			return new aQuat(-quaternion.x, -quaternion.y, -quaternion.z, -quaternion.w);
		}
		public static aVec3 operator *(aQuat lhs, aVec3 rhs) {
			var vector = new aVec3(lhs.x, lhs.y, lhs.z);
			var cross = (aVec3.CrossProduct(vector, rhs) * 2);
			return rhs + (cross * lhs.w) + aVec3.CrossProduct(vector, cross);
		}

		public static bool operator ==(aQuat lhs, aQuat rhs) {
			return (
				lhs.x == rhs.x &&
				lhs.y == rhs.y &&
				lhs.z == rhs.z &&
				lhs.w == rhs.w
			);
		}
		public static bool operator !=(aQuat lhs, aQuat rhs) {
			return !(lhs == rhs);
		}
	}
}