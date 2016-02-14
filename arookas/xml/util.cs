using System;
using System.Collections.Generic;
using System.Linq;

namespace arookas.Xml {
	public static class xUtil {
		static readonly xElements sEmptyElements = new xElements(new xElement[0]);
		static readonly xAttributes sEmptyAttributes = new xAttributes(new xAttribute[0]);

		// elements
		public static xElement Element(this xContainer container, string name) {
			aError.CheckNull(name, "name");
			if (container == null) {
				return null;
			}
			var child = container.XContainer.Element(name);
			if (child == null) {
				return null;
			}
			return new xElement(container._Document, container as xElement, child);
		}
		public static xElements Elements(this xContainer container) {
			if (container == null) {
				return sEmptyElements;
			}
			var elements = container.XContainer.Elements();
			return new xElements(elements.Select(i => new xElement(container._Document, container as xElement, i)));
		}
		public static xElements Elements(this xContainer container, string name) {
			aError.CheckNull(name, "name");
			if (container == null) {
				return sEmptyElements;
			}
			var elements = container.XContainer.Elements(name);
			return new xElements(elements.Select(i => new xElement(container._Document, container as xElement, i)));
		}

		public static xElements Element(this IEnumerable<xElement> elements, string name) {
			aError.CheckNull(name, "name");
			if (elements == null) {
				return sEmptyElements;
			}
			// keep null entries out of the collection
			return new xElements(elements.Select(i => i.Element(name)).Where(i => i != null));
		}
		public static xElements Elements(this IEnumerable<xElement> elements) {
			if (elements == null) {
				return sEmptyElements;
			}
			var children = new List<xElement>(100);
			foreach (var i in elements) {
				children.AddRange(i.Elements());
			}
			return new xElements(children);
		}
		public static xElements Elements(this IEnumerable<xElement> elements, string name) {
			aError.CheckNull(name, "name");
			if (elements == null) {
				return sEmptyElements;
			}
			var children = new List<xElement>(100);
			foreach (var i in elements) {
				children.AddRange(i.Elements(name));
			}
			return new xElements(children);
		}

		// attributes
		public static xAttribute Attribute(this xElement element, string name) {
			if (element == null) {
				return null;
			}
			if (!element.HasAttributes) {
				return null;
			}
			var attribute = element.XElement.Attribute(name);
			if (attribute == null) {
				return null;
			}
			return new xAttribute(element._Document, element, attribute);
		}
		public static xAttributes Attributes(this xElement element)
		{
			if (element == null) {
				return sEmptyAttributes;
			}
			var attributes = element.XElement.Attributes();
			return new xAttributes(attributes.Select(i => new xAttribute(element._Document, element, i)));
		}
		public static xAttributes Attributes(this xElement element, string name) {
			aError.CheckNull(name, "name");
			if (element == null) {
				return sEmptyAttributes;
			}
			var attributes = element.XElement.Attributes(name);
			return new xAttributes(attributes.Select(i => new xAttribute(element._Document, element, i)));
		}

		public static xAttributes Attribute(this IEnumerable<xElement> elements, string name) {
			aError.CheckNull(name, "name");
			if (elements == null) {
				return sEmptyAttributes;
			}
			// keep null entries out of the collection
			return new xAttributes(elements.Select(i => i.Attribute(name)).Where(i => i != null));
		}
		public static xAttributes Attributes(this IEnumerable<xElement> elements) {
			if (elements == null) {
				return sEmptyAttributes;
			}
			var attributes = new List<xAttribute>(100);
			foreach (var i in elements) {
				attributes.AddRange(i.Attributes());
			}
			return new xAttributes(attributes);
		}
		public static xAttributes Attributes(this IEnumerable<xElement> elements, string name) {
			aError.CheckNull(name, "name");
			if (elements == null) {
				return sEmptyAttributes;
			}
			var attributes = new List<xAttribute>(100);
			foreach (var i in elements) {
				attributes.AddRange(i.Attributes(name));
			}
			return new xAttributes(attributes);
		}

		// parents
		public static xElement Parent(this xObject obj) {
			if (obj == null) {
				return null;
			}
			return obj._Parent;
		}
		public static xElements Parent(this IEnumerable<xObject> objs) {
			if (objs == null) {
				return sEmptyElements;
			}
			// keep null entries out of the collection
			return new xElements(objs.Select(i => i.Parent()).Where(i => i != null));
		}

		// names
		public static string Name(this xElement element) {
			if (element == null) {
				return null;
			}
			return element.Name;
		}
		public static IEnumerable<string> Name(this IEnumerable<xElement> elements) {
			if (elements == null) {
				return null;
			}
			return elements.Where(i => i != null).Select(i => i.Name);
		}

		public static string Name(this xAttribute attribute) {
			if (attribute == null) {
				return null;
			}
			return attribute.Name;
		}
		public static IEnumerable<string> Name(this IEnumerable<xAttribute> attributes) {
			if (attributes == null) {
				return null;
			}
			return attributes.Where(i => i != null).Select(i => i.Name);
		}

		// values
		public static string Value(this xElement element) {
			if (element == null) {
				return null;
			}
			return element.Value;
		}
		public static IEnumerable<string> Value(this IEnumerable<xElement> elements) {
			if (elements == null) {
				return null;
			}
			return elements.Where(i => i != null).Select(i => i.Value);
		}

		public static string Value(this xAttribute attribute) {
			if (attribute == null) {
				return null;
			}
			return attribute.Value;
		}
		public static IEnumerable<string> Value(this IEnumerable<xAttribute> attributes) {
			if (attributes == null) {
				return null;
			}
			return attributes.Where(i => i != null).Select(i => i.Value);
		}

		// explicit casts
		public static TEnum AsEnum<TEnum>(this xElement element)
			where TEnum : struct {
			return AsEnum(element, default(TEnum));
		}
		public static TEnum AsEnum<TEnum>(this xElement element, TEnum defaultValue)
			where TEnum : struct {
			TEnum value;
			if (element != null && Enum.TryParse(element.Value, true, out value)) {
				return value;
			}
			return defaultValue;
		}
		public static TEnum AsEnum<TEnum>(this xAttribute attribute)
			where TEnum : struct {
			return AsEnum(attribute, default(TEnum));
		}
		public static TEnum AsEnum<TEnum>(this xAttribute attribute, TEnum defaultValue)
			where TEnum : struct {
			TEnum value;
			if (attribute != null && Enum.TryParse(attribute.Value, true, out value)) {
				return value;
			}
			return defaultValue;
		}

		public static TEnum[] AsEnum<TEnum>(this IEnumerable<xElement> elements)
			where TEnum : struct {
			return AsEnum(elements, default(TEnum));
		}
		public static TEnum[] AsEnum<TEnum>(this IEnumerable<xElement> elements, TEnum defaultValue)
			where TEnum : struct {
				if (elements == null) {
				return new TEnum[0];
			}
			return elements.Select(e => e.AsEnum(defaultValue)).ToArray();
		}
		public static TEnum[] AsEnum<TEnum>(this IEnumerable<xAttribute> attributes)
			where TEnum : struct { return AsEnum(attributes, default(TEnum));
		}
		public static TEnum[] AsEnum<TEnum>(this IEnumerable<xAttribute> attributes, TEnum defaultValue)
			where TEnum : struct {
			if (attributes == null) {
				return new TEnum[0];
			}
			return attributes.Select(a => a.AsEnum(defaultValue)).ToArray();
		}
	}
}
