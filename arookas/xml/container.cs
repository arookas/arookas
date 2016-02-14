using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;

namespace arookas.Xml {
	public abstract class xContainer : xObject, IEnumerable<xElement> {
		XContainer mContainer;

		internal XContainer XContainer {
			get { return mContainer; }
		}

		internal xContainer(xDocument document, xElement parent, XContainer container)
			: base(document, parent) {
			aError.CheckNull(container, "container");
			mContainer = container;
		}

		public IEnumerator<xElement> GetEnumerator() {
			return this.Elements().GetEnumerator();
		}
		IEnumerator IEnumerable.GetEnumerator() {
			return GetEnumerator();
		}
	}
}
