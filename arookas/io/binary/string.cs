using System;
using System.Text;

namespace arookas.IO.Binary {
	public abstract class aBinaryString {
		public abstract string Read(aBinaryReader reader);
		public abstract void Write(aBinaryWriter writer, string value);
	}

	public class aZSTR : aBinaryString {
		public aZSTR() { }

		public override string Read(aBinaryReader reader) {
			var sb = new StringBuilder(256);
			char c;
			while ((c = reader.ReadChar()) != '\x00') {
				sb.Append(c);
			}
			return sb.ToString();
		}
		public override void Write(aBinaryWriter writer, string value) {
			writer.WriteString(value);
			writer.WriteChar('\0');
		}
	}

	public class aBSTR : aBinaryString {
		public aBSTR() { }

		public override string Read(aBinaryReader reader) {
			var length = reader.Read8();
			var value = reader.ReadString(length);
			return value;
		}
		public override void Write(aBinaryWriter writer, string value) {
			aError.Check<ArgumentException>(value.Length <= Byte.MaxValue, "String value is too long for a BSTR.", "value");
			writer.Write8((byte)value.Length);
			writer.WriteString(value);
		}
	}

	public class aWSTR : aBinaryString {
		public aWSTR() { }

		public override string Read(aBinaryReader reader) {
			var length = reader.Read16();
			var value = reader.ReadString(length);
			return value;
		}
		public override void Write(aBinaryWriter writer, string value) {
			aError.Check<ArgumentException>(value.Length <= UInt16.MaxValue, "String value is too long for a WSTR.", "value");
			writer.Write16((ushort)value.Length);
			writer.WriteString(value);
		}
	}

	public class aBZSTR : aBSTR {
		public aBZSTR() { }

		public override string Read(aBinaryReader reader) {
			var value = base.Read(reader);
#if DEBUG
			aError.Check<Exception>(reader.ReadChar() == '\0', "BZSTR is not null-terminated.");
#else
			reader.Step(1);
#endif
			return value;
		}
		public override void Write(aBinaryWriter writer, string value) {
			base.Write(writer, value);
			writer.WriteChar('\0');
		}
	}

	public class aWZSTR : aWSTR {
		public aWZSTR() { }

		public override string Read(aBinaryReader reader) {
			var value = base.Read(reader);
#if DEBUG
			aError.Check<Exception>(reader.ReadChar() == '\0', "WZSTR is not null-terminated.");
#else
			reader.Step(1);
#endif
			return value;
		}
		public override void Write(aBinaryWriter writer, string value) {
			base.Write(writer, value);
			writer.WriteChar('\0');
		}
	}

	public class aCSTR : aBinaryString {
		int mMultiple;

		public aCSTR(int multiple) {
			aError.CheckMin(multiple, "multiple", 0);
			mMultiple = multiple;
		}

		public override string Read(aBinaryReader reader) {
			var value = reader.ReadString(mMultiple);
			var length = value.IndexOf('\x00');
			if (length > 0) {
				return value.Substring(0, length);
			}
			return value;
		}
		public override void Write(aBinaryWriter writer, string value) {
			if (value.Length > mMultiple) {
				value = value.Substring(0, mMultiple);
			}
			writer.WriteString(value);
			var count = mMultiple - value.Length;
			while (count-- > 0) {
				writer.WriteChar('\0');
			}
		}
	}
}
