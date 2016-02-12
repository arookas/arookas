using System;
using System.Runtime.InteropServices;

namespace arookas.Reflection {
	public static class aMarshal {
		public static IntPtr Allocate<T>() {
			return Marshal.AllocHGlobal(Marshal.SizeOf(typeof(T)));
		}
		public static int SizeOf<T>() {
			return Marshal.SizeOf(typeof(T));
		}
		public static T ToStructure<T>(this IntPtr pointer) {
			return (T)Marshal.PtrToStructure(pointer, typeof(T));
		}
	}
}
