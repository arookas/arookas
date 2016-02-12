﻿using System;

namespace arookas.IO.Binary {
	public static partial class aBinaryWriterUtil {
		public static void WritePadding(this aBinaryWriter writer, int multiple, byte padding) {
			aError.CheckNull(writer, "writer");
			var length = (int)(multiple - (writer.Position % multiple));
			if (length > 0) {
				var buffer = new byte[length];
				if (padding != 0) {
					for (var i = 0; i < length; ++i) {
						buffer[i] = padding;
					}
				}
				writer.Write8s(buffer);
			}
		}
		public static void WritePadding(this aBinaryWriter writer, int multiple, byte[] padding) {
			aError.CheckNull(writer, "writer");
			aError.CheckNull(padding, "padding");
			aError.Check<ArgumentException>(padding.Length > 0, "Array is empty.", "padding");
			var length = (int)(multiple - (writer.Position % multiple));
			var buffer = new byte[length];
			if (length > 0) {
				var p = 0;
				for (var i = 0; i < length; ++i) {
					buffer[i] = padding[p];
					if (++p >= padding.Length)
					{
						p = 0;
					}
				}
			}
			writer.Write8s(buffer);
		}
	}
}
