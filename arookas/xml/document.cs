using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;

namespace arookas.Xml {
	public sealed class xDocument : xContainer {
		XDocument mDocument;

		internal XDocument XDocument {
			get { return mDocument; }
		}

		public string Encoding {
			get { return XDocument.Declaration.Encoding; }
		}
		public string Version {
			get { return XDocument.Declaration.Version; }
		}
		public string Standalone {
			get { return XDocument.Declaration.Standalone; }
		}
		public xElement Root {
			get { return new xElement(this, null, XDocument.Root); }
		}

		internal xDocument(XDocument document)
			: base(null, null, document) {
			mDocument = document;
		}

		public xDocument(Stream stream)
			: this(XDocument.Load(stream, LoadOptions.SetLineInfo)) { }
		public xDocument(string uri)
			: this(XDocument.Load(uri, LoadOptions.SetLineInfo)) { }
		public xDocument(XmlReader reader)
			: this(XDocument.Load(reader, LoadOptions.SetLineInfo)) { }
		public xDocument(TextReader textReader)
			: this(XDocument.Load(textReader, LoadOptions.SetLineInfo)) { }

		public override bool Equals(object obj) {
			var document = (obj as xDocument);
			return (document != null) && (document == this);
		}
		public override int GetHashCode() {
			return XDocument.GetHashCode();
		}
		public override string ToString() {
			return XDocument.ToString();
		}

		public static bool operator ==(xDocument a, xDocument b) {
			// can't use == null because recursion
			var aa = Object.ReferenceEquals(a, null) ? null : a.mDocument;
			var bb = Object.ReferenceEquals(b, null) ? null : b.mDocument;
			return aa == bb;
		}
		public static bool operator !=(xDocument a, xDocument b) {
			return !(a == b);
		}
	}
}
