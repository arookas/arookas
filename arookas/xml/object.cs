using System;

namespace arookas.Xml {
	public abstract class xObject {
		xDocument mDocument;
		xElement mParent;

		internal xDocument _Document {
			get { return mDocument; }
		}
		internal xElement _Parent {
			get { return mParent; }
		}
		
		internal xObject(xDocument document, xElement parent) {
			mDocument = document;
			mParent = parent;
		}
	}
}
